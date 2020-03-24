using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class BuildDirectDebitPlanDtoProcessTests
    {
        IBuildDirectDebitPlanDtoProcess _buildDirectDebitPlanDtoProcess;
        Mock<ILogger<BuildDirectDebitPlanDtoProcess>> _loggerMock;

        [TestInitialize]
        public void Initialise()
        {
            _loggerMock = new Mock<ILogger<BuildDirectDebitPlanDtoProcess>>();

            _buildDirectDebitPlanDtoProcess = new BuildDirectDebitPlanDtoProcess(_loggerMock.Object);
        }

        [TestMethod]

        // Flag combinations - to ensure properly mapped
        [DataRow(false, false)]
        [DataRow(false, true)]
        [DataRow(true, false)]
        [DataRow(true, true)]

        public void BuildDirectDebitPlanDto_PopulatesDtoFromViewModel(
            bool testGuaranteeRead, bool testDiscountAccepted)
        {
            DirectDebitPlanOverviewVm directDebitPlanOverviewVm = new DirectDebitPlanOverviewVm()
            {
                LowellReference = "*LowellRef",
                PaymentAmount = 123.01m,
                StartDate = "*StartDate",
                PaymentType = "*PaymentType",
                AccountHoldersName = "Bobbert",
                AccountNumber = "12345678",
                SortCode = "123456",
                PlanTotal = 1123.45m,
                Frequency = "*freq",
                GuaranteeRead = testGuaranteeRead,
                DiscountAccepted = testDiscountAccepted,
                DiscountAmount = 17.50m,
                EmailAddress = "a@b.com"
            };

            DirectDebitPaymentDto directDebitPaymentDto = _buildDirectDebitPlanDtoProcess.BuildDirectDebitPlanDto(directDebitPlanOverviewVm);

            Assert.AreEqual("*LowellRef", directDebitPaymentDto.LowellReference);
            Assert.AreEqual(123.01m, directDebitPaymentDto.PaymentAmount);
            Assert.AreEqual("*StartDate", directDebitPaymentDto.StartDate);
            Assert.AreEqual("*PaymentType", directDebitPaymentDto.PaymentType);
            Assert.AreEqual("Bobbert", directDebitPaymentDto.AccountHoldersName);
            Assert.AreEqual("12345678", directDebitPaymentDto.AccountNumber);
            Assert.AreEqual("123456", directDebitPaymentDto.SortCode);
            Assert.AreEqual(1123.45m, directDebitPaymentDto.PlanTotal);
            Assert.AreEqual("*freq", directDebitPaymentDto.Frequency);
            Assert.AreEqual(testGuaranteeRead, directDebitPaymentDto.GuaranteeRead);
            Assert.AreEqual(testDiscountAccepted, directDebitPaymentDto.DiscountAccepted);
            Assert.AreEqual(17.50m, directDebitPaymentDto.DiscountAmount);
            Assert.AreEqual("a@b.com", directDebitPaymentDto.EmailAddress);
        }
    }
}
