using System;

namespace FinancialPortal.Models.ViewModels
{
    public class DocumentVm
    {
        public int DocumentId { get; set; }
        public string Subject { get; set; }
        public DateTime? Received { get; set; }
        public DateTime? Read { get; set; }
        public bool IsNewDocument { get; set; }
        public bool IsCustomer { get; set; }
    }
}
