using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class RegisterVm : IGtmEventRaisingVm
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string LowellReference { get; set; }
        public string ConfirmPassword { get; set; }
        public string HoneyPotTextBox { get; set; }
        public bool TsAndCsAccepted { get; set; }
        public string CallbackUrl { get; set; }
        public string UserId { get; set; }
        public string NotificationMessage { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}