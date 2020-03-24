using X.PagedList;

namespace FinancialPortal.Models.ViewModels
{
    public class DocumentAccountsVm
    {
        public string AccountName { get; set; }
        public string Reference { get; set; }
        public decimal OutstandingBalance { get; set; }
        public decimal? DiscountedBalance { get; set; }
        public bool UnreadDocuments { get; set; }
        public IPagedList<DocumentVm> Documents { get; set; }
        public string AccountReference { get; set; }
    }
}


