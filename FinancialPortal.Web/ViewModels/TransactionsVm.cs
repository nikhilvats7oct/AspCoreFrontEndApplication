using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;
using X.PagedList;

namespace FinancialPortal.Web.ViewModels
{
    public class TransactionsVm : IGtmEventRaisingVm
    {
        public TransactionsVm()
        {
            FilterTransactions = new FilterTransactionsVm();
        }

        public FilterTransactionsVm FilterTransactions { get; set; }
        public string AccountName { get; set; }
        public string AccountReference { get; set; }
        public decimal AccountBalance { get; set; }
        public IPagedList<TransactionVm> PagedList { get; set; }
        public string LoggedInUserID { get; set; }
        public string LoggedInLowellRef { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}