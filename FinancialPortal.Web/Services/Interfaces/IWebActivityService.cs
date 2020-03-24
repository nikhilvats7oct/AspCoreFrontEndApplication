using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IWebActivityService
    {
        Task LogLoginSuccess(string lowellRef, string userId);
        Task LogLoginFail(string lowellRef, string userId);
        Task LogDPACheckSuccess(string lowellRef, string userId);
        Task LogDPACheckFail(string lowellRef, string userId);
        Task LogRegistrationRequest(string lowellRef, string userId);
        Task LogRegistrationActivated(string lowellRef, string userId);
        Task LogOneOffPaymentSelected(string lowellRef, string userId, bool partial, bool discounted);
        Task LogOneOffPaymentDetailsEntered(string lowellRef, string userId, bool partial, bool discounted);
        Task LogOneOffPaymentComplete(string lowellRef, string userId, bool partial, bool discounted);
        Task LogOneOffPaymentFailure(string lowellRef, string userId, bool partial, bool discounted);
        Task LogOneOffPaymentCancelled(string lowellRef, string userId, bool partial, bool discounted);
        Task LogSetUpDDPlanSelected(string lowellRef, string userId, bool discounted);
        Task LogSetUpDDPlanDetailsEntered(string lowellRef, string userId, bool discounted);
        Task LogSetUpDDPlanComplete(string lowellRef, string userId, bool discounted);
        Task LogSetUpDDPlanFailure(string lowellRef, string userId, bool discounted);
        Task LogSetUpDDPlanCancelled(string lowellRef, string userId, bool discounted);
        Task LogAmendDDPlanSelected(string lowellRef, string userId);
        Task LogAmendDDPlanDetailsEntered(string lowellRef, string userId);
        Task LogAmendDDPlanComplete(string lowellRef, string userId);
        Task LogAmendDDPlanFailure(string lowellRef, string userId);
        Task LogAmendDDPlanCancelled(string lowellRef, string userId);
        Task LogBudgetCalculatorHouseholdDetails(string lowellRef, string userId);
        Task LogBudgetCalculatorIncome(string lowellRef, string userId);
        Task LogBudgetCalculatorBillsAndOutgoings1(string lowellRef, string userId);
        Task LogBudgetCalculatorExpenditure(string lowellRef, string userId);
        Task LogBudgetCalculatorCompleted(string lowellRef, string userId);
        Task LogBudgetCalculatorReplayed(string lowellRef, string userId);
        Task LogMyAccountsPageViewed(string lowellRef, string userId);
        Task LogAccountDetailsViewed(string lowellRef, string userId);
        Task LogAllTransactionsViewed(string lowellRef, string userId);
        Task LogStatementDownloaded(string lowellRef, string userId);
        Task LogMyProfileViewed(string lowellRef, string userId);
        Task LogPasswordChanged(string lowellRef, string userId);
        Task LogPasswordResetRequest(string lowellRef, string userId);
        Task LogPasswordResetConfirmed(string lowellRef, string userId);
        Task LogEmailAddressChangeRequest(string lowellRef, string userId);
        Task LogEmailAddressChangeConfirmed(string lowellRef, string userId);
        Task LogPasswordResetEmailRequested(string lowellRef, string userId);
        Task LogPasswordChangeRequest(string lowellRef, string userId);
        Task LogPasswordChangeConfirmed(string lowellRef, string userId);
        Task LogEmailAddressUpdateRequest(string lowellRef, string userId);
        Task LogEmailAddressUpdateConfirmed(string lowellRef, string userId);

        Task LogDirectDebitPaymentDIOptionSelected(string lowellRef, string userId);
        Task LogDirectDebitPaymentDIOptionDetailsEntered(string lowellRef, string userId);
        Task LogDirectDebitPaymentDIOptionCompleteSuccess(string lowellRef, string userId);
        Task LogDirectDebitPaymentDIOptionCompleteFailed(string lowellRef, string userId);
        Task LogDirectDebitPaymentDIOptionCompleteCancelled(string lowellRef, string userId);

        Task LogDirectDebitMyOfferOptionSelectedPositiveIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionDetailsEnteredPositiveIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteSuccessPositiveIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteFailedPositiveIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteCancelledPositiveIandE(string lowellRef, string userId);

        Task LogDirectDebitMyOfferOptionSelectedNegativeIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionDetailsEnteredNegativeIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteSuccessNegativeIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteFailedNegativeIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteCancelledNegativeIandE(string lowellRef, string userId);


        Task LogDirectDebitMyOfferOptionSelectedWithNoIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteFailedWithNoIandE(string lowellRef, string userId);
        Task LogDirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE(string lowellRef, string userId);

        Task LogDirectDebitASVOptionSelected(string lowellRef, string userId);
        Task LogDirectDebitPaymentASVOptionDetailsEntered(string lowellRef, string userId);
        Task LogDirectDebitPaymentASVOptionCompleteSuccess(string lowellRef, string userId);
        Task LogDirectDebitPaymentASVOptionCompleteFailed(string lowellRef, string userId);
        Task LogDirectDebitPaymentASVOptionCompleteCancelled(string lowellRef, string userId);

        Task LogAmendDirectDebitDIOptionSelected(string lowellRef, string userId);
        Task LogAmendDirectDebitDIOptionDetailsEntered(string lowellRef, string userId);
        Task LogAmendDirectDebitDIOptionCompleteSuccess(string lowellRef, string userId);
        Task LogAmendDirectDebitDIOptionCompleteFailed(string lowellRef, string userId);
        Task LogAmendDirectDebitDIOptionCompleteCancelled(string lowellRef, string userId);

        Task LogAmendDirectDebitMyOfferOptionSelectedWithPositiveIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionDetailsEnteredWithPositiveIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteSuccessWithPositiveIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteFailedWithPositiveIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteCancelledWithPositiveIandE(string lowellRef, string userId);

        Task LogAmendDirectDebitMyOfferOptionSelectedWithNegativeIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionDetailsEnteredWithNegativeIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteSuccessWithNegativeIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteFailedWithNegativeIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteCancelledWithNegativeIandE(string lowellRef, string userId);

        Task LogAmendDirectDebitMyOfferOptionSelectedWithNoIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionDetailsEnteredWithNoIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteSuccessWithNoIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteFailedWithNoIandE(string lowellRef, string userId);
        Task LogAmendDirectDebitMyOfferOptionCompleteCancelledWithNoIandE(string lowellRef, string userId);

        Task LogAmendDirectDebitASVOptionSelected(string lowellRef, string userId);
        Task LogAmendDirectDebitPaymentASVOptionDetailsEntered(string lowellRef, string userId);
        Task LogAmendDirectDebitPaymentASVOptionCompleteSuccess(string lowellRef, string userId);
        Task LogAmendDirectDebitPaymentASVOptionCompleteFailed(string lowellRef, string userId);
        Task LogAmendDirectDebitPaymentASVOptionCompleteCancelled(string lowellRef, string userId);

        Task LogDiscountDirectDebitDIOptionSelected(string lowellRef, string userId);
        Task LogDiscountDirectDebitDIOptionDetailsEntered(string lowellRef, string userId);
        Task LogDiscountDirectDebitDIOptionCompleteSuccess(string lowellRef, string userId);
        Task LogDiscountDirectDebitDIOptionCompleteFailed(string lowellRef, string userId);
        Task LogDiscountDirectDebitDIOptionCompleteCancelled(string lowellRef, string userId);

        Task LogDiscountDirectDebitMyOfferOptionSelectedWithPositiveIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithPositiveIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithPositiveIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteFailedWithPositiveIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithPositiveIandE(string lowellRef, string userId);

        Task LogDiscountDirectDebitMyOfferOptionSelectedWithNegativeIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithNegativeIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithNegativeIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteFailedWithNegativeIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithNegativeIandE(string lowellRef, string userId);

        Task LogDiscountDirectDebitMyOfferOptionSelectedWithNoIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithNoIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithNoIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteFailedWithNoIandE(string lowellRef, string userId);
        Task LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithNoIandE(string lowellRef, string userId);

        Task LogDiscountDirectDebitASVOptionSelected(string lowellRef, string userId);
        Task LogDiscountDirectDebitPaymentASVOptionDetailsEntered(string lowellRef, string userId);
        Task LogDiscountDirectDebitPaymentASVOptionCompleteSuccess(string lowellRef, string userId);
        Task LogDiscountDirectDebitPaymentASVOptionCompleteFailed(string lowellRef, string userId);
        Task LogDiscountDirectDebitPaymentASVOptionCompleteCancelled(string lowellRef, string userId);

        Task LogContactPreferencesPageVist(string lowellReference, string userId);
        Task LogMobilePhoneUpdatePageVist(string lowellReference, string userId);
        Task LogContactPreferencesSMSChange(string lowellReference, string userId, bool newValue, bool oldValue);
        Task LogContactPreferecesEmailChange(string lowellReference, string userId, bool newValue, bool oldValue);
        Task LogMobilePhoneNumberUpdateChange(string lowellReference, string userId, string newValue, string oldValue);

        Task LogOpenBankingAccessed(string lowellRef, string userId);
        Task LogOpenBankingComplete(string lowellRef, string userId);
        Task LogOpenBankingError(string lowellRef, string userId);

        Task LogViewLFLLetter(string lowellReference, string userId);
        Task LogSaveLFLLetter(string lowellReference, string userId);
    }
}
