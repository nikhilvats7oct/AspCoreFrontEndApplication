using AutoMapper;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using System;

namespace FinancialPortal.Web.Maps
{
    public class IncomeAndExpenditureServiceToApiConverter : ITypeConverter<IncomeAndExpenditure, IncomeAndExpenditureApiModel>
    {
        private IMapperHelper _mapperHelper;
        private ICalculatorService _calculatorService;

        public IncomeAndExpenditureServiceToApiConverter(IMapperHelper mapperHelper, ICalculatorService calculatorService)
        {
            _mapperHelper = mapperHelper;
            _calculatorService = calculatorService;
        }

        public IncomeAndExpenditureApiModel Convert(IncomeAndExpenditure source, IncomeAndExpenditureApiModel destination, ResolutionContext context)
        {
            if (source == null) { return null; }

            if (destination == null)
            {
                destination = new IncomeAndExpenditureApiModel();
            }

            destination.LowellReference = source.LowellReference;
            destination.User = source.BudgetSource == "MyBudget Tool"? "OpenWrks": "webuser";

            destination.Created = DateTime.UtcNow;
            destination.HasArrears = _calculatorService.InArrears(source);

            destination.AdultsInHousehold = source.AdultsInHousehold;
            destination.ChildrenUnder16 = source.ChildrenUnder16;
            destination.Children16to18 = source.Children16to18;

            destination.Salary = source.Salary;
            destination.SalaryFrequency = source.Salary == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.SalaryFrequency);

            destination.Pension = source.Pension;
            destination.PensionFrequency = source.Pension == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.PensionFrequency);

            destination.EarningsTotal = 0.00M;
            destination.EarningsTotalFrequency = "";

            destination.BenefitsTotal = source.BenefitsTotal;
            destination.BenefitsTotalFrequency = source.BenefitsTotal == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.BenefitsTotalFrequency);

            destination.OtherIncome = source.OtherIncome;
            destination.OtherincomeFrequency = source.OtherIncome == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.OtherincomeFrequency);

            destination.Mortgage = source.Mortgage;
            destination.MortgageFrequency = source.Mortgage == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.MortgageFrequency);
            destination.MortgageArrears = source.MortgageArrears;

            destination.Rent = source.Rent;
            destination.RentFrequency = source.Rent == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.RentFrequency);
            destination.RentArrears = source.RentArrears;

            destination.SecuredLoans = source.SecuredLoans;
            destination.SecuredLoansFrequency = source.SecuredLoans == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.SecuredLoansFrequency);
            destination.SecuredloansArrears = source.SecuredloansArrears;

            destination.CouncilTax = source.CouncilTax;
            destination.CouncilTaxFrequency = source.CouncilTax == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.CouncilTaxFrequency);
            destination.CouncilTaxArrears = source.CouncilTaxArrears;

            destination.Rental = source.Rental;
            destination.RentalFrequency = source.Rental == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.RentalFrequency);
            destination.RentalArrears = source.RentalArrears;

            destination.TvLicence = source.TvLicence;
            destination.TvLicenceFrequency = source.TvLicence == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.TvLicenceFrequency);
            destination.TvLicenceArrears = source.TvLicenceArrears;

            destination.HomeContents = 0.00M;
            destination.HomeContentsFrequency = "";
            destination.HomeContentsArrears = source.HomeContentsArrears;

            destination.Gas = source.Gas;
            destination.GasFrequency = source.Gas == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.GasFrequency);
            destination.GasArrears = source.GasArrears;

            destination.OtherUtilities = source.OtherUtilities;
            destination.OtherUtilitiesFrequency = source.OtherUtilities == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.OtherUtilitiesFrequency);
            destination.OtherUtilitiesArrears = source.OtherUtilitiesArrears;

            destination.Electricity = source.Electricity;
            destination.ElectricityFrequency = source.Electricity == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.ElectricityFrequency);
            destination.ElectricityArrears = source.ElectricityArrears;

            destination.Water = source.Water;
            destination.WaterFrequency = source.Water == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.WaterFrequency);
            destination.WaterArrears = source.WaterArrears;

            destination.UtilitiesTotal = 0.00m;
            destination.UtilitiesTotalFrequency = "";
            destination.UtilitiesTotalArrears = source.UtilitiesTotalArrears;

            destination.ChildMaintenance = source.ChildMaintenance;
            destination.ChildMaintenanceFrequency = source.ChildMaintenance == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.ChildMaintenanceFrequency);
            destination.ChildMaintenanceArrears = source.ChildMaintenanceArrears;

            destination.Housekeeping = source.Housekeeping;
            destination.HousekeepingFrequency = source.Housekeeping == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.HousekeepingFrequency);

            destination.PersonalCosts = source.PersonalCosts;
            destination.PersonalCostsFrequency = source.PersonalCosts == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.PersonalCostsFrequency);

            destination.Leisure = source.Leisure;
            destination.LeisureFrequency = source.Leisure == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.LeisureFrequency);

            destination.Travel = source.Travel;
            destination.TravelFrequency = source.Travel == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.TravelFrequency);

            destination.Healthcare = source.Healthcare;
            destination.HealthcareFrequency = source.Healthcare == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.HealthcareFrequency);

            destination.PensionInsurance = source.PensionInsurance;
            destination.PensionInsuranceFrequency = source.PensionInsurance == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.PensionInsuranceFrequency);

            destination.SchoolCosts = source.SchoolCosts;
            destination.SchoolCostsFrequency = source.SchoolCosts == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.SchoolCostsFrequency);

            destination.ProfessionalCosts = source.ProfessionalCosts;
            destination.ProfessionalCostsFrequency = source.ProfessionalCosts == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.ProfessionalCostsFrequency);

            destination.SavingsContributions = source.SavingsContributions;
            destination.SavingsContributionsFrequency = source.SavingsContributions == 0.00M ? "" : _mapperHelper.ConvertFrequencyToInitial(source.SavingsContributionsFrequency);

            destination.OtherDebts = _mapperHelper.CreateOtherDebts(source);


            destination.IncomeTotal = _calculatorService.CalculateMonthlyIncome(source).Total;
            destination.ExpenditureTotal = _calculatorService.CalculateMonthlyOutgoings(source).Total;
            destination.DisposableIncome = Math.Round(destination.IncomeTotal - destination.ExpenditureTotal, 2, MidpointRounding.AwayFromZero);
            destination.HousingStatus = _mapperHelper.ConvertToHousingStatusCaseflow(source.HousingStatus);
            destination.EmploymentStatus = _mapperHelper.ConvertToEmploymentStatusCaseflow(source.EmploymentStatus);
           

            return destination;
        }
    }
}
