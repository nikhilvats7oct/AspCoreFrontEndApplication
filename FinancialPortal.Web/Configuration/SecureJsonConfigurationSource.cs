using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace FinancialPortal.Web.Configuration
{
    public class SecureJsonConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            // FileProvider = FileProvider ?? builder.GetFileProvider();

            return new SecureJsonConfigurationProvider(this);
        }
    }
}