using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Models
{
    public class ExpenditureMetric
    {
        public string Name { get; set; }
        public decimal Vehicle { get; set; }
        public decimal Adult { get; set; }
        public decimal ChildrenUnder14 { get; set; }
        public decimal Children14To18 { get; set; }
        public decimal AdditionalAdult { get; set; }
        public int AdjustmentPercentage { get; set; }
    }
}
