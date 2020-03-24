using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;
using Newtonsoft.Json;

namespace FinancialPortal.Web.ViewModels
{
    [Serializable]
    public class DirectDebitDetailsVm : IGtmEventRaisingVm
    {
        //
        // Initial State of Direct Debit Details Serialised and Encrypted
        // Used to maintain values that aren't in fields or hidden fields on postback
        //
        [JsonIgnore] // We should not be including this in any serialisation, otherwise content may grow over round trips
        public string InitialState { get; set; }

        // State of Payment Options used prior to moving to Direct Debit
        public string PaymentOptionsFilledInState { get; set; }

        public string MessageForUser { get; set; }

        public string AccountHoldersName { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public bool AcceptDirectDebitGuarantee { get; set; }
        public string LowellRef { get; set; }
        public bool DiscountAvailable { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string PaymentFrequency { get; set; }
        public bool DiscountSelected { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
        public PlanSetupOptions? SelectedPlanSetupOption { get; set; }
    }
}