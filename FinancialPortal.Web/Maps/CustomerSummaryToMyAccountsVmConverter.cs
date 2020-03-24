using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class CustomerSummaryToMyAccountsVmConverter : ITypeConverter<CustomerSummary, MyAccountsVm>
    {
        private readonly IMapperHelper _mapperHelper;
        private readonly PortalSetting _portalSettings;

        public CustomerSummaryToMyAccountsVmConverter(IMapperHelper mapperHelper, PortalSetting portalSettings)
        {
            _mapperHelper = mapperHelper;
            _portalSettings = portalSettings;
        }

        public MyAccountsVm Convert(CustomerSummary source, MyAccountsVm destination, ResolutionContext context)
        {
            if (source == null || source.Accounts == null || source.SurrogateKeysByLowellReference == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new MyAccountsVm();
            }           

            Guid? lowellFinancialAccountSurrogateKey = null;
            destination.Accounts = new List<AccountSummaryVm>();
            foreach (AccountSummary account in source.Accounts)
            {
                AccountSummaryVm accountVm = new AccountSummaryVm();

                Guid lowellReferenceSurrogateKey = source.SurrogateKeysByLowellReference[account.AccountReference];
                
                accountVm.PlanDescription = _mapperHelper.DerivePlanDescription(account);
                accountVm.DiscountAvailableLinkTextOptional = _mapperHelper.DeriveDiscountDescription(account);

                if (account.CanManageAccount)
                {
                    accountVm.DetailsLinkText = "Manage account";
                }
                else
                {
                    accountVm.DetailsLinkText = "View";
                }

                accountVm.ArrearsMessage = _mapperHelper.DeriveArrearsSummary(account.PaymentPlanArrearsAmount);

                if (!account.AccountStatusIsWithSolicitors && lowellFinancialAccountSurrogateKey == null)
                {
                    lowellFinancialAccountSurrogateKey = lowellReferenceSurrogateKey;
                }

                accountVm.Class = "info-box";

                if (account.AccountStatusIsClosed)
                {
                    accountVm.Class = "info-box--disabled";
                }

                if (accountVm.ArrearsMessage != null)
                {
                    accountVm.Class = "info-box--warning";
                    accountVm.ShowWarningSymbol = true;
                }

                accountVm.AccountStatusIsWithSolicitors = account.AccountStatusIsWithSolicitors;
                accountVm.LowellReferenceSurrogateKey = lowellReferenceSurrogateKey;

                accountVm.OriginalCompanyText = account.OriginalCompany;
                accountVm.AccountReferenceText = account.AccountReference;
                accountVm.OutstandingBalanceText = account.OutstandingBalance.ToString("C", CultureInfo.CurrentCulture);
                accountVm.AccountStatusText = account.AccountStatus;
                accountVm.NextPlanPaymentDate = account.NextPlanPaymentDate;
                accountVm.ClientReference = account.ClientReference;
                accountVm.AddedSinceLastLogin = account.AddedSinceLastLogin;
                accountVm.HasArrangement = account.HasArrangement;
                accountVm.PlanPendingTransfer = account.PlanPendingTransfer;
                accountVm.PlanTransferredFrom = account.PlanTransferredFrom;

                if (!string.IsNullOrEmpty(accountVm.PlanTransferredFrom))
                {
                    accountVm.PlanTransferredFromAccounts = accountVm.PlanTransferredFrom.Split(',').ToList();
                    accountVm.PlanTransferredFromMessage =
                        _mapperHelper.DerivePlanTransferredFromMessage(accountVm.PlanTransferredFromAccounts);
                }

                accountVm.NeverAllowPlanTransfer = account.NeverAllowPlanTransfer;

                if (account.DiscountedBalance != null)
                {
                    accountVm.DiscountedBalanceTextOptional = account.DiscountedBalance.Value.ToString("C", CultureInfo.CurrentCulture);
                }

                if (account.AccountStatusIsWithSolicitors)
                {
                    accountVm.SolicitorsRedirectLink = _portalSettings.SolicitorsRedirectUrl;
                }

                accountVm.IsPaymentButtonAvailable = account.CanMakePayment;
                accountVm.CanAmendPlan = account.CanAmendPlan;

                if (account.AccountStatusIsViewOnly)
                {
                    accountVm.DiscountAvailableLinkTextOptional = null;
                }

                if (account.AccountStatusIsClosed)
                {
                    accountVm.ArrearsMessage = null;
                    accountVm.DiscountAvailableLinkTextOptional = null;
                    accountVm.IsPaymentButtonAvailable = false;
                    accountVm.CanAmendPlan = false;
                }

                destination.Accounts.Add(accountVm);
            }

            destination.LowellFinancialAccountSurrogateKey = lowellFinancialAccountSurrogateKey;

            if (source.IncomeAndExpenditure != null)
            {
                destination.IncomeAndExpenditureSubmitted = true;
                destination.IncomeAndExpenditureExpired = source.IncomeAndExpenditure.Created.AddMonths(6).Date < DateTime.Now.Date;
                destination.IandELessThanOrIs12MonthsOld = source.IncomeAndExpenditure.Created.AddMonths(12).Date >= DateTime.Now.Date;
            }

            destination.IncomeAndExpenditureSubmitted = source.IncomeAndExpenditure != null;
            destination.NewAccounts = destination.Accounts.Where(x => x.AddedSinceLastLogin && !x.HasArrangement).ToList();
            destination.DisposableIncome = source.IncomeAndExpenditure?.DisposableIncome;

            //Sort accounts so that any with plans pending transfer or have had plans transferred to are first
            destination.Accounts = destination.Accounts
                .OrderByDescending(x => x.PlanPendingTransfer || !String.IsNullOrEmpty(x.PlanTransferredFrom)).ToList();

            return destination;
        }
    }
}
