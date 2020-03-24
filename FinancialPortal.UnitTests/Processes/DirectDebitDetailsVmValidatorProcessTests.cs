using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class DirectDebitDetailsVmValidatorProcessTests
    {
        private IDirectDebitDetailsVmValidatorProcess _directDebitDetailsVmValidatorProcess;

        [TestInitialize]
        public void Initialise()
        {
            _directDebitDetailsVmValidatorProcess = new DirectDebitDetailsVmValidatorProcess();
        }

        [TestMethod]
        public void Validate_WhenModelIsValid_ReturnsTrue()
        {
            DirectDebitDetailsVm directDebitDetailsVm = new DirectDebitDetailsVm()
            {
                AccountHoldersName = "abc",
                AccountNumber = "12345678",
                SortCode = "123456",
                AcceptDirectDebitGuarantee = true
            };

            Assert.IsTrue(_directDebitDetailsVmValidatorProcess.Validate(directDebitDetailsVm));
        }

        [TestMethod]
        public void Validate_WhenModelIsInvalid_ReturnsFalse()
        {
            DirectDebitDetailsVm directDebitDetailsVm = new DirectDebitDetailsVm()
            {
            };

            Assert.IsFalse(_directDebitDetailsVmValidatorProcess.Validate(directDebitDetailsVm));
        }
    }
}
