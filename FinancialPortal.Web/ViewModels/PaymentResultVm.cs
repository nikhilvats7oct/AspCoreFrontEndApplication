using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class PaymentResultVm : IGtmEventRaisingVm
    {
        public string Reference { get; set; }
        public string Result { get; set; }
        public string TokenId { get; set; }
        public string ACode { get; set; }
        public OneOffPaymentDto PaymentInfo { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}
