using System.Diagnostics.CodeAnalysis;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Proxy.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddProxyMappings(
            this IServiceCollection services, ILoggerFactory loggerFactory, PortalSetting portalSetting)
        {
            var url = portalSetting.GatewayEndpoint;

            services.AddScoped<IApiGatewayProxy>(x =>
                new ApiGatewayProxy(
                    loggerFactory.CreateLogger<ApiGatewayProxy>(),
                    x.GetRequiredService<IRestClient>(),
                    url));


            return services;
        }
    }
}