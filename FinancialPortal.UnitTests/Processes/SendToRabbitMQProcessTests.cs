using System;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class SendToRabbitMQProcessTests
    {
        private Mock<IApiGatewayProxy> _apiGatewayProxy;
        private Mock<IRestClient> _restClient;
        private SendToRabbitMQProcess _sendToRabbitMQProcess;

        [TestInitialize]
        public void Initialise()
        {
            _restClient= new Mock<IRestClient>();
            _apiGatewayProxy = new Mock<IApiGatewayProxy>(MockBehavior.Strict);
            _sendToRabbitMQProcess = new SendToRabbitMQProcess(_apiGatewayProxy.Object, new PortalSetting(),  _restClient.Object);
        }

        [TestMethod]
        public void SendToRabbitMqTest()
        {
            WebActionDto webAction = new WebActionDto()
            {
                Company = 123,
                DateTime = DateTime.Now,
                Guid = Guid.NewGuid().ToString(),
                LowellReference = "123456789",
                WebActionType = WebActionType.AccountDetailsViewed
            };

            _apiGatewayProxy.Setup(x => x.SendToRabbitMQ(webAction)).Returns(Task.CompletedTask);

            _sendToRabbitMQProcess.SendToRabbitMQ(webAction).Wait();

            _apiGatewayProxy.Verify(x => x.SendToRabbitMQ(webAction), Times.Once);
        }

    }
}
