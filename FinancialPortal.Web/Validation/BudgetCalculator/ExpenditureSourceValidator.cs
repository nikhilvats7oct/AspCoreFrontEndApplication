using FinancialPortal.Web.ViewModels.Interfaces;
using FluentValidation;

namespace FinancialPortal.Web.Validation.BudgetCalculator
{
    public class ExpenditureSourceValidator : AbstractValidator<IExpenditureSource>
    {
        public ExpenditureSourceValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.AmountLessThanZero);
        }
    }
}