using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace FinancialPortal.Web.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddDatabaseConfigurationProvider(
            this IConfigurationBuilder configuration, Action<DbContextOptionsBuilder> setup)
        {
            configuration.Add(new DatabaseConfigurationSource(setup));
            return configuration;
        }

        public static IConfigurationBuilder AddSecureJsonFile(
            this IConfigurationBuilder builder, string path)
        {
            return AddSecureJsonFile(builder, null, path, false, false);
        }

        public static IConfigurationBuilder AddSecureJsonFile(
            this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddSecureJsonFile(builder, null, path, optional, false);
        }

        public static IConfigurationBuilder AddSecureJsonFile(
            this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return AddSecureJsonFile(builder, null, path, optional, reloadOnChange);
        }

        public static IConfigurationBuilder AddSecureJsonFile(
            this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
        {
            if (provider == null && Path.IsPathRooted(path))
            {
                provider = new PhysicalFileProvider(Path.GetDirectoryName(path));
                path = Path.GetFileName(path);
            }

            var source = new SecureJsonConfigurationSource
            {
                FileProvider = provider,
                Path = path,
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };

            builder.Add(source);

            return builder;
        }
    }
}