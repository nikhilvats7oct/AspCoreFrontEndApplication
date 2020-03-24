using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
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
    [ExcludeFromCodeCoverage]
    [AllowAnonymous]
    public class DataProtectionController : BaseController
    {
        private readonly IBuildDataProtectionVmService _buildDataProtectionVm;
        private readonly IDataProtectionService _dataProtectionService;
        private readonly IGtmService _gtmService;
        private readonly IWebActivityService _webActivityService;

        public DataProtectionController(ILogger<BaseController> logger,
            IConfiguration configuration,
            IBuildDataProtectionVmService buildDataProtectionVm,
            IDataProtectionService dataProtectionService,
            IGtmService gtmService,
            IWebActivityService webActionLoggingService,
            IDistributedCache distributedCache,
            IApplicationSessionState applicationSessionState) : base(logger, distributedCache, applicationSessionState, configuration)
        {
            _buildDataProtectionVm = buildDataProtectionVm;
            _dataProtectionService = dataProtectionService;
            _gtmService = gtmService;
            _webActivityService = webActionLoggingService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _buildDataProtectionVm.Build();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DataProtectionVm model)
        {
            if (!ModelState.IsValid)
            {
                _buildDataProtectionVm.PopulateDateComponents(model);
                model.NotificationMessage = ValidationMessages.DPAFailure;
                AddErrors(ValidationMessages.DPAFailure);
                _gtmService.RaiseDPACheckEvent(model, LoggedInUserId, "Invalid account details");
                await _webActivityService.LogDPACheckFail(model.LowellReference, LoggedInUserId);

                return View(model);
            }

            var resultDto = await _dataProtectionService.CheckDataProtection(model);

            if (resultDto.IsSuccessful)
            {
                var lowellReferenceSurrogateKey =
                    ApplicationSessionState.AddLowellReferenceSurrogateKey(model.LowellReference);

                ApplicationSessionState.SaveHasPassedDataProtection();

                _gtmService.RaiseDPACheckEvent(model, LoggedInUserId, null);
                await _webActivityService.LogDPACheckSuccess(model.LowellReference, LoggedInUserId);

                TempData["GTMEvents"] = JsonConvert.SerializeObject(model.GtmEvents);

                return RedirectToAction("Index", "PaymentOptions", new {id = lowellReferenceSurrogateKey});
            }

            model.NotificationMessage = resultDto.MessageForUser;
            AddErrors(ValidationMessages.DPAFailure);
            _buildDataProtectionVm.PopulateDateComponents(model);
            _gtmService.RaiseDPACheckEvent(model, LoggedInUserId, "Incorrect account details");
            await _webActivityService.LogDPACheckFail(model.LowellReference, LoggedInUserId);

            return View(model);
        }
    }
}