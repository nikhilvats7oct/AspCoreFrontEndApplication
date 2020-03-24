using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.ApiModels
{
    [Serializable]
    public class OpenWrksApiInvitationResponse
    {
        public string CustomerReference { get; set; }

        public string BudgetUrl { get; set; }
    }
}
