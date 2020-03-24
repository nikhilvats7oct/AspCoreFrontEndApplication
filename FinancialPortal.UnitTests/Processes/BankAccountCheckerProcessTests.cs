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
    public class BankAccountCheckerProcessTests
    {
        private Mock<ILogger<BankAccountCheckerProcess>> _loggerMock;
        private Mock<IApiGatewayProxy> _proxyMock;

        [TestMethod]
        public void CheckApiGatewayHeartbeat_CallsProxy()
        {
            _loggerMock = new Mock<ILogger<BankAccountCheckerProcess>>();
            _proxyMock = new Mock<IApiGatewayProxy>();

            BankAccountCheckerDto dtoPassedToProxy = new BankAccountCheckerDto();
            GetBankAccountValidationResultDto dtoReturnedFromProxy = new GetBankAccountValidationResultDto();

            _proxyMock.Setup(x => x.CheckBankAccount(dtoPassedToProxy)).ReturnsAsync(dtoReturnedFromProxy);

            IBankAccountCheckerProcess process = new BankAccountCheckerProcess(_loggerMock.Object, _proxyMock.Object);

            GetBankAccountValidationResultDto dtoReturnedFromProcess = process.CheckBankAccount(dtoPassedToProxy).Result;

            _proxyMock.Verify(x => x.CheckBankAccount(It.IsAny<BankAccountCheckerDto>()), Times.Once);

            Assert.AreEqual(dtoReturnedFromProcess, dtoReturnedFromProxy);
        }
    }
}
