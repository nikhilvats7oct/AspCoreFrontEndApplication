using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IGetTimeSlotsProcess
    {
        List<KeyValuePair<string, string>> Build(TimeSpan startTime, TimeSpan endTime);

        List<KeyValuePair<string, string>> Build(TimeSpan startTime, TimeSpan endTime, TimeSpan todayTime);

        List<KeyValuePair<string, string>> GetSlots(DateTime startTime, DateTime endTime, DateTime filterTime, int slotDurationInHours);
    }
}
