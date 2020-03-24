using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface ICheckDataProtectionAnonymousProcess
    {
        Task<ResultDto> CheckDataProtection(DataProtectionDto dataProtectionDto);
    }
}
