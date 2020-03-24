using AutoMapper;
using FinancialPortal.Models.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class FilterDocumentsVmToMyDocumentsVmConverter : ITypeConverter<FilterDocumentsVm, MyDocumentsVm>
    {
        MyDocumentsVm ITypeConverter<FilterDocumentsVm, MyDocumentsVm>.Convert(FilterDocumentsVm source, MyDocumentsVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new MyDocumentsVm();
            }

            destination.FilterItems.FromDay = source.FromDay;
            destination.FilterItems.FromMonth = source.FromMonth;
            destination.FilterItems.FromYear = source.FromYear;
            destination.FilterItems.ToDay = source.ToDay;
            destination.FilterItems.ToMonth = source.ToMonth;
            destination.FilterItems.ToYear = source.ToYear;
            destination.FilterItems.KeyWord = source.KeyWord;
            destination.FilterItems.OrderByReceived = source?.OrderByReceived ?? "ascending";
            destination.FilterItems.OrderByRead = source?.OrderByRead ?? "ascending"; ;
            destination.FilterItems.OrderBySubject = source?.OrderBySubject ?? "ascending";

            return destination;
        }
    }
}
