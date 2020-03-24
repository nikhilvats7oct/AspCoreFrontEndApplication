using AutoMapper;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Maps
{
    public class IncomeAndExpenditureToIncomeVmConverter : ITypeConverter<IncomeAndExpenditure, IncomeVm>
    {
        public IncomeVm Convert(IncomeAndExpenditure source, IncomeVm destination, ResolutionContext context)
        {
            if (source == null) { source = new IncomeAndExpenditure(); }
            if (destination == null) { destination = new IncomeVm(); }

            destination.BenefitsAndTaxCredits.Amount = source.BenefitsTotal;
            destination.BenefitsAndTaxCredits.Frequency = source.BenefitsTotalFrequency;

            destination.Earning.Amount = source.Salary;
            destination.Earning.Frequency = source.SalaryFrequency;

            destination.Other.Amount = source.OtherIncome;
            destination.Other.Frequency = source.OtherincomeFrequency;

            destination.Pension.Amount = source.Pension;
            destination.Pension.Frequency = source.PensionFrequency;

            return destination;
        }
    }
}
