using AutoMapper;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class IncomeAndExpenditureToBillsAndOutgoingsVmConverterTests
    {
        private IncomeAndExpenditureToBillsAndOutgoingsVmConverter _converter;
        private Mock<ICalculatorService> _calculatorService;
        private Mock<IMapper> _mapper;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._calculatorService = new Mock<ICalculatorService>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);

            this._converter = new IncomeAndExpenditureToBillsAndOutgoingsVmConverter(
                this._calculatorService.Object, this._mapper.Object);
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            IncomeAndExpenditure source = null;
            BillsAndOutgoingsVm destination = Utilities.CreateDefaultTestBillsAndOutgoingsVm();
            BillsAndOutgoingsVm expected = Utilities.DeepCopy(destination);

            MonthlyIncome monthlyIncome = new MonthlyIncome();
            MonthlyIncomeVm monthlyIncomeVm = new MonthlyIncomeVm()
            {
                Benefits = 100,
                Other = 200,
                Salary = 300,
                Pension = 400,
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

            expected.ApplianceOrFurnitureRental.Amount = 0;
            expected.ApplianceOrFurnitureRental.ArrearsAmount = 0;
            expected.ApplianceOrFurnitureRental.Frequency = null;
            expected.ApplianceOrFurnitureRental.InArrears = false;

            expected.Ccjs.Amount = 0;
            expected.Ccjs.ArrearsAmount = 0;
            expected.Ccjs.Frequency = null;
            expected.Ccjs.InArrears = false;

            expected.ChildMaintenance.Amount = 0;
            expected.ChildMaintenance.ArrearsAmount = 0;
            expected.ChildMaintenance.Frequency = null;
            expected.ChildMaintenance.InArrears = false;

            expected.CouncilTax.Amount = 0;
            expected.CouncilTax.ArrearsAmount = 0;
            expected.CouncilTax.Frequency = null;
            expected.CouncilTax.InArrears = false;

            expected.CourtFines.Amount = 0;
            expected.CourtFines.ArrearsAmount = 0;
            expected.CourtFines.Frequency = null;
            expected.CourtFines.InArrears = false;

            expected.Electric.Amount = 0;
            expected.Electric.ArrearsAmount = 0;
            expected.Electric.Frequency = null;
            expected.Electric.InArrears = false;

            expected.Gas.Amount = 0;
            expected.Gas.ArrearsAmount = 0;
            expected.Gas.Frequency = null;
            expected.Gas.InArrears = false;

            expected.IncomeSummary = monthlyIncomeVm;
            expected.OutgoingSummary = monthlyOutgoingsVm;

            expected.Mortgage.Amount = 0;
            expected.Mortgage.ArrearsAmount = 0;
            expected.Mortgage.Frequency = null;
            expected.Mortgage.InArrears = false;

            expected.OtherFuel.Amount = 0;
            expected.OtherFuel.ArrearsAmount = 0;
            expected.OtherFuel.Frequency = null;
            expected.OtherFuel.InArrears = false;

            expected.Rent.Amount = 0;
            expected.Rent.ArrearsAmount = 0;
            expected.Rent.Frequency = null;
            expected.Rent.InArrears = false;

            expected.SecuredLoan.Amount = 0;
            expected.SecuredLoan.ArrearsAmount = 0;
            expected.SecuredLoan.Frequency = null;
            expected.SecuredLoan.InArrears = false;

            expected.TvLicense.Amount = 0;
            expected.TvLicense.ArrearsAmount = 0;
            expected.TvLicense.Frequency = null;
            expected.TvLicense.InArrears = false;

            expected.Water.Amount = 0;
            expected.Water.ArrearsAmount = 0;
            expected.Water.Frequency = null;
            expected.Water.InArrears = false;

            BillsAndOutgoingsVm result = _converter.Convert(source, destination, null);

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
                Expenditures = 123,
                HouseholdBills = 456,
                Total = 789
            };

            BillsAndOutgoingsVm destination = Utilities.CreateDefaultTestBillsAndOutgoingsVm();
            BillsAndOutgoingsVm expected = Utilities.DeepCopy(destination);

            expected.Mortgage = new OutgoingSourceVm() { Amount = 100, ArrearsAmount = 10, Frequency = "monthly", InArrears = true };
            expected.ApplianceOrFurnitureRental = new OutgoingSourceVm() { Amount = 150, ArrearsAmount = 0, Frequency = "weekly", InArrears = false };
            expected.Electric = new OutgoingSourceVm() { Amount = 200, ArrearsAmount = 20, Frequency = "fortnightly", InArrears = true };
            expected.Ccjs = new OutgoingSourceVm() { Amount = 250, ArrearsAmount = 0, Frequency = "monthly", InArrears = false };
            expected.ChildMaintenance = new OutgoingSourceVm() { Amount = 300, ArrearsAmount = 30, Frequency = "weekly", InArrears = true };
            expected.CouncilTax = new OutgoingSourceVm() { Amount = 350, ArrearsAmount = 0, Frequency = "fortnightly", InArrears = false };
            expected.CourtFines = new OutgoingSourceVm() { Amount = 400, ArrearsAmount = 40, Frequency = "monthly", InArrears = true };
            expected.Gas = new OutgoingSourceVm() { Amount = 450, ArrearsAmount = 0, Frequency = "weekly", InArrears = false };
            expected.OtherFuel = new OutgoingSourceVm() { Amount = 500, ArrearsAmount = 50, Frequency = "fortnightly", InArrears = true };
            expected.Rent = new OutgoingSourceVm() { Amount = 550, ArrearsAmount = 0, Frequency = "monthly", InArrears = false };
            expected.SecuredLoan = new OutgoingSourceVm() { Amount = 600, ArrearsAmount = 60, Frequency = "weekly", InArrears = true };
            expected.TvLicense = new OutgoingSourceVm() { Amount = 650, ArrearsAmount = 0, Frequency = "fortnightly", InArrears = false };
            expected.Water = new OutgoingSourceVm() { Amount = 700, ArrearsAmount = 70, Frequency = "monthly", InArrears = true };
            expected.IncomeSummary = monthlyIncomeVm;
            expected.OutgoingSummary = monthlyOutgoingsVm;

            _calculatorService.Setup(x => x.CalculateMonthlyIncome(source)).Returns(monthlyIncome);
            _calculatorService.Setup(x => x.CalculateMonthlyOutgoings(source)).Returns(monthlyOutgoings);
            _mapper.Setup(x => x.Map<MonthlyIncomeVm>(monthlyIncome)).Returns(monthlyIncomeVm);
            _mapper.Setup(x => x.Map<MonthlyOutgoingsVm>(monthlyOutgoings)).Returns(monthlyOutgoingsVm);

            BillsAndOutgoingsVm result = _converter.Convert(source, destination, null);

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
                Expenditures = 123,
                HouseholdBills = 456,
                Total = 789
            };

            BillsAndOutgoingsVm destination = null;
            BillsAndOutgoingsVm expected = new BillsAndOutgoingsVm();

            expected.Mortgage = new OutgoingSourceVm() { Amount = 100, ArrearsAmount = 10, Frequency = "monthly", InArrears = true };
            expected.ApplianceOrFurnitureRental = new OutgoingSourceVm() { Amount = 150, ArrearsAmount = 0, Frequency = "weekly", InArrears = false };
            expected.Electric = new OutgoingSourceVm() { Amount = 200, ArrearsAmount = 20, Frequency = "fortnightly", InArrears = true };
            expected.Ccjs = new OutgoingSourceVm() { Amount = 250, ArrearsAmount = 0, Frequency = "monthly", InArrears = false };
            expected.ChildMaintenance = new OutgoingSourceVm() { Amount = 300, ArrearsAmount = 30, Frequency = "weekly", InArrears = true };
            expected.CouncilTax = new OutgoingSourceVm() { Amount = 350, ArrearsAmount = 0, Frequency = "fortnightly", InArrears = false };
            expected.CourtFines = new OutgoingSourceVm() { Amount = 400, ArrearsAmount = 40, Frequency = "monthly", InArrears = true };
            expected.Gas = new OutgoingSourceVm() { Amount = 450, ArrearsAmount = 0, Frequency = "weekly", InArrears = false };
            expected.OtherFuel = new OutgoingSourceVm() { Amount = 500, ArrearsAmount = 50, Frequency = "fortnightly", InArrears = true };
            expected.Rent = new OutgoingSourceVm() { Amount = 550, ArrearsAmount = 0, Frequency = "monthly", InArrears = false };
            expected.SecuredLoan = new OutgoingSourceVm() { Amount = 600, ArrearsAmount = 60, Frequency = "weekly", InArrears = true };
            expected.TvLicense = new OutgoingSourceVm() { Amount = 650, ArrearsAmount = 0, Frequency = "fortnightly", InArrears = false };
            expected.Water = new OutgoingSourceVm() { Amount = 700, ArrearsAmount = 70, Frequency = "monthly", InArrears = true };
            expected.IncomeSummary = monthlyIncomeVm;
            expected.OutgoingSummary = monthlyOutgoingsVm;

            _calculatorService.Setup(x => x.CalculateMonthlyIncome(source)).Returns(monthlyIncome);
            _calculatorService.Setup(x => x.CalculateMonthlyOutgoings(source)).Returns(monthlyOutgoings);
            _mapper.Setup(x => x.Map<MonthlyIncomeVm>(monthlyIncome)).Returns(monthlyIncomeVm);
            _mapper.Setup(x => x.Map<MonthlyOutgoingsVm>(monthlyOutgoings)).Returns(monthlyOutgoingsVm);

            BillsAndOutgoingsVm result = _converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

    }
}
