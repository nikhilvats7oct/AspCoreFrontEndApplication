using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Models
{
    public class MonthlyIncome
    {
        public decimal Salary { get; set; }
        public decimal Benefits { get; set; }
        public decimal Other { get; set; }
        public decimal Pension { get; set; }
        public decimal Total { get; set; }
    }
}
