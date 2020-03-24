using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    [Serializable()]
    public class DirectDebitPlanOverviewVm : IGtmEventRaisingVm
    {
        // Contains direct debit details form data, which in turn contains serialised payment options form data
        // (serialised and encrypted)
        public string DirectDebitDetailsFilledInState { get; set; }
        public string ClientName { get; set; }
        public string LowellReference { get; set; }
        public decimal PaymentAmount { get; set; }
        public string StartDate { get; set; }
        public string PaymentType { get; set; }
        public string AccountHoldersName { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
        public decimal PlanTotal { get; set; }
        public string Frequency { get; set; }
        public bool GuaranteeRead { get; set; }
        public bool DiscountAccepted { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool DiscountAvailable { get; set; }
        public string PaymentFrequency { get; set; }
        public bool UserLoggedIn { get; set; }
        public string EmailAddress { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
        public int TermYears { get; set; }
        public int TermMonths { get; set; }
        public PlanSetupOptions? SelectedPlanSetupOption { get; set; }
    }
}
