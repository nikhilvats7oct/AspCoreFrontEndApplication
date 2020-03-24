using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.Services.Models
{
    public class CustomerSummary
    {
        public IncomeAndExpenditure IncomeAndExpenditure { get; set; }
        public List<AccountSummary> Accounts { get; set; }
        public IDictionary<string, Guid> SurrogateKeysByLowellReference { get; set; }
    }
}
