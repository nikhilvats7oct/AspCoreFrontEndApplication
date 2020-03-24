using AutoMapper;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
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
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    [AllowAnonymous]
    public class RegisterController : BaseController
    {
        private readonly IBuildDataProtectionVmService _buildDataProtectionVm;
        private readonly IGtmService _gtmService;
        private readonly IRegisterService _registerService;
        private readonly IWebActivityService _webActivityService;
        private readonly IMapper _mapper;
        private readonly IContactLinksService _contactLinks;

        public RegisterController(ILogger<BaseController> logger,
            IBuildDataProtectionVmService buildDataProtectionVm,
            IRegisterService registerService,
            IGtmService gtmService,
            IWebActivityService webActionLoggingService,
            IDistributedCache distributedCache,
            IConfiguration configuration,
            IApplicationSessionState sessionState,
            IMapper mapper,
            IContactLinksService contactLinks)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _buildDataProtectionVm = buildDataProtectionVm;
            _registerService = registerService;
            _gtmService = gtmService;
            _webActivityService = webActionLoggingService;
            _mapper = mapper;
            _contactLinks = contactLinks;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _buildDataProtectionVm.Build();
            _gtmService.RaiseRegistrationEvent_ClickedToRegister(model, null);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DataProtectionVm model)
        {
            if (!ModelState.IsValid)
            {
                // List of days, months, years will not have been posted back, therefore repopulate
                _buildDataProtectionVm.PopulateDateComponents(model);
                _gtmService.RaiseRegistrationEvent_AccountDetailsEntered(model, "Invalid account details");

                return View(model);
            }

            var dataProtectionDto = _mapper.Map<DataProtectionVm, DataProtectionDto>(model);
            var dataProtectionResult = await _registerService.CheckDataProtection(dataProtectionDto);

            if (!dataProtectionResult.IsSuccessful)
            {
                AddErrors(dataProtectionResult.MessageForUser);

                model.NotificationMessage = dataProtectionResult.MessageForUser;
                _gtmService.RaiseRegistrationEvent_AccountDetailsEntered(model, "Incorrect account details");
                _buildDataProtectionVm.PopulateDateComponents(model);

                return View(model);
            }

            var webRegisteredDto = _mapper.Map<DataProtectionVm, WebRegisteredDto>(model);
            var isWebRegisteredResult = await _registerService.CheckIsWebRegistered(webRegisteredDto);

            if (!isWebRegisteredResult.IsSuccessful)
            {
                AddErrors(isWebRegisteredResult.MessageForUser);

                model.NotificationMessage = isWebRegisteredResult.MessageForUser;
                _gtmService.RaiseRegistrationEvent_AccountDetailsEntered(model, "Already registered");
                _buildDataProtectionVm.PopulateDateComponents(model);

                return View(model);
            }

            var registrationPending =
                await _registerService.IsPendingRegistration(model.LowellReference);

            if (registrationPending)
            {
                var error = "Account details provided is already registered. Please confirm your email account and login.";

                AddErrors(error);

                model.NotificationMessage = error;

                ModelState.AddModelError(string.Empty,
                    "Account details provided is already registered. Please confirm your email account and login.");

                return View(model);
            }

            ApplicationSessionState.SaveHasPassedDataProtection();

            var registerVm = new RegisterVm { LowellReference = model.LowellReference };
            _gtmService.RaiseRegistrationEvent_AccountDetailsEntered(registerVm, null);

            return View("CompleteRegistration", registerVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteRegistration(RegisterVm model, string returnUrl = "/MyAccounts")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _gtmService.RaiseRegistrationEvent_ActivationEmailSent(model, null, "Invalid registration details");
                    return View(model);
                }

                var result = await _registerService.CreateAccount(new RegisterAccount
                {
                    Password = model.Password,
                    EmailAddress = model.EmailAddress,
                    ExpiresAt = null,
                    LowellReferenceNumber = model.LowellReference
                });

                if (result.IsSuccess)
                {
                    _gtmService.RaiseRegistrationEvent_ActivationEmailSent(model, result.SubjectId.ToString(), null);
                    await _webActivityService.LogRegistrationRequest(model.LowellReference, string.Empty);

                    return View("SendRegistrationEmail", model);
                }
                else
                {
                    Logger.LogError($"Complete Registration Error: {JsonConvert.SerializeObject(result)}");

                    if (result.ErrorCode == 1000)
                    {
                        AddErrors(ValidationMessages.UserNameAlreadyExists);
                        model.NotificationMessage = ValidationMessages.UserNameAlreadyExists;
                        _gtmService.RaiseRegistrationEvent_ActivationEmailSent(model, null, "Username already exists");
                    }
                    else
                    {
                        AddErrors(ValidationMessages.ConfirmRegistrationFailed);
                        model.NotificationMessage = ValidationMessages.ConfirmRegistrationFailed;
                        _gtmService.RaiseRegistrationEvent_ActivationEmailSent(model, null, "service unavailable");
                    }

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Complete Registration Error: {ex.Message}");

                _gtmService.RaiseRegistrationEvent_ActivationEmailSent(model,
                    string.IsNullOrEmpty(LoggedInUserId) ? null : LoggedInUserId, $"Unhandled exception: {ex.Message}");
                return View("Error");
            }
        }

        public async Task<IActionResult> ResendRegistrationEmail(RegisterVm model)
        {
            Logger.LogInformation($"Resend Registration email callback url: {model.CallbackUrl}");

            await _registerService.ResendActivationLink(model.EmailAddress);

            return View("SendRegistrationEmail", model);
        }


        /// <summary>
        /// Caseflow provides link to this action via email.
        /// </summary>
        /// <param name="id">Guid provided generated by caseflow</param>
        /// <returns>Redirects user to registration screen</returns>
        [HttpGet]
        [Route("Register/{id}")]
        public async Task<IActionResult> Register(string id)
        {
            if (!Guid.TryParse(id, out _))
            {
                return View("Views/Errors/NotFound.cshtml");
            }

            await _contactLinks.Update(id);

            return RedirectToAction("Index", "Register");
        }
    }
}