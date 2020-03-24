using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation.BudgetCalculator
{
    public class BillsAndOutgoingsVmValidator : AbstractValidator<BillsAndOutgoingsVm>
    {
        public BillsAndOutgoingsVmValidator()
        {
            RuleFor(x => x.Mortgage).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.Rent).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.SecuredLoan).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.CouncilTax).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.ApplianceOrFurnitureRental).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.TvLicense).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.Gas).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.OtherFuel).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.Electric).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.Water).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.ChildMaintenance).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.CourtFines).SetValidator(new OutgoingSourceValidator());
            RuleFor(x => x.Ccjs).SetValidator(new OutgoingSourceValidator());
        }
    }
}