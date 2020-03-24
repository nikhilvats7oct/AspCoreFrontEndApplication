using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
  public class BillsAndOutgoingsVm : IGtmEventRaisingVm
    {
        public OutgoingSourceVm Mortgage { get; set; }
        public OutgoingSourceVm Rent { get; set; }
        public OutgoingSourceVm SecuredLoan { get; set; }
        public OutgoingSourceVm CouncilTax { get; set; }
        public OutgoingSourceVm ApplianceOrFurnitureRental { get; set; }
        public OutgoingSourceVm TvLicense { get; set; }
        public OutgoingSourceVm Gas { get; set; }
        public OutgoingSourceVm OtherFuel { get; set; }
        public OutgoingSourceVm Electric { get; set; }
        public OutgoingSourceVm Water { get; set; }
        public OutgoingSourceVm ChildMaintenance { get; set; }
        public OutgoingSourceVm CourtFines { get; set; }
        public OutgoingSourceVm Ccjs { get; set; }
        public MonthlyIncomeVm IncomeSummary { get; set; }
        public MonthlyOutgoingsVm OutgoingSummary { get; set; }

        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();

        public bool EnabledPartialSave { get; set; }
        public bool PartialSavedEvent { get; set; }
        public bool PartialSavedIAndE { get; set; }
        public bool HasErrorPartialSavedIAndE { get; set; }

        public BillsAndOutgoingsVm()
        {
            Mortgage = new OutgoingSourceVm();
            Rent = new OutgoingSourceVm();
            SecuredLoan = new OutgoingSourceVm();
            CouncilTax = new OutgoingSourceVm();
            ApplianceOrFurnitureRental = new OutgoingSourceVm();
            TvLicense = new OutgoingSourceVm();
            Gas = new OutgoingSourceVm();
            OtherFuel = new OutgoingSourceVm();
            Electric = new OutgoingSourceVm();
            Water = new OutgoingSourceVm();
            ChildMaintenance = new OutgoingSourceVm();
            CourtFines = new OutgoingSourceVm();
            Ccjs = new OutgoingSourceVm();
            IncomeSummary = new MonthlyIncomeVm();
            OutgoingSummary = new MonthlyOutgoingsVm();
        }
    }
}