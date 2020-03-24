namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class OneOffPaymentDto
    {
        public string LowellReference { get; set; }
        public string ClientName { get; set; }
        public decimal PaymentAmount { get; set; }
        public string SourceOfFunds { get; set; }
        public string SourceOfFundsOther { get; set; }
        public string UserID { get; set; }
        public bool PaidInFull { get; set; }
        public bool DiscountAvailable { get; set; }
        public bool DiscountSelected { get; set; }
        public bool PlanInPlace { get; set; }
        public bool InArrears { get; set; }
    }
}
