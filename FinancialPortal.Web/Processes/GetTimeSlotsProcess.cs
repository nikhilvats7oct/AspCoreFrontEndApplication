using System;
using System.Collections.Generic;
using System.Linq;
using FinancialPortal.Web.Processes.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class GetTimeSlotsProcess : IGetTimeSlotsProcess
    {
        private readonly ILogger<GetTimeSlotsProcess> _logger;

        public GetTimeSlotsProcess(ILogger<GetTimeSlotsProcess> logger)
        {
            _logger = logger;
        }

        public List<KeyValuePair<string, string>> Build(TimeSpan startTime, TimeSpan endTime)
        {
            string value = string.Empty;
            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>(null, "Time slot"));

            for (TimeSpan i = startTime; i < endTime; i += TimeSpan.FromHours(2))
            {
                value = DateTime.Today.Add(i).ToString("hh:mm tt") + " - "
                    + DateTime.Today.Add(i).Add(TimeSpan.FromHours(2)).ToString("hh:mm tt");
                list.Add(new KeyValuePair<string, string>(value, value));
            }
            return list;
        }

        public List<KeyValuePair<string, string>> Build(TimeSpan startTime, TimeSpan endTime, TimeSpan todayTime)
        {
            string value = string.Empty;
            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>(null, "Time slot"));

            for (TimeSpan i = startTime; i < endTime; i += TimeSpan.FromHours(2))
            {
                if (todayTime < i)
                {
                    value = DateTime.Today.Add(i).ToString("hh:mm tt") + " - "+ DateTime.Today.Add(i).Add(TimeSpan.FromHours(2)).ToString("hh:mm tt");
                    list.Add(new KeyValuePair<string, string>(value, value));
                }
            }
            return list;
        }

        public List<KeyValuePair<string, string>> GetSlots(DateTime startTime, DateTime endTime, DateTime filterTime, int slotDurationInHours)
        {
            var results = new List<KeyValuePair<DateTime, DateTime>>();

            while (startTime < endTime)
            {
                var slotEndtime = startTime.AddHours(slotDurationInHours);

                results.Add(new KeyValuePair<DateTime, DateTime>(startTime, slotEndtime));

                startTime  = startTime.AddHours(slotDurationInHours);
            }

            var list = results
                .Where(x => x.Key > filterTime)
                .Select(x => new
                 {
                     key = x.Key.ToString("hh:mm tt"),
                     value = x.Value.ToString("hh:mm tt")
                 })
                .AsEnumerable()
                .Select(o => new KeyValuePair<string, string>(o.key + " - " + o.value, o.key + " - " + o.value))
                .ToList();

            var returnList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(null, "Time slot")
            };

            returnList.AddRange(list);

            return returnList;
        }
    }
}

