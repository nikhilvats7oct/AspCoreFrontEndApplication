using FinancialPortal.Web.Models.Verifone;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class CreateVerifonePostProcessTests
    {
        private Mock<ILogger<CreateVerifonePostProcess>> _loggerMock;

        private ICreateVerifonePostProcess _createVerifonePostProcess;

        [TestInitialize]
        public void Initialise()
        {
            _loggerMock = new Mock<ILogger<CreateVerifonePostProcess>>();

            var verifoneSetting = new VerifoneSetting
            {
                 Merchant = new MerchantSetting {  }
            };

            _createVerifonePostProcess = new CreateVerifonePostProcess(_loggerMock.Object, verifoneSetting);
        }

        [TestMethod]
        public void CreateDataModel_InitialisesModelFromConfiguration()
        {
            string requestData = "anyoldShizzle";

            PostDataModel postDataModel = _createVerifonePostProcess.CreateDataModel(requestData);

            Assert.AreEqual(null, postDataModel.api);               // ConfigurationManager.AppSettings["Verifone:PostData:Api"];
            Assert.AreEqual(null, postDataModel.merchantid);        // ConfigurationManager.AppSettings["Verifone:Merchant:MerchantId"]
            Assert.AreEqual(null, postDataModel.systemid);          // ConfigurationManager.AppSettings["Verifone:Merchant:SystemId"]
            Assert.AreEqual("eftrequest", postDataModel.requesttype);
            Assert.AreEqual("anyoldShizzle", postDataModel.requestdata);

            Assert.Inconclusive("Unable to unit test completely. Should be taking IApplicationSettingsReader as a method parameter, so as to be able to mock config.");
        }
    }
}
