using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Models
{
    public class Outgoing
    {
        public decimal Amount { get; set; }
        public string Frequency { get; set; }
        public bool InArrears { get; set; }
        public decimal ArrearsAmount { get; set; }
    }
}
