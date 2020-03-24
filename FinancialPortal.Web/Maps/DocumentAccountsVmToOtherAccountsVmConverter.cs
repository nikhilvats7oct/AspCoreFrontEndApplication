using AutoMapper;
using FinancialPortal.Models.ViewModels;
using System.Collections.Generic;

namespace FinancialPortal.Web.Maps
{
    public class DocumentAccountsVmToOtherAccountsVmConverter : ITypeConverter<DocumentAccountsVm, LinkedAccountsVm>
    {
        public LinkedAccountsVm Convert(DocumentAccountsVm source, LinkedAccountsVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new LinkedAccountsVm();
            }

            destination.Account = new KeyValuePair<string, string>(source.AccountName, source.AccountReference);
            destination.Reference = source.Reference;
            destination.UnreadDocuments = source.UnreadDocuments;
            destination.IsSelected = false;

            return destination;
        }
    }
}
