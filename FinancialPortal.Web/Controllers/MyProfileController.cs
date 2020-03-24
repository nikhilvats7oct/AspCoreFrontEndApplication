using FinancialPortal.Web.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.Web.Controllers
{
    [Authorize]
    public class MyProfileController : BaseController
    {
        private readonly IMyProfileService _myProfileService;
        private readonly IGtmService _gtmService;
        private readonly IWebActivityService _webActivityService;
        private readonly IAccountsService _accountsService;
        private readonly PortalSetting _portalSetting;

        public MyProfileController(ILogger<BaseController> logger,
                                   IConfiguration configuration,
                                   IMyProfileService myProfileService,
                                   IGtmService gtmService,
                                   IDistributedCache distributedCache,
                                   IApplicationSessionState sessionState,
                                   IWebActivityService webActivityService,
                                   IAccountsService accountsService,
                                    PortalSetting portalSetting)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _myProfileService = myProfileService;
            _gtmService = gtmService;
            _webActivityService = webActivityService;
            _accountsService = accountsService;
            _portalSetting = portalSetting;
        }

        public async Task<ActionResult> Index()
        {
            Logger.LogInformation("Navigated to MyProfile/Index.");

            (var preferences, var lowellReference) = await GetCustomerPreferences();

            MyProfileVm vm = new MyProfileVm()
            {
                LowellReference = lowellReference,
                CustomerEmail = GetEmail(),
                LoggedInUserID = LoggedInUserId,
                ContactPreferencesVm = new ContactPreferencesVm
                {
                    MobileNumber = preferences?.PrimaryPhone,
                    AllowContactBySms = (preferences == null) ? false : preferences.ContactPreferenceSMS,
                    AllowContactByEmail = (preferences == null) ? false : preferences.ContactPreferenceEmail,
                    LowellReference = lowellReference
                },
                EnableContactPreferences = _portalSetting.Features.EnableContactPreferences
            };

            _gtmService.RaiseMyProfileViewedEvent(vm, LoggedInUserId);
            await _webActivityService.LogMyProfileViewed(lowellReference, LoggedInUserId);

            return View(vm);
        }

        [HttpPost]
        public IActionResult ProfileRedirect()
        {
            Logger.LogInformation("Profile Redirect");

            return View("ProfileRedirect");
        }

        [HttpPost]
        public IActionResult PasswordRedirect()
        {
            Logger.LogInformation("Password Redirect");

            return View("PasswordRedirect");
        }

        [HttpPost]
        public async Task<IActionResult> ContactPreferences(ContactPreferencesVm contactPreferencesVm)
        {
            if (!_portalSetting.Features.EnableContactPreferences)
            {
                return RedirectToAction("Index");
            }

            Logger.LogInformation("Navigated to MyProfile/ContactPreferences.");

            _gtmService.RaiseContactPreferecesPageEvent(contactPreferencesVm, LoggedInUserId);
            await _webActivityService.LogContactPreferencesPageVist(contactPreferencesVm.LowellReference, LoggedInUserId);

            return View(contactPreferencesVm);
        }

        public async Task<IActionResult> ContactPreferencesPost(ContactPreferencesVm contactPreferencesVm)
        {
            Logger.LogInformation("Saving ContactPreferences data.");

            if (!ModelState.IsValid)
            {
                Logger.LogDebug("Modelstate is invalid");
                return View("ContactPreferences", contactPreferencesVm);
            }

            if (contactPreferencesVm.AllowContactBySms && string.IsNullOrWhiteSpace(contactPreferencesVm.MobileNumber))
            {
                ModelState.AddModelError("MobileNumber", "Please enter a valid mobile number");
                return View("ContactPreferences", contactPreferencesVm);
            }

            await SaveContactPreferences(contactPreferencesVm);

            await LogEvents(contactPreferencesVm);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MobileNumber(ContactPreferencesVm contactPreferencesVm)
        {
            if (!_portalSetting.Features.EnableContactPreferences)
            {
                return RedirectToAction("Index");
            }

            Logger.LogInformation("Navigated to MyProfile/MobileNumber.");

            _gtmService.RaiseMobileUpdatePageEvent(contactPreferencesVm, LoggedInUserId);
            await _webActivityService.LogMobilePhoneUpdatePageVist(contactPreferencesVm.LowellReference, LoggedInUserId);

            return View(contactPreferencesVm);
        }

        [HttpPost]
        public async Task<IActionResult> MobileNumberPost(ContactPreferencesVm contactPreferencesVm)
        {
            Logger.LogInformation("Saving MobileNumber.");

            if (!ModelState.IsValid)
            {
                Logger.LogDebug("Modelstate is invalid");
                return View("MobileNumber", contactPreferencesVm);
            }

            if (string.IsNullOrWhiteSpace(contactPreferencesVm.MobileNumber))
            {
                ModelState.AddModelError("MobileNumber", "Please enter a valid mobile number");
                return View("MobileNumber", contactPreferencesVm);
            }

            var contactPreferencesCached = await GetFromCache<ContactPreferencesRetrievedDto>($"contact-preferences-{GetCaseflowUserId()}");

            await SaveMobileNumber(contactPreferencesVm);

            _gtmService.RaiseMobilePhoneNumberChangeEvent(contactPreferencesVm, LoggedInUserId, contactPreferencesVm.MobileNumber, contactPreferencesCached.PrimaryPhone);
            await _webActivityService.LogMobilePhoneNumberUpdateChange(contactPreferencesVm.LowellReference, LoggedInUserId, contactPreferencesVm.MobileNumber, contactPreferencesCached.PrimaryPhone);

            return RedirectToAction("Index");
        }

        private async Task LogEvents(ContactPreferencesVm contactPreferencesVm)
        {
            var contactPreferencesCached = await GetFromCache<ContactPreferencesRetrievedDto>($"contact-preferences-{GetCaseflowUserId()}");

            _gtmService.RaiseContactPreferecesSMSChangeEvent(contactPreferencesVm, LoggedInUserId, contactPreferencesVm.AllowContactBySms, contactPreferencesCached.ContactPreferenceSMS);
            await _webActivityService.LogContactPreferencesSMSChange(contactPreferencesVm.LowellReference, LoggedInUserId, contactPreferencesVm.AllowContactBySms, contactPreferencesCached.ContactPreferenceSMS);

            _gtmService.RaiseContactPreferecesEmailChangeEvent(contactPreferencesVm, LoggedInUserId, contactPreferencesVm.AllowContactByEmail, contactPreferencesCached.ContactPreferenceEmail);
            await _webActivityService.LogContactPreferecesEmailChange(contactPreferencesVm.LowellReference, LoggedInUserId, contactPreferencesVm.AllowContactByEmail, contactPreferencesCached.ContactPreferenceEmail);

            _gtmService.RaiseMobilePhoneNumberChangeEvent(contactPreferencesVm, LoggedInUserId, contactPreferencesVm.MobileNumber, contactPreferencesCached.PrimaryPhone);
            await _webActivityService.LogMobilePhoneNumberUpdateChange(contactPreferencesVm.LowellReference, LoggedInUserId, contactPreferencesVm.MobileNumber, contactPreferencesCached.PrimaryPhone);
        }

        private async Task<(ContactPreferencesRetrievedDto preferences, string lowellReference)> GetCustomerPreferences()
        {
            string caseflowUserId = GetCaseflowUserId();

            var lowellReference = await GetLowellReference(caseflowUserId);

            var preferences = await _myProfileService.GetContactPreferences(lowellReference);

            await CacheWithDefaultTtl(preferences, $"contact-preferences-{caseflowUserId}");

            return (preferences, lowellReference);
        }

        private async Task SaveContactPreferences(ContactPreferencesVm viewModel)
        {
            SaveContactPreferencesDto contactPreferences;

            var cached = await GetFromCache<ContactPreferencesRetrievedDto>($"contact-preferences-{GetCaseflowUserId()}");

            if (cached != null)
            {
                if (!viewModel.AllowContactBySms && !string.IsNullOrWhiteSpace(cached.PrimaryPhone))
                {
                    viewModel.MobileNumber = cached.PrimaryPhone;
                }

                contactPreferences = BuildSaveContactPreferencesModel(viewModel, cached);
            }
            else
            {
                var contactPreferencesRetrieved = await _myProfileService.GetContactPreferences(viewModel.LowellReference);

                if (!viewModel.AllowContactBySms && !string.IsNullOrWhiteSpace(contactPreferencesRetrieved.PrimaryPhone))
                {
                    viewModel.MobileNumber = contactPreferencesRetrieved.PrimaryPhone;
                }

                contactPreferences = BuildSaveContactPreferencesModel(viewModel, contactPreferencesRetrieved);
            }

            await _myProfileService.SaveContactPreferences(contactPreferences);
        }

        private async Task SaveMobileNumber(ContactPreferencesVm viewModel)
        {
            SaveContactPreferencesDto contactPreferences;

            var cached = await GetFromCache<ContactPreferencesRetrievedDto>($"contact-preferences-{GetCaseflowUserId()}");

            if (cached != null)
            {
                contactPreferences = BuildSaveContactPreferencesModel(viewModel, cached);
            }
            else
            {
                var contactPreferencesRetrieved = await _myProfileService.GetContactPreferences(viewModel.LowellReference);

                contactPreferences = BuildSaveContactPreferencesModel(viewModel, contactPreferencesRetrieved);
            }

            await _myProfileService.SaveContactPreferences(contactPreferences);
        }

        private async Task<string> GetLowellReference(string caseflowUserId)
        {
            var accountSummaries = await _accountsService.GetAccounts(caseflowUserId);
            var lowellReference = accountSummaries.Where(x => !x.AccountStatusIsClosed && !x.AccountStatusIsWithSolicitors)
                                                  .Select(x => x.AccountReference)
                                                  .ToList()
                                                  .First();
            return lowellReference;
        }

        private SaveContactPreferencesDto BuildSaveContactPreferencesModel(ContactPreferencesVm viewModel, ContactPreferencesRetrievedDto dto)
        {
            var contactPreferences = new SaveContactPreferencesDto
            {
                LowellReference = viewModel.LowellReference,
                CaseflowUserId = GetCaseflowUserId(),
                ContactPreferenceEmail = viewModel.AllowContactByEmail,
                ContactPreferenceSMS = viewModel.AllowContactBySms,
                PrimaryPhone = viewModel.MobileNumber,
                BusinessPhone = dto.BusinessPhone,
                EmailAddress = dto.EmailAddress,
                EmailConfirmedDate = dto.EmailConfirmedDate,
                Forename = dto.Forename,
                HomePhone = dto.HomePhone,
                OtherPhone = dto.OtherPhone,
                PaymentReminderEmails = dto.PaymentReminderEmails,
                PhoneChangedDate = dto.PhoneChangedDate,
                ReminderEmailsUpdBy = dto.ReminderEmailsUpdBy,
                Title = dto.Title,
                Salutation = dto.Salutation,
                StrategyEmails = dto.StrategyEmails,
                StrategyEmailsUpdBy = dto.StrategyEmailsUpdBy,
                StrategyEmailsUpdOn = dto.StrategyEmailsUpdOn,
                Surname = dto.Surname,
                Name = dto.Name,
                ReminderEmailsUpdOn = dto.ReminderEmailsUpdOn,
                Address = new List<Address>(),
            };

            foreach (var item in dto.Address)
            {
                contactPreferences.Address.Add(new Address
                {
                    Line1 = item.Line1,
                    Line2 = item.Line2,
                    Line3 = item.Line3,
                    Line4 = item.Line4,
                    Postcode = item.Postcode,
                    Type = item.Type
                });
            }

            return contactPreferences;
        }
    }
}