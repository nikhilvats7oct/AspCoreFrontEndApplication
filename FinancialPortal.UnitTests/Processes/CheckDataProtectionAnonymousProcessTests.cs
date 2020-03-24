using System.Threading.Tasks;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Proxy.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class CheckDataProtectionAnonymousProcessTests
    {
        [TestMethod]
        public void Check_WithDto_WhenProxyResultIsSuccessful_ReturnsTrue()
        {
            Mock<IApiGatewayProxy> proxyMock = new Mock<IApiGatewayProxy>();

            // Contents don't matter for this test - just passes along to proxy
            DataProtectionDto dtoInput = new DataProtectionDto();

            var resultDto = new ResultDto()
            {
                IsSuccessful = true,
                MessageForUser = null
            };
            proxyMock.Setup(x => x.CheckDataProtectionAnonymous(dtoInput)).Returns(Task.FromResult(resultDto));

            //
            // Test
            //
            CheckDataProtectionAnonymousProcess process = new CheckDataProtectionAnonymousProcess(proxyMock.Object);

            ResultDto result = process.CheckDataProtection(dtoInput).Result;

            Assert.AreEqual(true, result.IsSuccessful);
            Assert.AreEqual(null, result.MessageForUser);

            proxyMock.Verify(x => x.CheckDataProtectionAnonymous(dtoInput), Times.Once);
        }

        [TestMethod]
        public void Check_WithDto_WhenProxyResultIsSuccessful_ReturnsFalse()
        {
            Mock<IApiGatewayProxy> proxyMock = new Mock<IApiGatewayProxy>();

            // Contents don't matter for this test - just passes along to proxy
            DataProtectionDto dtoInput = new DataProtectionDto();

            var resultDto = new ResultDto()
            {
                IsSuccessful = false,
                MessageForUser = "wah"
            };
            proxyMock.Setup(x => x.CheckDataProtectionAnonymous(dtoInput)).Returns(Task.FromResult(resultDto));

            //
            // Test
            //
            CheckDataProtectionAnonymousProcess process = new CheckDataProtectionAnonymousProcess(proxyMock.Object);

            ResultDto result = process.CheckDataProtection(dtoInput).Result;

            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual("wah", result.MessageForUser);

            proxyMock.Verify(x => x.CheckDataProtectionAnonymous(dtoInput), Times.Once);
        }
    }
}
