using FinancialPortal.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Framework.Pdf.Aspose;
using Microsoft.Extensions.Caching.Distributed;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.Controllers
{
    [TestClass]
    public class ViewTransactionsControllerTests
    {
        private Mock<ILogger<ViewTransactionsController>> _logger;
        private Mock<IConfiguration> _config;
        private Mock<ITransactionsService> _transactionsService;
        private PortalSetting _portalSettings;
        private Mock<IGtmService> _gtmService;
        private Mock<IPdfGenerator<PdfViewModel>> _pdfGenerator;
        private Mock<IDistributedCache> _distributedCache;
        private Mock<IWebActivityService> _webActivityService;
        private Mock<IMapper> _mapper;
        private Mock<IApplicationSessionState> _sessionState;
        private Mock<IAccountsService> _accountsService;

        private string _caseflowUserId;

        private ViewTransactionsController _controller;

        [TestInitialize]
        public void TestInitialise()
        {
            this._logger = new Mock<ILogger<ViewTransactionsController>>(MockBehavior.Strict);
            this._config = new Mock<IConfiguration>(MockBehavior.Strict);
            this._transactionsService = new Mock<ITransactionsService>(MockBehavior.Strict);
            this._portalSettings = new PortalSetting()
            {
                PageSizeViewTransactions = 5,
                ViewTransactionsPageSize = 5
            };
            this._gtmService = new Mock<IGtmService>(MockBehavior.Strict);
            this._pdfGenerator = new Mock<IPdfGenerator<PdfViewModel>>(MockBehavior.Strict);
            this._distributedCache = new Mock<IDistributedCache>(MockBehavior.Strict);
            this._webActivityService = new Mock<IWebActivityService>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);
            this._sessionState = new Mock<IApplicationSessionState>(MockBehavior.Strict);
            this._accountsService = new Mock<IAccountsService>(MockBehavior.Strict);

            this._caseflowUserId = Guid.NewGuid().ToString();

            this._controller = new ViewTransactionsController(this._logger.Object, this._config.Object,
                this._transactionsService.Object, this._portalSettings, this._gtmService.Object,
                this._pdfGenerator.Object, this._distributedCache.Object, this._webActivityService.Object,
                this._mapper.Object, this._sessionState.Object, this._accountsService.Object);

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

        private void VerifyAll()
        {
            this._logger.VerifyAll();
            this._config.VerifyAll();
            this._transactionsService.VerifyAll();
            this._gtmService.VerifyAll();
            this._pdfGenerator.VerifyAll();
            this._distributedCache.VerifyAll();
            this._webActivityService.VerifyAll();
            this._mapper.VerifyAll();
            this._sessionState.VerifyAll();
            this._accountsService.VerifyAll();
        }

        [TestMethod]
        public async Task TransactionsTest_IdNull()
        {
            TransactionsVm vm = new TransactionsVm(){};

            RedirectToActionResult result = (RedirectToActionResult)await this._controller.Transactions(vm, 1);

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);

            this.VerifyAll();
        }

        [TestMethod]
        public async Task TransactionsTest_ModelStateInvalid()
        {
            TransactionsVm vm = new TransactionsVm() { };
            string id = Guid.NewGuid().ToString();

            this._controller.RouteData.Values.Add("id", id);
            this._controller.ModelState.AddModelError("Testing...", "Testing");

            ViewResult result = (ViewResult)await this._controller.Transactions(vm, 1);

            Assert.AreSame(result.Model, vm);

            this.VerifyAll();
        }

        [TestMethod]
        public async Task TransactionsTest_LowellRefNull()
        {
            TransactionsVm vm = new TransactionsVm() { };
            Guid id = Guid.NewGuid();

            this._controller.RouteData.Values.Add("id", id);

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns<string>(null);

            try
            {
                await this._controller.Transactions(vm, 1);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("No lowell reference found in cache", ex.Message);
                this.VerifyAll();
                return;
            }

            Assert.Fail("No exception thrown");
        }

        [TestMethod]
        public async Task TransactionsTest_NothingCached()
        {
            TransactionsVm vm = new TransactionsVm() { };
            Guid id = Guid.NewGuid();
            string lowellRef = "12345678";

            this._controller.RouteData.Values.Add("id", id);

            Account account = new Account()
            {
                AccountReference = "12345678",
                OutstandingBalance = 999.99M,
                OriginalCompany = "OriginalCompany",
            };

            List<Transaction> transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    Amount = 111.11M,
                    Date = DateTime.Now.AddDays(-28),
                    Description = "Description",
                    Type = "Type",
                    RollingBalance = 999.99M
                }
            };

            List<TransactionVm> transactionsVm = new List<TransactionVm>()
            {
                new TransactionVm()
                {
                    AmountText = "£111.11",
                    DateText = DateTime.Now.AddDays(-28).ToString(),
                    Description = "Description",
                    RollingBalanceText = "£999.99",
                }
            };

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns(lowellRef);
            this._sessionState.Setup(x => x.GetAccount(lowellRef)).Returns<Account>(null);
            this._accountsService.Setup(x => x.GetAccount(this._caseflowUserId, lowellRef)).Returns(Task.FromResult(account));
            this._sessionState.Setup(x => x.SaveAccount(account, lowellRef)).Verifiable();
            this._sessionState.Setup(x => x.GetTransactions(lowellRef)).Returns<List<Transaction>>(null);
            this._transactionsService.Setup(x => x.GetTransactions(lowellRef)).Returns(Task.FromResult(transactions));
            this._sessionState.Setup(x => x.SaveTransactions(transactions, lowellRef)).Verifiable();
            this._gtmService.Setup(x => x.RaiseTransactionsViewedEvent(vm, _caseflowUserId, "Regular Account")).Verifiable();
            this._webActivityService.Setup(x => x.LogAllTransactionsViewed("12345678", this._caseflowUserId)).Returns(Task.CompletedTask);
            this._mapper.Setup(x => x.Map<List<Transaction>, List<TransactionVm>>(transactions)).Returns(transactionsVm);
            this._sessionState.Setup(x => x.GetLoggedInLowellRef()).Returns("12345678");

            ViewResult result = (ViewResult)await this._controller.Transactions(vm, 1);

            Assert.AreSame(vm, result.Model);
            Assert.AreEqual("OriginalCompany", vm.AccountName);
            Assert.AreEqual(999.99M, vm.AccountBalance);
            Assert.AreEqual("12345678", vm.AccountReference);
            Assert.AreEqual(this._caseflowUserId, vm.LoggedInUserID);
            Assert.AreEqual("12345678", vm.LoggedInLowellRef);

            Assert.AreEqual(1, vm.PagedList.Count);
            Assert.AreEqual(1, vm.PagedList.TotalItemCount);
            Assert.AreEqual(1, vm.PagedList.PageNumber);
            Assert.AreEqual(5, vm.PagedList.PageSize);
            Assert.AreEqual(1, vm.PagedList.PageCount);

            this.VerifyAll();
        }

        [TestMethod]
        public async Task TransactionsTest_EverythingCached()
        {
            TransactionsVm vm = new TransactionsVm() { };
            Guid id = Guid.NewGuid();
            string lowellRef = "12345678";

            this._controller.RouteData.Values.Add("id", id);

            Account account = new Account()
            {
                AccountReference = "12345678",
                OutstandingBalance = 999.99M,
                OriginalCompany = "OriginalCompany",
            };

            List<Transaction> transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    Amount = 111.11M,
                    Date = DateTime.Now.AddDays(-28),
                    Description = "Description",
                    Type = "Type",
                    RollingBalance = 999.99M
                }
            };

            List<TransactionVm> transactionsVm = new List<TransactionVm>()
            {
                new TransactionVm()
                {
                    AmountText = "£111.11",
                    DateText = DateTime.Now.AddDays(-28).ToString(),
                    Description = "Description",
                    RollingBalanceText = "£999.99",
                }
            };

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns(lowellRef);
            this._sessionState.Setup(x => x.GetAccount(lowellRef)).Returns(account);
            this._sessionState.Setup(x => x.GetTransactions(lowellRef)).Returns(transactions);
            this._gtmService.Setup(x => x.RaiseTransactionsViewedEvent(vm, _caseflowUserId, "Regular Account")).Verifiable();
            this._webActivityService.Setup(x => x.LogAllTransactionsViewed("12345678", this._caseflowUserId)).Returns(Task.CompletedTask);
            this._mapper.Setup(x => x.Map<List<Transaction>, List<TransactionVm>>(transactions)).Returns(transactionsVm);
            this._sessionState.Setup(x => x.GetLoggedInLowellRef()).Returns("12345678");

            ViewResult result = (ViewResult)await this._controller.Transactions(vm, 1);

            Assert.AreSame(vm, result.Model);
            Assert.AreEqual("OriginalCompany", vm.AccountName);
            Assert.AreEqual(999.99M, vm.AccountBalance);
            Assert.AreEqual("12345678", vm.AccountReference);
            Assert.AreEqual(this._caseflowUserId, vm.LoggedInUserID);
            Assert.AreEqual("12345678", vm.LoggedInLowellRef);

            Assert.AreEqual(1, vm.PagedList.Count);
            Assert.AreEqual(1, vm.PagedList.TotalItemCount);
            Assert.AreEqual(1, vm.PagedList.PageNumber);
            Assert.AreEqual(5, vm.PagedList.PageSize);
            Assert.AreEqual(1, vm.PagedList.PageCount);

            this.VerifyAll();
        }

        [TestMethod]
        public async Task DownloadTest_IdNull()
        {
            string accountRef = "12345678";

            RedirectToActionResult result = (RedirectToActionResult)await this._controller.Download(accountRef);

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("MyAccounts", result.ControllerName);

            this.VerifyAll();
        }

        [TestMethod]
        public async Task DownloadTest_LowellRefNull()
        {
            string accountRef = "12345678";
            Guid id = Guid.NewGuid();

            this._controller.RouteData.Values.Add("id", id);

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns<string>(null);

            try
            {
                await this._controller.Download(accountRef);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("No lowell reference found in cache", ex.Message);
                this.VerifyAll();
                return;
            }

            Assert.Fail("No exception thrown");
        }

        [TestMethod]
        public async Task DownloadTest_NothingCached()
        {
            string accountRef = "12345678";
            Guid id = Guid.NewGuid();
            string lowellRef = "12345678";

            this._controller.RouteData.Values.Add("id", id);

            Account account = new Account()
            {
                AccountReference = "12345678",
                OutstandingBalance = 999.99M,
                OriginalCompany = "OriginalCompany",
            };

            List<Transaction> transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    Amount = 111.11M,
                    Date = DateTime.Now.AddDays(-28),
                    Description = "Description",
                    Type = "Type",
                    RollingBalance = 999.99M
                }
            };

            List<TransactionVm> transactionsVm = new List<TransactionVm>()
            {
                new TransactionVm()
                {
                    AmountText = "£111.11",
                    DateText = DateTime.Now.AddDays(-28).ToString(),
                    Description = "Description",
                    RollingBalanceText = "£999.99",
                }
            };

            byte[] pdf = new byte[0];

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns(lowellRef);
            this._sessionState.Setup(x => x.GetAccount(lowellRef)).Returns<Account>(null);
            this._accountsService.Setup(x => x.GetAccount(this._caseflowUserId, lowellRef)).Returns(Task.FromResult(account));
            this._sessionState.Setup(x => x.SaveAccount(account, lowellRef)).Verifiable();
            this._sessionState.Setup(x => x.GetTransactions(lowellRef)).Returns<List<Transaction>>(null);
            this._transactionsService.Setup(x => x.GetTransactions(lowellRef)).Returns(Task.FromResult(transactions));
            this._sessionState.Setup(x => x.SaveTransactions(transactions, lowellRef)).Verifiable();
            this._webActivityService.Setup(x => x.LogStatementDownloaded("12345678", this._caseflowUserId)).Returns(Task.CompletedTask);
            this._mapper.Setup(x => x.Map<List<Transaction>, List<TransactionVm>>(transactions)).Returns(transactionsVm);
            this._pdfGenerator.Setup(x => x.Generate(It.Is<PdfViewModel>(
                m => m.AccountBalance == 999.99M && m.AccountName == "OriginalCompany" && 
                m.AccountReference == "12345678" && m.PaymentDetails == transactionsVm))).Returns(pdf);
            
            FileStreamResult result = (FileStreamResult)await this._controller.Download(accountRef);

            Assert.AreEqual("application/pdf", result.ContentType);
            Assert.AreEqual($"12345678_{DateTime.UtcNow:ddMMMyyyyHHmmss}.pdf", result.FileDownloadName);

            this.VerifyAll();
        }

        [TestMethod]
        public async Task DownloadTest_EverythingCached()
        {
            string accountRef = "12345678";
            Guid id = Guid.NewGuid();
            string lowellRef = "12345678";

            this._controller.RouteData.Values.Add("id", id);

            Account account = new Account()
            {
                AccountReference = "12345678",
                OutstandingBalance = 999.99M,
                OriginalCompany = "OriginalCompany",
            };

            List<Transaction> transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    Amount = 111.11M,
                    Date = DateTime.Now.AddDays(-28),
                    Description = "Description",
                    Type = "Type",
                    RollingBalance = 999.99M
                }
            };

            List<TransactionVm> transactionsVm = new List<TransactionVm>()
            {
                new TransactionVm()
                {
                    AmountText = "£111.11",
                    DateText = DateTime.Now.AddDays(-28).ToString(),
                    Description = "Description",
                    RollingBalanceText = "£999.99",
                }
            };

            byte[] pdf = new byte[0];

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns(lowellRef);
            this._sessionState.Setup(x => x.GetAccount(lowellRef)).Returns(account);
            this._sessionState.Setup(x => x.GetTransactions(lowellRef)).Returns(transactions);
            this._webActivityService.Setup(x => x.LogStatementDownloaded("12345678", this._caseflowUserId)).Returns(Task.CompletedTask);
            this._mapper.Setup(x => x.Map<List<Transaction>, List<TransactionVm>>(transactions)).Returns(transactionsVm);
            this._pdfGenerator.Setup(x => x.Generate(It.Is<PdfViewModel>(
                m => m.AccountBalance == 999.99M && m.AccountName == "OriginalCompany" &&
                m.AccountReference == "12345678" && m.PaymentDetails == transactionsVm))).Returns(pdf);

            FileStreamResult result = (FileStreamResult)await this._controller.Download(accountRef);

            Assert.AreEqual("application/pdf", result.ContentType);
            Assert.AreEqual($"12345678_{DateTime.UtcNow:ddMMMyyyyHHmmss}.pdf", result.FileDownloadName);

            this.VerifyAll();
        }
    }
}
