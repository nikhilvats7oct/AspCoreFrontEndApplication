using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class AmendDirectDebitVmValidatorProcessTests
    {
        private IAmendDirectDebitVmValidatorProcess _amendDirectDebitVmValidatorProcess;

        [TestInitialize]
        public void Initialise()
        {
            _amendDirectDebitVmValidatorProcess = new AmendDirectDebitVmValidatorProcess();
        }

        [TestMethod]
        public void Validate_WhenModelIsValid_ReturnsTrue()
        {
            AmendDirectDebitVm amendDirectDebitVm = new AmendDirectDebitVm()
            {
                OutstandingBalance = 20.0m,
                DirectDebitAmount = 10.0m,
                PlanFrequency = "*freq",
                EarliestStartDate = new DateTime(2018, 10, 4),
                LatestStartDate = new DateTime(2018, 10, 6),
                PlanStartDate = new DateTime(2018, 10, 5),
                SelectedPlanSetupOption = PlanSetupOptions.AverageSetupValue
            };

            Assert.IsTrue(_amendDirectDebitVmValidatorProcess.Validate(amendDirectDebitVm));
        }

        [TestMethod]
        public void Validate_WhenModelIsInvalid_ReturnsFalse()
        {
            AmendDirectDebitVm amendDirectDebitVm = new AmendDirectDebitVm();

            Assert.IsFalse(_amendDirectDebitVmValidatorProcess.Validate(amendDirectDebitVm));
        }

    }
}
