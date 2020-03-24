using AutoMapper;
using FinancialPortal.Models.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class ListDocumentAccountsVmToMyDocumentsVmConverter : ITypeConverter<DocumentAccountsVm, MyDocumentsVm>
    {
        public MyDocumentsVm Convert(DocumentAccountsVm source, MyDocumentsVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new MyDocumentsVm();
            }

            destination.Account = source;


            return destination;
        }
    }
}
