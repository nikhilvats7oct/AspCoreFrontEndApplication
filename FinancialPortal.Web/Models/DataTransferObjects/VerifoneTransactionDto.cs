namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class VerifoneTransactionDto
    {
        public string TransactionGuid { get; set; }
        public string TransactionData { get; set; }
        public byte Status { get; set; }
        public string Result { get; set; }
        public string TokenId { get; set; }
        public string AuthorisationCode { get; set; }
        public int CompanyCode { get; set; }

    }
}
