using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FinancialPortal.Web.Controllers
{
    [AllowAnonymous]
    public class CallbackController : BaseController
    {
        private readonly IBuildCallbackVmService _callbackVmService;
        private readonly ICallbackService _callbackService;

        public CallbackController(IBuildCallbackVmService callbackVmService,
             ICallbackService callbackService,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            ILogger<CallbackController> logger) : base(logger, distributedCache, sessionState, configuration)
        {
            _callbackVmService = callbackVmService;
            _callbackService = callbackService;
        }

        [HttpGet]        
        public ActionResult Index()
        {
            var callbackVm = _callbackVmService.CreateNewCallbackVm();

            return View(callbackVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCallback(CallbackVm model)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation("Modelstate is NOT valid. Return data back to Callback view.");
            }

            else
            {
                var result = await _callbackService.SendCallbackMessage(model);

                if (result.IsSuccessful)
                {
                    model.HasCallbackRequestBeenSentSuccessfully = true;
                }

                model.HasCallbackRequestBeenSentSuccessfully = true;
            }

            return View("Index", model);
        }

    }
}