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
    public class IncomeAndExpenditureToExpendituresVmConverter : ITypeConverter<IncomeAndExpenditure, ExpendituresVm>
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IMapper _mapper;
        public IncomeAndExpenditureToExpendituresVmConverter(ICalculatorService calculatorService, IMapper mapper) 
        {
            _calculatorService = calculatorService;
            _mapper = mapper;
        }

        public ExpendituresVm Convert(IncomeAndExpenditure source, ExpendituresVm destination, ResolutionContext context)
        {
            if (source == null) { source = new IncomeAndExpenditure(); }
            if (destination == null) { destination = new ExpendituresVm(); }

            destination.CareAndHealthCosts.Amount = source.Healthcare;
            destination.CareAndHealthCosts.Frequency = source.HealthcareFrequency;

            destination.CommunicationsAndLeisure.Amount = source.Leisure;
            destination.CommunicationsAndLeisure.Frequency = source.LeisureFrequency;

            destination.FoodAndHouseKeeping.Amount = source.Housekeeping;
            destination.FoodAndHouseKeeping.Frequency = source.HousekeepingFrequency;

            destination.IncomeVmSummary = _mapper.Map<MonthlyIncomeVm>(_calculatorService.CalculateMonthlyIncome(source));

            destination.Other.Amount = source.OtherExpenditure;
            destination.Other.Frequency = source.OtherExpenditureFrequency;

            destination.OutgoingsVmSummary = _mapper.Map<MonthlyOutgoingsVm>(_calculatorService.CalculateMonthlyOutgoings(source));
            
            destination.PensionsAndInsurance.Amount = source.PensionInsurance;
            destination.PensionsAndInsurance.Frequency = source.PensionInsuranceFrequency;

            destination.PersonalCosts.Amount = source.PersonalCosts;
            destination.PersonalCosts.Frequency = source.PersonalCostsFrequency;

            destination.Professional.Amount = source.ProfessionalCosts;
            destination.Professional.Frequency = source.ProfessionalCostsFrequency;

            destination.Savings.Amount = source.SavingsContributions;
            destination.Savings.Frequency = source.SavingsContributionsFrequency;

            destination.SchoolCosts.Amount = source.SchoolCosts;
            destination.SchoolCosts.Frequency = source.SchoolCostsFrequency;

            destination.TravelAndTransport.Amount = source.Travel;
            destination.TravelAndTransport.Frequency = source.TravelFrequency;

            return destination;
        }
    }
}
