using System;

namespace FinancialPortal.Web.DataTransferObjects
{
    public class CallbackDetailsDto
    {
        public string AccountHolderStatus { get; set; }        

        public string LowellReferenceNumber { get; set; }

        public string FullName { get; set; }

        public string PreferredTelephoneNumber { get; set; }        

        public DateTime CallbackDate { get; set; }

        public string TimeSlot { get; set; }

        public bool CallmeNow { get; set; }
    }
}
