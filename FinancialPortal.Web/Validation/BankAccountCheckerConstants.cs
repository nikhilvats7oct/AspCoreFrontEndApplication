namespace FinancialPortal.Web.Validation
{
    public static class BankAccountCheckerConstants
    {
        public const string AccountInvalid = "Please enter a valid account number. If your account number has fewer than 8 digits please add zeroes in front of the account number";
        public static string SortCodeInvalid = "Some of your sort code wasn’t recognised. Please try again or <a target=\"_blank\" href=\"todo\">talk to us.</a>";
        public const string ModulusCheckFailed = "Please check your bank account and sort code are correct - if you believe they are valid then contact us so we can find a resolution.";
    }
}
