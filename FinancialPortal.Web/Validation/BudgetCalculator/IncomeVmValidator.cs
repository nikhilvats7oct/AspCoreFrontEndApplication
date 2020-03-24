using FinancialPortal.Web.ViewModels;
using FluentValidation;
using FluentValidation.Validators;

namespace FinancialPortal.Web.Validation.BudgetCalculator
{
    public class IncomeVmValidator : AbstractValidator<IncomeVm>
    {
        public IncomeVmValidator()
        {
            RuleFor(x => x.Earning).SetValidator(new IncomeSourceValidator());
            RuleFor(x => x.BenefitsAndTaxCredits).SetValidator(new IncomeSourceValidator());
            RuleFor(x => x.Pension).SetValidator(new IncomeSourceValidator());
            RuleFor(x => x.Other).SetValidator(new IncomeSourceValidator());

            RuleFor(x => x).Custom(ErrorIfAllFieldsDoNotHaveIncome);
        }

        private void ErrorIfAllFieldsDoNotHaveIncome(IncomeVm model, CustomContext context)
        {
            if (model.BenefitsAndTaxCredits.Amount == 0.00M &&
                model.Earning.Amount == 0.00M &&
                model.Other.Amount == 0.00M &&
                model.Pension.Amount == 0.00M)
            {
                context.AddFailure("All income fields cannot be zero");
            }
        }
    }
}