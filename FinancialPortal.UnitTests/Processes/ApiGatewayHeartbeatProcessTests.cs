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
    public class ApiGatewayHeartbeatProcessTests
    {
        private Mock<ILogger<ApiGatewayHeartbeatProcess>> _loggerMock;
        private Mock<IApiGatewayProxy> _proxyMock;

        [TestMethod]
        public void CheckApiGatewayHeartbeat_CallsProxy()
        {
            _loggerMock = new Mock<ILogger<ApiGatewayHeartbeatProcess>>();
            _proxyMock = new Mock<IApiGatewayProxy>();

            HeartBeatDto dtoReturnedFromProxy = new HeartBeatDto();

            _proxyMock.Setup(x => x.CheckAllServices()).ReturnsAsync(dtoReturnedFromProxy);

            IApiGatewayHeartbeatProcess process = new ApiGatewayHeartbeatProcess(_loggerMock.Object, _proxyMock.Object);

            HeartBeatDto dtoReturnedFromProcess = process.CheckApiGatewayHeartbeat().Result;

            _proxyMock.Verify(x => x.CheckAllServices(), Times.Once);

            Assert.AreEqual(dtoReturnedFromProcess, dtoReturnedFromProxy);
        }
    }
}
