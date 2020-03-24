using AutoMapper;
using FinancialPortal.Models.ViewModels;
using X.PagedList;

namespace FinancialPortal.Web.Maps
{
    public class PagedDocumentListToDocumentsVmConverter : ITypeConverter<IPagedList<DocumentVm>, MyDocumentsVm>
    {
        public MyDocumentsVm Convert(IPagedList<DocumentVm> source, MyDocumentsVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new MyDocumentsVm();
            }

            destination.Account.Documents = source;

            return destination;
        }
    }
}
