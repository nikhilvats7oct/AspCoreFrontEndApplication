using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class BudgetSummaryVm : IGtmEventRaisingVm
    {
        public string EmploymentStatus { get; set; }
        public string HousingStatus { get; set; }
        public string LoggedInUserId { get; set; }
        public bool AnonUser { get; set; }
        public bool IsSaved { get; set; }
        public decimal IncomeTotal { get; set; }
        public decimal Salary { get; set; }
        public decimal Benefits { get; set; }
        public decimal Pension { get; set; }
        public decimal Other { get; set; }

        public decimal TotalExpenditure { get; set; }
        public decimal HouseholdBills { get; set; }
        public decimal Expenditure { get; set; }

        public decimal DisposableIncome { get; set; }
        public string Frequency { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
        public bool PriorityBillsInArrears { get; set; } = false;
        public bool ExternallyLaunched { get; set; }

        public string BudgetSource { get; set; }
    }
}
