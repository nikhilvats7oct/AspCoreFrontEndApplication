using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{

    [TestClass]
    public class BuildPaymentDtoTests
    {
        private Mock<ILogger<BuildPaymentDtoProcess>> _logger;
        private BuildPaymentDtoProcess _buildPaymentDtoProcess;

        [TestInitialize]
        public void Setup()
        {
            _logger = new Mock<ILogger<BuildPaymentDtoProcess>>();
            _buildPaymentDtoProcess = new BuildPaymentDtoProcess(_logger.Object);
        }

        [TestMethod]

        // Settlement Amount - both flags must be true
        [DataRow(false, false, false, false, false)]
        [DataRow(false, true, false, false, false)]
        [DataRow(false, false, true, false, false)]
        [DataRow(false, true, true, false, true)]

        // Plan In Place flag
        [DataRow(false, false, false, false, false)]
        [DataRow(true, false, false, true, false)]

        public void BuildPaymentDto_MapsAsExpected(
            bool testPlanInPlace, bool testDiscountSelected, bool testPaidInFull,
            bool expectedPlanInPlace, bool expectedSettlementAmount)
        {
            //Arrange 
            var paymentResultVm = new PaymentResultVm()
            {
                ACode = "blah",
                PaymentInfo = new OneOffPaymentDto()
                {
                    PlanInPlace = testPlanInPlace,
                    DiscountSelected = testDiscountSelected,
                    PaidInFull = testPaidInFull,
                    UserID  = "Testing..."
                }
            };
            var oneOffPaymentDto = new OneOffPaymentDto()
            {
                LowellReference = "This",
                PaymentAmount = 12,
                SourceOfFunds = "A",
                SourceOfFundsOther = "Test"
            };

            //Act 
            var paymentDto = _buildPaymentDtoProcess.BuildPaymentDto(paymentResultVm, oneOffPaymentDto);

            //Assert
            Assert.AreEqual(expectedPlanInPlace, paymentDto.PlanInPlace);
            Assert.AreEqual(expectedSettlementAmount, paymentDto.SettlementAmount);
            Assert.AreEqual("This", paymentDto.LowellReference);
            Assert.AreEqual("blah", paymentDto.AuthCode);
            Assert.AreEqual(12, paymentDto.Amount);
            Assert.AreEqual("A:Test", paymentDto.SourceOfFunds);
            Assert.AreEqual("", paymentDto.CardId);
            Assert.AreEqual(null, paymentDto.ReplayId);
            Assert.AreEqual("WebUser", paymentDto.User);
        }
    }
}
