using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.ViewModels
{
    public class PdfViewModel
    {
        public DateTime GeneratedDateTime { get; set; }
        public string AccountName { get; set; }
        public decimal AccountBalance { get; set; }
        public string AccountReference { get; set; }
        public List<TransactionVm> PaymentDetails { get; set; }
    }
}