using System.Collections.Generic;
using System.ComponentModel;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;
using Newtonsoft.Json;

namespace FinancialPortal.Web.ViewModels
{
    public class HouseholdStatusVm : IGtmEventRaisingVm
    {
        public HouseholdStatusVm()
        {
        }

        [DisplayName("What's your employment status?")]
        public string EmploymentStatus { get; set; }

        [DisplayName("What's your housing status?")]
        public string HousingStatus { get; set; }
        public int? AdultsInHousehold { get; set; }
        public int? ChildrenUnder16 { get; set; }
        public int? ChildrenOver16 { get; set; }

        public bool SavedIAndE { get; set; }

        public bool PartialSavedIAndE { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
        public bool ExternallyLaunched { get; set; }
    }
}