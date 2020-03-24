using System;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services
{
    public class GtmService : IGtmService
    {
        private static class GtmEvents
        {
            public static string Registration { get { return "Registration"; } }
            public static string AgentRegistration { get { return "AgentRegistration"; } }
            public static string Login { get { return "Login"; } }
            public static string PasswordReset { get { return "PasswordReset"; } }
            public static string DPACheck { get { return "DPACheck"; } }
            public static string AccountActivity { get { return "AccountActivity"; } }
            public static string PaymentEvent { get { return "PaymentEvent"; } }
            public static string BudgetCalculator { get { return "BudgetCalculator"; } }
            public static string ContactPreference { get { return "Contact preference"; } }
        }
        private static class AccountActivities
        {
            public static string MyAccountsPageViewed { get { return "My Accounts page viewed"; } }
            public static string MyProfileViewed { get { return "Profile viewed"; } }
            public static string AccountDetailsViewed { get { return "Account detail viewed"; } }
            public static string TransactionsViewed { get { return "View all transactions"; } }
            public static string StatementDownloaded { get { return "Statement downloaded"; } }
        }
        private static class PaymentSteps
        {
            public static string Step1VisitMakeAPaymentPage { get { return "1 - Visited Make a Payment Page"; } }
            public static string Step1ClickedToChangedPlan { get { return "1 - Clicked to Change Plan"; } }
            public static string Step2PaymentOptionsSelected { get { return "2 - Payment Options Selected"; } }
            public static string Step2NewPaymentDetailsSelected { get { return "2 - New Payment Details Selected"; } }
            public static string Step3PaymentDetailsReviewed { get { return "3 - Payment Details Reviewed"; } }
            public static string Step3BankDetailsEntered { get { return "3 - Bank Details Entered"; } }
            public static string step3ChangesConfirmed { get { return "3 - Changes confirmed (New Plan Set)"; } }
            public static string step4PaymentComplete { get { return "4 - Payment Complete"; } }
            public static string step4PaymentFailed { get { return "4 - Payment Failed"; } }
            public static string step4PaymentCancelled { get { return "4 - Payment Cancelled"; } }
            public static string step4PlanSetUp { get { return "4 - Plan Set Up"; } }
        }
        private static class BudgetCalculatorSteps
        {
            public static string Step1Started { get { return "1 - Started Budget Calculator"; } }
            public static string Step2HouseholdDetailsSubmitted { get { return "2 - Submitted Household Details"; } }
            public static string Step3IncomeSubmitted { get { return "3 - Submitted Income"; } }
            public static string Step4ExpenditureSubmitted { get { return "4 - Submitted Expenditure"; } }
            public static string Step5Completed { get { return "5 - Completed - Viewed Budget Summary"; } }
            public static string Step6ContinuedToPayment { get { return "6 - Continued to Make a Payment"; } }
        }
        private static class RegistrationSteps
        {
            public static string Step1ClickedToRegister { get { return "1 – Clicked to Register"; } }
            public static string Step2AccountDetailsEntered { get { return "2 - Account Details Entered"; } }
            public static string Step3ActivationEmailSent { get { return "3 - Activation Email Sent"; } }
            public static string Step4RegistrationComplete { get { return "4 - Registration Complete"; } }
        }
        private static class PasswordResetSteps
        {
            public static string Step1RequestSubmitted { get { return "1 - Request Submitted"; } }
            public static string Step2RequestVerified { get { return "2 - Request Verified"; } }
            public static string Step3RequestCompleted { get { return "3 - Request Completed"; } }
        }
        private static class AgentRegistrationSteps
        {
            public static string Step1RequestVerified { get { return "1-Request Verified"; } }
            public static string Step2RegistrationComplete { get { return "2-Registration Complete"; } }
        }

        private string ConvertPlanSetupOption(PlanSetupOptions? planSetupOption)
        {
            switch (planSetupOption)
            {
                case PlanSetupOptions.AverageSetupValue: return "ASV";
                case PlanSetupOptions.DisposableIncome: return "DI";
                case PlanSetupOptions.OtherPaymentOffer: return "MyPaymentOffer";
                default: return "";
            }
        }
        public void RaiseAccountDetailsViewedEvent(IGtmEventRaisingVm vm, string userId, string planType)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.AccountActivity,
                guid = userId,
                action_taken = AccountActivities.AccountDetailsViewed,
                plan_type = planType
            });
        }

        public void RaiseAmendDirectDebitEvent_Confirmed(AmendDirectDebitVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.step3ChangesConfirmed,
                payment_type = "Amended Direct Debit",
                payment_amount = vm.DirectDebitAmount,
                payment_detail = vm.PlanFrequency,
                discount_available = "No discount available",
                balance_selected = "Full Balance",
                plan_type = "Regular Account",
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                payment_option_chosen = ConvertPlanSetupOption(vm.SelectedPlanSetupOption)
            });
        }

        public void RaiseAmendDirectDebitEvent_DetailsSelected(AmendDirectDebitVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.Step2NewPaymentDetailsSelected,
                payment_type = "Amended Direct Debit",
                payment_amount = vm.DirectDebitAmount,
                payment_detail = vm.PlanFrequency,
                discount_available = "No discount available",
                balance_selected = "Full Balance",
                plan_type = "Regular Account",
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                payment_option_chosen = ConvertPlanSetupOption(vm.SelectedPlanSetupOption)
            });
        }

        public void RaiseAmendDirectDebitEvent_PageVisited(IGtmEventRaisingVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.Step1ClickedToChangedPlan,
                payment_type = "Amended Direct Debit",
                discount_available = "No discount available",
                plan_type = "Regular Account",
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                payment_option_chosen = ""
            });
        }

        public void RaiseDirectDebitEvent_BankDetails(DirectDebitPlanOverviewVm vm, string userId, string planType)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.Step3BankDetailsEntered,
                payment_type = "Direct Debit Plan",
                payment_amount = vm.PaymentAmount,
                payment_detail = vm.PaymentFrequency,
                discount_available = vm.DiscountAvailable ? "Discount available" : "No discount available",
                balance_selected = vm.DiscountAccepted ? "Discounted Balance" : "Full Balance",
                plan_type = planType,
                plan_status = "New Plan",
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                source_of_funds = null,
                payment_option_chosen = ConvertPlanSetupOption(vm.SelectedPlanSetupOption)
            });
        }

        public void RaiseDirectDebitEvent_OptionsSelected(DirectDebitDetailsVm vm, string userId, string planType)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.Step2PaymentOptionsSelected,
                payment_type = "Direct Debit Plan",
                payment_amount = vm.PaymentAmount,
                payment_detail = vm.PaymentFrequency,
                discount_available = vm.DiscountAvailable ? "Discount available" : "No discount available",
                balance_selected = vm.DiscountSelected ? "Discounted Balance" : "Full Balance",
                plan_type = planType,
                plan_status = "New Plan",
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                source_of_funds = null,
                payment_option_chosen = ConvertPlanSetupOption(vm.SelectedPlanSetupOption)
            });
        }

        public void RaiseDirectDebitEvent_PlanSetUp(DirectDebitPlanOverviewVm vm, string userId, string planType)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.step4PlanSetUp,
                payment_type = "Direct Debit Plan",
                payment_amount = vm.PaymentAmount,
                payment_detail = vm.PaymentFrequency,
                discount_available = vm.DiscountAvailable ? "Discount available" : "No discount available",
                balance_selected = vm.DiscountAccepted ? "Discounted Balance" : "Full Balance",
                plan_type = planType,
                plan_status = "New Plan",
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                source_of_funds = null,
                payment_option_chosen = ConvertPlanSetupOption(vm.SelectedPlanSetupOption)
            });
        }

        public void RaiseDPACheckEvent(IGtmEventRaisingVm vm, string userId, string error)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.DPACheck,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                result = String.IsNullOrEmpty(error) ? "Success" : "Failure",
                error_message = String.IsNullOrEmpty(error) ? null : error
            });
        }

        public void RaiseLoginFailureEvent(IGtmEventRaisingVm vm, string reason)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.Login,
                error_message = reason,
                result = "Failure"
            });
        }

        public void RaiseLoginSuccessfullEvent(IGtmEventRaisingVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.Login,
                guid = userId,
                result = "Success",
            });
        }

        public void RaiseMyAccountsPageViewedEvent(IGtmEventRaisingVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.AccountActivity,
                guid = userId,
                action_taken = AccountActivities.MyAccountsPageViewed,
            });
        }

        public void RaiseMyProfileViewedEvent(IGtmEventRaisingVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.AccountActivity,
                guid = userId,
                action_taken = AccountActivities.MyProfileViewed,
            });
        }

        public void RaiseOneOffPaymentEvent_OptionsSelected(OneOffPaymentReviewVm vm, string userId, string planType)
        {
            string plan_status = vm.PlanInPlace ? "Payment against Plan" : "No Plan in Place";
            plan_status = vm.InArrears ? "Plan Arrears Payment" : plan_status;

            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.Step2PaymentOptionsSelected,
                payment_type = "One Off Payment",
                payment_amount = vm.PaymentAmount,
                payment_detail = vm.PaidInFull ? "Full Balance" : "Partial Payment",
                discount_available = vm.DiscountAvailable ? "Discount available" : "No discount available",
                plan_type = planType,
                balance_selected = vm.DiscountSelected ? "Discounted Balance" : "Full Balance",
                plan_status = plan_status,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                source_of_funds = vm.SourceOfFunds
            });
        }

        public void RaiseOneOffPaymentEvent_PaymentCancelled(PaymentResultVm vm, string userId, string planType)
        {
            string plan_status = vm.PaymentInfo.PlanInPlace ? "Payment against Plan" : "No Plan in Place";
            plan_status = vm.PaymentInfo.InArrears ? "Plan Arrears Payment" : plan_status;

            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.step4PaymentCancelled,
                payment_type = "One Off Payment",
                payment_amount = vm.PaymentInfo.PaymentAmount,
                payment_detail = vm.PaymentInfo.PaidInFull ? "Full Balance" : "Partial Payment",
                discount_available = vm.PaymentInfo.DiscountAvailable ? "Discount available" : "No discount available",
                plan_type = planType,
                balance_selected = vm.PaymentInfo.DiscountSelected ? "Discounted Balance" : "Full Balance",
                plan_status = plan_status,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                source_of_funds = vm.PaymentInfo.SourceOfFunds
            });
        }

        public void RaiseOneOffPaymentEvent_PaymentComplete(SuccessfulOneOffPaymentVm vm, string userId, string planType)
        {
            string plan_status = vm.PaymentInfo.PlanInPlace ? "Payment against Plan" : "No Plan in Place";
            plan_status = vm.PaymentInfo.InArrears ? "Plan Arrears Payment" : plan_status;

            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.step4PaymentComplete,
                payment_type = "One Off Payment",
                payment_amount = vm.PaymentInfo.PaymentAmount,
                payment_detail = vm.PaymentInfo.PaidInFull ? "Full Balance" : "Partial Payment",
                discount_available = vm.PaymentInfo.DiscountAvailable ? "Discount available" : "No discount available",
                plan_type = planType,
                balance_selected = vm.PaymentInfo.DiscountSelected ? "Discounted Balance" : "Full Balance",
                plan_status = plan_status,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                source_of_funds = vm.PaymentInfo.SourceOfFunds
            });
        }

        public void RaiseOneOffPaymentEvent_PaymentFailed(PaymentResultVm vm, string userId, string planType)
        {
            string plan_status = vm.PaymentInfo.PlanInPlace ? "Payment against Plan" : "No Plan in Place";
            plan_status = vm.PaymentInfo.InArrears ? "Plan Arrears Payment" : plan_status;

            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.step4PaymentFailed,
                payment_type = "One Off Payment",
                payment_amount = vm.PaymentInfo.PaymentAmount,
                payment_detail = vm.PaymentInfo.PaidInFull ? "Full Balance" : "Partial Payment",
                discount_available = vm.PaymentInfo.DiscountAvailable ? "Discount available" : "No discount available",
                plan_type = planType,
                balance_selected = vm.PaymentInfo.DiscountSelected ? "Discounted Balance" : "Full Balance",
                plan_status = plan_status,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                source_of_funds = vm.PaymentInfo.SourceOfFunds
            });
        }

        public void RaisePasswordResetEvent_RequestComplete(IGtmEventRaisingVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PasswordReset,
                step = PasswordResetSteps.Step3RequestCompleted,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
            });
        }

        public void RaisePasswordResetEvent_RequestSubmitted(IGtmEventRaisingVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PasswordReset,
                step = PasswordResetSteps.Step1RequestSubmitted,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
            });
        }

        public void RaisePasswordResetEvent_RequestVerified(IGtmEventRaisingVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PasswordReset,
                step = PasswordResetSteps.Step2RequestVerified,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
            });
        }

        public void RaisePaymentEvent_PageVisited(PaymentOptionsVm vm, string userId, string planType)
        {
            string plan_status = vm.PlanInPlace ? "Payment against Plan" : "No Plan in Place";
            plan_status = !String.IsNullOrEmpty(vm.ArrearsMessage) ? "Plan Arrears Payment" : plan_status;

            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.PaymentEvent,
                step = PaymentSteps.Step1VisitMakeAPaymentPage,
                discount_available = vm.DiscountBalanceAvailable ? "Discount available" : "No discount available",
                plan_type = planType,
                plan_status = plan_status,
                guid = String.IsNullOrEmpty(userId) ? null : userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In"
            });
        }

        public void RaiseRegistrationEvent_AccountDetailsEntered(IGtmEventRaisingVm vm, string error)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.Registration,
                step = RegistrationSteps.Step2AccountDetailsEntered,
                result = String.IsNullOrEmpty(error) ? "Success" : "Failure",
                error_message = String.IsNullOrEmpty(error) ? null : error
            });
        }

        public void RaiseRegistrationEvent_ActivationEmailSent(IGtmEventRaisingVm vm, string userId, string error)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.Registration,
                step = RegistrationSteps.Step3ActivationEmailSent,
                result = String.IsNullOrEmpty(error) ? "Success" : "Failure",
                error_message = String.IsNullOrEmpty(error) ? null : error,
                guid = userId
            });
        }

        public void RaiseRegistrationEvent_ClickedToRegister(IGtmEventRaisingVm vm, string error)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.Registration,
                step = RegistrationSteps.Step1ClickedToRegister,
                result = String.IsNullOrEmpty(error) ? "Success" : "Failure",
                error_message = String.IsNullOrEmpty(error) ? null : error
            });
        }

        public void RaiseRegistrationEvent_Complete(IGtmEventRaisingVm vm, string userId, string error)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.Registration,
                step = RegistrationSteps.Step4RegistrationComplete,
                result = String.IsNullOrEmpty(error) ? "Success" : "Failure",
                error_message = String.IsNullOrEmpty(error) ? null : error,
                guid = userId
            });
        }

        public void RaiseStatementDownloadedEvent(IGtmEventRaisingVm vm, string userId, string planType)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.AccountActivity,
                guid = userId,
                action_taken = AccountActivities.StatementDownloaded,
                plan_type = planType
            });
        }

        public void RaiseTransactionsViewedEvent(IGtmEventRaisingVm vm, string userId, string planType)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.AccountActivity,
                guid = userId,
                action_taken = AccountActivities.TransactionsViewed,
                plan_type = planType
            });
        }

        public void RaiseBudgetCalculatorStartedEvent(HouseholdStatusVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.BudgetCalculator,
                step = BudgetCalculatorSteps.Step1Started,
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",     
                housing_status = vm == null ? null : String.IsNullOrEmpty(vm.HousingStatus) ? null : vm.HousingStatus,
                employment_status = vm  == null ? null : String.IsNullOrEmpty(vm.EmploymentStatus) ? null : vm.EmploymentStatus
            });
        }

        public void RaiseBudgetCalculatorHouseholdDetailsEvent(IGtmEventRaisingVm vm, string userId, string housingStatus, string employmentStatus)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.BudgetCalculator,
                step = BudgetCalculatorSteps.Step2HouseholdDetailsSubmitted,
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                housing_status = String.IsNullOrEmpty(housingStatus) ? null : housingStatus,
                employment_status = String.IsNullOrEmpty(employmentStatus) ? null : employmentStatus
            });
        }

        public void RaiseBudgetCalculatorIncomeEvent(IGtmEventRaisingVm vm, string userId, string employmentStatus, string housingStatus, decimal income)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.BudgetCalculator,
                step = BudgetCalculatorSteps.Step3IncomeSubmitted,
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                housing_status = String.IsNullOrEmpty(housingStatus) ? null : housingStatus,
                employment_status = String.IsNullOrEmpty(employmentStatus) ? null : employmentStatus,
                monthly_income = income,
            });
        }

        public void RaiseBudgetCalculatorExpenditureEvent(IGtmEventRaisingVm vm, string userId, string employmentStatus, string housingStatus, decimal income, decimal expenses, decimal disposableIncome)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.BudgetCalculator,
                step = BudgetCalculatorSteps.Step4ExpenditureSubmitted,
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                housing_status = String.IsNullOrEmpty(housingStatus) ? null : housingStatus,
                employment_status = String.IsNullOrEmpty(employmentStatus) ? null : employmentStatus,
                monthly_income = income,
                monthly_expenses = expenses,
                monthly_disposable_income = disposableIncome
            });
        }

        public void RaiseBudgetCalculatorCompletedEvent(BudgetSummaryVm vm, string userId, string employmentStatus, string housingStatus)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.BudgetCalculator,
                step = BudgetCalculatorSteps.Step5Completed,
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                housing_status = String.IsNullOrEmpty(housingStatus) ? null : housingStatus,
                employment_status = String.IsNullOrEmpty(employmentStatus) ? null : employmentStatus,
                monthly_income = vm.IncomeTotal,
                monthly_expenses = vm.TotalExpenditure,
                monthly_disposable_income = vm.DisposableIncome
            });
        }

        public void RaiseBudgetCalculatorContinuedToPaymentEvent(BudgetSummaryVm vm, string userId, string employmentStatus, string housingStatus)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.BudgetCalculator,
                step = BudgetCalculatorSteps.Step6ContinuedToPayment,
                guid = userId,
                user_status = String.IsNullOrEmpty(userId) ? "Not Logged In" : "Logged In",
                housing_status = String.IsNullOrEmpty(housingStatus) ? null : housingStatus,
                employment_status = String.IsNullOrEmpty(employmentStatus) ? null : employmentStatus,
                monthly_income = vm.IncomeTotal,
                monthly_expenses = vm.TotalExpenditure,
                monthly_disposable_income = vm.DisposableIncome
            });
        }

        public void RaiseAgentRegistration_RequestVerifiedEvent(IGtmEventRaisingVm vm, string userId, string errorMessage)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.AgentRegistration,
                step = AgentRegistrationSteps.Step1RequestVerified,
                guid = userId,
                error_message = String.IsNullOrEmpty(errorMessage) ? null : errorMessage
            });
        }

        public void RaiseAgentRegistration_RegistrationCompleteEvent(IGtmEventRaisingVm vm, string userId, string errorMessage)
        {
            vm.GtmEvents.Add(new GtmEvent()
            {
                gtm_event = GtmEvents.AgentRegistration,
                step = AgentRegistrationSteps.Step2RegistrationComplete,
                guid = userId,
                error_message = String.IsNullOrEmpty(errorMessage) ? null : errorMessage
            });
        }

        public void RaiseContactPreferecesPageEvent(ContactPreferencesVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent
            {
                gtm_event = GtmEvents.ContactPreference,
                guid = userId,
                action_taken = "Contact Prefereces Page Viewed"
            });
        }

        public void RaiseMobileUpdatePageEvent(ContactPreferencesVm vm, string userId)
        {
            vm.GtmEvents.Add(new GtmEvent
            {
                gtm_event = GtmEvents.ContactPreference,
                guid = userId,
                action_taken = "Mobile Update Page Viewed"
            });
        }

        public void RaiseContactPreferecesSMSChangeEvent(ContactPreferencesVm vm, string userId, bool newValue, bool oldValue)
        {
            if (newValue != oldValue)
            {
                string accountActivities = newValue ? "SMS selected" : "SMS deselected";

                vm.GtmEvents.Add(new GtmEvent
                {
                    gtm_event = GtmEvents.ContactPreference,
                    guid = userId,
                    action_taken = accountActivities
                });
            }
        }

        public void RaiseContactPreferecesEmailChangeEvent(ContactPreferencesVm vm, string userId, bool newValue, bool oldValue)
        {
            if (newValue != oldValue)
            {
                string accountActivities = newValue ? "Email selected" : "Email deselected";

                vm.GtmEvents.Add(new GtmEvent
                {
                    gtm_event = GtmEvents.ContactPreference,
                    guid = userId,
                    action_taken = accountActivities
                });
            }
        }

        public void RaiseMobilePhoneNumberChangeEvent(ContactPreferencesVm vm, string userId, string newValue, string oldValue)
        {
            if (newValue != oldValue)
            {
                vm.GtmEvents.Add(new GtmEvent
                {
                    gtm_event = GtmEvents.ContactPreference,
                    guid = userId,
                    action_taken = "Mobile number updated"
                });
            }
        }
    }
}
