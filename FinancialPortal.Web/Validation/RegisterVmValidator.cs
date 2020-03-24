using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class RegisterVmValidator : AbstractValidator<RegisterVm>
    {
        public RegisterVmValidator(PortalSetting portalSetting)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage(ValidationMessages.InvalidEmailAddress)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmailAddress)
                .Matches(@"^[a-zA-Z0-9\.\-_]*@((?!lowell).*)$")
                    .When(x => !portalSetting.AllowLowellEmailAddresses, ApplyConditionTo.CurrentValidator).WithMessage(ValidationMessages.InvalidEmailAddress)
                .Matches(@"^[a-zA-Z0-9\.\-_]+@(.+)$")
                    .When(x => portalSetting.AllowLowellEmailAddresses, ApplyConditionTo.CurrentValidator).WithMessage(ValidationMessages.InvalidEmailAddress);

            RuleFor(x => x.Password)
                .Equal(x => x.ConfirmPassword).WithMessage(ValidationMessages.MismatchPassword)
                .NotEmpty().WithMessage(ValidationMessages.BadFormatPassword)
                .MinimumLength(8).WithMessage(ValidationMessages.BadFormatPassword)
                .MaximumLength(50).WithMessage(ValidationMessages.BadFormatPassword)
                .Matches(@"^(?=.*[=£)(|<>~:;""'\-\+_\\/!@\{\}#$%^&\[\]*.,?])(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,50}").WithMessage(ValidationMessages.BadFormatPassword);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(ValidationMessages.MismatchPassword)
                .NotEmpty().WithMessage(ValidationMessages.BadFormatPassword)
                .MinimumLength(8).WithMessage(ValidationMessages.BadFormatPassword)
                .MaximumLength(50).WithMessage(ValidationMessages.BadFormatPassword)
                .Matches(@"^(?=.*[=£)(|<>~:;""'\-\+_\\/!@\{\}#$%^&\[\]*.,?])(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,50}").WithMessage(ValidationMessages.BadFormatPassword);

            RuleFor(x => x.TsAndCsAccepted).Equal(true).WithMessage(ValidationMessages.NotAcceptedTermsAndConditions);

            RuleFor(x => x.HoneyPotTextBox)
                .Null()
                .MaximumLength(0);
        }
    }
}