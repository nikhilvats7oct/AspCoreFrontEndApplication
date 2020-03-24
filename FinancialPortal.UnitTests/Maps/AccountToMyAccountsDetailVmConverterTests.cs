using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class AccountToMyAccountsDetailVmConverterTests
    {
        private AccountToMyAccountsDetailVmConverter _converter;
        private PortalSetting _portalSettings;
        private Mock<IMapperHelper> _mapperHelper;
        private Mock<IMapper> _mapper;

        [TestInitialize]
        public void TestInitialise()
        {
            this._portalSettings = new PortalSetting();
            this._mapperHelper = new Mock<IMapperHelper>(MockBehavior.Strict);
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);

            this._converter = new AccountToMyAccountsDetailVmConverter(this._mapperHelper.Object, this._mapper.Object, this._portalSettings);
        }

        [TestMethod]
        public void ConvertTest_SourceNull()
        {
            Account source = null;
            MyAccountsDetailVm destination = new MyAccountsDetailVm();
            MyAccountsDetailVm expected = null;

            MyAccountsDetailVm result = _converter.Convert(source, destination, null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTest()
        {
            Account source = new Account()
            {
                AccountMessage = "AccountMessage",
                AccountReference = "AccountReference",
                AccountStatus = "AccountStatus",
                AccountStatusIsClosed = false,
                AccountStatusIsViewOnly = false,
                AccountStatusIsWithSolicitors = false,
                AccountStatusSort = 1,
                AddedSinceLastLogin = true,
                CanAmendPlan = true,
                CanMakePayment = true,
                CanManageAccount = true,
                ClientReference = "ClientReference",
                DirectDebitInFlight = false,
                DiscountedBalance = 111.11M,
                DiscountOfferAmount = 222.22M,
                DiscountOfferExpiry = DateTime.Now.AddDays(30),
                HasArrangement = true,
                NeverAllowPlanTransfer = false,
                NextPlanPaymentDate = DateTime.Now.AddDays(7),
                OriginalCompany = "OriginalCompany",
                OutstandingBalance = 555.55M,
                PaymentPlanAmount = 50,
                PaymentPlanArrearsAmount = 100,
                PaymentPlanFrequency = "monthly",
                PaymentPlanIsAutomated = true,
                PaymentPlanMethod = "direct debit",
                PlanPendingTransfer = false,
                PlanTransferredFrom = "PlanTransferredFrom",
                PlanMessages = new string[] { "Plan Message 1", "Plan Message 2" },   
                RecentTransactions = new List<Transaction>()
                {
                    new Transaction(){ Amount = 23.50M, Description = "Transaction 1", Type = "credit", RollingBalance = 550.00M, Date = DateTime.Now.AddDays(-150) },
                    new Transaction(){ Amount = 60.50M, Description = "Transaction 2", Type = "credit", RollingBalance = 500.00M, Date = DateTime.Now.AddDays(-120) },
                    new Transaction(){ Amount = 34.50M, Description = "Transaction 3", Type = "credit", RollingBalance = 450.00M, Date = DateTime.Now.AddDays(-90) },
                    new Transaction(){ Amount = 67.50M, Description = "Transaction 4", Type = "credit", RollingBalance = 400.00M, Date = DateTime.Now.AddDays(-60) },
                    new Transaction(){ Amount = 54.50M, Description = "Transaction 5", Type = "credit", RollingBalance = 350.00M, Date = DateTime.Now.AddDays(-30) },
                }
            };

            MyAccountsDetailVm destination = new MyAccountsDetailVm();

            MyAccountsDetailVm expected = new MyAccountsDetailVm()
            {
                PlanDescription = "PlanDescription",
                DiscountAvailableLinkTextOptional = "DiscountAvailableLinkTextOptional",
                AccountMessage = "AccountMessage",
                ArrearsMessage = "ArrearsMessage",
                RecentTransactions = new List<TransactionVm>()
                {
                    new TransactionVm(){ AmountText = "£23.50", Description = "Transaction 1", RollingBalanceText = "£550.00", DateText = DateTime.Now.AddDays(-150).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                    new TransactionVm(){ AmountText = "£60.50", Description = "Transaction 1", RollingBalanceText = "£500.00", DateText = DateTime.Now.AddDays(-120).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                    new TransactionVm(){ AmountText = "£34.50", Description = "Transaction 1", RollingBalanceText = "£450.00", DateText = DateTime.Now.AddDays(-90).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                    new TransactionVm(){ AmountText = "£67.50", Description = "Transaction 1", RollingBalanceText = "£400.00", DateText = DateTime.Now.AddDays(-60).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                    new TransactionVm(){ AmountText = "£54.50", Description = "Transaction 1", RollingBalanceText = "£350.00", DateText = DateTime.Now.AddDays(-30).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                },
                OriginalCompanyText = "OriginalCompany",
                AccountReferenceText = "AccountReference",
                OutstandingBalanceText = "£555.55",
                AccountStatusText = "AccountStatus",
                NextPlanPaymentDate = source.NextPlanPaymentDate,
                ClientReference = "ClientReference",
                AddedSinceLastLogin = true,
                HasArrangement = true,
                DiscountedBalanceTextOptional = "£111.11",
                IsPaymentButtonAvailable = true,
                CanAmendPlan = true,
                DirectDebitInFlight = false,
                AccountWithSolicitors = false,
                PlanMessage = "Plan Message 1",
                PlanPendingTransfer = false,
                PlanTransferredFrom = "PlanTransferredFrom",
                PlanTransferredFromAccounts = new List<string>() { "PlanTransferredFrom" },
                NeverAllowPlanTransfer = false,
                PlanTransferredFromAccountsFormatted = "DerivePlanTransferredFromAccountsFormatted",
                PlanTransferOptOutNumber = "PlanTransferOptOutNumber"
            };

            _mapperHelper.Setup(x => x.DerivePlanDescription(source)).Returns("PlanDescription");
            _mapperHelper.Setup(x => x.DeriveDiscountDescription(source)).Returns("DiscountAvailableLinkTextOptional");
            _mapperHelper.Setup(x => x.DeriveArrearsDetail(source.PaymentPlanArrearsAmount, source.PaymentPlanIsAutomated)).Returns("ArrearsMessage");
            _mapperHelper.Setup(x => x.DerivePlanTransferredFromAccountsFormatted(
                It.Is<List<string>>(a => a != null && a.Count == 1 && a[0] == "PlanTransferredFrom")))
                .Returns("DerivePlanTransferredFromAccountsFormatted");
            _portalSettings.SolicitorsRedirectUrl = "SolicitorsRedirectUrl";
            _portalSettings.PlanTransferOptOutNumber = "PlanTransferOptOutNumber";

            _mapper.Setup(x => x.Map<List<Transaction>, List<TransactionVm>>(source.RecentTransactions)).Returns(expected.RecentTransactions);

            MyAccountsDetailVm result = _converter.Convert(source, destination, null);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            Account source = new Account()
            {
                AccountMessage = "AccountMessage",
                AccountReference = "AccountReference",
                AccountStatus = "AccountStatus",
                AccountStatusIsClosed = false,
                AccountStatusIsViewOnly = false,
                AccountStatusIsWithSolicitors = false,
                AccountStatusSort = 1,
                AddedSinceLastLogin = true,
                CanAmendPlan = true,
                CanMakePayment = true,
                CanManageAccount = true,
                ClientReference = "ClientReference",
                DirectDebitInFlight = false,
                DiscountedBalance = 111.11M,
                DiscountOfferAmount = 222.22M,
                DiscountOfferExpiry = DateTime.Now.AddDays(30),
                HasArrangement = true,
                NeverAllowPlanTransfer = false,
                NextPlanPaymentDate = DateTime.Now.AddDays(7),
                OriginalCompany = "OriginalCompany",
                OutstandingBalance = 555.55M,
                PaymentPlanAmount = 50,
                PaymentPlanArrearsAmount = 100,
                PaymentPlanFrequency = "monthly",
                PaymentPlanIsAutomated = true,
                PaymentPlanMethod = "direct debit",
                PlanPendingTransfer = false,
                PlanTransferredFrom = "11111111,22222222,33333333",
                PlanMessages = new string[] { "Plan Message 1", "Plan Message 2" },
                RecentTransactions = new List<Transaction>()
                {
                    new Transaction(){ Amount = 23.50M, Description = "Transaction 1", Type = "credit", RollingBalance = 550.00M, Date = DateTime.Now.AddDays(-150) },
                    new Transaction(){ Amount = 60.50M, Description = "Transaction 2", Type = "credit", RollingBalance = 500.00M, Date = DateTime.Now.AddDays(-120) },
                    new Transaction(){ Amount = 34.50M, Description = "Transaction 3", Type = "credit", RollingBalance = 450.00M, Date = DateTime.Now.AddDays(-90) },
                    new Transaction(){ Amount = 67.50M, Description = "Transaction 4", Type = "credit", RollingBalance = 400.00M, Date = DateTime.Now.AddDays(-60) },
                    new Transaction(){ Amount = 54.50M, Description = "Transaction 5", Type = "credit", RollingBalance = 350.00M, Date = DateTime.Now.AddDays(-30) },
                }
            };

            MyAccountsDetailVm destination = null;

            MyAccountsDetailVm expected = new MyAccountsDetailVm()
            {
                PlanDescription = "PlanDescription",
                DiscountAvailableLinkTextOptional = "DiscountAvailableLinkTextOptional",
                AccountMessage = "AccountMessage",
                ArrearsMessage = "ArrearsMessage",
                RecentTransactions = new List<TransactionVm>()
                {
                    new TransactionVm(){ AmountText = "£23.50", Description = "Transaction 1", RollingBalanceText = "£550.00", DateText = DateTime.Now.AddDays(-150).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                    new TransactionVm(){ AmountText = "£60.50", Description = "Transaction 1", RollingBalanceText = "£500.00", DateText = DateTime.Now.AddDays(-120).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                    new TransactionVm(){ AmountText = "£34.50", Description = "Transaction 1", RollingBalanceText = "£450.00", DateText = DateTime.Now.AddDays(-90).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                    new TransactionVm(){ AmountText = "£67.50", Description = "Transaction 1", RollingBalanceText = "£400.00", DateText = DateTime.Now.AddDays(-60).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                    new TransactionVm(){ AmountText = "£54.50", Description = "Transaction 1", RollingBalanceText = "£350.00", DateText = DateTime.Now.AddDays(-30).ToString("dd MMM y", CultureInfo.CurrentCulture) },
                },
                OriginalCompanyText = "OriginalCompany",
                AccountReferenceText = "AccountReference",
                OutstandingBalanceText = "£555.55",
                AccountStatusText = "AccountStatus",
                NextPlanPaymentDate = source.NextPlanPaymentDate,
                ClientReference = "ClientReference",
                AddedSinceLastLogin = true,
                HasArrangement = true,
                DiscountedBalanceTextOptional = "£111.11",
                IsPaymentButtonAvailable = true,
                CanAmendPlan = true,
                DirectDebitInFlight = false,
                AccountWithSolicitors = false,
                PlanMessage = "Plan Message 1",
                PlanPendingTransfer = false,
                PlanTransferredFrom = "11111111,22222222,33333333",
                PlanTransferredFromAccounts = new List<string>() { "11111111", "22222222", "33333333" },
                NeverAllowPlanTransfer = false,
                PlanTransferredFromAccountsFormatted = "DerivePlanTransferredFromAccountsFormatted",
                PlanTransferOptOutNumber = "PlanTransferOptOutNumber"
            };

            _mapperHelper.Setup(x => x.DerivePlanDescription(source)).Returns("PlanDescription");
            _mapperHelper.Setup(x => x.DeriveDiscountDescription(source)).Returns("DiscountAvailableLinkTextOptional");
            _mapperHelper.Setup(x => x.DeriveArrearsDetail(source.PaymentPlanArrearsAmount, source.PaymentPlanIsAutomated)).Returns("ArrearsMessage");
            _portalSettings.SolicitorsRedirectUrl = "SolicitorsRedirectUrl";
            _mapper.Setup(x => x.Map<List<Transaction>, List<TransactionVm>>(source.RecentTransactions)).Returns(expected.RecentTransactions);
            _mapperHelper.Setup(x => x.DerivePlanTransferredFromAccountsFormatted(
                It.Is<List<string>>(a => a != null && a.Count == 3 && a[0] == "11111111" && a[1] == "22222222" && a[2] == "33333333")))
                .Returns("DerivePlanTransferredFromAccountsFormatted");
            _portalSettings.PlanTransferOptOutNumber = "PlanTransferOptOutNumber";

            MyAccountsDetailVm result = _converter.Convert(source, destination, null);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

    }
}
