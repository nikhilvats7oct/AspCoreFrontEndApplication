using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class ExpendituresVmToIncomeAndExpenditureConverterTests
    {
        private ExpendituresVmToIncomeAndExpenditureConverter _converter;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._converter = new ExpendituresVmToIncomeAndExpenditureConverter();
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            ExpendituresVm source = null;
            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();
            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);

            expected.Healthcare = 0;
            expected.HealthcareFrequency = null;
            expected.Leisure = 0;
            expected.LeisureFrequency = null;
            expected.Housekeeping = 0;
            expected.HousekeepingFrequency = null;
            expected.OtherExpenditure = 0;
            expected.OtherExpenditureFrequency = null;
            expected.PensionInsurance = 0;
            expected.PensionInsuranceFrequency = null;
            expected.PersonalCosts = 0;
            expected.PersonalCostsFrequency = null;
            expected.ProfessionalCosts = 0;
            expected.ProfessionalCostsFrequency = null;
            expected.SavingsContributions = 0;
            expected.SavingsContributionsFrequency = null;
            expected.SchoolCosts = 0;
            expected.SchoolCostsFrequency = null;
            expected.Travel = 0;
            expected.TravelFrequency = null;

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest()
        {
            ExpendituresVm source = new ExpendituresVm() 
            {
                CareAndHealthCosts = new ExpenditureSourceVm() { Amount = 100, Frequency = "monthly" },
                CommunicationsAndLeisure = new ExpenditureSourceVm() { Amount = 150, Frequency = "weekly" },
                FoodAndHouseKeeping = new ExpenditureSourceVm() { Amount = 200, Frequency = "fortnightly" },
                Other = new ExpenditureSourceVm() { Amount = 250, Frequency = "monthly" },
                PensionsAndInsurance = new ExpenditureSourceVm() { Amount = 300, Frequency = "weekly" },
                PersonalCosts = new ExpenditureSourceVm() { Amount = 350, Frequency = "fortnightly" },
                Professional = new ExpenditureSourceVm() { Amount = 400, Frequency = "monthly" },
                Savings = new ExpenditureSourceVm() { Amount = 450, Frequency = "weekly" },
                SchoolCosts = new ExpenditureSourceVm() { Amount = 500, Frequency = "fortnightly"},
                TravelAndTransport = new ExpenditureSourceVm() { Amount = 550, Frequency = "monthly" },
                EnabledPartialSave = true,
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "123456789" } },
                IncomeVmSummary = new MonthlyIncomeVm() 
                {
                    Benefits = 100,
                    Other = 200,
                    Pension = 300,
                    Salary = 400,
                    Total = 1000
                },
                OutgoingsVmSummary = new MonthlyOutgoingsVm() 
                {
                    Expenditures = 1000,
                    HouseholdBills = 2000,
                    Total = 3000
                },
                CommunicationsAndLeisureTriggerMax = 456,
                CommunicationsAndLeisureTriggerMin = 123,
                HasErrorPartialSavedIAndE = true,
                FoodAndHouseKeepingTriggerMin = 567,
                FoodAndHouseKeepingTriggerMax = 789,
                PartialSavedEvent = true,
                PartialSavedIAndE = true,
                PersonalCostsTriggerMax = 234,
                PersonalCostsTriggerMin = 567
            };

            //Create a copy of source for later
            ExpendituresVm sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();
            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);

            expected.Healthcare = 100;
            expected.HealthcareFrequency = "monthly";
            expected.Leisure = 150;
            expected.LeisureFrequency = "weekly";
            expected.Housekeeping = 200;
            expected.HousekeepingFrequency = "fortnightly";
            expected.OtherExpenditure = 250;
            expected.OtherExpenditureFrequency = "monthly";
            expected.PensionInsurance = 300;
            expected.PensionInsuranceFrequency = "weekly";
            expected.PersonalCosts = 350;
            expected.PersonalCostsFrequency = "fortnightly";
            expected.ProfessionalCosts = 400;
            expected.ProfessionalCostsFrequency = "monthly";
            expected.SavingsContributions = 450;
            expected.SavingsContributionsFrequency = "weekly";
            expected.SchoolCosts = 500;
            expected.SchoolCostsFrequency = "fortnightly";
            expected.Travel = 550;
            expected.TravelFrequency = "monthly";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            ExpendituresVm source = new ExpendituresVm()
            {
                CareAndHealthCosts = new ExpenditureSourceVm() { Amount = 100, Frequency = "monthly" },
                CommunicationsAndLeisure = new ExpenditureSourceVm() { Amount = 150, Frequency = "weekly" },
                FoodAndHouseKeeping = new ExpenditureSourceVm() { Amount = 200, Frequency = "fortnightly" },
                Other = new ExpenditureSourceVm() { Amount = 250, Frequency = "monthly" },
                PensionsAndInsurance = new ExpenditureSourceVm() { Amount = 300, Frequency = "weekly" },
                PersonalCosts = new ExpenditureSourceVm() { Amount = 350, Frequency = "fortnightly" },
                Professional = new ExpenditureSourceVm() { Amount = 400, Frequency = "monthly" },
                Savings = new ExpenditureSourceVm() { Amount = 450, Frequency = "weekly" },
                SchoolCosts = new ExpenditureSourceVm() { Amount = 500, Frequency = "fortnightly" },
                TravelAndTransport = new ExpenditureSourceVm() { Amount = 550, Frequency = "monthly" },
                EnabledPartialSave = true,
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "123456789" } },
                IncomeVmSummary = new MonthlyIncomeVm()
                {
                    Benefits = 100,
                    Other = 200,
                    Pension = 300,
                    Salary = 400,
                    Total = 1000
                },
                OutgoingsVmSummary = new MonthlyOutgoingsVm()
                {
                    Expenditures = 1000,
                    HouseholdBills = 2000,
                    Total = 3000
                },
                CommunicationsAndLeisureTriggerMax = 456,
                CommunicationsAndLeisureTriggerMin = 123,
                HasErrorPartialSavedIAndE = true,
                FoodAndHouseKeepingTriggerMin = 567,
                FoodAndHouseKeepingTriggerMax = 789,
                PartialSavedEvent = true,
                PartialSavedIAndE = true,
                PersonalCostsTriggerMax = 234,
                PersonalCostsTriggerMin = 567
            };

            //Create a copy of source for later
            ExpendituresVm sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = null;
            IncomeAndExpenditure expected = new IncomeAndExpenditure();

            expected.Healthcare = 100;
            expected.HealthcareFrequency = "monthly";
            expected.Leisure = 150;
            expected.LeisureFrequency = "weekly";
            expected.Housekeeping = 200;
            expected.HousekeepingFrequency = "fortnightly";
            expected.OtherExpenditure = 250;
            expected.OtherExpenditureFrequency = "monthly";
            expected.PensionInsurance = 300;
            expected.PensionInsuranceFrequency = "weekly";
            expected.PersonalCosts = 350;
            expected.PersonalCostsFrequency = "fortnightly";
            expected.ProfessionalCosts = 400;
            expected.ProfessionalCostsFrequency = "monthly";
            expected.SavingsContributions = 450;
            expected.SavingsContributionsFrequency = "weekly";
            expected.SchoolCosts = 500;
            expected.SchoolCostsFrequency = "fortnightly";
            expected.Travel = 550;
            expected.TravelFrequency = "monthly";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

    }
}
