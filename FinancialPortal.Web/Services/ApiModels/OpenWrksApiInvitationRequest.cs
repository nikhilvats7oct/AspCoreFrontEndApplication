using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.ApiModels
{
    [Serializable]
    public class OpenWrksApiInvitationRequest
    {
        public string LowellReference { get; set; }

        public string UserId { get; set; }

        public string RedirectUri { get; set; }
    }
}
