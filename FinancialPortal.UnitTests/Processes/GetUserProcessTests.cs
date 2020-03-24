using System;
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
    public class GetUserProcessTests
    {
        private Mock<ILogger<GetUserProcess>> _mockLogger;
        private Mock<IApiGatewayProxy> _mockProxy;
        private GetUserProcess _process;

        [TestInitialize]
        public void Initialise()
        {
            _mockLogger = new Mock<ILogger<GetUserProcess>>();
            _mockProxy = new Mock<IApiGatewayProxy>(MockBehavior.Strict);
            _process = new GetUserProcess(_mockLogger.Object, _mockProxy.Object);
        }

        [TestMethod]
        public void GetUser_Test_Returns_Result_of_Proxy()
        {
            UserDto user = new UserDto()
            {
                Company = 1,
                EmailAddress = "stewart.hartley@lowellgroup.co.uk",
                LowellReference = "123456789",
                IsSuccessful = true,
                MessageForUser = "hello"
            };

            string userId = Guid.NewGuid().ToString();

            _mockProxy.Setup(x => x.GetUser(userId)).Returns(Task.FromResult(user));

            UserDto result = _process.GetUser(userId).Result;

            Assert.AreEqual(user, result);

            //Check that the proxy was called once
            _mockProxy.Verify(x => x.GetUser(userId), Times.Once);
            _mockProxy.VerifyNoOtherCalls();
        }
    }
}
