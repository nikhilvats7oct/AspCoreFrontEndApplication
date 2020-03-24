using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Settings
{
    public class TemplateSetting
    {
        public int LanguageTemplateId { get; set; }

        public MerchantTemplateSetting MerchantTemplateId { get; set; }
    }
}
