using System.ComponentModel.DataAnnotations;
using FinancialPortal.Web.Validation;

namespace FinancialPortal.Web.ViewModels
{
    public class ChangeEmailVm
    {
        public string CurrentEmail { get; set; }
        public string NewEmail { get; set; }

        // Had to put data annotations here for client side validations to work.
        [DataType(DataType.EmailAddress)]
        [Compare("NewEmail", ErrorMessage = ValidationMessages.MismatchEmailAddress)]
        public string ConfirmedEmail { get; set; }
        public string ConfirmedPassword { get; set; }
        public bool Success { get; set; }
        public bool Failure { get; set; }
        public int ChangeEmailRetryCount { get; set; }
    }
}