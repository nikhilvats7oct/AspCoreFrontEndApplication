using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class AmendDirectDebitConfirmVm : AmendDirectDebitStateVm,  IGtmEventRaisingVm
    {
        public string LowellReference { get; set; }
        public string ClientName { get; set; }
        public decimal OutstandingBalance { get; set; }

        public decimal RegularAccountPaymentAmount { get; set; }
        public string PlanFrequency { get; set; }
        public DateTime PlanStartDate { get; set; }

        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
        public int TermYears { get; set; }
        public int TermMonths { get; set; }
    }
}
