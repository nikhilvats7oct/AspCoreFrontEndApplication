using System;

namespace FinancialPortal.Web.Services.Models
{
    public class Document
    {
        public string DocumentId { get; set; }
        public bool NewDocument { get; set; }
        public string Subject { get; set; }
        public DateTime Recieved { get; set; }
        public DateTime Read { get; set; }
    }
}
