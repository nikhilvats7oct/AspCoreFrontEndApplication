using System;

namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public enum WebActionType
    {
        LoginSuccess = 1,
        LoginFail = 2,
        DPACheckSuccess = 3,
        DPACheckFail = 4,
        RegistrationRequest = 5,
        RegistrationActivated = 6,
        PartialPaymentSelectOptions = 7,
        PartialPaymentEnterPaymentDetails = 8,
        PartialPaymentCompleteSuccess = 9,
        PartialPaymentCompleteFailure = 10,
        PartialPaymentCompleteCancelled = 11,
        FullPaymentSelectOptions = 12,
        FullPaymentEnterPaymentDetails = 13,
        FullPaymentCompleteSuccess = 14,
        FullPaymentCompleteFailure = 15,
        FullPaymentCompleteCancelled = 16,
        DiscountedPaymentSelectOptions = 17,
        DiscountedPaymentEnterPaymentDetails = 18,
        DiscountedPaymentCompleteSuccess = 19,
        DiscountedPaymentCompleteFailure = 20,
        DiscountedPaymentCompleteCancelled = 21,
        DDPlanSelectOptions = 22,
        DDPlanEnterPaymentDetails = 23,
        DDPlanCompleteSuccess = 24,
        DDPlanCompleteFailure = 25,
        DDPlanCompleteCancelled = 26,
        DDAmendPlanSelectOptions = 27,
        DDAmendPlanEnterPaymentDetails = 28,
        DDAmendPlanCompleteSuccess = 29,
        DDAmendPlanCompleteFailure = 30,
        DDAmendPlanCompleteCancelled = 31,
        DiscountedDDPlanSelectOptions = 32,
        DiscountedDDPlanEnterPaymentDetails = 33,
        DiscountedDDPlanCompleteSuccess = 34,
        DiscountedDDPlanCompleteFailure = 35,
        DiscountedDDPlanCompleteCancelled = 36,
        BudgetCalculatorHouseholdDetails = 37,
        BudgetCalculatorIncome = 38,
        BudgetCalculatorBillsAndOutgoings1 = 39,
        BudgetCalculatorExpenditure = 40,
        BudgetCalculatorCompleted = 41,
        BudgetCalculatorReplayed = 42,
        MyAccountsPageViewed = 43,
        AccountDetailsViewed = 44,
        AllTransactionsViewed = 45,
        StatementDownloaded = 46,
        ProfileViewed = 47,

        PasswordResetRequest = 54,
        PasswordResetConfirmed = 55,
        PasswordChangeRequest = 56,
        PasswordChangeConfirmed = 57,


        PasswordChanged = 51,

        EmailAddressChangeRequest = 58,
        EmailAddressChangeConfirmed = 59,

        PasswordResetEmailRequested = 71,

        EmailAddressUpdateRequest = 74,
        EmailAddressUpdateConfirmed = 75,

        DirectDebitPaymentDIOptionSelected = 109,
        DirectDebitPaymentDIOptionDetailsEntered = 110,
        DirectDebitPaymentDIOptionCompleteSuccess = 111,
        DirectDebitPaymentDIOptionCompleteFailure = 112,
        DirectDebitPaymentDIOptionCompleteCancelled = 113,

        DirectDebitPaymentMyOfferOptionSelectedWithPositiveIandE = 114,
        DirectDebitPaymentMyOfferOptionDetailsEnteredWithPositiveIandE = 115,
        DirectDebitPaymentMyOfferOptionCompleteSuccessWithPositiveIandE = 116,
        DirectDebitPaymentMyOfferOptionCompleteFailureWithPositiveIandE = 117,
        DirectDebitPaymentMyOfferOptionCompleteCancelledWithPositiveIandE = 118,

        DirectDebitPaymentMyOfferOptionSelectedWithNegativeIandE = 119,
        DirectDebitPaymentMyOfferOptionDetailsEnteredWithNegativeIandE = 120,
        DirectDebitPaymentMyOfferOptionCompleteSuccessWithNegativeIandE = 121,
        DirectDebitPaymentMyOfferOptionCompleteFailureWithNegativeIandE = 122,
        DirectDebitPaymentMyOfferOptionCompleteCancelledWithNegativeIandE = 123,

        DirectDebitPaymentMyOfferOptionSelectedWithNoIandE = 124,
        DirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE = 125,
        DirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE = 126,
        DirectDebitPaymentMyOfferOptionCompleteFailureWithNoIandE = 127,
        DirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE = 128,

        DirectDebitPaymentASVOptionSelected = 129,
        DirectDebitPaymentASVOptionDetailsEntered = 130,
        DirectDebitPaymentASVOptionCompleteSuccess = 131,
        DirectDebitPaymentASVOptionCompleteFailure = 132,
        DirectDebitPaymentASVOptionCompleteCancelled = 133,

        AmendDirectDebitPaymentDIOptionSelected = 134,
        AmendDirectDebitPaymentDIOptionDetailsEntered = 135,
        AmendDirectDebitPaymentDIOptionCompleteSuccess = 136,
        AmendDirectDebitPaymentDIOptionCompleteFailure = 137,
        AmendDirectDebitPaymentDIOptionCompleteCancelled = 138,

        AmendDirectDebitPaymentMyOfferOptionSelectedWithPositiveIandE = 139,
        AmendDirectDebitPaymentMyOfferOptionDetailsEnteredWithPositiveIandE = 140,
        AmendDirectDebitPaymentMyOfferOptionCompleteSuccessWithPositiveIandE = 141,
        AmendDirectDebitPaymentMyOfferOptionCompleteFailureWithPositiveIandE = 142,
        AmendDirectDebitPaymentMyOfferOptionCompleteCancelledWithPositiveIandE = 143,

        AmendDirectDebitPaymentMyOfferOptionSelectedWithNegativeIandE = 144,
        AmendDirectDebitPaymentMyOfferOptionDetailsEnteredWithNegativeIandE = 145,
        AmendDirectDebitPaymentMyOfferOptionCompleteSuccessWithNegativeIandE = 146,
        AmendDirectDebitPaymentMyOfferOptionCompleteFailureWithNegativeIandE = 147,
        AmendDirectDebitPaymentMyOfferOptionCompleteCancelledWithNegativeIandE = 148,

        AmendDirectDebitPaymentMyOfferOptionSelectedWithNoIandE = 149,
        AmendDirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE = 150,
        AmendDirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE = 151,
        AmendDirectDebitPaymentMyOfferOptionCompleteFailureWithNoIandE = 152,
        AmendDirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE = 153,

        AmendDirectDebitPaymentASVOptionSelected = 154,
        AmendDirectDebitPaymentASVOptionDetailsEntered = 155,
        AmendDirectDebitPaymentASVOptionCompleteSuccess = 156,
        AmendDirectDebitPaymentASVOptionCompleteFailure = 157,
        AmendDirectDebitPaymentASVOptionCompleteCancelled = 158,

        DiscountDirectDebitPaymentDIOptionSelected = 159,
        DiscountDirectDebitPaymentDIOptionDetailsEntered = 160,
        DiscountDirectDebitPaymentDIOptionCompleteSuccess = 161,
        DiscountDirectDebitPaymentDIOptionCompleteFailure = 162,
        DiscountDirectDebitPaymentDIOptionCompleteCancelled = 163,

        DiscountDirectDebitPaymentMyOfferOptionSelectedWithPositiveIandE = 164,
        DiscountDirectDebitPaymentMyOfferOptionDetailsEnteredWithPositiveIandE = 165,
        DiscountDirectDebitPaymentMyOfferOptionCompleteSuccessWithPositiveIandE = 166,
        DiscountDirectDebitPaymentMyOfferOptionCompleteFailureWithPositiveIandE = 167,
        DiscountDirectDebitPaymentMyOfferOptionCompleteCancelledWithPositiveIandE = 168,

        DiscountDirectDebitPaymentMyOfferOptionSelectedWithNegativeIandE = 169,
        DiscountDirectDebitPaymentMyOfferOptionDetailsEnteredWithNegativeIandE = 170,
        DiscountDirectDebitPaymentMyOfferOptionCompleteSuccessWithNegativeIandE = 171,
        DiscountDirectDebitPaymentMyOfferOptionCompleteFailureWithNegativeIandE = 172,
        DiscountDirectDebitPaymentMyOfferOptionCompleteCancelledWithNegativeIandE = 173,

        DiscountDirectDebitPaymentMyOfferOptionSelectedWithNoIandE = 174,
        DiscountDirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE = 175,
        DiscountDirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE = 176,
        DiscountDirectDebitPaymentMyOfferOptionCompleteFailureWithNoIandE = 177,
        DiscountDirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE = 178,

        DiscountDirectDebitPaymentASVOptionSelected = 179,
        DiscountDirectDebitPaymentASVOptionDetailsEntered = 180,
        DiscountDirectDebitPaymentASVOptionCompleteSuccess = 181,
        DiscountDirectDebitPaymentASVOptionCompleteFailure = 182,
        DiscountDirectDebitPaymentASVOptionCompleteCancelled = 183,

        LFLContactPreferencesPageViewed = 321,
        LFLMobilePhoneUpdatePageViewed = 322,
        LFLSmsSelected = 323,
        LFLEmailSelected = 324,
        LFLMobilePhoneNumberUpdated = 325,
        LFLSmsDeselected = 326,
        LFLEmailDeselected = 327,

        OpenBankingAccessed = 267,
        OpenBankingComplete = 268,
        OpenBankingError = 269,
        ViewLFLLetter = 328,
        SaveLFLLetter = 329
    }

    public class WebActionDto
    {
        public string LowellReference { get; set; }
        public int Company { get; set; }
        public DateTime DateTime { get; set; }
        public WebActionType WebActionType { get; set; }
        public string Guid { get; set; }
    }
}
