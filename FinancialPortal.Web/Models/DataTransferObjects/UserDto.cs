namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class UserDto : ResultDto
    {
        public string LowellReference { get; set; }
        public string EmailAddress { get; set; }
        public int Company { get; set; }
    }
}
