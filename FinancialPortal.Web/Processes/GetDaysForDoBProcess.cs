using System.Collections.Generic;
using FinancialPortal.Web.Processes.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class GetDaysForDoBProcess : IGetDaysForDoBProcess
    {
        private readonly ILogger<GetDaysForDoBProcess> _logger;

        public GetDaysForDoBProcess(ILogger<GetDaysForDoBProcess> logger)
        {
            _logger = logger;
        }

        public List<KeyValuePair<int?, string>> Build()
        {
            var list = new List<KeyValuePair<int?, string>>();
            list.Add(new KeyValuePair<int?, string>(null, "Day"));
            for (int i = 1; i < 32; i++)
            {
                list.Add(new KeyValuePair<int?, string>(i, i.ToString()));
            }
            return list;
        }
    }
}