using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class PaymentOptionsDto
    {
        public decimal OutstandingBalance { get; set; }
        public string LowellReference { get; set; }
        public string ClientName { get; set; }
        public string AccountStatus { get; set; }
        public bool CanMakeFullPayment { get; set; }
        public bool CanMakePartialPayment { get; set; }
        public bool CanSetupDirectDebit { get; set; }
        public string ExcludedAccountMessage { get; set; }
        public bool PlanInPlace { get; set; }
        public bool WithLowellSolicitors { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountExpiryDate { get; set; }
        public decimal DiscountedBalance { get; set; }
        public decimal ProposedDiscountedBalanceIfAccepted { get; set; }
        public bool DiscountedBalancePreviouslyAccepted { get; set; }
        public bool DiscountBalanceAvailable { get; set; }
        public List<string> SourceOfFundsPaymentOptions { get; set; }
        public List<string> DirectDebitFrequencies { get; set; }

        public decimal? PaymentPlanArrearsAmount { get; set; }         // will be null if not in arrears or does not have payment plan
        public bool PaymentPlanIsAutomated { get; set; }
        public bool PaymentPlanIsDirectDebit { get; set; }

        public DateTime DirectDebitStartDateEarliest { get; set; }
        public DateTime DirectDebitStartDateLatest { get; set; }
        public bool StandingOrder { get; set; }
        public string StandingOrderMessage { get; set; }
    }
}
