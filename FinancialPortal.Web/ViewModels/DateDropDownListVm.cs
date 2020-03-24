using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.ViewModels
{
    public class DateDropDownListVm
    {
        public List<string> DateFromDays { get; set; }
        public List<string> DateFromMonths { get; set; }
        public List<string> DateFromYears { get; set; }

        public List<string> DateToDays { get; set; }
        public List<string> DateToMonths { get; set; }
        public List<string> DateToYears { get; set; }

        public DateDropDownListVm()
        {
            DateFromDays = GetDays();
            DateFromMonths = GetMonths();
            DateFromYears = GetYears();

            DateToDays = GetDays();
            DateToMonths = GetMonths();
            DateToYears = GetYears();
        }

        private Dictionary<string, int> MonthAndDays()
        {
            return new Dictionary<string, int>()
            {
                 { "January", 31 },
                 { "February", 28},
                 { "March" , 31 },
                 { "April" , 30 },
                 { "May" , 31 },
                 { "June" , 30 },
                 { "July" , 31 },
                 { "August", 31 },
                 { "September", 30 },
                 { "October" , 31 },
                 { "November" , 30 },
                 { "December", 31 }
            };
        }

        private List<string> GetYears()
        {
            var years = new List<string>();
            years.Add("Year");
            for (int i = 1990; i <= DateTime.UtcNow.Year; i++)
            {
                years.Add(i.ToString());
            }

            return years;
        }

        public List<string> GetDays()
        {
            var days = new List<string>()
            {
                "Day",
                "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13",
                "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26",
                "27", "28", "29", "30", "31"
            };

            return days;
        }

        private List<string> GetMonths()
        {
            var months = new List<string>();
            months.Add("Month");
            foreach (var item in MonthAndDays().Keys)
            {
                months.Add(item);
            }

            return months;
        }
    }
}
