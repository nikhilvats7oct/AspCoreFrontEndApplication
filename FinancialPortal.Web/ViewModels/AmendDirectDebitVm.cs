using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;
using Newtonsoft.Json;

namespace FinancialPortal.Web.ViewModels
{
    [Serializable]
    public class AmendDirectDebitVm : IGtmEventRaisingVm
    {
        //
        // Initial State of Payment Options Serialised and Encrypted
        // Used to maintain values that aren't in fields or hidden fields on postback
        //
        [JsonIgnore] // We should not be including this in any serialisation, otherwise content may grow over round trips
        public string InitialState { get; set; }

        public string LowellReference { get; set; }
        public string ClientName { get; set; }
        public decimal OutstandingBalance { get; set; }
        public string PlanType { get; set; }
        public DateTime EarliestStartDate { get; set; }
        public DateTime LatestStartDate { get; set; }
        public List<DirectDebitPaymentFrequencyVm> Frequency { get; set; }

        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime? DiscountExpiry { get; set; }
        public decimal DiscountedBalance { get; set; }

        /*This will change on amend*/
        public decimal? DirectDebitAmount { get; set; }
        public string PlanFrequency { get; set; }
        public DateTime PlanStartDate { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();

        public bool IandENotAvailable { get; set; }
        public bool IandELessThanOrIs12MonthsOld { get; set; }

        public decimal MonthlyDisposableIncome { get; set; }
        public decimal MonthlyDisposableIncomePerAccount { get; set; }
        public decimal AverageMonthlyPayment { get; set; }
        public PlanSetupOptions? SelectedPlanSetupOption { get; set; } = PlanSetupOptions.None;
        public int AccountCount { get; set; }
        public bool DiscountedBalancePreviouslyAccepted { get; set; }
        public decimal DerivedBalance
        {
            get
            {
                if (DiscountedBalancePreviouslyAccepted)
                {
                    return DiscountedBalance;
                }
                else
                {
                    return OutstandingBalance;
                }
            }
        }
    }
}