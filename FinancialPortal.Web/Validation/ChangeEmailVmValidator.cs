using System;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class ChangeEmailVmValidator : AbstractValidator<ChangeEmailVm>
    {
        public ChangeEmailVmValidator(PortalSetting portalSettings)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage(ValidationMessages.InvalidEmailAddress)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmailAddress)
                .Matches(@"^[a-zA-Z0-9\.\-_]*@((?!lowell).*)$")
                    .When(x => !portalSettings.AllowLowellEmailAddresses, ApplyConditionTo.CurrentValidator).WithMessage(ValidationMessages.InvalidEmailAddress)
                .Matches(@"^[a-zA-Z0-9\.\-_]+@(.+)$")
                    .When(x => portalSettings.AllowLowellEmailAddresses, ApplyConditionTo.CurrentValidator).WithMessage(ValidationMessages.InvalidEmailAddress)
                .NotEqual(x => x.CurrentEmail, StringComparer.CurrentCultureIgnoreCase).WithMessage(ValidationMessages.NewEmailMatchesCurrent);

            RuleFor(x => x.ConfirmedEmail)
                .NotEmpty().When(HasNewEmail).WithMessage("Please confirm your email address")
                .Equal(x => x.NewEmail).When(HasNewEmail).WithMessage(ValidationMessages.MismatchEmailAddress);

            RuleFor(x => x.ConfirmedPassword)
                .NotEmpty().WithMessage(ValidationMessages.IncorrectPassword);
        }

        private bool HasNewEmail(ChangeEmailVm model)
        {
            return !String.IsNullOrEmpty(model.NewEmail);
        }
    }

}
