using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services
{
    public class AmendDirectDebitPlanService : IAmendDirectDebitPlanService
    {
        private readonly IAmendDirectDebitVmValidatorProcess _amendDirectDebitVmValidatorProcess;
        private readonly IDirectDebitFrequencyTranslator _directDebitFrequencyTranslator;
        private readonly IBuildAmendDirectDebitVmService _buildAmendDirectDebitVmService;
        private readonly ISendAmendDirectDebitPlanProcess _sendAmendDirectDebitPlanProcess;
        private readonly IDirectDebitTermCalculator _directDebitTermCalculator;

        public AmendDirectDebitPlanService(IAmendDirectDebitVmValidatorProcess amendDirectDebitVmValidatorProcess,
                                           IDirectDebitFrequencyTranslator directDebitFrequencyTranslator,
                                           IBuildAmendDirectDebitVmService buildAmendDirectDebitVmService,
                                           ISendAmendDirectDebitPlanProcess sendAmendDirectDebitPlanProcess,
                                           IDirectDebitTermCalculator directDebitTermCalculator)
        {
            _amendDirectDebitVmValidatorProcess = amendDirectDebitVmValidatorProcess;
            _directDebitFrequencyTranslator = directDebitFrequencyTranslator;
            _buildAmendDirectDebitVmService = buildAmendDirectDebitVmService;
            _sendAmendDirectDebitPlanProcess = sendAmendDirectDebitPlanProcess;
            _directDebitTermCalculator = directDebitTermCalculator;
        }

        public async Task<bool> AmendDirectDebitPlan(IUserIdentity loggedInUser, 
                                                     IApplicationSessionState applicationSessionState,
                                                     Guid lowellReferenceSurrogateKey, 
                                                     AmendDirectDebitVm amendDirectDebitVmWithUserEntries,
                                                     string caseflowUserId)
        {
            // Must be a logged in user
            if (loggedInUser.IsLoggedInUser == false)
                throw new ApplicationException("AmendDirectDebitPlan can only be used for a logged in user due to requiring email address.");

            // Reload from CaseFlow
            AmendDirectDebitVm amendDirectDebitVm = await _buildAmendDirectDebitVmService.Build(applicationSessionState, lowellReferenceSurrogateKey, caseflowUserId);

            // Populate user entries, giving a clean model to validate
            _buildAmendDirectDebitVmService.UpdateFieldsFromUserEntries(amendDirectDebitVm, amendDirectDebitVmWithUserEntries);

            if (!_amendDirectDebitVmValidatorProcess.Validate(amendDirectDebitVm))
                return false;

            // Ensure we are not using this - must use clean, validated model (defensive coding)
            // ReSharper disable once RedundantAssignment
            amendDirectDebitVmWithUserEntries = null;

            var directDebitPaymentDto = new DirectDebitPaymentDto
            {
                Frequency = _directDebitFrequencyTranslator.TranslateClientScriptCompatibleValueToDescription(amendDirectDebitVm.PlanFrequency),
                LowellReference = amendDirectDebitVm.LowellReference,
                PaymentAmount = amendDirectDebitVm.DirectDebitAmount.Value,
                StartDate = amendDirectDebitVm.PlanStartDate.ToShortDateString(),
                PlanTotal = amendDirectDebitVm.OutstandingBalance,
                DiscountAmount = amendDirectDebitVm.DiscountAmount,
                DiscountAccepted = amendDirectDebitVm.DiscountedBalance < amendDirectDebitVm.OutstandingBalance,
                EmailAddress = loggedInUser.EmailAddress,
                User = loggedInUser.IsLoggedInUser ? "WebUser" : "WebAnon"
            };

            await _sendAmendDirectDebitPlanProcess.SendAmendDirectDebitPlanAsync(directDebitPaymentDto);

            return true;
        }

        public int CalculateTermInMonths(decimal balance, decimal paymentAmount, string paymentFrequency)
        {
            return this._directDebitTermCalculator.CalculateTermInMonths(balance, paymentAmount, paymentFrequency);
        }
    }
}