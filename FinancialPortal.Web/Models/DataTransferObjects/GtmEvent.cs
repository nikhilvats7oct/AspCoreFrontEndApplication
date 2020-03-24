using System;
using Newtonsoft.Json;

namespace FinancialPortal.Web.Models.DataTransferObjects
{
    [Serializable]
    public class GtmEvent
    {
        public string account_ref { get; set; }
        public string guid { get; set; }
        public string result { get; set; }
        public string error_message { get; set; }

        [JsonProperty(PropertyName = "event")]
        public string gtm_event { get; set; }
        public string step { get; set; }
        public string action_taken { get; set; }
        public string payment_type { get; set; }
        public decimal? payment_amount { get; set; } 
        public string discount_available { get; set; }
        public string balance_selected { get; set; }
        public DateTime? instalment_start_date { get; set; }
        public string user_status { get; set; }
        public string plan_status { get; set; }
        public string plan_type { get; set; }
        public string payment_detail { get; set; }
        public decimal monthly_income { get; set; }
        public int? dependents { get; set; }
        public double? priority_household_bills { get; set; }
        public double? other_priority_expenditure { get; set; }
        public double? other_household_bills { get; set; }
        public double? travel { get; set; }
        public double? other_outgoings { get; set; }        
        public decimal monthly_disposable_income { get; set; }
        public double? pay_predictor_balance { get; set; }
        public int? pay_predictor_number_of_instalments { get; set; }
        public double? pay_predictor_payment_amount { get; set; }
        public double? pay_predictor_instalment_period { get; set; }
        public string source_of_funds { get; set; }
        public string employment_status { get; set; }
        public string housing_status { get; set; }       
        public decimal monthly_expenses { get; set; }
        public string payment_option_chosen { get; set; }
    }
}
