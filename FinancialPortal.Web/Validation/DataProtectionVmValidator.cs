using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class DataProtectionVmValidator : AbstractValidator<DataProtectionVm>
    {
        public DataProtectionVmValidator()
        {
            string DateOfBirthPropertyName = "DateOfBirth";

            RuleFor(x => x.LowellReference)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .MaximumLength(10).WithMessage(ValidationMessages.InvalidLowellRef)
                .Matches(@"^[a-zA-Z0-9\/\-]*$").WithMessage(ValidationMessages.InvalidLowellRef)
                    // ^ indicates match from start of string
                    // $ indicates match to end of string
                    // [ ] inside of this is the valid character ranges
                    // * zero to infinity characters
                .NotEmpty().WithMessage(ValidationMessages.MissingLowellRef);

            // To avoid duplicate messages, we are stopping if any date element (Day, Month, Year) is missing
            // Validity is checked later when the date is assembled
            // Checking component parts first, then date derived from them
            RuleFor(x => x.Day)
                .NotNull().OverridePropertyName(DateOfBirthPropertyName).WithMessage(ValidationMessages.MissingDateOfBirth)
                .DependentRules(() =>
                {
                    RuleFor(x => x.Month)
                        .MaximumLength(10).WithMessage(ValidationMessages.InvalidDateOfBirth)
                        .NotNull().OverridePropertyName(DateOfBirthPropertyName).WithMessage(ValidationMessages.MissingDateOfBirth)
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Year)
                                .NotNull().OverridePropertyName(DateOfBirthPropertyName).WithMessage(ValidationMessages.MissingDateOfBirth)
                                .DependentRules(() =>
                                {
                                    // After converted from components, DateOfBirth property should be a date (or null if can't convert)
                                    RuleFor(x => x.DateOfBirth)
                                        .NotNull().WithMessage(ValidationMessages.InvalidDateOfBirth);
                                });
                        });
                });

            RuleFor(x => x.Postcode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .MaximumLength(8).WithMessage(ValidationMessages.InvalidPostCode)
                .Matches(@"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s?[0-9][A-Za-z]{2})")
                .WithMessage(ValidationMessages.InvalidPostCode)
                .NotEmpty().WithMessage(ValidationMessages.MissingPostCode);
        }
    }
}