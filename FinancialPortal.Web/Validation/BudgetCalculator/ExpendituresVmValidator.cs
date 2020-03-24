using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation.BudgetCalculator
{
    public class ExpendituresVmValidator : AbstractValidator<ExpendituresVm>
    {
        public ExpendituresVmValidator()
        {
            RuleFor(x => x.FoodAndHouseKeeping).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.PersonalCosts).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.CommunicationsAndLeisure).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.TravelAndTransport).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.CareAndHealthCosts).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.PensionsAndInsurance).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.SchoolCosts).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.Professional).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.Savings).SetValidator(new ExpenditureSourceValidator());
            RuleFor(x => x.Other).SetValidator(new ExpenditureSourceValidator());
        }
    }
}