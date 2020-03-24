using System.Collections.Generic;
using FinancialPortal.Web.Processes.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class GetMonthsForDoBProcess : IGetMonthsForDoBProcess
    {
        private readonly ILogger<GetMonthsForDoBProcess> _logger;

        public GetMonthsForDoBProcess(ILogger<GetMonthsForDoBProcess> logger)
        {
            _logger = logger;
        }

        public List<KeyValuePair<string, string>> Build()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(null, "Month"),
                MonthKeyValuePair("January"),
                MonthKeyValuePair("February"),
                MonthKeyValuePair("March"),
                MonthKeyValuePair("April"),
                MonthKeyValuePair("May"),
                MonthKeyValuePair("June"),
                MonthKeyValuePair("July"),
                MonthKeyValuePair("August"),
                MonthKeyValuePair("September"),
                MonthKeyValuePair("October"),
                MonthKeyValuePair("November"),
                MonthKeyValuePair("December")
            };

            return list;
        }

        KeyValuePair<string, string> MonthKeyValuePair(string monthName)
        {
            return new KeyValuePair<string, string>(monthName, monthName);
        }
    }
}