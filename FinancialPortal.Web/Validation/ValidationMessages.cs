namespace FinancialPortal.Web.Validation
{
    public static class ValidationMessages
    {

       
        //Login
        public const string InvalidEmailAddress = "Please enter a valid email address";
        public const string NewEmailMatchesCurrent = "New email address matches your current email address";
        public const string MismatchEmailAddress = "Please ensure the emails match";
        public const string MissingEmailAddress = "Please enter your email address";
        public const string BadFormatPassword = "Make sure it’s more than 8 characters long, includes lower and uppercase letters and has at least 1 number. Also include a special character, like this, !, &, ?, $, *, ( etc";
        public const string MismatchPassword = "Please ensure the passwords match";
        public const string MissingPassword = "Please enter your password";
        public const string IncorrectPassword = "Please enter your password correctly";
        public static string InvalidLoginCredentials = "There’s a problem with your email address or password. Please check and try again. If you need any help  <a target=\"_blank\" href=\"TODO\">talk to us.</a>";
        public const string AccountNotActivated = "You need to activate your account, please check your email and spam folders.";
        public const string TechnicalError = "Our system is temporarily unavailable. Please try again later or contact us on 0333 556 5551. Our opening times are Mon – Fri 8am – 8pm and Sat 8.00am-2pm.";

        //DataProtection
        public const string MissingLowellRef = "Please enter your Lowell reference";
        public const string InvalidLowellRef = "Please enter a valid Lowell reference";
        public const string MissingDateOfBirth = "Please enter your date of birth, DD/MM/YYYY";
        public const string InvalidDateOfBirth = "Please enter a valid date, DD/MM/YYYY";
        public const string MissingPostCode = "Please enter your postcode";
        public const string InvalidPostCode = "Please enter a valid postcode";
        public const string DPAFailure = "The details provided do not match our records. Please try again";

        //Register
        public const string UserNameAlreadyExists = "This account is already set up for use on our website. Please try login in using your account details.";
        public const string ConfirmRegistrationFailed = "Unable to complete registration";

        //ForgotPassword
        public const string AccountNotActive = "Online Account has not been activated.  Please check your inbox and spam folders for validation email.";

        //public const string AmountEmpty = "Please enter a payment amount in pounds and pence.";
        public const string AmountLessThanOneGbp = "Amount must be £1.00 or greater";
        public const string AmountLessThanZero = "Amount cannot be negative.";
        public const string AmountGreaterThenAllowed = "You have entered a value greater than allowed. Please enter an amount less than or equal to £";
        public const string InvalidSourceOfFunds = "To complete payment please tell us where your funds have come from";
        public const string InvalidOtherSourceOfFundsCharacterCount = "Enter less than 50 characters";
        public const string SourceOfFundsOtherEmpty = "To complete payment please tell us where your funds have come from";
        public const string OneOffPaymentAmountSameAsDiscountTotalBalance = "The amount entered matches the discount available. If you wish to pay this amount, please click to accept the discount offer. If you do not wish to accept the discounted offer please enter a higher or lower amount.";
        public const string NotAcceptedTermsAndConditions = "Please accept the terms and conditions.";

        public const string NoFrequencySelected = "Please select a payment frequency";

        public const string NoSelectedStartDate = "Please select a start date";
        public const string SelectedStartDateTooLate = "Please select an earlier date";
        public const string SelectedStartDateTooEarly = "Please select a later date";


        //Bank Account Details
        public const string EmptyAccountHoldersName = "Please enter the account holders name";
        public const string EmptyAccountHoldersAccountNumber = "Please enter an account number";
        public const string EmptyAccountHoldersSortCode = "Please enter a sort code";
        public const string NotAcceptedDirectDebitGuarantee = "Please accept the Direct Debit Guarantee";
        
        public const string AccountHoldersNameContainsInvalidCharacter = "Account name can only contains alphanumeric characters, spaces, hyphens and apostrophes only";
        public const string AccountHoldersNameMoreThan40Characters = "Name must be less than 40 characters";

        public const string AccountNumberNot8Digits = "Account Number must be 8 digits long";

        public const string SortCodeNot6Digits = "Sort code must be 6 digits long";

        public const string InvalidAmount = "Please enter a payment amount in pounds and pence.";

        public const string InvalidDiscountBalance = "The outstanding discounted balance is invalid, please contact us on 0333 556 5551. Our opening times are Mon – Fri 8am – 8pm and Sat 8.00am-2pm.";

        public const string EmailAlreadyExists = "This email address is already in use on our website. Please try again.";
        public const string ConfirmDDDetails = "Please confirm your details";
        public const string PleaseSelectAnOption = "Please select an option";
        //Contact Us

        public static class ContactUs
        {
            public const string EmptyQueryArea = "Please make a selection";

            public const string EmptyFullName = "Please enter a name";

            public const string FullNameLongerThanAllowed = "Enter less than {0} characters";

            public const string FullNameHasInvalidCharacters = "Please enter valid name";

            public const string EmptyAccountHolderStatus = "Please make a selection";

            public const string EmptyThirdPartyPassword = "Please enter authorised 3rd party password";

            public const string ThirdPartyPasswordPresent = "Authorised Third Party password is not required.";

            public const string EmptyOrInvalidEmailAddress = "Please enter a valid email address";

            public const string EmptyOrInvalidAccountReference = "Please enter a valid reference";

            public const string EmptyOrInvalidFirstLineOfAddress = "Please enter an address";

            public const string EmptyOrInvalidPostcode = "Please enter a valid postcode";

            public const string EmptyOrInvalidDateOfBirth = "Please enter a valid date of birth.";

            public const string EmptyMessage = "Please enter a message.";
        }

        //Callback
        public static class Callback
        {
            public const string EmptyFullName = "Please enter your full name to continue.";

            public const string FullNameLongerThanAllowed = "Enter less than {0} characters";

            public const string FullNameHasInvalidCharacters = "Please enter valid name";

            public const string EmptyAccountHolderStatus = "Please make a selection to proceed";

            public const string EmptyThirdPartyPassword = "Please enter authorised 3rd party password";

            public const string ThirdPartyPasswordPresent = "Authorised Third Party password is not required.";

            public const string EmptyOrInvalidAccountReference = "Please enter a valid reference number";

            public const string InvalidPreferredTelephoneNumber = "Please enter a valid number";            

            public const string EmptyOrInvalidCallbackDate = "Please enter a callback date to continue";

            public const string EmptyTimeSlot = "Please select a timeslot to proceed";
        }

    }
}