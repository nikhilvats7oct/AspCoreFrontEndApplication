using System;

namespace FinancialPortal.Web.ViewModels
{
    [Serializable()]
    public class PaymentOptionsSourceOfFundsSelectionsVm
    {
        public string Value { get; set; }
        public string DisplayedText { get; set; }
        public string DataFormValue { get; set; }   // used to identify options to client-side scripts
    }
}
