using AutoMapper;
using FinancialPortal.Web.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class BillsAndOutgoingsVmToIncomeAndExpenditureConverter : ITypeConverter<BillsAndOutgoingsVm, IncomeAndExpenditure>
    {
        public IncomeAndExpenditure Convert(BillsAndOutgoingsVm source, IncomeAndExpenditure destination, ResolutionContext context)
        {
            if (source == null) { source = new BillsAndOutgoingsVm(); }
            if (destination == null) { destination = new IncomeAndExpenditure(); }

            destination.Rental = source.ApplianceOrFurnitureRental.Amount;
            destination.RentalArrears = source.ApplianceOrFurnitureRental.ArrearsAmount;
            destination.RentalFrequency = source.ApplianceOrFurnitureRental.Frequency;

            destination.CCJs = source.Ccjs.Amount;
            destination.CCJsArrears = source.Ccjs.ArrearsAmount;
            destination.CCJsFrequency = source.Ccjs.Frequency;

            destination.ChildMaintenance = source.ChildMaintenance.Amount;
            destination.ChildMaintenanceArrears = source.ChildMaintenance.ArrearsAmount;
            destination.ChildMaintenanceFrequency = source.ChildMaintenance.Frequency;

            destination.CouncilTax = source.CouncilTax.Amount;
            destination.CouncilTaxArrears = source.CouncilTax.ArrearsAmount;
            destination.CouncilTaxFrequency = source.CouncilTax.Frequency;

            destination.CourtFines = source.CourtFines.Amount;
            destination.CourtFinesArrears = source.CourtFines.ArrearsAmount;
            destination.CourtFinesFrequency = source.CourtFines.Frequency;

            destination.Electricity = source.Electric.Amount;
            destination.ElectricityArrears = source.Electric.ArrearsAmount;
            destination.ElectricityFrequency = source.Electric.Frequency;

            destination.Gas = source.Gas.Amount;
            destination.GasArrears = source.Gas.ArrearsAmount;
            destination.GasFrequency = source.Gas.Frequency;

            destination.Mortgage = source.Mortgage.Amount;
            destination.MortgageArrears = source.Mortgage.ArrearsAmount;
            destination.MortgageFrequency = source.Mortgage.Frequency;

            destination.OtherUtilities = source.OtherFuel.Amount;
            destination.OtherUtilitiesArrears = source.OtherFuel.ArrearsAmount;
            destination.OtherUtilitiesFrequency = source.OtherFuel.Frequency;

            destination.Rent = source.Rent.Amount;
            destination.RentArrears = source.Rent.ArrearsAmount;
            destination.RentFrequency = source.Rent.Frequency;

            destination.SecuredLoans = source.SecuredLoan.Amount;
            destination.SecuredloansArrears = source.SecuredLoan.ArrearsAmount;
            destination.SecuredLoansFrequency = source.SecuredLoan.Frequency;

            destination.TvLicence = source.TvLicense.Amount;
            destination.TvLicenceArrears = source.TvLicense.ArrearsAmount;
            destination.TvLicenceFrequency = source.TvLicense.Frequency;

            destination.Water = source.Water.Amount;
            destination.WaterArrears = source.Water.ArrearsAmount;
            destination.WaterFrequency = source.Water.Frequency;

            return destination;
        }
    }
}
