using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation.BudgetCalculator
{
    public class HouseholdStatusVmValidator : AbstractValidator<HouseholdStatusVm>
    {
        public HouseholdStatusVmValidator()
        {
            RuleFor(x => x.EmploymentStatus).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEqual("Please select")
                .WithMessage("Please select a circumstance that applies to you");

            RuleFor(x => x.HousingStatus).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEqual("Please select")
                .WithMessage("Please select a housing status that applies to you");

            RuleFor(x => x.AdultsInHousehold).Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(0)
                .WithMessage("Number of adults in the household must be 1 or greater");
        }
    }
    
}
