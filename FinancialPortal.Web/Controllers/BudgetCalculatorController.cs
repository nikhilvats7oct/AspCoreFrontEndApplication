using AutoMapper;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.Web.Controllers
{
    public class BudgetCalculatorController : BaseController
    {
        private readonly IGtmService _gtmService;
        private readonly ICalculatorService _calculatorService;
        private readonly IBudgetCalculatorService _budgetCalculatorService;
        private readonly IMapper _mapper;
        private readonly PortalSetting _portalSettings;
        private readonly IWebActivityService _webActivityService;
        private readonly ITriggerFigureService _triggerFigureService;

        public BudgetCalculatorController(ILogger<BaseController> logger,
            IConfiguration configuration,
            IGtmService gtmService,
            IDistributedCache distributedCache,
            ICalculatorService calculatorService,
            IApplicationSessionState sessionState,
            IWebActivityService webActivityService,
            ITriggerFigureService triggerFigureService,
            IBudgetCalculatorService budgetCalculatorService,
             PortalSetting portalSettings,
            IMapper mapper)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _gtmService = gtmService;
            _calculatorService = calculatorService;
            _budgetCalculatorService = budgetCalculatorService;
            _portalSettings = portalSettings;
            _mapper = mapper;
            _webActivityService = webActivityService;
            _triggerFigureService = triggerFigureService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // TODO: Is this correct ??!! You are adding Session ID to Lowell Ref!
            Guid id = ApplicationSessionState.AddLowellReferenceSurrogateKey(ApplicationSessionState.SessionId);
            ApplicationSessionState.IandELaunchedExternally = true;

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(id);

            HouseholdStatusVm vm = new HouseholdStatusVm
            {
                ExternallyLaunched = true,
                SavedIAndE = false,
                AdultsInHousehold = 1
            };

            if (incomeAndExpenditure != null)
            {
                vm = _mapper.Map<HouseholdStatusVm>(incomeAndExpenditure);

                vm.ExternallyLaunched = true;
                vm.SavedIAndE = false;
                vm.AdultsInHousehold = 1;
                ApplicationSessionState.SaveIncomeAndExpenditure(incomeAndExpenditure, id);
            }      

            _gtmService.RaiseBudgetCalculatorStartedEvent(vm, LoggedInUserId);
            await _webActivityService.LogBudgetCalculatorIncome(ApplicationSessionState.GetLowellReferenceFromSurrogate(id), LoggedInUserId);

            RouteData.Values.Add("id", id);

            return View("HouseholdStatus", vm);
        }

        [AllowAnonymous]
        public async Task<IActionResult> HouseholdStatus(HouseholdStatusVm viewModel)
        {
            ModelState.Clear();
            ApplicationSessionState.IandELaunchedExternally = false;

            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            ApplicationSessionState.CheckSessionStatus(id.ToString());
            Guid guid = Guid.Parse(id.ToString());

            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(guid);

            Guid caseflowUserId = LoggedInUser.IsLoggedInUser ? Guid.Parse(GetCaseflowUserId()) : Guid.NewGuid();

            IncomeAndExpenditure savedIAndE = await _budgetCalculatorService.GetSavedIncomeAndExpenditure(lowellReference);
            IncomeAndExpenditure partialSavedIAndE = viewModel.PartialSavedIAndE ? await _budgetCalculatorService.GetPartiallySavedIncomeAndExpenditure(LoggedInUserId, caseflowUserId) : null;
            IncomeAndExpenditure iAndE = partialSavedIAndE != null ? partialSavedIAndE : savedIAndE;

            if (iAndE == null)
            {
                //See if there is a cached I & E from OpenWrks
                iAndE = this.ApplicationSessionState.GetIncomeAndExpenditure(guid);
            }

            if (iAndE != null)
            {
                iAndE.BudgetSource = "";

                ApplicationSessionState.SaveIncomeAndExpenditure(iAndE, guid);
                await _webActivityService.LogBudgetCalculatorReplayed(ApplicationSessionState.GetLoggedInLowellRef(), LoggedInUserId);

                viewModel = _mapper.Map(iAndE, viewModel);
                viewModel.SavedIAndE = true;
                viewModel.PartialSavedIAndE = partialSavedIAndE != null;
            }

            await _webActivityService.LogBudgetCalculatorHouseholdDetails(lowellReference, LoggedInUserId);
            _gtmService.RaiseBudgetCalculatorStartedEvent(viewModel, LoggedInUserId);

            if (viewModel.AdultsInHousehold == null) { viewModel.AdultsInHousehold = 1; }

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost, ActionName("HouseholdStatus")]
        public async Task<IActionResult> HouseholdStatusPost(HouseholdStatusVm viewModel)
        {
            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            ApplicationSessionState.CheckSessionStatus(id.ToString());

            if (!ModelState.IsValid)
            {
                return View("HouseholdStatus", viewModel);
            }

            Guid guid = Guid.Parse(id.ToString());

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);
            incomeAndExpenditure = _mapper.Map(viewModel, incomeAndExpenditure);
            ApplicationSessionState.SaveIncomeAndExpenditure(incomeAndExpenditure, guid);

            await _webActivityService.LogBudgetCalculatorIncome(ApplicationSessionState.GetLowellReferenceFromSurrogate(guid), LoggedInUserId);

            return RedirectToAction("YourIncome", new { id });
        }

        [AllowAnonymous]
        public IActionResult YourIncome(IncomeVm viewModel)
        {
            ModelState.Clear();

            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            ApplicationSessionState.CheckSessionStatus(id.ToString());
            viewModel.EnabledPartialSave = _portalSettings.Features.EnablePartialSave && LoggedInUser.IsLoggedInUser;

            Guid guid = Guid.Parse(id.ToString());

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);

            _gtmService.RaiseBudgetCalculatorHouseholdDetailsEvent(viewModel, LoggedInUserId, incomeAndExpenditure?.HousingStatus, incomeAndExpenditure?.EmploymentStatus);

            viewModel = _mapper.Map(incomeAndExpenditure, viewModel);

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost, ActionName("YourIncome")]
        public async Task<IActionResult> YourIncomePost(IncomeVm viewModel, string submit)
        {
            if (submit == "saveforlater") { viewModel.PartialSavedEvent = true; }

            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            Guid guid = Guid.Parse(id.ToString());
            ApplicationSessionState.CheckSessionStatus(id.ToString());
            viewModel.EnabledPartialSave = _portalSettings.Features.EnablePartialSave && LoggedInUser.IsLoggedInUser;

            if (!ModelState.IsValid)
            {
                return View("YourIncome", viewModel);
            }

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);
            incomeAndExpenditure = _mapper.Map(viewModel, incomeAndExpenditure);
            ApplicationSessionState.SaveIncomeAndExpenditure(incomeAndExpenditure, guid);

            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(guid);

            await _webActivityService.LogBudgetCalculatorBillsAndOutgoings1(lowellReference, LoggedInUserId);

            if (submit == "saveforlater" && LoggedInUser.IsLoggedInUser && _portalSettings.Features.EnablePartialSave)
            {
                bool isPartialSaved = await PartialSave(incomeAndExpenditure, lowellReference);

                viewModel.PartialSavedIAndE = isPartialSaved;
                viewModel.HasErrorPartialSavedIAndE = !isPartialSaved;

                return View("YourIncome", viewModel);
            }

            return RedirectToAction("BillsAndOutgoings", new { id });
        }

        [AllowAnonymous]
        public IActionResult BillsAndOutgoings(BillsAndOutgoingsVm viewModel)
        {
            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            Guid guid = Guid.Parse(id.ToString());
            ApplicationSessionState.CheckSessionStatus(id.ToString());
            viewModel.EnabledPartialSave = _portalSettings.Features.EnablePartialSave && LoggedInUser.IsLoggedInUser;

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);

            if (incomeAndExpenditure == null)
            {
                return View(viewModel);
            }

            viewModel = _mapper.Map(incomeAndExpenditure, viewModel);

            string housingStatus = incomeAndExpenditure.HousingStatus;
            string employmentStatus = incomeAndExpenditure.EmploymentStatus;

            _gtmService.RaiseBudgetCalculatorIncomeEvent(viewModel, LoggedInUserId, employmentStatus, housingStatus, viewModel.IncomeSummary.Total);

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost, ActionName("BillsAndOutgoings")]
        public async Task<IActionResult> BillsAndOutgoingsPost(BillsAndOutgoingsVm viewModel, string submit)
        {
            if (submit == "saveforlater") { viewModel.PartialSavedEvent = true; }

            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            Guid guid = Guid.Parse(id.ToString());
            ApplicationSessionState.CheckSessionStatus(id.ToString());
            viewModel.EnabledPartialSave = _portalSettings.Features.EnablePartialSave && LoggedInUser.IsLoggedInUser;

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);
            incomeAndExpenditure = _mapper.Map(viewModel, incomeAndExpenditure);
            ApplicationSessionState.SaveIncomeAndExpenditure(incomeAndExpenditure, guid);

            MonthlyIncome incomeSummary = _calculatorService.CalculateMonthlyIncome(incomeAndExpenditure);

            if (!ModelState.IsValid)
            {
                viewModel.IncomeSummary = _mapper.Map<MonthlyIncome, MonthlyIncomeVm>(incomeSummary);
                return View("BillsAndOutgoings", viewModel);
            }

            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(guid);

            if (submit == "saveforlater" && LoggedInUser.IsLoggedInUser && _portalSettings.Features.EnablePartialSave)
            {
                bool isPartialSaved = await PartialSave(incomeAndExpenditure, lowellReference);

                viewModel.PartialSavedIAndE = isPartialSaved;
                viewModel.HasErrorPartialSavedIAndE = !isPartialSaved;

                return View("BillsAndOutgoings", viewModel);
            }

            return RedirectToAction("Expenditure", new { id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Expenditure(ExpendituresVm viewModel)
        {
            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            Guid guid = Guid.Parse(id.ToString());

            ApplicationSessionState.CheckSessionStatus(id.ToString());
            viewModel.EnabledPartialSave = _portalSettings.Features.EnablePartialSave && LoggedInUser.IsLoggedInUser;

            IncomeAndExpenditure iAndE = ApplicationSessionState.GetIncomeAndExpenditure(guid);

            var guideLines = await _triggerFigureService.GetExpenditureMetrics(ApplicationSessionState);
            var foodAndHousekeepingTrigger = _triggerFigureService.CalculateTriggerFigure(guideLines.FoodAndHousekeeping, iAndE.AdultsInHousehold, iAndE.ChildrenUnder16, iAndE.Children16to18);
            var personalCostsTrigger = _triggerFigureService.CalculateTriggerFigure(guideLines.PersonalCosts, iAndE.AdultsInHousehold, iAndE.ChildrenUnder16, iAndE.Children16to18);
            var communicationsAndLeisureTrigger = _triggerFigureService.CalculateTriggerFigure(guideLines.CommsAndLeisure, iAndE.AdultsInHousehold, iAndE.ChildrenUnder16, iAndE.Children16to18);

            viewModel = _mapper.Map(iAndE, viewModel);

            viewModel.FoodAndHouseKeepingTriggerMin = _triggerFigureService.CalculateMinTriggerValue(foodAndHousekeepingTrigger);
            viewModel.FoodAndHouseKeepingTriggerMax = _triggerFigureService.CalculateMaxTriggerValue(foodAndHousekeepingTrigger);
            viewModel.PersonalCostsTriggerMin = _triggerFigureService.CalculateMinTriggerValue(personalCostsTrigger);
            viewModel.PersonalCostsTriggerMax = _triggerFigureService.CalculateMaxTriggerValue(personalCostsTrigger);
            viewModel.CommunicationsAndLeisureTriggerMin = _triggerFigureService.CalculateMinTriggerValue(communicationsAndLeisureTrigger);
            viewModel.CommunicationsAndLeisureTriggerMax = _triggerFigureService.CalculateMaxTriggerValue(communicationsAndLeisureTrigger);

            string employmentStatus = iAndE.EmploymentStatus;
            string housingStatus = iAndE.HousingStatus;
            decimal income = viewModel.IncomeVmSummary.Total;
            decimal totalExpenditure = viewModel.OutgoingsVmSummary.Total;
            decimal disposableIncome = _calculatorService.CalculateDisposableIncome(income, totalExpenditure);

            _gtmService.RaiseBudgetCalculatorExpenditureEvent(viewModel, LoggedInUserId, employmentStatus, housingStatus, income, totalExpenditure, disposableIncome);

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost, ActionName("Expenditure")]
        public async Task<IActionResult> ExpenditurePost(ExpendituresVm viewModel, string submit)
        {
            if (submit == "saveforlater") { viewModel.PartialSavedEvent = true; }

            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            Guid guid = Guid.Parse(id.ToString());

            ApplicationSessionState.CheckSessionStatus(id.ToString());
            viewModel.EnabledPartialSave = _portalSettings.Features.EnablePartialSave && LoggedInUser.IsLoggedInUser;

            var incomeExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);
            incomeExpenditure = _mapper.Map(viewModel, incomeExpenditure);
            ApplicationSessionState.SaveIncomeAndExpenditure(incomeExpenditure, guid);

            if (!ModelState.IsValid)
            {
                MonthlyIncome monthlyIncome = _calculatorService.CalculateMonthlyIncome(incomeExpenditure);
                MonthlyOutgoings monthlyOutgoings = _calculatorService.CalculateMonthlyOutgoings(incomeExpenditure);

                viewModel.IncomeVmSummary = _mapper.Map<MonthlyIncome, MonthlyIncomeVm>(monthlyIncome);
                viewModel.OutgoingsVmSummary = _mapper.Map<MonthlyOutgoings, MonthlyOutgoingsVm>(monthlyOutgoings);

                return View("Expenditure", viewModel);
            }

            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(guid);

            await _webActivityService.LogBudgetCalculatorCompleted(lowellReference, LoggedInUserId);

            if (submit == "saveforlater" && LoggedInUser.IsLoggedInUser && _portalSettings.Features.EnablePartialSave)
            {
                bool isPartialSaved = await PartialSave(incomeExpenditure, lowellReference);

                viewModel.PartialSavedIAndE = isPartialSaved;
                viewModel.HasErrorPartialSavedIAndE = !isPartialSaved;

                return View("Expenditure", viewModel);
            }

            return RedirectToAction("BudgetSummary", new { id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> BudgetSummary()
        {
            var id = RouteData.Values["id"];

            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            ApplicationSessionState.CheckSessionStatus(id.ToString());
            Guid guid = Guid.Parse(id.ToString());

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);
            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(guid);

            var budgetSummary = GetBudgetSummary(incomeAndExpenditure, guid);

            if (budgetSummary != null && incomeAndExpenditure != null)
            {
                string employmentStatus = incomeAndExpenditure.EmploymentStatus;
                string housingStatus = incomeAndExpenditure.HousingStatus;

                _gtmService.RaiseBudgetCalculatorCompletedEvent(budgetSummary, LoggedInUserId, employmentStatus, housingStatus);
            }

            if (LoggedInUser.IsLoggedInUser && incomeAndExpenditure.BudgetSource != "MyBudget Tool")
            {
                await SaveBudgetSummary(incomeAndExpenditure, lowellReference);
                budgetSummary.IsSaved = true;
            }

            if (incomeAndExpenditure.BudgetSource == "MyBudget Tool" && incomeAndExpenditure.IncomeTotal <= 0)
            {
                return RedirectToAction("ZeroIncome");
            }
            else if (incomeAndExpenditure.BudgetSource == "MyBudget Tool")
            {
                budgetSummary.IsSaved = true;
            }

            budgetSummary.ExternallyLaunched = ApplicationSessionState.IandELaunchedExternally;
            budgetSummary.BudgetSource = incomeAndExpenditure.BudgetSource ?? "Budget Calculator";

            return View(budgetSummary);
        }

        [AllowAnonymous]
        public IActionResult ZeroIncome()
        {
            return View("ZeroIncome");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Save()
        {
            var id = RouteData.Values["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            ApplicationSessionState.CheckSessionStatus(id.ToString());
            Guid guid = Guid.Parse(id.ToString());

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);
            string lowellReference = ApplicationSessionState.GetLowellReferenceFromSurrogate(guid);

            var budgetSummary = GetBudgetSummary(incomeAndExpenditure, guid);
            await SaveBudgetSummary(incomeAndExpenditure, lowellReference);
            budgetSummary.LoggedInUserId = LoggedInUserId;
            budgetSummary.IsSaved = true;

            if (incomeAndExpenditure != null)
            {
                budgetSummary.HousingStatus = incomeAndExpenditure.HousingStatus;
                budgetSummary.EmploymentStatus = incomeAndExpenditure.EmploymentStatus;
            }

            return View("BudgetSummary", budgetSummary);
        }

        [ActionName("Options")]
        [AllowAnonymous]
        public IActionResult BudgetCalculatorOptions()
        {
            var id = RouteData.Values["id"] ?? ApplicationSessionState.GetTopLowellSurrogateKey();
          
            if (id == null)
            {
                return RedirectToAction("Index", "MyAccounts");
            }

            return View("Options", Guid.Parse(id.ToString()));
        }

        public ActionResult MakePayment(Guid id)
        {
            Guid guid = Guid.Parse(id.ToString());

            IncomeAndExpenditure incomeAndExpenditure = ApplicationSessionState.GetIncomeAndExpenditure(guid);

            BudgetSummaryVm budgetSummary = GetBudgetSummary(incomeAndExpenditure, guid);

            if (budgetSummary != null && incomeAndExpenditure != null)
            {
                string employmentStatus = incomeAndExpenditure.EmploymentStatus;
                string housingStatus = incomeAndExpenditure.HousingStatus;

                _gtmService.RaiseBudgetCalculatorContinuedToPaymentEvent(budgetSummary, LoggedInUserId, employmentStatus, housingStatus);
            }

            return RedirectToAction("Index", "PaymentOptions", new { @id = id });
        }

        private BudgetSummaryVm GetBudgetSummary(IncomeAndExpenditure iAndE, Guid lowellReferenceSurrogateKey)
        {
            var model = this._budgetCalculatorService.GetBudgetSummary(iAndE, lowellReferenceSurrogateKey, LoggedInUserId);

            if (model != null)
            {
                return _mapper.Map<BudgetSummary, BudgetSummaryVm>(model);
            }

            return null;
        }

        private async Task SaveBudgetSummary(IncomeAndExpenditure iAndE, string lowellReference)
        {
            if (string.IsNullOrWhiteSpace(iAndE.BudgetSource))
            {
                await this._budgetCalculatorService.SaveIncomeAndExpenditure(iAndE, lowellReference);

                if (_portalSettings.Features.EnablePartialSave)
                {
                    Guid caseflowUserId = LoggedInUser.IsLoggedInUser ? Guid.Parse(GetCaseflowUserId()) : Guid.NewGuid();
                    await this._budgetCalculatorService.RemovePartialSaved(caseflowUserId);
                }
            }
        }

        private async Task<bool> PartialSave(IncomeAndExpenditure iAndE, string lowellReference)
        {
            Guid caseflowUserId = LoggedInUser.IsLoggedInUser ? Guid.Parse(GetCaseflowUserId()) : Guid.NewGuid();

            return await this._budgetCalculatorService.PartiallySaveIncomeAndExpenditure(iAndE, lowellReference, caseflowUserId);
        }
    }
}