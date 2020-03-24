using System.ComponentModel.DataAnnotations;
using FinancialPortal.Web.Validation;

namespace FinancialPortal.Web.ViewModels
{
    public class ChangePasswordVm
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        // Had to put data annotations here for client side validations to work.
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = ValidationMessages.MismatchPassword)]
        public string ConfirmPassword { get; set; }

        public bool Success { get; set; }
        public bool Failure { get; set; }
    }
}