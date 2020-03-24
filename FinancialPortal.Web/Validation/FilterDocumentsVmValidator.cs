using FinancialPortal.Models.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class FilterDocumentsVmValidator : AbstractValidator<FilterDocumentsVm>
    {
        public FilterDocumentsVmValidator()
        {
            RuleFor(x => x).Custom((filterTransactions, context) =>
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
