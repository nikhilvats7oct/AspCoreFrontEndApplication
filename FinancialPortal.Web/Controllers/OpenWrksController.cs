using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Middleware;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Controllers
{
    public class OpenWrksController : BaseController
    {
        private readonly IAccountsService _accountService;
        private readonly IBudgetCalculatorService _budgetCalculatorService;
        private readonly IPortalCryptoAlgorithm _cryptoAlgorithm;
        private readonly IOpenWrksService _openWrkService;
        private readonly OpenWrksSetting _openWrksSetting;
        private readonly PortalSetting _portalSetting;
        private readonly IWebActivityService _webActivityService;

        public OpenWrksController(
            ILogger<OpenWrksController> logger,
            IDistributedCache distributedCache,
            IConfiguration configuration,
            OpenWrksSetting openWrksSetting,
            IApplicationSessionState sessionState,
            IWebActivityService webActivityService,
            IAccountsService accountsService,
            PortalSetting portalSetting,
            IBudgetCalculatorService budgetCalculatorService,
            IPortalCryptoAlgorithm cryptoAlgorithm,
            IOpenWrksService openWrkService) :
            base(logger, distributedCache, sessionState, configuration)
        {
            _openWrksSetting = openWrksSetting;
            _openWrkService = openWrkService;
            _webActivityService = webActivityService;
            _budgetCalculatorService = budgetCalculatorService;
            _cryptoAlgorithm = cryptoAlgorithm;
            _accountService = accountsService;
            _portalSetting = portalSetting;
        }

        public async Task<IActionResult> Index()
        {
            if (!_portalSetting.Features.EnableOpenWrks)
            {
                return NotFound();
            }

            var caseflowUserId = GetCaseflowUserId();


            var lowellReference = ApplicationSessionState.GetTopLowellReference();

            if (!string.IsNullOrWhiteSpace(caseflowUserId) && !string.IsNullOrWhiteSpace(lowellReference))
            {
                var newInvitationRequest = new OpenWrksInvitationRequest
                {
                    CustomerReference = GenerateOpenWrksCustomerReference(),
                    JourneyEndRedirectUrl =
                        $"{Request.Scheme}://{Request.Host}{Request.PathBase}/OpenWrks/BudgetComplete"
                };

                var result = await _openWrkService.SendInvitationRequest(newInvitationRequest);

                await _webActivityService.LogOpenBankingAccessed(lowellReference, caseflowUserId);

                return Redirect(result.BudgetUrl);
            }

            throw new Exception(
                "Failed to get Invitation Link. User must log on to use OpenWorks Budget Calculator. ");
        }

        [HttpGet]
        public async Task<IActionResult> BudgetComplete(
            [FromQuery(Name = "customerReference")]
            string base64EncodedEncryptedCustomerReference,
            [FromQuery(Name = "errorCode")] string errorCode)
        {
            if (!_portalSetting.Features.EnableOpenWrks)
            {
                return NotFound();
            }

            var lowellSurrogateKey = ApplicationSessionState.GetTopLowellSurrogateKey();
            var lowellReference = ApplicationSessionState.GetTopLowellReference();

            if (lowellSurrogateKey == null || lowellSurrogateKey == Guid.Empty)
            {
                // At this point it looks like the Users's session has expired and was forced to re-login. 
                // As a result all the surrogatekeys will be empty the following lines will save surrogate key to session again 
                // then we will simply get the top key.

                var accountSummaries = await _accountService.GetAccounts(GetCaseflowUserId());
                var lowellReferences = accountSummaries.Select(x => x.AccountReference).ToList();
                var surrogateKeysByLowellReference =
                    ApplicationSessionState.AddLowellReferenceSurrogateKeys(lowellReferences);
                lowellSurrogateKey = ApplicationSessionState.GetTopLowellSurrogateKey();
                lowellReference = ApplicationSessionState.GetTopLowellReference();
            }

            var occurrenceId = Request.Headers.ContainsKey(TraceMiddleware.TraceHeaderName)
                ? Request.Headers[TraceMiddleware.TraceHeaderName].ToString()
                : "";

            var errorModel = new OpenWrksErrorVm
            {
                LowellFinancialAccountSurrogateKey = lowellSurrogateKey,
                OccurrenceId = occurrenceId
            };

            var caseflowUserId = GetCaseflowUserId();

            var decryptedCustomerReference = "";
            try
            {
                var base64DecodedEncryptedCustomerRefAsString = GetBase64DecodeString(base64EncodedEncryptedCustomerReference);
              
                decryptedCustomerReference =
                    _cryptoAlgorithm.DecryptUsingAes(base64DecodedEncryptedCustomerRefAsString);
            }
            catch (Exception e)
            {
                Logger.LogError(e,
                    $"Unable to decode and decrypt the customerReference provided by OpenWrks. customerReference - {base64EncodedEncryptedCustomerReference}.");

                await _webActivityService.LogOpenBankingError(lowellReference, GetCaseflowUserId());

                return View("Error", errorModel);
            }

            if (string.IsNullOrEmpty(errorCode) &&
                string.Equals(decryptedCustomerReference, caseflowUserId, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(lowellReference))
                {
                    var model = new OpenWrksSuccessVm
                    {
                        LowellFinancialAccountSurrogateKey = lowellSurrogateKey,
                        OccurrenceId = occurrenceId
                    };
                    if (_openWrksSetting.UseLandingPage)
                    {
                        return View("Success", model);
                    }


                    Logger.LogInformation("Fetching translated budget from API GW");
                    var customerReference = GenerateOpenWrksCustomerReference();
                    var budget =
                        await _openWrkService.GetOpenWorksBudgetTranslatedToCaseflowBudgetModel(
                            base64EncodedEncryptedCustomerReference);

                    Logger.LogInformation("Saving budget to Distributed Cache.");
                    ApplicationSessionState.SaveIncomeAndExpenditure(
                        budget, model.LowellFinancialAccountSurrogateKey.Value);

                    try
                    {
                        Logger.LogInformation("Saving budget to Caseflow.");
                        await _budgetCalculatorService.SaveIncomeAndExpenditure(budget, lowellReference);

                        await _webActivityService.LogOpenBankingComplete(lowellReference, GetCaseflowUserId());

                     
                        return RedirectToAction(
                            "BudgetSummary",
                            "BudgetCalculator",
                            new { id = model.LowellFinancialAccountSurrogateKey });
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex,
                            $"Saving OpenBanking budget to Caseflow has returned error with code - {errorCode}.");
                        await _webActivityService.LogOpenBankingError(lowellReference, GetCaseflowUserId());

                        return View("Error", errorModel);
                    }
                }

                Logger.LogWarning("Account reference number is not found.");
            }
            else if (!string.IsNullOrEmpty(errorCode))
            {
                Logger.LogError($"OpenBanking has returned error with code - {errorCode}.");
            }
            else
            {
                Logger.LogError(
                    $"OpenBanking has returned invalid customer reference in BudgetComplete notification. Customer Reference {base64EncodedEncryptedCustomerReference} is not matched with {GetCaseflowUserId()}");
            }

            await _webActivityService.LogOpenBankingError(lowellReference, GetCaseflowUserId());

            return View("Error", errorModel);
        }

        public IActionResult Error(string occurrenceId)
        {
            if (!_portalSetting.Features.EnableOpenWrks)
            {
                return NotFound();
            }

            occurrenceId ??= Request.Headers.ContainsKey(TraceMiddleware.TraceHeaderName)
                ? Request.Headers[TraceMiddleware.TraceHeaderName].ToString()
                : "";

            var errorModel = new OpenWrksErrorVm
            {
                LowellFinancialAccountSurrogateKey = ApplicationSessionState.GetTopLowellSurrogateKey(),
                OccurrenceId = occurrenceId
            };

            return View(errorModel);
        }

        public async Task<IActionResult> SaveBudget()
        {
            if (!_portalSetting.Features.EnableOpenWrks)
            {
                return NotFound();
            }

            var lowellSurrogateKey = ApplicationSessionState.GetTopLowellSurrogateKey();
            var lowellReference = ApplicationSessionState.GetTopLowellReference();
            var occurrenceId = Request.Headers.ContainsKey(TraceMiddleware.TraceHeaderName)
                ? Request.Headers[TraceMiddleware.TraceHeaderName].ToString()
                : "";

            try
            {
                Logger.LogInformation("Fetching translated budget from OpenWorks");
                var customerReference = GenerateOpenWrksCustomerReference();
                var budget = await _openWrkService.GetOpenWorksBudgetTranslatedToCaseflowBudgetModel(customerReference);

                if (budget != null)
                {
                    Logger.LogInformation("Saving budget to Distributed Cache.");
                    ApplicationSessionState.SaveIncomeAndExpenditure(budget, lowellSurrogateKey.Value);

                    if (budget.IncomeTotal > 0)
                    {
                        Logger.LogInformation("Saving budget to Caseflow");
                        await _budgetCalculatorService.SaveIncomeAndExpenditure(budget, lowellReference);
                    }
                    else
                    {
                        Logger.LogWarning("NOT saving budget to Caseflow as income is less than or equal to ZERO");
                    }

                    await _webActivityService.LogOpenBankingComplete(lowellReference, GetCaseflowUserId());

                    return Json(new { success = true });
                }

                Logger.LogError("Budget not found in OpenWrks.");

                return Json(new { success = false, message = "Budget not found in OpenWrks.", occurrenceId });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to save Budget from OpenWorks");

                return Json(new { success = false, message = "Failed to save Budget from OpenWorks", occurrenceId });
            }
        }

        private string GenerateOpenWrksCustomerReference()
        {
            char[] padding = { '=' };
            var encrypted = _cryptoAlgorithm.EncryptUsingAes(GetCaseflowUserId());
            var plainTextBytes = Encoding.UTF8.GetBytes(encrypted);
            return Convert.ToBase64String(plainTextBytes).TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }

        private string GetBase64DecodeString(string input)
        {
            string incoming = input
        .Replace('_', '/').Replace('-', '+');
            switch (input.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            byte[] bytes = Convert.FromBase64String(incoming);
            string originalText = Encoding.UTF8.GetString(bytes);

            return originalText;
        }
    }
}