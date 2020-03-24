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
    public class BillsAndOutgoingsVmToIncomeAndExpenditureConverterTests
    {
        private BillsAndOutgoingsVmToIncomeAndExpenditureConverter _converter;

        [TestInitialize]
        public void TestInitialise() 
        {
            this._converter = new BillsAndOutgoingsVmToIncomeAndExpenditureConverter();
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            BillsAndOutgoingsVm source = null;
            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();
            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);

            expected.Rental = 0;
            expected.RentalArrears = 0;
            expected.RentalFrequency = null;
            expected.CCJs = 0;
            expected.CCJsArrears = 0;
            expected.CCJsFrequency = null;
            expected.ChildMaintenance = 0;
            expected.ChildMaintenanceArrears = 0;
            expected.ChildMaintenanceFrequency = null;
            expected.CouncilTax = 0;
            expected.CouncilTaxArrears = 0;
            expected.CouncilTaxFrequency = null;
            expected.CourtFines = 0;
            expected.CourtFinesArrears = 0;
            expected.CourtFinesFrequency = null;
            expected.Electricity = 0;
            expected.ElectricityArrears = 0;
            expected.ElectricityFrequency = null;
            expected.Gas = 0;
            expected.GasArrears = 0;
            expected.GasFrequency = null;
            expected.Mortgage = 0;
            expected.MortgageArrears = 0;
            expected.MortgageFrequency = null;
            expected.OtherUtilities = 0;
            expected.OtherUtilitiesArrears = 0;
            expected.OtherUtilitiesFrequency = null;
            expected.Rent = 0;
            expected.RentArrears = 0;
            expected.RentFrequency = null;
            expected.SecuredLoans = 0;
            expected.SecuredloansArrears = 0;
            expected.SecuredLoansFrequency = null;
            expected.TvLicence = 0;
            expected.TvLicenceArrears = 0;
            expected.TvLicenceFrequency = null;
            expected.Water = 0;
            expected.WaterArrears = 0;
            expected.WaterFrequency = null; ;

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest()
        {
            BillsAndOutgoingsVm source = new BillsAndOutgoingsVm() 
            {
                Mortgage = new OutgoingSourceVm() { Amount = 100, ArrearsAmount = 10, Frequency = "monthly", InArrears = true },
                ApplianceOrFurnitureRental = new OutgoingSourceVm() { Amount = 150, ArrearsAmount = 0, Frequency = "weekly", InArrears = false },
                Electric = new OutgoingSourceVm() { Amount = 200, ArrearsAmount = 20, Frequency = "fortnightly", InArrears = true },
                Ccjs = new OutgoingSourceVm() { Amount = 250, ArrearsAmount = 0, Frequency = "monthly", InArrears = false },
                ChildMaintenance = new OutgoingSourceVm() { Amount = 300, ArrearsAmount = 30, Frequency = "weekly", InArrears = true },
                CouncilTax = new OutgoingSourceVm() { Amount = 350, ArrearsAmount = 0, Frequency = "fortnightly", InArrears = false },
                CourtFines = new OutgoingSourceVm() { Amount = 400, ArrearsAmount = 40, Frequency = "monthly", InArrears = true },
                Gas = new OutgoingSourceVm() { Amount = 450, ArrearsAmount = 0, Frequency = "weekly", InArrears = false },
                OtherFuel = new OutgoingSourceVm() { Amount = 500, ArrearsAmount = 50, Frequency = "fortnightly", InArrears = true },
                Rent = new OutgoingSourceVm() { Amount = 550, ArrearsAmount = 0, Frequency = "monthly", InArrears = false },
                SecuredLoan = new OutgoingSourceVm() { Amount = 600, ArrearsAmount = 60, Frequency = "weekly", InArrears = true },
                TvLicense = new OutgoingSourceVm() { Amount = 650, ArrearsAmount = 0, Frequency = "fortnightly", InArrears = false },
                Water = new OutgoingSourceVm() { Amount = 700, ArrearsAmount = 70, Frequency = "monthly", InArrears = true },
                EnabledPartialSave = true,
                HasErrorPartialSavedIAndE = false,
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "123456789" } },
                PartialSavedEvent = true,
                PartialSavedIAndE = false,
                IncomeSummary = new MonthlyIncomeVm() { Benefits = 12, Other = 34, Pension = 56, Salary = 78, Total = 90  },
                OutgoingSummary = new MonthlyOutgoingsVm() { Expenditures = 123, HouseholdBills = 456, Total = 789 }
            };

            //Create a copy of source for later
            BillsAndOutgoingsVm sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = Utilities.CreateDefaultTestIAndE();

            IncomeAndExpenditure expected = Utilities.DeepCopy(destination);
            expected.Mortgage = 100;
            expected.MortgageArrears = 10;
            expected.MortgageFrequency = "monthly";
            expected.Rental = 150;
            expected.RentalArrears = 0;
            expected.RentalFrequency = "weekly";
            expected.Electricity = 200;
            expected.ElectricityArrears = 20;
            expected.ElectricityFrequency = "fortnightly";
            expected.CCJs = 250;
            expected.CCJsArrears = 0;
            expected.CCJsFrequency = "monthly";
            expected.ChildMaintenance = 300;
            expected.ChildMaintenanceArrears = 30;
            expected.ChildMaintenanceFrequency = "weekly";
            expected.CouncilTax = 350;
            expected.CouncilTaxArrears = 0;
            expected.CouncilTaxFrequency = "fortnightly";
            expected.CourtFines = 400;
            expected.CourtFinesArrears = 40;
            expected.CourtFinesFrequency = "monthly";
            expected.Gas = 450;
            expected.GasArrears = 0;
            expected.GasFrequency = "weekly";
            expected.OtherUtilities = 500;
            expected.OtherUtilitiesArrears = 50;
            expected.OtherUtilitiesFrequency = "fortnightly";
            expected.Rent = 550;
            expected.RentArrears = 0;
            expected.RentFrequency = "monthly";
            expected.SecuredLoans = 600;
            expected.SecuredloansArrears = 60;
            expected.SecuredLoansFrequency = "weekly";
            expected.TvLicence = 650;
            expected.TvLicenceArrears = 0;
            expected.TvLicenceFrequency = "fortnightly";
            expected.Water = 700;
            expected.WaterArrears = 70;
            expected.WaterFrequency = "monthly";
            
            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            BillsAndOutgoingsVm source = new BillsAndOutgoingsVm()
            {
                Mortgage = new OutgoingSourceVm() { Amount = 100, ArrearsAmount = 10, Frequency = "monthly", InArrears = true },
                ApplianceOrFurnitureRental = new OutgoingSourceVm() { Amount = 150, ArrearsAmount = 0, Frequency = "weekly", InArrears = false },
                Electric = new OutgoingSourceVm() { Amount = 200, ArrearsAmount = 20, Frequency = "fortnightly", InArrears = true },
                Ccjs = new OutgoingSourceVm() { Amount = 250, ArrearsAmount = 0, Frequency = "monthly", InArrears = false },
                ChildMaintenance = new OutgoingSourceVm() { Amount = 300, ArrearsAmount = 30, Frequency = "weekly", InArrears = true },
                CouncilTax = new OutgoingSourceVm() { Amount = 350, ArrearsAmount = 0, Frequency = "fortnightly", InArrears = false },
                CourtFines = new OutgoingSourceVm() { Amount = 400, ArrearsAmount = 40, Frequency = "monthly", InArrears = true },
                Gas = new OutgoingSourceVm() { Amount = 450, ArrearsAmount = 0, Frequency = "weekly", InArrears = false },
                OtherFuel = new OutgoingSourceVm() { Amount = 500, ArrearsAmount = 50, Frequency = "fortnightly", InArrears = true },
                Rent = new OutgoingSourceVm() { Amount = 550, ArrearsAmount = 0, Frequency = "monthly", InArrears = false },
                SecuredLoan = new OutgoingSourceVm() { Amount = 600, ArrearsAmount = 60, Frequency = "weekly", InArrears = true },
                TvLicense = new OutgoingSourceVm() { Amount = 650, ArrearsAmount = 0, Frequency = "fortnightly", InArrears = false },
                Water = new OutgoingSourceVm() { Amount = 700, ArrearsAmount = 70, Frequency = "monthly", InArrears = true },
                EnabledPartialSave = true,
                HasErrorPartialSavedIAndE = false,
                GtmEvents = new List<GtmEvent>() { new GtmEvent() { account_ref = "123456789" } },
                PartialSavedEvent = true,
                PartialSavedIAndE = false,
                IncomeSummary = new MonthlyIncomeVm() { Benefits = 12, Other = 34, Pension = 56, Salary = 78, Total = 90 },
                OutgoingSummary = new MonthlyOutgoingsVm() { Expenditures = 123, HouseholdBills = 456, Total = 789 }
            };

            //Create a copy of source for later
            BillsAndOutgoingsVm sourceCopy = Utilities.DeepCopy(source);

            IncomeAndExpenditure destination = null;

            IncomeAndExpenditure expected = new IncomeAndExpenditure();
            expected.Mortgage = 100;
            expected.MortgageArrears = 10;
            expected.MortgageFrequency = "monthly";
            expected.Rental = 150;
            expected.RentalArrears = 0;
            expected.RentalFrequency = "weekly";
            expected.Electricity = 200;
            expected.ElectricityArrears = 20;
            expected.ElectricityFrequency = "fortnightly";
            expected.CCJs = 250;
            expected.CCJsArrears = 0;
            expected.CCJsFrequency = "monthly";
            expected.ChildMaintenance = 300;
            expected.ChildMaintenanceArrears = 30;
            expected.ChildMaintenanceFrequency = "weekly";
            expected.CouncilTax = 350;
            expected.CouncilTaxArrears = 0;
            expected.CouncilTaxFrequency = "fortnightly";
            expected.CourtFines = 400;
            expected.CourtFinesArrears = 40;
            expected.CourtFinesFrequency = "monthly";
            expected.Gas = 450;
            expected.GasArrears = 0;
            expected.GasFrequency = "weekly";
            expected.OtherUtilities = 500;
            expected.OtherUtilitiesArrears = 50;
            expected.OtherUtilitiesFrequency = "fortnightly";
            expected.Rent = 550;
            expected.RentArrears = 0;
            expected.RentFrequency = "monthly";
            expected.SecuredLoans = 600;
            expected.SecuredloansArrears = 60;
            expected.SecuredLoansFrequency = "weekly";
            expected.TvLicence = 650;
            expected.TvLicenceArrears = 0;
            expected.TvLicenceFrequency = "fortnightly";
            expected.Water = 700;
            expected.WaterArrears = 70;
            expected.WaterFrequency = "monthly";

            IncomeAndExpenditure result = _converter.Convert(source, destination, null);

            //Check that the result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));
        }
    }
}
