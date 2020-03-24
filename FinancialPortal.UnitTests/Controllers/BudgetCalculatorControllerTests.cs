using AutoMapper;
using FinancialPortal.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Services.ApiModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.Controllers
{
    [TestClass]
    public class BudgetCalculatorControllerTests
    {
        private Mock<ILogger<BudgetCalculatorController>> _logger;
        private Mock<IConfiguration> _config;
        private Mock<IGtmService> _gtmService;
        private Mock<IDistributedCache> _distributedCache;
        private Mock<ICalculatorService> _calculatorService;
        private Mock<IApplicationSessionState> _sessionState;
        private Mock<IWebActivityService> _webActivityService;
        private Mock<ITriggerFigureService> _triggerFigureService;
        private Mock<IBudgetCalculatorService> _budgetCalculatorService;
        private PortalSetting _portalSettings;
        private Mock<IMapper> _mapper;
        private string _caseflowUserId;

        private BudgetCalculatorController _controller;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._portalSettings = new PortalSetting { Features = new Features() };
            this._logger = new Mock<ILogger<BudgetCalculatorController>>(MockBehavior.Strict);
            this._config = new Mock<IConfiguration>(MockBehavior.Strict);
            this._gtmService = new Mock<IGtmService>(MockBehavior.Strict);
            this._distributedCache = new Mock<IDistributedCache>(MockBehavior.Strict);
            this._calculatorService = new Mock<ICalculatorService>(MockBehavior.Strict);
            this._sessionState = new Mock<IApplicationSessionState>(MockBehavior.Strict);
            this._webActivityService = new Mock<IWebActivityService>(MockBehavior.Strict);
            this._triggerFigureService = new Mock<ITriggerFigureService>(MockBehavior.Strict);
            this._budgetCalculatorService = new Mock<IBudgetCalculatorService>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);
            this._caseflowUserId = Guid.NewGuid().ToString();

            this._controller = new BudgetCalculatorController(this._logger.Object, this._config.Object, this._gtmService.Object,
                this._distributedCache.Object, this._calculatorService.Object, this._sessionState.Object,
                this._webActivityService.Object, this._triggerFigureService.Object, this._budgetCalculatorService.Object,
                this._portalSettings, this._mapper.Object);

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                    {
                        new ClaimsIdentity(new List<Claim>()
                        {
                            new Claim("caseflow_userid", _caseflowUserId)
                        }, "testing...")
                    })
                },
                RouteData = new RouteData()
            };

            _controller.ControllerContext = context;
        }

        [TestMethod]
        public async Task IndexTest() 
        {            
            string sessionId = Guid.NewGuid().ToString();
            Guid id = Guid.NewGuid();
            string lowellReference = "123456789";

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            HouseholdStatusVm vm = new HouseholdStatusVm();

            this._sessionState.Setup(x => x.SessionId).Returns(sessionId);
            this._sessionState.Setup(x => x.AddLowellReferenceSurrogateKey(sessionId)).Returns(id);
            this._sessionState.SetupSet(x => x.IandELaunchedExternally = true);

            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(id)).Returns(iAndE);
            this._mapper.Setup(x => x.Map<HouseholdStatusVm>(iAndE)).Returns(vm);

            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, id));
            this._gtmService.Setup(x => x.RaiseBudgetCalculatorStartedEvent(vm, this._caseflowUserId));

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns(lowellReference);
            this._webActivityService.Setup(x => x.LogBudgetCalculatorIncome(lowellReference, this._caseflowUserId))
                .Returns(Task.CompletedTask);

            ViewResult result = (ViewResult)await this._controller.Index();

            Assert.AreEqual(id, this._controller.RouteData.Values["id"]);
            Assert.AreEqual(vm, result.Model);
            Assert.AreEqual("HouseholdStatus", result.ViewName);

            Assert.AreEqual(true, vm.ExternallyLaunched);
            Assert.AreEqual(false, vm.SavedIAndE);
            Assert.AreEqual(1, vm.AdultsInHousehold);            
        }

        [TestMethod]
        public async Task HouseholdStatusTest_IdNull() 
        {
            this._sessionState.SetupSet(x => x.IandELaunchedExternally = false);

            RedirectToActionResult result = (RedirectToActionResult)await this._controller.HouseholdStatus(new HouseholdStatusVm());

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public async Task HouseholdStatusTest_PartialIandENull()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);

            this._sessionState.SetupSet(x => x.IandELaunchedExternally = false);
            this._sessionState.Setup(x => x.CheckSessionStatus(id));

            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id)))
                .Returns(lowellReference);

            IncomeAndExpenditure savedIandE = new IncomeAndExpenditure();
            this._budgetCalculatorService.Setup(x => x.GetSavedIncomeAndExpenditure(lowellReference))
                .Returns(Task.FromResult(savedIandE));

            IncomeAndExpenditure partiallySavedIandE = null;
            this._budgetCalculatorService.Setup(x => x.GetPartiallySavedIncomeAndExpenditure(this._caseflowUserId, Guid.Parse(this._caseflowUserId)))
                .Returns(Task.FromResult(partiallySavedIandE));

            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(savedIandE, Guid.Parse(id)));
            this._sessionState.Setup(x => x.GetLoggedInLowellRef()).Returns(lowellReference);
            this._webActivityService.Setup(x => x.LogBudgetCalculatorReplayed(lowellReference, this._caseflowUserId))
                .Returns(Task.CompletedTask);

            HouseholdStatusVm vm = new HouseholdStatusVm();
            HouseholdStatusVm expectedVm = new HouseholdStatusVm();
            this._mapper.Setup(x => x.Map(savedIandE, vm)).Returns(expectedVm);

            this._webActivityService.Setup(x => x.LogBudgetCalculatorHouseholdDetails(lowellReference, this._caseflowUserId))
                .Returns(Task.CompletedTask);

            this._gtmService.Setup(x => x.RaiseBudgetCalculatorStartedEvent(expectedVm, this._caseflowUserId));

            ViewResult result = (ViewResult)await this._controller.HouseholdStatus(vm);

            Assert.AreEqual(expectedVm, result.Model);
            Assert.AreEqual(true, expectedVm.SavedIAndE);
            Assert.AreEqual(false, expectedVm.PartialSavedIAndE);
        }

        [TestMethod]
        public async Task HouseholdStatusTest_PartialIandENotNull()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);

            this._sessionState.SetupSet(x => x.IandELaunchedExternally = false);
            this._sessionState.Setup(x => x.CheckSessionStatus(id));

            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id)))
                .Returns(lowellReference);

            IncomeAndExpenditure savedIandE = new IncomeAndExpenditure();
            this._budgetCalculatorService.Setup(x => x.GetSavedIncomeAndExpenditure(lowellReference))
                .Returns(Task.FromResult(savedIandE));

            IncomeAndExpenditure partiallySavedIandE = new IncomeAndExpenditure();
            this._budgetCalculatorService.Setup(x => x.GetPartiallySavedIncomeAndExpenditure(this._caseflowUserId, Guid.Parse(this._caseflowUserId)))
                .Returns(Task.FromResult(partiallySavedIandE));

            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(partiallySavedIandE, Guid.Parse(id)));
            this._sessionState.Setup(x => x.GetLoggedInLowellRef()).Returns(lowellReference);
            this._webActivityService.Setup(x => x.LogBudgetCalculatorReplayed(lowellReference, this._caseflowUserId))
                .Returns(Task.CompletedTask);

            HouseholdStatusVm vm = new HouseholdStatusVm() { PartialSavedIAndE = true };
            HouseholdStatusVm expectedVm = new HouseholdStatusVm() { PartialSavedIAndE = true };
            this._mapper.Setup(x => x.Map(partiallySavedIandE, vm)).Returns(expectedVm);

            this._webActivityService.Setup(x => x.LogBudgetCalculatorHouseholdDetails(lowellReference, this._caseflowUserId))
                .Returns(Task.CompletedTask);

            this._gtmService.Setup(x => x.RaiseBudgetCalculatorStartedEvent(expectedVm, this._caseflowUserId));

            ViewResult result = (ViewResult)await this._controller.HouseholdStatus(vm);

            Assert.AreEqual(expectedVm, result.Model);
            Assert.AreEqual(true, expectedVm.SavedIAndE);
            Assert.AreEqual(true, expectedVm.PartialSavedIAndE);
        }

        [TestMethod]
        public async Task HouseholdStatusPostTest_IdNull() 
        {
            RedirectToActionResult result = (RedirectToActionResult)await this._controller.HouseholdStatusPost(new HouseholdStatusVm());

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public async Task HouseholdStatusPostTest_ModelInvalid()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._controller.ModelState.AddModelError("testing", "testing");

            HouseholdStatusVm vm = new HouseholdStatusVm() 
            {
                AdultsInHousehold = 0,
            };
            
            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id))).Returns(lowellReference);

            ViewResult result = (ViewResult)await this._controller.HouseholdStatusPost(vm);

            Assert.AreEqual("HouseholdStatus", result.ViewName);
            Assert.AreEqual(vm, result.Model);
        }

        [TestMethod]
        public async Task HouseholdStatusPostTest()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);

            HouseholdStatusVm vm = new HouseholdStatusVm()
            {
                AdultsInHousehold = 1,
                ChildrenOver16 = 2,
                ChildrenUnder16 = 3,
                HousingStatus = "homeowner",
                EmploymentStatus = "full-time",
                ExternallyLaunched = false,
                GtmEvents = new List<GtmEvent>(),
                PartialSavedIAndE = false,
                SavedIAndE = false
            };

            string lowellReference = "123456789";

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id))).Returns(lowellReference);
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id))).Returns(lowellReference);
            this._webActivityService.Setup(x => x.LogBudgetCalculatorIncome(lowellReference, this._caseflowUserId))
                .Returns(Task.CompletedTask);

            RedirectToActionResult result = (RedirectToActionResult)await this._controller.HouseholdStatusPost(vm);

            Assert.AreEqual("YourIncome", result.ActionName);
            Assert.AreEqual(id, result.RouteValues["id"]);
        }

        [TestMethod]
        public void YourIncomeTest_IdNull() 
        {
            RedirectToActionResult result = (RedirectToActionResult)this._controller.YourIncome(new IncomeVm());

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public void YourIncomeTest()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            this._sessionState.Setup(x => x.CheckSessionStatus(id));

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure() 
            {
                HousingStatus = "housing-status",
                EmploymentStatus = "employment-status"
            };

            IncomeVm vm = new IncomeVm();

            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._gtmService.Setup(x => x.RaiseBudgetCalculatorHouseholdDetailsEvent(
                vm, this._caseflowUserId, "housing-status", "employment-status"));

            this._mapper.Setup(x => x.Map(iAndE, vm)).Returns(vm);


            ViewResult result = (ViewResult)this._controller.YourIncome(vm);

            Assert.AreEqual(vm, result.Model);
            Assert.AreEqual(true, vm.EnabledPartialSave);
        }

        [TestMethod]
        public async Task YourIncomePostTest_IdNull() 
        {
            RedirectToActionResult result = (RedirectToActionResult)await this._controller.YourIncomePost(new IncomeVm(), "");

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public async Task YourIncomePostTest_ModelStateInvalid() 
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._controller.ModelState.AddModelError("testing", "testing");
            this._portalSettings.Features.EnablePartialSave = true;

            IncomeVm vm = new IncomeVm();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));

            ViewResult result = (ViewResult)await this._controller.YourIncomePost(vm, "testing");

            Assert.AreEqual("YourIncome", result.ViewName);
            Assert.AreEqual(vm, result.Model);
            Assert.IsTrue(vm.EnabledPartialSave);
        }

        [TestMethod]
        public async Task YourIncomePostTest()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            IncomeVm vm = new IncomeVm();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));

            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id))).Returns(lowellReference);
            this._webActivityService.Setup(x => x.LogBudgetCalculatorBillsAndOutgoings1(
                lowellReference, this._caseflowUserId)).Returns(Task.CompletedTask);

            RedirectToActionResult result = (RedirectToActionResult)await this._controller.YourIncomePost(vm, "testing");

            Assert.AreEqual("BillsAndOutgoings", result.ActionName);
            Assert.AreEqual(id, result.RouteValues["id"]);
            Assert.IsTrue(vm.EnabledPartialSave);
        }

        [TestMethod]
        public async Task YourIncomePostTest_SaveForLater()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            IncomeVm vm = new IncomeVm();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));

            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id))).Returns(lowellReference);
            this._webActivityService.Setup(x => x.LogBudgetCalculatorBillsAndOutgoings1(
                lowellReference, this._caseflowUserId)).Returns(Task.CompletedTask);

            this._budgetCalculatorService.Setup(x => x.PartiallySaveIncomeAndExpenditure(
                iAndE, lowellReference, Guid.Parse(this._caseflowUserId))).Returns(Task.FromResult(true));

            ViewResult result = (ViewResult)await this._controller.YourIncomePost(vm, "saveforlater");

            Assert.AreEqual("YourIncome", result.ViewName);
            Assert.AreEqual(vm, result.Model);
            Assert.IsTrue(vm.EnabledPartialSave);
            Assert.IsTrue(vm.PartialSavedEvent);
            Assert.IsTrue(vm.PartialSavedIAndE);
            Assert.IsFalse(vm.HasErrorPartialSavedIAndE);
        }

        [TestMethod]
        public void BillsAndOutgoingsTest_IdNull() 
        {
            RedirectToActionResult result = (RedirectToActionResult)this._controller
                .BillsAndOutgoings(new BillsAndOutgoingsVm());

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public void BillsAndOutgoingsTest_NullIandE()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            this._sessionState.Setup(x => x.CheckSessionStatus(id));

            BillsAndOutgoingsVm vm = new BillsAndOutgoingsVm();
            IncomeAndExpenditure iAndE = null;

            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);

            ViewResult result = (ViewResult)this._controller
                .BillsAndOutgoings(vm);

            Assert.AreEqual(vm, result.Model);
            Assert.IsTrue(vm.EnabledPartialSave);
        }

        [TestMethod]
        public void BillsAndOutgoingsTest()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            this._sessionState.Setup(x => x.CheckSessionStatus(id));

            BillsAndOutgoingsVm vm = new BillsAndOutgoingsVm() 
            {
                IncomeSummary = new MonthlyIncomeVm() 
                {
                    Benefits = 100,
                    Other = 200,
                    Pension = 300,
                    Salary = 400,
                    Total = 1000
                } 
            };

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure() 
            {
                HousingStatus = "housing-status",
                EmploymentStatus = "employment-status"
            };

            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._mapper.Setup(x => x.Map(iAndE, vm)).Returns(vm);
            this._gtmService.Setup(x => x.RaiseBudgetCalculatorIncomeEvent(
                vm, this._caseflowUserId, "employment-status", "housing-status", 1000));

            ViewResult result = (ViewResult)this._controller
                .BillsAndOutgoings(vm);

            Assert.AreEqual(vm, result.Model);
            Assert.IsTrue(vm.EnabledPartialSave);
        }

        [TestMethod]
        public async Task BillsAndOutgoingsPostTest_IdNull() 
        {
            RedirectToActionResult result = (RedirectToActionResult)await _controller.BillsAndOutgoingsPost(
                new BillsAndOutgoingsVm(), "testing");

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public async Task BillsAndOutgoingsPostTest_InvalidModelState() 
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._controller.ModelState.AddModelError("testing", "testing");
            this._portalSettings.Features.EnablePartialSave = true;

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            BillsAndOutgoingsVm vm = new BillsAndOutgoingsVm();
            MonthlyIncome monthlyIncome = new MonthlyIncome();
            MonthlyIncomeVm monthlyIncomeVm = new MonthlyIncomeVm();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id)))
                .Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));
            this._calculatorService.Setup(x => x.CalculateMonthlyIncome(iAndE)).Returns(monthlyIncome);

            this._mapper.Setup(x => x.Map<MonthlyIncome, MonthlyIncomeVm>(monthlyIncome)).Returns(monthlyIncomeVm);

            ViewResult result = (ViewResult)await this._controller.BillsAndOutgoingsPost(vm, "testing");

            Assert.AreEqual("BillsAndOutgoings", result.ViewName);
            Assert.AreEqual(vm, result.Model);
            Assert.AreEqual(monthlyIncomeVm, vm.IncomeSummary);
            Assert.IsTrue(vm.EnabledPartialSave);
        }

        [TestMethod]
        public async Task BillsAndOutgoingsPostTest()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            BillsAndOutgoingsVm vm = new BillsAndOutgoingsVm();
            MonthlyIncome monthlyIncome = new MonthlyIncome();
            MonthlyIncomeVm monthlyIncomeVm = new MonthlyIncomeVm();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id)))
                .Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));
            this._calculatorService.Setup(x => x.CalculateMonthlyIncome(iAndE)).Returns(monthlyIncome);

            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id)))
                .Returns(lowellReference);

            RedirectToActionResult result = (RedirectToActionResult)await this._controller
                .BillsAndOutgoingsPost(vm, "testing");

            Assert.AreEqual("Expenditure", result.ActionName);
            Assert.AreEqual(id, result.RouteValues["id"]);
            Assert.IsTrue(vm.EnabledPartialSave);
        }

        [TestMethod]
        public async Task BillsAndOutgoingsPostTest_SaveForLater()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            BillsAndOutgoingsVm vm = new BillsAndOutgoingsVm();
            MonthlyIncome monthlyIncome = new MonthlyIncome();
            MonthlyIncomeVm monthlyIncomeVm = new MonthlyIncomeVm();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id)))
                .Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));
            this._calculatorService.Setup(x => x.CalculateMonthlyIncome(iAndE)).Returns(monthlyIncome);

            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id)))
                .Returns(lowellReference);

            this._budgetCalculatorService.Setup(x => x.PartiallySaveIncomeAndExpenditure(
                iAndE, lowellReference, Guid.Parse(this._caseflowUserId))).Returns(Task.FromResult(true));

            ViewResult result = (ViewResult)await this._controller
                .BillsAndOutgoingsPost(vm, "saveforlater");

            Assert.AreEqual("BillsAndOutgoings", result.ViewName);
            Assert.AreEqual(vm, result.Model);
            Assert.IsTrue(vm.EnabledPartialSave);
            Assert.IsTrue(vm.PartialSavedEvent);
            Assert.IsTrue(vm.PartialSavedIAndE);
            Assert.IsFalse(vm.HasErrorPartialSavedIAndE);
        }

        [TestMethod]
        public async Task ExpenditureTest_IdNull() 
        {
            RedirectToActionResult result = (RedirectToActionResult)await _controller.Expenditure(new ExpendituresVm());

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public async Task ExpendituresTest() 
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            this._sessionState.Setup(x => x.CheckSessionStatus(id));

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure() 
            {
                AdultsInHousehold = 1,
                Children16to18 = 2,
                ChildrenUnder16 = 3,
                EmploymentStatus = "employment-status",
                HousingStatus = "housing-status"
            };

            ExpendituresVm vm = new ExpendituresVm() 
            {
                IncomeVmSummary = new MonthlyIncomeVm() 
                {
                    Benefits = 100,
                    Other = 200,
                    Pension = 300,
                    Salary = 400,
                    Total = 1000
                },
                OutgoingsVmSummary = new MonthlyOutgoingsVm() 
                {
                    Expenditures = 1000,
                    HouseholdBills = 2000,
                    Total = 3000
                }
            };

            ExpenditureMetrics expenditureMetrics = new ExpenditureMetrics()
            {
                CommsAndLeisure = new ExpenditureMetric(),
                FoodAndHousekeeping = new ExpenditureMetric(),
                PersonalCosts = new ExpenditureMetric()
            };

            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._triggerFigureService.Setup(x => x.GetExpenditureMetrics(this._sessionState.Object))
                .Returns(Task.FromResult(expenditureMetrics));
            this._triggerFigureService.Setup(x => x.CalculateTriggerFigure(expenditureMetrics.FoodAndHousekeeping, 1, 3, 2))
                .Returns(100);
            this._triggerFigureService.Setup(x => x.CalculateTriggerFigure(expenditureMetrics.PersonalCosts, 1, 3, 2))
                .Returns(200);
            this._triggerFigureService.Setup(x => x.CalculateTriggerFigure(expenditureMetrics.CommsAndLeisure, 1, 3, 2))
                .Returns(300);

            this._mapper.Setup(x => x.Map(iAndE, vm)).Returns(vm);

            this._triggerFigureService.Setup(x => x.CalculateMinTriggerValue(100)).Returns(80);
            this._triggerFigureService.Setup(x => x.CalculateMaxTriggerValue(100)).Returns(120);
            this._triggerFigureService.Setup(x => x.CalculateMinTriggerValue(200)).Returns(160);
            this._triggerFigureService.Setup(x => x.CalculateMaxTriggerValue(200)).Returns(240);
            this._triggerFigureService.Setup(x => x.CalculateMinTriggerValue(300)).Returns(180);
            this._triggerFigureService.Setup(x => x.CalculateMaxTriggerValue(300)).Returns(360);

            this._calculatorService.Setup(x => x.CalculateDisposableIncome(1000, 3000)).Returns(1500);

            this._gtmService.Setup(x => x.RaiseBudgetCalculatorExpenditureEvent(
                vm, this._caseflowUserId, "employment-status", "housing-status", 1000, 3000, 1500));

            ViewResult result = (ViewResult)await this._controller.Expenditure(vm);

            Assert.AreEqual(vm, result.Model);

            Assert.IsTrue(vm.EnabledPartialSave);
            Assert.AreEqual(vm.FoodAndHouseKeepingTriggerMin, 80);
            Assert.AreEqual(vm.FoodAndHouseKeepingTriggerMax, 120);
            Assert.AreEqual(vm.PersonalCostsTriggerMin, 160);
            Assert.AreEqual(vm.PersonalCostsTriggerMax, 240);
            Assert.AreEqual(vm.CommunicationsAndLeisureTriggerMin, 180);
            Assert.AreEqual(vm.CommunicationsAndLeisureTriggerMax, 360);
        }

        [TestMethod]
        public async Task ExpenditurePostTest_IdNull()
        {
            RedirectToActionResult result = (RedirectToActionResult)await _controller.ExpenditurePost(
                new ExpendituresVm(), "testing");

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public async Task ExpenditurePostTest_InvalidModelState() 
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._controller.ModelState.AddModelError("testing", "testing");
            this._portalSettings.Features.EnablePartialSave = true;

            ExpendituresVm vm = new ExpendituresVm();
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            MonthlyIncome monthlyIncome = new MonthlyIncome();
            MonthlyIncomeVm monthlyIncomeVm = new MonthlyIncomeVm();
            MonthlyOutgoings monthlyOutgoings = new MonthlyOutgoings();
            MonthlyOutgoingsVm monthlyOutgoingsVm = new MonthlyOutgoingsVm();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));

            this._calculatorService.Setup(x => x.CalculateMonthlyIncome(iAndE)).Returns(monthlyIncome);
            this._calculatorService.Setup(x => x.CalculateMonthlyOutgoings(iAndE)).Returns(monthlyOutgoings);
            this._mapper.Setup(x => x.Map<MonthlyIncome, MonthlyIncomeVm>(monthlyIncome))
                .Returns(monthlyIncomeVm);
            this._mapper.Setup(x => x.Map<MonthlyOutgoings, MonthlyOutgoingsVm>(monthlyOutgoings))
                .Returns(monthlyOutgoingsVm);

            ViewResult result = (ViewResult)await this._controller.ExpenditurePost(vm, "testing");

            Assert.AreEqual("Expenditure", result.ViewName);
            Assert.AreEqual(vm, result.Model);
            Assert.AreEqual(vm.IncomeVmSummary, monthlyIncomeVm);
            Assert.AreEqual(vm.OutgoingsVmSummary, monthlyOutgoingsVm);
            Assert.IsTrue(vm.EnabledPartialSave);
        }

        [TestMethod]
        public async Task ExpenditurePostTest()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            ExpendituresVm vm = new ExpendituresVm();
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));

            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id)))
                .Returns(lowellReference);

            this._webActivityService.Setup(x => x.LogBudgetCalculatorCompleted(lowellReference, this._caseflowUserId))
                .Returns(Task.CompletedTask);

            RedirectToActionResult result = (RedirectToActionResult)await this._controller
                .ExpenditurePost(vm, "testing");

            Assert.IsTrue(vm.EnabledPartialSave);
            Assert.AreEqual("BudgetSummary", result.ActionName);
            Assert.AreEqual(id, result.RouteValues["id"]);
        }

        [TestMethod]
        public async Task ExpenditurePostTest_SaveForLater()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            ExpendituresVm vm = new ExpendituresVm();
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._mapper.Setup(x => x.Map(vm, iAndE)).Returns(iAndE);
            this._sessionState.Setup(x => x.SaveIncomeAndExpenditure(iAndE, Guid.Parse(id)));

            string lowellReference = "123456789";
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id)))
                .Returns(lowellReference);

            this._webActivityService.Setup(x => x.LogBudgetCalculatorCompleted(lowellReference, this._caseflowUserId))
                .Returns(Task.CompletedTask);

            this._budgetCalculatorService.Setup(x => x.PartiallySaveIncomeAndExpenditure(
                iAndE, lowellReference, Guid.Parse(this._caseflowUserId))).Returns(Task.FromResult(true));

            ViewResult result = (ViewResult)await this._controller
                .ExpenditurePost(vm, "saveforlater");

            Assert.IsTrue(vm.EnabledPartialSave);
            Assert.IsTrue(vm.PartialSavedIAndE);
            Assert.IsFalse(vm.HasErrorPartialSavedIAndE);
            Assert.AreEqual("Expenditure", result.ViewName);
            Assert.AreEqual(vm, result.Model);
        }

        [TestMethod]
        public async Task BudgetSummary_IdNull() 
        {
            RedirectToActionResult result = (RedirectToActionResult)await this._controller.BudgetSummary();

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public async Task BudgetSummary()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure() 
            {
                EmploymentStatus = "employment-status",
                HousingStatus = "housing-status",
            };

            string lowellReference = "123456789";
            
            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id)))
                .Returns(lowellReference);

            BudgetSummary budgetSummary = new BudgetSummary() {IncomeTotal = 5000};
            BudgetSummaryVm budgetSummaryVm = new BudgetSummaryVm(){ IncomeTotal = 5000};

            this._budgetCalculatorService.Setup(x => x.GetBudgetSummary(
                iAndE, Guid.Parse(id), this._caseflowUserId)).Returns(budgetSummary);
            this._mapper.Setup(x => x.Map<BudgetSummary, BudgetSummaryVm>(budgetSummary)).Returns(budgetSummaryVm);

            this._gtmService.Setup(x => x.RaiseBudgetCalculatorCompletedEvent(
                budgetSummaryVm, this._caseflowUserId, "employment-status", "housing-status"));

            this._budgetCalculatorService.Setup(x => x.SaveIncomeAndExpenditure(iAndE, lowellReference))
                .Returns(Task.CompletedTask);
            this._budgetCalculatorService.Setup(x => x.RemovePartialSaved(Guid.Parse(this._caseflowUserId)))
                .Returns(Task.CompletedTask);

            this._sessionState.Setup(x => x.IandELaunchedExternally).Returns(true);

            ViewResult result = (ViewResult)await this._controller.BudgetSummary();

            Assert.AreEqual(budgetSummaryVm, result.Model);
            Assert.IsTrue(budgetSummaryVm.IsSaved);
            Assert.IsTrue(budgetSummaryVm.ExternallyLaunched);
        }

        [TestMethod]
        public async Task SaveTest_IdNull() 
        {
            RedirectToActionResult result = (RedirectToActionResult)await this._controller.Save();

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);
        }

        [TestMethod]
        public async Task SaveTest()
        {
            string id = Guid.NewGuid().ToString();
            this._controller.RouteData.Values.Add("id", id);
            this._portalSettings.Features.EnablePartialSave = true;

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure() 
            {
                HousingStatus = "housing-status",
                EmploymentStatus = "employment-status"
            };

            BudgetSummary budgetSummary = new BudgetSummary();
            BudgetSummaryVm budgetSummaryVm = new BudgetSummaryVm();

            string lowellReference = "123456789";

            this._sessionState.Setup(x => x.CheckSessionStatus(id));
            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(Guid.Parse(id))).Returns(iAndE);
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(Guid.Parse(id))).Returns(lowellReference);

            this._budgetCalculatorService.Setup(x => x.GetBudgetSummary(
                iAndE, Guid.Parse(id), this._caseflowUserId)).Returns(budgetSummary);
            this._mapper.Setup(x => x.Map<BudgetSummary, BudgetSummaryVm>(budgetSummary)).Returns(budgetSummaryVm);
            this._budgetCalculatorService.Setup(x => x.SaveIncomeAndExpenditure(iAndE, lowellReference)).Returns(Task.CompletedTask);
            this._budgetCalculatorService.Setup(x => x.RemovePartialSaved(Guid.Parse(this._caseflowUserId))).Returns(Task.CompletedTask);

            ViewResult result = (ViewResult)await this._controller.Save();

            Assert.AreEqual("BudgetSummary", result.ViewName);
            Assert.AreEqual(budgetSummaryVm, result.Model);

            Assert.AreEqual(this._caseflowUserId, budgetSummaryVm.LoggedInUserId);
            Assert.IsTrue(budgetSummaryVm.IsSaved);
            Assert.AreEqual("housing-status", budgetSummaryVm.HousingStatus);
            Assert.AreEqual("employment-status", budgetSummaryVm.EmploymentStatus);
        }

        [TestMethod]
        public void MakePaymentTest() 
        {
            Guid id = Guid.NewGuid();

            IncomeAndExpenditure iAndE = new IncomeAndExpenditure() 
            {
                HousingStatus = "housing-status",
                EmploymentStatus = "employment-status"
            };

            BudgetSummary budgetSummary = new BudgetSummary();
            BudgetSummaryVm budgetSummaryVm = new BudgetSummaryVm();

            this._sessionState.Setup(x => x.GetIncomeAndExpenditure(id)).Returns(iAndE);
            this._budgetCalculatorService.Setup(x => x.GetBudgetSummary(
                iAndE, id, this._caseflowUserId)).Returns(budgetSummary);
            this._mapper.Setup(x => x.Map<BudgetSummary, BudgetSummaryVm>(budgetSummary)).Returns(budgetSummaryVm);

            this._gtmService.Setup(x => x.RaiseBudgetCalculatorContinuedToPaymentEvent(
                budgetSummaryVm, this._caseflowUserId, "employment-status", "housing-status"));

            RedirectToActionResult result = (RedirectToActionResult)this._controller.MakePayment(id);

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("PaymentOptions", result.ControllerName);
            Assert.AreEqual(id, result.RouteValues["id"]);
        }

    }
}
