using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class CallbackVm : IGtmEventRaisingVm
    {
        public string AccountHolderStatus { get; set; }

        public string CallbackTimeStatus { get; set; }

        public string LowellReferenceNumber { get; set; }

        public string FullName { get; set; }

        public string PreferredTelephoneNumber { get; set; }

        public IEnumerable<KeyValuePair<string, string>> AccountHolderStatuses { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CallbackOptions { get; set; }

        public DateTime? CallbackDate { get; set; }

        public List<KeyValuePair<string, string>> TimeSlotsSunday { get; set; }

        public List<KeyValuePair<string, string>> TimeSlotsToday { get; set; }

        public List<KeyValuePair<string, string>> TimeSlotsFirstAvailableDay { get; set; }

        public List<KeyValuePair<string, string>> TimeSlotsWeekday { get; set; }

        public List<KeyValuePair<string, string>> TimeSlotsSaturday { get; set; }

        public string TimeSlot { get; set; }

        public string TimeSlotSunday { get; set; }

        public string TimeSloFirstDay { get; set; }       

        public string TimeSlotWeekday { get; set; }

        public string TimeSlotSaturday { get; set; }

        public string CallmeNow { get; set; }       

        public bool HasCallbackRequestBeenSentSuccessfully { get; set; }

        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();

        public int SlotsAvailableForCurrentDay { get; set; }

        public string FirstAvailableDate { get; set; }

        public CallbackVm()
        {
            TimeSlotsSunday = new List<KeyValuePair<string, string>>();
            TimeSlotsToday = new List<KeyValuePair<string, string>>();           
            TimeSlotsWeekday = new List<KeyValuePair<string, string>>();
            TimeSlotsSaturday = new List<KeyValuePair<string, string>>();
        }
    }
}
