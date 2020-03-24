using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Interfaces.ViewModelBuilders;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FinancialPortal.Web.Controllers
{
    [AllowAnonymous]
    public class ContactUsController : BaseController
    {
        private readonly IBuildContactUsVmService _contactUsVmService;
        private readonly IContactUsService _contactUsService;

        public ContactUsController(IBuildContactUsVmService contactUsVmService,
            IContactUsService contactUsService,
            IDistributedCache distributedCache,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IApplicationSessionState sessionState,
            ILogger<ContactUsController> logger) :base(logger, distributedCache, sessionState, configuration)
        {
            _contactUsVmService = contactUsVmService;
            _contactUsService = contactUsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var contactUsVm = _contactUsVmService.CreateNewContactUsVm();

            return View(contactUsVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMessage(ContactUsVm model)
        {
            if (!ModelState.IsValid)
            {
                Logger.LogInformation("Modelstate is NOT valid. Return data back to Contact view.");
            }

            else
            {
                var result = await _contactUsService.SendContactUsMessage(model);

                if (result.IsSuccessful)
                {
                    model.HasMessageBeenSentSuccessfully = true;
                }

                model.HasMessageBeenSentSuccessfully = true;
            }
            
            return View("Index", model);
        }
    }
}