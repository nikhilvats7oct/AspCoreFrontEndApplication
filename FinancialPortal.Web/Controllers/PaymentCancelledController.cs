using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FinancialPortal.Web.Controllers
{
    [AllowAnonymous]
    public class PaymentCancelledController : BaseController
    {
        private readonly IWebActivityService _webActivityService;

        public PaymentCancelledController(
            ILogger<BaseController> logger,
            IWebActivityService webActivityService,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IConfiguration configuration)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _webActivityService = webActivityService;
        }

        public async Task<ActionResult> Index(PaymentCancelledDto dto)
        {
            if (dto.PaymentStateType == nameof(PaymentOptionsVm))
            {
                PaymentOptionsVm vm = DeserialiseModel<PaymentOptionsVm>(dto.PaymentState);

                if (ApplicationSessionState.LogPaymentResult)
                {
                    await _webActivityService.LogOneOffPaymentCancelled(vm.LowellReference, LoggedInUserId, vm.SelectedPaymentOption == "partial-payment", vm.DiscountAccepted);
                    ApplicationSessionState.LogPaymentResult = false;
                }
            }

            if (dto.PaymentStateType == nameof(DirectDebitDetailsVm))
            {
                var vm = DeserialiseModel<DirectDebitDetailsVm>(dto.PaymentState);

                if (ApplicationSessionState.LogSetUpPlanResult)
                {
                    if (vm.DiscountSelected)
                    {
                        await LogDiscountDirectDebitCancelled(vm);
                    }
                    else
                    {
                        await LogDirectDebitCancelled(vm);
                    }

                    ApplicationSessionState.LogSetUpPlanResult = false;
                }
            }

            if (dto.PaymentStateType == nameof(AmendDirectDebitVm))
            {
                var vm = DeserialiseModel<AmendDirectDebitVm>(dto.PaymentState);

                if (ApplicationSessionState.LogSetUpPlanResult)
                {
                    await LogAmendDirectDebitCancelled(vm);

                    ApplicationSessionState.LogSetUpPlanResult = false;
                }
            }

            return View();
        }

        private async Task LogDirectDebitCancelled(DirectDebitDetailsVm directDebitDetailsVm)
        {
            var paymentOptionsVm = ConvertToPaymentOptionsVm(directDebitDetailsVm.PaymentOptionsFilledInState);

            if (paymentOptionsVm?.SelectedPlanSetupOption != null)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDirectDebitPaymentASVOptionCompleteCancelled(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }

                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDirectDebitPaymentDIOptionCompleteCancelled(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }

                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogDirectDebitPaymentMyOfferOptionCompleteCancelledWithNoIandE(
                                    paymentOptionsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService
                                    .LogDirectDebitPaymentMyOfferOptionCompleteCancelledPositiveIandE(
                                        paymentOptionsVm.LowellReference,
                                        LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService
                                    .LogDirectDebitPaymentMyOfferOptionCompleteCancelledNegativeIandE(
                                        paymentOptionsVm.LowellReference,
                                        LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

        private async Task LogDiscountDirectDebitCancelled(DirectDebitDetailsVm directDebitDetailsVm)
        {
            var paymentOptionsVm = ConvertToPaymentOptionsVm(directDebitDetailsVm.PaymentOptionsFilledInState);

            if (paymentOptionsVm?.SelectedPlanSetupOption != null)
            {
                switch (paymentOptionsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogDiscountDirectDebitPaymentASVOptionCompleteCancelled(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }

                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogDiscountDirectDebitDIOptionCompleteCancelled(
                                paymentOptionsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }

                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (paymentOptionsVm.IandENotAvailable || !paymentOptionsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService
                                    .LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithNoIandE(
                                        paymentOptionsVm.LowellReference,
                                        LoggedInUserId);
                            }
                            else if (paymentOptionsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService
                                    .LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithPositiveIandE(
                                        paymentOptionsVm.LowellReference,
                                        LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService
                                    .LogDiscountDirectDebitMyOfferOptionCompleteCancelledWithNegativeIandE(
                                        paymentOptionsVm.LowellReference,
                                        LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }


        private async Task LogAmendDirectDebitCancelled(AmendDirectDebitVm directDebitDetailsVm)
        {
            if (directDebitDetailsVm?.SelectedPlanSetupOption != null)
            {
                switch (directDebitDetailsVm.SelectedPlanSetupOption.Value)
                {
                    case PlanSetupOptions.AverageSetupValue:
                        {
                            await _webActivityService.LogAmendDirectDebitPaymentASVOptionCompleteCancelled(directDebitDetailsVm.LowellReference,
                                 LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.DisposableIncome:
                        {
                            await _webActivityService.LogAmendDirectDebitDIOptionCompleteCancelled(directDebitDetailsVm.LowellReference,
                                LoggedInUserId);
                            break;
                        }
                    case PlanSetupOptions.OtherPaymentOffer:
                        {
                            if (directDebitDetailsVm.IandENotAvailable || !directDebitDetailsVm.IandELessThanOrIs12MonthsOld)
                            {
                                await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteCancelledWithNoIandE(directDebitDetailsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else if (directDebitDetailsVm.MonthlyDisposableIncome > 0)
                            {
                                await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteCancelledWithPositiveIandE(directDebitDetailsVm.LowellReference,
                                    LoggedInUserId);
                            }
                            else
                            {
                                await _webActivityService.LogAmendDirectDebitMyOfferOptionCompleteCancelledWithNegativeIandE(directDebitDetailsVm.LowellReference,
                                    LoggedInUserId);
                            }

                            break;
                        }
                }
            }
        }

        private PaymentOptionsVm ConvertToPaymentOptionsVm(string seralisedState)
        {
            try
            {
                PaymentOptionsVm paymentOptionsVm = DeserialiseModel<PaymentOptionsVm>(seralisedState);

                return paymentOptionsVm;
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to convert to PaymentVm. Exception details - {e.Message}");
            }

            return null;

        }
    }
}