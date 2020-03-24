using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class ContactPreferencesVm : IGtmEventRaisingVm
    {
        public bool AllowContactByEmail { get; set; }

        public bool AllowContactBySms { get; set; }

        public string MobileNumber { get; set; }

        public string LowellReference { get; set; }

        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}
