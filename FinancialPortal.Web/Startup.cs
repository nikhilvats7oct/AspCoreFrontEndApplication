using AutoMapper;
using FinancialPortal.Web.Filters;
using FinancialPortal.Web.HttpHandlers;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Middleware;
using FinancialPortal.Web.Processes.DependencyInjection;
using FinancialPortal.Web.Proxy.DependencyInjection;
using FinancialPortal.Web.Security;
using FinancialPortal.Web.Services;
using FinancialPortal.Web.Services.DependencyInjection;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.Validation.ClientSideValidator;
using FluentValidation.AspNetCore;
using Framework.Pdf.Aspose;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace FinancialPortal.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;
        private PortalSetting _portalSettings;

        public Startup(IConfiguration configuration,
            ILoggerFactory loggerFactory,
            IHostingEnvironment hostingEnvironment)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            _loggerFactory = loggerFactory;
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Settings
            var cmsEndpoints = new CmsEndpoints();
            Configuration.Bind("CmsEndpoints", cmsEndpoints);
            services.AddSingleton(cmsEndpoints);

            _portalSettings = new PortalSetting();
            Configuration.Bind("PortalSettings", _portalSettings);
            services.AddSingleton(_portalSettings);

            var identitySetting = new IdentitySetting();
            Configuration.Bind("Identity", identitySetting);
            services.AddSingleton(identitySetting);

            var heartbeatSetting = new HeartbeatSetting();
            Configuration.Bind("HeartBeatSettings", heartbeatSetting);
            services.AddSingleton(heartbeatSetting);

            var verifoneSetting = new VerifoneSetting();
            Configuration.Bind("Verifone", verifoneSetting);
            services.AddSingleton(verifoneSetting);

            var contactUsSetting = new ContactUsSetting { QueryAreas = new List<string>() };
            Configuration.Bind("ContactUs", contactUsSetting);
            services.AddSingleton(contactUsSetting);

            var callbackSetting = new CallbackSetting();
            Configuration.Bind("Callback", callbackSetting);
            services.AddSingleton(callbackSetting);

            var encryptionSetting = new EncryptionSetting();
            Configuration.Bind("Encryption", encryptionSetting);
            services.AddSingleton(encryptionSetting);

            var openWrksSetting = new OpenWrksSetting();
            Configuration.Bind("OpenWrksSetting", openWrksSetting);
            services.AddSingleton(openWrksSetting);

            var contentSecurityPolicyHeadersSetting = new ContentSecurityPolicyHeaderSetting();
            Configuration.Bind("ContentSecurityPolicyHeader", contentSecurityPolicyHeadersSetting);
            services.AddSingleton(contentSecurityPolicyHeadersSetting);

            SetupThreadPoolForRedis();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // This is where the application state will be stored.
            services.AddStackExchangeRedisCache(options => { options.Configuration = _portalSettings.RedisConfiguration; });

            // Data protection
            var redis = ConnectionMultiplexer.Connect(_portalSettings.RedisConfiguration);
            services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "FinancialPortal-DataProtection-Keys");

            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => { builder.SetIsOriginAllowedToAllowWildcardSubdomains(); });
            });

            services.AddSession(options =>
            {
                // Since there are no link between the User's token and Session, setting this idle time out to an hour.
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;

                if (!_hostingEnvironment.IsDevelopment())
                {
                    // On a non development environment we should be using HTTPS and hence cookies will demand https.
                    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                }

                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddMvc(options =>
                {
                    if (!_hostingEnvironment.IsDevelopment())
                    {
                        options.RequireHttpsPermanent = true;
                    }

                    // Following code forces all actions in the controller to be secure by the default auth scheme.
                    // Actions or Controller marked with AllowAnonymous will trigger auth scheme.
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                    options.Filters.Add(typeof(SecurityHeadersAttribute));
                    options.Filters.Add(typeof(LoggingAsyncActionFilter));
                    options.Filters.Add(typeof(ExceptionLoggerFilter));

                    options.ModelBindingMessageProvider
                        .SetAttemptedValueIsInvalidAccessor((value, displayName) =>
                            displayName.Contains("date of birth", StringComparison.CurrentCultureIgnoreCase)
                                ? "Please enter your date of birth in the format DD/MM/YYYY"
                                : $"The value '{WebUtility.HtmlEncode(WebUtility.UrlEncode(value))}' is not valid for {displayName}.");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<AmendDirectDebitVmValidator>();
                    fv.ConfigureClientsideValidation(clientSideValidation =>
                    {
                        clientSideValidation.Add(typeof(RequiredIfValidator),
                            (context, rule, validator) => new RequiredIfClientValidator(rule, validator));
                    });
                });

            services.AddOpenIdConnectAuth(new OpenIdAuthOptions
            {
                AuthorityEndpoint = identitySetting.Authority, // NO slash at the end
                ClientId = identitySetting.ClientId,
                ClientSecret = identitySetting.ClientSecret,
                Scopes = identitySetting.Scopes,
                ScopeId = identitySetting.ScopeId,
                ScopeSecret = identitySetting.ScopeSecret,
                RequireHttpsMetadata = false,
                RedisConfiguration = _portalSettings.RedisConfiguration,
                TokenType = identitySetting.TokenType
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Licenses
            AsposeLicense.SetLicenseFromFile("Aspose.Total.lic");

            // PDF
            services.AddScoped<ITemplateProvider, EmbeddedTemplateProvider>();
            services.AddScoped(typeof(IPdfGenerator<>), typeof(AsposePdfGenerator<>));

            services.AddHttpContextAccessor();

            services.AddTransient<TracingHandler>();
            services.AddHttpClient<InternalIdentityTokenHandler>();
            services.AddHttpClient<OpenWrksIdentityTokenHandler>();

            services.AddHttpClient();

            services.AddHttpClient<IRestClient, RestClient>()
                .AddHttpMessageHandler<TracingHandler>()
                .AddHttpMessageHandler<InternalIdentityTokenHandler>();

            services.AddHttpClient("openwrks")
                .AddHttpMessageHandler<TracingHandler>()
                .AddHttpMessageHandler<OpenWrksIdentityTokenHandler>();

            services.AddScoped<IPortalCryptoAlgorithm, PortalCryptoAlgorithm>();
            services.AddScoped<IDistributedTokenProvider, DistributedTokenProvider>();
            services.AddTransient<IUnreadDocumentsService, UnreadDocumentsService>();
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IMapperHelper, MapperHelper>();

            services.AddHttpClient<IAnonymousAccessTokenProvider, AnonymousAccessTokenProvider>();

            services.AddServiceMappings(_loggerFactory, Configuration);
            services.AddProcessMappings(_loggerFactory, Configuration);
            services.AddProxyMappings(_loggerFactory, _portalSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _loggerFactory.AddSerilog();

            app.UseAuthentication();
            app.UseSession();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers",
                        "Origin, X-Requested-With, Content-Type, Accept");
                }
            });

            // This will add a request to the header that can be used to trace all the way tot Caseflow.
            app.AddSubjectIdToLog();
            app.AddTracing();
            app.UseCookiePolicy();

            app.UseSerilogRequestLogging();

            if (_hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/errorcode", "?code={0}");
                app.UseExceptionHandler("/error");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SetupThreadPoolForRedis()
        {
            // Get the current settings.
            ThreadPool.GetMinThreads(out var currentMinWorker, out var currentMinIocp);

            Log.Logger.Information("Current configuration value for IOCP = {0} and WORKER = {1}",
                currentMinIocp, currentMinWorker);

            // Change the minimum number of worker threads and minimum asynchronous I/O completion threads.
            if (ThreadPool.SetMinThreads(
                _portalSettings.MinimumWorkerThreads, _portalSettings.MinimumIocpThreads))
            {
                // The minimum number of threads was set successfully.
                Log.Logger.Information(
                    $"Minimum configuration value set - IOCP = {_portalSettings.MinimumIocpThreads} " +
                    $"and WORKER threads = {_portalSettings.MinimumWorkerThreads}");
            }
            else
            {
                // The minimum number of threads was not changed.
                Log.Logger.Information("The minimum number of threads was not changed");
            }
        }
    }
}