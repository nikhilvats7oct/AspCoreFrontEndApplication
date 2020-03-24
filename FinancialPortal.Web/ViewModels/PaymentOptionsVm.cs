using System;
using System.Collections.Generic;
using FinancialPortal.Web.ViewModels.Base;
using Newtonsoft.Json;

namespace FinancialPortal.Web.ViewModels
{
    [Serializable]
    public class PaymentOptionsVm : GtmEventRaisingVm
    {
        //
        // Initial State of Payment Options Serialised and Encrypted
        // Used to maintain values that aren't in fields or hidden fields on postback
        //
        [JsonIgnore] // We should not be including this in any serialisation, otherwise content may grow over round trips
        public string InitialState { get; set; }

        //
        // Fields used in message banners
        //
        public string ArrearsMessage { get; set; } // null indicates not in arrears
        public bool PlanInPlace { get; set; }
        public bool PlanIsDirectDebit { get; set; }
        public bool HasNonDirectDebitPlanInPlace { get; set; }

        public bool DiscountBalanceAvailable { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountExpiryDate { get; set; }

        public string ExcludedAccountMessage { get; set; } // null indicates no excluded message

        public bool WithLowellSolicitors { get; set; }
        public string LowellSolicitorsRedirectLink { get; set; }


        //
        // Header information
        //
        public string ClientName { get; set; }
        public string LowellReference { get; set; }
        public decimal OutstandingBalance { get; set; }
        public bool DiscountedBalancePreviouslyAccepted { get; set; }
        public decimal DiscountedBalance { get; set; }

        //
        // Background fields required for scripts, validation etc.
        //
        public decimal FullPaymentBalance { get; set; }
        public decimal ProposedDiscountedBalanceIfAccepted { get; set; }

        //
        // Common Form fields
        //
        public bool DiscountAccepted { get; set; }

        public string SelectedPaymentOption { get; set; }
        public List<PaymentOptionsSelectionsVm> PaymentOptions { get; set; }

        public List<PaymentOptionsSourceOfFundsSelectionsVm> SourceOfFunds { get; set; }

        public bool IsAcceptTermsAndConditionsFieldVisible { get; set; }
        public bool AcceptTermsAndConditions { get; set; }

        // For verifone transactions
        public string VerifoneTransactionGuid { get; set; }

        //
        // Full payment fields
        //
        public string FullPaymentSelectedSourceOfFunds { get; set; }

        public string FullPaymentSourceOfFundsOtherText { get; set; }
        // uses shared SourceOfFunds list member

        public decimal FullPaymentAmountDerived
        {
            get
            {
                // If discount is offered and accepted by ticking box
                if (DiscountBalanceAvailable && DiscountAccepted)
                {
                    return ProposedDiscountedBalanceIfAccepted;
                }

                // Otherwise, falls back on either full balance or discounted balance
                // (if previously accepted a discount)
                return FullPaymentBalance;
            }
        }

        //
        // Partial Payment Fields
        //
        public decimal? PartialPaymentAmount { get; set; }
        public string PartialPaymentSelectedSourceOfFunds { get; set; }
        public string PartialPaymentSourceOfFundsOtherText { get; set; }

        //
        // Direct Debit Payment Fields
        //

        // Fields used for messages
        public bool StandingOrder { get; set; }
        public string StandingOrderMessage { get; set; }

        // Background fields (for scripts and validation)
        public DateTime DirectDebitStartDateEarliest { get; set; }
        public DateTime DirectDebitStartDateLatest { get; set; }

        public string DirectDebitSelectedFrequency { get; set; }
        public List<DirectDebitPaymentFrequencyVm> DirectDebitFrequency { get; set; }
        public string DirectDebitSelectedStartDate { get; set; }
        public decimal? DirectDebitAmount { get; set; }

        public bool DirectDebitIsEmailAddressFieldVisible { get; set; }
        public string DirectDebitEmailAddress { get; set; }

        public bool ShowPayOffDiscountedDDPlanWarning =>
            PlanInPlace && DiscountedBalancePreviouslyAccepted;

        public Guid LowellReferenceSurrogate { get; set; }
        public bool IandELessThanOrIs12MonthsOld { get; set; }
        public bool IandENotAvailable { get; set; }
        public PlanSetupOptions? SelectedPlanSetupOption { get; set; } = PlanSetupOptions.None;
        public decimal MonthlyDisposableIncome { get; set; }
        public decimal MonthlyDisposableIncomePerAccount { get; set; }
        public decimal AverageMonthlyPayment { get; set; }   
        public int AccountCount { get; set; }

        public string PlanMessage { get; set; }
    }

    public enum PlanSetupOptions
    {
        None,
        DisposableIncome,
        AverageSetupValue,
        OtherPaymentOffer
    }
}