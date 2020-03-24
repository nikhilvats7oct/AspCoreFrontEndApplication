using FinancialPortal.Web.Validation.ClientSideValidator;
using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class CallbackVmValidator : AbstractValidator<CallbackVm>
    {
        public CallbackVmValidator()
        {

            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.Callback.EmptyFullName)
                .Matches(@"^[a-zA-Z][a-zA-Z\/\s]*$").WithMessage("Please enter your full name without any special characters (eg Smith-Bloggs enter smith bloggs)");

            RuleFor(x => x.AccountHolderStatus)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.Callback.EmptyAccountHolderStatus);

            RuleFor(x => x.LowellReferenceNumber)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .Matches(@"^[a-zA-Z0-9]{1,10}$")
               .WithMessage(ValidationMessages.Callback.EmptyOrInvalidAccountReference);


            ////Preferred Telephone number
            ////Number can be no longer than 17 characters if starting with + 44
            ////Number can be no longer than 12 characters if starting with 0 i.e 11 numbers plus a space
            ////Number can include digits(0 - 9), single spaces, and the '+' symbol as the first character
            ////contains at least 11 digits only numeric characters and spaces

            RuleFor(x => x.PreferredTelephoneNumber)
                .NotEmpty().WithMessage(ValidationMessages.Callback.InvalidPreferredTelephoneNumber)
                .Matches(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$")
                .WithMessage(ValidationMessages.Callback.InvalidPreferredTelephoneNumber)
                .MaximumLength(17);

            RuleFor(x => x.TimeSlot)
                .SetValidator(new RequiredIfValidator("CallmeNow", "Appointment", ValidationMessages.Callback.EmptyTimeSlot))
                .WithMessage(ValidationMessages.Callback.EmptyTimeSlot);

            RuleFor(x => x.TimeSlotSunday)
                .SetValidator(new RequiredIfValidator("CallmeNow", "Appointment", ValidationMessages.Callback.EmptyTimeSlot))
                .WithMessage(ValidationMessages.Callback.EmptyTimeSlot);


            RuleFor(x => x.TimeSlotWeekday)
                .SetValidator(new RequiredIfValidator("CallmeNow", "Appointment", ValidationMessages.Callback.EmptyTimeSlot))
                .WithMessage(ValidationMessages.Callback.EmptyTimeSlot);

            RuleFor(x => x.TimeSlotSaturday)
                .SetValidator(new RequiredIfValidator("CallmeNow", "Appointment", ValidationMessages.Callback.EmptyTimeSlot))
                .WithMessage(ValidationMessages.Callback.EmptyTimeSlot);

            RuleFor(x => x.CallbackDate)
               .SetValidator(new RequiredIfValidator("TimeSlot", "", ValidationMessages.Callback.EmptyOrInvalidCallbackDate))
               .WithMessage(ValidationMessages.Callback.EmptyOrInvalidCallbackDate);
        }
    }
}
