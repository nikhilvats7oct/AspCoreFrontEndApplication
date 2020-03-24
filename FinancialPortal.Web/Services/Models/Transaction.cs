using System;

namespace FinancialPortal.Web.Services.Models
{
    public class Transaction
    {
        public Transaction() { }

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal RollingBalance { get; set; }
    }
}
