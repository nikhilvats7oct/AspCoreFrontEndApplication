using System;

namespace FinancialPortal.Web.Settings
{
    public class CallbackSetting
    {      

        public TimeSpan TimeSlotWeekdayStartTime { get; set; }

        public TimeSpan TimeSlotWeekdayEndTime { get; set; }

        public TimeSpan TimeSlotSaturdayStartTime { get; set; }

        public TimeSpan TimeSlotSaturdayEndTime { get; set; }
    }
}
