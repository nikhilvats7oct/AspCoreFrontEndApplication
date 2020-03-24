using System;

namespace FinancialPortal.Web.Services.ApiModels
{
    public class DocumentItem
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime? Received { get; set; }
        public DateTime? Read { get; set; }
        public bool IsNewDocument { get; set; }
        public bool IsCustomer { get; set; }
    }
}
