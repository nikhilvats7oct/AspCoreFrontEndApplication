using AutoMapper;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.DependencyInjection;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.Services.ViewModelBuilders;
using FinancialPortal.Web.Services.WebActivityLogs;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace FinancialPortal.Web.Services.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceMappings(
            this IServiceCollection services, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            services.AddScoped<IBuildDataProtectionVmService, BuildDataProtectionVmService>();
            services.AddScoped<IMyProfileService, MyProfileService>();
            services.AddScoped<IBuildPaymentOptionsVmService, BuildPaymentOptionsVmService>();
            services.AddScoped<IBuildOneOffPaymentReviewVmService, BuildOneOffPaymentReviewVmService>();
            services.AddScoped<IBuildDirectDebitDetailsVmService, BuildDirectDebitDetailsVmService>();

            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IDataProtectionService, DataProtectionService>();

            services.AddScoped<ITransactionsService, TransactionsService>();

            services.AddScoped<IVerifonePaymentProviderService, VerifonePaymentProviderService>();
            services.AddScoped<IWebActivityService, WebActivityService>();

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IGtmService, GtmService>();
            services.AddScoped<IDirectDebitPlanSetupService, DirectDebitPlanSetupService>();

            services.AddHttpClient<IOpenWrksTokenProvider, OpenWrksTokenProvider>();

            services.AddScoped<IOpenWrksService, OpenWrksService>(x => new OpenWrksService(
                x.GetRequiredService<PortalSetting>(),
                new RestClient(
                        x.GetRequiredService<ILogger<RestClient>>(),
                        x.GetRequiredService<IHttpClientFactory>().CreateClient("openwrks")),
                x.GetRequiredService<OpenWrksSetting>(),
                x.GetRequiredService<ILogger<OpenWrksService>>(),
                x.GetRequiredService<IMapper>()));

            services.AddScoped<IBuildPaymentOptionsVmService, BuildPaymentOptionsVmService>();

            services.AddScoped<IBuildDirectDebitPlanOverviewVmService, BuildDirectDebitPlanOverviewVmService>();

            services.AddScoped<IBuildContactUsVmService, BuildContactUsVmService>();
            services.AddScoped<IContactUsService, ContactUsService>();

            services.AddScoped<IBuildCallbackVmService, BuildCallbackVmService>();
            services.AddScoped<ICallbackService, CallbackService>();

            services.AddScoped<IApiGatewayHeartbeatService, ApiGatewayHeartbeatService>();
            services.AddScoped<IBuildAmendDirectDebitVmService, BuildAmendDirectDebitVmService>();
            services.AddScoped<IAmendDirectDebitPlanService, AmendDirectDebitPlanService>();

            services.AddProcessMappings(loggerFactory, configuration);
            services.AddScoped<ITriggerFigureService, TriggerFigureService>();
            services.AddScoped<ICalculatorService, CalculatorService>();

            services.AddScoped<IDirectDebitTermCalculator, DirectDebitTermCalculator>();

            services.AddHttpContextAccessor();
            services.AddScoped<IApplicationSessionState, ApplicationSessionState>();
            services.AddTransient<ICallbackService, CallbackService>();

            services.AddScoped<IBudgetCalculatorService, BudgetCalculatorService>();
            services.AddScoped<IDocumentsService, DocumentsService>();
            services.AddScoped<IContactLinksService, ContactLinksService>();

            return services;
        }
    }
}
