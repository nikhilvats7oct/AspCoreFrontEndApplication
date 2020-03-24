using AutoMapper;
using FinancialPortal.Models.ViewModels;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class OrderByVmToFilterDocumentsVmConverter : ITypeConverter<OrderByVm, FilterDocumentsVm>
    {
        public FilterDocumentsVm Convert(OrderByVm source, FilterDocumentsVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new FilterDocumentsVm();
            }

            destination.FromDay = source.DateFrom?.Day.ToString();
            destination.FromMonth = source.DateFrom?.Month.ToString();
            destination.FromYear = source.DateTo?.Year.ToString();
            destination.ToDay = source.DateTo?.Day.ToString();
            destination.ToMonth = source.DateTo?.Month.ToString();
            destination.ToYear = source.DateTo?.Year.ToString();
            destination.KeyWord = source.Keyword;
            destination.OrderByRead = source.OrderByRead;
            destination.OrderByReceived = source.OrderByReceived;
            destination.OrderBySubject = source.OrderBySubject;

            return destination;
        }
    }
}
