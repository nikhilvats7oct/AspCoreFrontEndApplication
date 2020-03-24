using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;
using FluentValidation.Results;

namespace FinancialPortal.Web.Processes
{
    public class DirectDebitDetailsVmValidatorProcess : IDirectDebitDetailsVmValidatorProcess
    {
        // Simple check to see if model is valid
        // This is only used for final validation to detect account changes or tampering
        // therefore does not need to feed back errors to user
        public bool Validate(DirectDebitDetailsVm directDebitDetailsVm)
        {
            DirectDebitDetailsVmValidator validator = new DirectDebitDetailsVmValidator();
            ValidationResult result = validator.Validate(directDebitDetailsVm);

            return result.IsValid;
        }
    }
}
