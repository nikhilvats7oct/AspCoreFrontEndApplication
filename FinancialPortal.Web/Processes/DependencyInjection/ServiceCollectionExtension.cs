using System.Diagnostics.CodeAnalysis;
using FinancialPortal.Web.Processes.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddProcessMappings(
            this IServiceCollection services, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            services.AddScoped<IBuildDataProtectionDtoProcess, BuildDataProtectionDtoProcess>();
            services.AddScoped<IArrearsDescriptionProcess, ArrearsDescriptionProcess>();

            services.AddScoped<ICheckDataProtectionAnonymousProcess, CheckDataProtectionAnonymousProcess>();
            services.AddScoped<IGetUserProcess, GetUserProcess>();

            services.AddScoped<IGetDaysForDoBProcess, GetDaysForDoBProcess>();
            services.AddScoped<IGetMonthsForDoBProcess, GetMonthsForDoBProcess>();
            services.AddScoped<IGetYearsForDoBProcess, GetYearsForDoBProcess>();
            services.AddScoped<IBuildCompleteRegistrationDtoProcess, BuildCompleteRegistrationDtoProcess>();
            services.AddScoped<IGetTimeSlotsProcess, GetTimeSlotsProcess>();

            services.AddScoped<ICreateVerifoneRequestProcess, CreateVerifoneRequestProcess>();
            services.AddScoped<ICreateVerifonePostProcess, CreateVerifonePostProcess>();

            services.AddScoped<IGetCurrentDirectDebitProcess, GetCurrentDirectDebitProcess>();
            services.AddScoped<ISendPaymentProcess, SendPaymentProcess>();
            services.AddScoped<IBuildPaymentDtoProcess, BuildPaymentDtoProcess>();
            services.AddScoped<IBuildDirectDebitPlanDtoProcess, BuildDirectDebitPlanDtoProcess>();
            services.AddScoped<ISendDirectDebitPlanProcess, SendDirectDebitPlanProcess>();
            services.AddScoped<IBankAccountCheckerProcess, BankAccountCheckerProcess>();
            services.AddScoped<IBuildFrequencyListProcess, BuildFrequencyListProcess>();
            services.AddScoped<IDirectDebitFrequencyTranslator, DirectDebitFrequencyTranslator>();

            services.AddScoped<IApiGatewayHeartbeatProcess, ApiGatewayHeartbeatProcess>();
            services.AddScoped<ISendAmendDirectDebitPlanProcess, SendAmendDirectDebitPlanProcess>();

            services.AddScoped<IPaymentOptionsVmValidatorProcess, PaymentOptionsVmValidatorProcess>();
            services.AddScoped<IDirectDebitDetailsVmValidatorProcess, DirectDebitDetailsVmValidatorProcess>();
            services.AddScoped<ISendToRabbitMQProcess, SendToRabbitMQProcess>();

            services.AddScoped<IAmendDirectDebitVmValidatorProcess, AmendDirectDebitVmValidatorProcess>();

            return services;
        }
    }
}
