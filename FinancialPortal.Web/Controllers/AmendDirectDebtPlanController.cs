using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Validation;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Controllers
{
    public class AmendDirectDebtPlanController : BaseController
    {
        private const string ControllerName = "AmendDirectDebtPlan";
        private readonly IAmendDirectDebitPlanService _amendDirectDebitPlanService;
        private readonly IBuildAmendDirectDebitVmService _buildAmendDirectDebitVmService;
        private readonly IGtmService _gtmService;
        private readonly IWebActivityService _webActivityService;

        public AmendDirectDebtPlanController(ILogger<BaseController> logger,
            IConfiguration configuration,
            IBuildAmendDirectDebitVmService buildAmendDirectDebitVmService,
            IAmendDirectDebitPlanService amendDirectDebitPlanService,
            IWebActivityService webActivityService,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IGtmService gtmService) : base(logger, distributedCache, sessionState, configuration)
        {
            _buildAmendDirectDebitVmService = buildAmendDirectDebitVmService;
            _amendDirectDebitPlanService = amendDirectDebitPlanService;
            _webActivityService = webActivityService;
            _gtmService = gtmService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var model = await _buildAmendDirectDebitVmService.Build(ApplicationSessionState, id, GetCaseflowUserId());

            Debug.Assert(model.InitialState == null);
            model.InitialState = SerialiseModel(model);

            await _webActivityService.LogAmendDDPlanSelected(model.LowellReference, LoggedInUserId);
            _gtmService.RaiseAmendDirectDebitEvent_PageVisited(model, LoggedInUserId);
            ApplicationSessionState.LogSetUpPlanResult = true;

            model.SelectedPlanSetupOption = PlanSetupOptions.None;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid id, AmendDirectDebitVm amendDirectDebitVmPostedBack)
        {
            // Restore VM from state field, so that we get lists and all other fields required to build view
            var amendDirectDebitVm = DeserialiseModel<AmendDirectDebitVm>(amendDirectDebitVmPostedBack.InitialState);
            Debug.Assert(amendDirectDebitVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");


            // Place form field selections into restored VM
            _buildAmendDirectDebitVmService.UpdateFieldsFromUserEntries(amendDirectDebitVm,
                amendDirectDebitVmPostedBack);
            await LogAmendDirectDebitOptionsSelected(amendDirectDebitVm);
            // Carry state through
            amendDirectDebitVm.InitialState = amendDirectDebitVmPostedBack.InitialState;
            ApplicationSessionState.LogSetUpPlanResult = true;

            await LogAmendDirectDebitDetailsEntered(amendDirectDebitVm);

            return await ProcessPost(id, amendDirectDebitVm);
        }

        [HttpPost]
        public async Task<IActionResult> Complete(Guid id, AmendDirectDebitConfirmVm amendDirectDebitCompleteVm)
        {
            // Take from cached form state
            var amendDirectDebitVm = DeserialiseModel<AmendDirectDebitVm>(amendDirectDebitCompleteVm.FilledInState);

            // Perform final validation and amend
            var result = await _amendDirectDebitPlanService.AmendDirectDebitPlan(
                LoggedInUser, ApplicationSessionState, id, amendDirectDebitVm, GetCaseflowUserId());

            if (!result)
            {
                await LogAmendDirectDebitCompleteFailure(amendDirectDebitVm);

                return View("ValidationFailed");
            }

            await LogAmendDirectDebitCompleteSuccess(amendDirectDebitVm);

            _gtmService.RaiseAmendDirectDebitEvent_Confirmed(amendDirectDebitVm, LoggedInUserId);
            ApplicationSessionState.LogSetUpPlanResult = false;


            return View(amendDirectDebitVm);
        }

        [HttpPost]
        public IActionResult PaymentMethods(Guid id, AmendDirectDebitVm amendDirectDebitVmPostedBack)
        {
            // Restore VM from state field, so that we get lists and all other fields required to build view
            var amendDirectDebitVm = DeserialiseModel<AmendDirectDebitVm>(amendDirectDebitVmPostedBack.InitialState);
            Debug.Assert(amendDirectDebitVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Carry state through
            amendDirectDebitVm.InitialState = amendDirectDebitVmPostedBack.InitialState;

            // Place form field selections into restored VM
            _buildAmendDirectDebitVmService.UpdateFieldsFromUserEntries(amendDirectDebitVm,
                amendDirectDebitVmPostedBack);

            // Construct model for Cancel view
            var returnToFormVm = new ReturnToFormVm
            {
                State = SerialiseModel(amendDirectDebitVm),
                StateFieldName = nameof(AmendDirectDebitStateVm.FilledInState),
                StateType = nameof(AmendDirectDebitVm),
                ReturnControllerName = ControllerName,
                ReturnActionName = nameof(Change)
            };

            return View("PaymentMethods", returnToFormVm);
        }

        [HttpPost]
        public IActionResult Change(Guid id, AmendDirectDebitStateVm amendDirectDebitStateVm)
        {
            // Restore VM from state field, so that form is as it was before moving on to next step
            var amendDirectDebitVm = DeserialiseModel<AmendDirectDebitVm>(amendDirectDebitStateVm.FilledInState);
            Debug.Assert(amendDirectDebitVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Save state for round-trip (to re-populate lists, messages etc)
            amendDirectDebitVm.InitialState = amendDirectDebitStateVm.FilledInState;

            return View("Index", amendDirectDebitVm);
        }


        [HttpPost]
        public async Task<IActionResult> Continue(Guid id, AmendDirectDebitStateVm amendDirectDebitStateVm)
        {
            // Restore VM from state field, so that form is as it was before moving on to next step
            var amendDirectDebitVm = DeserialiseModel<AmendDirectDebitVm>(amendDirectDebitStateVm.FilledInState);
            Debug.Assert(amendDirectDebitVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Process as if user posted it
            return await ProcessPost(id, amendDirectDebitVm);
        }

        [HttpPost]
        public IActionResult Cancel(Guid id, AmendDirectDebitVm amendDirectDebitVmPostedBack)
        {
            // Restore VM from state field, so that we get lists and all other fields required to build view
            var amendDirectDebitVm = DeserialiseModel<AmendDirectDebitVm>(amendDirectDebitVmPostedBack.InitialState);
            Debug.Assert(amendDirectDebitVm.InitialState == null,
                "Serialisation Loop Detected - InitialState should be marked as JsonIgnore");

            // Carry state through
            amendDirectDebitVm.InitialState = amendDirectDebitVmPostedBack.InitialState;

            // Place form field selections into restored VM
            _buildAmendDirectDebitVmService.UpdateFieldsFromUserEntries(amendDirectDebitVm,
                amendDirectDebitVmPostedBack);

            // Construct model for Cancel view
            var cancelVm = new ReturnToFormVm
            {
                State = SerialiseModel(amendDirectDebitVm),
                StateFieldName = nameof(AmendDirectDebitStateVm.FilledInState),
                StateType = nameof(AmendDirectDebitVm),
                ReturnControllerName = ControllerName,
                ReturnActionName = nameof(Change)
            };

            return View("PaymentCancel", cancelVm);
        }

        [HttpPost]
        public IActionResult ConfirmCancel(Guid id, AmendDirectDebitStateVm amendDirectDebitStateVm)
        {
            // Construct model for Cancel view
            var cancelVm = new ReturnToFormVm
            {
                State = amendDirectDebitStateVm.FilledInState,
                StateFieldName = nameof(AmendDirectDebitStateVm.FilledInState),
                StateType = nameof(AmendDirectDebitVm),
                ReturnControllerName = ControllerName,
                ReturnActionName = nameof(Continue)
            };

            return View("PaymentCancel", cancelVm);
        }

        //
        // Private
        //
        private async Task<IActionResult> ProcessPost(Guid id, AmendDirectDebitVm amendDirectDebitVmPostedBack)
        {
            if (!ModelState.IsValid)
            {
                if (amendDirectDebitVmPostedBack.SelectedPlanSetupOption == PlanSetupOptions.AverageSetupValue ||
                    amendDirectDebitVmPostedBack.SelectedPlanSetupOption == PlanSetupOptions.DisposableIncome)
                {
                    if (amendDirectDebitVmPostedBack.DirectDebitAmount >
                        amendDirectDebitVmPostedBack.OutstandingBalance)
                    {
                        ModelState.Clear();
                        var initialVm = DeserialiseModel<AmendDirectDebitVm>(amendDirectDebitVmPostedBack.InitialState);
                        amendDirectDebitVmPostedBack.DirectDebitAmount = initialVm.DirectDebitAmount;
                        ModelState.AddModelError(nameof(AmendDirectDebitVm.SelectedPlanSetupOption),
                            $"{ValidationMessages.AmountGreaterThenAllowed}{amendDirectDebitVmPostedBack.OutstandingBalance}");
                    }
                }

                Debug.Assert(amendDirectDebitVmPostedBack.InitialState != null,
                    "Initial state must be stashed to re-create view on round-trip");
                return View(amendDirectDebitVmPostedBack);
            }

            _gtmService.RaiseAmendDirectDebitEvent_DetailsSelected(amendDirectDebitVmPostedBack, LoggedInUserId);
            ApplicationSessionState.LogSetUpPlanResult = true;
            
            var totalMonths = _amendDirectDebitPlanService.CalculateTermInMonths(
                amendDirectDebitVmPostedBack.DerivedBalance, amendDirectDebitVmPostedBack.DirectDebitAmount.Value,
                amendDirectDebitVmPostedBack.PlanFrequency);
            var years = totalMonths / 12;
            var months = totalMonths % 12;

            var amendDirectDebitConfirmVm = new AmendDirectDebitConfirmVm
            {
                // Stash filled in form, as other fields will not be in inputs on confirm page
                FilledInState = SerialiseModel(amendDirectDebitVmPostedBack),

                LowellReference = amendDirectDebitVmPostedBack.LowellReference,
                ClientName = amendDirectDebitVmPostedBack.ClientName,
                OutstandingBalance = amendDirectDebitVmPostedBack.OutstandingBalance,
                RegularAccountPaymentAmount = amendDirectDebitVmPostedBack.DirectDebitAmount.Value,
                PlanFrequency = amendDirectDebitVmPostedBack.PlanFrequency,
                PlanStartDate = amendDirectDebitVmPostedBack.PlanStartDate,
                TermYears = years,
                TermMonths = months
            };

            return View("Confirm", amendDirectDebitConfirmVm);
        }

        private async Task LogAmendDirectDebitOptionsSelected(AmendDirectDebitVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                    {
                        await _webActivityService.LogAmendDirectDebitASVOptionSelected(paymentOptionsVm.LowellReference,
                            LoggedInUserId);
                        break;
                    }
                    case PlanSetupOptions.DisposableIncome:
                    {
                        await _webActivityService.LogAmendDirectDebitDIOptionSelected(paymentOptionsVm.LowellReference,
                            LoggedInUserId);
                        break;
                    }
                    case PlanSetupOptions.OtherPaymentOffer:
                    {
                        if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionSelectedWithNoIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }
                        else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionSelectedWithPositiveIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }
                        else
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionSelectedWithNegativeIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }

                        break;
                    }
                }
            }
        }

        private async Task LogAmendDirectDebitDetailsEntered(AmendDirectDebitVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                    {
                        await _webActivityService.LogAmendDirectDebitPaymentASVOptionDetailsEntered(
                            paymentOptionsVm.LowellReference,
                            LoggedInUserId);
                        break;
                    }
                    case PlanSetupOptions.DisposableIncome:
                    {
                        await _webActivityService.LogAmendDirectDebitDIOptionDetailsEntered(
                            paymentOptionsVm.LowellReference,
                            LoggedInUserId);
                        break;
                    }
                    case PlanSetupOptions.OtherPaymentOffer:
                    {
                        if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionDetailsEnteredWithNoIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }
                        else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionDetailsEnteredWithPositiveIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }
                        else
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionDetailsEnteredWithNegativeIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }

                        break;
                    }
                }
            }
        }

        private async Task LogAmendDirectDebitCompleteSuccess(AmendDirectDebitVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                    {
                        await _webActivityService.LogAmendDirectDebitPaymentASVOptionCompleteSuccess(
                            paymentOptionsVm.LowellReference,
                            LoggedInUserId);
                        break;
                    }
                    case PlanSetupOptions.DisposableIncome:
                    {
                        await _webActivityService.LogAmendDirectDebitDIOptionCompleteSuccess(
                            paymentOptionsVm.LowellReference,
                            LoggedInUserId);
                        break;
                    }
                    case PlanSetupOptions.OtherPaymentOffer:
                    {
                        if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteSuccessWithNoIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }
                        else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteSuccessWithPositiveIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }
                        else
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteSuccessWithNegativeIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }

                        break;
                    }
                }
            }
        }

        private async Task LogAmendDirectDebitCompleteFailure(AmendDirectDebitVm paymentOptionsVm)
        {
            if (paymentOptionsVm.SelectedPlanSetupOption.HasValue)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                    {
                        await _webActivityService.LogAmendDirectDebitPaymentASVOptionCompleteFailed(
                            paymentOptionsVm.LowellReference,
                            LoggedInUserId);
                        break;
                    }
                    case PlanSetupOptions.DisposableIncome:
                    {
                        await _webActivityService.LogAmendDirectDebitDIOptionCompleteFailed(
                            paymentOptionsVm.LowellReference,
                            LoggedInUserId);
                        break;
                    }
                    case PlanSetupOptions.OtherPaymentOffer:
                    {
                        if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteFailedWithNoIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }
                        else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteFailedWithPositiveIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }
                        else
                        {
                            await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteFailedWithNegativeIandE(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                        }

                        break;
                    }
                }
            }
        }
    }
}