using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using FinancialPortal.Web.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Runtime;
using Serilog;

namespace FinancialPortal.Web
{
    internal static class Program
    {
        /// <summary>
        ///     This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            var sfAppName = Environment.GetEnvironmentVariable("Fabric_ApplicationName");
            var isSf = sfAppName != null;

            if (isSf)
            {
                RunAsFabric();
            }
            else
            {
                RunAsConsole();
            }
        }

        private static void RunAsConsole()
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();

            var endpoint = "http://localhost:50041";

            var builder = WebHost
                 .CreateDefaultBuilder()
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
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
                 .UseUrls(endpoint)
                 .Build();

            builder.Run();

            Console.WriteLine($"Running on {endpoint}.");
            Console.ReadLine();
        }

        private static void RunAsFabric()
        {
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                ServiceRuntime.RegisterServiceAsync("FinancialPortal.WebType",
                    context => new FinancialPortal.Web.Web(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Web).Name);

                // Prevents this host process from terminating so services keeps running. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }

    internal class CurrentDirectoryHelpers
    {
        internal const string AspNetCoreModuleDll = "aspnetcorev2_inprocess.dll";

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(AspNetCoreModuleDll)]
        private static extern int http_get_application_properties(ref IISConfigurationData iiConfigData);

        public static void SetCurrentDirectory()
        {
            try
            {
                // Check if physical path was provided by ANCM
                var sitePhysicalPath = Environment.GetEnvironmentVariable("ASPNETCORE_IIS_PHYSICAL_PATH");
                if (string.IsNullOrEmpty(sitePhysicalPath))
                {
                    // Skip if not running ANCM InProcess
                    if (GetModuleHandle(AspNetCoreModuleDll) == IntPtr.Zero) return;

                    var configurationData = default(IISConfigurationData);
                    if (http_get_application_properties(ref configurationData) != 0) return;

                    sitePhysicalPath = configurationData.pwzFullApplicationPath;
                }

                Environment.CurrentDirectory = sitePhysicalPath;
            }
            catch
            {
                // ignore
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IISConfigurationData
        {
            public readonly IntPtr pNativeApplication;
            [MarshalAs(UnmanagedType.BStr)] public readonly string pwzFullApplicationPath;
            [MarshalAs(UnmanagedType.BStr)] public readonly string pwzVirtualApplicationPath;
            public readonly bool fWindowsAuthEnabled;
            public readonly bool fBasicAuthEnabled;
            public readonly bool fAnonymousAuthEnable;
        }
    }
}