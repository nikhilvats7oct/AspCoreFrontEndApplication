using AutoMapper;
using FinancialPortal.Web.Controllers;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FinancialPortal.Web.Services;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.UnitTests.Controllers
{
    [TestClass]
    public class OpenWrksControllerTests
    {
        private Mock<ILogger<OpenWrksController>> _logger;
        private Mock<IDistributedCache> _distributedCache;
        private Mock<IConfiguration> _config;
        private Mock<IMapper> _mapper;
        private Mock<IApplicationSessionState> _sessionState;
        private Mock<IWebActivityService> _webActivityService;
        private Mock<IBudgetCalculatorService> _budgetCalculatorService;
        private Mock<IAccountsService> _accountService;
        private Mock<IPortalCryptoAlgorithm> _portalCryptoAlgorithm;
        private Mock<IOpenWrksService> _openWrksService;
        private OpenWrksSetting _openWrksSettings;
        private string _caseflowUserId;
        private PortalSetting _portalSetting;

        private OpenWrksController _controller;

        [TestInitialize]
        public void TestInitialise()
        {
            this._logger = new Mock<ILogger<OpenWrksController>>(MockBehavior.Default);
            this._distributedCache = new Mock<IDistributedCache>(MockBehavior.Strict);
            this._config = new Mock<IConfiguration>(MockBehavior.Strict);
            this._sessionState = new Mock<IApplicationSessionState>(MockBehavior.Strict);
            this._webActivityService = new Mock<IWebActivityService>(MockBehavior.Strict);
            this._openWrksService = new Mock<IOpenWrksService>(MockBehavior.Strict);
            this._openWrksService = new Mock<IOpenWrksService>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);
            this._budgetCalculatorService = new Mock<IBudgetCalculatorService>(MockBehavior.Strict);
            this._openWrksSettings = new OpenWrksSetting {UseLandingPage = true};
            this._caseflowUserId = Guid.NewGuid().ToString();
            this._accountService = new Mock<IAccountsService>();
            this._portalSetting = new PortalSetting { Features = new Features() { EnableOpenWrks = true } };
            this._portalCryptoAlgorithm = new Mock<IPortalCryptoAlgorithm>();

            this._controller = new OpenWrksController(_logger.Object,
                                                      _distributedCache.Object, 
                                                      _config.Object, 
                                                      _openWrksSettings, 
                                                      _sessionState.Object, 
                                                      _webActivityService.Object, 
                                                      _accountService.Object,
                                                      _portalSetting,
                                                      _budgetCalculatorService.Object, 
                                                      _portalCryptoAlgorithm.Object,
                                                      _openWrksService.Object);

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
        public async Task IndexTest_LoggedIn()
        {
            ////Guid id = Guid.NewGuid();
            ////this._controller.RouteData.Values["id"] = id.ToString();
            ////string lowellRef = "123456789";

            ////OpenWrksInvitationResponse openWrksResponse = new OpenWrksInvitationResponse()
            ////{
            ////    Link = "OpenWrksLink",
            ////    CustomerReference = "123456789",
            ////    InvitationId = "InvitationId"
            ////};

            ////////LowellReference = lowellReference,
            ////////        RedirectUri = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/OpenWrks/BudgetComplete",
            ////////        UserId = caseflowUserId

            ////this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns(lowellRef);
            ////this._openWrksService.Setup(x => x.SendInvitationRequest(It.Is<OpenWrksInvitationRequest>(
            ////    r => r.LowellReference == lowellRef && r.RedirectUri == ":///OpenWrks/BudgetComplete" && r.UserId == this._caseflowUserId)))
            ////    .Returns(Task.FromResult(openWrksResponse));
            ////this._webActivityService.Setup(x => x.LogOpenBankingAccessed(lowellRef, this._caseflowUserId)).Returns(Task.CompletedTask);

            ////RedirectResult result = (RedirectResult)await this._controller.Index();

            ////Assert.AreEqual("OpenWrksLink", result.Url);
        }

        // TODO: Fix this. [TestMethod]
        public async Task IndexTest_NotLoggedIn()
        {
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                    {
                        new ClaimsIdentity(new List<Claim>()
                        {
                            new Claim("testing...", _caseflowUserId)
                        }, "testing...")
                    })
                },
                RouteData = new RouteData()
            };

            _controller.ControllerContext = context;

            Guid id = Guid.NewGuid();
            this._controller.RouteData.Values["id"] = id.ToString();

            this._sessionState.Setup(x => x.GetLowellReferenceFromSurrogate(id)).Returns<string>(null);

            Assert.ThrowsException<System.Exception>(()=> this._controller.Index());

          
        }

        [TestMethod]
        public async Task BudgetCompleteTest_NoErrorCode()
        {
            Guid lowellSurrogateKey = Guid.NewGuid();
            string lowellReference = "12345678";

            string customerReference = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._caseflowUserId))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            this._sessionState.Setup(ss => ss.GetTopLowellSurrogateKey()).Returns(lowellSurrogateKey);
            this._sessionState.Setup(ss => ss.GetTopLowellReference()).Returns(lowellReference);
            this._portalCryptoAlgorithm.Setup(x => x.DecryptUsingAes(It.IsAny<string>())).Returns(this._caseflowUserId);

            this._webActivityService.Setup(x => x.LogOpenBankingComplete(lowellReference, _caseflowUserId))
                .Returns(Task.CompletedTask);

            ViewResult result = (ViewResult)await this._controller.BudgetComplete( customerReference, string.Empty);

            var model = (OpenWrksSuccessVm)result.Model;

            Assert.AreEqual("Success", result.ViewName);
            Assert.AreEqual(lowellSurrogateKey, model.LowellFinancialAccountSurrogateKey);
            Assert.AreEqual("", model.OccurrenceId);
        }

        [TestMethod]
        public async Task BudgetCompleteTest_ErrorCode()
        {
            string lowellReference = "123456789";
            Guid lowellSurrogateKey = Guid.NewGuid();
            string customerReference = _caseflowUserId;

            this._sessionState.Setup(x => x.GetTopLowellSurrogateKey()).Returns(lowellSurrogateKey);
            this._sessionState.Setup(x => x.GetTopLowellReference()).Returns(lowellReference);

            this._webActivityService.Setup(x => x.LogOpenBankingError(lowellReference, _caseflowUserId))
                .Returns(Task.CompletedTask);
            
            ViewResult result = (ViewResult)await this._controller.BudgetComplete(customerReference, "ERR");

            Assert.AreEqual("Error", result.ViewName);

            OpenWrksErrorVm errorVm = (OpenWrksErrorVm)result.Model;

            Assert.AreEqual("", errorVm.OccurrenceId);
            Assert.AreEqual(lowellSurrogateKey, errorVm.LowellFinancialAccountSurrogateKey);
        }

        [TestMethod]
        public void ErrorTest()
        {
            Guid lowellSurrogateKey = Guid.NewGuid();

            this._sessionState.Setup(ss => ss.GetTopLowellSurrogateKey()).Returns(lowellSurrogateKey);

            ViewResult result = (ViewResult)this._controller.Error("");

            Assert.AreEqual(null, result.ViewName);

            OpenWrksErrorVm errorVm = (OpenWrksErrorVm)result.Model;

            Assert.AreEqual("", errorVm.OccurrenceId);
            Assert.AreEqual(lowellSurrogateKey, errorVm.LowellFinancialAccountSurrogateKey);
        }


    }
}
