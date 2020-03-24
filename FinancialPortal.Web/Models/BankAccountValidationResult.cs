namespace FinancialPortal.Web.Models
{
    public enum BankAccountValidationResult
    {
        BankDetailsValid = 0,
        AccountInvalid = 1,
        SortCodeInvalid = 2,
        ModulusCheckFailed = 3
    }
}
