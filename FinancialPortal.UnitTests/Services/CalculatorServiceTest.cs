using FinancialPortal.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.Services
{
    [TestClass]
    public class CalculatorServiceTest
    {
        private CalculatorService _service;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._service = new CalculatorService();
        }

        [TestMethod]
        public void ConvertToMonthlyTest()
        {
            Assert.AreEqual(0, _service.ConvertToMonthly(1, null));
            Assert.AreEqual(0, _service.ConvertToMonthly(2, ""));
            Assert.AreEqual(0, _service.ConvertToMonthly(3, "testing..."));

            Assert.AreEqual(100, _service.ConvertToMonthly(100, "M"));
            Assert.AreEqual(110.42M, _service.ConvertToMonthly(25.50M, "W"));
            Assert.AreEqual(65.75M, _service.ConvertToMonthly(30.30M, "F"));
            Assert.AreEqual(48.00M, _service.ConvertToMonthly(44.44M, "4"));
            Assert.AreEqual(18.52M, _service.ConvertToMonthly(55.55M, "Q"));
            Assert.AreEqual(45.67M, _service.ConvertToMonthly(45.67M, "L"));
            Assert.AreEqual(41.67M, _service.ConvertToMonthly(499.99M, "A"));

            Assert.AreEqual(100, _service.ConvertToMonthly(100, "m"));
            Assert.AreEqual(110.42M, _service.ConvertToMonthly(25.50M, "w"));
            Assert.AreEqual(65.75M, _service.ConvertToMonthly(30.30M, "f"));
            Assert.AreEqual(48.00M, _service.ConvertToMonthly(44.44M, "4"));
            Assert.AreEqual(18.52M, _service.ConvertToMonthly(55.55M, "q"));
            Assert.AreEqual(45.67M, _service.ConvertToMonthly(45.67M, "l"));
            Assert.AreEqual(41.67M, _service.ConvertToMonthly(499.99M, "a"));

            Assert.AreEqual(100, _service.ConvertToMonthly(100, "monthly"));
            Assert.AreEqual(110.42M, _service.ConvertToMonthly(25.50M, "weekly"));
            Assert.AreEqual(65.75M, _service.ConvertToMonthly(30.30M, "fortnightly"));
            Assert.AreEqual(48.00M, _service.ConvertToMonthly(44.44M, "4week"));
            Assert.AreEqual(18.52M, _service.ConvertToMonthly(55.55M, "quarterly"));
            Assert.AreEqual(41.67M, _service.ConvertToMonthly(499.99M, "annually"));

            Assert.AreEqual(100, _service.ConvertToMonthly(100, "Monthly"));
            Assert.AreEqual(110.42M, _service.ConvertToMonthly(25.50M, "Weekly"));
            Assert.AreEqual(65.75M, _service.ConvertToMonthly(30.30M, "Fortnightly"));
            Assert.AreEqual(48.00M, _service.ConvertToMonthly(44.44M, "4Week"));
            Assert.AreEqual(18.52M, _service.ConvertToMonthly(55.55M, "Quarterly"));
            Assert.AreEqual(41.67M, _service.ConvertToMonthly(499.99M, "Annually"));
        }

    }
}
