using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IAmendDirectDebitVmValidatorProcess
    {
        bool Validate(AmendDirectDebitVm directDebitVm);
    }
}
