using System.Collections.Generic;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class BuildFrequencyListProcessTests
    {
        private IBuildFrequencyListProcess _buildFrequencyListProcess;

        [TestInitialize]
        public void TestInitialise()
        {
            _buildFrequencyListProcess = new BuildFrequencyListProcess();
        }

        [TestMethod]
        public void BuildFrequencyList_ReplacesEvery4WeeksValue()
        {
            List<string> strings = new List<string>()
            {
                "*anything",
                "Every 4 weeks"
            };

            IList<DirectDebitPaymentFrequencyVm> list = _buildFrequencyListProcess.BuildFrequencyList(strings);

            Assert.AreEqual(2, list.Count);

            Assert.AreEqual("*anything", list[0].Value);
            Assert.AreEqual("*anything", list[0].DisplayedText);

            Assert.AreEqual("4week", list[1].Value);                    // changed to fit with JayWing's requirements
            Assert.AreEqual("Every 4 weeks", list[1].DisplayedText);
        }
    }
}
