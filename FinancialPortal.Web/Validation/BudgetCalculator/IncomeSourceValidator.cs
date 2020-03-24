using FinancialPortal.Web.ViewModels.Base;
using FluentValidation;

namespace FinancialPortal.Web.Validation.BudgetCalculator
{
    public class IncomeSourceValidator : AbstractValidator<IncomeSourceVm>
    {
        public IncomeSourceValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.AmountLessThanZero);
        }
    }
}