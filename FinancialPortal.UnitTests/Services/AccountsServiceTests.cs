using AutoMapper;
using FinancialPortal.Web.Services;
using FinancialPortal.Web.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.UnitTests.Services
{
    [TestClass]
    public class AccountsServiceTests
    {
        private PortalSetting _portalSettings;
        private Mock<IRestClient> _restClient;
        private Mock<IMapper> _mapper;
        private Mock<IBudgetCalculatorService> _budgetCalculatorService;

        private AccountsService _service;

        [TestInitialize]
        public void TestInitialise()
        {
            this._portalSettings = new PortalSetting() { GatewayEndpoint = "TestEndpoint/" };
            this._restClient = new Mock<IRestClient>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);
            this._budgetCalculatorService = new Mock<IBudgetCalculatorService>();

            this._service = new AccountsService(this._portalSettings, this._restClient.Object, this._mapper.Object, this._budgetCalculatorService.Object);
        }

        [TestMethod]
        public async Task GetAccountsTest()
        {
            string userId = Guid.NewGuid().ToString();
            string uri = $"{_portalSettings.GatewayEndpoint}api/MyAccounts/GetMyAccounts";

            ApiAccountSummaries apiModel = new ApiAccountSummaries()
            {
                Accounts = new List<ApiAccountSummary>()
                {
                    new ApiAccountSummary(){ AccountReference = "11111111" },
                    new ApiAccountSummary(){ AccountReference = "22222222" },
                    new ApiAccountSummary(){ AccountReference = "33333333" },
                    new ApiAccountSummary(){ AccountReference = "44444444" },
                    new ApiAccountSummary(){ AccountReference = "55555555" },
                }
            };

            AccountSummaries serviceModel = new AccountSummaries()
            {
                Accounts = new List<AccountSummary>()
                {
                    new AccountSummary(){ AccountReference = "11111111" },
                    new AccountSummary(){ AccountReference = "22222222" },
                    new AccountSummary(){ AccountReference = "33333333" },
                    new AccountSummary(){ AccountReference = "44444444" },
                    new AccountSummary(){ AccountReference = "55555555" },
                }
            };

            List<AccountSummary> expected = new List<AccountSummary>()
            {
                new AccountSummary(){ AccountReference = "11111111" },
                new AccountSummary(){ AccountReference = "22222222" },
                new AccountSummary(){ AccountReference = "33333333" },
                new AccountSummary(){ AccountReference = "44444444" },
                new AccountSummary(){ AccountReference = "55555555" },
            };

            this._restClient.Setup(x => x.PostAsync<ApiUserIdModel, ApiAccountSummaries>(uri, It.Is<ApiUserIdModel>(m => m.UserId == userId))).Returns(Task.FromResult(apiModel));
            this._mapper.Setup(x => x.Map<ApiAccountSummaries, AccountSummaries>(apiModel)).Returns(serviceModel);

            List<AccountSummary> result = await _service.GetAccounts(userId);

            string expectedStr = JsonConvert.SerializeObject(expected);
            string resultStr = JsonConvert.SerializeObject(result);

            Assert.AreEqual(expectedStr, resultStr);
        }

        [TestMethod]
        public async Task GetAccountTest()
        {
            string userId = Guid.NewGuid().ToString();
            string lowellRef = "12345678";
            string uri = $"{_portalSettings.GatewayEndpoint}api/MyAccounts/GetMyAccountsDetail";

            ApiAccount apiModel = new ApiAccount()
            {
                AccountReference = "12345678"
            };

            Account serviceModel = new Account()
            {
                AccountReference = "12345678"
            };

            Account expected = new Account()
            {
                AccountReference = "12345678"
            };

            _restClient.Setup(x => x.PostAsync<AccountDetailApiRequest, ApiAccount>(uri, It.Is<AccountDetailApiRequest>(m => m.UserId == userId && m.LowellReference == lowellRef))).Returns(Task.FromResult(apiModel));
            _mapper.Setup(x => x.Map<ApiAccount, Account>(apiModel)).Returns(serviceModel);

            Account result = await _service.GetAccount(userId, lowellRef);

            string expectedStr = JsonConvert.SerializeObject(expected);
            string resultStr = JsonConvert.SerializeObject(result);

            Assert.AreEqual(expectedStr, resultStr);
        }

        [TestMethod]
        public async Task CreateCustomerSummaryTest()
        {
            List<AccountSummary> accountSummaries = new List<AccountSummary>()
            {
                new AccountSummary(){ AccountReference = "11111111" },
                new AccountSummary(){ AccountReference = "22222222" },
                new AccountSummary(){ AccountReference = "33333333" },
                new AccountSummary(){ AccountReference = "44444444" },
                new AccountSummary(){ AccountReference = "55555555" },
            };

            Dictionary<string, Guid> surrogateKeysByLowellReference = new Dictionary<string, Guid>()
            {
                { "11111111", Guid.NewGuid() },
                { "22222222", Guid.NewGuid() },
                { "33333333", Guid.NewGuid() },
                { "44444444", Guid.NewGuid() },
                { "55555555", Guid.NewGuid() },
            };

            string lowellRef = "12345678";

            IncomeAndExpenditure incomeAndExpenditure = new IncomeAndExpenditure()
            {
                Created = DateTime.Now,
                DisposableIncome = 999.99M
            };

            _budgetCalculatorService.Setup(x => x.GetSavedIncomeAndExpenditure(lowellRef))
                .Returns(Task.FromResult(incomeAndExpenditure));

            CustomerSummary expected = new CustomerSummary()
            {
                Accounts = accountSummaries,
                IncomeAndExpenditure = incomeAndExpenditure,
                SurrogateKeysByLowellReference = surrogateKeysByLowellReference
            };

            CustomerSummary result = await _service.CreateCustomerSummary(accountSummaries, lowellRef, surrogateKeysByLowellReference);

            string expectedStr = JsonConvert.SerializeObject(expected);
            string resultStr = JsonConvert.SerializeObject(result);

            Assert.AreEqual(expectedStr, resultStr);
        }

    }
}
