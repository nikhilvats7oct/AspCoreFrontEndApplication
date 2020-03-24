using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Helpers;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Proxy;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FinancialPortal.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static CookieConsentSetting GetCookieConsentSetting(this HttpContext context)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

            try
            {
                var headerCookie = context.Request.Headers["Cookie"];

                logger.LogDebug($"Cookie header found to be {headerCookie}.");

                var consentCookie = headerCookie.ToString().Split(';')
                    .FirstOrDefault(x => x.Trim().StartsWith(ConsentCookieSetting.Name));

                if (consentCookie == null)
                {
                    return null;
                }

                var consentCookieValue = consentCookie.Split('=')[1];
                var consentCookieValueDecoded = WebUtility.UrlDecode(consentCookieValue);
                var consentSetting = JsonConvert.DeserializeObject<CookieConsentSetting>(consentCookieValueDecoded);

                return consentSetting;
            }
            catch (Exception exception)
            {
                logger.LogError(exception,
                    "An unknown error has occurred while trying to check if " +
                    "cookie consent required from the cookies in request header.");

                return null;
            }
        }

        public static async Task ConfirmAccountAsync(
            this HttpContext context, ClaimsIdentity identity, string accessToken)
        {
            if (identity.Claims.All(x => x.Type != Constants.CaseflowUserConfirmed))
            {
                try
                {
                    const string uri = "api/Register/CompleteRegistration";

                    var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();

                    var client = httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");


                    var portalSetting = context.RequestServices.GetRequiredService<PortalSetting>();

                    var userDetails = GetUserDetailsFromClaims(context, identity);

                    var completeRegistrationDto = new
                    {
                        LowellReference = userDetails.lowellReferenceNumber, EmailAddress = userDetails.email,
                        UserId = userDetails.userId
                    };

                    var inputMessage = new HttpRequestMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(completeRegistrationDto), Encoding.UTF8,
                            "application/json")
                    };

                    inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.PostAsync($"{portalSetting.GatewayEndpoint}{uri}", inputMessage.Content);

                    if (!result.IsSuccessStatusCode)
                    {
                        throw new RestException(result.StatusCode, await result.Content.ReadAsStringAsync());
                    }

                    var registerService = context.RequestServices.GetRequiredService<IRegisterService>();

                    await registerService.UpdateIdentityServerAccountWithAccountConfirmationProperty(
                        identity.FindFirst("sub")?.Value);

                    var gtmService = context.RequestServices.GetRequiredService<IGtmService>();

                    gtmService.RaiseRegistrationEvent_Complete(
                        new LoginVm
                        {
                            EmailAddress = userDetails.email, LowellReference = userDetails.lowellReferenceNumber
                        }, userDetails.userId, null);

                    var webActivityService = context.RequestServices.GetRequiredService<IWebActivityService>();
                    await webActivityService.LogRegistrationActivated(userDetails.lowellReferenceNumber, userDetails.userId);
                }
                catch (Exception e)
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

                    logger.LogError(e,
                        "An unknown error has occurred while trying to confirm user in caseflow and identity.");

                    throw;
                }
            }
        }

        public static string GetUrl(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";
        }

        private static (
            string lowellReferenceNumber, 
            string email, 
            string userId) 
            GetUserDetailsFromClaims(
            HttpContext context, ClaimsIdentity identity)
        {
            var caseflowReference = identity.FindFirst("caseflow_reference")?.Value;

            var encryption = context.RequestServices.GetRequiredService<EncryptionSetting>();

            var reference = AesHelper.Decrypt(caseflowReference, encryption.AesKey, encryption.AesInitializationVector);

            var email = identity.FindFirst("email")?.Value;

            var userId = identity.FindFirst(Constants.CaseflowUserId)?.Value;

            return (reference, email, userId);
        }
    }
}