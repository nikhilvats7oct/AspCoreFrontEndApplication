using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;
using FinancialPortal.Web.ViewModels.Base;

namespace FinancialPortal.Web.ViewModels
{
    public class IncomeVm : IGtmEventRaisingVm
    {
        public IncomeVm()
        {
            Earning = new IncomeSourceVm();
            BenefitsAndTaxCredits = new IncomeSourceVm();
            Pension = new IncomeSourceVm();
            Other = new IncomeSourceVm();
        }

        public IncomeSourceVm Earning { get; set; }
        public IncomeSourceVm BenefitsAndTaxCredits { get; set; }
        public IncomeSourceVm Pension { get; set; }
        public IncomeSourceVm Other { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();


        public bool EnabledPartialSave { get; set; }
        public bool PartialSavedEvent { get; set; }
        public bool PartialSavedIAndE { get; set; }
        public bool HasErrorPartialSavedIAndE { get; set; }
    }
}