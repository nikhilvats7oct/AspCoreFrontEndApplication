using System;

namespace FinancialPortal.Web.ViewModels
{
    [Serializable()]
    public class PaymentOptionsSelectionsVm
    {
        public class Values
        {
            // IMPORTANT: do not change these values, otherwise client-side scripts will break
            public const string FullPayment = "full-payment";
            public const string PartialPayment = "partial-payment";
            public const string DirectDebit = "direct-debit";
            public const string PleaseSelect = "please-select";
        }

        public string Value { get; set; }
        public string DisplayedText { get; set; }
        public string DataFormValue { get; set; }   // used to identify options to client-side scripts
        public string ClassValue { get; set; }      // some options have special class names, used to hide/show them
    }
}
