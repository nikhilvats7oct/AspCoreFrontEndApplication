using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.ViewModels
{
    public class AccountSummaryVm
    {
        public AccountSummaryVm()
        {
            this.PlanTransferredFromAccounts = new List<string>(); 
        }

        // Used in hyperlinks to avoid having Lowell Reference in URLS
        public Guid LowellReferenceSurrogateKey { get; set; }

        //
        // Text for elements is exposed. May be null in some cases (suffixed Optional), suppressing output.
        //
        public string OriginalCompanyText { get; set; }
        public string AccountReferenceCaption => "Your Reference: ";
        public string AccountReferenceText { get; set; }
        public string OutstandingBalanceCaption => "Balance: ";
        public string OutstandingBalanceText { get; set; }
        public string DiscountedBalanceCaption => DiscountedBalanceTextOptional == null ? null : "Discounted Balance: ";
        public string DiscountedBalanceTextOptional { get; set; }
        public string AccountStatusText { get; set; }           // null indicates 'Open'
        public string PlanDescription { get; set; }
        public DateTime? NextPlanPaymentDate { get; set; }
        public string ArrearsMessage { get; set; }              // null indicates not in arrears
        public string Class { get; set; }       // style applied to the whole account panel
        public bool ShowWarningSymbol { get; set; }
        public string DetailsLinkText { get; set; }
        public bool IsDiscountLinkAvailable => DiscountAvailableLinkTextOptional != null;
        public string DiscountAvailableLinkTextOptional { get; set; }
        public bool IsSolicitorsRedirectLinkAvailable => SolicitorsRedirectLink != null;
        public string SolicitorsRedirectLink { get; set; }
        public bool IsPaymentButtonAvailable { get; set; }
        public bool CanAmendPlan { get; set; }
        public bool AccountStatusIsWithSolicitors { get; set; }
        public string ClientReference { get; set; }
        public bool AddedSinceLastLogin { get; set; }
        public bool HasArrangement { get; set; }
        public string ClassBottom // style applied to panel containing payment button and discount offer
            => IsDiscountLinkAvailable ? "info - box__bottom--discount" : null;
        public bool NeverAllowPlanTransfer { get; set; }
        public bool PlanPendingTransfer { get; set; }
        public string PlanTransferredFrom { get; set; }
        public List<string> PlanTransferredFromAccounts { get; set; } 
        public string PlanTransferredFromMessage { get; set; }
    }
}
