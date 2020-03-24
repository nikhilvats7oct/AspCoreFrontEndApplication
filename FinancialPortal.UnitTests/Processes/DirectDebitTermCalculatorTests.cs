using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class DirectDebitTermCalculatorTests
    {
        private IDirectDebitTermCalculator _directDebitTermCalculator;

        [TestInitialize]
        public void Initialise()
        {
            _directDebitTermCalculator = new DirectDebitTermCalculator();
        }

        [TestMethod]
        public void CalculateTermInMonthsTest_Monthly()
        {
            Assert.AreEqual(10, _directDebitTermCalculator.CalculateTermInMonths(100, 10, "Monthly"));
        }

        [TestMethod]
        public void CalculateTermInMonthsTest_Weekly()
        {
            Assert.AreEqual(3, _directDebitTermCalculator.CalculateTermInMonths(100, 10, "Weekly"));
        }

        [TestMethod]
        public void CalculateTermInMonthsTest_Fortnightly()
        {
            Assert.AreEqual(5, _directDebitTermCalculator.CalculateTermInMonths(100, 10, "Fortnightly"));
        }

        [TestMethod]
        public void CalculateTermInMonthsTest_4Weekly()
        {
            Assert.AreEqual(10, _directDebitTermCalculator.CalculateTermInMonths(100, 10, "4week"));
        }
    }
}
