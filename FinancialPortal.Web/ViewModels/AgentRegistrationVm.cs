using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class AgentRegistrationVm : IGtmEventRaisingVm
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool TermsAndConditionsAccepted { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}