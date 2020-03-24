using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class CustomerSummaryToMyAccountsVmConverterTests
    {
        private CustomerSummaryToMyAccountsVmConverter _converter;
        private PortalSetting _portalSettings;
        private Mock<IMapperHelper> _mapperHelper;

        [TestInitialize]
        public void TestInitialise()
        {
            this._portalSettings = new PortalSetting();
            this._mapperHelper = new Mock<IMapperHelper>(MockBehavior.Strict);

            this._converter = new CustomerSummaryToMyAccountsVmConverter(this._mapperHelper.Object, this._portalSettings);
        }

        [TestMethod]
        public void ConvertTest_SourceNull()
        {
            CustomerSummary source = null;
            MyAccountsVm destination = new MyAccountsVm();
            MyAccountsVm expected = null;

            MyAccountsVm result = _converter.Convert(source, destination, null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTest_SourceAccountsNull()
        {
            CustomerSummary source = new CustomerSummary()
            {
                Accounts = null,
                IncomeAndExpenditure = null,
                SurrogateKeysByLowellReference = new Dictionary<string, Guid>()
            };

            MyAccountsVm destination = new MyAccountsVm();
            MyAccountsVm expected = null;

            MyAccountsVm result = _converter.Convert(source, destination, null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTest_SourceSurrogateKeysByLowellReferenceNull()
        {
            CustomerSummary source = new CustomerSummary()
            {
                Accounts = new List<AccountSummary>(),
                IncomeAndExpenditure = null,
                SurrogateKeysByLowellReference = null
            };

            MyAccountsVm destination = new MyAccountsVm();
            MyAccountsVm expected = null;

            MyAccountsVm result = _converter.Convert(source, destination, null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTest()
        {
            Dictionary<string, Guid> SurrogateKeysByLowellReference = new Dictionary<string, Guid>()
            {
                { "11111111", Guid.NewGuid() },
                { "22222222", Guid.NewGuid() },
                { "33333333", Guid.NewGuid() }
            };

            CustomerSummary source = new CustomerSummary()
            {
                Accounts = new List<AccountSummary>()
                {
                    new AccountSummary()
                    {
                        AccountReference = "11111111",
                        AccountStatus = "Account1Status",
                        AccountStatusIsClosed = false,
                        AccountStatusIsViewOnly = false,
                        AccountStatusIsWithSolicitors = false,
                        AccountStatusSort = 1,
                        AddedSinceLastLogin = true,
                        CanAmendPlan = true,
                        CanMakePayment = true,
                        CanManageAccount = true,
                        ClientReference = "ClientReference1",
                        DirectDebitInFlight = true,
                        DiscountedBalance = 111.11M,
                        DiscountOfferAmount = 222.22M,
                        DiscountOfferExpiry = DateTime.Now.AddDays(30),
                        HasArrangement = true,
                        NextPlanPaymentDate = DateTime.Now.AddDays(7),
                        OriginalCompany = "OriginalCompany1",
                        OutstandingBalance = 999.99M,
                        PaymentPlanAmount = 55.50M,
                        PaymentPlanArrearsAmount = 15.00M,
                        PaymentPlanFrequency = "monthly",
                        PaymentPlanIsAutomated = true,
                        PaymentPlanMethod = "direct debit",
                        NeverAllowPlanTransfer = true,
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = ""
                    },
                    new AccountSummary()
                    {
                        AccountReference = "22222222",
                        AccountStatus = "Account2Status",
                        AccountStatusIsClosed = true,
                        AccountStatusIsViewOnly = true,
                        AccountStatusIsWithSolicitors = false,
                        AccountStatusSort = 2,
                        AddedSinceLastLogin = false,
                        CanAmendPlan = false,
                        CanMakePayment = false,
                        CanManageAccount = false,
                        ClientReference = "ClientReference2",
                        DirectDebitInFlight = false,
                        DiscountedBalance = 222.22M,
                        DiscountOfferAmount = 333.33M,
                        DiscountOfferExpiry = DateTime.Now.AddDays(15),
                        HasArrangement = true,
                        NextPlanPaymentDate = DateTime.Now.AddDays(14),
                        OriginalCompany = "OriginalCompany2",
                        OutstandingBalance = 888.88M,
                        PaymentPlanAmount = 45.50M,
                        PaymentPlanArrearsAmount = 25.00M,
                        PaymentPlanFrequency = "weekly",
                        PaymentPlanIsAutomated = false,
                        PaymentPlanMethod = "direct debit",
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "11111111",
                        NeverAllowPlanTransfer = true,
                    },
                    new AccountSummary()
                    {
                        AccountReference = "33333333",
                        AccountStatus = "Account3Status",
                        AccountStatusIsClosed = false,
                        AccountStatusIsViewOnly = false,
                        AccountStatusIsWithSolicitors = false,
                        AccountStatusSort = 3,
                        AddedSinceLastLogin = false,
                        CanAmendPlan = true,
                        CanMakePayment = true,
                        CanManageAccount = true,
                        ClientReference = "ClientReference3",
                        DirectDebitInFlight = true,
                        DiscountedBalance = 333.33M,
                        DiscountOfferAmount = 444.44M,
                        DiscountOfferExpiry = DateTime.Now.AddDays(7),
                        HasArrangement = true,
                        NextPlanPaymentDate = DateTime.Now.AddDays(5),
                        OriginalCompany = "OriginalCompany3",
                        OutstandingBalance = 777.77M,
                        PaymentPlanAmount = 32.50M,
                        PaymentPlanArrearsAmount = 90.00M,
                        PaymentPlanFrequency = "fortnightly",
                        PaymentPlanIsAutomated = true,
                        PaymentPlanMethod = "direct debit",
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "11111111,22222222,33333333",
                        NeverAllowPlanTransfer = true,
                    }
                },
                IncomeAndExpenditure = new IncomeAndExpenditure()
                {
                    DisposableIncome = 444.44M,
                    Created = DateTime.Now.AddMonths(-5)
                },
                SurrogateKeysByLowellReference = SurrogateKeysByLowellReference
            };

            MyAccountsVm destination = new MyAccountsVm();

            _mapperHelper.Setup(x => x.DerivePlanDescription(source.Accounts[0])).Returns("PlanDescription1");
            _mapperHelper.Setup(x => x.DerivePlanDescription(source.Accounts[1])).Returns("PlanDescription2");
            _mapperHelper.Setup(x => x.DerivePlanDescription(source.Accounts[2])).Returns("PlanDescription3");
            _mapperHelper.Setup(x => x.DeriveDiscountDescription(source.Accounts[0])).Returns("DiscountAvailableLinkTextOptional1");
            _mapperHelper.Setup(x => x.DeriveDiscountDescription(source.Accounts[1])).Returns("DiscountAvailableLinkTextOptional2");
            _mapperHelper.Setup(x => x.DeriveDiscountDescription(source.Accounts[2])).Returns("DiscountAvailableLinkTextOptional3");
            _mapperHelper.Setup(x => x.DeriveArrearsSummary(source.Accounts[0].PaymentPlanArrearsAmount)).Returns("ArrearsMessage1");
            _mapperHelper.Setup(x => x.DeriveArrearsSummary(source.Accounts[1].PaymentPlanArrearsAmount)).Returns("ArrearsMessage2");
            _mapperHelper.Setup(x => x.DeriveArrearsSummary(source.Accounts[2].PaymentPlanArrearsAmount)).Returns("ArrearsMessage3");

            _mapperHelper.Setup(x => x.DerivePlanTransferredFromMessage(null)).Returns("");
            _mapperHelper.Setup(x => x.DerivePlanTransferredFromMessage(It.Is<List<string>>(a => a.Count == 0))).Returns("");
            _mapperHelper.Setup(x => x.DerivePlanTransferredFromMessage(It.Is<List<string>>(a => 
                a.Count == 1 && a[0] == "11111111"))).Returns("Plan auto transferred from 11111111");
            _mapperHelper.Setup(x => x.DerivePlanTransferredFromMessage(It.Is<List<string>>(a =>
                a.Count == 3 && a[0] == "11111111" && a[1] == "22222222" && a[2] == "33333333")))
                .Returns("Plan auto transferred from 11111111, 22222222 and 33333333");

            _portalSettings.SolicitorsRedirectUrl = "SolicitorsRedirectUrl";

            MyAccountsVm expected = new MyAccountsVm()
            {
                Accounts = new List<AccountSummaryVm>()
                {
                    new AccountSummaryVm()
                    {
                        PlanDescription = "PlanDescription2",
                        DiscountAvailableLinkTextOptional = null,
                        DetailsLinkText = "View",
                        ArrearsMessage = null,
                        Class = "info-box--warning",
                        ShowWarningSymbol = true,
                        AccountStatusIsWithSolicitors = false,
                        LowellReferenceSurrogateKey = SurrogateKeysByLowellReference["22222222"],
                        OriginalCompanyText = "OriginalCompany2",
                        AccountReferenceText = "22222222",
                        OutstandingBalanceText = "£888.88",
                        AccountStatusText = "Account2Status",
                        NextPlanPaymentDate = source.Accounts[1].NextPlanPaymentDate,
                        ClientReference = "ClientReference2",
                        AddedSinceLastLogin = false,
                        HasArrangement = true,
                        DiscountedBalanceTextOptional = "£222.22",
                        IsPaymentButtonAvailable = false,
                        CanAmendPlan = false,
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "11111111",
                        NeverAllowPlanTransfer = true,
                        PlanTransferredFromAccounts = new List<string>(){ "11111111" },
                        PlanTransferredFromMessage = "Plan auto transferred from 11111111"
                    },
                    new AccountSummaryVm()
                    {
                        PlanDescription = "PlanDescription3",
                        DiscountAvailableLinkTextOptional = "DiscountAvailableLinkTextOptional3",
                        DetailsLinkText = "Manage account",
                        ArrearsMessage = "ArrearsMessage3",
                        Class = "info-box--warning",
                        ShowWarningSymbol = true,
                        AccountStatusIsWithSolicitors = false,
                        LowellReferenceSurrogateKey = SurrogateKeysByLowellReference["33333333"],
                        OriginalCompanyText = "OriginalCompany3",
                        AccountReferenceText = "33333333",
                        OutstandingBalanceText = "£777.77",
                        AccountStatusText = "Account3Status",
                        NextPlanPaymentDate = source.Accounts[2].NextPlanPaymentDate,
                        ClientReference = "ClientReference3",
                        AddedSinceLastLogin = false,
                        HasArrangement = true,
                        DiscountedBalanceTextOptional = "£333.33",
                        IsPaymentButtonAvailable = true,
                        CanAmendPlan = true,
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "11111111,22222222,33333333",
                        NeverAllowPlanTransfer = true,
                        PlanTransferredFromAccounts = new List<string>(){ "11111111", "22222222", "33333333" },
                        PlanTransferredFromMessage = "Plan auto transferred from 11111111, 22222222 and 33333333"
                    },
                    new AccountSummaryVm()
                    {
                        PlanDescription = "PlanDescription1",
                        DiscountAvailableLinkTextOptional = "DiscountAvailableLinkTextOptional1",
                        DetailsLinkText = "Manage account",
                        ArrearsMessage = "ArrearsMessage1",
                        Class = "info-box--warning",
                        ShowWarningSymbol = true,
                        AccountStatusIsWithSolicitors = false,
                        LowellReferenceSurrogateKey = SurrogateKeysByLowellReference["11111111"],
                        OriginalCompanyText = "OriginalCompany1",
                        AccountReferenceText = "11111111",
                        OutstandingBalanceText = "£999.99",
                        AccountStatusText = "Account1Status",
                        NextPlanPaymentDate = source.Accounts[0].NextPlanPaymentDate,
                        ClientReference = "ClientReference1",
                        AddedSinceLastLogin = true,
                        HasArrangement = true,
                        DiscountedBalanceTextOptional = "£111.11",
                        IsPaymentButtonAvailable = true,
                        CanAmendPlan = true,
                        NeverAllowPlanTransfer = true,
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "",
                        PlanTransferredFromAccounts = new List<string>(),
                        PlanTransferredFromMessage = null
                    },
                },
                LowellFinancialAccountSurrogateKey = SurrogateKeysByLowellReference["11111111"],
                DisposableIncome = 444.44M,
                IandELessThanOrIs12MonthsOld = true,
                IncomeAndExpenditureExpired = false,
                IncomeAndExpenditureSubmitted = true,
                NewAccounts = new List<AccountSummaryVm>()                
            };

            MyAccountsVm result = _converter.Convert(source, destination, null);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            Dictionary<string, Guid> SurrogateKeysByLowellReference = new Dictionary<string, Guid>()
            {
                { "11111111", Guid.NewGuid() },
                { "22222222", Guid.NewGuid() },
                { "33333333", Guid.NewGuid() }
            };

            CustomerSummary source = new CustomerSummary()
            {
                Accounts = new List<AccountSummary>()
                {
                    new AccountSummary()
                    {
                        AccountReference = "11111111",
                        AccountStatus = "Account1Status",
                        AccountStatusIsClosed = false,
                        AccountStatusIsViewOnly = false,
                        AccountStatusIsWithSolicitors = false,
                        AccountStatusSort = 1,
                        AddedSinceLastLogin = true,
                        CanAmendPlan = true,
                        CanMakePayment = true,
                        CanManageAccount = true,
                        ClientReference = "ClientReference1",
                        DirectDebitInFlight = true,
                        DiscountedBalance = 111.11M,
                        DiscountOfferAmount = 222.22M,
                        DiscountOfferExpiry = DateTime.Now.AddDays(30),
                        HasArrangement = true,                        
                        NextPlanPaymentDate = DateTime.Now.AddDays(7),
                        OriginalCompany = "OriginalCompany1",
                        OutstandingBalance = 999.99M,
                        PaymentPlanAmount = 55.50M,
                        PaymentPlanArrearsAmount = 15.00M,
                        PaymentPlanFrequency = "monthly",
                        PaymentPlanIsAutomated = true,
                        PaymentPlanMethod = "direct debit",
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "",
                        NeverAllowPlanTransfer = true,
                    },
                    new AccountSummary()
                    {
                        AccountReference = "22222222",
                        AccountStatus = "Account2Status",
                        AccountStatusIsClosed = true,
                        AccountStatusIsViewOnly = true,
                        AccountStatusIsWithSolicitors = false,
                        AccountStatusSort = 2,
                        AddedSinceLastLogin = false,
                        CanAmendPlan = false,
                        CanMakePayment = false,
                        CanManageAccount = false,
                        ClientReference = "ClientReference2",
                        DirectDebitInFlight = false,
                        DiscountedBalance = 222.22M,
                        DiscountOfferAmount = 333.33M,
                        DiscountOfferExpiry = DateTime.Now.AddDays(15),
                        HasArrangement = true,                        
                        NextPlanPaymentDate = DateTime.Now.AddDays(14),
                        OriginalCompany = "OriginalCompany2",
                        OutstandingBalance = 888.88M,
                        PaymentPlanAmount = 45.50M,
                        PaymentPlanArrearsAmount = 25.00M,
                        PaymentPlanFrequency = "weekly",
                        PaymentPlanIsAutomated = false,
                        PaymentPlanMethod = "direct debit",
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "11111111",
                        NeverAllowPlanTransfer = true,
                    },
                    new AccountSummary()
                    {
                        AccountReference = "33333333",
                        AccountStatus = "Account3Status",
                        AccountStatusIsClosed = false,
                        AccountStatusIsViewOnly = false,
                        AccountStatusIsWithSolicitors = false,
                        AccountStatusSort = 3,
                        AddedSinceLastLogin = false,
                        CanAmendPlan = true,
                        CanMakePayment = true,
                        CanManageAccount = true,
                        ClientReference = "ClientReference3",
                        DirectDebitInFlight = true,
                        DiscountedBalance = 333.33M,
                        DiscountOfferAmount = 444.44M,
                        DiscountOfferExpiry = DateTime.Now.AddDays(7),
                        HasArrangement = true,                        
                        NextPlanPaymentDate = DateTime.Now.AddDays(5),
                        OriginalCompany = "OriginalCompany3",
                        OutstandingBalance = 777.77M,
                        PaymentPlanAmount = 32.50M,
                        PaymentPlanArrearsAmount = 90.00M,
                        PaymentPlanFrequency = "fortnightly",
                        PaymentPlanIsAutomated = true,
                        PaymentPlanMethod = "direct debit",
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "11111111,22222222,33333333",
                        NeverAllowPlanTransfer = true,
                    }
                },
                IncomeAndExpenditure = new IncomeAndExpenditure()
                {
                    DisposableIncome = 444.44M,
                    Created = DateTime.Now.AddMonths(-5)
                },
                SurrogateKeysByLowellReference = SurrogateKeysByLowellReference
            };

            MyAccountsVm destination = null;

            _mapperHelper.Setup(x => x.DerivePlanDescription(source.Accounts[0])).Returns("PlanDescription1");
            _mapperHelper.Setup(x => x.DerivePlanDescription(source.Accounts[1])).Returns("PlanDescription2");
            _mapperHelper.Setup(x => x.DerivePlanDescription(source.Accounts[2])).Returns("PlanDescription3");
            _mapperHelper.Setup(x => x.DeriveDiscountDescription(source.Accounts[0])).Returns("DiscountAvailableLinkTextOptional1");
            _mapperHelper.Setup(x => x.DeriveDiscountDescription(source.Accounts[1])).Returns("DiscountAvailableLinkTextOptional2");
            _mapperHelper.Setup(x => x.DeriveDiscountDescription(source.Accounts[2])).Returns("DiscountAvailableLinkTextOptional3");
            _mapperHelper.Setup(x => x.DeriveArrearsSummary(source.Accounts[0].PaymentPlanArrearsAmount)).Returns("ArrearsMessage1");
            _mapperHelper.Setup(x => x.DeriveArrearsSummary(source.Accounts[1].PaymentPlanArrearsAmount)).Returns("ArrearsMessage2");
            _mapperHelper.Setup(x => x.DeriveArrearsSummary(source.Accounts[2].PaymentPlanArrearsAmount)).Returns("ArrearsMessage3");
            _portalSettings.SolicitorsRedirectUrl = "SolicitorsRedirectUrl";

            _mapperHelper.Setup(x => x.DerivePlanTransferredFromMessage(null)).Returns("");
            _mapperHelper.Setup(x => x.DerivePlanTransferredFromMessage(It.Is<List<string>>(a => a.Count == 0))).Returns("");
            _mapperHelper.Setup(x => x.DerivePlanTransferredFromMessage(It.Is<List<string>>(a =>
                a.Count == 1 && a[0] == "11111111"))).Returns("Plan auto transferred from 11111111");
            _mapperHelper.Setup(x => x.DerivePlanTransferredFromMessage(It.Is<List<string>>(a =>
                a.Count == 3 && a[0] == "11111111" && a[1] == "22222222" && a[2] == "33333333")))
                .Returns("Plan auto transferred from 11111111, 22222222 and 33333333");

            MyAccountsVm expected = new MyAccountsVm()
            {
                Accounts = new List<AccountSummaryVm>()
                {
                    new AccountSummaryVm()
                    {
                        PlanDescription = "PlanDescription2",
                        DiscountAvailableLinkTextOptional = null,
                        DetailsLinkText = "View",
                        ArrearsMessage = null,
                        Class = "info-box--warning",
                        ShowWarningSymbol = true,
                        AccountStatusIsWithSolicitors = false,
                        LowellReferenceSurrogateKey = SurrogateKeysByLowellReference["22222222"],
                        OriginalCompanyText = "OriginalCompany2",
                        AccountReferenceText = "22222222",
                        OutstandingBalanceText = "£888.88",
                        AccountStatusText = "Account2Status",
                        NextPlanPaymentDate = source.Accounts[1].NextPlanPaymentDate,
                        ClientReference = "ClientReference2",
                        AddedSinceLastLogin = false,
                        HasArrangement = true,
                        DiscountedBalanceTextOptional = "£222.22",
                        IsPaymentButtonAvailable = false,
                        CanAmendPlan = false,
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "11111111",
                        NeverAllowPlanTransfer = true,
                        PlanTransferredFromAccounts = new List<string>(){ "11111111" },
                        PlanTransferredFromMessage = "Plan auto transferred from 11111111"
                    },
                    new AccountSummaryVm()
                    {
                        PlanDescription = "PlanDescription3",
                        DiscountAvailableLinkTextOptional = "DiscountAvailableLinkTextOptional3",
                        DetailsLinkText = "Manage account",
                        ArrearsMessage = "ArrearsMessage3",
                        Class = "info-box--warning",
                        ShowWarningSymbol = true,
                        AccountStatusIsWithSolicitors = false,
                        LowellReferenceSurrogateKey = SurrogateKeysByLowellReference["33333333"],
                        OriginalCompanyText = "OriginalCompany3",
                        AccountReferenceText = "33333333",
                        OutstandingBalanceText = "£777.77",
                        AccountStatusText = "Account3Status",
                        NextPlanPaymentDate = source.Accounts[2].NextPlanPaymentDate,
                        ClientReference = "ClientReference3",
                        AddedSinceLastLogin = false,
                        HasArrangement = true,
                        DiscountedBalanceTextOptional = "£333.33",
                        IsPaymentButtonAvailable = true,
                        CanAmendPlan = true,
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "11111111,22222222,33333333",
                        NeverAllowPlanTransfer = true,
                        PlanTransferredFromAccounts = new List<string>(){ "11111111", "22222222", "33333333" },
                        PlanTransferredFromMessage = "Plan auto transferred from 11111111, 22222222 and 33333333"
                    },
                    new AccountSummaryVm()
                    {
                        PlanDescription = "PlanDescription1",
                        DiscountAvailableLinkTextOptional = "DiscountAvailableLinkTextOptional1",
                        DetailsLinkText = "Manage account",
                        ArrearsMessage = "ArrearsMessage1",
                        Class = "info-box--warning",
                        ShowWarningSymbol = true,
                        AccountStatusIsWithSolicitors = false,
                        LowellReferenceSurrogateKey = SurrogateKeysByLowellReference["11111111"],
                        OriginalCompanyText = "OriginalCompany1",
                        AccountReferenceText = "11111111",
                        OutstandingBalanceText = "£999.99",
                        AccountStatusText = "Account1Status",
                        NextPlanPaymentDate = source.Accounts[0].NextPlanPaymentDate,
                        ClientReference = "ClientReference1",
                        AddedSinceLastLogin = true,
                        HasArrangement = true,
                        DiscountedBalanceTextOptional = "£111.11",
                        IsPaymentButtonAvailable = true,
                        CanAmendPlan = true,
                        PlanPendingTransfer = false,
                        PlanTransferredFrom = "",
                        NeverAllowPlanTransfer = true,
                        PlanTransferredFromAccounts = new List<string>(),
                        PlanTransferredFromMessage = null,
                    },
                },
                LowellFinancialAccountSurrogateKey = SurrogateKeysByLowellReference["11111111"],
                DisposableIncome = 444.44M,
                IandELessThanOrIs12MonthsOld = true,
                IncomeAndExpenditureExpired = false,
                IncomeAndExpenditureSubmitted = true,
                NewAccounts = new List<AccountSummaryVm>()
            };

            MyAccountsVm result = _converter.Convert(source, destination, null);

            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }
    }
}
