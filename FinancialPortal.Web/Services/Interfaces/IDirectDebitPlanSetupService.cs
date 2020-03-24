using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IDirectDebitPlanSetupService
    {
        Task CreateDirectDebitPlan(DirectDebitPlanOverviewVm directDebitPlanOverviewVm);
        Task<ResultDto> CheckDirectDebitDetails(DirectDebitDetailsVm directDebitDetailsVm);
    }
}
