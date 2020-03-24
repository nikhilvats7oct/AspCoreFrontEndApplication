namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class RegistrationEmailDto : ResultDto
    {
        public string CallBackUrl { get; set; }
        public string EmailAddress { get; set; }
        public string LowellReference { get; set; }
        public string EmailName { get; set; }
    }
}