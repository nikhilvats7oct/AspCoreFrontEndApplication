using FinancialPortal.Web.Controllers;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinancialPortal.UnitTests.Controllers
{
    [TestClass]
    public class OneOffPaymentResultControllerTest
    {
        private Mock<IGtmService> _gtmService;
        private Mock<IPaymentService> _paymentService;
        private Mock<IVerifonePaymentProviderService> _verifonePaymentProviderService;
        private Mock<IWebActivityService> _webActivityService;
        private Mock<ILogger<BaseController>> _logger;
        private Mock<IConfiguration> _configuration;
        private Mock<IDistributedCache> _distributedCache;
        private Mock<IApplicationSessionState> _sessionState;
        private OneOffPaymentResultController _controller;
        private string _caseflowUserId;

        private void VerifyAll()
        {
            _logger.VerifyAll();
            _configuration.VerifyAll();
            _distributedCache.VerifyAll();
            _webActivityService.VerifyAll();
            _sessionState.VerifyAll();
        }

        [TestInitialize]
        public void Initialize()
        {
            _gtmService = new Mock<IGtmService>();
            _paymentService = new Mock<IPaymentService>();
            _verifonePaymentProviderService = new Mock<IVerifonePaymentProviderService>();
            _webActivityService = new Mock<IWebActivityService>();
            _logger = new Mock<ILogger<BaseController>>();
            _configuration = new Mock<IConfiguration>();
            _distributedCache = new Mock<IDistributedCache>();
            _sessionState = new Mock<IApplicationSessionState>();
            _caseflowUserId = Guid.NewGuid().ToString();

            _controller = new OneOffPaymentResultController(_logger.Object, _configuration.Object, _paymentService.Object, _verifonePaymentProviderService.Object,
                _gtmService.Object, _distributedCache.Object, _sessionState.Object, _webActivityService.Object);

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
        public async Task Index_Process_Verifone_Response_SUCCESS()
        {
            //Arrange
            string transactionGuid = "123456789_3d86fa56-3b9c-4225-98fa-f4926f2683bd";
            string result = "SUCCESS";
            string tokenId = "10032863201";
            string authrorisationCode = "789DE";

            //Act
            var _verifoneTransactionDto = new VerifoneTransactionDto()
            {
                TransactionData = "{'LowellReference':'257113803','ClientName':'Lamb','PaymentAmount':501.53,'SourceOfFunds':'Disposable Income','SourceOfFundsOther':null,'UserID':'2e5321ad - af0c - 4a69 - 8036 - 5ecf703017cb','PaidInFull':true,'DiscountAvailable':false,'DiscountSelected':false,'PlanInPlace':false,'InArrears':false}",
                Status = 0
            };

            _verifonePaymentProviderService.Setup(x => x.GetVerifoneTransactionAsync(transactionGuid)).Returns(Task.FromResult(_verifoneTransactionDto));

            var oneOffPaymentDto =
                   JsonConvert.DeserializeObject<OneOffPaymentDto>(_verifoneTransactionDto.TransactionData);

            var model = new PaymentResultVm
            {
                Reference = transactionGuid,
                Result = result,
                TokenId = tokenId,
                ACode = authrorisationCode,
                PaymentInfo = oneOffPaymentDto
            };

            var successfulOneOffPaymentVm = new SuccessfulOneOffPaymentVm
            {
                ClientName = oneOffPaymentDto.ClientName,
                PaymentInfo = model.PaymentInfo,
                UserLoggedIn = !string.IsNullOrEmpty(_caseflowUserId)

            };

            _paymentService.Setup(x => x.MakePayment(model, oneOffPaymentDto)).Verifiable();
            _verifonePaymentProviderService.Setup(x => x.UpdateVerifoneTransactionAsync(_verifoneTransactionDto)).Verifiable();

            _sessionState.Setup(x => x.LogPaymentResult).Returns(true);
            _gtmService.Setup(x => x.RaiseOneOffPaymentEvent_PaymentComplete(successfulOneOffPaymentVm, _caseflowUserId, "Regular Account")).Verifiable();
            _webActivityService.Setup(x => x.LogOneOffPaymentComplete(model.PaymentInfo.LowellReference, _caseflowUserId, !model.PaymentInfo.PaidInFull, model.PaymentInfo.DiscountSelected)).Returns(Task.CompletedTask);

            ViewResult response = (ViewResult)await _controller.Index(transactionGuid, result, tokenId, authrorisationCode);          

            SuccessfulOneOffPaymentVm res = (SuccessfulOneOffPaymentVm)response.Model;

            //Assert            
            Assert.AreEqual(successfulOneOffPaymentVm.ClientName, res.ClientName);
            Assert.AreEqual(model.PaymentInfo.LowellReference, res.PaymentInfo.LowellReference);
            VerifyAll();
        }

        [TestMethod]
        public async Task Index_Process_Verifone_Response_CANCELLED()
        {
            //Arrange
            string transactionGuid = "123456789_3d86fa56-3b9c-4225-98fa-f4926f2683bd";
            string result = "CANCELLED";
            string tokenId = "10032863201";
            string authrorisationCode = "789DE";

            //Act
            var _verifoneTransactionDto = new VerifoneTransactionDto()
            {
                TransactionGuid = "123456789_3d86fa56-3b9c-4225-98fa-f4926f2683bd",
                TransactionData = "{'LowellReference':'257113803','ClientName':'Lamb','PaymentAmount':501.53,'SourceOfFunds':'Disposable Income','SourceOfFundsOther':null,'UserID':'2e5321ad - af0c - 4a69 - 8036 - 5ecf703017cb','PaidInFull':true,'DiscountAvailable':false,'DiscountSelected':false,'PlanInPlace':false,'InArrears':false}",
                Status = 0
            };

            _verifonePaymentProviderService.Setup(x => x.GetVerifoneTransactionAsync(transactionGuid)).Returns(Task.FromResult(_verifoneTransactionDto));

            var oneOffPaymentDto =
                   JsonConvert.DeserializeObject<OneOffPaymentDto>(_verifoneTransactionDto.TransactionData);

            var model = new PaymentResultVm
            {
                Reference = transactionGuid,
                Result = result,
                TokenId = tokenId,
                ACode = authrorisationCode,
                PaymentInfo = oneOffPaymentDto
            };
            
            _verifonePaymentProviderService.Setup(x => x.UpdateVerifoneTransactionAsync(_verifoneTransactionDto)).Verifiable();

            _sessionState.Setup(x => x.LogPaymentResult).Returns(true);
            _gtmService.Setup(x => x.RaiseOneOffPaymentEvent_PaymentCancelled(model, _caseflowUserId, "Regular Account")).Verifiable();
            _webActivityService.Setup(x => x.LogOneOffPaymentCancelled(model.PaymentInfo.LowellReference, _caseflowUserId, !model.PaymentInfo.PaidInFull, model.PaymentInfo.DiscountSelected)).Returns(Task.CompletedTask);

            ViewResult response = (ViewResult)await _controller.Index(transactionGuid, result, tokenId, authrorisationCode);
            PaymentResultVm res = (PaymentResultVm)response.Model;

            //Assert            
            Assert.AreEqual(model.Result, res.Result);
            Assert.AreEqual(model.PaymentInfo.LowellReference, res.PaymentInfo.LowellReference);
            VerifyAll();
        }

        [TestMethod]
        public async Task Index_Process_Verifone_Response_FAILED()
        {
            //Arrange
            string transactionGuid = "123456789_3d86fa56-3b9c-4225-98fa-f4926f2683bd";
            string result = "FAILED";
            string tokenId = "10032863201";
            string authrorisationCode = "789DE";

            //Act
            var _verifoneTransactionDto = new VerifoneTransactionDto()
            {
                TransactionGuid = "123456789_3d86fa56-3b9c-4225-98fa-f4926f2683bd",
                TransactionData = "{'LowellReference':'257113803','ClientName':'Lamb','PaymentAmount':501.53,'SourceOfFunds':'Disposable Income','SourceOfFundsOther':null,'UserID':'2e5321ad - af0c - 4a69 - 8036 - 5ecf703017cb','PaidInFull':true,'DiscountAvailable':false,'DiscountSelected':false,'PlanInPlace':false,'InArrears':false}",
                Status = 0
            };

            _verifonePaymentProviderService.Setup(x => x.GetVerifoneTransactionAsync(transactionGuid)).Returns(Task.FromResult(_verifoneTransactionDto));

            var oneOffPaymentDto =
                   JsonConvert.DeserializeObject<OneOffPaymentDto>(_verifoneTransactionDto.TransactionData);

            var model = new PaymentResultVm
            {
                Reference = transactionGuid,
                Result = result,
                TokenId = tokenId,
                ACode = authrorisationCode,
                PaymentInfo = oneOffPaymentDto
            };
            
            _verifonePaymentProviderService.Setup(x => x.UpdateVerifoneTransactionAsync(_verifoneTransactionDto)).Verifiable();

            _sessionState.Setup(x => x.LogPaymentResult).Returns(true);
            _gtmService.Setup(x => x.RaiseOneOffPaymentEvent_PaymentFailed(model, _caseflowUserId, "Regular Account")).Verifiable();
            _webActivityService.Setup(x => x.LogOneOffPaymentFailure(model.PaymentInfo.LowellReference, _caseflowUserId, !model.PaymentInfo.PaidInFull, model.PaymentInfo.DiscountSelected)).Returns(Task.CompletedTask);

            ViewResult response = (ViewResult)await _controller.Index(transactionGuid, result, tokenId, authrorisationCode);
            PaymentResultVm res = (PaymentResultVm)response.Model;

            //Assert            
            Assert.AreEqual(model.Result, res.Result);
            Assert.AreEqual(model.PaymentInfo.LowellReference, res.PaymentInfo.LowellReference);
            VerifyAll();
        }

        [TestMethod]
        public async Task Index_Process_Verifone_Response_transaction_not_exists_or_already_processed()
        {
            //Arrange
            string transactionGuid = "123456789_3d86fa56-3b9c-4225-98fa-f4926f2683bd";
            string result = "FAILED";
            string tokenId = "10032863201";
            string authrorisationCode = "789DE";

            //Act
            var _verifoneTransactionDto = new VerifoneTransactionDto()
            {
                TransactionGuid = "123456789_3d86fa56-3b9c-4225-98fa-f4926f2683bd",
                TransactionData = "{'LowellReference':'257113803','ClientName':'Lamb','PaymentAmount':501.53,'SourceOfFunds':'Disposable Income','SourceOfFundsOther':null,'UserID':'2e5321ad - af0c - 4a69 - 8036 - 5ecf703017cb','PaidInFull':true,'DiscountAvailable':false,'DiscountSelected':false,'PlanInPlace':false,'InArrears':false}",
                Status = 1
            };

            _verifonePaymentProviderService.Setup(x => x.GetVerifoneTransactionAsync(transactionGuid)).Returns(Task.FromResult(_verifoneTransactionDto));

            var oneOffPaymentDto =
                   JsonConvert.DeserializeObject<OneOffPaymentDto>(_verifoneTransactionDto.TransactionData);

            var model = new PaymentResultVm
            {
                Reference = transactionGuid,
                Result = result,
                TokenId = tokenId,
                ACode = authrorisationCode,
                PaymentInfo = oneOffPaymentDto
            };

            _sessionState.Setup(x => x.LogPaymentResult).Returns(true);
            _gtmService.Setup(x => x.RaiseOneOffPaymentEvent_PaymentFailed(model, _caseflowUserId, "Regular Account")).Verifiable();
            _webActivityService.Setup(x => x.LogOneOffPaymentFailure(model.PaymentInfo.LowellReference, _caseflowUserId, !model.PaymentInfo.PaidInFull, model.PaymentInfo.DiscountSelected)).Returns(Task.CompletedTask);

            ViewResult response = (ViewResult)await _controller.Index(transactionGuid, result, tokenId, authrorisationCode);
            
            _verifonePaymentProviderService.Setup(x => x.UpdateVerifoneTransactionAsync(_verifoneTransactionDto)).Verifiable();

            PaymentResultVm res = (PaymentResultVm)response.Model;

            //Assert            
            Assert.AreEqual(model.Result, res.Result);
            Assert.AreEqual(model.PaymentInfo.LowellReference, res.PaymentInfo.LowellReference);
            VerifyAll();
        }

    }
}
