using AutoMapper;
using FinancialPortal.Models.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class FilterDocumentsVmToFilterDocumentsVmConverter : ITypeConverter<FilterDocumentsVm, FilterDocumentsVm>
    {
        public FilterDocumentsVm Convert(FilterDocumentsVm source, FilterDocumentsVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new FilterDocumentsVm();
            }

            destination.FromDay = source.FromDay ?? destination.FromDay;
            destination.FromMonth = source.FromMonth ?? destination.FromMonth;
            destination.FromYear = source.FromYear ?? destination.FromYear;
            destination.ToDay = source.ToDay ?? destination.ToDay;
            destination.ToMonth = source.ToMonth ?? destination.ToMonth;
            destination.ToYear = source.ToYear ?? destination.ToYear;
            destination.KeyWord = source.KeyWord ?? destination.KeyWord;
            destination.OrderByReceived = source?.OrderByReceived;
            destination.OrderByRead = source?.OrderByRead;
            destination.OrderBySubject = source?.OrderBySubject;

            return destination;
        }
    }
}
