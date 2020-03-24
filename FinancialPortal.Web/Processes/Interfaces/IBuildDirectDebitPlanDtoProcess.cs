using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IBuildDirectDebitPlanDtoProcess
    {
        DirectDebitPaymentDto BuildDirectDebitPlanDto(DirectDebitPlanOverviewVm directDebitPlanOverviewVm);
    }
}
