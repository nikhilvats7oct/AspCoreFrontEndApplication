using FinancialPortal.Web.Models.Interfaces;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IGtmService
    {
        void RaiseLoginSuccessfullEvent(IGtmEventRaisingVm vm, string userId);
        void RaiseLoginFailureEvent(IGtmEventRaisingVm vm, string reason);
        void RaiseRegistrationEvent_ClickedToRegister(IGtmEventRaisingVm vm, string error);
        void RaiseRegistrationEvent_AccountDetailsEntered(IGtmEventRaisingVm vm, string error);
        void RaiseRegistrationEvent_ActivationEmailSent(IGtmEventRaisingVm vm, string userId, string error);
        void RaiseRegistrationEvent_Complete(IGtmEventRaisingVm vm, string userId, string error);
        void RaisePasswordResetEvent_RequestSubmitted(IGtmEventRaisingVm vm, string userId);
        void RaisePasswordResetEvent_RequestVerified(IGtmEventRaisingVm vm, string userId);
        void RaisePasswordResetEvent_RequestComplete(IGtmEventRaisingVm vm, string userId);
        void RaiseMyAccountsPageViewedEvent(IGtmEventRaisingVm vm, string userId);
        void RaiseAccountDetailsViewedEvent(IGtmEventRaisingVm vm, string userId, string planType);
        void RaiseTransactionsViewedEvent(IGtmEventRaisingVm vm, string userId, string planType);
        void RaiseStatementDownloadedEvent(IGtmEventRaisingVm vm, string userId, string planType);
        void RaiseMyProfileViewedEvent(IGtmEventRaisingVm vm, string userId);
        void RaiseDPACheckEvent(IGtmEventRaisingVm vm, string userId, string error);
        void RaisePaymentEvent_PageVisited(PaymentOptionsVm vm, string userId, string planType);
        void RaiseOneOffPaymentEvent_OptionsSelected(OneOffPaymentReviewVm vm, string userId, string planType);
        void RaiseOneOffPaymentEvent_PaymentComplete(SuccessfulOneOffPaymentVm vm, string userId, string planType);
        void RaiseOneOffPaymentEvent_PaymentFailed(PaymentResultVm vm, string userId, string planType);
        void RaiseOneOffPaymentEvent_PaymentCancelled(PaymentResultVm vm, string userId, string planType);
        void RaiseDirectDebitEvent_OptionsSelected(DirectDebitDetailsVm vm, string userId, string planType);
        void RaiseDirectDebitEvent_BankDetails(DirectDebitPlanOverviewVm vm, string userId, string planType);
        void RaiseDirectDebitEvent_PlanSetUp(DirectDebitPlanOverviewVm vm, string userId, string planType);
        void RaiseAmendDirectDebitEvent_PageVisited(IGtmEventRaisingVm vm, string userId);
        void RaiseAmendDirectDebitEvent_DetailsSelected(AmendDirectDebitVm vm, string userId);
        void RaiseAmendDirectDebitEvent_Confirmed(AmendDirectDebitVm vm, string userId);
        void RaiseBudgetCalculatorStartedEvent(HouseholdStatusVm vm, string userId);
        void RaiseBudgetCalculatorHouseholdDetailsEvent(IGtmEventRaisingVm vm, string userId, string housingStatus, string employmentStatus);
        void RaiseBudgetCalculatorIncomeEvent(IGtmEventRaisingVm vm, string userId, string employmentStatus, string housingStatus, decimal income);
        void RaiseBudgetCalculatorExpenditureEvent(IGtmEventRaisingVm vm, string userId, string employmentStatus, string housingStatus, decimal income, decimal expenses, decimal disposableIncome);
        void RaiseBudgetCalculatorCompletedEvent(BudgetSummaryVm vm, string userId, string employmentStatus, string housingStatus);
        void RaiseBudgetCalculatorContinuedToPaymentEvent(BudgetSummaryVm vm, string userId, string employmentStatus, string housingStatus);
        void RaiseAgentRegistration_RequestVerifiedEvent(IGtmEventRaisingVm vm, string userId, string errorMessage);
        void RaiseAgentRegistration_RegistrationCompleteEvent(IGtmEventRaisingVm vm, string userId, string errorMessage);


        void RaiseContactPreferecesPageEvent(ContactPreferencesVm vm, string userId);
        void RaiseMobileUpdatePageEvent(ContactPreferencesVm vm, string userId);
        void RaiseContactPreferecesSMSChangeEvent(ContactPreferencesVm vm, string userId, bool newValue, bool oldValue);
        void RaiseContactPreferecesEmailChangeEvent(ContactPreferencesVm vm, string userId, bool newValue, bool oldValue);
        void RaiseMobilePhoneNumberChangeEvent(ContactPreferencesVm vm, string userId, string newValue, string oldValue);
    }
}
