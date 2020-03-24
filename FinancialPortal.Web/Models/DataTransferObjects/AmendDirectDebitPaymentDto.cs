using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class AmendDirectDebitPaymentDto
    {
        public string LowellReference { get; set; }
        public string ClientName { get; set; }
        public decimal OutstandingBalance { get; set; }
        public decimal RegularAccountPaymentAmount { get; set; }
        public string PlanFrequency { get; set; }
        public string PlanType { get; set; }
        public DateTime EarliestInstalmentDate { get; set; }
        public DateTime LatestPlanSetupDate { get; set; }
        public DateTime EarliestAmendInstalment { get; set; }   
        
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime? DiscountExpiry { get; set; }
        public decimal DiscountedBalance { get; set; }        
        public List<string> DirectDebitFrequencies { get; set; }
    }
}