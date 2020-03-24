using System;
using System.Collections.Generic;
using System.Linq;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class MyAccountsVm : IGtmEventRaisingVm
    {
        public Guid? LowellFinancialAccountSurrogateKey { get; set; }
        public bool IncomeAndExpenditureSubmitted { get; set; }
        public bool IncomeAndExpenditureExpired { get; set; }        
        public List<AccountSummaryVm> Accounts { get; set; }
        public List<AccountSummaryVm> NewAccounts { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
        public bool IandELessThanOrIs12MonthsOld { get; set; }
        public decimal? DisposableIncome { get; set; }
        public bool ShowBudgetCalculatorLink
        {
            get
            {
                if (!LowellFinancialAccountSurrogateKey.HasValue) { return false; }
                return Accounts.Any(x => !x.AccountStatusIsWithSolicitors);
            }
        }
    }
}