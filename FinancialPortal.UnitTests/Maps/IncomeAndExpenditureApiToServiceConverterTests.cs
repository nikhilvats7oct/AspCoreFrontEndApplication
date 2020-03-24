using AutoMapper;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class IncomeAndExpenditureApiToServiceConverterTests
    {
        private Mock<IMapperHelper> _mapperHelper;
        private Mock<IMapper> _mapper;

        private IncomeAndExpenditureApiToServiceConverter _converter;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._mapperHelper = new Mock<IMapperHelper>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>();

            this._converter = new IncomeAndExpenditureApiToServiceConverter(
                this._mapperHelper.Object, this._mapper.Object);
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            IncomeAndExpenditureApiModel source = null;
            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();
            IncomeAndExpenditure destinationCopy = Utilities.DeepCopy(destination);
            IncomeAndExpenditure expected = null;

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            Assert.AreEqual(expected, result);

            //Check that destination has not been modified
            Assert.IsTrue(Utilities.DeepCompare(destination, destinationCopy));
        }

        [TestMethod]
        public void ConvertTest()
        {
            IncomeAndExpenditureApiModel source = new IncomeAndExpenditureApiModel()
            {
                AdultsInHousehold = 1,
                Children16to18 = 2,
                ChildrenUnder16 = 3,
                Created = DateTime.Now.Date,
                CouncilTax = 100,
                CouncilTaxArrears = 110,
                CouncilTaxFrequency = "M",
                ChildMaintenance = 120,
                ChildMaintenanceArrears = 130,
                ChildMaintenanceFrequency = "F",
                HomeContents = 140,
                HomeContentsArrears = 150,
                HomeContentsFrequency = "W",
                HasArrears = true,
                EmploymentStatus = "full-time",
                Healthcare = 160,
                HealthcareFrequency = "M",
                Housekeeping = 170,
                HousekeepingFrequency = "F",
                HousingStatus = "owner",
                Leisure = 180,
                LeisureFrequency = "W",
                LowellReference = "123456789",
                Mortgage = 190,
                MortgageArrears = 200,
                MortgageFrequency = "M",
                OtherUtilities = 210,
                OtherUtilitiesArrears = 220,
                OtherUtilitiesFrequency = "F",
                OtherIncome = 230,
                OtherincomeFrequency = "W",
                PersonalCosts = 240,
                PersonalCostsFrequency = "F",
                Pension = 250,
                PensionFrequency = "W",
                PensionInsurance = 260,
                PensionInsuranceFrequency = "M",
                Rental = 270,
                RentalArrears = 280,
                RentalFrequency = "F",
                Rent = 290,
                RentArrears = 300,
                RentFrequency = "W",
                Salary = 310,
                SalaryFrequency = "M",
                SavingsContributions = 320,
                SavingsContributionsFrequency = "F",
                SchoolCosts = 330,
                SchoolCostsFrequency = "W",
                ProfessionalCosts = 340,
                ProfessionalCostsFrequency = "M",
                SecuredLoans = 350,
                SecuredloansArrears = 360,
                SecuredLoansFrequency = "F",
                Travel = 370,
                TravelFrequency = "W",
                TvLicence = 380,
                TvLicenceArrears = 390,
                TvLicenceFrequency = "M",
                User = "webuser",
                OtherDebts = new List<SaveOtherDebtsApiModel>() {  },
                Electricity = 400,
                ElectricityArrears = 410,
                ElectricityFrequency = "F",
                Gas = 420,
                GasArrears = 430,
                GasFrequency = "W",
                Water = 440,
                WaterArrears = 450,
                WaterFrequency = "M",
                BenefitsTotal = 460,
                BenefitsTotalFrequency = "F",
                EarningsTotal = 470,
                EarningsTotalFrequency = "W",
                ExpenditureTotal = 480,
                IncomeTotal = 490,
                DisposableIncome = 500,
                UtilitiesTotal = 510,
                UtilitiesTotalArrears = 520,
                UtilitiesTotalFrequency = "M"
            };

            List<SaveOtherDebts> otherDebts = new List<SaveOtherDebts>();

            //Create a copy of source for later
            IncomeAndExpenditureApiModel sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = new IncomeAndExpenditure();
            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);

            expected.LowellReference = "123456789";
            expected.User = "webuser";
            expected.Created = DateTime.Now.Date;
            expected.HasArrears = true;

            _mapperHelper.Setup(x => x.ConvertEmploymentStatusFromCaseflow("full-time")).Returns("employed-full-time");
            _mapperHelper.Setup(x => x.ConvertHousingStatusFromCaseflow("owner")).Returns("homeowner");

            expected.EmploymentStatus = "employed-full-time";
            expected.HousingStatus = "homeowner";
            expected.AdultsInHousehold = 1;
            expected.Children16to18 = 2;
            expected.ChildrenUnder16 = 3;

            expected.IncomeTotal = 490;
            expected.ExpenditureTotal = 480;
            expected.DisposableIncome = 500;

            _mapper.Setup(x => x.Map<List<SaveOtherDebts>>(source.OtherDebts)).Returns(otherDebts);
            expected.OtherDebts = otherDebts;

            _mapperHelper.Setup(x => x.MapRegularPayment(310, "M"))
                .Returns(new RegularPayment() { Amount = 310, Frequency = "monthly" });
            expected.Salary = 310;
            expected.SalaryFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(250, "W"))
                .Returns(new RegularPayment() { Amount = 250, Frequency = "weekly" });
            expected.Pension = 250;
            expected.PensionFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapRegularPayment(470, "W"))
                .Returns(new RegularPayment() { Amount = 470, Frequency = "weekly" });
            expected.EarningsTotal = 470;
            expected.EarningsTotalFrequency = "weekly";
 
            _mapperHelper.Setup(x => x.MapRegularPayment(460, "F"))
                .Returns(new RegularPayment() { Amount = 460, Frequency = "fortnightly" });
            expected.BenefitsTotal = 460;
            expected.BenefitsTotalFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapRegularPayment(230, "W"))
                .Returns(new RegularPayment() { Amount = 230, Frequency = "weekly" });
            expected.OtherIncome = 230;
            expected.OtherincomeFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapOutgoing(270, "F", 280))
                .Returns(new Outgoing() { Amount = 270, ArrearsAmount = 280, Frequency = "fortnightly", InArrears = true });
            expected.Rental = 270;
            expected.RentalArrears = 280;
            expected.RentalFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(120, "F", 130))
                .Returns(new Outgoing() { Amount = 120, ArrearsAmount = 130, Frequency = "fortnightly", InArrears = true });
            expected.ChildMaintenance = 120;
            expected.ChildMaintenanceArrears = 130;
            expected.ChildMaintenanceFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(100, "M", 110))
                .Returns(new Outgoing() { Amount = 100, ArrearsAmount = 110, Frequency = "monthly", InArrears = true });
            expected.CouncilTax = 100;
            expected.CouncilTaxArrears = 110;
            expected.CouncilTaxFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapOutgoing(510, "M", 520))
                .Returns(new Outgoing() { Amount = 510, ArrearsAmount = 520, Frequency = "monthly", InArrears = true });
            expected.UtilitiesTotal = 510;
            expected.UtilitiesTotalArrears = 520;
            expected.UtilitiesTotalFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapOutgoing(400, "F", 410))
                .Returns(new Outgoing() { Amount = 400, ArrearsAmount = 410, Frequency = "fortnightly", InArrears = true });
            expected.Electricity = 400;
            expected.ElectricityArrears = 410;
            expected.ElectricityFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(420, "W", 430))
                .Returns(new Outgoing() { Amount = 420, ArrearsAmount = 430, Frequency = "weekly", InArrears = true });
            expected.Gas = 420;
            expected.GasArrears = 430;
            expected.GasFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapOutgoing(210, "F", 220))
                .Returns(new Outgoing() { Amount = 210, ArrearsAmount = 220, Frequency = "fortnightly", InArrears = true });
            expected.OtherUtilities = 210;
            expected.OtherUtilitiesArrears = 220;
            expected.OtherUtilitiesFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(190, "M", 200))
                .Returns(new Outgoing() { Amount = 190, ArrearsAmount = 200, Frequency = "monthly", InArrears = true });
            expected.Mortgage = 190;
            expected.MortgageArrears = 200;
            expected.MortgageFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapOutgoing(140, "W", 150))
                .Returns(new Outgoing() { Amount = 140, ArrearsAmount = 150, Frequency = "weekly", InArrears = true });
            expected.HomeContents = 140;
            expected.HomeContentsArrears = 150;
            expected.HomeContentsFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapOutgoing(290, "W", 300))
                .Returns(new Outgoing() { Amount = 290, ArrearsAmount = 300, Frequency = "weekly", InArrears = true });
            expected.Rent = 290;
            expected.RentArrears = 300;
            expected.RentFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapOutgoing(350, "F", 360))
                .Returns(new Outgoing() { Amount = 350, ArrearsAmount = 360, Frequency = "fortnightly", InArrears = true });
            expected.SecuredLoans = 350;
            expected.SecuredloansArrears = 360;
            expected.SecuredLoansFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(380, "M", 390))
                .Returns(new Outgoing() { Amount = 380, ArrearsAmount = 390, Frequency = "monthly", InArrears = true });
            expected.TvLicence = 380;
            expected.TvLicenceArrears = 390;
            expected.TvLicenceFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapOutgoing(440, "M", 450))
                .Returns(new Outgoing() { Amount = 440, ArrearsAmount = 450, Frequency = "monthly", InArrears = true });
            expected.Water = 440;
            expected.WaterArrears = 450;
            expected.WaterFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(160, "M"))
                .Returns(new RegularPayment() { Amount = 160, Frequency = "monthly" });
            expected.Healthcare = 160;
            expected.HealthcareFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(180, "W"))
                .Returns(new RegularPayment() { Amount = 180, Frequency = "weekly" });
            expected.Leisure = 180;
            expected.LeisureFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapRegularPayment(170, "F"))
                .Returns(new RegularPayment() { Amount = 170, Frequency = "fortnightly" });
            expected.Housekeeping = 170;
            expected.HousekeepingFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapRegularPayment(260, "M"))
                .Returns(new RegularPayment() { Amount = 260, Frequency = "monthly" });
            expected.PensionInsurance = 260;
            expected.PensionInsuranceFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(240, "F"))
                .Returns(new RegularPayment() { Amount = 240, Frequency = "fortnightly" });
            expected.PersonalCosts = 240;
            expected.PersonalCostsFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapRegularPayment(340, "M"))
                .Returns(new RegularPayment() { Amount = 340, Frequency = "monthly" });
            expected.ProfessionalCosts = 340;
            expected.ProfessionalCostsFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(320, "F"))
                .Returns(new RegularPayment() { Amount = 320, Frequency = "fortnightly" });
            expected.SavingsContributions = 320;
            expected.SavingsContributionsFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapRegularPayment(330, "W"))
                .Returns(new RegularPayment() { Amount = 330, Frequency = "weekly" });
            expected.SchoolCosts = 330;
            expected.SchoolCostsFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapRegularPayment(370, "W"))
                .Returns(new RegularPayment() { Amount = 370, Frequency = "weekly" });
            expected.Travel = 370;
            expected.TravelFrequency = "weekly";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));

            //Check that result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            IncomeAndExpenditureApiModel source = new IncomeAndExpenditureApiModel()
            {
                AdultsInHousehold = 1,
                Children16to18 = 2,
                ChildrenUnder16 = 3,
                Created = DateTime.Now.Date,
                CouncilTax = 100,
                CouncilTaxArrears = 110,
                CouncilTaxFrequency = "M",
                ChildMaintenance = 120,
                ChildMaintenanceArrears = 130,
                ChildMaintenanceFrequency = "F",
                HomeContents = 140,
                HomeContentsArrears = 150,
                HomeContentsFrequency = "W",
                HasArrears = true,
                EmploymentStatus = "full-time",
                Healthcare = 160,
                HealthcareFrequency = "M",
                Housekeeping = 170,
                HousekeepingFrequency = "F",
                HousingStatus = "owner",
                Leisure = 180,
                LeisureFrequency = "W",
                LowellReference = "123456789",
                Mortgage = 190,
                MortgageArrears = 200,
                MortgageFrequency = "M",
                OtherUtilities = 210,
                OtherUtilitiesArrears = 220,
                OtherUtilitiesFrequency = "F",
                OtherIncome = 230,
                OtherincomeFrequency = "W",
                PersonalCosts = 240,
                PersonalCostsFrequency = "F",
                Pension = 250,
                PensionFrequency = "W",
                PensionInsurance = 260,
                PensionInsuranceFrequency = "M",
                Rental = 270,
                RentalArrears = 280,
                RentalFrequency = "F",
                Rent = 290,
                RentArrears = 300,
                RentFrequency = "W",
                Salary = 310,
                SalaryFrequency = "M",
                SavingsContributions = 320,
                SavingsContributionsFrequency = "F",
                SchoolCosts = 330,
                SchoolCostsFrequency = "W",
                ProfessionalCosts = 340,
                ProfessionalCostsFrequency = "M",
                SecuredLoans = 350,
                SecuredloansArrears = 360,
                SecuredLoansFrequency = "F",
                Travel = 370,
                TravelFrequency = "W",
                TvLicence = 380,
                TvLicenceArrears = 390,
                TvLicenceFrequency = "M",
                User = "webuser",
                OtherDebts = new List<SaveOtherDebtsApiModel>() { },
                Electricity = 400,
                ElectricityArrears = 410,
                ElectricityFrequency = "F",
                Gas = 420,
                GasArrears = 430,
                GasFrequency = "W",
                Water = 440,
                WaterArrears = 450,
                WaterFrequency = "M",
                BenefitsTotal = 460,
                BenefitsTotalFrequency = "F",
                EarningsTotal = 470,
                EarningsTotalFrequency = "W",
                ExpenditureTotal = 480,
                IncomeTotal = 490,
                DisposableIncome = 500,
                UtilitiesTotal = 510,
                UtilitiesTotalArrears = 520,
                UtilitiesTotalFrequency = "M"
            };

            List<SaveOtherDebts> otherDebts = new List<SaveOtherDebts>();

            //Create a copy of source for later
            IncomeAndExpenditureApiModel sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = null;
            IncomeAndExpenditure expected = new IncomeAndExpenditure();

            expected.LowellReference = "123456789";
            expected.User = "webuser";
            expected.Created = DateTime.Now.Date;
            expected.HasArrears = true;

            _mapperHelper.Setup(x => x.ConvertEmploymentStatusFromCaseflow("full-time")).Returns("employed-full-time");
            _mapperHelper.Setup(x => x.ConvertHousingStatusFromCaseflow("owner")).Returns("homeowner");

            expected.EmploymentStatus = "employed-full-time";
            expected.HousingStatus = "homeowner";
            expected.AdultsInHousehold = 1;
            expected.Children16to18 = 2;
            expected.ChildrenUnder16 = 3;

            expected.IncomeTotal = 490;
            expected.ExpenditureTotal = 480;
            expected.DisposableIncome = 500;

            _mapper.Setup(x => x.Map<List<SaveOtherDebts>>(source.OtherDebts)).Returns(otherDebts);
            expected.OtherDebts = otherDebts;

            _mapperHelper.Setup(x => x.MapRegularPayment(310, "M"))
                .Returns(new RegularPayment() { Amount = 310, Frequency = "monthly" });
            expected.Salary = 310;
            expected.SalaryFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(250, "W"))
                .Returns(new RegularPayment() { Amount = 250, Frequency = "weekly" });
            expected.Pension = 250;
            expected.PensionFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapRegularPayment(470, "W"))
                .Returns(new RegularPayment() { Amount = 470, Frequency = "weekly" });
            expected.EarningsTotal = 470;
            expected.EarningsTotalFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapRegularPayment(460, "F"))
                .Returns(new RegularPayment() { Amount = 460, Frequency = "fortnightly" });
            expected.BenefitsTotal = 460;
            expected.BenefitsTotalFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapRegularPayment(230, "W"))
                .Returns(new RegularPayment() { Amount = 230, Frequency = "weekly" });
            expected.OtherIncome = 230;
            expected.OtherincomeFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapOutgoing(270, "F", 280))
                .Returns(new Outgoing() { Amount = 270, ArrearsAmount = 280, Frequency = "fortnightly", InArrears = true });
            expected.Rental = 270;
            expected.RentalArrears = 280;
            expected.RentalFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(120, "F", 130))
                .Returns(new Outgoing() { Amount = 120, ArrearsAmount = 130, Frequency = "fortnightly", InArrears = true });
            expected.ChildMaintenance = 120;
            expected.ChildMaintenanceArrears = 130;
            expected.ChildMaintenanceFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(100, "M", 110))
                .Returns(new Outgoing() { Amount = 100, ArrearsAmount = 110, Frequency = "monthly", InArrears = true });
            expected.CouncilTax = 100;
            expected.CouncilTaxArrears = 110;
            expected.CouncilTaxFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapOutgoing(510, "M", 520))
                .Returns(new Outgoing() { Amount = 510, ArrearsAmount = 520, Frequency = "monthly", InArrears = true });
            expected.UtilitiesTotal = 510;
            expected.UtilitiesTotalArrears = 520;
            expected.UtilitiesTotalFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapOutgoing(400, "F", 410))
                .Returns(new Outgoing() { Amount = 400, ArrearsAmount = 410, Frequency = "fortnightly", InArrears = true });
            expected.Electricity = 400;
            expected.ElectricityArrears = 410;
            expected.ElectricityFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(420, "W", 430))
                .Returns(new Outgoing() { Amount = 420, ArrearsAmount = 430, Frequency = "weekly", InArrears = true });
            expected.Gas = 420;
            expected.GasArrears = 430;
            expected.GasFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapOutgoing(210, "F", 220))
                .Returns(new Outgoing() { Amount = 210, ArrearsAmount = 220, Frequency = "fortnightly", InArrears = true });
            expected.OtherUtilities = 210;
            expected.OtherUtilitiesArrears = 220;
            expected.OtherUtilitiesFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(190, "M", 200))
                .Returns(new Outgoing() { Amount = 190, ArrearsAmount = 200, Frequency = "monthly", InArrears = true });
            expected.Mortgage = 190;
            expected.MortgageArrears = 200;
            expected.MortgageFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapOutgoing(140, "W", 150))
                .Returns(new Outgoing() { Amount = 140, ArrearsAmount = 150, Frequency = "weekly", InArrears = true });
            expected.HomeContents = 140;
            expected.HomeContentsArrears = 150;
            expected.HomeContentsFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapOutgoing(290, "W", 300))
                .Returns(new Outgoing() { Amount = 290, ArrearsAmount = 300, Frequency = "weekly", InArrears = true });
            expected.Rent = 290;
            expected.RentArrears = 300;
            expected.RentFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapOutgoing(350, "F", 360))
                .Returns(new Outgoing() { Amount = 350, ArrearsAmount = 360, Frequency = "fortnightly", InArrears = true });
            expected.SecuredLoans = 350;
            expected.SecuredloansArrears = 360;
            expected.SecuredLoansFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapOutgoing(380, "M", 390))
                .Returns(new Outgoing() { Amount = 380, ArrearsAmount = 390, Frequency = "monthly", InArrears = true });
            expected.TvLicence = 380;
            expected.TvLicenceArrears = 390;
            expected.TvLicenceFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapOutgoing(440, "M", 450))
                .Returns(new Outgoing() { Amount = 440, ArrearsAmount = 450, Frequency = "monthly", InArrears = true });
            expected.Water = 440;
            expected.WaterArrears = 450;
            expected.WaterFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(160, "M"))
                .Returns(new RegularPayment() { Amount = 160, Frequency = "monthly" });
            expected.Healthcare = 160;
            expected.HealthcareFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(180, "W"))
                .Returns(new RegularPayment() { Amount = 180, Frequency = "weekly" });
            expected.Leisure = 180;
            expected.LeisureFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapRegularPayment(170, "F"))
                .Returns(new RegularPayment() { Amount = 170, Frequency = "fortnightly" });
            expected.Housekeeping = 170;
            expected.HousekeepingFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapRegularPayment(260, "M"))
                .Returns(new RegularPayment() { Amount = 260, Frequency = "monthly" });
            expected.PensionInsurance = 260;
            expected.PensionInsuranceFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(240, "F"))
                .Returns(new RegularPayment() { Amount = 240, Frequency = "fortnightly" });
            expected.PersonalCosts = 240;
            expected.PersonalCostsFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapRegularPayment(340, "M"))
                .Returns(new RegularPayment() { Amount = 340, Frequency = "monthly" });
            expected.ProfessionalCosts = 340;
            expected.ProfessionalCostsFrequency = "monthly";

            _mapperHelper.Setup(x => x.MapRegularPayment(320, "F"))
                .Returns(new RegularPayment() { Amount = 320, Frequency = "fortnightly" });
            expected.SavingsContributions = 320;
            expected.SavingsContributionsFrequency = "fortnightly";

            _mapperHelper.Setup(x => x.MapRegularPayment(330, "W"))
                .Returns(new RegularPayment() { Amount = 330, Frequency = "weekly" });
            expected.SchoolCosts = 330;
            expected.SchoolCostsFrequency = "weekly";

            _mapperHelper.Setup(x => x.MapRegularPayment(370, "W"))
                .Returns(new RegularPayment() { Amount = 370, Frequency = "weekly" });
            expected.Travel = 370;
            expected.TravelFrequency = "weekly";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));

            //Check that result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }
    }
}
