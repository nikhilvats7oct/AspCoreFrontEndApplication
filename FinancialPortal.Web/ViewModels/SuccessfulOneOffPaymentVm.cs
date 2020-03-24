using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class SuccessfulOneOffPaymentVm : IGtmEventRaisingVm
    {
        public string ClientName { get; set; }
        public OneOffPaymentDto PaymentInfo { get; set; }
        public bool UserLoggedIn { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}
