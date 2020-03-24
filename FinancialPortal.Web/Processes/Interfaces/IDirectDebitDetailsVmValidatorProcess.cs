using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IDirectDebitDetailsVmValidatorProcess
    {
        bool Validate(DirectDebitDetailsVm directDebitDetailsVm);
    }
}
