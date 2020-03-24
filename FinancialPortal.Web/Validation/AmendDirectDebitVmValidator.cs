using FinancialPortal.Web.ViewModels;
using FluentValidation;
using FluentValidation.Validators;

namespace FinancialPortal.Web.Validation
{
    public class AmendDirectDebitVmValidator : AbstractValidator<AmendDirectDebitVm>
    {
        public AmendDirectDebitVmValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

                RuleFor(x => x.DirectDebitAmount)
                    .GreaterThan(0).WithMessage(ValidationMessages.InvalidAmount)
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.DirectDebitAmount)
                            .GreaterThan(0).WithMessage(ValidationMessages.InvalidAmount);

                        RuleFor(x => x.DirectDebitAmount).SetValidator(new ScalePrecisionValidator(2, 10)).WithMessage(ValidationMessages.InvalidAmount);
                    });

                RuleFor(x => x.DirectDebitAmount)
                    .LessThanOrEqualTo(x => x.OutstandingBalance).WithMessage(x =>
                        ValidationMessages.AmountGreaterThenAllowed + $"{x.OutstandingBalance}");

                RuleFor(x => x.PlanFrequency)
                    .NotEmpty().WithMessage(ValidationMessages.NoFrequencySelected);

                RuleFor(x => x.PlanStartDate)
                    .NotEmpty().WithMessage(ValidationMessages.NoSelectedStartDate)
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.PlanStartDate)
                            .LessThanOrEqualTo(x => x.LatestStartDate).WithMessage(ValidationMessages.SelectedStartDateTooLate);

                        RuleFor(x => x.PlanStartDate)
                            .GreaterThanOrEqualTo(x => x.EarliestStartDate).WithMessage(ValidationMessages.SelectedStartDateTooEarly);
                    });

            RuleFor(x => x.SelectedPlanSetupOption).NotEqual(PlanSetupOptions.None).WithMessage(ValidationMessages.PleaseSelectAnOption);
            RuleFor(x => x.SelectedPlanSetupOption).NotNull().WithMessage(ValidationMessages.PleaseSelectAnOption);

        }
    }
}