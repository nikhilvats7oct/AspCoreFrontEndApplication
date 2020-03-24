using AutoMapper;
using FinancialPortal.Web.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Controllers
{
    public class MyAccountsController : BaseController
    {
        private readonly IGtmService _gtmService;
        private readonly IWebActivityService _webActivityService;
        private readonly IAccountsService _accountsService;
        private readonly IMapper _mapper;

        public MyAccountsController(ILogger<BaseController> logger,
            IConfiguration configuration,
            IDistributedCache distributedCache,
            IGtmService gtmService,
            IWebActivityService webActivityService,
            IAccountsService accountsService,
            IApplicationSessionState sessionState,
            IMapper mapper)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _gtmService = gtmService;
            _webActivityService = webActivityService;
            _accountsService = accountsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string caseflowUserId = GetCaseflowUserId();

            List<AccountSummary> accountSummaries = await _accountsService.GetAccounts(caseflowUserId);
            List<string> lowellReferences = accountSummaries.Select(x => x.AccountReference).ToList();
            IDictionary<string, Guid> surrogateKeysByLowellReference = ApplicationSessionState.AddLowellReferenceSurrogateKeys(lowellReferences);

            Guid? lowellFinancialAccountSurrogateKey = null;
            string loggedInLowellRef = null;

            foreach (AccountSummary account in accountSummaries)
            {
                if (!account.AccountStatusIsWithSolicitors && lowellFinancialAccountSurrogateKey == null)
                {
                    lowellFinancialAccountSurrogateKey = surrogateKeysByLowellReference[account.AccountReference];
                    loggedInLowellRef = ApplicationSessionState.GetLowellReferenceFromSurrogate(lowellFinancialAccountSurrogateKey.Value);
                }
            }

            CustomerSummary customerSummary = await _accountsService.CreateCustomerSummary(accountSummaries, loggedInLowellRef, surrogateKeysByLowellReference);

            MyAccountsVm vm = _mapper.Map<CustomerSummary, MyAccountsVm>(customerSummary);

            if (TempData != null && TempData.ContainsKey("GTMEvents"))
            {
                vm.GtmEvents = JsonConvert.DeserializeObject<List<GtmEvent>>(TempData["GTMEvents"].ToString());
                TempData.Remove("GTMEvents");
            }

            _gtmService.RaiseMyAccountsPageViewedEvent(vm, LoggedInUserId);

            await _webActivityService.LogMyAccountsPageViewed(loggedInLowellRef, LoggedInUserId);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(id);

            Account account = await _accountsService.GetAccount(LoggedInUserId, lowellReference);

            MyAccountsDetailVm vm = _mapper.Map<Account, MyAccountsDetailVm>(account);

            vm.LoggedInUserId = LoggedInUserId;
            vm.LoggedInLowellRef = ApplicationSessionState.GetLoggedInLowellRef();
            vm.LowellReferenceSurrogateKey = id;

            _gtmService.RaiseAccountDetailsViewedEvent(vm, LoggedInUserId, "Regular Account");
            await _webActivityService.LogAccountDetailsViewed(vm.AccountReferenceText, LoggedInUserId);

            return View(vm);
        }

    }
}