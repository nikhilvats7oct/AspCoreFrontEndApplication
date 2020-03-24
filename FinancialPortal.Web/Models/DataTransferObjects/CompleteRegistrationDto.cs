namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class CompleteRegistrationDto : ResultDto
    {
        public string LowellReference { get; set; }
        public string EmailAddress { get; set; }
        public string UserId { get; set; }
    }
}
