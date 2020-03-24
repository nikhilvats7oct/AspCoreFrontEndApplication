using AutoMapper;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Maps
{
    public class HouseholdStatusVmToIncomeAndExpenditureConverter : ITypeConverter<HouseholdStatusVm, IncomeAndExpenditure>
    {
        public IncomeAndExpenditure Convert(HouseholdStatusVm source, IncomeAndExpenditure destination, ResolutionContext context)
        {
            if (source == null) { source = new HouseholdStatusVm(); }
            if (destination == null) { destination = new IncomeAndExpenditure(); }

            destination.AdultsInHousehold = source.AdultsInHousehold ?? 0;
            destination.Children16to18 = source.ChildrenOver16 ?? 0;
            destination.ChildrenUnder16 = source.ChildrenUnder16 ?? 0;
            destination.EmploymentStatus = source.EmploymentStatus;
            destination.HousingStatus = source.HousingStatus;

            return destination;
        }
    }
}
