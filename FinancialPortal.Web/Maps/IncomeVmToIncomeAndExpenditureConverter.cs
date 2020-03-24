using AutoMapper;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class IncomeVmToIncomeAndExpenditureConverter : ITypeConverter<IncomeVm, IncomeAndExpenditure>
    {
        public IncomeAndExpenditure Convert(IncomeVm source, IncomeAndExpenditure destination, ResolutionContext context)
        {
            if (source == null) { source = new IncomeVm(); }
            if (destination == null) { destination = new IncomeAndExpenditure(); }

            destination.BenefitsTotal = source.BenefitsAndTaxCredits.Amount;
            destination.BenefitsTotalFrequency = source.BenefitsAndTaxCredits.Frequency;

            destination.Salary = source.Earning.Amount;
            destination.SalaryFrequency = source.Earning.Frequency;

            destination.OtherIncome = source.Other.Amount;
            destination.OtherincomeFrequency = source.Other.Frequency;

            destination.Pension = source.Pension.Amount;
            destination.PensionFrequency = source.Pension.Frequency;

            return destination;
        }
    }
}
