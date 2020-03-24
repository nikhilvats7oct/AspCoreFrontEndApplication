using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class HouseholdStatusVmToIncomeAndExpenditureConverterTests
    {
        private HouseholdStatusVmToIncomeAndExpenditureConverter _converter;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._converter = new HouseholdStatusVmToIncomeAndExpenditureConverter();
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            HouseholdStatusVm source = null;
            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();
            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);

            expected.AdultsInHousehold = 0;
            expected.Children16to18 = 0;
            expected.ChildrenUnder16 = 0;
            expected.EmploymentStatus = null;
            expected.HousingStatus = null;

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest()
        {
            HouseholdStatusVm source = new HouseholdStatusVm() 
            {
                AdultsInHousehold = 1,
                ChildrenOver16 = 2,
                ChildrenUnder16 = 3,
                EmploymentStatus = "employed-full-time",
                ExternallyLaunched = true,
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "123456789" } },
                HousingStatus = "homeowner",
                PartialSavedIAndE = true,
                SavedIAndE = true
            };

            //Create a copy of source for later
            HouseholdStatusVm sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();
            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);

            expected.AdultsInHousehold = 1;
            expected.Children16to18 = 2;
            expected.ChildrenUnder16 = 3;
            expected.EmploymentStatus = "employed-full-time";
            expected.HousingStatus = "homeowner";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            HouseholdStatusVm source = new HouseholdStatusVm()
            {
                AdultsInHousehold = 1,
                ChildrenOver16 = 2,
                ChildrenUnder16 = 3,
                EmploymentStatus = "employed-full-time",
                ExternallyLaunched = true,
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "123456789" } },
                HousingStatus = "homeowner",
                PartialSavedIAndE = true,
                SavedIAndE = true
            };

            //Create a copy of source for later
            HouseholdStatusVm sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = null;
            IncomeAndExpenditure expected = new IncomeAndExpenditure();

            expected.AdultsInHousehold = 1;
            expected.Children16to18 = 2;
            expected.ChildrenUnder16 = 3;
            expected.EmploymentStatus = "employed-full-time";
            expected.HousingStatus = "homeowner";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

    }
}
