using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IBuildCompleteRegistrationDtoProcess
    {
        CompleteRegistrationDto Build(string lowellReference, string userId, string userEmail);
    }
}
