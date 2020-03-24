using System;
using System.Collections.Generic;
using FinancialPortal.Web.Processes.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class GetYearsForDoBProcess : IGetYearsForDoBProcess
    {
        private readonly ILogger<GetYearsForDoBProcess> _logger;

        public GetYearsForDoBProcess(ILogger<GetYearsForDoBProcess> logger)
        {
            _logger = logger;
        }

        public List<KeyValuePair<int?, string>> Build(int minYear, int maxYear)
        {
            if (minYear > maxYear)
                throw new ArgumentException("minYear must be <= maxYear");

            var list = new List<KeyValuePair<int?, string>>();
            list.Add(new KeyValuePair<int?, string>(null, "Year"));
            for (int i = maxYear; i >= minYear; i--)
            {
                list.Add(new KeyValuePair<int?, string>(i, i.ToString()));
            }

            return list;
        }
    }
}