using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Extensions;
using FinancialPortal.Web.Services;
using FinancialPortal.Web.Settings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FinancialPortal.Web.Security
{
    [ExcludeFromCodeCoverage]
    public static class OpenIdAuthMiddleware
    {
        public static void AddOpenIdConnectAuth(
            this IServiceCollection services, OpenIdAuthOptions authOptions)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    // Surprisingly there are no DI for this!
                    options.SessionStore = new RedisCacheTicketStore(new RedisCache(new RedisCacheOptions
                    {
                        Configuration = authOptions.RedisConfiguration
                    }));

                    options.Cookie.SameSite = SameSiteMode.None;

                    // This is so we can manually check if the Token is expired so we can re-new it or logout the user.
                    options.Events.OnValidatePrincipal = context => OnValidatePrincipal(context, authOptions);
                })
                .AddOpenIdConnect(options =>
                {
                    options.Authority = authOptions.AuthorityEndpoint;

                    options.ClientId = authOptions.ClientId;
                    options.ClientSecret = authOptions.ClientSecret;

                    options.ResponseType = "code id_token";
                    options.RequireHttpsMetadata = authOptions.RequireHttpsMetadata;

                    // This sets the cookie expiry to match the ID Token if true. This is of no use so setting it to false.
                    options.UseTokenLifetime = false;

                    options.SaveTokens = true;

                    foreach (var scope in authOptions.Scopes)
                    {
                        options.Scope.Add(scope);
                    }

                    // Form post is more secure.
                    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.FormPost;
                    options.RequireHttpsMetadata = options.RequireHttpsMetadata;

                    if (options.Events == null)
                    {
                        options.Events = new OpenIdConnectEvents();
                    }

                    options.Events.OnRemoteFailure = HandleRemoteFailure;
                    options.Events.OnAuthenticationFailed = HandleAuthenticationFailure;
                    options.Events.OnTicketReceived = context => SetupIdentityAndClaimsAndGa(context, authOptions);
                    options.Events.OnRedirectToIdentityProvider = SetupAdditionalProperties;

                    options.SecurityTokenValidator = new JwtSecurityTokenHandler
                    {
                        InboundClaimTypeMap = new Dictionary<string, string>()
                    };

                    options.TokenValidationParameters.NameClaimType = ClaimTypes.Name;
                });

            services.AddSingleton<IDiscoveryCache>(r =>
            {
                var factory = r.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(authOptions.AuthorityEndpoint, () => factory.CreateClient(),
                    new DiscoveryPolicy
                    {
                        RequireHttps = authOptions.RequireHttpsMetadata,
                        AuthorityNameComparison = StringComparison.CurrentCultureIgnoreCase
                    });
            });
        }

        private static Task OnValidatePrincipal(
            CookieValidatePrincipalContext context, OpenIdAuthOptions authOptions)
        {
            // Don't need to worry about about unauth request.
            if (context.Principal?.Identity == null || !context.Principal.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            var absoluteExpiry = DateTime.Parse(context.Properties.Items[".Token.expires_at"]).ToUniversalTime();

            // If the token is still not expired then don't need to reject the principal.
            if (absoluteExpiry > DateTime.UtcNow)
            {
                return Task.CompletedTask;
            }

            // Token has expired, time to reject the Principal which will kick off the auth process again.
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
            logger.LogInformation(
                $"Token has expired @ {absoluteExpiry:O} compared to current date time of {DateTime.UtcNow:O}");

            context.RejectPrincipal();

            return Task.CompletedTask;
        }

        private static async Task<bool> RefreshAccessToken(
            CookieValidatePrincipalContext context, OpenIdAuthOptions authOptions)
        {
            // True = token refreshed. False = Token not refreshed and Principal will have to be rejected so the pipeline will kick off the login process.
            // Note that this function is not working as expected due to an issue with the context properties not updating correctly when setting the new 
            // Expiry and Refresh token.

            if (context.Properties.Items.ContainsKey(".Token.refresh_token"))
            {
                var refreshToken = context.Properties.Items[".Token.refresh_token"];

                // Reference token.
                var discoveryCache = context.HttpContext.RequestServices.GetRequiredService<IDiscoveryCache>();

                var disco = await discoveryCache.GetAsync();

                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }

                var client = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>()
                    .CreateClient();

                var refreshTokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = authOptions.ClientId,
                    ClientSecret = authOptions.ClientSecret,
                    RefreshToken = refreshToken
                });

                if (!refreshTokenResponse.IsError)
                {
                    var id =
                        await GetClaimsIdentity(
                            context.Scheme.Name, refreshTokenResponse.AccessToken, context.HttpContext, authOptions);

                    context.ReplacePrincipal(new ClaimsPrincipal(id));
                    context.Properties.Items[".Token.refresh_token"] = refreshTokenResponse.RefreshToken;
                    context.Properties.Items[".Token.expires_at"] =
                        DateTime.UtcNow.AddSeconds(refreshTokenResponse.ExpiresIn).ToString("O");

                    return true;
                }
            }

            return false;
        }

        private static Task HandleAuthenticationFailure(AuthenticationFailedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
            var contextAsJson = string.Empty;
            try
            {
                contextAsJson = JsonConvert.SerializeObject(context);
            }
            catch (Exception)
            {
            }

            logger.LogError(context.Exception,
                $"An error has occurred while authenticating user against identity server. JSON: {contextAsJson}.");

            context.HandleResponse();
            context.Response.Redirect("/error/authfailure");

            return Task.CompletedTask;
        }

        private static Task HandleRemoteFailure(RemoteFailureContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
            var contextAsJson = string.Empty;
            try
            {
                contextAsJson = JsonConvert.SerializeObject(context);
            }
            catch (Exception)
            {
            }

            logger.LogError(context.Failure,
                $"An error has occurred while authenticating user against identity server. JSON: {contextAsJson}.");

            context.HandleResponse();
            context.Response.Redirect("/error/authfailure");

            return Task.CompletedTask;
        }

        private static Task SetupAdditionalProperties(RedirectContext context)
        {
            if (context.Properties.Items.ContainsKey("acr_values"))
            {
                context.ProtocolMessage.AcrValues = context.Properties.Items["acr_values"];
            }

            return Task.CompletedTask;
        }

        private static async Task SetupIdentityAndClaimsAndGa(
            TicketReceivedContext context, OpenIdAuthOptions authOptions)
        {
            var id = await GetClaimsIdentity(
                context.Scheme.Name,
                context.Properties.GetTokenValue("access_token"),
                context.HttpContext,
                authOptions,
                context.Principal.Claims?.ToArray());

            await RegisterExternalAccount(context, id);

            try
            {
                await context.HttpContext.ConfirmAccountAsync(
                    id,
                    context.Properties.GetTokenValue("access_token"));
            }
            catch (Exception)
            {
                // Not logging here as it's already logged elsewhere.

                context.HandleResponse();
                context.Response.Redirect("/error/authfailure");

                return;
            }

            context.Principal = new ClaimsPrincipal(id);

            // The following is for GA.
            var subjectIdClaim = id.Claims.FirstOrDefault(x => x.Type == "sub");

            if (subjectIdClaim != null)
            {
                var distributedCache =
                    context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

                await distributedCache.SetAsync($"JUST-LOGGED-IN-{subjectIdClaim?.Value}",
                    Encoding.UTF8.GetBytes("TRUE"),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                    });
            }
        }

        private static async Task<ClaimsIdentity> GetClaimsIdentity(
            string schemeName,
            string accessToken,
            HttpContext context,
            OpenIdAuthOptions authOptions,
            Claim[] additionalClaims = null)
        {
            var id = new ClaimsIdentity(schemeName);

            if (string.Equals(authOptions.TokenType, TokenTypes.Jwt, StringComparison.CurrentCultureIgnoreCase))
            {
                // JWT token.

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(accessToken);

                id.AddClaims(jwtToken.Claims);
            }
            else
            {
                // Reference token.
                var discoveryCache = context.RequestServices.GetRequiredService<IDiscoveryCache>();

                var disco = await discoveryCache.GetAsync();

                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }

                var client = context.RequestServices.GetRequiredService<IHttpClientFactory>()
                    .CreateClient();

                var introspectionResponse = await client.IntrospectTokenAsync(new TokenIntrospectionRequest
                {
                    Address = disco.IntrospectionEndpoint,
                    ClientId = authOptions.ScopeId,
                    ClientSecret = authOptions.ScopeSecret,

                    Token = accessToken
                });

                if (introspectionResponse.IsError)
                {
                    throw new Exception(introspectionResponse.Error);
                }

                if (!introspectionResponse.IsActive)
                {
                    throw new Exception("Obtained token is inactive.");
                }

                id.AddClaims(introspectionResponse.Claims);
            }

            if (additionalClaims != null && additionalClaims.Any())
            {
                id.AddClaims(additionalClaims);
            }

            return id;
        }

        private static async Task RegisterExternalAccount(TicketReceivedContext context, ClaimsIdentity identity)
        {
            var setting = context.HttpContext.RequestServices.GetRequiredService<PortalSetting>();

            ////if (!setting.EnableExternalLoginProviders)
            ////{
            ////    //// External providers are not enabled.
            ////    return;
            ////}

            try
            {
                ////var encAlg = context.HttpContext.RequestServices.GetRequiredService<IPortalCryptoAlgorithm>();

                ////var state = context.Properties.Items["state"];
                ////var decryptedState = encAlg.DecryptUsingAes(state);

                ////    if (!string.IsNullOrWhiteSpace(decryptedState))
                ////    {
                ////        var vm = JsonConvert.DeserializeObject<ExternalRegisterViewModel>(decryptedState);

                ////        if (vm != null)
                ////        {
                ////            if (identity.Claims.Any(x => x.Type == Constants.CaseflowUserId))
                ////            {
                ////                /*
                ////                 * This user seem to already have the caseflow userid. Consider the following scenario.
                ////                 *
                ////                 * Husband  - Registers using Google with email a@gmail.com against Lowell Account Ref = X
                ////                 * Wife     - Registers using Google with email a@gmail.com against Lowell Account Ref = Y
                ////                 *
                ////                 * If the following try-catch runs then Wife basically hijacks Husband's account,
                ////                 * but wife only sees her account data. When husband logs in to his account he sees Wife's data.
                ////                 * Which is a security breach!
                ////                 *
                ////                 * If we don't run the try-catch and simply return then Wife will see Husbands account data
                ////                 * which is also a security breach.
                ////                 *
                ////                 * We can throw here to say you already have an account and you will have to register with an account
                ////                 * that we don't know of.
                ////                 *
                ////                 */

                ////                throw new SecurityException("Account already exists.");
                ////            }

                ////            var registerManager =
                ////                context.HttpContext.RequestServices.GetRequiredService<IRegisterService>();

                ////            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();

                ////            var step1VmBytes = await cache.GetAsync(vm.Key);
                ////            var step1VmString = Encoding.UTF8.GetString(step1VmBytes);
                ////            var step1Vm = JsonConvert.DeserializeObject<RegisterStep1ViewModel>(step1VmString);

                ////            var subjectClaim = identity.FindFirst("sub");
                ////            var subjectId = subjectClaim?.Value;

                ////            var result = await registerManager.LinkAccount(new LinkAccountViewModel
                ////            {
                ////                CaseOrReferenceNumber = step1Vm.CaseOrReferenceNumber,
                ////                Postcode = step1Vm.Postcode,
                ////                DateOfBirth = step1Vm.DateOfBirth,
                ////                SubjectId = subjectId
                ////            });

                ////            if (!result.IsSuccess)
                ////            {
                ////                throw new Exception(result.Message);
                ////            }

                ////            identity.AddClaim(new Claim(Constants.CaseflowUserId, result.CaseflowUserId));
                ////        }
                ////    }
                ////}
                ////catch (SecurityException exception)
                ////{
                ////    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                ////    logger.LogError(exception,
                ////        "An unknown error has occurred while trying to setup external account registration.");

                ////    throw;
            }
            catch (Exception exception)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                logger.LogError(exception,
                    "An unknown error has occurred while trying to setup external account registration.");
            }
        }
    }
}