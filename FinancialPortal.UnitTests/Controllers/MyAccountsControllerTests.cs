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
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace FinancialPortal.UnitTests.Controllers
{
    [TestClass]
    public class MyAccountsControllerTests
    {
        private Mock<ILogger<MyAccountsController>> _logger;
        private Mock<IConfiguration> _configuration;
        private Mock<IDistributedCache> _distributedCache;
        private Mock<IGtmService> _gtmService;
        private Mock<IWebActivityService> _webActivityService;
        private Mock<IAccountsService> _accountsService;
        private Mock<IMapper> _mapper;
        private Mock<IApplicationSessionState> _sessionState;
        private string _caseflowUserId;

        private MyAccountsController _controller;

        private void VerifyAll()
        {
            this._logger.VerifyAll();
            this._configuration.VerifyAll();
            this._distributedCache.VerifyAll();
            this._gtmService.VerifyAll();
            this._webActivityService.VerifyAll();
            this._accountsService.VerifyAll();
            this._mapper.VerifyAll();
            this._sessionState.VerifyAll();
        }

        [TestInitialize]
        public void TestInitialise()
        {
            this._logger = new Mock<ILogger<MyAccountsController>>(MockBehavior.Strict);
            this._configuration = new Mock<IConfiguration>(MockBehavior.Strict);
            this._distributedCache = new Mock<IDistributedCache>(MockBehavior.Strict);
            this._gtmService = new Mock<IGtmService>(MockBehavior.Strict);
            this._webActivityService = new Mock<IWebActivityService>(MockBehavior.Strict);
            this._accountsService = new Mock<IAccountsService>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);
            this._sessionState = new Mock<IApplicationSessionState>(MockBehavior.Strict);

            this._caseflowUserId = Guid.NewGuid().ToString();

            this._controller = new MyAccountsController(this._logger.Object, this._configuration.Object, 
                this._distributedCache.Object, this._gtmService.Object, this._webActivityService.Object, 
                this._accountsService.Object, this._sessionState.Object, this._mapper.Object);

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
            List<AccountSummary> accountSummaries = new List<AccountSummary>()
            {
                new AccountSummary(){ AccountReference = "11111111", OutstandingBalance = 111.11M },
                new AccountSummary(){ AccountReference = "22222222", OutstandingBalance = 222.22M },
                new AccountSummary(){ AccountReference = "33333333", OutstandingBalance = 333.33M },
                new AccountSummary(){ AccountReference = "44444444", OutstandingBalance = 444.44M },
                new AccountSummary(){ AccountReference = "55555555", OutstandingBalance = 555.55M }
            };

            Dictionary<string, Guid> surrogateKeysByLowellReference = new Dictionary<string, Guid>()
            {
                { "11111111", Guid.NewGuid() },
                { "22222222", Guid.NewGuid() },
                { "33333333", Guid.NewGuid() },
                { "44444444", Guid.NewGuid() },
                { "55555555", Guid.NewGuid() },
            };

            CustomerSummary customerSummary = new CustomerSummary()
            {
                Accounts = accountSummaries,
                SurrogateKeysByLowellReference = surrogateKeysByLowellReference,
                IncomeAndExpenditure = new IncomeAndExpenditure()
            };

            MyAccountsVm myAccountsVm = new MyAccountsVm()
            {
                Accounts = new List<AccountSummaryVm>()
                {
                    new AccountSummaryVm(){ AccountReferenceText = "11111111", OutstandingBalanceText = "£111.11" },
                    new AccountSummaryVm(){ AccountReferenceText = "22222222", OutstandingBalanceText = "£222.22" },
                    new AccountSummaryVm(){ AccountReferenceText = "33333333", OutstandingBalanceText = "£333.33" },
                    new AccountSummaryVm(){ AccountReferenceText = "44444444", OutstandingBalanceText = "£444.44" },
                    new AccountSummaryVm(){ AccountReferenceText = "55555555", OutstandingBalanceText = "£555.55" },
                },
            };

            this._accountsService.Setup(x => x.GetAccounts(this._caseflowUserId)).Returns(Task.FromResult(accountSummaries));
            this._sessionState.Setup(x => x.AddLowellReferenceSurrogateKeys(It.Is<List<string>>(a =>
            a.Count == 5 && a[0] == "11111111" && a[1] == "22222222" && a[2] == "33333333" && a[3] == "44444444" && a[4] == "55555555")))
                .Returns(surrogateKeysByLowellReference);
            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(surrogateKeysByLowellReference["11111111"]))
                .Returns("11111111");
            this._accountsService.Setup(x => x.CreateCustomerSummary(accountSummaries, "11111111", surrogateKeysByLowellReference))
                .Returns(Task.FromResult(customerSummary));
            this._mapper.Setup(x => x.Map<CustomerSummary, MyAccountsVm>(customerSummary)).Returns(myAccountsVm);
            this._gtmService.Setup(x => x.RaiseMyAccountsPageViewedEvent(myAccountsVm, this._caseflowUserId)).Verifiable();
            this._webActivityService.Setup(x => x.LogMyAccountsPageViewed("11111111", this._caseflowUserId)).Returns(Task.CompletedTask);

            ViewResult result = (ViewResult)await this._controller.Index();

            Assert.AreSame(myAccountsVm, result.Model);

            this._gtmService.Verify(x => x.RaiseMyAccountsPageViewedEvent(myAccountsVm, this._caseflowUserId), Times.Once);
            this._webActivityService.Verify(x => x.LogMyAccountsPageViewed("11111111", this._caseflowUserId), Times.Once);

            this.VerifyAll();
        }

        [TestMethod]
        public async Task DetailsTest()
        {
            Guid id = Guid.NewGuid();
            string lowellRef = "12345678";

            Account account = new Account()
            {
                AccountReference = "12345678",
                OutstandingBalance = 999.99M,
            };

            MyAccountsDetailVm vm = new MyAccountsDetailVm()
            {
                AccountReferenceText = "12345678",
                OutstandingBalanceText = "£999.99"
            };

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns("12345678");
            this._accountsService.Setup(x => x.GetAccount(this._caseflowUserId, lowellRef)).Returns(Task.FromResult(account));
            this._mapper.Setup(x => x.Map<Account, MyAccountsDetailVm>(account)).Returns(vm);
            this._gtmService.Setup(x => x.RaiseAccountDetailsViewedEvent(vm, this._caseflowUserId, "Regular Account")).Verifiable();
            this._webActivityService.Setup(x => x.LogAccountDetailsViewed("12345678", this._caseflowUserId)).Returns(Task.CompletedTask);
            this._sessionState.Setup(x => x.GetLoggedInLowellRef()).Returns("12345678");

            ViewResult result = (ViewResult)await this._controller.Details(id);

            this._gtmService.Verify(x => x.RaiseAccountDetailsViewedEvent(vm, this._caseflowUserId, "Regular Account"), Times.Once);
            this.VerifyAll();

            Assert.AreSame(vm, result.Model);
            Assert.AreEqual(this._caseflowUserId, ((MyAccountsDetailVm)result.Model).LoggedInUserId);
            Assert.AreEqual("12345678", ((MyAccountsDetailVm)result.Model).LoggedInLowellRef);
            Assert.AreEqual(id, ((MyAccountsDetailVm)result.Model).LowellReferenceSurrogateKey);
        }


    }
}
