using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<ResultDto> CheckDataProtection(DataProtectionDto dataProtectionDto);
        Task<ResultDto> CheckIsWebRegistered(WebRegisteredDto webRegisteredDto);
        Task CompleteRegistration(CompleteRegistrationDto completeRegistrationDto);
        Task<RegisterAccountResult> CreateAccount(RegisterAccount registerAccount);
        Task<ResendActivationLinkResult> ResendActivationLink(string emailAddress);
        Task<bool> IsPendingRegistration(string reference);
        Task UpdateIdentityServerAccountWithAccountConfirmationProperty(string subjectId);
    }
}
