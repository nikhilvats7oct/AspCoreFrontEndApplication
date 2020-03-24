using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class MyAccountsDetailVm : IGtmEventRaisingVm
    {
        public MyAccountsDetailVm()
        {
            PlanTransferredFromAccounts = new List<string>();
        }

        public Guid LowellReferenceSurrogateKey { get; set; }
        public string OriginalCompanyText { get; set; }
        public string AccountReferenceCaption => "Your Reference: ";
        public string AccountReferenceText { get; set; }
        public string OutstandingBalanceCaption => "Balance: ";
        public string OutstandingBalanceText { get; set; }
        public bool IsDiscountAccepted => DiscountedBalanceTextOptional != null;
        public string DiscountedBalanceCaption => DiscountedBalanceTextOptional == null ? null : "Discounted Balance: ";
        public string DiscountedBalanceTextOptional { get; set; }
        public string AccountStatusText { get; set; }           // null indicates 'Open'
        public string PlanDescription { get; set; }
        public string ArrearsMessage { get; set; }              // null indicates not in arrears
        public string AccountMessage { get; set; }
        public bool IsDiscountLinkAvailable => DiscountAvailableLinkTextOptional != null;
        public string DiscountAvailableLinkTextOptional { get; set; }
        public bool IsSolicitorsRedirectLinkAvailable => SolicitorsRedirectLink != null;
        public string SolicitorsRedirectLink { get; set; }
        public bool AccountWithSolicitors { get; set; }
        public bool IsPaymentButtonAvailable { get; set; }
        public string LoggedInUserId { get; set; }
        public string LoggedInLowellRef { get; set; }
        public List<TransactionVm> RecentTransactions { get; set; }
        public bool CanAmendPlan { get; set; }
        public DateTime? NextPlanPaymentDate { get; set; }
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
        public bool DirectDebitInFlight { get; set; }
        public string ClientReference { get; set; }
        public bool AddedSinceLastLogin { get; set; }
        public bool HasArrangement { get; set; }
        public string PlanMessage { get; set; }
        public bool NeverAllowPlanTransfer { get; set; }
        public bool PlanPendingTransfer { get; set; }
        public string PlanTransferredFrom { get; set; }
        public List<string> PlanTransferredFromAccounts { get; set; }
        public string PlanTransferOptOutNumber { get; set; }
        public string PlanTransferredFromAccountsFormatted { get; set; }
    }
}
