using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
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
    public class OneOffPaymentResultController : BaseController
    {
        private readonly IGtmService _gtmService;
        private readonly IPaymentService _paymentService;
        private readonly IVerifonePaymentProviderService _verifonePaymentProviderService;
        private readonly IWebActivityService _webActivityService;

        public OneOffPaymentResultController(ILogger<BaseController> logger,
            IConfiguration configuration,
            IPaymentService paymentService,
            IVerifonePaymentProviderService verifonePaymentProviderService,
            IGtmService gtmService,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IWebActivityService webActivityService)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _paymentService = paymentService;
            _verifonePaymentProviderService = verifonePaymentProviderService;
            _gtmService = gtmService;
            _webActivityService = webActivityService;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "ref")] string transactionGuid,
            [FromQuery] string result,
            [FromQuery] string tokenId,
            [FromQuery(Name = "acode")] string authrorisationCode)
        {
            try
            {
                var verifoneTransactionDto =
                    await _verifonePaymentProviderService.GetVerifoneTransactionAsync(transactionGuid);

                //Transaction not exists in web or return url tampered
                if (string.IsNullOrEmpty(verifoneTransactionDto.TransactionData))
                {
                    Logger.LogWarning($"Payment transaction { transactionGuid} and this request looks like not exists in webpayment and will be ignored. If this need be actioned for any reason use auth code { authrorisationCode} token { tokenId}");

                    return View("Failed", new PaymentResultVm { Reference = transactionGuid, Result = result, TokenId = tokenId, ACode = authrorisationCode });
                }

                var oneOffPaymentDto =
                    JsonConvert.DeserializeObject<OneOffPaymentDto>(verifoneTransactionDto.TransactionData);

                var model = new PaymentResultVm
                {
                    Reference = transactionGuid,
                    Result = result,
                    TokenId = tokenId,
                    ACode = authrorisationCode,
                    PaymentInfo = oneOffPaymentDto
                };

                //Transaction already processed or page got refreshed
                if (verifoneTransactionDto.Status != 0)
                {
                    if (ApplicationSessionState.LogPaymentResult)
                    {
                        _gtmService.RaiseOneOffPaymentEvent_PaymentFailed(model, LoggedInUserId, "Regular Account");
                        await _webActivityService.LogOneOffPaymentFailure(model.PaymentInfo.LowellReference, LoggedInUserId, !model.PaymentInfo.PaidInFull, model.PaymentInfo.DiscountSelected);
                        ApplicationSessionState.LogPaymentResult = false;
                    }

                    Logger.LogWarning($"Payment transaction { transactionGuid} is in status { verifoneTransactionDto.Status} and this request looks like a duplicate and will be ignored. If this need be actioned for any reason use auth code { authrorisationCode} token { tokenId}");

                    return View("Failed", model);
                }

                //set transaction details
                verifoneTransactionDto.TransactionGuid = transactionGuid;
                verifoneTransactionDto.Result = result;
                verifoneTransactionDto.TokenId = tokenId;
                verifoneTransactionDto.AuthorisationCode = authrorisationCode;

                if (model.Result == "CANCELLED")
                {
                    await _verifonePaymentProviderService.UpdateVerifoneTransactionAsync(verifoneTransactionDto);

                    if (ApplicationSessionState.LogPaymentResult)
                    {
                        _gtmService.RaiseOneOffPaymentEvent_PaymentCancelled(model, LoggedInUserId, "Regular Account");
                        await _webActivityService.LogOneOffPaymentCancelled(model.PaymentInfo.LowellReference, LoggedInUserId, !model.PaymentInfo.PaidInFull, model.PaymentInfo.DiscountSelected);
                        ApplicationSessionState.LogPaymentResult = false;
                    }

                    return View("Cancelled", model);
                }

                if (model.Result == "FAILED")
                {
                    await _verifonePaymentProviderService.UpdateVerifoneTransactionAsync(verifoneTransactionDto);

                    if (ApplicationSessionState.LogPaymentResult)
                    {
                        _gtmService.RaiseOneOffPaymentEvent_PaymentFailed(model, LoggedInUserId, "Regular Account");
                        await _webActivityService.LogOneOffPaymentFailure(model.PaymentInfo.LowellReference, LoggedInUserId, !model.PaymentInfo.PaidInFull, model.PaymentInfo.DiscountSelected);
                        ApplicationSessionState.LogPaymentResult = false;
                    }

                    return View("Failed", model);
                }

                if (model.Result == "SUCCESS")
                {
                    var successfulOneOffPaymentVm = new SuccessfulOneOffPaymentVm
                    {
                        ClientName = oneOffPaymentDto.ClientName,
                        PaymentInfo = model.PaymentInfo,
                        UserLoggedIn = !string.IsNullOrEmpty(LoggedInUserId)
                    };

                    await _paymentService.MakePayment(model, oneOffPaymentDto);

                    await _verifonePaymentProviderService.UpdateVerifoneTransactionAsync(verifoneTransactionDto);

                    if (ApplicationSessionState.LogPaymentResult)
                    {
                        _gtmService.RaiseOneOffPaymentEvent_PaymentComplete(successfulOneOffPaymentVm, LoggedInUserId, "Regular Account");
                        await _webActivityService.LogOneOffPaymentComplete(model.PaymentInfo.LowellReference, LoggedInUserId, !model.PaymentInfo.PaidInFull, model.PaymentInfo.DiscountSelected);
                        ApplicationSessionState.LogPaymentResult = false;
                    }

                    return View("Success", successfulOneOffPaymentVm);
                }

                return View("Error");
            }
            catch (Exception ex)
            {
                Logger.LogError("One off payment error", ex);
                return View("Error");
            }
        }
    }
}