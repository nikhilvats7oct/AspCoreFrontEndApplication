using System.Collections.Generic;
using FinancialPortal.Web.Processes;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class GetMonthsForDoBProcessTests
    {
        private Mock<ILogger<GetMonthsForDoBProcess>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<GetMonthsForDoBProcess>>();
        }

        [TestMethod]
        public void Build_WhenCalled_ReturnsNullAndAllMonths()
        {
            GetMonthsForDoBProcess process = new GetMonthsForDoBProcess(_mockLogger.Object);

            List<KeyValuePair<string, string>> list = process.Build();
            Assert.AreEqual(13, list.Count);

            AssertKeyValuePair(null, "Month", list[0]);
            AssertKeyValuePair("January", list[1]);
            AssertKeyValuePair("February", list[2]);
            AssertKeyValuePair("March", list[3]);
            AssertKeyValuePair("April", list[4]);
            AssertKeyValuePair("May", list[5]);
            AssertKeyValuePair("June", list[6]);
            AssertKeyValuePair("July", list[7]);
            AssertKeyValuePair("August", list[8]);
            AssertKeyValuePair("September", list[9]);
            AssertKeyValuePair("October", list[10]);
            AssertKeyValuePair("November", list[11]);
            AssertKeyValuePair("December", list[12]);
        }

        void AssertKeyValuePair(string expectedKeyAndValue, KeyValuePair<string, string> keyValuePair)
        {
            AssertKeyValuePair(expectedKeyAndValue, expectedKeyAndValue, keyValuePair);
        }

        void AssertKeyValuePair(string expectedKey, string expectedValue, KeyValuePair<string, string> keyValuePair)
        {
            Assert.AreEqual(expectedKey, keyValuePair.Key);
            Assert.AreEqual(expectedValue, keyValuePair.Value);
        }
    }
}
