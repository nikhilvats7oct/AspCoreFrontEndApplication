using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.Interfaces;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Services.ViewModelBuilders
{
    public class BuildOneOffPaymentReviewVmService : IBuildOneOffPaymentReviewVmService
    {
        private readonly IBuildPaymentOptionsVmService _buildPaymentOptionsVmService;
        private readonly IPaymentOptionsVmValidatorProcess _paymentOptionsVmValidatorProcess;
        private readonly VerifoneSetting  _verifoneSetting;
        private readonly IVerifonePaymentProviderService _verifonePaymentProviderService;

        class ValidationResult : IBuildOneOffPaymentReviewVmValidationResult
        {
            public bool IsValid { get; internal set; }
            public OneOffPaymentReviewVm OneOffPaymentReviewVm { get; internal set; }
        }

        public BuildOneOffPaymentReviewVmService(
            IBuildPaymentOptionsVmService buildPaymentOptionsVmService,
            IPaymentOptionsVmValidatorProcess paymentOptionsVmValidatorProcess,
            VerifoneSetting verifoneSetting, 
            IVerifonePaymentProviderService verifonePaymentProviderService)
        {
            _buildPaymentOptionsVmService = buildPaymentOptionsVmService;
            _paymentOptionsVmValidatorProcess = paymentOptionsVmValidatorProcess;
            _verifoneSetting = verifoneSetting;
            _verifonePaymentProviderService = verifonePaymentProviderService;
        }

        // Checks the information entered by the user against CaseFlow afresh
        // If validation fails, returns false and OneOffPaymentReviewVm will be output as NULL
        // If successful, returns true and OneOffPaymentReviewVm will be output populated
        public async Task<IBuildOneOffPaymentReviewVmValidationResult> ValidateAndBuild(
            IUserIdentity loggedInUser, IApplicationSessionState applicationSessionState,
            Guid lowellReferenceSurrogateKey, PaymentOptionsVm paymentOptionsVmWithUserEntries, string caseflowUserId)
        {
            // Reload from CaseFlow
            PaymentOptionsVm paymentOptionsVm =
                await _buildPaymentOptionsVmService.Build(loggedInUser, applicationSessionState, lowellReferenceSurrogateKey, caseflowUserId);

            // Populate user entries, giving a clean model to validate
            _buildPaymentOptionsVmService.UpdateFieldsFromUserEntries(loggedInUser, paymentOptionsVm, paymentOptionsVmWithUserEntries);

            if (!_paymentOptionsVmValidatorProcess.Validate(paymentOptionsVm))
                return new ValidationResult() { IsValid = false };

            // Ensure we are not using this - must use clean, validated model (defensive coding)
            // ReSharper disable once RedundantAssignment
            paymentOptionsVmWithUserEntries = null;

            var postDataXml = _verifonePaymentProviderService.CreatePayload(paymentOptionsVm);

            OneOffPaymentReviewVm vm = new OneOffPaymentReviewVm()
            {
                LowellReference = paymentOptionsVm.LowellReference,
                ClientName = paymentOptionsVm.ClientName,
                VerifoneUrl = _verifoneSetting.ApiEndpoint,
                VerifonePostDataXml = postDataXml,
                VerifoneTransactionGuid = paymentOptionsVm.VerifoneTransactionGuid.ToString(),
                UserID = loggedInUser.UserId,
                PaidInFull = paymentOptionsVm.SelectedPaymentOption == PaymentOptionsSelectionsVm.Values.FullPayment,
                DiscountAvailable = paymentOptionsVm.DiscountBalanceAvailable,
                DiscountSelected = paymentOptionsVm.DiscountAccepted,
                PlanInPlace = paymentOptionsVm.PlanInPlace,
                InArrears = !String.IsNullOrEmpty(paymentOptionsVm.ArrearsMessage)
            };

            // Source payment information from appropriate nested object
            if (paymentOptionsVm.SelectedPaymentOption == PaymentOptionsSelectionsVm.Values.FullPayment)
            {
                vm.PaymentAmount = paymentOptionsVm.FullPaymentAmountDerived;
                vm.SourceOfFunds = paymentOptionsVm.FullPaymentSelectedSourceOfFunds;
                vm.SourceOfFundsOther = paymentOptionsVm.FullPaymentSourceOfFundsOtherText;
                vm.PaidInFull = true;
            }
            else if (paymentOptionsVm.SelectedPaymentOption == PaymentOptionsSelectionsVm.Values.PartialPayment)
            {
                Debug.Assert(paymentOptionsVm.PartialPaymentAmount != null, "paymentOptionsVmWithUserEntries.PartialPaymentAmount != null");

                vm.PaymentAmount = paymentOptionsVm.PartialPaymentAmount.Value;
                vm.SourceOfFunds = paymentOptionsVm.PartialPaymentSelectedSourceOfFunds;
                vm.SourceOfFundsOther = paymentOptionsVm.PartialPaymentSourceOfFundsOtherText;
            }
            else
                throw new ApplicationException("Invalid SelectedPaymentOption");

            return new ValidationResult() { IsValid = true, OneOffPaymentReviewVm = vm };
        }

    }
}
