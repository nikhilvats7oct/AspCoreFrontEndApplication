using System.Collections.Generic;

namespace FinancialPortal.Models.ViewModels
{
    public class LinkedAccountsVm
    {
        public KeyValuePair<string, string> Account { get; set; }
        public string Reference { get; set; }
        public bool UnreadDocuments { get; set; }
        public bool IsSelected { get; set; }
    }
}
