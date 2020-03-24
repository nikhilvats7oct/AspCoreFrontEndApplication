using System.Collections.Generic;
using FinancialPortal.Web.Processes;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class GetDaysForDoBProcessTests
    {
        private Mock<ILogger<GetDaysForDoBProcess>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<GetDaysForDoBProcess>>();
        }

        [TestMethod]
        public void Build_WhenCall_ReturnsNullAndAllDaysOfMonth()
        {
            GetDaysForDoBProcess process = new GetDaysForDoBProcess(_mockLogger.Object);

            List<KeyValuePair<int?, string>> list = process.Build();
            Assert.AreEqual(32, list.Count);

            AssertKeyValuePair(null, "Day", list[0]);

            // Test first few and last few
            AssertKeyValuePair(null, "Day", list[0]);
            AssertKeyValuePair(1, "1", list[1]);
            AssertKeyValuePair(2, "2", list[2]);
            AssertKeyValuePair(3, "3", list[3]);

            AssertKeyValuePair(29, "29", list[29]);
            AssertKeyValuePair(30, "30", list[30]);
            AssertKeyValuePair(31, "31", list[31]);

            // test all numbers
            for (int i = 1; i <= 31; i++)
                AssertKeyValuePair(i, i.ToString(), list[i]);
        }

        void AssertKeyValuePair(int? expectedKey, string expectedValue, KeyValuePair<int?, string> pair)
        {
            Assert.AreEqual(expectedKey, pair.Key);
            Assert.AreEqual(expectedValue, pair.Value);
        }

    }
}
