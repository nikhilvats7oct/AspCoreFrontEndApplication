using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class SendPaymentProcessTests
    {
        private Mock<ILogger<SendPaymentProcess>> _loggerMock;
        private Mock<IApiGatewayProxy> _apiGatewayProxyMock;

        private SendPaymentProcess _sendPaymentProcess;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<SendPaymentProcess>>();
            _apiGatewayProxyMock = new Mock<IApiGatewayProxy>();
            _sendPaymentProcess = new SendPaymentProcess(_loggerMock.Object, _apiGatewayProxyMock.Object);
        }

        [TestMethod]
        public void BuildPaymentDto_MapsAsExpected()
        {
            PaymentDto paymentDto = new PaymentDto();

            _apiGatewayProxyMock.Setup(x => x.MakePayment(paymentDto))
                .Callback((PaymentDto dto) =>
                {
                    Assert.AreEqual(paymentDto, dto);
                })
                .Returns(Task.CompletedTask);

            _sendPaymentProcess.SendPayment(paymentDto).Wait();

            _apiGatewayProxyMock.Verify(x => x.MakePayment(paymentDto), Times.Once);
        }
    }
}