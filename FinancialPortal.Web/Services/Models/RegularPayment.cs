using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Models
{
    public class RegularPayment
    {
        public decimal Amount { get; set; }
        public string Frequency { get; set; }
    }
}
