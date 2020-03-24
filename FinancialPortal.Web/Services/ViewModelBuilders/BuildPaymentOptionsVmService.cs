using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.ViewModelBuilders
{
    public class BuildPaymentOptionsVmService : IBuildPaymentOptionsVmService
    {
        private readonly IApiGatewayProxy _apiGatewayProxy;
        private readonly IArrearsDescriptionProcess _arrearsDescriptionProcess;
        private readonly IDirectDebitFrequencyTranslator _directDebitFrequencyTranslator;
        private readonly PortalSetting  _portalSetting;
        private readonly IAccountsService _accountsService;

        public BuildPaymentOptionsVmService(
            IApiGatewayProxy apiGatewayProxy,
            IArrearsDescriptionProcess arrearsDescriptionProcess,
            IDirectDebitFrequencyTranslator directDebitFrequencyTranslator,
            PortalSetting portalSetting,
            IAccountsService accountsService)
        {
            _apiGatewayProxy = apiGatewayProxy;
            _arrearsDescriptionProcess = arrearsDescriptionProcess;
            _directDebitFrequencyTranslator = directDebitFrequencyTranslator;
            _portalSetting = portalSetting;
            _accountsService = accountsService;
        }

        public async Task<PaymentOptionsVm> Build(IUserIdentity loggedInUser, IApplicationSessionState applicationSessionState, Guid lowellReferenceSurrogateKey, string caseflowUserId)
        {
            string lowellReference = applicationSessionState.GetLowellReferenceFromSurrogate(lowellReferenceSurrogateKey);
            AccountReferenceDto accountReferenceDto = new AccountReferenceDto() {LowellReference = lowellReference};
            PaymentOptionsDto paymentOptionsDto = await _apiGatewayProxy.GetPaymentOptions(accountReferenceDto);
            IncomeAndExpenditureApiModel incomeAndExpenditureDto = await _apiGatewayProxy.GetIncomeAndExpenditure(lowellReference);
            List<AccountSummary> accounts;

            if (caseflowUserId != null)
            {
                accounts = await _accountsService.GetAccounts(caseflowUserId);
            }
            else
            {
                accounts = await _accountsService.GetMyAccountsSummary(lowellReference);
            }

            var workingAccounts = accounts.Count(a => !a.AccountStatusIsClosed);

            if (workingAccounts == 0)
            {
                workingAccounts = 1;
            }

            var accountDetails = await _accountsService.GetAccount(caseflowUserId, lowellReference);

            string[] planMessages = accountDetails.PlanMessages;

            var paymentOptionsVm = new PaymentOptionsVm()
            {
                OutstandingBalance = paymentOptionsDto.OutstandingBalance,
                LowellReference = paymentOptionsDto.LowellReference,
                ClientName = paymentOptionsDto.ClientName,
                ExcludedAccountMessage = paymentOptionsDto.ExcludedAccountMessage,
                PlanInPlace = paymentOptionsDto.PlanInPlace,
                PlanIsDirectDebit = paymentOptionsDto.PaymentPlanIsDirectDebit,
                WithLowellSolicitors = paymentOptionsDto.WithLowellSolicitors,
                PaymentOptions = new List<PaymentOptionsSelectionsVm>(),
                DiscountPercentage = paymentOptionsDto.DiscountPercentage,
                DiscountAmount = paymentOptionsDto.DiscountAmount,
                DiscountExpiryDate = paymentOptionsDto.DiscountExpiryDate,
                DiscountedBalance = paymentOptionsDto.DiscountedBalance,
                DiscountBalanceAvailable = paymentOptionsDto.DiscountBalanceAvailable,
                ProposedDiscountedBalanceIfAccepted = paymentOptionsDto.ProposedDiscountedBalanceIfAccepted,
                DiscountedBalancePreviouslyAccepted = paymentOptionsDto.DiscountedBalancePreviouslyAccepted,
                ArrearsMessage = _arrearsDescriptionProcess.DeriveArrearsDetail(paymentOptionsDto.PaymentPlanArrearsAmount,
                    paymentOptionsDto.PaymentPlanIsAutomated),
                StandingOrder = paymentOptionsDto.StandingOrder,
                StandingOrderMessage = paymentOptionsDto.StandingOrderMessage,
                VerifoneTransactionGuid = $"{paymentOptionsDto.LowellReference}_{Guid.NewGuid()}",
                DiscountAccepted = paymentOptionsDto.DiscountedBalancePreviouslyAccepted,
                PlanMessage = planMessages!=null && planMessages.Length>0? planMessages[0]:string.Empty
            };

            if (loggedInUser.IsLoggedInUser)
                paymentOptionsVm.DirectDebitEmailAddress = loggedInUser.EmailAddress;
            else
                paymentOptionsVm.DirectDebitIsEmailAddressFieldVisible = true;

            // Logged in user has accept T&C defaulted and will be hidden.
            // Anon user has tick box displayed. Must be ticked.
            if (loggedInUser.IsLoggedInUser)
                paymentOptionsVm.AcceptTermsAndConditions = true;
            else
                paymentOptionsVm.IsAcceptTermsAndConditionsFieldVisible = true;

            // Work out amount that needs to be paid to clear balance
            if (paymentOptionsVm.DiscountedBalancePreviouslyAccepted)
                paymentOptionsVm.FullPaymentBalance = paymentOptionsVm.DiscountedBalance;
            else
                paymentOptionsVm.FullPaymentBalance = paymentOptionsVm.OutstandingBalance;

            // Customer has a plan but it isn't direct debit.
            // Used to display a message informing customer that they can change to a DD online.
            if (paymentOptionsDto.PlanInPlace && !paymentOptionsDto.PaymentPlanIsDirectDebit)
                paymentOptionsVm.HasNonDirectDebitPlanInPlace = true;

            if (paymentOptionsDto.WithLowellSolicitors)
                paymentOptionsVm.LowellSolicitorsRedirectLink = _portalSetting.SolicitorsRedirectDataProtectionUrl;

            // Shared list of options for partial / full
            paymentOptionsVm.SourceOfFunds = BuildSourceOfFundsSelections(paymentOptionsDto);

            // Direct Debit
            paymentOptionsVm.DirectDebitFrequency = BuildFrequencyList(paymentOptionsDto.DirectDebitFrequencies);
            paymentOptionsVm.DirectDebitStartDateEarliest = paymentOptionsDto.DirectDebitStartDateEarliest;
            paymentOptionsVm.DirectDebitStartDateLatest = paymentOptionsDto.DirectDebitStartDateLatest;

            // TODO: Wrap the code below in a strategy pattern
            if (paymentOptionsDto.CanMakeFullPayment)
            {
                paymentOptionsVm.PaymentOptions.Add(new PaymentOptionsSelectionsVm()
                {
                    DisplayedText = "Card payment (Pay in Full)",
                    Value = PaymentOptionsSelectionsVm.Values.FullPayment,
                    DataFormValue = PaymentOptionsSelectionsVm.Values.FullPayment
                });
            }
            if (paymentOptionsDto.CanMakePartialPayment)
            {
                paymentOptionsVm.PaymentOptions.Add(new PaymentOptionsSelectionsVm()
                {
                    DisplayedText = "Card payment (Partial amount)",
                    Value = PaymentOptionsSelectionsVm.Values.PartialPayment,
                    DataFormValue = PaymentOptionsSelectionsVm.Values.PartialPayment,
                    ClassValue = "js-hide-option"           // script hides this
                });
            }
            if (paymentOptionsDto.CanSetupDirectDebit)
            {
                paymentOptionsVm.PaymentOptions.Add(new PaymentOptionsSelectionsVm()
                {
                    DisplayedText = "Direct Debit plan",
                    Value = PaymentOptionsSelectionsVm.Values.DirectDebit,
                    DataFormValue = PaymentOptionsSelectionsVm.Values.DirectDebit
                });
            }

            // Only add 'please select' if there are options
            // Required because view checks for availability of payment options
            if (paymentOptionsVm.PaymentOptions.Count > 0)
            {
                paymentOptionsVm.PaymentOptions.Insert(0, new PaymentOptionsSelectionsVm()
                {
                    DisplayedText = "Please Select",
                    Value = PaymentOptionsSelectionsVm.Values.PleaseSelect,
                    DataFormValue = PaymentOptionsSelectionsVm.Values.PleaseSelect
                });
            }

            paymentOptionsVm.LowellReferenceSurrogate = lowellReferenceSurrogateKey;

            paymentOptionsVm.IandENotAvailable = incomeAndExpenditureDto == null;

           paymentOptionsVm.IandELessThanOrIs12MonthsOld =
                (incomeAndExpenditureDto != null && incomeAndExpenditureDto.Created.AddMonths(12).Date >= DateTime.Now.Date);
            paymentOptionsVm.AverageMonthlyPayment = _portalSetting.AverageMonthlyPaymentAmount;
            paymentOptionsVm.MonthlyDisposableIncome =
                (incomeAndExpenditureDto == null ? 0 : (incomeAndExpenditureDto.DisposableIncome * (_portalSetting.MonthlyDisposableIncomePlanSetupPercentage / 100)));

            paymentOptionsVm.AccountCount = workingAccounts;
            paymentOptionsVm.MonthlyDisposableIncomePerAccount = paymentOptionsVm.MonthlyDisposableIncome / workingAccounts;

            return paymentOptionsVm;
        }

        // Used to re-instate user entries after loading fresh data from CaseFlow or from cache
        public void UpdateFieldsFromUserEntries(IUserIdentity loggedInUser, PaymentOptionsVm paymentOptionsToUpdate, PaymentOptionsVm paymentOptionsUserEntries)
        {
            paymentOptionsToUpdate.DiscountAccepted = paymentOptionsUserEntries.DiscountAccepted;
            paymentOptionsToUpdate.SelectedPaymentOption = paymentOptionsUserEntries.SelectedPaymentOption;

            paymentOptionsToUpdate.FullPaymentSelectedSourceOfFunds = paymentOptionsUserEntries.FullPaymentSelectedSourceOfFunds;
            paymentOptionsToUpdate.FullPaymentSourceOfFundsOtherText = paymentOptionsUserEntries.FullPaymentSourceOfFundsOtherText;

            paymentOptionsToUpdate.PartialPaymentAmount = paymentOptionsUserEntries.PartialPaymentAmount;
            paymentOptionsToUpdate.PartialPaymentSelectedSourceOfFunds = paymentOptionsUserEntries.PartialPaymentSelectedSourceOfFunds;
            paymentOptionsToUpdate.PartialPaymentSourceOfFundsOtherText = paymentOptionsUserEntries.PartialPaymentSourceOfFundsOtherText;

            paymentOptionsToUpdate.DirectDebitSelectedFrequency = paymentOptionsUserEntries.DirectDebitSelectedFrequency;
            paymentOptionsToUpdate.DirectDebitSelectedStartDate = paymentOptionsUserEntries.DirectDebitSelectedStartDate;
            paymentOptionsToUpdate.DirectDebitAmount = paymentOptionsUserEntries.DirectDebitAmount;
            paymentOptionsToUpdate.AverageMonthlyPayment = paymentOptionsUserEntries.AverageMonthlyPayment;
            paymentOptionsToUpdate.MonthlyDisposableIncome = paymentOptionsUserEntries.MonthlyDisposableIncome;
            paymentOptionsToUpdate.SelectedPlanSetupOption = paymentOptionsUserEntries.SelectedPlanSetupOption;
            paymentOptionsToUpdate.IandELessThanOrIs12MonthsOld = paymentOptionsUserEntries.IandELessThanOrIs12MonthsOld;

            // User should only have provided Direct Debit Email Address if they are anonymous (not logged in)
            // Email will have been pre-populated with user's email address during building of VM if they are logged in
            if (!loggedInUser.IsLoggedInUser)
                paymentOptionsToUpdate.DirectDebitEmailAddress = paymentOptionsUserEntries.DirectDebitEmailAddress;

            // User only ticks T&C box when anonymous. Will be defaulted to true by builder if logged in.
            if (!loggedInUser.IsLoggedInUser)
                paymentOptionsToUpdate.AcceptTermsAndConditions = paymentOptionsUserEntries.AcceptTermsAndConditions;
        }

        List<PaymentOptionsSourceOfFundsSelectionsVm> BuildSourceOfFundsSelections(PaymentOptionsDto paymentOptionsDto)
        {
            List<PaymentOptionsSourceOfFundsSelectionsVm> sourceOfFunds = new List<PaymentOptionsSourceOfFundsSelectionsVm>();

            if (paymentOptionsDto.SourceOfFundsPaymentOptions == null)
                paymentOptionsDto.SourceOfFundsPaymentOptions = new List<string>();

            foreach (string optionString in paymentOptionsDto.SourceOfFundsPaymentOptions)
            {
                sourceOfFunds.Add(new PaymentOptionsSourceOfFundsSelectionsVm()
                {
                    Value = optionString,
                    DisplayedText = optionString,

                    // TODO: should be refactored into a flag from Payment Service
                    // TODO: ideally, flag or special value should come from CaseFlow
                    DataFormValue = (optionString == "Other") ? "other-field" : null
                });
            }

            return sourceOfFunds;
        }

        List<DirectDebitPaymentFrequencyVm> BuildFrequencyList(IList<string> stringList)
        {
            List<DirectDebitPaymentFrequencyVm> vmList = new List<DirectDebitPaymentFrequencyVm>();

            foreach (string s in stringList)
            {
                vmList.Add(new DirectDebitPaymentFrequencyVm()
                {
                    Value = _directDebitFrequencyTranslator.TranslateDescriptionToClientScriptCompatibleValue(s),
                    DisplayedText = s
                });
            }

            return vmList;
        }
    }
}
