using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class PaymentOptionsValidatorProcessTests
    {
        private IPaymentOptionsVmValidatorProcess _paymentOptionsValidatorProcess;

        [TestInitialize]
        public void Initialise()
        {
            _paymentOptionsValidatorProcess = new PaymentOptionsVmValidatorProcess();
        }
        [TestMethod]
        public void Validate_WhenModelIsValid_ReturnsTrue()
        {
            // Simplest possible valid scenario
            PaymentOptionsVm paymentOptionsVm = new PaymentOptionsVm()
            {
                SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.FullPayment,
                FullPaymentSelectedSourceOfFunds = "anything"
            };

            Assert.IsTrue(_paymentOptionsValidatorProcess.Validate(paymentOptionsVm));
        }

        [TestMethod]
        public void Validate_WhenModelIsInvalid_ReturnsFalse()
        {
            PaymentOptionsVm paymentOptionsVm = new PaymentOptionsVm()
            {
                SelectedPaymentOption = PaymentOptionsSelectionsVm.Values.PleaseSelect
            };

            Assert.IsFalse(_paymentOptionsValidatorProcess.Validate(paymentOptionsVm));
        }
    }
}
