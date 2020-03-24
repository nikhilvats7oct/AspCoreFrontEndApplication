using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IGetCurrentDirectDebitProcess
    {
        Task<AmendDirectDebitPaymentDto> GetCurrentDirectDebit(AccountReferenceDto accountReferenceDto);
    }
}