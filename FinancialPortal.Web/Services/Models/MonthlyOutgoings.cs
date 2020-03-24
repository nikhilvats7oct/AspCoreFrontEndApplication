using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Models
{
    public class MonthlyOutgoings
    {
        public decimal HouseholdBills { get; set; }
        public decimal Expenditures { get; set; }
        public decimal Total { get; set; }
    }
}
