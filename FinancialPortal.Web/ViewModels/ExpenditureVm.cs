using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class ExpendituresVm : IGtmEventRaisingVm
    {
        public ExpendituresVm()
        {
            FoodAndHouseKeeping = new ExpenditureSourceVm();
            PersonalCosts = new ExpenditureSourceVm();
            CommunicationsAndLeisure = new ExpenditureSourceVm();
            TravelAndTransport = new ExpenditureSourceVm();
            CareAndHealthCosts = new ExpenditureSourceVm();
            PensionsAndInsurance = new ExpenditureSourceVm();
            SchoolCosts = new ExpenditureSourceVm();
            Professional = new ExpenditureSourceVm();
            Savings = new ExpenditureSourceVm();
            Other = new ExpenditureSourceVm();
            OutgoingsVmSummary = new MonthlyOutgoingsVm();
            IncomeVmSummary = new MonthlyIncomeVm();
        }

        public ExpenditureSourceVm FoodAndHouseKeeping { get; set; }
        public decimal FoodAndHouseKeepingTriggerMin { get; set; }
        public decimal FoodAndHouseKeepingTriggerMax { get; set; }
        public ExpenditureSourceVm PersonalCosts { get; set; }
        public decimal PersonalCostsTriggerMin { get; set; }
        public decimal PersonalCostsTriggerMax { get; set; }
        public ExpenditureSourceVm CommunicationsAndLeisure { get; set; }
        public decimal CommunicationsAndLeisureTriggerMin { get; set; }
        public decimal CommunicationsAndLeisureTriggerMax { get; set; }
        public ExpenditureSourceVm TravelAndTransport { get; set; }
        public ExpenditureSourceVm CareAndHealthCosts { get; set; }
        public ExpenditureSourceVm PensionsAndInsurance { get; set; }
        public ExpenditureSourceVm SchoolCosts { get; set; }
        public ExpenditureSourceVm Professional { get; set; }
        public ExpenditureSourceVm Savings { get; set; }
        public ExpenditureSourceVm Other { get; set; }
        public MonthlyOutgoingsVm OutgoingsVmSummary { get; set; }
        public MonthlyIncomeVm IncomeVmSummary { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();

        public bool EnabledPartialSave { get; set; }
        public bool PartialSavedEvent { get; set; }
        public bool PartialSavedIAndE { get; set; }
        public bool HasErrorPartialSavedIAndE { get; set; }
    }
}