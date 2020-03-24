using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.Interfaces;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.ViewModelBuilders
{
    public class BuildDirectDebitPlanOverviewVmService : IBuildDirectDebitPlanOverviewVmService
    {
        private readonly IBuildPaymentOptionsVmService _buildPaymentOptionsVmService;
        private readonly IBuildDirectDebitDetailsVmService _buildDirectDebitDetailsVmService;
        private readonly IPaymentOptionsVmValidatorProcess _paymentOptionsVmValidatorProcess;
        private readonly IDirectDebitDetailsVmValidatorProcess _directDebitDetailsVmValidatorProcess;
        private readonly IDirectDebitFrequencyTranslator _directDebitFrequencyTranslator;
        private readonly IDirectDebitTermCalculator _directDebitTermCalculator;

        class ValidationResult : IBuildDirectDebitPlanOverviewVmValidationResult
        {
            public bool IsValid { get; internal set; }

            public DirectDebitPlanOverviewVm DirectDebitPlanOverviewVm { get; internal set; }
        }

        public BuildDirectDebitPlanOverviewVmService(
            IBuildPaymentOptionsVmService buildPaymentOptionsVmService,
            IBuildDirectDebitDetailsVmService buildDirectDebitDetailsVmService,
            IPaymentOptionsVmValidatorProcess paymentOptionsVmValidatorProcess,
            IDirectDebitDetailsVmValidatorProcess directDebitDetailsVmValidatorProcess,
            IDirectDebitFrequencyTranslator directDebitFrequencyTranslator,
            IDirectDebitTermCalculator directDebitTermCalculator)
        {
            _buildPaymentOptionsVmService = buildPaymentOptionsVmService;
            _buildDirectDebitDetailsVmService = buildDirectDebitDetailsVmService;
            _paymentOptionsVmValidatorProcess = paymentOptionsVmValidatorProcess;
            _directDebitDetailsVmValidatorProcess = directDebitDetailsVmValidatorProcess;
            _directDebitFrequencyTranslator = directDebitFrequencyTranslator;
            _directDebitTermCalculator = directDebitTermCalculator;
        }

        public DirectDebitPlanOverviewVm Build(PaymentOptionsVm accountVm, DirectDebitDetailsVm directDebitDetailsVm)
        {
            // Should never be null, as validated with Fluent
            Debug.Assert(accountVm.DirectDebitAmount != null, "accountVm.DirectDebitPayment.Amount != null");

            int totalMonths = _directDebitTermCalculator.CalculateTermInMonths(
                                accountVm.FullPaymentAmountDerived, accountVm.DirectDebitAmount.Value,
                                accountVm.DirectDebitSelectedFrequency);
            int years = totalMonths / 12;
            int months = totalMonths % 12;

            DirectDebitPlanOverviewVm directDebitPlanOverview = new DirectDebitPlanOverviewVm()
            {
                SortCode = directDebitDetailsVm.SortCode,
                AccountNumber = directDebitDetailsVm.AccountNumber,
                AccountHoldersName = directDebitDetailsVm.AccountHoldersName,
                ClientName = accountVm.ClientName,
                LowellReference = accountVm.LowellReference,
                PaymentAmount = accountVm.DirectDebitAmount.Value,
                PaymentType = AccountMessages.DirectDebit,
                StartDate = accountVm.DirectDebitSelectedStartDate,
                Frequency =
                    _directDebitFrequencyTranslator.TranslateClientScriptCompatibleValueToDescription(
                        accountVm.DirectDebitSelectedFrequency),
                GuaranteeRead = directDebitDetailsVm.AcceptDirectDebitGuarantee,
                PlanTotal = accountVm.DiscountedBalancePreviouslyAccepted
                    ? accountVm.DiscountedBalance
                    : accountVm.OutstandingBalance,
                DiscountAccepted = accountVm.DiscountAccepted,
                DiscountAmount = accountVm.DiscountAmount,
                EmailAddress = accountVm.DirectDebitEmailAddress,
                DiscountAvailable = directDebitDetailsVm.DiscountAvailable,
                PaymentFrequency = directDebitDetailsVm.PaymentFrequency,
                TermYears = years,
                TermMonths = months,
                SelectedPlanSetupOption = directDebitDetailsVm.SelectedPlanSetupOption
            };

            return directDebitPlanOverview;
        }

        public async Task<IBuildDirectDebitPlanOverviewVmValidationResult> ValidateAndBuild(
            IUserIdentity loggedInUser, IApplicationSessionState applicationSessionState, Guid lowellReferenceSurrogateKey,
            PaymentOptionsVm paymentOptionsVmWithUserEntries, DirectDebitDetailsVm directDebitDetailsVmWithUserEntries, string caseflowUserId)
        {
            //
            // Payment Options - reconstruct from CaseFlow, populate user entris  and validate clean
            //

            PaymentOptionsVm paymentOptionsVm = await _buildPaymentOptionsVmService.Build(loggedInUser, applicationSessionState, lowellReferenceSurrogateKey, caseflowUserId);
            
            _buildPaymentOptionsVmService.UpdateFieldsFromUserEntries(loggedInUser, paymentOptionsVm, paymentOptionsVmWithUserEntries);

            if (!_paymentOptionsVmValidatorProcess.Validate(paymentOptionsVm))
                return new ValidationResult() { IsValid = false };

            //
            // Direct Debit Details - reconstruct from fresh payment options, populate user entris and validate clean
            //
            DirectDebitDetailsVm directDebitDetailsVm = _buildDirectDebitDetailsVmService.Build(paymentOptionsVm);

            _buildDirectDebitDetailsVmService.UpdateFieldsFromUserEntries(directDebitDetailsVm, directDebitDetailsVmWithUserEntries);

            if (!_directDebitDetailsVmValidatorProcess.Validate(directDebitDetailsVm))
                return new ValidationResult() { IsValid = false };

            //
            // Return valid result with overview built clean
            //
            return new ValidationResult()
            {
                IsValid = true,
                DirectDebitPlanOverviewVm = Build(paymentOptionsVm, directDebitDetailsVm)
            };
        }

    }
}
