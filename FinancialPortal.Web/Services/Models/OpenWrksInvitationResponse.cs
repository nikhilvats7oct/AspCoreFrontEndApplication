using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Models
{
    [Serializable]
    public class OpenWrksInvitationResponse
    {
        public string CustomerReference { get; set; }

        public string BudgetUrl { get; set; }
    }
}
