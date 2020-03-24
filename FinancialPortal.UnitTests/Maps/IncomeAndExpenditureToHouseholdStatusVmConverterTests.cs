using AutoMapper;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class IncomeAndExpenditureToHouseholdStatusVmConverterTests
    {
        private IncomeAndExpenditureToHouseholdStatusVmConverter _converter;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._converter = new IncomeAndExpenditureToHouseholdStatusVmConverter();
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            IncomeAndExpenditure source = null;
            HouseholdStatusVm destination = new HouseholdStatusVm()
            {
                AdultsInHousehold = Int32.MaxValue,
                ChildrenOver16 = Int32.MaxValue,
                ChildrenUnder16 = Int32.MaxValue,
                HousingStatus = "test",
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "test" } },
                EmploymentStatus = "test",
                ExternallyLaunched = true,
                PartialSavedIAndE = true,
                SavedIAndE = true
            };

            HouseholdStatusVm expected = Utilities.DeepCopy(destination);

            expected.AdultsInHousehold = 0;
            expected.ChildrenOver16 = 0;
            expected.ChildrenUnder16 = 0;
            expected.EmploymentStatus = null;
            expected.HousingStatus = null;

            HouseholdStatusVm result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest()
        {
            IncomeAndExpenditure source = new IncomeAndExpenditure()
            {
                Mortgage = 100,
                MortgageArrears = 10,
                MortgageFrequency = "monthly",
                Rental = 150,
                RentalArrears = 0,
                RentalFrequency = "weekly",
                Electricity = 200,
                ElectricityArrears = 20,
                ElectricityFrequency = "fortnightly",
                CCJs = 250,
                CCJsArrears = 0,
                CCJsFrequency = "monthly",
                ChildMaintenance = 300,
                ChildMaintenanceArrears = 30,
                ChildMaintenanceFrequency = "weekly",
                CouncilTax = 350,
                CouncilTaxArrears = 0,
                CouncilTaxFrequency = "fortnightly",
                CourtFines = 400,
                CourtFinesArrears = 40,
                CourtFinesFrequency = "monthly",
                Gas = 450,
                GasArrears = 0,
                GasFrequency = "weekly",
                OtherUtilities = 500,
                OtherUtilitiesArrears = 50,
                OtherUtilitiesFrequency = "fortnightly",
                Rent = 550,
                RentArrears = 0,
                RentFrequency = "monthly",
                SecuredLoans = 600,
                SecuredloansArrears = 60,
                SecuredLoansFrequency = "weekly",
                TvLicence = 650,
                TvLicenceArrears = 0,
                TvLicenceFrequency = "fortnightly",
                Water = 700,
                WaterArrears = 70,
                WaterFrequency = "monthly",
                AdultsInHousehold = 1,
                ChildrenUnder16 = 2,
                Children16to18 = 3,
                HasArrears = true,
                UtilitiesTotal = 750,
                UtilitiesTotalArrears = 0,
                UtilitiesTotalFrequency = "weekly",
                User = "webuser",
                Created = DateTime.Now.Date,
                Healthcare = 800,
                HealthcareFrequency = "fortnightly",
                HomeContents = 850,
                HomeContentsArrears = 0,
                HomeContentsFrequency = "monthly",
                EmploymentStatus = "student",
                Housekeeping = 900,
                HousekeepingFrequency = "monthly",
                HousingStatus = "homeowner",
                DisposableIncome = 999.99M,
                BenefitsTotal = 888.88M,
                BenefitsTotalFrequency = "monthly",
                Leisure = 50,
                LeisureFrequency = "weekly",
                OtherDebts = new List<SaveOtherDebts>(),
                OtherExpenditure = 60,
                OtherExpenditureFrequency = "fortnightly",
                OtherIncome = 500,
                OtherincomeFrequency = "monthly",
                LowellReference = "123456789",
                EarningsTotal = 10000,
                EarningsTotalFrequency = "monthly",
                PersonalCosts = 225,
                PersonalCostsFrequency = "monthly",
                Pension = 400,
                PensionFrequency = "monthly",
                Salary = 9999,
                SalaryFrequency = "monthly",
                SavingsContributions = 55,
                SavingsContributionsFrequency = "weekly",
                SchoolCosts = 5,
                SchoolCostsFrequency = "weekly",
                Travel = 45,
                TravelFrequency = "fortnightly",
                ProfessionalCosts = 123,
                ProfessionalCostsFrequency = "monthly",
                IncomeTotal = 1234.56M,
                PensionInsurance = 75,
                PensionInsuranceFrequency = "monthly",
                ExpenditureTotal = 1000
            };

            //Create a copy of source for later
            IncomeAndExpenditure sourceCopy = Utilities.DeepCopy(source);

            HouseholdStatusVm destination = new HouseholdStatusVm()
            {
                AdultsInHousehold = Int32.MaxValue,
                ChildrenOver16 = Int32.MaxValue,
                ChildrenUnder16 = Int32.MaxValue,
                HousingStatus = "test",
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "test" } },
                EmploymentStatus = "test",
                ExternallyLaunched = true,
                PartialSavedIAndE = true,
                SavedIAndE = true
            };

            HouseholdStatusVm expected = Utilities.DeepCopy(destination);

            expected.AdultsInHousehold = 1;
            expected.ChildrenOver16 = 3;
            expected.ChildrenUnder16 = 2;
            expected.EmploymentStatus = "student";
            expected.HousingStatus = "homeowner";

            HouseholdStatusVm result = _converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            IncomeAndExpenditure source = new IncomeAndExpenditure()
            {
                Mortgage = 100,
                MortgageArrears = 10,
                MortgageFrequency = "monthly",
                Rental = 150,
                RentalArrears = 0,
                RentalFrequency = "weekly",
                Electricity = 200,
                ElectricityArrears = 20,
                ElectricityFrequency = "fortnightly",
                CCJs = 250,
                CCJsArrears = 0,
                CCJsFrequency = "monthly",
                ChildMaintenance = 300,
                ChildMaintenanceArrears = 30,
                ChildMaintenanceFrequency = "weekly",
                CouncilTax = 350,
                CouncilTaxArrears = 0,
                CouncilTaxFrequency = "fortnightly",
                CourtFines = 400,
                CourtFinesArrears = 40,
                CourtFinesFrequency = "monthly",
                Gas = 450,
                GasArrears = 0,
                GasFrequency = "weekly",
                OtherUtilities = 500,
                OtherUtilitiesArrears = 50,
                OtherUtilitiesFrequency = "fortnightly",
                Rent = 550,
                RentArrears = 0,
                RentFrequency = "monthly",
                SecuredLoans = 600,
                SecuredloansArrears = 60,
                SecuredLoansFrequency = "weekly",
                TvLicence = 650,
                TvLicenceArrears = 0,
                TvLicenceFrequency = "fortnightly",
                Water = 700,
                WaterArrears = 70,
                WaterFrequency = "monthly",
                AdultsInHousehold = 1,
                ChildrenUnder16 = 2,
                Children16to18 = 3,
                HasArrears = true,
                UtilitiesTotal = 750,
                UtilitiesTotalArrears = 0,
                UtilitiesTotalFrequency = "weekly",
                User = "webuser",
                Created = DateTime.Now.Date,
                Healthcare = 800,
                HealthcareFrequency = "fortnightly",
                HomeContents = 850,
                HomeContentsArrears = 0,
                HomeContentsFrequency = "monthly",
                EmploymentStatus = "student",
                Housekeeping = 900,
                HousekeepingFrequency = "monthly",
                HousingStatus = "homeowner",
                DisposableIncome = 999.99M,
                BenefitsTotal = 888.88M,
                BenefitsTotalFrequency = "monthly",
                Leisure = 50,
                LeisureFrequency = "weekly",
                OtherDebts = new List<SaveOtherDebts>(),
                OtherExpenditure = 60,
                OtherExpenditureFrequency = "fortnightly",
                OtherIncome = 500,
                OtherincomeFrequency = "monthly",
                LowellReference = "123456789",
                EarningsTotal = 10000,
                EarningsTotalFrequency = "monthly",
                PersonalCosts = 225,
                PersonalCostsFrequency = "monthly",
                Pension = 400,
                PensionFrequency = "monthly",
                Salary = 9999,
                SalaryFrequency = "monthly",
                SavingsContributions = 55,
                SavingsContributionsFrequency = "weekly",
                SchoolCosts = 5,
                SchoolCostsFrequency = "weekly",
                Travel = 45,
                TravelFrequency = "fortnightly",
                ProfessionalCosts = 123,
                ProfessionalCostsFrequency = "monthly",
                IncomeTotal = 1234.56M,
                PensionInsurance = 75,
                PensionInsuranceFrequency = "monthly",
                ExpenditureTotal = 1000
            };

            //Create a copy of source for later
            IncomeAndExpenditure sourceCopy = Utilities.DeepCopy(source);

            HouseholdStatusVm destination = null;
            HouseholdStatusVm expected = new HouseholdStatusVm();

            expected.AdultsInHousehold = 1;
            expected.ChildrenOver16 = 3;
            expected.ChildrenUnder16 = 2;
            expected.EmploymentStatus = "student";
            expected.HousingStatus = "homeowner";

            HouseholdStatusVm result = _converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }
    }
}
