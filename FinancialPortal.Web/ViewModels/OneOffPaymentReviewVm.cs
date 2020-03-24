using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    [Serializable()]

    public class OneOffPaymentReviewVm : IGtmEventRaisingVm
    {
        public string FilledInPaymentOptionsState { get; set; }

        public string ClientName { get; set; }
        public string LowellReference { get; set; }
        public bool ConfirmedDetailsCorrect { get; set; }
        public decimal PaymentAmount { get; set; }
        public string VerifoneUrl { get; set; }
        public string VerifonePostDataXml { get; set; }
        public string VerifoneTransactionGuid { get; set; }
        public string SourceOfFunds { get; set; }
        public string SourceOfFundsOther { get; set; }
        public string UserID { get; set; }
        public bool PaidInFull { get; set; }
        public bool DiscountAvailable { get; set; }
        public bool DiscountSelected { get; set; }
        public bool PlanInPlace { get; set; }
        public bool InArrears { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}
