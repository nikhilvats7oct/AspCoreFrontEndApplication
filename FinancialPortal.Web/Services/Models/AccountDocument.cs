using System.Collections.Generic;

namespace FinancialPortal.Web.Services.Models
{
    public class AccountDocument
    {
        public string AccountReference { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal DiscountedBalance { get; set; }

        public List<Document> Documents { get; set; }
    }
}
