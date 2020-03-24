using AutoMapper;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Maps
{
    public class IncomeAndExpenditureToBillsAndOutgoingsVmConverter : ITypeConverter<IncomeAndExpenditure, BillsAndOutgoingsVm>
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IMapper _mapper;

        public IncomeAndExpenditureToBillsAndOutgoingsVmConverter(ICalculatorService calculatorService, IMapper mapper) 
        {
            this._calculatorService = calculatorService;
            this._mapper = mapper;
        }

        public BillsAndOutgoingsVm Convert(IncomeAndExpenditure source, BillsAndOutgoingsVm destination, ResolutionContext context)
        {
            if (source == null) { source = new IncomeAndExpenditure(); }
            if (destination == null) { destination = new BillsAndOutgoingsVm(); }

            destination.ApplianceOrFurnitureRental.Amount = source.Rental;
            destination.ApplianceOrFurnitureRental.ArrearsAmount = source.RentalArrears;
            destination.ApplianceOrFurnitureRental.Frequency = source.RentalFrequency;
            destination.ApplianceOrFurnitureRental.InArrears = source.RentalArrears > 0;

            destination.Ccjs.Amount = source.CCJs;
            destination.Ccjs.ArrearsAmount = source.CCJsArrears;
            destination.Ccjs.Frequency = source.CCJsFrequency;
            destination.Ccjs.InArrears = source.CCJsArrears > 0;

            destination.ChildMaintenance.Amount = source.ChildMaintenance;
            destination.ChildMaintenance.ArrearsAmount = source.ChildMaintenanceArrears;
            destination.ChildMaintenance.Frequency = source.ChildMaintenanceFrequency;
            destination.ChildMaintenance.InArrears = source.ChildMaintenanceArrears > 0;

            destination.CouncilTax.Amount = source.CouncilTax;
            destination.CouncilTax.ArrearsAmount = source.CouncilTaxArrears;
            destination.CouncilTax.Frequency = source.CouncilTaxFrequency;
            destination.CouncilTax.InArrears = source.CouncilTaxArrears > 0;

            destination.CourtFines.Amount = source.CourtFines;
            destination.CourtFines.ArrearsAmount = source.CourtFinesArrears;
            destination.CourtFines.Frequency = source.CourtFinesFrequency;
            destination.CourtFines.InArrears = source.CourtFinesArrears > 0;

            destination.Electric.Amount = source.Electricity;
            destination.Electric.ArrearsAmount = source.ElectricityArrears;
            destination.Electric.Frequency = source.ElectricityFrequency;
            destination.Electric.InArrears = source.ElectricityArrears > 0;

            destination.Gas.Amount = source.Gas;
            destination.Gas.ArrearsAmount = source.GasArrears;
            destination.Gas.Frequency = source.GasFrequency;
            destination.Gas.InArrears = source.GasArrears > 0;

            destination.IncomeSummary = _mapper.Map<MonthlyIncomeVm>(_calculatorService.CalculateMonthlyIncome(source));

            destination.Mortgage.Amount = source.Mortgage;
            destination.Mortgage.ArrearsAmount = source.MortgageArrears;
            destination.Mortgage.Frequency = source.MortgageFrequency;
            destination.Mortgage.InArrears = source.MortgageArrears > 0;

            destination.OtherFuel.Amount = source.OtherUtilities;
            destination.OtherFuel.ArrearsAmount = source.OtherUtilitiesArrears;
            destination.OtherFuel.Frequency = source.OtherUtilitiesFrequency;
            destination.OtherFuel.InArrears = source.OtherUtilitiesArrears > 0;

            destination.OutgoingSummary = _mapper.Map<MonthlyOutgoingsVm>(_calculatorService.CalculateMonthlyOutgoings(source));

            destination.Rent.Amount = source.Rent;
            destination.Rent.ArrearsAmount = source.RentArrears;
            destination.Rent.Frequency = source.RentFrequency;
            destination.Rent.InArrears = source.RentArrears > 0;

            destination.SecuredLoan.Amount = source.SecuredLoans;
            destination.SecuredLoan.ArrearsAmount = source.SecuredloansArrears;
            destination.SecuredLoan.Frequency = source.SecuredLoansFrequency;
            destination.SecuredLoan.InArrears = source.SecuredloansArrears > 0;

            destination.TvLicense.Amount = source.TvLicence;
            destination.TvLicense.ArrearsAmount = source.TvLicenceArrears;
            destination.TvLicense.Frequency = source.TvLicenceFrequency;
            destination.TvLicense.InArrears = source.TvLicenceArrears > 0;

            destination.Water.Amount = source.Water;
            destination.Water.ArrearsAmount = source.WaterArrears;
            destination.Water.Frequency = source.WaterFrequency;
            destination.Water.InArrears = source.WaterArrears > 0;

            return destination;
        }

    }
}
