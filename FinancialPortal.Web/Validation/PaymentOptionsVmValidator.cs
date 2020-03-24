using System;
using FinancialPortal.Web.ViewModels;
using FluentValidation;
using FluentValidation.Validators;

namespace FinancialPortal.Web.Validation
{
    public class PaymentOptionsVmValidator : AbstractValidator<PaymentOptionsVm>
    {
        public PaymentOptionsVmValidator()
        {
            string SelectedStartDate = nameof(PaymentOptionsVm.DirectDebitSelectedStartDate);
            string DiscountAmount = nameof(PaymentOptionsVm.DiscountAmount);
            
            When(x => x.DiscountAccepted == true, () =>
            {
                RuleFor(x => x.OutstandingBalance - x.DiscountAmount)
                .GreaterThan(0).OverridePropertyName(DiscountAmount).WithMessage(ValidationMessages.InvalidDiscountBalance);
            });

            RuleFor(x => x.SelectedPaymentOption)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Please select a payment type")
                .NotEqual(PaymentOptionsSelectionsVm.Values.PleaseSelect).WithMessage("Please select a payment type");

            //
            // Full Payment
            //
            When(x => x.SelectedPaymentOption == PaymentOptionsSelectionsVm.Values.FullPayment, () =>
            {
                RuleFor(x => x.FullPaymentSelectedSourceOfFunds)
                    .NotEqual("Please select").WithMessage(ValidationMessages.InvalidSourceOfFunds)
                    .NotEmpty().WithMessage(ValidationMessages.InvalidSourceOfFunds);

                RuleFor(x => x.FullPaymentSourceOfFundsOtherText)
                    .MaximumLength(50).WithMessage(ValidationMessages.InvalidOtherSourceOfFundsCharacterCount)
                    .NotEmpty().WithMessage(ValidationMessages.SourceOfFundsOtherEmpty)
                    .When(x => x.FullPaymentSelectedSourceOfFunds != null && x.FullPaymentSelectedSourceOfFunds.ToUpper() == "OTHER");
            });

            //
            // Partial Payment
            //
            When(x => x.SelectedPaymentOption == PaymentOptionsSelectionsVm.Values.PartialPayment, () =>
            {
                RuleFor(x => x.PartialPaymentAmount).Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty().WithMessage(ValidationMessages.InvalidAmount);

                RuleFor(x => x.PartialPaymentAmount).Cascade(CascadeMode.StopOnFirstFailure)
                    .GreaterThanOrEqualTo(1).WithMessage(ValidationMessages.AmountLessThanOneGbp)
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.PartialPaymentAmount).SetValidator(new ScalePrecisionValidator(2, 10))
                            .WithMessage(ValidationMessages.InvalidAmount);

                        RuleFor(x => x.PartialPaymentAmount)
                            .LessThanOrEqualTo(x => x.OutstandingBalance).WithMessage(x =>
                                ValidationMessages.AmountGreaterThenAllowed + $"{x.OutstandingBalance}")
                            .When(x => x.DiscountAccepted == false);

                        RuleFor(x => x.PartialPaymentAmount)
                            .LessThanOrEqualTo(x => x.DiscountedBalance).WithMessage(x =>
                                ValidationMessages.AmountGreaterThenAllowed + $"{x.DiscountedBalance}")
                            .When(x => x.DiscountAccepted);

                        RuleFor(x => x.PartialPaymentAmount)
                            .LessThanOrEqualTo(x => x.FullPaymentBalance).WithMessage(x =>
                            ValidationMessages.AmountGreaterThenAllowed + $"{x.FullPaymentBalance}");

                        RuleFor(x => x.PartialPaymentAmount)
                            .NotEqual(x => x.ProposedDiscountedBalanceIfAccepted)
                            .WithMessage(ValidationMessages.OneOffPaymentAmountSameAsDiscountTotalBalance)
                            .When(x => x.DiscountAccepted == false);
                    });

                RuleFor(x => x.PartialPaymentSelectedSourceOfFunds)
                    .NotEqual("Please select").WithMessage(ValidationMessages.InvalidSourceOfFunds)
                    .NotEmpty().WithMessage(ValidationMessages.InvalidSourceOfFunds);

                RuleFor(x => x.PartialPaymentSourceOfFundsOtherText)
                    .MaximumLength(50).WithMessage(ValidationMessages.InvalidOtherSourceOfFundsCharacterCount)
                    .NotEmpty().WithMessage(ValidationMessages.SourceOfFundsOtherEmpty)
                    .When(x => x.PartialPaymentSelectedSourceOfFunds != null && x.PartialPaymentSelectedSourceOfFunds.ToUpper() == "OTHER");
            });

            //
            // Direct Debit
            //
            When(x => x.SelectedPaymentOption == PaymentOptionsSelectionsVm.Values.DirectDebit, () =>
            {
                RuleFor(x => x.DirectDebitAmount)
                    .NotEmpty().WithMessage(ValidationMessages.InvalidAmount);

                RuleFor(x => x.DirectDebitAmount)
                    .GreaterThan(0).WithMessage(ValidationMessages.InvalidAmount)
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.DirectDebitAmount).SetValidator(new ScalePrecisionValidator(2, 10)).WithMessage(ValidationMessages.InvalidAmount);
                    });

                RuleFor(x => x.SelectedPlanSetupOption).NotEqual(PlanSetupOptions.None).WithMessage(ValidationMessages.PleaseSelectAnOption);
                RuleFor(x => x.SelectedPlanSetupOption).NotNull().WithMessage(ValidationMessages.PleaseSelectAnOption);

                RuleFor(x => x.DirectDebitAmount)
                    .LessThanOrEqualTo(x => x.OutstandingBalance).WithMessage(x =>
                        ValidationMessages.AmountGreaterThenAllowed + $"{x.OutstandingBalance}")
                    .When(x => x.DiscountAccepted == false);

                RuleFor(x => x.DirectDebitSelectedFrequency)
                    .NotEmpty().WithMessage(ValidationMessages.NoFrequencySelected);

                RuleFor(x => x.DirectDebitSelectedStartDate)
                    .NotEmpty().WithMessage(ValidationMessages.NoSelectedStartDate)
                    .DependentRules(() =>
                    {
                        RuleFor(x => DateTime.Parse(x.DirectDebitSelectedStartDate))
                            .LessThanOrEqualTo(x => x.DirectDebitStartDateLatest).OverridePropertyName(SelectedStartDate).WithMessage(ValidationMessages.SelectedStartDateTooLate);

                        RuleFor(x => DateTime.Parse(x.DirectDebitSelectedStartDate))
                            .GreaterThanOrEqualTo(x => x.DirectDebitStartDateEarliest).OverridePropertyName(SelectedStartDate).WithMessage(ValidationMessages.SelectedStartDateTooEarly);
                    });

                When(x => x.DirectDebitIsEmailAddressFieldVisible, () =>
                {
                    RuleFor(x => x.DirectDebitEmailAddress)
                        .NotEmpty().WithMessage(ValidationMessages.InvalidEmailAddress)
                        .EmailAddress().WithMessage(ValidationMessages.InvalidEmailAddress)
                        .Matches(@"^((?!lowell).)*$").WithMessage(ValidationMessages.InvalidEmailAddress);
                });
            });

            When(x => x.IsAcceptTermsAndConditionsFieldVisible, () =>
            {
                RuleFor(x => x.AcceptTermsAndConditions)
                    .Equal(true).WithMessage(ValidationMessages.NotAcceptedTermsAndConditions);
            });
        }

    }
}
