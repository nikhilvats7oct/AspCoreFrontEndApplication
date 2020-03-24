using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class TransactionsVmValidator : AbstractValidator<TransactionsVm>
    {
        public TransactionsVmValidator()
        {
            RuleFor(x => x.FilterTransactions).Custom((filterTransactions, context) =>
            {
                if (filterTransactions.DateFrom.HasValue && filterTransactions.DateTo.HasValue)
                {
                    if (filterTransactions.DateFrom > filterTransactions.DateTo)
                    {
                        context.AddFailure("Date from should not be greater than Date to.");
                    }
                }
            });
        }
    }
}
