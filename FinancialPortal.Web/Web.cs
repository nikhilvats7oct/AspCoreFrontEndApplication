using System.Collections.Generic;
using System.Fabric;
using System.IO;
using FinancialPortal.Web.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Serilog;

namespace FinancialPortal.Web
{
    /// <summary>
    ///     The FabricRuntime creates an instance of this class for each service type instance.
    /// </summary>
    internal class Web : StatelessService
    {
        public Web(StatelessServiceContext context)
            : base(context)
        {
        }

        /// <summary>
        ///     Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                            .UseKestrel()
                            .ConfigureServices(
                                services => services
                                    .AddSingleton(serviceContext))
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .ConfigureAppConfiguration((hostingContext, config) =>
                            {
                                config.SetBasePath(Directory.GetCurrentDirectory());
                                config.AddSecureJsonFile("appsettings.json", false, true);
                           })
                            .ConfigureLogging((hostingContext, logging) =>
                            {
                                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                                logging.AddConsole();
                                logging.AddDebug();
                            })
                            .UseStartup<Startup>()
                            .UseSerilog()
                            .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                            .UseUrls(url)
                            .Build();
                    }))
            };
        }
    }
}