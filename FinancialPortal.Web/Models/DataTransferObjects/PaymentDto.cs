namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class PaymentDto
    {
        public string LowellReference { get; set; }
        public string CardId { get; set; }
        public decimal Amount { get; set; }
        public bool SettlementAmount { get; set; }
        public string AuthCode { get; set; }
        public string ReplayId { get; set; }
        public string SourceOfFunds { get; set; }
        public bool PlanInPlace { get; set; }
        public string User { get; set; }
    }
}
