using FinancialPortal.Web.ViewModels;
using FluentValidation;

namespace FinancialPortal.Web.Validation
{
    public class DirectDebitDetailsVmValidator : AbstractValidator<DirectDebitDetailsVm>
    {
        public DirectDebitDetailsVmValidator()
        {

            RuleFor(x => x.AccountHoldersName)
                .Matches(@"^[a-zA-Z0-9\s-']*$").WithMessage(ValidationMessages.AccountHoldersNameContainsInvalidCharacter);

            RuleFor(x => x.AccountHoldersName)
                .NotEmpty().WithMessage(ValidationMessages.EmptyAccountHoldersName);

            RuleFor(x => x.AccountHoldersName)
                .MaximumLength(40).WithMessage(ValidationMessages.AccountHoldersNameMoreThan40Characters);

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage(ValidationMessages.EmptyAccountHoldersAccountNumber)
                .Matches(@"^[0-9]*$").WithMessage(BankAccountCheckerConstants.AccountInvalid);

            RuleFor(x => x.AccountNumber)
                .MaximumLength(8).WithMessage(ValidationMessages.AccountNumberNot8Digits)
                .MinimumLength(8).WithMessage(ValidationMessages.AccountNumberNot8Digits);

            RuleFor(x => x.SortCode)
                .NotEmpty().WithMessage(ValidationMessages.EmptyAccountHoldersSortCode)
                .Matches(@"^[0-9]*$").WithMessage(BankAccountCheckerConstants.SortCodeInvalid);

            RuleFor(x => x.SortCode)
                .MaximumLength(6).WithMessage(ValidationMessages.SortCodeNot6Digits)
                .MinimumLength(6).WithMessage(ValidationMessages.SortCodeNot6Digits);

            RuleFor(x => x.AcceptDirectDebitGuarantee)
                .Equal(true).WithMessage(ValidationMessages.NotAcceptedDirectDebitGuarantee);
        }
    }
}
