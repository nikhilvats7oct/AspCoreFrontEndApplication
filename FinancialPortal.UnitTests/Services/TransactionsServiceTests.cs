using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using FinancialPortal.Web.Services.Interfaces;
using AutoMapper;
using System.Threading.Tasks;
using FinancialPortal.Web.Services;
using FinancialPortal.Web.Services.ApiModels;
using Newtonsoft.Json;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.UnitTests.Services
{
    [TestClass]
    public class TransactionsServiceTests
    {
        private Mock<IAccountsService> _accountsService;
        private PortalSetting _portalSettings;
        private Mock<IRestClient> _restClient;
        private Mock<IMapper> _mapper;

        private TransactionsService _service;

        [TestInitialize]
        public void TestInitialise()
        {
            this._accountsService = new Mock<IAccountsService>(MockBehavior.Strict);
            this._portalSettings = new PortalSetting() { GatewayEndpoint = "TestEndpoint/" };
            this._restClient = new Mock<IRestClient>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);

            this._service = new TransactionsService(this._accountsService.Object, this._portalSettings, 
                this._restClient.Object, this._mapper.Object);
        }

        [TestMethod]
        public async Task GetTransactionsTest()
        {
            string lowellRef = "12345678";
            string uri = $"{_portalSettings.GatewayEndpoint}api/ViewTransactions/GetTransactions";

            ApiTransactionsWrapper apiModel = new ApiTransactionsWrapper()
            {
                Payments = new List<ApiTransaction>()
                {
                    new ApiTransaction()
                    {
                        Amount = 111.11M,
                        Date = DateTime.Now.AddDays(-180),
                        Description = "Description1",
                        Type = "Type1",
                        RollingBalance = 999.99M
                    },
                    new ApiTransaction()
                    {
                        Amount = 222.22M,
                        Date = DateTime.Now.AddDays(-150),
                        Description = "Description2",
                        Type = "Type2",
                        RollingBalance = 888.88M
                    },
                    new ApiTransaction()
                    {
                        Amount = 333.33M,
                        Date = DateTime.Now.AddDays(-120),
                        Description = "Description3",
                        Type = "Type3",
                        RollingBalance = 777.77M
                    },
                    new ApiTransaction()
                    {
                        Amount = 444.44M,
                        Date = DateTime.Now.AddDays(-90),
                        Description = "Description4",
                        Type = "Type4",
                        RollingBalance = 666.66M
                    },
                    new ApiTransaction()
                    {
                        Amount = 333.33M,
                        Date = DateTime.Now.AddDays(-60),
                        Description = "Description5",
                        Type = "Type5",
                        RollingBalance = 555.55M
                    },
                }
            };

            List<Transaction> serviceModel = new List<Transaction>()
            {
                    new Transaction()
                    {
                        Amount = 111.11M,
                        Date = DateTime.Now.AddDays(-180),
                        Description = "Description1",
                        Type = "Type1",
                        RollingBalance = 999.99M
                    },
                    new Transaction()
                    {
                        Amount = 222.22M,
                        Date = DateTime.Now.AddDays(-150),
                        Description = "Description2",
                        Type = "Type2",
                        RollingBalance = 888.88M
                    },
                    new Transaction()
                    {
                        Amount = 333.33M,
                        Date = DateTime.Now.AddDays(-120),
                        Description = "Description3",
                        Type = "Type3",
                        RollingBalance = 777.77M
                    },
                    new Transaction()
                    {
                        Amount = 444.44M,
                        Date = DateTime.Now.AddDays(-90),
                        Description = "Description4",
                        Type = "Type4",
                        RollingBalance = 666.66M
                    },
                    new Transaction()
                    {
                        Amount = 333.33M,
                        Date = DateTime.Now.AddDays(-60),
                        Description = "Description5",
                        Type = "Type5",
                        RollingBalance = 555.55M
                    },
            };

            this._restClient.Setup(x =>
                x.PostAsync<ApiAccountReferenceModel, ApiTransactionsWrapper>(uri, It.Is<ApiAccountReferenceModel>(m => 
                    m.AccountReference == "12345678"))).Returns(Task.FromResult(apiModel));
            this._mapper.Setup(x => x.Map<List<ApiTransaction>, List<Transaction>>(apiModel.Payments)).Returns(serviceModel);

            List<Transaction> result = await _service.GetTransactions(lowellRef);

            string expectedString = JsonConvert.SerializeObject(serviceModel);
            string resultString = JsonConvert.SerializeObject(result);

            Assert.AreEqual(expectedString, resultString);
        }

    }
}
