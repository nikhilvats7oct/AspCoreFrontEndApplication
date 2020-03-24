using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IMyProfileService
    {
        Task<ContactPreferencesRetrievedDto> GetContactPreferences(string lowellReference);

        Task SaveContactPreferences(SaveContactPreferencesDto model);
    }
}
