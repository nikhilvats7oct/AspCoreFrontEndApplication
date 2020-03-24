using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Settings
{
    public class OpenWrksSetting
    {
        public string ApiEndpoint { get; set; }

        public bool EnableMockDataForTesting { get; set; }

        public string TestDataFolder { get; set; }

        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string[] Scopes { get; set; }

        public int TokenExpiryTimeSpanInMinutes { get; set; }

        public bool UseLandingPage { get; set; }
    }
}
