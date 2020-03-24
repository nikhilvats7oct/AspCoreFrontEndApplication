using System;
using System.Collections.Generic;
using System.Text;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Services.Models.OpenWrks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class OpenWrksAndIandEMappingConverterTests
    {
        private OpenWrksAndIandEMappingConverter _converter;

        [TestInitialize]
        public void TestInitialise()
        {
            this._converter = new OpenWrksAndIandEMappingConverter();
        }

        [TestMethod]
        public void ConvertTest_SourceNull()
        {
            IncomeAndExpenditure destination = new IncomeAndExpenditure();
         
            IncomeAndExpenditure result = _converter.Convert(null, destination, null);

            Assert.AreEqual(null, result);

            //Check that destination hasn't been modified
            Assert.IsTrue((Utilities.DeepCompare(new IncomeAndExpenditure(), destination)));
        }

        [TestMethod]
        public void ConvertTest()
        {
            OpenWrksBudget source = new OpenWrksBudget
            {
                CustomerReference = "12345678",
                DisposableIncome = 2345.67M,
                MoneyLeft = 1234.56M,
                Income = new OpenWrksIncome[]
                {
                    new OpenWrksIncome(){ MonthlyAmount = 11, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Salary },
                    new OpenWrksIncome(){ MonthlyAmount = 13, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PartnerSalary },
                    new OpenWrksIncome(){ MonthlyAmount = 15, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherEarnings },
                    new OpenWrksIncome(){ MonthlyAmount = 17, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherIncome },
                    new OpenWrksIncome(){ MonthlyAmount = 19, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.StudentGrants },
                    new OpenWrksIncome(){ MonthlyAmount = 21, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ChildSupport },
                    new OpenWrksIncome(){ MonthlyAmount = 23, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherPensions },
                    new OpenWrksIncome(){ MonthlyAmount = 25, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.DisabilityBenefits },
                    new OpenWrksIncome(){ MonthlyAmount = 27, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.CarersAllowance },
                    new OpenWrksIncome(){ MonthlyAmount = 29, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.EmploymentSupport },
                    new OpenWrksIncome(){ MonthlyAmount = 31, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.IncomeSupport },
                    new OpenWrksIncome(){ MonthlyAmount = 33, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.JobSeekersAllowance },
                    new OpenWrksIncome(){ MonthlyAmount = 35, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.LocalHousingAllowance },
                    new OpenWrksIncome(){ MonthlyAmount = 37, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherBenefits },
                    new OpenWrksIncome(){ MonthlyAmount = 39, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.UniversalCredit },
                    new OpenWrksIncome(){ MonthlyAmount = 41, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.WorkingTaxCredit },
                    new OpenWrksIncome(){ MonthlyAmount = 43, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.StatePensions },
                    new OpenWrksIncome(){ MonthlyAmount = 45, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PensionCredit },
                    new OpenWrksIncome(){ MonthlyAmount = 47, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PrivateOrWorkPension },

                },
                Expenditure = new OpenWrksExpenditure[]
                {
                    new OpenWrksExpenditure() { MonthlyAmount = 10, MonthlyHouseholdAmount = 11, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Mortgage },
                    new OpenWrksExpenditure() { MonthlyAmount = 12, MonthlyHouseholdAmount = 13, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Rent },
                    new OpenWrksExpenditure() { MonthlyAmount = 14, MonthlyHouseholdAmount = 15, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.CouncilTax },
                    new OpenWrksExpenditure() { MonthlyAmount = 16, MonthlyHouseholdAmount = 17, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.TvLicence },
                    new OpenWrksExpenditure() { MonthlyAmount = 18, MonthlyHouseholdAmount = 19, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.SecuredLoans },
                    new OpenWrksExpenditure() { MonthlyAmount = 20, MonthlyHouseholdAmount = 21, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ApplianceRental },
                    new OpenWrksExpenditure() { MonthlyAmount = 22, MonthlyHouseholdAmount = 23, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Energy },
                    new OpenWrksExpenditure() { MonthlyAmount = 24, MonthlyHouseholdAmount = 25, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.WaterSupply },
                    new OpenWrksExpenditure() { MonthlyAmount = 26, MonthlyHouseholdAmount = 27, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ChildSupport },
                    new OpenWrksExpenditure() { MonthlyAmount = 28, MonthlyHouseholdAmount = 29, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ChildcareCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 30, MonthlyHouseholdAmount = 31, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.AdultCareCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 32, MonthlyHouseholdAmount = 33, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PrescriptsAndMedicines },
                    new OpenWrksExpenditure() { MonthlyAmount = 34, MonthlyHouseholdAmount = 35, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherHealthCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 36, MonthlyHouseholdAmount = 37, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.BreakdownCover },
                    new OpenWrksExpenditure() { MonthlyAmount = 38, MonthlyHouseholdAmount = 39, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.CarInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 40, MonthlyHouseholdAmount = 41, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.FuelAndParkingCharges },
                    new OpenWrksExpenditure() { MonthlyAmount = 42, MonthlyHouseholdAmount = 43, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.RoadTax },
                    new OpenWrksExpenditure() { MonthlyAmount = 44, MonthlyHouseholdAmount = 45, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.VehicleMaintenance },
                    new OpenWrksExpenditure() { MonthlyAmount = 46, MonthlyHouseholdAmount = 47, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HirePurchase },
                    new OpenWrksExpenditure() { MonthlyAmount = 48, MonthlyHouseholdAmount = 49, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PublicTransport },
                    new OpenWrksExpenditure() { MonthlyAmount = 50, MonthlyHouseholdAmount = 51, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherTransportAndTravelCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 52, MonthlyHouseholdAmount = 53, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.SchoolCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 54, MonthlyHouseholdAmount = 55, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PensionPayments },
                    new OpenWrksExpenditure() { MonthlyAmount = 56, MonthlyHouseholdAmount = 57, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.LifeInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 58, MonthlyHouseholdAmount = 59, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HealthInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 60, MonthlyHouseholdAmount = 61, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.BuildingInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 62, MonthlyHouseholdAmount = 63, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherPensionAndInsuranceCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 64, MonthlyHouseholdAmount = 65, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ProfessionCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 66, MonthlyHouseholdAmount = 67, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Mobile },
                    new OpenWrksExpenditure() { MonthlyAmount = 68, MonthlyHouseholdAmount = 69, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Gifts },
                    new OpenWrksExpenditure() { MonthlyAmount = 70, MonthlyHouseholdAmount = 71, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HomePhoneAndInternet },
                    new OpenWrksExpenditure() { MonthlyAmount = 72, MonthlyHouseholdAmount = 73, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HobbiesAndSport },
                    new OpenWrksExpenditure() { MonthlyAmount = 74, MonthlyHouseholdAmount = 75, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.GymsAndExecrises },
                    new OpenWrksExpenditure() { MonthlyAmount = 76, MonthlyHouseholdAmount = 77, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PubsAndCafes },
                    new OpenWrksExpenditure() { MonthlyAmount = 78, MonthlyHouseholdAmount = 79, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.EatingOutAndTakeaways },
                    new OpenWrksExpenditure() { MonthlyAmount = 80, MonthlyHouseholdAmount = 81, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Groceries },
                    new OpenWrksExpenditure() { MonthlyAmount = 82, MonthlyHouseholdAmount = 83, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HouseRepairs },
                    new OpenWrksExpenditure() { MonthlyAmount = 84, MonthlyHouseholdAmount = 85, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PetInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 86, MonthlyHouseholdAmount = 87, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PersonalCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 88, MonthlyHouseholdAmount = 89, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Savings },
                    new OpenWrksExpenditure() { MonthlyAmount = 90, MonthlyHouseholdAmount = 91, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.NonPriorityDebts },
                    new OpenWrksExpenditure() { MonthlyAmount = 92, MonthlyHouseholdAmount = 93, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.NonPriorityDebts },
                    new OpenWrksExpenditure() { MonthlyAmount = 94, MonthlyHouseholdAmount = 95, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PriorityDebts },
                    new OpenWrksExpenditure() { MonthlyAmount = 96, MonthlyHouseholdAmount = 97, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PriorityDebts },
                },
                CardsAndLoans = new OpenWrksCardAndLoan[]
                {
                },
                ArrearsAndFines = new OpenWrksArrearsAndFine[]
                {
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 10, MonthlyHouseholdAmount = 11, IsPaying = true, Type = Constants.OpenWorksArrears.Energy },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 12, MonthlyHouseholdAmount = 13, IsPaying = true, Type = Constants.OpenWorksArrears.Water },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 14, MonthlyHouseholdAmount = 15, IsPaying = true, Type = Constants.OpenWorksArrears.ChildMaintanence },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 16, MonthlyHouseholdAmount = 17, IsPaying = true, Type = Constants.OpenWorksArrears.TvLicence },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 18, MonthlyHouseholdAmount = 19, IsPaying = true, Type = Constants.OpenWorksArrears.CouncilTax },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 20, MonthlyHouseholdAmount = 21, IsPaying = true, Type = Constants.OpenWorksArrears.Rent },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 22, MonthlyHouseholdAmount = 23, IsPaying = true, Type = Constants.OpenWorksArrears.Mortgage },
                },
                Household = new OpenWrksHousehold
                {
                    LivesWithPartner = true,
                    DependentChildren = 1,
                    DependentYoungAdults = 2,
                    DependentAdults = 3,
                    NonDependentCohabitants = 4
                },
                Message = null
            };

            //Create a copy of source for later
            OpenWrksBudget sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = new IncomeAndExpenditure();
            IncomeAndExpenditure expected = new IncomeAndExpenditure
            {
                LowellReference = null,
                User = null,
                Created = default,
                HasArrears = true,
                AdultsInHousehold = 9,
                ChildrenUnder16 = 1,
                Children16to18 = 2,
                Salary = 11,
                SalaryFrequency = "monthly",
                Pension = 135,
                PensionFrequency = "monthly",
                EarningsTotal = 551,
                EarningsTotalFrequency = "monthly",
                BenefitsTotal = 297,
                BenefitsTotalFrequency = "monthly",
                OtherIncome = 108,
                OtherincomeFrequency = "monthly",
                Mortgage = 11,
                MortgageFrequency = "monthly",
                MortgageArrears = 23,
                Rent = 13,
                RentFrequency = "monthly",
                RentArrears = 21,
                SecuredLoans = 19,
                SecuredLoansFrequency = "monthly",
                SecuredloansArrears = 0,
                CouncilTax = 15,
                CouncilTaxFrequency = "monthly",
                CouncilTaxArrears = 19,
                Rental = 21,
                RentalFrequency = "monthly",
                RentalArrears = 0,
                TvLicence = 17,
                TvLicenceFrequency = "monthly",
                TvLicenceArrears = 17,
                HomeContents = 0,
                HomeContentsFrequency = null,
                HomeContentsArrears = 0,
                Gas = 0,
                GasFrequency = null,
                GasArrears = 0,
                OtherUtilities = 23,
                OtherUtilitiesFrequency = "monthly",
                OtherUtilitiesArrears = 11,
                Electricity = 0,
                ElectricityFrequency = null,
                ElectricityArrears = 0,
                Water = 25,
                WaterFrequency = "monthly",
                WaterArrears = 13,
                UtilitiesTotal = 0,
                UtilitiesTotalFrequency = null,
                UtilitiesTotalArrears = 0,
                ChildMaintenance = 56,
                ChildMaintenanceFrequency = "monthly",
                ChildMaintenanceArrears = 15,
                CCJs = 0,
                CCJsFrequency = null,
                CCJsArrears = 0,
                CourtFines = 0,
                CourtFinesFrequency = null,
                CourtFinesArrears = 0,
                Housekeeping = 249,
                HousekeepingFrequency = "monthly",
                PersonalCosts = 87,
                PersonalCostsFrequency = "monthly",
                Leisure = 511,
                LeisureFrequency = "monthly",
                Travel = 352,
                TravelFrequency = "monthly",
                Healthcare = 99,
                HealthcareFrequency = "monthly",
                PensionInsurance = 295,
                PensionInsuranceFrequency = "monthly",
                SchoolCosts = 53,
                SchoolCostsFrequency = "monthly",
                ProfessionalCosts = 65,
                ProfessionalCostsFrequency = "monthly",
                SavingsContributions = 89,
                SavingsContributionsFrequency = "monthly",
                OtherExpenditure = 0,
                OtherExpenditureFrequency = null,
                IncomeTotal = 551,
                ExpenditureTotal = 2094,
                DisposableIncome = 0,
                HousingStatus = null,
                EmploymentStatus = null,
                OtherDebts = new List<SaveOtherDebts>()
                {
                    new SaveOtherDebts
                    {
                        Amount = 91,
                        Frequency = "monthly",
                        CountyCourtJudgement = false,
                        Arrears = 0
                    },
                    new SaveOtherDebts
                    {
                        Amount = 93,
                        Frequency = "monthly",
                        CountyCourtJudgement = false,
                        Arrears = 0
                    },
                    new SaveOtherDebts
                    {
                        Amount = 95,
                        Frequency = "monthly",
                        CountyCourtJudgement = false,
                        Arrears = 0
                    },
                    new SaveOtherDebts
                    {
                        Amount = 97,
                        Frequency = "monthly",
                        CountyCourtJudgement = false,
                        Arrears = 0
                    }
                }
            };

            IncomeAndExpenditure result = this._converter.Convert(source, destination, null);

            Assert.AreSame(result, destination);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            OpenWrksBudget source = new OpenWrksBudget
            {
                CustomerReference = "12345678",
                DisposableIncome = 2345.67M,
                MoneyLeft = 1234.56M,
                Income = new OpenWrksIncome[]
                {
                    new OpenWrksIncome(){ MonthlyAmount = 11, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Salary },
                    new OpenWrksIncome(){ MonthlyAmount = 13, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PartnerSalary },
                    new OpenWrksIncome(){ MonthlyAmount = 15, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherEarnings },
                    new OpenWrksIncome(){ MonthlyAmount = 17, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherIncome },
                    new OpenWrksIncome(){ MonthlyAmount = 19, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.StudentGrants },
                    new OpenWrksIncome(){ MonthlyAmount = 21, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ChildSupport },
                    new OpenWrksIncome(){ MonthlyAmount = 23, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherPensions },
                    new OpenWrksIncome(){ MonthlyAmount = 25, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.DisabilityBenefits },
                    new OpenWrksIncome(){ MonthlyAmount = 27, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.CarersAllowance },
                    new OpenWrksIncome(){ MonthlyAmount = 29, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.EmploymentSupport },
                    new OpenWrksIncome(){ MonthlyAmount = 31, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.IncomeSupport },
                    new OpenWrksIncome(){ MonthlyAmount = 33, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.JobSeekersAllowance },
                    new OpenWrksIncome(){ MonthlyAmount = 35, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.LocalHousingAllowance },
                    new OpenWrksIncome(){ MonthlyAmount = 37, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherBenefits },
                    new OpenWrksIncome(){ MonthlyAmount = 39, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.UniversalCredit },
                    new OpenWrksIncome(){ MonthlyAmount = 41, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.WorkingTaxCredit },
                    new OpenWrksIncome(){ MonthlyAmount = 43, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.StatePensions },
                    new OpenWrksIncome(){ MonthlyAmount = 45, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PensionCredit },
                    new OpenWrksIncome(){ MonthlyAmount = 47, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PrivateOrWorkPension },

                },
                Expenditure = new OpenWrksExpenditure[]
                {
                    new OpenWrksExpenditure() { MonthlyAmount = 10, MonthlyHouseholdAmount = 11, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Mortgage },
                    new OpenWrksExpenditure() { MonthlyAmount = 12, MonthlyHouseholdAmount = 13, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Rent },
                    new OpenWrksExpenditure() { MonthlyAmount = 14, MonthlyHouseholdAmount = 15, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.CouncilTax },
                    new OpenWrksExpenditure() { MonthlyAmount = 16, MonthlyHouseholdAmount = 17, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.TvLicence },
                    new OpenWrksExpenditure() { MonthlyAmount = 18, MonthlyHouseholdAmount = 19, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.SecuredLoans },
                    new OpenWrksExpenditure() { MonthlyAmount = 20, MonthlyHouseholdAmount = 21, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ApplianceRental },
                    new OpenWrksExpenditure() { MonthlyAmount = 22, MonthlyHouseholdAmount = 23, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Energy },
                    new OpenWrksExpenditure() { MonthlyAmount = 24, MonthlyHouseholdAmount = 25, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.WaterSupply },
                    new OpenWrksExpenditure() { MonthlyAmount = 26, MonthlyHouseholdAmount = 27, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ChildSupport },
                    new OpenWrksExpenditure() { MonthlyAmount = 28, MonthlyHouseholdAmount = 29, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ChildcareCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 30, MonthlyHouseholdAmount = 31, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.AdultCareCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 32, MonthlyHouseholdAmount = 33, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PrescriptsAndMedicines },
                    new OpenWrksExpenditure() { MonthlyAmount = 34, MonthlyHouseholdAmount = 35, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherHealthCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 36, MonthlyHouseholdAmount = 37, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.BreakdownCover },
                    new OpenWrksExpenditure() { MonthlyAmount = 38, MonthlyHouseholdAmount = 39, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.CarInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 40, MonthlyHouseholdAmount = 41, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.FuelAndParkingCharges },
                    new OpenWrksExpenditure() { MonthlyAmount = 42, MonthlyHouseholdAmount = 43, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.RoadTax },
                    new OpenWrksExpenditure() { MonthlyAmount = 44, MonthlyHouseholdAmount = 45, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.VehicleMaintenance },
                    new OpenWrksExpenditure() { MonthlyAmount = 46, MonthlyHouseholdAmount = 47, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HirePurchase },
                    new OpenWrksExpenditure() { MonthlyAmount = 48, MonthlyHouseholdAmount = 49, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PublicTransport },
                    new OpenWrksExpenditure() { MonthlyAmount = 50, MonthlyHouseholdAmount = 51, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherTransportAndTravelCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 52, MonthlyHouseholdAmount = 53, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.SchoolCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 54, MonthlyHouseholdAmount = 55, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PensionPayments },
                    new OpenWrksExpenditure() { MonthlyAmount = 56, MonthlyHouseholdAmount = 57, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.LifeInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 58, MonthlyHouseholdAmount = 59, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HealthInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 60, MonthlyHouseholdAmount = 61, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.BuildingInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 62, MonthlyHouseholdAmount = 63, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.OtherPensionAndInsuranceCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 64, MonthlyHouseholdAmount = 65, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.ProfessionCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 66, MonthlyHouseholdAmount = 67, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Mobile },
                    new OpenWrksExpenditure() { MonthlyAmount = 68, MonthlyHouseholdAmount = 69, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Gifts },
                    new OpenWrksExpenditure() { MonthlyAmount = 70, MonthlyHouseholdAmount = 71, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HomePhoneAndInternet },
                    new OpenWrksExpenditure() { MonthlyAmount = 72, MonthlyHouseholdAmount = 73, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HobbiesAndSport },
                    new OpenWrksExpenditure() { MonthlyAmount = 74, MonthlyHouseholdAmount = 75, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.GymsAndExecrises },
                    new OpenWrksExpenditure() { MonthlyAmount = 76, MonthlyHouseholdAmount = 77, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PubsAndCafes },
                    new OpenWrksExpenditure() { MonthlyAmount = 78, MonthlyHouseholdAmount = 79, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.EatingOutAndTakeaways },
                    new OpenWrksExpenditure() { MonthlyAmount = 80, MonthlyHouseholdAmount = 81, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Groceries },
                    new OpenWrksExpenditure() { MonthlyAmount = 82, MonthlyHouseholdAmount = 83, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.HouseRepairs },
                    new OpenWrksExpenditure() { MonthlyAmount = 84, MonthlyHouseholdAmount = 85, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PetInsurance },
                    new OpenWrksExpenditure() { MonthlyAmount = 86, MonthlyHouseholdAmount = 87, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PersonalCosts },
                    new OpenWrksExpenditure() { MonthlyAmount = 88, MonthlyHouseholdAmount = 89, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.Savings },
                    new OpenWrksExpenditure() { MonthlyAmount = 90, MonthlyHouseholdAmount = 91, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.NonPriorityDebts },
                    new OpenWrksExpenditure() { MonthlyAmount = 92, MonthlyHouseholdAmount = 93, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.NonPriorityDebts },
                    new OpenWrksExpenditure() { MonthlyAmount = 94, MonthlyHouseholdAmount = 95, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PriorityDebts },
                    new OpenWrksExpenditure() { MonthlyAmount = 96, MonthlyHouseholdAmount = 97, SfsCategory = Constants.OpenWrksIncomeAndExpenditures.PriorityDebts },
                },
                CardsAndLoans = new OpenWrksCardAndLoan[]
                {
                },
                ArrearsAndFines = new OpenWrksArrearsAndFine[]
                {
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 10, MonthlyHouseholdAmount = 11, IsPaying = true, Type = Constants.OpenWorksArrears.Energy },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 12, MonthlyHouseholdAmount = 13, IsPaying = true, Type = Constants.OpenWorksArrears.Water },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 14, MonthlyHouseholdAmount = 15, IsPaying = true, Type = Constants.OpenWorksArrears.ChildMaintanence },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 16, MonthlyHouseholdAmount = 17, IsPaying = true, Type = Constants.OpenWorksArrears.TvLicence },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 18, MonthlyHouseholdAmount = 19, IsPaying = true, Type = Constants.OpenWorksArrears.CouncilTax },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 20, MonthlyHouseholdAmount = 21, IsPaying = true, Type = Constants.OpenWorksArrears.Rent },
                    new OpenWrksArrearsAndFine() { MonthlyAmount = 22, MonthlyHouseholdAmount = 23, IsPaying = true, Type = Constants.OpenWorksArrears.Mortgage },
                },
                Household = new OpenWrksHousehold
                {
                    LivesWithPartner = true,
                    DependentChildren = 1,
                    DependentYoungAdults = 2,
                    DependentAdults = 3,
                    NonDependentCohabitants = 4
                },
                Message = null
            };

            //Create a copy of source for later
            OpenWrksBudget sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = null;
            IncomeAndExpenditure expected = new IncomeAndExpenditure
            {
                LowellReference = null,
                User = null,
                Created = default,
                HasArrears = true,
                AdultsInHousehold = 9,
                ChildrenUnder16 = 1,
                Children16to18 = 2,
                Salary = 11,
                SalaryFrequency = "monthly",
                Pension = 135,
                PensionFrequency = "monthly",
                EarningsTotal = 551,
                EarningsTotalFrequency = "monthly",
                BenefitsTotal = 297,
                BenefitsTotalFrequency = "monthly",
                OtherIncome = 108,
                OtherincomeFrequency = "monthly",
                Mortgage = 11,
                MortgageFrequency = "monthly",
                MortgageArrears = 23,
                Rent = 13,
                RentFrequency = "monthly",
                RentArrears = 21,
                SecuredLoans = 19,
                SecuredLoansFrequency = "monthly",
                SecuredloansArrears = 0,
                CouncilTax = 15,
                CouncilTaxFrequency = "monthly",
                CouncilTaxArrears = 19,
                Rental = 21,
                RentalFrequency = "monthly",
                RentalArrears = 0,
                TvLicence = 17,
                TvLicenceFrequency = "monthly",
                TvLicenceArrears = 17,
                HomeContents = 0,
                HomeContentsFrequency = null,
                HomeContentsArrears = 0,
                Gas = 0,
                GasFrequency = null,
                GasArrears = 0,
                OtherUtilities = 23,
                OtherUtilitiesFrequency = "monthly",
                OtherUtilitiesArrears = 11,
                Electricity = 0,
                ElectricityFrequency = null,
                ElectricityArrears = 0,
                Water = 25,
                WaterFrequency = "monthly",
                WaterArrears = 13,
                UtilitiesTotal = 0,
                UtilitiesTotalFrequency = null,
                UtilitiesTotalArrears = 0,
                ChildMaintenance = 56,
                ChildMaintenanceFrequency = "monthly",
                ChildMaintenanceArrears = 15,
                CCJs = 0,
                CCJsFrequency = null,
                CCJsArrears = 0,
                CourtFines = 0,
                CourtFinesFrequency = null,
                CourtFinesArrears = 0,
                Housekeeping = 249,
                HousekeepingFrequency = "monthly",
                PersonalCosts = 87,
                PersonalCostsFrequency = "monthly",
                Leisure = 511,
                LeisureFrequency = "monthly",
                Travel = 352,
                TravelFrequency = "monthly",
                Healthcare = 99,
                HealthcareFrequency = "monthly",
                PensionInsurance = 295,
                PensionInsuranceFrequency = "monthly",
                SchoolCosts = 53,
                SchoolCostsFrequency = "monthly",
                ProfessionalCosts = 65,
                ProfessionalCostsFrequency = "monthly",
                SavingsContributions = 89,
                SavingsContributionsFrequency = "monthly",
                OtherExpenditure = 0,
                OtherExpenditureFrequency = null,
                IncomeTotal = 551,
                ExpenditureTotal = 2094,
                DisposableIncome = 0,
                HousingStatus = null,
                EmploymentStatus = null,
                OtherDebts = new List<SaveOtherDebts>()
                {
                    new SaveOtherDebts
                    {
                        Amount = 91,
                        Frequency = "monthly",
                        CountyCourtJudgement = false,
                        Arrears = 0
                    },
                    new SaveOtherDebts
                    {
                        Amount = 93,
                        Frequency = "monthly",
                        CountyCourtJudgement = false,
                        Arrears = 0
                    },
                    new SaveOtherDebts
                    {
                        Amount = 95,
                        Frequency = "monthly",
                        CountyCourtJudgement = false,
                        Arrears = 0
                    },
                    new SaveOtherDebts
                    {
                        Amount = 97,
                        Frequency = "monthly",
                        CountyCourtJudgement = false,
                        Arrears = 0
                    }
                }
            };

            IncomeAndExpenditure result = this._converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

    }
}
