using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.ApiModels
{
    public class PartialBudgetApiModel
    {
        public Guid CaseflowUserId { get; set; }
        public string LowellReference { get; set; }
        public DateTime CreatedDate { get; set; }        
        public IncomeAndExpenditureApiModel PartialBudget { get; set; }
    }
}
