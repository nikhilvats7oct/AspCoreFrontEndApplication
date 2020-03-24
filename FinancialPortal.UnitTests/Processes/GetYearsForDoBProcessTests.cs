using System;
using System.Collections.Generic;
using FinancialPortal.Web.Processes;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class GetYearsForDoBProcessTests
    {
        private Mock<ILogger<GetYearsForDoBProcess>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<GetYearsForDoBProcess>>();
        }

        [TestMethod]
        public void Build_WhenCalledWithSingleYear_ReturnsNullAndSpecifiedYear()
        {
            GetYearsForDoBProcess process = new GetYearsForDoBProcess(_mockLogger.Object);

            List<KeyValuePair<int?, string>> list = process.Build(2001, 2001);

            Assert.AreEqual(2, list.Count);
            AssertKeyValuePair(null, "Year", list[0]);
            AssertKeyValuePair(2001, "2001", list[1]);
        }

        [TestMethod]
        public void Build_WhenCalledWithTwoYears_ReturnsNullAndSpecifiedYearsInReverseOrder()
        {
            GetYearsForDoBProcess process = new GetYearsForDoBProcess(_mockLogger.Object);

            List<KeyValuePair<int?, string>> list = process.Build(2001, 2002);

            Assert.AreEqual(3, list.Count);
            AssertKeyValuePair(null, "Year", list[0]);
            AssertKeyValuePair(2002, "2002", list[1]);
            AssertKeyValuePair(2001, "2001", list[2]);
        }

        [TestMethod]
        public void Build_WhenCalledWithFiveYearss_ReturnsNullAndSpecifiedYearsInReverseOrder()
        {
            GetYearsForDoBProcess process = new GetYearsForDoBProcess(_mockLogger.Object);

            List<KeyValuePair<int?, string>> list = process.Build(1999, 2003);

            Assert.AreEqual(6, list.Count);
            AssertKeyValuePair(null, "Year", list[0]);
            AssertKeyValuePair(2003, "2003", list[1]);
            AssertKeyValuePair(2002, "2002", list[2]);
            AssertKeyValuePair(2001, "2001", list[3]);
            AssertKeyValuePair(2000, "2000", list[4]);
            AssertKeyValuePair(1999, "1999", list[5]);
        }

        [TestMethod]
        public void Build_WhenCalledWithYearsInWrongOrder_ThrowsArgumentException()
        {
            GetYearsForDoBProcess process = new GetYearsForDoBProcess(_mockLogger.Object);

            try
            {
                List<KeyValuePair<int?, string>> list = process.Build(2000, 1999);

                Assert.Fail("Expected exception did not occur");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("minYear must be <= maxYear", ex.Message);
            }
        }

        void AssertKeyValuePair(int? expectedKey, string expectedValue, KeyValuePair<int?, string> pair)
        {
            Assert.AreEqual(expectedKey, pair.Key);
            Assert.AreEqual(expectedValue, pair.Value);
        }

    }
}
