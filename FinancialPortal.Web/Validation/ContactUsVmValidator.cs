using FinancialPortal.Web.Validation.ClientSideValidator;
using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class ContactUsVmValidator : AbstractValidator<ContactUsVm>
    {
        public ContactUsVmValidator()
        {
            string DateOfBirthPropertyName = "DateOfBirth";


            RuleFor(x => x.QueryTopic)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.ContactUs.EmptyQueryArea);

            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.ContactUs.EmptyFullName)
                .Matches(@"^[a-zA-Z\/\s]{1,100}$").WithMessage(ValidationMessages.ContactUs.EmptyFullName);

            RuleFor(x => x.AccountHolderStatus)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.ContactUs.EmptyAccountHolderStatus);


            RuleFor(x => x.AuthorisedThirdPartyPassword)
                .SetValidator(new RequiredIfValidator("AccountHolderStatus", "Authorised 3rd Party", ValidationMessages.ContactUs.EmptyThirdPartyPassword))
                .WithMessage(ValidationMessages.ContactUs.EmptyThirdPartyPassword);

            // To avoid duplicate messages, we are stopping if any date element (Day, Month, Year) is missing
            // Validity is checked later when the date is assembled
            // Checking component parts first, then date derived from them
            RuleFor(x => x.Day)
                .NotNull().OverridePropertyName(DateOfBirthPropertyName)
                .WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidDateOfBirth)
                .DependentRules(() =>
                {
                    RuleFor(x => x.Month)
                        .MaximumLength(10).WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidDateOfBirth)
                        .NotNull().OverridePropertyName(DateOfBirthPropertyName)
                        .WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidDateOfBirth)
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Year)
                                .NotNull().OverridePropertyName(DateOfBirthPropertyName)
                                .WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidDateOfBirth);
                        });
                });

            RuleFor(x => x.ContactUsEmailAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidEmailAddress)
                .Matches(@"^[a-zA-Z0-9]+(([\.\-_]{0,1})([a-zA-Z0-9]+))@((?!lowell))[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")
                .WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidEmailAddress);


            RuleFor(x => x.LowellReferenceNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidAccountReference)
                .Matches(@"^[a-zA-Z0-9\/\-]{9,10}$")
                .WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidAccountReference);


            RuleFor(x => x.FirstLineOfAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidFirstLineOfAddress)
                .Matches(@"^[a-zA-Z0-9\/\s]{1,200}$")
                .WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidFirstLineOfAddress);

            RuleFor(x => x.Postcode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidPostcode)
                .Matches(@"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})")
                .WithMessage(ValidationMessages.ContactUs.EmptyOrInvalidPostcode);

           
            RuleFor(x => x.MessageContent)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidationMessages.ContactUs.EmptyMessage)
                .MaximumLength(500)
                .WithMessage(ValidationMessages.ContactUs.EmptyMessage);
        }
    }
}
