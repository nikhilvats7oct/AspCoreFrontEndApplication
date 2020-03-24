using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Models.Interfaces
{
    public interface IBuildDirectDebitPlanOverviewVmValidationResult
    {
        bool IsValid { get; }

        DirectDebitPlanOverviewVm DirectDebitPlanOverviewVm { get; }
    }
}
