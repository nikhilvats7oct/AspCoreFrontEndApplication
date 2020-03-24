using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
        public class SendDirectDebitPlanProcessTests
        {
            private Mock<ILogger<SendDirectDebitPlanProcess>> _logger;
            private Mock<IApiGatewayProxy> _apiGatewayProxy;

            private SendDirectDebitPlanProcess _directDebitPlanProcess;

            [TestInitialize]
            public void Setup()
            {
                _logger = new Mock<ILogger<SendDirectDebitPlanProcess>>();
                _apiGatewayProxy = new Mock<IApiGatewayProxy>();
                _directDebitPlanProcess = new SendDirectDebitPlanProcess(_logger.Object, _apiGatewayProxy.Object);
            }

            [TestMethod]
            public void BuildPaymentDto_MapsAsExpected()
            {


                //Arrange 
                var paymentDto = new DirectDebitPaymentDto();
                

                //Act 
                var result = _directDebitPlanProcess.SendDirectDebitPlanAsync(paymentDto);

                //Assert
                _apiGatewayProxy.Verify(x => x.SetupDirectDebitPlanAsync(paymentDto), Times.Once);


            }
        }
    }