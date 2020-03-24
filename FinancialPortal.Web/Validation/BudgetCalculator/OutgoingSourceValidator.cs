using System.Text.RegularExpressions;
using FinancialPortal.Web.ViewModels.Interfaces;
using FluentValidation;
using FluentValidation.Validators;

namespace FinancialPortal.Web.Validation.BudgetCalculator
{
    public class OutgoingSourceValidator : AbstractValidator<IOutgoingSourceVm>
    {
        public OutgoingSourceValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.AmountLessThanZero);

            RuleFor(x => x.ArrearsAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.AmountLessThanZero);

            RuleFor(x => x).Custom((model, context) =>
            {
                ValidateArrears(model, context);
            });
        }

        private void ValidateArrears(IOutgoingSourceVm model, CustomContext context)
        {
            if (model != null)
            {
                string[] nameArray = Regex.Split(model.GetType().Name, @"(?<!^)(?=[A-Z])");
                string name = string.Join(" ", nameArray).ToLower();

                // Rule 1: 
                // 'In arrears' is checked. 
                // Bill amount > 0
                // Arrears amount = 0
                if (model.InArrears && model.Amount > 0.00M)
                {
                    if (model.ArrearsAmount == 0.00M)
                    {
                        context.AddFailure($"You have checked in arrears, please add the arrears amount you are paying for your {name} ");
                    }
                }

                // Rule 2: 
                // 'In arrears' is checked. 
                // Bill amount = 0
                // Arrears amount > 0
                if (model.InArrears && model.Amount == 0.00M)
                {
                    if (model.ArrearsAmount > 0.00M)
                    {
                        context.AddFailure($"Arrears cannot be added without providing a payment amount for {name}");
                    }
                }

                // Rule 3: 
                // 'In arrears' is checked. 
                // Bill amount = 0
                // Arrears amount = 0
                if (model.InArrears && model.Amount == 0.00M)
                {
                    if (model.ArrearsAmount == 0.00M)
                    {
                        context.AddFailure($"You have checked in arrears but not payments for {name} and {name} arrears");
                    }
                }

                // Rule 4: 
                // 'In arrears' is NOT checked. 
                // Bill amount = 0
                // Arrears amount > 0
                if (!model.InArrears && model.Amount == 0.00M)
                {
                    if (model.ArrearsAmount > 0.00M)
                    {
                        context.AddFailure($"You have entered arrears amount and unchecked in arrears. Please check in arrears and enter payment amount for {name}");
                    }
                }

                // Rule 5: 
                // 'In arrears' is NOT checked. 
                // Bill amount > 0
                // Arrears amount > 0
                if (!model.InArrears && model.Amount > 0.00M)
                {
                    if (model.ArrearsAmount > 0.00M)
                    {
                        context.AddFailure($"You have entered arrears amount and unchecked in arrears. Please check in arrears.");
                    }
                }
            }
        }
    }
}