using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class ForgotPasswordVmValidator : AbstractValidator<ForgotPasswordVm>
    {
        public ForgotPasswordVmValidator(PortalSetting portalSettings)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage(ValidationMessages.InvalidEmailAddress)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmailAddress)
                .Matches(@"^[a-zA-Z0-9\.\-_]*@((?!lowell).*)$")
                    .When(x => !portalSettings.AllowLowellEmailAddresses, ApplyConditionTo.CurrentValidator).WithMessage(ValidationMessages.InvalidEmailAddress)
                .Matches(@"^[a-zA-Z0-9\.\-_]+@(.+)$")
                    .When(x => portalSettings.AllowLowellEmailAddresses, ApplyConditionTo.CurrentValidator).WithMessage(ValidationMessages.InvalidEmailAddress);

            RuleFor(x => x.HoneyPotTextBox)
                .Null()
                .MaximumLength(0);
        }
    }
}