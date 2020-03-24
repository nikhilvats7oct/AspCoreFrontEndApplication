namespace FinancialPortal.Web.Services.Interfaces.Utility
{
    public interface IUserIdentity
    {
        bool IsLoggedInUser { get; }
        string UserId { get; }
        string EmailAddress { get; }
    }
}
