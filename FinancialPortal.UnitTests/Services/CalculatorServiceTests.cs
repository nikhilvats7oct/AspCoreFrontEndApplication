using FinancialPortal.Web.Services;
using FinancialPortal.Web.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.Services
{
    [TestClass]
    public class CalculatorServiceTests
    {
        private CalculatorService _calculatorService;

        [TestInitialize]
        public void TestInitialse() 
        {
            this._calculatorService = new CalculatorService();
        }

        [TestMethod]
        public void CalculateMonthlyIncomeTest_IAndENull() 
        {
            IncomeAndExpenditure iAndE = null;

            MonthlyIncome expected = new MonthlyIncome() 
            {
                Salary = 0.00M,
                Benefits = 0.00M,
                Other = 0.00M,
                Pension = 0.00M,
                Total = 0.00M
            };

            MonthlyIncome result = this._calculatorService.CalculateMonthlyIncome(iAndE);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void CalculateMonthlyIncomeTest()
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure() 
            {
                Salary = 2500,
                SalaryFrequency = "monthly",
                BenefitsTotal = 250,
                BenefitsTotalFrequency = "monthly",
                Pension = 100,
                PensionFrequency = "fortnightly",
                OtherIncome = 1000,
                OtherincomeFrequency = "annually"
            };

            MonthlyIncome expected = new MonthlyIncome()
            {
                Salary = 2500,
                Benefits = 250,
                Other = 83.33M,
                Pension = 217,
                Total = 3050.33M
            };

            MonthlyIncome result = this._calculatorService.CalculateMonthlyIncome(iAndE);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void CalculateMonthlyOutgoingsTest_IAndENull() 
        {
            IncomeAndExpenditure iAndE = null;

            MonthlyOutgoings expected = new MonthlyOutgoings()
            {
                Expenditures = 0,
                HouseholdBills = 0,
                Total = 0
            };

            MonthlyOutgoings result = this._calculatorService.CalculateMonthlyOutgoings(iAndE);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void CalculateMonthlyOutgoingsTest()
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure() 
            {
                Mortgage = 500,
                MortgageFrequency = "monthly",
                MortgageArrears = 50,
                Rent = 600,
                RentFrequency = "monthly",
                RentArrears = 60,
                SecuredLoans = 100,
                SecuredLoansFrequency = "monthly",
                SecuredloansArrears = 10,
                CouncilTax = 200,
                CouncilTaxFrequency = "monthly",
                CouncilTaxArrears = 20,
                Rental = 50,
                RentalFrequency = "weekly",
                RentalArrears = 5,
                TvLicence = 10,
                TvLicenceFrequency = "fortnightly",
                TvLicenceArrears = 1,
                Gas = 50,
                GasFrequency = "monthly",
                GasArrears = 5,
                Electricity = 60,
                ElectricityFrequency = "monthly",
                ElectricityArrears = 6,
                OtherUtilities = 1200,
                OtherUtilitiesFrequency = "annually",
                OtherUtilitiesArrears = 120,
                Water = 100,
                WaterFrequency = "quarterly",
                WaterArrears = 10,
                ChildMaintenance = 150,
                ChildMaintenanceFrequency = "monthly",
                ChildMaintenanceArrears = 15,
                CourtFines = 25,
                CourtFinesFrequency = "monthly",
                CourtFinesArrears = 25,
                CCJs = 80,
                CCJsFrequency = "monthly",
                CCJsArrears = 8,

                Housekeeping = 100,
                HousekeepingFrequency = "monthly",
                PersonalCosts = 50,
                PersonalCostsFrequency = "weekly",
                Leisure = 75,
                LeisureFrequency = "fortnightly",
                Travel = 30,
                TravelFrequency = "weekly",
                Healthcare = 1200,
                HealthcareFrequency = "annually",
                PensionInsurance = 100,
                PensionInsuranceFrequency = "quarterly",
                SchoolCosts = 600,
                SchoolCostsFrequency = "quarterly",
                ProfessionalCosts = 15,
                ProfessionalCostsFrequency = "monthly",
                SavingsContributions = 250,
                SavingsContributionsFrequency = "monthly",
                OtherExpenditure = 125,
                OtherExpenditureFrequency = "monthly"
            };

            MonthlyOutgoings expected = new MonthlyOutgoings()
            {
                Expenditures = 1332.48M,
                HouseholdBills = 2471.53M,
                Total = 3804.01M
            };

            MonthlyOutgoings result = this._calculatorService.CalculateMonthlyOutgoings(iAndE);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void CalculateDisposableIncomeTest() 
        {
            Assert.AreEqual(500, this._calculatorService.CalculateDisposableIncome(3000, 2500));
        }

        [TestMethod]
        public void CalculateMonthlyExpenditureTest() 
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure()
            {
                Housekeeping = 100,
                HousekeepingFrequency = "monthly",
                PersonalCosts = 50,
                PersonalCostsFrequency = "weekly",
                Leisure = 75,
                LeisureFrequency = "fortnightly",
                Travel = 30,
                TravelFrequency = "weekly",
                Healthcare = 1200,
                HealthcareFrequency = "annually",
                PensionInsurance = 100,
                PensionInsuranceFrequency = "quarterly",
                SchoolCosts = 600,
                SchoolCostsFrequency = "quarterly",
                ProfessionalCosts = 15,
                ProfessionalCostsFrequency = "monthly",
                SavingsContributions = 250,
                SavingsContributionsFrequency = "monthly",
                OtherExpenditure = 125,
                OtherExpenditureFrequency = "monthly"
            };

            Assert.AreEqual(1332.48M, this._calculatorService.CalculateMonthlyExpenditure(iAndE));
        }

        [TestMethod]
        public void CalculateMonthlyBillsAndOutgoingsTest() 
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure()
            {
                Mortgage = 500,
                MortgageFrequency = "monthly",
                MortgageArrears = 50,
                Rent = 600,
                RentFrequency = "monthly",
                RentArrears = 60,
                SecuredLoans = 100,
                SecuredLoansFrequency = "monthly",
                SecuredloansArrears = 10,
                CouncilTax = 200,
                CouncilTaxFrequency = "monthly",
                CouncilTaxArrears = 20,
                Rental = 50,
                RentalFrequency = "weekly",
                RentalArrears = 5,
                TvLicence = 10,
                TvLicenceFrequency = "fortnightly",
                TvLicenceArrears = 1,
                Gas = 50,
                GasFrequency = "monthly",
                GasArrears = 5,
                Electricity = 60,
                ElectricityFrequency = "monthly",
                ElectricityArrears = 6,
                OtherUtilities = 1200,
                OtherUtilitiesFrequency = "annually",
                OtherUtilitiesArrears = 120,
                Water = 100,
                WaterFrequency = "quarterly",
                WaterArrears = 10,
                ChildMaintenance = 150,
                ChildMaintenanceFrequency = "monthly",
                ChildMaintenanceArrears = 15,
                CourtFines = 25,
                CourtFinesFrequency = "monthly",
                CourtFinesArrears = 25,
                CCJs = 80,
                CCJsFrequency = "monthly",
                CCJsArrears = 8,
            };

            Assert.AreEqual(2136.53M, this._calculatorService.CalculateMonthlyBillsAndOutgoings(iAndE));
        }

        [TestMethod]
        public void ConvertToMonthlyTest() 
        {
            Assert.AreEqual(100, this._calculatorService.ConvertToMonthly(100, "m"));
            Assert.AreEqual(100, this._calculatorService.ConvertToMonthly(100, "M"));
            Assert.AreEqual(100, this._calculatorService.ConvertToMonthly(100, "monthly"));
            Assert.AreEqual(100, this._calculatorService.ConvertToMonthly(100, "Monthly"));
            Assert.AreEqual(433, this._calculatorService.ConvertToMonthly(100, "w"));
            Assert.AreEqual(433, this._calculatorService.ConvertToMonthly(100, "W"));
            Assert.AreEqual(433, this._calculatorService.ConvertToMonthly(100, "weekly"));
            Assert.AreEqual(433, this._calculatorService.ConvertToMonthly(100, "Weekly"));
            Assert.AreEqual(217, this._calculatorService.ConvertToMonthly(100, "f"));
            Assert.AreEqual(217, this._calculatorService.ConvertToMonthly(100, "F"));
            Assert.AreEqual(217, this._calculatorService.ConvertToMonthly(100, "fortnightly"));
            Assert.AreEqual(217, this._calculatorService.ConvertToMonthly(100, "Fortnightly"));
            Assert.AreEqual(108, this._calculatorService.ConvertToMonthly(100, "4"));
            Assert.AreEqual(108, this._calculatorService.ConvertToMonthly(100, "4week"));
            Assert.AreEqual(108, this._calculatorService.ConvertToMonthly(100, "4WEEK"));
            Assert.AreEqual(33.33M, this._calculatorService.ConvertToMonthly(100, "q"));
            Assert.AreEqual(33.33M, this._calculatorService.ConvertToMonthly(100, "Q"));
            Assert.AreEqual(33.33M, this._calculatorService.ConvertToMonthly(100, "quarterly"));
            Assert.AreEqual(33.33M, this._calculatorService.ConvertToMonthly(100, "Quarterly"));
            Assert.AreEqual(100, this._calculatorService.ConvertToMonthly(100, "l"));
            Assert.AreEqual(100, this._calculatorService.ConvertToMonthly(100, "L"));
            Assert.AreEqual(8.33M, this._calculatorService.ConvertToMonthly(100, "a"));
            Assert.AreEqual(8.33M, this._calculatorService.ConvertToMonthly(100, "A"));
            Assert.AreEqual(8.33M, this._calculatorService.ConvertToMonthly(100, "annually"));
            Assert.AreEqual(8.33M, this._calculatorService.ConvertToMonthly(100, "Annually"));
            Assert.AreEqual(0, this._calculatorService.ConvertToMonthly(100, ""));
            Assert.AreEqual(0, this._calculatorService.ConvertToMonthly(100, null));
            Assert.AreEqual(0, this._calculatorService.ConvertToMonthly(100, "testing"));
        }

        [TestMethod]
        public void InArrearsTest() 
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure();

            Assert.IsFalse(this._calculatorService.InArrears(iAndE));

            iAndE.RentalArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.RentalArrears = 0;
            iAndE.CCJsArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.CCJsArrears = 0;
            iAndE.ChildMaintenanceArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.ChildMaintenanceArrears = 0;
            iAndE.CouncilTaxArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.CouncilTaxArrears = 0;
            iAndE.CourtFinesArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.CourtFinesArrears = 0;
            iAndE.ElectricityArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.ElectricityArrears = 0;
            iAndE.GasArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.GasArrears = 0;
            iAndE.MortgageArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.MortgageArrears = 0;
            iAndE.OtherUtilitiesArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.OtherUtilitiesArrears = 0;
            iAndE.RentalArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.RentalArrears = 0;
            iAndE.SecuredloansArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.SecuredloansArrears = 0;
            iAndE.TvLicenceArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));

            iAndE.TvLicenceArrears = 0;
            iAndE.WaterArrears = 100;

            Assert.IsTrue(this._calculatorService.InArrears(iAndE));
        }

    }
}
