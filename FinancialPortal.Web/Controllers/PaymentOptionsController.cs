using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FinancialPortal.Web.Controllers
{
    [AllowAnonymous]
    public class PaymentOptionsController : BaseController
    {
        private const string ControllerName = "PaymentOptions";
        private readonly IBuildDirectDebitDetailsVmService _buildDirectDebitDetailsVmService;
        private readonly IBuildDirectDebitPlanOverviewVmService _buildDirectDebitPlanOverviewVmService;
        private readonly IBuildOneOffPaymentReviewVmService _buildOneOffPaymentReviewVmService;
        private readonly IBuildPaymentOptionsVmService _buildPaymentOptionsVmService;
        private readonly IDirectDebitPlanSetupService _directDebitPlanSetupService;
        private readonly IGtmService _gtmService;
        private readonly IMapper _mapper;

        private readonly ILogger<BaseController> _logger;
        private readonly IVerifonePaymentProviderService _verifonePaymentProviderService;
        private readonly IWebActivityService _webActivityService;

        public PaymentOptionsController(ILogger<BaseController> logger,
            IConfiguration configuration,
            IBuildPaymentOptionsVmService buildPaymentOptionsVmService,
            IBuildOneOffPaymentReviewVmService buildOneOffPaymentReviewVmService,
            IVerifonePaymentProviderService verifonePaymentProviderService,
            IWebActivityService webActivityService,
            IBuildDirectDebitDetailsVmService buildDirectDebitDetailsVmService,
            IBuildDirectDebitPlanOverviewVmService buildDirectDebitPlanOverviewVmService,
            IDirectDebitPlanSetupService directDebitPlanSetupService,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IGtmService gtmService,
            IMapper mapper)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _logger = logger;
            _buildPaymentOptionsVmService = buildPaymentOptionsVmService;
            _buildOneOffPaymentReviewVmService = buildOneOffPaymentReviewVmService;
            _verifonePaymentProviderService = verifonePaymentProviderService;
            _buildDirectDebitDetailsVmService = buildDirectDebitDetailsVmService;
            _buildDirectDebitPlanOverviewVmService = buildDirectDebitPlanOverviewVmService;
            _directDebitPlanSetupService = directDebitPlanSetupService;
            _gtmService = gtmService;
            _mapper = mapper;
            _webActivityService = webActivityService;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var paymentOptionsVm = await _buildPaymentOptionsVmService.Build(
                LoggedInUser, ApplicationSessionState, id, GetCaseflowUserId());

            if (TempData.ContainsKey("GTMEvents"))
            {
                paymentOptionsVm.GtmEvents = JsonConvert.DeserializeObject<List<GtmEvent>>(TempData["GTMEvents"].ToString());
            }

            _gtmService.RaisePaymentEvent_PageVisited(paymentOptionsVm, LoggedInUserId, "Regular Account");

            // Save initial state in 'State' field, so that it can be reconstructed on postback
            // Field values lost due to a lack of hidden fields etc. will be retained this way
            paymentOptionsVm.InitialState = SerialiseModel(paymentOptionsVm);

            return View(paymentOptionsVm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid id, PaymentOptionsVm paymentOptionsVmPostedBack)
        {
            // Restore VM from state field, so that we get lists and all other fields required to build view
            var paymentOptionsVm =
                DeserialiseModel<PaymentOptionsVm>(paymentOptionsVmPostedBack.InitialState);
            Debug.Assert(paymentOptionsVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Carry state through
            paymentOptionsVm.InitialState = paymentOptionsVmPostedBack.InitialState;

            // Place form field selections into restored VM
            _buildPaymentOptionsVmService.UpdateFieldsFromUserEntries(
                LoggedInUser, paymentOptionsVm, paymentOptionsVmPostedBack);

            return await ProcessPaymentOptionsPost(id, paymentOptionsVm);
        }

        [HttpPost]
        public IActionResult PaymentMethods(Guid id, PaymentOptionsVm paymentOptionsVmPostedBack)
        {
            // Restore VM from state field, so that we get lists and all other fields required to build view
            var paymentOptionsVm =
                DeserialiseModel<PaymentOptionsVm>(paymentOptionsVmPostedBack.InitialState);
            Debug.Assert(paymentOptionsVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Carry state through
            paymentOptionsVm.InitialState = paymentOptionsVmPostedBack.InitialState;

            // Place form field selections into restored VM
            _buildPaymentOptionsVmService.UpdateFieldsFromUserEntries(
                LoggedInUser, paymentOptionsVm, paymentOptionsVmPostedBack);

            // Construct model for Cancel view
            var returnToFormVm = new ReturnToFormVm
            {
                State = SerialiseModel(paymentOptionsVm),
                StateFieldName = nameof(PaymentOptionsStateVm.FilledInState),
                StateType = nameof(PaymentOptionsVm),
                ReturnControllerName = ControllerName,
                ReturnActionName = nameof(Change)
            };

            return View("PaymentMethods", returnToFormVm);
        }

        [HttpPost]
        public IActionResult Change(Guid id, PaymentOptionsStateVm paymentOptionsStateVm)
        {
            // Restore VM from state field, so that form is as it was before moving on to next step
            var paymentOptionsVm =
                DeserialiseModel<PaymentOptionsVm>(paymentOptionsStateVm.FilledInState);
            Debug.Assert(paymentOptionsVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Save state for round-trip (to re-populate lists, messages etc)
            paymentOptionsVm.InitialState = paymentOptionsStateVm.FilledInState;

            paymentOptionsVm.SelectedPlanSetupOption = PlanSetupOptions.None;
            paymentOptionsVm.DirectDebitAmount = null;

            return View("Index", paymentOptionsVm);
        }

        [HttpPost]
        public async Task<IActionResult> Continue(Guid id, PaymentOptionsStateVm paymentOptionsStateVm)
        {
            // Restore VM from state field, so that form is as it was before moving on to next step
            var paymentOptionsVm =
                DeserialiseModel<PaymentOptionsVm>(paymentOptionsStateVm.FilledInState);
            Debug.Assert(paymentOptionsVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Process as if user posted it
            return await ProcessPaymentOptionsPost(id, paymentOptionsVm);
        }

        [HttpPost]
        public IActionResult Cancel(Guid id, PaymentOptionsVm paymentOptionsVmPostedBack)
        {
            // Restore VM from state field, so that we get lists and all other fields required to build view
            var paymentOptionsVm =
                DeserialiseModel<PaymentOptionsVm>(paymentOptionsVmPostedBack.InitialState);
            Debug.Assert(paymentOptionsVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Carry state through
            paymentOptionsVm.InitialState = paymentOptionsVmPostedBack.InitialState;

            // Place form field selections into restored VM
            _buildPaymentOptionsVmService.UpdateFieldsFromUserEntries(LoggedInUser, paymentOptionsVm,
                paymentOptionsVmPostedBack);

            // Construct model for Cancel view
            var cancelVm = new ReturnToFormVm
            {
                State = SerialiseModel(paymentOptionsVm),
                StateFieldName = nameof(PaymentOptionsStateVm.FilledInState),
                StateType = nameof(PaymentOptionsVm),
                ReturnControllerName = ControllerName,
                ReturnActionName = nameof(Change)
            };

            return View("PaymentCancel", cancelVm);
        }

        [HttpPost]
        public IActionResult OneOffCancel(Guid id, PaymentOptionsStateVm paymentOptionsStateVm)
        {
            // Construct model for Cancel view
            var cancelVm = new ReturnToFormVm
            {
                State = paymentOptionsStateVm.FilledInState,
                StateFieldName = nameof(PaymentOptionsStateVm.FilledInState),
                StateType = nameof(PaymentOptionsVm),
                ReturnControllerName = ControllerName,
                ReturnActionName = nameof(Continue)
            };

            return View("PaymentCancel", cancelVm);
        }

        [HttpPost]
        public async Task OneOffCardDetails(string lowellRef, string userId, bool fullPayment, bool discounted)
        {
            await _webActivityService.LogOneOffPaymentDetailsEntered(lowellRef, userId, !fullPayment, discounted);
            ApplicationSessionState.LogPaymentResult = true;
        }

        [HttpPost]
        public async Task<IActionResult> DirectDebit(Guid id, DirectDebitDetailsVm directDebitDetailsVmPostedBack)
        {
            // Restore fields not included in form (see One-Off for explanation)
            var directDebitDetailsVm =
                DeserialiseModel<DirectDebitDetailsVm>(directDebitDetailsVmPostedBack.InitialState);
            Debug.Assert(directDebitDetailsVm.InitialState == null,
                "Serialisation Loop Detected - a previous step must have serialised a model that already contains InitialState");

            // Round-trip stashed view model state
            directDebitDetailsVm.InitialState = directDebitDetailsVmPostedBack.InitialState;
            directDebitDetailsVm.PaymentOptionsFilledInState =
                directDebitDetailsVmPostedBack.PaymentOptionsFilledInState;

            _buildDirectDebitDetailsVmService.UpdateFieldsFromUserEntries(directDebitDetailsVm,
                directDebitDetailsVmPostedBack);

            ApplicationSessionState.LogSetUpPlanResult = true;
            return await ProcessDirectDebitDetailsPost(id, directDebitDetailsVm);
        }

        public IActionResult DirectDebitChange(Guid id, DirectDebitDetailsStateVm directDebitDetailsStateVm)
        {
            // Restore VM from state field, so that form is as it was before moving on to next step
            var directDebitDetailsVm =
                DeserialiseModel<DirectDebitDetailsVm>(directDebitDetailsStateVm.FilledInState);
            Debug.Assert(directDebitDetailsVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Save state for round-trip (to re-populate lists, messages etc)
            directDebitDetailsVm.InitialState = directDebitDetailsStateVm.FilledInState;

            return View("DirectDebit", directDebitDetailsVm);
        }

        public async Task<IActionResult> DirectDebitContinue(Guid id,
            DirectDebitDetailsStateVm directDebitDetailsStateVm)
        {
            // Restore VM from state field, so that form is as it was before moving on to next step
            var directDebitDetailsVm =
                DeserialiseModel<DirectDebitDetailsVm>(directDebitDetailsStateVm.FilledInState);
            Debug.Assert(directDebitDetailsVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Process as if user posted it
            ApplicationSessionState.LogSetUpPlanResult = true;
            return await ProcessDirectDebitDetailsPost(id, directDebitDetailsVm);
        }

        public async Task<IActionResult> DirectDebitComplete(Guid id,
            DirectDebitDetailsStateVm directDebitDetailsStateVm)
        {
            var directDebitDetailsVm =
                DeserialiseModel<DirectDebitDetailsVm>(directDebitDetailsStateVm.FilledInState);
            var paymentOptionsVm =
                DeserialiseModel<PaymentOptionsVm>(directDebitDetailsVm
                    .PaymentOptionsFilledInState);

            Debug.Assert(directDebitDetailsVm.InitialState == null,
                "Serialisation Loop Detected - a previous step must have serialised a model that already contains InitialState");
            Debug.Assert(paymentOptionsVm.InitialState == null,
                "Serialisation Loop Detected - a previous step must have serialised a model that already contains InitialState");

            //
            // IMPORTANT: Validates models again versus CaseFlow, to ensure no tamper or state changes since
            // started process of entering payment information
            //
            DirectDebitPlanOverviewVm directDebitPlanOverview;
            {
                var result = await _buildDirectDebitPlanOverviewVmService.ValidateAndBuild(
                    LoggedInUser, ApplicationSessionState, id, paymentOptionsVm, directDebitDetailsVm, GetCaseflowUserId());

                if (result.IsValid == false)
                {
                    return View("ValidationFailed");
                }

                directDebitPlanOverview = result.DirectDebitPlanOverviewVm;
                directDebitPlanOverview.UserLoggedIn = !string.IsNullOrEmpty(LoggedInUserId);
            }

            if (ApplicationSessionState.LogSetUpPlanResult)
            {
                if (directDebitDetailsVm.DiscountSelected)
                {
                    await LogDiscountDirectDebitCompleteSuccess(paymentOptionsVm);
                }
                else
                {
                    await LogDirectDebitCompleteSuccess(paymentOptionsVm);
                }

                await _directDebitPlanSetupService.CreateDirectDebitPlan(directDebitPlanOverview);
                _gtmService.RaiseDirectDebitEvent_PlanSetUp(directDebitPlanOverview, LoggedInUserId,
                    "Direct Debit Plan");
                ApplicationSessionState.LogSetUpPlanResult = false;
            }

            return View(directDebitPlanOverview);
        }

        // Generates cancel view required when click 'Cancel' on Direct Debit bank details page
        public IActionResult DirectDebitCancel(Guid id, DirectDebitDetailsVm directDebitDetailsVmPostedBack)
        {
            // Restore fields not included in form (see Cancel for explanation)
            var directDebitDetailsVm =
                DeserialiseModel<DirectDebitDetailsVm>(directDebitDetailsVmPostedBack.InitialState);
            Debug.Assert(directDebitDetailsVm.InitialState == null,
                "Serialisation Loop Detected - a previous step must have serialised a model that already contains InitialState");

            // Carry state through
            directDebitDetailsVm.InitialState = directDebitDetailsVmPostedBack.InitialState;
            directDebitDetailsVm.PaymentOptionsFilledInState =
                directDebitDetailsVmPostedBack.PaymentOptionsFilledInState;

            // Place form field selections into restored VM
            _buildDirectDebitDetailsVmService.UpdateFieldsFromUserEntries(directDebitDetailsVm,
                directDebitDetailsVmPostedBack);

            // Construct model for Cancel view
            var cancelVm = new ReturnToFormVm
            {
                State = SerialiseModel(directDebitDetailsVm),
                StateFieldName = nameof(DirectDebitDetailsStateVm.FilledInState),
                StateType = nameof(DirectDebitDetailsVm),
                ReturnControllerName = ControllerName,
                ReturnActionName = nameof(DirectDebitChange)
            };

            return View("PaymentCancel", cancelVm);
        }

        // Generates Cancel view required when click 'Cancel' on Direct Debit confirmation page
        public IActionResult DirectDebitConfirmCancel(Guid id, DirectDebitDetailsStateVm directDebitDetailsStateVm)
        {
            // Construct model for Cancel view
            var cancelVm = new ReturnToFormVm
            {
                State = directDebitDetailsStateVm.FilledInState,
                StateFieldName = nameof(DirectDebitDetailsStateVm.FilledInState),
                StateType = nameof(DirectDebitDetailsVm),
                ReturnControllerName = ControllerName,
                ReturnActionName = nameof(DirectDebitContinue)
            };

            return View("PaymentCancel", cancelVm);
        }

        //
        // Private
        //
        private async Task<IActionResult> ProcessPaymentOptionsPost(Guid lowellReferenceSurrogateKey,
            PaymentOptionsVm paymentOptionsVmWithUserEntries)
        {
            if (!ModelState.IsValid)
            {
                var outstandingPayment = paymentOptionsVmWithUserEntries.FullPaymentAmountDerived;

                bool isDirectDebitPayment =
                    paymentOptionsVmWithUserEntries.SelectedPlanSetupOption == PlanSetupOptions.AverageSetupValue ||
                    paymentOptionsVmWithUserEntries.SelectedPlanSetupOption == PlanSetupOptions.DisposableIncome ||
                    paymentOptionsVmWithUserEntries.SelectedPlanSetupOption == PlanSetupOptions.OtherPaymentOffer;
                if (isDirectDebitPayment && paymentOptionsVmWithUserEntries.DirectDebitAmount > outstandingPayment)
                {    
                        ModelState.Clear();
                        paymentOptionsVmWithUserEntries.DirectDebitAmount = null;
                        ModelState.AddModelError(nameof(PaymentOptionsVm.SelectedPlanSetupOption),
                            $"{ValidationMessages.AmountGreaterThenAllowed}{outstandingPayment}");
                }
                else if (!isDirectDebitPayment && paymentOptionsVmWithUserEntries.PartialPaymentAmount.HasValue &&
                         paymentOptionsVmWithUserEntries.PartialPaymentAmount.Value >
                         outstandingPayment)
                {
                    ModelState.Clear();
                    paymentOptionsVmWithUserEntries.PartialPaymentAmount = null;
                    ModelState.AddModelError(nameof(PaymentOptionsVm.PartialPaymentAmount),
                        $"{ValidationMessages.AmountGreaterThenAllowed}{outstandingPayment}");
                }
                else if (!isDirectDebitPayment && paymentOptionsVmWithUserEntries.PartialPaymentAmount.HasValue &&
                         paymentOptionsVmWithUserEntries.PartialPaymentAmount.Value <1.0m
                        )
                {
                    ModelState.Clear();
                    paymentOptionsVmWithUserEntries.PartialPaymentAmount = null;
                    ModelState.AddModelError(nameof(PaymentOptionsVm.PartialPaymentAmount),
                        $"{ValidationMessages.AmountLessThanOneGbp}");
                }

                Debug.Assert(paymentOptionsVmWithUserEntries.InitialState != null, "Initial state must be stashed to re-create view on round-trip");
                return View(paymentOptionsVmWithUserEntries);
            }

            if (paymentOptionsVmWithUserEntries.SelectedPaymentOption ==
                PaymentOptionsSelectionsVm.Values.PartialPayment
                || paymentOptionsVmWithUserEntries.SelectedPaymentOption ==
                PaymentOptionsSelectionsVm.Values.FullPayment)
            {
                OneOffPaymentReviewVm oneOffPaymentReviewVm;
                {
                    //
                    // IMPORTANT: Validates models again versus CaseFlow, to ensure no tamper or state changes since
                    // started process of entering payment information
                    //
                    var result = await _buildOneOffPaymentReviewVmService.ValidateAndBuild(
                        LoggedInUser, ApplicationSessionState, lowellReferenceSurrogateKey,
                        paymentOptionsVmWithUserEntries, GetCaseflowUserId());

                    if (result.IsValid == false)
                    {
                        return View("ValidationFailed");
                    }

                    oneOffPaymentReviewVm = result.OneOffPaymentReviewVm;
                }

                // Stash state of payment options form, so that 'change' feature can return
                oneOffPaymentReviewVm.FilledInPaymentOptionsState =
                    SerialiseModel(paymentOptionsVmWithUserEntries);

                var oneOffPaymentDto = _mapper.Map<OneOffPaymentReviewVm, OneOffPaymentDto>(oneOffPaymentReviewVm);

                _logger.LogDebug("Before AddVerifoneTransactionAsync");

                await _verifonePaymentProviderService.AddVerifoneTransactionAsync(
                    new VerifoneTransactionDto
                    {
                        CompanyCode = 1, //LFL
                        TransactionGuid = oneOffPaymentReviewVm.VerifoneTransactionGuid,
                        TransactionData = JsonConvert.SerializeObject(oneOffPaymentDto)
                    });

                _gtmService.RaiseOneOffPaymentEvent_OptionsSelected(oneOffPaymentReviewVm, LoggedInUserId,
                    "Regular Account");

                await _webActivityService.LogOneOffPaymentSelected(oneOffPaymentReviewVm.LowellReference,
                    LoggedInUserId, !oneOffPaymentReviewVm.PaidInFull, oneOffPaymentReviewVm.DiscountSelected);

                // Ensure no model state is used in view render
                ModelState.Clear();

                ApplicationSessionState.LogPaymentResult = true;
                return View("OneOff", oneOffPaymentReviewVm);
            }

            if (paymentOptionsVmWithUserEntries.SelectedPaymentOption == PaymentOptionsSelectionsVm.Values.DirectDebit)
            {
                // Not validated here - occurs on the final step (DirectDebitComplete)

                var directDebitDetailsVm =
                    _buildDirectDebitDetailsVmService.Build(paymentOptionsVmWithUserEntries);

                // Stash model in state, to allow view to be rebuilt on postback
                directDebitDetailsVm.InitialState = SerialiseModel(directDebitDetailsVm);

                // Stash payment options VM in Direct Debit VM to allow it to be passed back
                // (e.g. if user clicks 'change' and goes back to Payment Options)
                directDebitDetailsVm.PaymentOptionsFilledInState =
                    SerialiseModel(paymentOptionsVmWithUserEntries);

                if (directDebitDetailsVm.DiscountSelected)
                {
                    await LogDiscountDirectDebitOptionsSelected(paymentOptionsVmWithUserEntries);
                }
                else
                {
                    await LogDirectDebitOptionsSelected(paymentOptionsVmWithUserEntries);
                }

                // TODO: change over to building and returning view directly
                _gtmService.RaiseDirectDebitEvent_OptionsSelected(directDebitDetailsVm, LoggedInUserId,
                    "Regular Account");

                ApplicationSessionState.LogSetUpPlanResult = true;

                return View("DirectDebit", directDebitDetailsVm);
            }

            throw new ApplicationException("Invalid SelectedPaymentOption");
        }

        private async Task<IActionResult> ProcessDirectDebitDetailsPost(Guid lowellReferenceSurrogateKey,
            DirectDebitDetailsVm directDebitDetailsVmWithUserEntries)
        {
            if (!ModelState.IsValid)
            {
                Debug.Assert(directDebitDetailsVmWithUserEntries.InitialState != null,
                    "Initial state must be stashed to re-create view on round-trip");
                return View(directDebitDetailsVmWithUserEntries);
            }

            //TODO: Change to async 
            var result =
                await _directDebitPlanSetupService.CheckDirectDebitDetails(directDebitDetailsVmWithUserEntries);

            // Restore Payment Options from state embedded in Direct Debit VM
            PaymentOptionsVm paymentOptionsVm = DeserialiseModel<PaymentOptionsVm>(directDebitDetailsVmWithUserEntries.PaymentOptionsFilledInState);
            Debug.Assert(paymentOptionsVm.InitialState == null, "Serialisation Loop Detected - a previous step must have serialised a model that already contains InitialState");

            if (!result.IsSuccessful)
            {
                directDebitDetailsVmWithUserEntries.MessageForUser = result.MessageForUser;

                if (ApplicationSessionState.LogSetUpPlanResult)
                {
                    if (directDebitDetailsVmWithUserEntries.DiscountSelected)
                    {
                        await LogDiscountDirectDebitCompleteFailure(paymentOptionsVm);
                    }
                    else
                    {
                        await LogDirectDebitCompleteFailure(paymentOptionsVm);
                    }

                    ApplicationSessionState.LogSetUpPlanResult = false;
                }

                return View(directDebitDetailsVmWithUserEntries);
            }

            DirectDebitPlanOverviewVm directDebitPlanOverview = _buildDirectDebitPlanOverviewVmService.Build(paymentOptionsVm, directDebitDetailsVmWithUserEntries);

            // Need to stash direct debit model, so that we can round-trip when 'change' options are selected on confirmation screen. 
            Debug.Assert(!string.IsNullOrEmpty(directDebitDetailsVmWithUserEntries.PaymentOptionsFilledInState),
                "Stashed model MUST contain stashed payment options model, so that it can be round-tripped back to Payment Options if choose to change that");

            directDebitPlanOverview.DirectDebitDetailsFilledInState =
                SerialiseModel(directDebitDetailsVmWithUserEntries);

            _gtmService.RaiseDirectDebitEvent_BankDetails(directDebitPlanOverview, LoggedInUserId, "Direct Debit Plan");
            
            if (directDebitDetailsVmWithUserEntries.DiscountSelected)
            {
                await LogDiscountDirectDebitDetailsEntered(paymentOptionsVm);
            }
            else
            {
                await LogDirectDebitDetailsEntered(paymentOptionsVm);
            }

          

            return View("DirectDebitConfirm", directDebitPlanOverview);
        }

        private async Task LogDirectDebitOptionsSelected(PaymentOptionsVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDirectDebitASVOptionSelected(paymentOptionsVm.LowellReference,
                                 LoggedInUserId);
                            break;

                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDirectDebitPaymentDIOptionSelected(paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDirectDebitMyOfferOptionSelectedWithNoIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogDirectDebitMyOfferOptionSelectedPositiveIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogDirectDebitMyOfferOptionSelectedNegativeIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

        private async Task LogDirectDebitDetailsEntered(PaymentOptionsVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDirectDebitPaymentASVOptionDetailsEntered(paymentOptionsVm.LowellReference,
                                 LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDirectDebitPaymentDIOptionDetailsEntered(paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionDetailsEnteredWithNoIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionDetailsEnteredPositiveIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionDetailsEnteredNegativeIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

        private async Task LogDirectDebitCompleteSuccess(PaymentOptionsVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDirectDebitPaymentASVOptionCompleteSuccess(paymentOptionsVm.LowellReference,
                                 LoggedInUserId);
                            break;

                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDirectDebitPaymentDIOptionCompleteSuccess(paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionCompleteSuccessWithNoIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionCompleteSuccessPositiveIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionCompleteSuccessNegativeIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

        private async Task LogDirectDebitCompleteFailure(PaymentOptionsVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDirectDebitPaymentASVOptionCompleteFailed(paymentOptionsVm.LowellReference,
                                 LoggedInUserId);
                            break;

                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDirectDebitPaymentDIOptionCompleteFailed(paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionCompleteFailedWithNoIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionCompleteFailedPositiveIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionCompleteFailedNegativeIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

        private async Task LogDiscountDirectDebitOptionsSelected(PaymentOptionsVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDiscountDirectDebitASVOptionSelected(paymentOptionsVm.LowellReference,
                                 LoggedInUserId);
                            break;

                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDiscountDirectDebitDIOptionSelected(paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionSelectedWithNoIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionSelectedWithPositiveIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionSelectedWithNegativeIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

        private async Task LogDiscountDirectDebitDetailsEntered(PaymentOptionsVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDiscountDirectDebitPaymentASVOptionDetailsEntered(paymentOptionsVm.LowellReference,
                                 LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDiscountDirectDebitDIOptionDetailsEntered(paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithNoIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithPositiveIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionDetailsEnteredWithNegativeIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

        private async Task LogDiscountDirectDebitCompleteSuccess(PaymentOptionsVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDiscountDirectDebitPaymentASVOptionCompleteSuccess(paymentOptionsVm.LowellReference,
                                 LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDiscountDirectDebitDIOptionCompleteSuccess(paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithNoIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithPositiveIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionCompleteSuccessWithNegativeIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }


        private async Task LogDiscountDirectDebitCompleteFailure(PaymentOptionsVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDiscountDirectDebitPaymentASVOptionCompleteFailed(paymentOptionsVm.LowellReference,
                                 LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDiscountDirectDebitDIOptionCompleteFailed(paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionCompleteFailedWithNoIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionCompleteFailedWithPositiveIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogDiscountDirectDebitMyOfferOptionCompleteFailedWithNegativeIandE(paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

    
    }
}