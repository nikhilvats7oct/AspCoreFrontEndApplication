using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IDataProtectionService
    {
        Task<ResultDto> CheckDataProtection(DataProtectionVm dataProtectionVm);
    }
}
