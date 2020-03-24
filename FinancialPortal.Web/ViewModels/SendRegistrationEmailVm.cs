using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class SendRegistrationEmailVm : IGtmEventRaisingVm
    {
        public bool ReSend { get; set; }
        public string LowellReference { get; set; }
        public string UserID { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}
