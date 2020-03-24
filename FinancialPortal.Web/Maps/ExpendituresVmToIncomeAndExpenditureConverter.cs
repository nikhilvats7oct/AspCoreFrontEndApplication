using AutoMapper;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class ExpendituresVmToIncomeAndExpenditureConverter : ITypeConverter<ExpendituresVm, IncomeAndExpenditure>
    {
        public IncomeAndExpenditure Convert(ExpendituresVm source, IncomeAndExpenditure destination, ResolutionContext context)
        {
            if (source == null) { source = new ExpendituresVm(); }
            if (destination == null) { destination = new IncomeAndExpenditure(); }

            destination.Healthcare = source.CareAndHealthCosts.Amount;
            destination.HealthcareFrequency = source.CareAndHealthCosts.Frequency;

            destination.Leisure = source.CommunicationsAndLeisure.Amount;
            destination.LeisureFrequency = source.CommunicationsAndLeisure.Frequency;

            destination.Housekeeping = source.FoodAndHouseKeeping.Amount;
            destination.HousekeepingFrequency = source.FoodAndHouseKeeping.Frequency;

            destination.OtherExpenditure = source.Other.Amount;
            destination.OtherExpenditureFrequency = source.Other.Frequency;

            destination.PensionInsurance = source.PensionsAndInsurance.Amount;
            destination.PensionInsuranceFrequency = source.PensionsAndInsurance.Frequency;

            destination.PersonalCosts = source.PersonalCosts.Amount;
            destination.PersonalCostsFrequency = source.PersonalCosts.Frequency;

            destination.ProfessionalCosts = source.Professional.Amount;
            destination.ProfessionalCostsFrequency = source.Professional.Frequency;

            destination.SavingsContributions = source.Savings.Amount;
            destination.SavingsContributionsFrequency = source.Savings.Frequency;

            destination.SchoolCosts = source.SchoolCosts.Amount;
            destination.SchoolCostsFrequency = source.SchoolCosts.Frequency;

            destination.Travel = source.TravelAndTransport.Amount;
            destination.TravelFrequency = source.TravelAndTransport.Frequency;

            return destination;
        }
    }
}
