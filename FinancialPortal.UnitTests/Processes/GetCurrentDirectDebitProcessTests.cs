using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class GetCurrentDirectDebitProcessTests
    {
        private Mock<IApiGatewayProxy> _apiGatewayProxy;
        private IGetCurrentDirectDebitProcess _getCurrentDirectDebitProcess;

        [TestInitialize]
        public void Initialise()
        {
            _apiGatewayProxy = new Mock<IApiGatewayProxy>(MockBehavior.Strict);
            _getCurrentDirectDebitProcess = new GetCurrentDirectDebitProcess(_apiGatewayProxy.Object);
        }

        [TestMethod]
        public void GetCurrentDirectDebit_RetrievesDtoViaProxy()
        {
            AccountReferenceDto accountReferenceDto = new AccountReferenceDto();

            AmendDirectDebitPaymentDto amendDirectDebitPaymentDto = new AmendDirectDebitPaymentDto();

            _apiGatewayProxy.Setup(x => x.GetCurrentDirectDebit(accountReferenceDto)).ReturnsAsync(amendDirectDebitPaymentDto);

            Assert.AreEqual(amendDirectDebitPaymentDto, _getCurrentDirectDebitProcess.GetCurrentDirectDebit(accountReferenceDto).Result);
        }

    }
}
