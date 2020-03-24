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

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class IncomeAndExpenditureToExpendituresVmConverterTests
    {
        private Mock<ICalculatorService> _calculatorService;
        private Mock<IMapper> _mapper;

        private IncomeAndExpenditureToExpendituresVmConverter _converter;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._calculatorService = new Mock<ICalculatorService>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);

            this._converter = new IncomeAndExpenditureToExpendituresVmConverter(
                this._calculatorService.Object, this._mapper.Object);
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            IncomeAndExpenditure source = null;
            ExpendituresVm destination = Utilities.CreateDefaultTestExpendituresVm();
            ExpendituresVm expected = Utilities.DeepCopy(destination);

            MonthlyIncome monthlyIncome = new MonthlyIncome();
            MonthlyIncomeVm monthlyIncomeVm = new MonthlyIncomeVm() 
            {
                Benefits = 100,
                Other = 200,
                Pension = 300, 
                Salary = 400,
                Total = 1000
            };

            MonthlyOutgoings monthlyOutgoings = new MonthlyOutgoings();
            MonthlyOutgoingsVm monthlyOutgoingsVm = new MonthlyOutgoingsVm() 
            {
                Expenditures = 1000,
                HouseholdBills = 2000,
                Total = 3000
            };

            _calculatorService.Setup(x => x.CalculateMonthlyIncome(It.IsAny<IncomeAndExpenditure>()))
                .Returns(monthlyIncome);
            _mapper.Setup(x => x.Map<MonthlyIncomeVm>(monthlyIncome)).Returns(monthlyIncomeVm);
            _calculatorService.Setup(x => x.CalculateMonthlyOutgoings(It.IsAny<IncomeAndExpenditure>()))
                .Returns(monthlyOutgoings);
            _mapper.Setup(x => x.Map<MonthlyOutgoingsVm>(monthlyOutgoings)).Returns(monthlyOutgoingsVm);

            expected.CareAndHealthCosts.Amount = 0;
            expected.CareAndHealthCosts.Frequency = null;
            expected.CommunicationsAndLeisure.Amount = 0;
            expected.CommunicationsAndLeisure.Frequency = null;
            expected.FoodAndHouseKeeping.Amount = 0;
            expected.FoodAndHouseKeeping.Frequency = null;
            expected.IncomeVmSummary = monthlyIncomeVm;
            expected.Other.Amount = 0;
            expected.Other.Frequency = null;
            expected.OutgoingsVmSummary = monthlyOutgoingsVm;
            expected.PensionsAndInsurance.Amount = 0;
            expected.PensionsAndInsurance.Frequency = null;
            expected.PersonalCosts.Amount = 0;
            expected.PersonalCosts.Frequency = null;
            expected.Professional.Amount = 0;
            expected.Professional.Frequency = null;
            expected.Savings.Amount = 0;
            expected.Savings.Frequency = null;
            expected.SchoolCosts.Amount = 0;
            expected.SchoolCosts.Frequency = null;
            expected.TravelAndTransport.Amount = 0;
            expected.TravelAndTransport.Frequency = null;

            ExpendituresVm result = _converter.Convert(source, destination, null);

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

            ExpendituresVm destination = Utilities.CreateDefaultTestExpendituresVm();
            ExpendituresVm expected = Utilities.DeepCopy(destination);

            MonthlyIncome monthlyIncome = new MonthlyIncome();
            MonthlyIncomeVm monthlyIncomeVm = new MonthlyIncomeVm()
            {
                Benefits = 100,
                Other = 200,
                Pension = 300,
                Salary = 400,
                Total = 1000
            };

            MonthlyOutgoings monthlyOutgoings = new MonthlyOutgoings();
            MonthlyOutgoingsVm monthlyOutgoingsVm = new MonthlyOutgoingsVm()
            {
                Expenditures = 1000,
                HouseholdBills = 2000,
                Total = 3000
            };

            _calculatorService.Setup(x => x.CalculateMonthlyIncome(source))
                .Returns(monthlyIncome);
            _mapper.Setup(x => x.Map<MonthlyIncomeVm>(monthlyIncome)).Returns(monthlyIncomeVm);
            _calculatorService.Setup(x => x.CalculateMonthlyOutgoings(source))
                .Returns(monthlyOutgoings);
            _mapper.Setup(x => x.Map<MonthlyOutgoingsVm>(monthlyOutgoings)).Returns(monthlyOutgoingsVm);

            expected.CareAndHealthCosts.Amount = 800;
            expected.CareAndHealthCosts.Frequency = "fortnightly";
            expected.CommunicationsAndLeisure.Amount = 50;
            expected.CommunicationsAndLeisure.Frequency = "weekly";
            expected.FoodAndHouseKeeping.Amount = 900;
            expected.FoodAndHouseKeeping.Frequency = "monthly";
            expected.IncomeVmSummary = monthlyIncomeVm;
            expected.Other.Amount = 60;
            expected.Other.Frequency = "fortnightly";
            expected.OutgoingsVmSummary = monthlyOutgoingsVm;
            expected.PensionsAndInsurance.Amount = 75;
            expected.PensionsAndInsurance.Frequency = "monthly";
            expected.PersonalCosts.Amount = 225;
            expected.PersonalCosts.Frequency = "monthly";
            expected.Professional.Amount = 123;
            expected.Professional.Frequency = "monthly";
            expected.Savings.Amount = 55;
            expected.Savings.Frequency = "weekly";
            expected.SchoolCosts.Amount = 5;
            expected.SchoolCosts.Frequency = "weekly";
            expected.TravelAndTransport.Amount = 45;
            expected.TravelAndTransport.Frequency = "fortnightly";

            ExpendituresVm result = _converter.Convert(source, destination, null);

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

            ExpendituresVm destination = null;
            ExpendituresVm expected = new ExpendituresVm();

            MonthlyIncome monthlyIncome = new MonthlyIncome();
            MonthlyIncomeVm monthlyIncomeVm = new MonthlyIncomeVm()
            {
                Benefits = 100,
                Other = 200,
                Pension = 300,
                Salary = 400,
                Total = 1000
            };

            MonthlyOutgoings monthlyOutgoings = new MonthlyOutgoings();
            MonthlyOutgoingsVm monthlyOutgoingsVm = new MonthlyOutgoingsVm()
            {
                Expenditures = 1000,
                HouseholdBills = 2000,
                Total = 3000
            };

            _calculatorService.Setup(x => x.CalculateMonthlyIncome(source))
                .Returns(monthlyIncome);
            _mapper.Setup(x => x.Map<MonthlyIncomeVm>(monthlyIncome)).Returns(monthlyIncomeVm);
            _calculatorService.Setup(x => x.CalculateMonthlyOutgoings(source))
                .Returns(monthlyOutgoings);
            _mapper.Setup(x => x.Map<MonthlyOutgoingsVm>(monthlyOutgoings)).Returns(monthlyOutgoingsVm);

            expected.CareAndHealthCosts.Amount = 800;
            expected.CareAndHealthCosts.Frequency = "fortnightly";
            expected.CommunicationsAndLeisure.Amount = 50;
            expected.CommunicationsAndLeisure.Frequency = "weekly";
            expected.FoodAndHouseKeeping.Amount = 900;
            expected.FoodAndHouseKeeping.Frequency = "monthly";
            expected.IncomeVmSummary = monthlyIncomeVm;
            expected.Other.Amount = 60;
            expected.Other.Frequency = "fortnightly";
            expected.OutgoingsVmSummary = monthlyOutgoingsVm;
            expected.PensionsAndInsurance.Amount = 75;
            expected.PensionsAndInsurance.Frequency = "monthly";
            expected.PersonalCosts.Amount = 225;
            expected.PersonalCosts.Frequency = "monthly";
            expected.Professional.Amount = 123;
            expected.Professional.Frequency = "monthly";
            expected.Savings.Amount = 55;
            expected.Savings.Frequency = "weekly";
            expected.SchoolCosts.Amount = 5;
            expected.SchoolCosts.Frequency = "weekly";
            expected.TravelAndTransport.Amount = 45;
            expected.TravelAndTransport.Frequency = "fortnightly";

            ExpendituresVm result = _converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }
    }
}
