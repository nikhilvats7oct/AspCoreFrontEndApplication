using System.Collections.Generic;

namespace FinancialPortal.Models.ViewModels
{
    public class MyDocumentsVm
    {
        public FilterDocumentsVm FilterItems { get; set; } = new FilterDocumentsVm();
        public DocumentAccountsVm Account { get; set; }
        public List<LinkedAccountsVm> LinkedAccounts { get; set; }
        public bool AccountsHaveUnreadDocuments { get; set; }
    }
}
