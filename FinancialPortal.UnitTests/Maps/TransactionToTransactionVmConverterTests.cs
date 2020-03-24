using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class TransactionToTransactionVmConverterTests
    {
        private TransactionToTransactionVmConverter _converter;

        [TestInitialize]
        public void TestInitialise()
        {
            this._converter = new TransactionToTransactionVmConverter();
        }

        [TestMethod]
        public void ConvertTest_SourceNull()
        {
            Transaction source = null;
            TransactionVm destination = new TransactionVm();
            TransactionVm expected = null;

            TransactionVm result = _converter.Convert(source, destination, null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTest()
        {
            Transaction source = new Transaction()
            {
                Amount = 123.45M,
                Date = DateTime.Now,
                Description = "Testing...",
                RollingBalance = 543.21M,
                Type = "Credit"
            };

            TransactionVm destination = new TransactionVm();

            TransactionVm expected = new TransactionVm()
            {
                AmountText = "£123.45",
                DateText = DateTime.Now.ToString("dd MMM y", CultureInfo.CurrentCulture),
                Description = "Testing...",
                RollingBalanceText = "£543.21"
            }; 

            TransactionVm result = _converter.Convert(source, destination, null);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            Transaction source = new Transaction()
            {
                Amount = 123.45M,
                Date = DateTime.Now,
                Description = "Testing...",
                RollingBalance = 543.21M,
                Type = "Credit"
            };

            TransactionVm destination = null;

            TransactionVm expected = new TransactionVm()
            {
                AmountText = "£123.45",
                DateText = DateTime.Now.ToString("dd MMM y", CultureInfo.CurrentCulture),
                Description = "Testing...",
                RollingBalanceText = "£543.21"
            };

            TransactionVm result = _converter.Convert(source, destination, null);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }


    }
}
