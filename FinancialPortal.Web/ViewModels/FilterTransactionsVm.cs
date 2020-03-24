using System;
using System.Globalization;

namespace FinancialPortal.Web.ViewModels
{
    public class FilterTransactionsVm
    {
        public string FromDay { get; set; }
        public string FromMonth { get; set; }
        public string FromYear { get; set; }
        public string ToDay { get; set; }
        public string ToMonth { get; set; }
        public string ToYear { get; set; }
        public string KeyWord { get; set; }
        public string DateMessage { get; set; }
        public DateDropDownListVm DatesDropDownList { get; set; }

        public FilterTransactionsVm()
        {
            DatesDropDownList = new DateDropDownListVm();
        }

        public DateTime? DateFrom
        {
            get
            {
                if (DateTime.TryParse($"{FromDay}/{FromMonth}/{FromYear}", new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime date))
                    return date;
                else
                    return null;
            }
        }

        public DateTime? DateTo
        {
            get
            {
                if (DateTime.TryParse($"{ToDay}/{ToMonth}/{ToYear}", new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime date))
                    return date;
                else
                    return null;
            }
        }
    }
}
