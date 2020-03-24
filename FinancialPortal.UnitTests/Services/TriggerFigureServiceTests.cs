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
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.UnitTests.Services
{
    [TestClass]
    public class TriggerFigureServiceTests
    {
        private Mock<IRestClient> _restClient;
        private Mock<IMapper> _mapper;
        private PortalSetting _portalSettings;
        private Mock<IApplicationSessionState> _sessionState;

        private TriggerFigureService _triggerFigureService;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._portalSettings = new PortalSetting() 
            {
                GatewayEndpoint = "TESTING/"
            };

            this._restClient = new Mock<IRestClient>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);
            this._sessionState = new Mock<IApplicationSessionState>(MockBehavior.Strict);

            this._triggerFigureService = new TriggerFigureService(
                this._restClient.Object, this._portalSettings, this._mapper.Object);
        }

        [TestMethod]
        public async Task GetExpenditureMetricsTest_MetricsCached()         
        {
            ExpenditureMetrics expenditureMetrics = new ExpenditureMetrics()
            {
                CommsAndLeisure = new ExpenditureMetric(),
                FoodAndHousekeeping = new ExpenditureMetric(),
                PersonalCosts = new ExpenditureMetric()
            };

            this._sessionState.Setup(x => x.GetExpenditureMetrics()).Returns(expenditureMetrics);

            ExpenditureMetrics result = await this._triggerFigureService.GetExpenditureMetrics(this._sessionState.Object);

            Assert.AreEqual(expenditureMetrics, result);
        }

        [TestMethod]
        public async Task GetExpenditureMetricsTest_MetricsNotCached()
        {
            ExpenditureMetrics expenditureMetrics = new ExpenditureMetrics()
            {
                CommsAndLeisure = new ExpenditureMetric(),
                FoodAndHousekeeping = new ExpenditureMetric(),
                PersonalCosts = new ExpenditureMetric()
            };

            ExpenditureMetricsApiModel apiModel = new ExpenditureMetricsApiModel();

            this._sessionState.Setup(x => x.GetExpenditureMetrics()).Returns<ExpenditureMetrics>(null);

            this._restClient.Setup(x => x.GetAsync<ExpenditureMetricsApiModel>("TESTING/api/BudgetCalculator/GetTriggerFigures"))
                .Returns(Task.FromResult(apiModel));

            this._mapper.Setup(x => x.Map<ExpenditureMetrics>(apiModel)).Returns(expenditureMetrics);

            this._sessionState.Setup(x => x.SaveExpenditureMetrics(expenditureMetrics));

            ExpenditureMetrics result = await this._triggerFigureService.GetExpenditureMetrics(this._sessionState.Object);

            Assert.AreEqual(expenditureMetrics, result);
        }

        [TestMethod]
        public void CalculateTriggerFigureTest() 
        {
            ExpenditureMetric metric = new ExpenditureMetric() 
            {
                AdditionalAdult = 0.5M,
                Adult = 0.6M,
                Children14To18 = 0.7M,
                ChildrenUnder14 = 0.8M,
            };

            decimal result = this._triggerFigureService.CalculateTriggerFigure(metric, 2, 3, 4);

            Assert.AreEqual(6.3M, result);
        }

        [TestMethod]
        public void CalculateMaxTriggerValueTest() 
        {
            Assert.AreEqual(120.00M, this._triggerFigureService.CalculateMaxTriggerValue(100));
        }

        [TestMethod]
        public void CalculateMinTriggerValueTest()
        {
            Assert.AreEqual(80.00M, this._triggerFigureService.CalculateMinTriggerValue(100));
        }

    }
}
