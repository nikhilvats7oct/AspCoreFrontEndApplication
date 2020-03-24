using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class SendAmendDirectDebitPlanProcessTests
    {
        private Mock<ILogger<SendAmendDirectDebitPlanProcess>> _loggerMock;
        private Mock<IApiGatewayProxy> _apiGatewayProxyMock;

        private ISendAmendDirectDebitPlanProcess _sendAmendDirectDebitPlanProcess;

        [TestInitialize]
        public void Initialise()
        {
            _loggerMock = new Mock<ILogger<SendAmendDirectDebitPlanProcess>>();
            _apiGatewayProxyMock = new Mock<IApiGatewayProxy>();

            _sendAmendDirectDebitPlanProcess = new SendAmendDirectDebitPlanProcess(_loggerMock.Object, _apiGatewayProxyMock.Object);
        }

        [TestMethod]
        public void SendAmendDirectDebitPlanAsync_CallsAmendPlanOnProxy()
        {
            DirectDebitPaymentDto directDebitPaymentDto = new DirectDebitPaymentDto();

            _apiGatewayProxyMock.Setup(x => x.AmendDirectDebitPlanAsync(directDebitPaymentDto))
                .Callback((DirectDebitPaymentDto dto) =>
                {
                    Assert.AreEqual(directDebitPaymentDto, dto);
                })
                .Returns(Task.CompletedTask);

            _sendAmendDirectDebitPlanProcess.SendAmendDirectDebitPlanAsync(directDebitPaymentDto).Wait();

            _apiGatewayProxyMock.Verify(x => x.AmendDirectDebitPlanAsync(directDebitPaymentDto), Times.Once);
        }

    }
}
