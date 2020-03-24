using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class LoginVmValidator  : AbstractValidator<LoginVm>
    {
        public LoginVmValidator()
        {
            RuleFor(x => x.EmailAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.MissingEmailAddress)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmailAddress);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationMessages.MissingPassword);

            RuleFor(x => x.HoneyPotTextBox)
                .Null()
                .MaximumLength(0);
        }
    }
}