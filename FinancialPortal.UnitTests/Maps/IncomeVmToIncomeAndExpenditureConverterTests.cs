using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using FinancialPortal.Web.ViewModels.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class IncomeVmToIncomeAndExpenditureConverterTests
    {
        private IncomeVmToIncomeAndExpenditureConverter _converter;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._converter = new IncomeVmToIncomeAndExpenditureConverter();
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            IncomeVm source = null;
            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();
            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);

            expected.BenefitsTotal = 0;
            expected.BenefitsTotalFrequency = null;
            expected.Salary = 0;
            expected.SalaryFrequency = null;
            expected.OtherIncome = 0;
            expected.OtherincomeFrequency = null;
            expected.Pension = 0;
            expected.PensionFrequency = null;

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest()
        {
            IncomeVm source = new IncomeVm() 
            {
                BenefitsAndTaxCredits = new IncomeSourceVm() { Amount = 100, Frequency = "weekly" },
                Earning = new IncomeSourceVm() { Amount = 3000, Frequency = "monthly" },
                Pension = new IncomeSourceVm() { Amount = 125, Frequency = "fortnightly" },
                Other = new IncomeSourceVm() { Amount = 50, Frequency = "weekly" },
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "123456789" } },
                EnabledPartialSave = true,
                HasErrorPartialSavedIAndE = true,
                PartialSavedEvent = true,
                PartialSavedIAndE = true
            };

            //Create a copy of source for later
            IncomeVm sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();
            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);

            expected.BenefitsTotal = 100;
            expected.BenefitsTotalFrequency = "weekly";
            expected.Salary = 3000;
            expected.SalaryFrequency = "monthly";
            expected.OtherIncome = 50;
            expected.OtherincomeFrequency = "weekly";
            expected.Pension = 125;
            expected.PensionFrequency = "fortnightly";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            IncomeVm source = new IncomeVm()
            {
                BenefitsAndTaxCredits = new IncomeSourceVm() { Amount = 100, Frequency = "weekly" },
                Earning = new IncomeSourceVm() { Amount = 3000, Frequency = "monthly" },
                Pension = new IncomeSourceVm() { Amount = 125, Frequency = "fortnightly" },
                Other = new IncomeSourceVm() { Amount = 50, Frequency = "weekly" },
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "123456789" } },
                EnabledPartialSave = true,
                HasErrorPartialSavedIAndE = true,
                PartialSavedEvent = true,
                PartialSavedIAndE = true
            };

            //Create a copy of source for later
            IncomeVm sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = null;
            IncomeAndExpenditure expected = new IncomeAndExpenditure();

            expected.BenefitsTotal = 100;
            expected.BenefitsTotalFrequency = "weekly";
            expected.Salary = 3000;
            expected.SalaryFrequency = "monthly";
            expected.OtherIncome = 50;
            expected.OtherincomeFrequency = "weekly";
            expected.Pension = 125;
            expected.PensionFrequency = "fortnightly";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }
    }
}
