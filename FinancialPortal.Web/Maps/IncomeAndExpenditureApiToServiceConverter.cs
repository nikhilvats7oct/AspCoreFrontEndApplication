using AutoMapper;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Maps
{
    public class IncomeAndExpenditureApiToServiceConverter : ITypeConverter<IncomeAndExpenditureApiModel, IncomeAndExpenditure>
    {
        private readonly IMapperHelper _mapperHelper;
        private readonly IMapper _mapper;

        public IncomeAndExpenditureApiToServiceConverter(IMapperHelper mapperHelper, IMapper mapper) 
        {
            this._mapperHelper = mapperHelper;
            this._mapper = mapper;
        }

        public IncomeAndExpenditure Convert(IncomeAndExpenditureApiModel source, IncomeAndExpenditure destination, ResolutionContext context)
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new IncomeAndExpenditure(); }

            destination.LowellReference = source.LowellReference;
            destination.User = source.User;
            destination.Created = source.Created;
            destination.HasArrears = source.HasArrears;

            destination.EmploymentStatus = _mapperHelper.ConvertEmploymentStatusFromCaseflow(source.EmploymentStatus);
            destination.HousingStatus = _mapperHelper.ConvertHousingStatusFromCaseflow(source.HousingStatus);
            destination.AdultsInHousehold = source.AdultsInHousehold;
            destination.Children16to18 = source.Children16to18;
            destination.ChildrenUnder16 = source.ChildrenUnder16;

            destination.IncomeTotal = source.IncomeTotal;
            destination.ExpenditureTotal = source.ExpenditureTotal;
            destination.DisposableIncome = source.DisposableIncome;

            destination.OtherDebts = _mapper.Map<List<SaveOtherDebts>>(source.OtherDebts);

            RegularPayment salary = _mapperHelper.MapRegularPayment(source.Salary, source.SalaryFrequency);
            destination.Salary = salary.Amount;
            destination.SalaryFrequency = salary.Frequency;

            RegularPayment pension = _mapperHelper.MapRegularPayment(source.Pension, source.PensionFrequency);
            destination.Pension = pension.Amount;
            destination.PensionFrequency = pension.Frequency;

            RegularPayment earningsTotal = _mapperHelper.MapRegularPayment(source.EarningsTotal, source.EarningsTotalFrequency);
            destination.EarningsTotal = earningsTotal.Amount;
            destination.EarningsTotalFrequency = earningsTotal.Frequency;

            RegularPayment benefits = _mapperHelper.MapRegularPayment(source.BenefitsTotal, source.BenefitsTotalFrequency);
            destination.BenefitsTotal = benefits.Amount;
            destination.BenefitsTotalFrequency = benefits.Frequency;

            RegularPayment otherIncome = _mapperHelper.MapRegularPayment(source.OtherIncome, source.OtherincomeFrequency);
            destination.OtherIncome = otherIncome.Amount;
            destination.OtherincomeFrequency = otherIncome.Frequency;

            Outgoing applianceOrFurnitureRental = _mapperHelper.MapOutgoing(source.Rental, source.RentalFrequency, source.RentalArrears);
            destination.Rental = applianceOrFurnitureRental.Amount;
            destination.RentalArrears = applianceOrFurnitureRental.ArrearsAmount;
            destination.RentalFrequency = applianceOrFurnitureRental.Frequency;

            Outgoing childMaintenance = _mapperHelper.MapOutgoing(source.ChildMaintenance, source.ChildMaintenanceFrequency, source.ChildMaintenanceArrears);
            destination.ChildMaintenance = childMaintenance.Amount;
            destination.ChildMaintenanceArrears = childMaintenance.ArrearsAmount;
            destination.ChildMaintenanceFrequency = childMaintenance.Frequency;

            Outgoing councilTax = _mapperHelper.MapOutgoing(source.CouncilTax, source.CouncilTaxFrequency, source.CouncilTaxArrears);
            destination.CouncilTax = councilTax.Amount;
            destination.CouncilTaxArrears = councilTax.ArrearsAmount;
            destination.CouncilTaxFrequency = councilTax.Frequency;

            Outgoing utilitiesTotal = _mapperHelper.MapOutgoing(source.UtilitiesTotal, source.UtilitiesTotalFrequency, source.UtilitiesTotalArrears);
            destination.UtilitiesTotal = utilitiesTotal.Amount;
            destination.UtilitiesTotalArrears = utilitiesTotal.ArrearsAmount;
            destination.UtilitiesTotalFrequency = utilitiesTotal.Frequency;

            Outgoing electricity = new Outgoing();
            Outgoing gas = new Outgoing();
            Outgoing otherUtilities = new Outgoing();

            if ((source.Electricity > 0.00M || source.Gas > 0.00M || source.OtherUtilities > 0.00M) && source.UtilitiesTotal >= 0.00M)
            {
                electricity = _mapperHelper.MapOutgoing(source.Electricity, source.ElectricityFrequency, source.ElectricityArrears);
                destination.Electricity = electricity.Amount;
                destination.ElectricityArrears = electricity.ArrearsAmount;
                destination.ElectricityFrequency = electricity.Frequency;

                gas = _mapperHelper.MapOutgoing(source.Gas, source.GasFrequency, source.GasArrears);
                destination.Gas = gas.Amount;
                destination.GasArrears = gas.ArrearsAmount;
                destination.GasFrequency = gas.Frequency;

                otherUtilities = _mapperHelper.MapOutgoing(source.OtherUtilities, source.OtherUtilitiesFrequency, source.OtherUtilitiesArrears);
                destination.OtherUtilities = otherUtilities.Amount;
                destination.OtherUtilitiesArrears = otherUtilities.ArrearsAmount;
                destination.OtherUtilitiesFrequency = otherUtilities.Frequency;
            }

            if ((source.Electricity == 0.00M && source.Gas == 0.00M && source.OtherUtilities == 0.00M) && source.UtilitiesTotal > 0.00M) 
            {
                decimal utilities = source.UtilitiesTotal / 2;
                decimal arrears = source.UtilitiesTotalArrears / 2;
                var frequency = source.UtilitiesTotalFrequency;

                electricity = _mapperHelper.MapOutgoing(source.UtilitiesTotal / 2, source.UtilitiesTotalFrequency, source.UtilitiesTotalArrears / 2);
                destination.Electricity = electricity.Amount;
                destination.ElectricityArrears = electricity.ArrearsAmount;
                destination.ElectricityFrequency = electricity.Frequency;

                gas = _mapperHelper.MapOutgoing(source.UtilitiesTotal / 2, source.UtilitiesTotalFrequency, source.UtilitiesTotalArrears / 2);
                destination.Gas = gas.Amount;
                destination.GasArrears = gas.ArrearsAmount;
                destination.GasFrequency = gas.Frequency;
            }

            Outgoing mortgage = _mapperHelper.MapOutgoing(source.Mortgage, source.MortgageFrequency, source.MortgageArrears);
            destination.Mortgage = mortgage.Amount;
            destination.MortgageArrears = mortgage.ArrearsAmount;
            destination.MortgageFrequency = mortgage.Frequency;

            Outgoing homeContents = _mapperHelper.MapOutgoing(source.HomeContents, source.HomeContentsFrequency, source.HomeContentsArrears);
            destination.HomeContents = homeContents.Amount;
            destination.HomeContentsArrears = homeContents.ArrearsAmount;
            destination.HomeContentsFrequency = homeContents.Frequency;

            Outgoing rent = _mapperHelper.MapOutgoing(source.Rent, source.RentFrequency, source.RentArrears);
            destination.Rent = rent.Amount;
            destination.RentArrears = rent.ArrearsAmount;
            destination.RentFrequency = rent.Frequency;

            Outgoing securedLoans = _mapperHelper.MapOutgoing(source.SecuredLoans, source.SecuredLoansFrequency, source.SecuredloansArrears);
            destination.SecuredLoans = securedLoans.Amount;
            destination.SecuredloansArrears = securedLoans.ArrearsAmount;
            destination.SecuredLoansFrequency = securedLoans.Frequency;

            Outgoing tvLicense = _mapperHelper.MapOutgoing(source.TvLicence, source.TvLicenceFrequency, source.TvLicenceArrears);
            destination.TvLicence = tvLicense.Amount;
            destination.TvLicenceArrears = tvLicense.ArrearsAmount;
            destination.TvLicenceFrequency = tvLicense.Frequency;

            Outgoing water = _mapperHelper.MapOutgoing(source.Water, source.WaterFrequency, source.WaterArrears);
            destination.Water = water.Amount;
            destination.WaterArrears = water.ArrearsAmount;
            destination.WaterFrequency = water.Frequency;

            RegularPayment healthcare = _mapperHelper.MapRegularPayment(source.Healthcare, source.HealthcareFrequency);
            destination.Healthcare = healthcare.Amount;
            destination.HealthcareFrequency = healthcare.Frequency;

            RegularPayment leisure = _mapperHelper.MapRegularPayment(source.Leisure, source.LeisureFrequency);
            destination.Leisure = leisure.Amount;
            destination.LeisureFrequency = leisure.Frequency;

            RegularPayment housekeeping = _mapperHelper.MapRegularPayment(source.Housekeeping, source.HousekeepingFrequency);
            destination.Housekeeping = housekeeping.Amount;
            destination.HousekeepingFrequency = housekeeping.Frequency;

            RegularPayment pensionInsurance = _mapperHelper.MapRegularPayment(source.PensionInsurance, source.PensionInsuranceFrequency);
            destination.PensionInsurance = pensionInsurance.Amount;
            destination.PensionInsuranceFrequency = pensionInsurance.Frequency;

            RegularPayment personalCosts = _mapperHelper.MapRegularPayment(source.PersonalCosts, source.PersonalCostsFrequency);
            destination.PersonalCosts = personalCosts.Amount;
            destination.PersonalCostsFrequency = personalCosts.Frequency;

            RegularPayment professionalCosts = _mapperHelper.MapRegularPayment(source.ProfessionalCosts, source.ProfessionalCostsFrequency);
            destination.ProfessionalCosts = professionalCosts.Amount;
            destination.ProfessionalCostsFrequency = professionalCosts.Frequency;

            RegularPayment savings = _mapperHelper.MapRegularPayment(source.SavingsContributions, source.SavingsContributionsFrequency);
            destination.SavingsContributions = savings.Amount;
            destination.SavingsContributionsFrequency = savings.Frequency;

            RegularPayment schoolCosts = _mapperHelper.MapRegularPayment(source.SchoolCosts, source.SchoolCostsFrequency);
            destination.SchoolCosts = schoolCosts.Amount;
            destination.SchoolCostsFrequency = schoolCosts.Frequency;

            RegularPayment travel = _mapperHelper.MapRegularPayment(source.Travel, source.TravelFrequency);
            destination.Travel = travel.Amount;
            destination.TravelFrequency = travel.Frequency;

            return destination;
        }
    }
}
