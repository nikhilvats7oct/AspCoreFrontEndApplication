using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Settings
{
    public class VerifoneSetting
    {
        public string ApiEndpoint { get; set; }

        public string CompletionUrlOverride { get; set; }

        public string CompletionPaymentPlanUrlOverride { get; set; }

        public string ApiVersion { get; set; }

        public string AllowedPaymentSchemes { get; set; }

        public string PaymentDescriptionFormatString { get; set; }

        public MerchantSetting Merchant { get; set; }

        public TemplateSetting Template { get; set; }
    }
}
