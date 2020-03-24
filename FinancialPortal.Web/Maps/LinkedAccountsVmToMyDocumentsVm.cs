using AutoMapper;
using FinancialPortal.Models.ViewModels;
using System.Collections.Generic;

namespace FinancialPortal.Web.Maps
{
    public class LinkedAccountsVmToMyDocumentsVm : ITypeConverter<List<LinkedAccountsVm>, MyDocumentsVm>
    {
        public MyDocumentsVm Convert(List<LinkedAccountsVm> source, MyDocumentsVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new MyDocumentsVm();
            }

            destination.LinkedAccounts = source;

            return destination;

        }
    }
}
