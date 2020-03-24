using AutoMapper;
using FinancialPortal.Web.Services;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.UnitTests.Services
{
    [TestClass]
    public class BudgetCalculatorServiceTests
    {
        private PortalSetting _portalSettings;
        private Mock<ICalculatorService> _calculatorService;
        private Mock<IRestClient> _restClient;
        private Mock<IMapper> _mapper;

        private BudgetCalculatorService _budgetCalculatorService;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._portalSettings = new PortalSetting() 
            {
                GatewayEndpoint = "TESTING/",
                Features = new Features()
            };

            this._calculatorService = new Mock<ICalculatorService>(MockBehavior.Strict);
            this._restClient = new Mock<IRestClient>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);

            this._budgetCalculatorService = new BudgetCalculatorService(
                this._restClient.Object, this._calculatorService.Object, this._portalSettings,
                this._mapper.Object);
        }

        [TestMethod]
        public async Task GetSavedIncomeAndExpenditureTest_ResultNull() 
        {
            string lowellReference = "123456789";

            this._restClient.Setup(x => x.PostAsync<IncomeAndExpenditureApiRequest, IncomeAndExpenditureApiModel>(
                "TESTING/api/BudgetCalculator/GetSavedIncomeAndExpenditure",
                It.Is<IncomeAndExpenditureApiRequest>(m => m.LowellReference == "123456789")))
                .Returns(Task.FromResult<IncomeAndExpenditureApiModel>(null));

            IncomeAndExpenditure result = await _budgetCalculatorService.GetSavedIncomeAndExpenditure(lowellReference);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetSavedIncomeAndExpenditureTest_ResultOld()
        {
            string lowellReference = "123456789";

            IncomeAndExpenditureApiModel apiModel = new IncomeAndExpenditureApiModel()
            {
                Created = DateTime.Now.AddDays(-200)
            };

            this._restClient.Setup(x => x.PostAsync<IncomeAndExpenditureApiRequest, IncomeAndExpenditureApiModel>(
                "TESTING/api/BudgetCalculator/GetSavedIncomeAndExpenditure",
                It.Is<IncomeAndExpenditureApiRequest>(m => m.LowellReference == "123456789")))
                .Returns(Task.FromResult(apiModel));

            IncomeAndExpenditure result = await _budgetCalculatorService.GetSavedIncomeAndExpenditure(lowellReference);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetSavedIncomeAndExpenditureTest()
        {
            string lowellReference = "123456789";

            IncomeAndExpenditureApiModel apiModel = new IncomeAndExpenditureApiModel()
            {
                Created = DateTime.Now.AddDays(-30)
            };

            IncomeAndExpenditure expected = new IncomeAndExpenditure()
            {
                Created = DateTime.Now.AddDays(-30)
            };
            
            this._restClient.Setup(x => x.PostAsync<IncomeAndExpenditureApiRequest, IncomeAndExpenditureApiModel>(
                "TESTING/api/BudgetCalculator/GetSavedIncomeAndExpenditure",
                It.Is<IncomeAndExpenditureApiRequest>(m => m.LowellReference == "123456789")))
                .Returns(Task.FromResult(apiModel));

            this._mapper.Setup(x => x.Map<IncomeAndExpenditure>(apiModel)).Returns(expected);

            IncomeAndExpenditure result = await _budgetCalculatorService.GetSavedIncomeAndExpenditure(lowellReference);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task GetPartiallySavedIncomeAndExpenditureTest_LoggedInUserIdNull() 
        {
            this._portalSettings.Features.EnablePartialSave = true;

            string loggedInUserId = null;
            Guid caseflowUserId = Guid.NewGuid();

            Assert.IsNull(await this._budgetCalculatorService
                .GetPartiallySavedIncomeAndExpenditure(loggedInUserId, caseflowUserId));
        }

        [TestMethod]
        public async Task GetPartiallySavedIncomeAndExpenditureTest_EnablePartialSaveFalse()
        {
            this._portalSettings.Features.EnablePartialSave = false;

            string loggedInUserId = Guid.NewGuid().ToString();
            Guid caseflowUserId = Guid.NewGuid();

            Assert.IsNull(await this._budgetCalculatorService
                .GetPartiallySavedIncomeAndExpenditure(loggedInUserId, caseflowUserId));
        }

        [TestMethod]
        public async Task GetPartiallySavedIncomeAndExpenditureTest_ResultNull()
        {
            this._portalSettings.Features.EnablePartialSave = true;

            string loggedInUserId = Guid.NewGuid().ToString();
            Guid caseflowUserId = Guid.NewGuid();

            IncomeAndExpenditureApiModel apiModel = null;

            this._restClient.Setup(x => x.PostAsync<PartialBudgetApiRequest, IncomeAndExpenditureApiModel>(
                "TESTING/api/BudgetCalculator/GetPartialSavedIncomeAndExpenditure", 
                It.Is<PartialBudgetApiRequest>(m => m.CaseflowUserId == caseflowUserId)))
                .Returns(Task.FromResult(apiModel));

            Assert.IsNull(await this._budgetCalculatorService
                .GetPartiallySavedIncomeAndExpenditure(loggedInUserId, caseflowUserId));
        }

        [TestMethod]
        public async Task GetPartiallySavedIncomeAndExpenditureTest_ResultOld()
        {
            this._portalSettings.Features.EnablePartialSave = true;

            string loggedInUserId = Guid.NewGuid().ToString();
            Guid caseflowUserId = Guid.NewGuid();

            IncomeAndExpenditureApiModel apiModel = new IncomeAndExpenditureApiModel() 
            {
                Created = DateTime.Now.AddDays(-45)
            };

            this._restClient.Setup(x => x.PostAsync<PartialBudgetApiRequest, IncomeAndExpenditureApiModel>(
                "TESTING/api/BudgetCalculator/GetPartialSavedIncomeAndExpenditure",
                It.Is<PartialBudgetApiRequest>(m => m.CaseflowUserId == caseflowUserId)))
                .Returns(Task.FromResult(apiModel));

            Assert.IsNull(await this._budgetCalculatorService
                .GetPartiallySavedIncomeAndExpenditure(loggedInUserId, caseflowUserId));
        }

        [TestMethod]
        public async Task GetPartiallySavedIncomeAndExpenditureTest()
        {
            this._portalSettings.Features.EnablePartialSave = true;

            string loggedInUserId = Guid.NewGuid().ToString();
            Guid caseflowUserId = Guid.NewGuid();

            IncomeAndExpenditureApiModel apiModel = new IncomeAndExpenditureApiModel()
            {
                Created = DateTime.Now.AddDays(-5)
            };

            IncomeAndExpenditure expected = new IncomeAndExpenditure()             
            {
                Created = DateTime.Now.AddDays(-5)
            };

            this._restClient.Setup(x => x.PostAsync<PartialBudgetApiRequest, IncomeAndExpenditureApiModel>(
                "TESTING/api/BudgetCalculator/GetPartialSavedIncomeAndExpenditure",
                It.Is<PartialBudgetApiRequest>(m => m.CaseflowUserId == caseflowUserId)))
                .Returns(Task.FromResult(apiModel));

            this._mapper.Setup(x => x.Map<IncomeAndExpenditure>(apiModel)).Returns(expected);

            IncomeAndExpenditure result = await this._budgetCalculatorService.
                GetPartiallySavedIncomeAndExpenditure(loggedInUserId, caseflowUserId);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetBudgetSummaryTest() 
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            Guid lowellReferenceSurrogateKey = Guid.NewGuid();
            string loggedInUserId = Guid.NewGuid().ToString();

            MonthlyIncome monthlyIncome = new MonthlyIncome() 
            {
                Benefits = 100,
                Other = 200,
                Pension = 300,
                Salary = 2400,
                Total = 3500
            };

            MonthlyOutgoings monthlyOutgoings = new MonthlyOutgoings() 
            {
                Expenditures = 1000,
                HouseholdBills = 2000,
                Total = 3000
            };

            decimal disposableIncome = 500;

            this._calculatorService.Setup(x => x.CalculateMonthlyIncome(iAndE)).Returns(monthlyIncome);
            this._calculatorService.Setup(x => x.CalculateMonthlyOutgoings(iAndE)).Returns(monthlyOutgoings);
            this._calculatorService.Setup(x => x.CalculateDisposableIncome(3500, 3000)).Returns(disposableIncome);

            BudgetSummary expected = new BudgetSummary()
            {
                IncomeTotal = 3500,
                Salary = 2400,
                Benefits = 100,
                Pension = 300,
                Other = 200,
                TotalExpenditure = 3000,
                HouseholdBills = 2000,
                Expenditure = 1000,
                DisposableIncome = 500,
                Frequency = "monthly",
                AnonUser = false,
                PriorityBillsInArrears = false
            };

            BudgetSummary result = this._budgetCalculatorService.GetBudgetSummary(iAndE, lowellReferenceSurrogateKey, loggedInUserId);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public async Task RemovePartialSavedTest() 
        {
            Guid caseflowUserId = Guid.NewGuid();

            this._restClient.Setup(x => x.PostNoResponseAsync(
                "TESTING/api/BudgetCalculator/RemovePartialSavedIncomeAndExpenditure", 
                It.Is<PartialBudgetApiRequest>(m => m.CaseflowUserId == caseflowUserId)))
                .Returns(Task.CompletedTask);

            await this._budgetCalculatorService.RemovePartialSaved(caseflowUserId);
        }

        [TestMethod]
        public async Task SaveIncomeAndExpenditureTest() 
        {
            string lowellReference = "123456789";
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            IncomeAndExpenditureApiModel apiModel = new IncomeAndExpenditureApiModel();

            this._mapper.Setup(x => x.Map<IncomeAndExpenditure, IncomeAndExpenditureApiModel>(iAndE))
                .Returns(apiModel);
            this._restClient.Setup(x => x.PostNoResponseAsync(
                "TESTING/api/BudgetCalculator/SaveIncomeAndExpenditure", apiModel)).Returns(Task.CompletedTask);

            await this._budgetCalculatorService.SaveIncomeAndExpenditure(iAndE, lowellReference);

            Assert.AreEqual(lowellReference, iAndE.LowellReference);
        }

        [TestMethod]
        public async Task PartiallySaveIncomeAndExpenditureTest() 
        {
            string lowellReference = "123456789";
            Guid caseflowUserId = Guid.NewGuid();
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();
            IncomeAndExpenditureApiModel apiModel = new IncomeAndExpenditureApiModel();

            this._mapper.Setup(x => x.Map<IncomeAndExpenditure, IncomeAndExpenditureApiModel>(iAndE))
                .Returns(apiModel);

            this._restClient.Setup(x => x.PostAsync<PartialBudgetApiModel, bool>(
                "TESTING/api/BudgetCalculator/PartialSaveIncomeAndExpenditure",
                It.Is<PartialBudgetApiModel>(
                    m => m.LowellReference == lowellReference && 
                    m.CaseflowUserId == caseflowUserId && 
                    m.PartialBudget == apiModel &&
                    Math.Abs(m.CreatedDate.Subtract(DateTime.UtcNow).TotalSeconds) < 5)))
                .Returns(Task.FromResult(true));

            Assert.IsTrue(await this._budgetCalculatorService
                .PartiallySaveIncomeAndExpenditure(iAndE, lowellReference, caseflowUserId));

            Assert.AreEqual(lowellReference, iAndE.LowellReference);
        }

    }
}
