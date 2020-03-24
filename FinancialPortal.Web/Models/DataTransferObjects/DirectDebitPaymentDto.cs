namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class DirectDebitPaymentDto
    {
        public string LowellReference { get; set; }
        public decimal PaymentAmount { get; set; }
        public string StartDate { get; set; }
        public string PaymentType { get; set; }
        public string AccountHoldersName { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
        public decimal PlanTotal { get; set; }
        public string Frequency { get; set; }
        public bool GuaranteeRead { get; set; }
        public bool DiscountAccepted { get; set; }
        public decimal DiscountAmount { get; set; }
        public string EmailAddress { get; set; }
        public string User { get; set; }
    }
}
