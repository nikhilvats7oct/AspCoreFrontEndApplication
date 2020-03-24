using AutoMapper;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class IncomeAndExpenditureServiceToApiConverterTests
    {
        private Mock<IMapperHelper> _mapperHelper;
        private Mock<ICalculatorService> _calculatorService;

        private IncomeAndExpenditureServiceToApiConverter _converter;

        [TestInitialize]
        public void TestInitialise()
        {
            this._mapperHelper = new Mock<IMapperHelper>(MockBehavior.Strict);
            this._calculatorService = new Mock<ICalculatorService>(MockBehavior.Strict);

            this._converter = new IncomeAndExpenditureServiceToApiConverter(
                this._mapperHelper.Object, this._calculatorService.Object);
        }

        [TestMethod]
        public void ConvertTest_SourceNull()
        {
            IncomeAndExpenditure source = null;
            IncomeAndExpenditureApiModel destination = new IncomeAndExpenditureApiModel();
            IncomeAndExpenditureApiModel destinationCopy = Utilities.DeepCopy(destination);
            IncomeAndExpenditureApiModel expected = null;

            IncomeAndExpenditureApiModel result = _converter.Convert(source, destination, null);

            Assert.AreEqual(expected, result);

            //Check that destination has not been modified
            Assert.IsTrue(Utilities.DeepCompare(destination, destinationCopy));
        }

        [TestMethod]
        public void ConvertTest()
        {
            IncomeAndExpenditure source = new IncomeAndExpenditure()
            {
                AdultsInHousehold = 1,
                Children16to18 = 2,
                ChildrenUnder16 = 3,
                Created = DateTime.Now.Date,
                CouncilTax = 100,
                CouncilTaxArrears = 110,
                CouncilTaxFrequency = "monthly",
                ChildMaintenance = 120,
                ChildMaintenanceArrears = 130,
                ChildMaintenanceFrequency = "fortnightly",
                HomeContents = 140,
                HomeContentsArrears = 150,
                HomeContentsFrequency = "weekly",
                HasArrears = true,
                EmploymentStatus = "employed full time",
                Healthcare = 160,
                HealthcareFrequency = "monthly",
                Housekeeping = 170,
                HousekeepingFrequency = "fortnightly",
                HousingStatus = "homeowner",
                Leisure = 180,
                LeisureFrequency = "weekly",
                LowellReference = "123456789",
                Mortgage = 190,
                MortgageArrears = 200,
                MortgageFrequency = "monthly",
                OtherUtilities = 210,
                OtherUtilitiesArrears = 220,
                OtherUtilitiesFrequency = "fortnightly",
                OtherIncome = 230,
                OtherincomeFrequency = "weekly",
                PersonalCosts = 240,
                PersonalCostsFrequency = "fortnightly",
                Pension = 250,
                PensionFrequency = "weekly",
                PensionInsurance = 260,
                PensionInsuranceFrequency = "monthly",
                Rental = 270,
                RentalArrears = 280,
                RentalFrequency = "fortnightly",
                Rent = 290,
                RentArrears = 300,
                RentFrequency = "weekly",
                Salary = 310,
                SalaryFrequency = "monthly",
                SavingsContributions = 320,
                SavingsContributionsFrequency = "fortnightly",
                SchoolCosts = 330,
                SchoolCostsFrequency = "weekly",
                ProfessionalCosts = 340,
                ProfessionalCostsFrequency = "monthly",
                SecuredLoans = 350,
                SecuredloansArrears = 360,
                SecuredLoansFrequency = "fortnightly",
                Travel = 370,
                TravelFrequency = "weekly",
                TvLicence = 380,
                TvLicenceArrears = 390,
                TvLicenceFrequency = "monthly",
                User = "webuser",
                OtherDebts = new List<SaveOtherDebts>() { },
                Electricity = 400,
                ElectricityArrears = 410,
                ElectricityFrequency = "fortnightly",
                Gas = 420,
                GasArrears = 430,
                GasFrequency = "weekly",
                Water = 440,
                WaterArrears = 450,
                WaterFrequency = "monthly",
                BenefitsTotal = 460,
                BenefitsTotalFrequency = "fortnightly",
                EarningsTotal = 470,
                EarningsTotalFrequency = "weekly",
                ExpenditureTotal = 480,
                IncomeTotal = 490,
                DisposableIncome = 500,
                UtilitiesTotal = 510,
                UtilitiesTotalArrears = 520,
                UtilitiesTotalFrequency = "monthly",
                CCJs = 530,
                CCJsArrears = 540,
                CCJsFrequency = "fortnightly",
                CourtFines = 550,
                CourtFinesArrears = 560,
                CourtFinesFrequency = "weekly",
                OtherExpenditure = 570,
                OtherExpenditureFrequency = "monthly"
            };

            //Create a copy of source for later
            IncomeAndExpenditure sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditureApiModel destination = new IncomeAndExpenditureApiModel();
            IncomeAndExpenditureApiModel expected = Utilities.DeepCopy(destination);

            this._calculatorService.Setup(x => x.InArrears(source)).Returns(true);

            expected.LowellReference = "123456789";
            expected.User = "webuser";
            expected.Created = DateTime.UtcNow;
            expected.HasArrears = true;

            expected.AdultsInHousehold = 1;
            expected.ChildrenUnder16 = 3;
            expected.Children16to18 = 2;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.Salary = 310;
            expected.SalaryFrequency = "M";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Pension = 250;
            expected.PensionFrequency = "W";

            expected.EarningsTotal = 0.00M;
            expected.EarningsTotalFrequency = "";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.BenefitsTotal = 460;
            expected.BenefitsTotalFrequency = "F";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.OtherIncome = 230;
            expected.OtherincomeFrequency = "W";
            
            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.Mortgage = 190;
            expected.MortgageFrequency = "M";
            expected.MortgageArrears = 200;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Rent = 290; ;
            expected.RentFrequency = "W";
            expected.RentArrears = 300;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.SecuredLoans = 350;
            expected.SecuredLoansFrequency = "F";
            expected.SecuredloansArrears = 360;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.CouncilTax = 100;
            expected.CouncilTaxFrequency = "M";
            expected.CouncilTaxArrears = 110;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.Rental = 270;
            expected.RentalFrequency = "F";
            expected.RentalArrears = 280;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.TvLicence = 380;
            expected.TvLicenceFrequency = "M";
            expected.TvLicenceArrears = 390;

            expected.HomeContents = 0.00M;
            expected.HomeContentsFrequency = "";
            expected.HomeContentsArrears = 150.00M;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Gas = 420;
            expected.GasFrequency = "W";
            expected.GasArrears = 430;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.OtherUtilities = 210;
            expected.OtherUtilitiesFrequency = "F";
            expected.OtherUtilitiesArrears = 220;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.Electricity = 400;
            expected.ElectricityFrequency = "F";
            expected.ElectricityArrears = 410;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.Water = 440;
            expected.WaterFrequency = "M";
            expected.WaterArrears = 450;

            expected.UtilitiesTotal = 0.00M;
            expected.UtilitiesTotalFrequency = "";
            expected.UtilitiesTotalArrears = 520.00M;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.ChildMaintenance = 120;
            expected.ChildMaintenanceFrequency = "F";
            expected.ChildMaintenanceArrears = 130;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.Housekeeping = 170;
            expected.HousekeepingFrequency = "F";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.PersonalCosts = 240;
            expected.PersonalCostsFrequency = "F";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Leisure = 180;
            expected.LeisureFrequency = "W";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Travel = 370;
            expected.TravelFrequency = "W";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.Healthcare = 160;
            expected.HealthcareFrequency = "M";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.PensionInsurance = 260;
            expected.PensionInsuranceFrequency = "M";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.SchoolCosts = 330;
            expected.SchoolCostsFrequency = "W";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.ProfessionalCosts = 340;
            expected.ProfessionalCostsFrequency = "M";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.SavingsContributions = 320;
            expected.SavingsContributionsFrequency = "F";

            List<SaveOtherDebtsApiModel> otherDebts = new List<SaveOtherDebtsApiModel>()
            {
                new SaveOtherDebtsApiModel(){ Amount = 570, Arrears = 580, Frequency = "M", CountyCourtJudgement = true }
            };

            _mapperHelper.Setup(x => x.CreateOtherDebts(source)).Returns(otherDebts);
            expected.OtherDebts = otherDebts;

            MonthlyIncome monthlyIncome = new MonthlyIncome() { Total = 590 };
            this._calculatorService.Setup(x => x.CalculateMonthlyIncome(source)).Returns(monthlyIncome);

            MonthlyOutgoings monthlyOutgoings = new MonthlyOutgoings() { Total = 600 };
            this._calculatorService.Setup(x => x.CalculateMonthlyOutgoings(source)).Returns(monthlyOutgoings);

            this._mapperHelper.Setup(x => x.ConvertToHousingStatusCaseflow("homeowner")).Returns("Owner");
            this._mapperHelper.Setup(x => x.ConvertToEmploymentStatusCaseflow("employed full time")).Returns("Full-Time");

            expected.IncomeTotal = 590;
            expected.ExpenditureTotal = 600;
            expected.DisposableIncome = -10;
            expected.HousingStatus = "Owner";
            expected.EmploymentStatus = "Full-Time";

            IncomeAndExpenditureApiModel result = _converter.Convert(source, destination, null);

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));

            //Check that result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result, 1000));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            IncomeAndExpenditure source = new IncomeAndExpenditure()
            {
                AdultsInHousehold = 1,
                Children16to18 = 2,
                ChildrenUnder16 = 3,
                Created = DateTime.Now.Date,
                CouncilTax = 100,
                CouncilTaxArrears = 110,
                CouncilTaxFrequency = "monthly",
                ChildMaintenance = 120,
                ChildMaintenanceArrears = 130,
                ChildMaintenanceFrequency = "fortnightly",
                HomeContents = 140,
                HomeContentsArrears = 150,
                HomeContentsFrequency = "weekly",
                HasArrears = true,
                EmploymentStatus = "employed full time",
                Healthcare = 160,
                HealthcareFrequency = "monthly",
                Housekeeping = 170,
                HousekeepingFrequency = "fortnightly",
                HousingStatus = "homeowner",
                Leisure = 180,
                LeisureFrequency = "weekly",
                LowellReference = "123456789",
                Mortgage = 190,
                MortgageArrears = 200,
                MortgageFrequency = "monthly",
                OtherUtilities = 210,
                OtherUtilitiesArrears = 220,
                OtherUtilitiesFrequency = "fortnightly",
                OtherIncome = 230,
                OtherincomeFrequency = "weekly",
                PersonalCosts = 240,
                PersonalCostsFrequency = "fortnightly",
                Pension = 250,
                PensionFrequency = "weekly",
                PensionInsurance = 260,
                PensionInsuranceFrequency = "monthly",
                Rental = 270,
                RentalArrears = 280,
                RentalFrequency = "fortnightly",
                Rent = 290,
                RentArrears = 300,
                RentFrequency = "weekly",
                Salary = 310,
                SalaryFrequency = "monthly",
                SavingsContributions = 320,
                SavingsContributionsFrequency = "fortnightly",
                SchoolCosts = 330,
                SchoolCostsFrequency = "weekly",
                ProfessionalCosts = 340,
                ProfessionalCostsFrequency = "monthly",
                SecuredLoans = 350,
                SecuredloansArrears = 360,
                SecuredLoansFrequency = "fortnightly",
                Travel = 370,
                TravelFrequency = "weekly",
                TvLicence = 380,
                TvLicenceArrears = 390,
                TvLicenceFrequency = "monthly",
                User = "webuser",
                OtherDebts = new List<SaveOtherDebts>() { },
                Electricity = 400,
                ElectricityArrears = 410,
                ElectricityFrequency = "fortnightly",
                Gas = 420,
                GasArrears = 430,
                GasFrequency = "weekly",
                Water = 440,
                WaterArrears = 450,
                WaterFrequency = "monthly",
                BenefitsTotal = 460,
                BenefitsTotalFrequency = "fortnightly",
                EarningsTotal = 470,
                EarningsTotalFrequency = "weekly",
                ExpenditureTotal = 480,
                IncomeTotal = 490,
                DisposableIncome = 500,
                UtilitiesTotal = 510,
                UtilitiesTotalArrears = 520,
                UtilitiesTotalFrequency = "monthly",
                CCJs = 530,
                CCJsArrears = 540,
                CCJsFrequency = "fortnightly",
                CourtFines = 550,
                CourtFinesArrears = 560,
                CourtFinesFrequency = "weekly",
                OtherExpenditure = 570,
                OtherExpenditureFrequency = "monthly"
            };

            //Create a copy of source for later
            IncomeAndExpenditure sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditureApiModel destination = null;
            IncomeAndExpenditureApiModel expected = new IncomeAndExpenditureApiModel();

            this._calculatorService.Setup(x => x.InArrears(source)).Returns(true);

            expected.LowellReference = "123456789";
            expected.User = "webuser";
            expected.Created = DateTime.UtcNow;
            expected.HasArrears = true;

            expected.AdultsInHousehold = 1;
            expected.ChildrenUnder16 = 3;
            expected.Children16to18 = 2;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.Salary = 310;
            expected.SalaryFrequency = "M";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Pension = 250;
            expected.PensionFrequency = "W";

            expected.EarningsTotal = 0.00M;
            expected.EarningsTotalFrequency = "";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.BenefitsTotal = 460;
            expected.BenefitsTotalFrequency = "F";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.OtherIncome = 230;
            expected.OtherincomeFrequency = "W";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.Mortgage = 190;
            expected.MortgageFrequency = "M";
            expected.MortgageArrears = 200;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Rent = 290; ;
            expected.RentFrequency = "W";
            expected.RentArrears = 300;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.SecuredLoans = 350;
            expected.SecuredLoansFrequency = "F";
            expected.SecuredloansArrears = 360;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.CouncilTax = 100;
            expected.CouncilTaxFrequency = "M";
            expected.CouncilTaxArrears = 110;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.Rental = 270;
            expected.RentalFrequency = "F";
            expected.RentalArrears = 280;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.TvLicence = 380;
            expected.TvLicenceFrequency = "M";
            expected.TvLicenceArrears = 390;

            expected.HomeContents = 0.00M;
            expected.HomeContentsFrequency = "";
            expected.HomeContentsArrears = 150.00M;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Gas = 420;
            expected.GasFrequency = "W";
            expected.GasArrears = 430;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.OtherUtilities = 210;
            expected.OtherUtilitiesFrequency = "F";
            expected.OtherUtilitiesArrears = 220;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.Electricity = 400;
            expected.ElectricityFrequency = "F";
            expected.ElectricityArrears = 410;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.Water = 440;
            expected.WaterFrequency = "M";
            expected.WaterArrears = 450;

            expected.UtilitiesTotal = 0.00M;
            expected.UtilitiesTotalFrequency = "";
            expected.UtilitiesTotalArrears = 520.00M;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.ChildMaintenance = 120;
            expected.ChildMaintenanceFrequency = "F";
            expected.ChildMaintenanceArrears = 130;

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.Housekeeping = 170;
            expected.HousekeepingFrequency = "F";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.PersonalCosts = 240;
            expected.PersonalCostsFrequency = "F";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Leisure = 180;
            expected.LeisureFrequency = "W";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.Travel = 370;
            expected.TravelFrequency = "W";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.Healthcare = 160;
            expected.HealthcareFrequency = "M";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.PensionInsurance = 260;
            expected.PensionInsuranceFrequency = "M";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("weekly")).Returns("W");
            expected.SchoolCosts = 330;
            expected.SchoolCostsFrequency = "W";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("monthly")).Returns("M");
            expected.ProfessionalCosts = 340;
            expected.ProfessionalCostsFrequency = "M";

            this._mapperHelper.Setup(x => x.ConvertFrequencyToInitial("fortnightly")).Returns("F");
            expected.SavingsContributions = 320;
            expected.SavingsContributionsFrequency = "F";

            List<SaveOtherDebtsApiModel> otherDebts = new List<SaveOtherDebtsApiModel>()
            {
                new SaveOtherDebtsApiModel(){ Amount = 570, Arrears = 580, Frequency = "M", CountyCourtJudgement = true }
            };

            _mapperHelper.Setup(x => x.CreateOtherDebts(source)).Returns(otherDebts);
            expected.OtherDebts = otherDebts;

            MonthlyIncome monthlyIncome = new MonthlyIncome() { Total = 590 };
            this._calculatorService.Setup(x => x.CalculateMonthlyIncome(source)).Returns(monthlyIncome);

            MonthlyOutgoings monthlyOutgoings = new MonthlyOutgoings() { Total = 600 };
            this._calculatorService.Setup(x => x.CalculateMonthlyOutgoings(source)).Returns(monthlyOutgoings);

            this._mapperHelper.Setup(x => x.ConvertToHousingStatusCaseflow("homeowner")).Returns("Owner");
            this._mapperHelper.Setup(x => x.ConvertToEmploymentStatusCaseflow("employed full time")).Returns("Full-Time");

            expected.IncomeTotal = 590;
            expected.ExpenditureTotal = 600;
            expected.DisposableIncome = -10;
            expected.HousingStatus = "Owner";
            expected.EmploymentStatus = "Full-Time";

            IncomeAndExpenditureApiModel result = _converter.Convert(source, destination, null);

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));

            //Check that result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result, 1000));
        }
    }
}
