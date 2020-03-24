using AutoMapper;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Maps
{
    public class IncomeAndExpenditureToHouseholdStatusVmConverter : ITypeConverter<IncomeAndExpenditure, HouseholdStatusVm>
    {
        public HouseholdStatusVm Convert(IncomeAndExpenditure source, HouseholdStatusVm destination, ResolutionContext context)
        {
            if (source == null) { source = new IncomeAndExpenditure(); }
            if (destination == null) { destination = new HouseholdStatusVm(); }

            destination.AdultsInHousehold = source.AdultsInHousehold;
            destination.ChildrenOver16 = source.Children16to18;
            destination.ChildrenUnder16 = source.ChildrenUnder16;
            destination.EmploymentStatus = source.EmploymentStatus;
            destination.HousingStatus = source.HousingStatus;

            return destination;
        }
    }
}
