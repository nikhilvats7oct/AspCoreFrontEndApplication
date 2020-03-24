using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class ContactPreferencesVmValidator : AbstractValidator<ContactPreferencesVm>
    {
        public ContactPreferencesVmValidator()
        {
            RuleFor(x => x.MobileNumber)
                .Matches(@"^(\+44\s?7\d{3}|\(?07\d{3}\)?)\s?\d{3}\s?\d{3}$")
                .WithMessage("Please enter a valid mobile number")
                .MaximumLength(17);
        }
    }
}
