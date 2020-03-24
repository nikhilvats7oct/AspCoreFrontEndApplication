namespace FinancialPortal.Web.ViewModels
{
    public class TransactionVm
    {
        public TransactionVm() { }

        public string DateText { get; set; }
        public string Description { get; set; }
        public string AmountText { get; set; }
        public string RollingBalanceText { get; set; }
    }
}
