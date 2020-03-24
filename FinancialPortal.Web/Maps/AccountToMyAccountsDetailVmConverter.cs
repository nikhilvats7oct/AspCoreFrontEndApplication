using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class AccountToMyAccountsDetailVmConverter : ITypeConverter<Account, MyAccountsDetailVm>
    {
        private readonly IMapperHelper _mapperHelper;
        private readonly IMapper _mapper;
        private readonly PortalSetting _portalSettings;
        public AccountToMyAccountsDetailVmConverter(IMapperHelper mapperHelper, IMapper mapper, PortalSetting portalSettings)
        {
            _mapperHelper = mapperHelper;
            _mapper = mapper;
            _portalSettings = portalSettings;
        }

        public MyAccountsDetailVm Convert(Account source, MyAccountsDetailVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new MyAccountsDetailVm();
            }

            destination.PlanDescription = _mapperHelper.DerivePlanDescription(source);
            destination.DiscountAvailableLinkTextOptional = _mapperHelper.DeriveDiscountDescription(source);

            if (!string.IsNullOrEmpty(source.AccountMessage))
            {
                destination.AccountMessage = source.AccountMessage;
            }

            destination.ArrearsMessage = _mapperHelper.DeriveArrearsDetail(source.PaymentPlanArrearsAmount, source.PaymentPlanIsAutomated);
            destination.RecentTransactions = _mapper.Map<List<Transaction>, List<TransactionVm>>(source.RecentTransactions);

            destination.OriginalCompanyText = source.OriginalCompany;
            destination.AccountReferenceText = source.AccountReference;
            destination.OutstandingBalanceText = source.OutstandingBalance.ToString("C", CultureInfo.CurrentCulture);
            destination.AccountStatusText = source.AccountStatus;
            destination.NextPlanPaymentDate = source.NextPlanPaymentDate;
            destination.ClientReference = source.ClientReference;
            destination.AddedSinceLastLogin = source.AddedSinceLastLogin;
            destination.HasArrangement = source.HasArrangement;
            destination.PlanPendingTransfer = source.PlanPendingTransfer;
            destination.PlanTransferredFrom = source.PlanTransferredFrom;
            destination.NeverAllowPlanTransfer = source.NeverAllowPlanTransfer;
            destination.PlanTransferOptOutNumber = _portalSettings.PlanTransferOptOutNumber;

            if (!string.IsNullOrEmpty(destination.PlanTransferredFrom))
            {
                destination.PlanTransferredFromAccounts = destination.PlanTransferredFrom.Split(',').ToList();
                destination.PlanTransferredFromAccountsFormatted = 
                    _mapperHelper.DerivePlanTransferredFromAccountsFormatted(destination.PlanTransferredFromAccounts);
            }

            if (source.DiscountedBalance != null)
            {
                destination.DiscountedBalanceTextOptional = source.DiscountedBalance.Value.ToString("C", CultureInfo.CurrentCulture);
            }

            if (source.AccountStatusIsWithSolicitors)
            {
                destination.SolicitorsRedirectLink = _portalSettings.SolicitorsRedirectUrl;
            }

            destination.IsPaymentButtonAvailable = source.CanMakePayment;
            destination.CanAmendPlan = source.CanAmendPlan;

            if (source.AccountStatusIsViewOnly)
            {
                destination.DiscountAvailableLinkTextOptional = null;
            }

            if (source.AccountStatusIsClosed)
            {
                destination.ArrearsMessage = null;
                destination.DiscountAvailableLinkTextOptional = null;
                destination.IsPaymentButtonAvailable = false;
                destination.CanAmendPlan = false;
            }

            destination.DirectDebitInFlight = source.DirectDebitInFlight;
            destination.AccountWithSolicitors = source.AccountStatusIsWithSolicitors;
            destination.PlanMessage = (source.PlanMessages != null && source.PlanMessages.Length > 0)
                ? source.PlanMessages[0]
                : string.Empty;

            return destination;
        }
    }
}
