using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class MapperHelperTests
    {
        private MapperHelper _helper;
        private PortalSetting _portalSettings;
        private Mock<ICalculatorService> _calculatorService;

        [TestInitialize]
        public void TestInitialise()
        {
            this._portalSettings = new PortalSetting();
            this._calculatorService = new Mock<ICalculatorService>(MockBehavior.Strict);

            this._helper = new MapperHelper(this._portalSettings, this._calculatorService.Object);
        }

        [TestMethod]
        public void AppendNonNullWithSpaceTest_StringToAddNull()
        {
            string stringBefore = "stringBefore";
            string stringToAdd = null;
            string inbetweenText = "inbetweenText";

            string expected = "stringBefore";
            string result = _helper.AppendNonNullWithSpace(stringBefore, stringToAdd, inbetweenText);

            Assert.AreEqual(expected, result);
            Assert.AreSame(stringBefore, result);
        }

        [TestMethod]
        public void AppendNonNullWithSpaceTest_StringBeforeEmpty()
        {
            string stringBefore = "";
            string stringToAdd = "stringToAdd";
            string inbetweenText = "inbetweenText";

            string expected = "inbetweenTextstringToAdd";
            string result = _helper.AppendNonNullWithSpace(stringBefore, stringToAdd, inbetweenText);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AppendNonNullWithSpaceTest_StringBeforeNotEmpty()
        {
            string stringBefore = "stringBefore";
            string stringToAdd = "stringToAdd";
            string inbetweenText = "inbetweenText";

            string expected = "stringBefore inbetweenTextstringToAdd";
            string result = _helper.AppendNonNullWithSpace(stringBefore, stringToAdd, inbetweenText);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveDiscountDescriptionTest_DiscountOfferAmountNull()
        {
            Account account = new Account()
            {
                DiscountOfferAmount = null,
                DiscountOfferExpiry = DateTime.Now
            };

            string result = _helper.DeriveDiscountDescription(account);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeriveDiscountDescriptionTest_DiscountOfferExpiryNull()
        {
            Account account = new Account()
            {
                DiscountOfferAmount = 123.45M,
                DiscountOfferExpiry = null
            };

            string result = _helper.DeriveDiscountDescription(account);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeriveDiscountDescriptionTest()
        {
            Account account = new Account()
            {
                DiscountOfferAmount = 123.45M,
                DiscountOfferExpiry = DateTime.Now,
                OutstandingBalance = 500M
            };

            string expected = $"Save £123.45 until {DateTime.Now.ToString("d", CultureInfo.CurrentCulture)}. You will pay £376.55. To accept, set up a Direct Debit Plan or Pay In Full.";
            string result = _helper.DeriveDiscountDescription(account);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveAccountPlanDescriptionTest_PaymentPlanAmountNull()
        {
            Account account = new Account()
            {
                PaymentPlanAmount = null
            };

            string result = _helper.DerivePlanDescription(account);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeriveAccountPlanDescriptionTest_PaymentPlanAmountZero()
        {
            Account account = new Account()
            {
                PaymentPlanAmount = 0
            };

            string result = _helper.DerivePlanDescription(account);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeriveAccountPlanDescriptionTest()
        {
            Account account = new Account()
            {
                PaymentPlanAmount = 123.45M,
                PaymentPlanFrequency = "monthly",
                PaymentPlanMethod = "direct debit"
            };

            string expected = ": £123.45 monthly by direct debit";
            string result = _helper.DerivePlanDescription(account);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveArrearsDetailTest_PaymentPlanArrearsAmountNull()
        {
            decimal? paymentPlanArrearsAmount = null;
            bool paymentPlanIsAutomated = true;

            string result = _helper.DeriveArrearsDetail(paymentPlanArrearsAmount, paymentPlanIsAutomated);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeriveArrearsDetailTest_PaymentPlanArrearsAmountZero()
        {
            decimal? paymentPlanArrearsAmount = 0;
            bool paymentPlanIsAutomated = true;

            string result = _helper.DeriveArrearsDetail(paymentPlanArrearsAmount, paymentPlanIsAutomated);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeriveArrearsDetailTest_PaymentPlanIsAutomated()
        {
            _portalSettings.TalkToUsUrl = "TalkToUsUrl";
            decimal? paymentPlanArrearsAmount = 123.45M;
            bool paymentPlanIsAutomated = true;

            string expected = $"Unfortunately, your last payment didn’t go through as planned and your account has fallen in to arrears.<br><br>If you are able to you can get your plan up to date by making a one-off payment of £123.45.<br><br>After that, your next payment will be taken as normal. Don’t worry if you need to change your plan, you can do it after you’ve made this payment. <a target=\"_blank\" href=\"TalkToUsUrl\">talk to us</a> or call us on 0333 556 5550 and we’ll be happy to help.";
            string result = _helper.DeriveArrearsDetail(paymentPlanArrearsAmount, paymentPlanIsAutomated);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveArrearsDetailTest_PaymentPlanIsNotAutomated()
        {
            _portalSettings.TalkToUsUrl = "TalkToUsUrl";
            decimal? paymentPlanArrearsAmount = 123.45M;
            bool paymentPlanIsAutomated = false;

            string expected = $"Unfortunately, your last payment didn’t go through as planned and your account has fallen in to arrears.<br><br>If you are able to you can get your plan up to date by making a one-off payment of £123.45.<br><br>After that, your next payment will be taken as normal. Don’t worry if you need to change your plan, you can do it after you’ve made this payment. <a target=\"_blank\" href=\"TalkToUsUrl\">talk to us</a> or call us on 0333 556 5550 and we’ll be happy to help.";
            string result = _helper.DeriveArrearsDetail(paymentPlanArrearsAmount, paymentPlanIsAutomated);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AccountHasDiscountOfferTest_DiscountOfferAmountNull()
        {
            AccountSummary account = new AccountSummary()
            {
                DiscountOfferAmount = null,
                DiscountOfferExpiry = DateTime.Now
            };

            bool expected = false;
            bool result = _helper.AccountHasDiscountOffer(account);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AccountHasDiscountOfferTest_DiscountOfferExpiryNull()
        {
            AccountSummary account = new AccountSummary()
            {
                DiscountOfferAmount = 123.45M,
                DiscountOfferExpiry = null
            };

            bool expected = false;
            bool result = _helper.AccountHasDiscountOffer(account);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AccountHasDiscountOfferTest()
        {
            AccountSummary account = new AccountSummary()
            {
                DiscountOfferAmount = 123.45M,
                DiscountOfferExpiry = DateTime.Now
            };

            bool expected = true;
            bool result = _helper.AccountHasDiscountOffer(account);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveDiscountDescription_AccountHasDiscountOffer()
        {
            AccountSummary account = new AccountSummary()
            {
                DiscountOfferAmount = 123.45M,
                DiscountOfferExpiry = DateTime.Now
            };

            string expected = $"Save £123.45 until {DateTime.Now.ToString("d", CultureInfo.CurrentCulture)}";
            string result = _helper.DeriveDiscountDescription(account);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveDiscountDescription_AccountHasNoDiscountOffer()
        {
            AccountSummary account = new AccountSummary()
            {
                DiscountOfferAmount = null,
                DiscountOfferExpiry = null
            };

            string expected = null;
            string result = _helper.DeriveDiscountDescription(account);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveAccountSummaryPlanDescriptionTest_PaymentPlanAmountNull()
        {
            AccountSummary account = new AccountSummary()
            {
                PaymentPlanAmount = null
            };

            string result = _helper.DerivePlanDescription(account);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeriveAccountSummaryPlanDescriptionTest_PaymentPlanAmountZero()
        {
            AccountSummary account = new AccountSummary()
            {
                PaymentPlanAmount = 0
            };

            string result = _helper.DerivePlanDescription(account);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeriveAccountSummaryPlanDescriptionTest()
        {
            AccountSummary account = new AccountSummary()
            {
                PaymentPlanAmount = 123.45M,
                PaymentPlanFrequency = "monthly",
                PaymentPlanMethod = "direct debit"
            };

            string expected = ": £123.45 monthly";
            string result = _helper.DerivePlanDescription(account);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveArrearsSummaryTest_PaymentPlanArrearsAmountZero()
        {
            string expected = null;
            string result = _helper.DeriveArrearsSummary(0);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveArrearsSummaryTest_PaymentPlanArrearsAmountNull()
        {
            string expected = null;
            string result = _helper.DeriveArrearsSummary(null);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DeriveArrearsSummaryTest()
        {
            _portalSettings.TalkToUsUrl = "TalkToUsUrl";
            string expected = $"In arrears<br>If you are able to, please make your payment today or <a target=\"_blank\" href=\"TalkToUsUrl\">talk to us</a> for help.";
            string result = _helper.DeriveArrearsSummary(123.45M);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DerivePlanTransferredFromMessageTest_accountNumbersNull()
        {
            Assert.AreEqual("", _helper.DerivePlanTransferredFromMessage(null));
        }

        [TestMethod]
        public void DerivePlanTransferredFromMessageTest_accountNumbersEmpty()
        {
            Assert.AreEqual("", _helper.DerivePlanTransferredFromMessage(new List<string>()));
        }

        [TestMethod]
        public void DerivePlanTransferredFromMessageTest_OneAccountNumber()
        {
            string expected = "You've paid off account 12345678. " +
                "The payment has been auto transferred to this account.";
            Assert.AreEqual(expected, _helper.DerivePlanTransferredFromMessage(new List<string>() { "12345678" }));
        }

        [TestMethod]
        public void DerivePlanTransferredFromMessageTest_TwoAccountNumbers()
        {
            List<string> accountNumbers = new List<string>()
            {
                "11111111",
                "22222222"
            };

            string expected = "You've paid off account 11111111 and 22222222. " +
                "The payment has been auto transferred to this account.";
            Assert.AreEqual(expected, _helper.DerivePlanTransferredFromMessage(accountNumbers));
        }

        [TestMethod]
        public void DerivePlanTransferredFromMessageTest_MultipleAccountNumbers()
        {
            List<string> accountNumbers = new List<string>()
            {
                "11111111",
                "22222222",
                "33333333",
                "44444444",
                "55555555"
            };

            string expected = "You've paid off account 11111111, 22222222, 33333333, 44444444 and 55555555. " +
                "The payment has been auto transferred to this account.";
            Assert.AreEqual(expected, _helper.DerivePlanTransferredFromMessage(accountNumbers));
        }

        [TestMethod]
        public void DerivePlanTransferredFromAccountsFormattedTest_AccountNumbersNull()
        {
            Assert.AreEqual("", _helper.DerivePlanTransferredFromAccountsFormatted(null));
        }

        [TestMethod]
        public void DerivePlanTransferredFromAccountsFormattedTest_AccountNumbersEmpty()
        {
            Assert.AreEqual("", _helper.DerivePlanTransferredFromAccountsFormatted(new List<string>()));
        }

        [TestMethod]
        public void DerivePlanTransferredFromAccountsFormattedTest_OneAccountNumber()
        {
            string expected = "account ref: 12345678";
            Assert.AreEqual(expected, _helper.DerivePlanTransferredFromAccountsFormatted(new List<string>() { "12345678" }));
        }

        [TestMethod]
        public void DerivePlanTransferredFromAccountsFormattedTest_TwoAccountNumbers()
        {
            List<string> accountNumbers = new List<string>()
            {
                "11111111",
                "22222222"
            };

            string expected = "accounts 11111111 and 22222222";
            Assert.AreEqual(expected, _helper.DerivePlanTransferredFromAccountsFormatted(accountNumbers));
        }

        [TestMethod]
        public void DerivePlanTransferredFromAccountsFormattedTest_MultipleAccountNumbers()
        {
            List<string> accountNumbers = new List<string>()
            {
                "11111111",
                "22222222",
                "33333333",
                "44444444",
                "55555555"
            };

            string expected = "accounts 11111111, 22222222, 33333333, 44444444 and 55555555";
            Assert.AreEqual(expected, _helper.DerivePlanTransferredFromAccountsFormatted(accountNumbers));
        }

        [TestMethod]
        public void ConvertEmploymentStatusFromCaseflowTest() 
        {
            Assert.AreEqual(string.Empty, _helper.ConvertEmploymentStatusFromCaseflow(null));
            Assert.AreEqual(string.Empty, _helper.ConvertEmploymentStatusFromCaseflow(""));
            Assert.AreEqual(string.Empty, _helper.ConvertEmploymentStatusFromCaseflow("testing..."));

            Assert.AreEqual("employed-full-time", _helper.ConvertEmploymentStatusFromCaseflow("Full-Time"));
            Assert.AreEqual("employed-full-time", _helper.ConvertEmploymentStatusFromCaseflow("full-time"));
            Assert.AreEqual("employed-part-time", _helper.ConvertEmploymentStatusFromCaseflow("Part-Time"));
            Assert.AreEqual("employed-part-time", _helper.ConvertEmploymentStatusFromCaseflow("part-time"));
            Assert.AreEqual("self-employed", _helper.ConvertEmploymentStatusFromCaseflow("Self-Employed"));
            Assert.AreEqual("self-employed", _helper.ConvertEmploymentStatusFromCaseflow("self-employed"));
            Assert.AreEqual("unemployed", _helper.ConvertEmploymentStatusFromCaseflow("Unemployed"));
            Assert.AreEqual("unemployed", _helper.ConvertEmploymentStatusFromCaseflow("unemployed"));
            Assert.AreEqual("student", _helper.ConvertEmploymentStatusFromCaseflow("Student"));
            Assert.AreEqual("student", _helper.ConvertEmploymentStatusFromCaseflow("student"));
            Assert.AreEqual("carer", _helper.ConvertEmploymentStatusFromCaseflow("Carer"));
            Assert.AreEqual("carer", _helper.ConvertEmploymentStatusFromCaseflow("carer"));
            Assert.AreEqual("retired", _helper.ConvertEmploymentStatusFromCaseflow("Retired"));
            Assert.AreEqual("retired", _helper.ConvertEmploymentStatusFromCaseflow("retired"));
            Assert.AreEqual("illness-disability-related", _helper.ConvertEmploymentStatusFromCaseflow("Illness/Disability Related"));
            Assert.AreEqual("illness-disability-related", _helper.ConvertEmploymentStatusFromCaseflow("illness/disability related"));
            Assert.AreEqual("other", _helper.ConvertEmploymentStatusFromCaseflow("Other"));
            Assert.AreEqual("other", _helper.ConvertEmploymentStatusFromCaseflow("other"));
        }

        [TestMethod]
        public void ConvertToEmploymentStatusCaseflowTest() 
        {
            Assert.AreEqual(string.Empty, _helper.ConvertToEmploymentStatusCaseflow(null));
            Assert.AreEqual(string.Empty, _helper.ConvertToEmploymentStatusCaseflow(""));
            Assert.AreEqual(string.Empty, _helper.ConvertToEmploymentStatusCaseflow("testing..."));

            Assert.AreEqual("Full-Time", _helper.ConvertToEmploymentStatusCaseflow("employed-full-time"));
            Assert.AreEqual("Full-Time", _helper.ConvertToEmploymentStatusCaseflow("Employed-Full-Time"));
            Assert.AreEqual("Part-Time", _helper.ConvertToEmploymentStatusCaseflow("employed-part-time"));
            Assert.AreEqual("Part-Time", _helper.ConvertToEmploymentStatusCaseflow("Employed-Part-Time"));
            Assert.AreEqual("Self-Employed", _helper.ConvertToEmploymentStatusCaseflow("self-employed"));
            Assert.AreEqual("Self-Employed", _helper.ConvertToEmploymentStatusCaseflow("Self-Employed"));
            Assert.AreEqual("Unemployed", _helper.ConvertToEmploymentStatusCaseflow("unemployed"));
            Assert.AreEqual("Unemployed", _helper.ConvertToEmploymentStatusCaseflow("Unemployed"));
            Assert.AreEqual("Student", _helper.ConvertToEmploymentStatusCaseflow("student"));
            Assert.AreEqual("Student", _helper.ConvertToEmploymentStatusCaseflow("Student"));
            Assert.AreEqual("Carer", _helper.ConvertToEmploymentStatusCaseflow("carer"));
            Assert.AreEqual("Carer", _helper.ConvertToEmploymentStatusCaseflow("Carer"));
            Assert.AreEqual("Retired", _helper.ConvertToEmploymentStatusCaseflow("retired"));
            Assert.AreEqual("Retired", _helper.ConvertToEmploymentStatusCaseflow("Retired"));
            Assert.AreEqual("Illness/Disability Related", _helper.ConvertToEmploymentStatusCaseflow("illness-disability-related"));
            Assert.AreEqual("Illness/Disability Related", _helper.ConvertToEmploymentStatusCaseflow("Illness-Disability-Related"));
            Assert.AreEqual("Other", _helper.ConvertToEmploymentStatusCaseflow("other"));
            Assert.AreEqual("Other", _helper.ConvertToEmploymentStatusCaseflow("Other"));
        }

        [TestMethod]
        public void ConvertHousingStatusFromCaseflowTest() 
        {
            Assert.AreEqual(string.Empty, _helper.ConvertHousingStatusFromCaseflow(null));
            Assert.AreEqual(string.Empty, _helper.ConvertHousingStatusFromCaseflow(""));
            Assert.AreEqual(string.Empty, _helper.ConvertHousingStatusFromCaseflow("testing..."));

            Assert.AreEqual("homeowner", _helper.ConvertHousingStatusFromCaseflow("owner"));
            Assert.AreEqual("homeowner", _helper.ConvertHousingStatusFromCaseflow("Owner"));
            Assert.AreEqual("mortgage", _helper.ConvertHousingStatusFromCaseflow("mortgage"));
            Assert.AreEqual("mortgage", _helper.ConvertHousingStatusFromCaseflow("Mortgage"));
            Assert.AreEqual("homeless", _helper.ConvertHousingStatusFromCaseflow("homeless"));
            Assert.AreEqual("homeless", _helper.ConvertHousingStatusFromCaseflow("Homeless"));
            Assert.AreEqual("tenant-private", _helper.ConvertHousingStatusFromCaseflow("tenant - private"));
            Assert.AreEqual("tenant-private", _helper.ConvertHousingStatusFromCaseflow("Tenant - Private"));
            Assert.AreEqual("tenant-social", _helper.ConvertHousingStatusFromCaseflow("tenant - social"));
            Assert.AreEqual("tenant-social", _helper.ConvertHousingStatusFromCaseflow("Tenant - Social"));
            Assert.AreEqual("living-with-parents", _helper.ConvertHousingStatusFromCaseflow("living with parents"));
            Assert.AreEqual("living-with-parents", _helper.ConvertHousingStatusFromCaseflow("Living With Parents"));
            Assert.AreEqual("Illness/Disability related", _helper.ConvertHousingStatusFromCaseflow("illness/disability related"));
            Assert.AreEqual("Illness/Disability related", _helper.ConvertHousingStatusFromCaseflow("Illness/Disability Related"));
            Assert.AreEqual("other", _helper.ConvertHousingStatusFromCaseflow("other"));
            Assert.AreEqual("other", _helper.ConvertHousingStatusFromCaseflow("Other"));
        }

        [TestMethod]
        public void ConvertToHousingStatusCaseflowTest() 
        {
            Assert.AreEqual(string.Empty, _helper.ConvertToHousingStatusCaseflow(null));
            Assert.AreEqual(string.Empty, _helper.ConvertToHousingStatusCaseflow(""));
            Assert.AreEqual(string.Empty, _helper.ConvertToHousingStatusCaseflow("testing..."));

            Assert.AreEqual("Owner", _helper.ConvertToHousingStatusCaseflow("homeowner"));
            Assert.AreEqual("Owner", _helper.ConvertToHousingStatusCaseflow("Homeowner"));
            Assert.AreEqual("Mortgage", _helper.ConvertToHousingStatusCaseflow("mortgage"));
            Assert.AreEqual("Mortgage", _helper.ConvertToHousingStatusCaseflow("Mortgage"));
            Assert.AreEqual("HomeLess", _helper.ConvertToHousingStatusCaseflow("homeless"));
            Assert.AreEqual("HomeLess", _helper.ConvertToHousingStatusCaseflow("Homeless"));
            Assert.AreEqual("Tenant - Private", _helper.ConvertToHousingStatusCaseflow("tenant-private"));
            Assert.AreEqual("Tenant - Private", _helper.ConvertToHousingStatusCaseflow("Tenant-Private"));
            Assert.AreEqual("Tenant - Social", _helper.ConvertToHousingStatusCaseflow("tenant-social"));
            Assert.AreEqual("Tenant - Social", _helper.ConvertToHousingStatusCaseflow("Tenant-Social"));
            Assert.AreEqual("Living With Parents", _helper.ConvertToHousingStatusCaseflow("living-with-parents"));
            Assert.AreEqual("Living With Parents", _helper.ConvertToHousingStatusCaseflow("Living-With-Parents"));
            Assert.AreEqual("Illness/Disability Related", _helper.ConvertToHousingStatusCaseflow("Illness/Disability related"));
            Assert.AreEqual("Illness/Disability Related", _helper.ConvertToHousingStatusCaseflow("illness/disability related"));
            Assert.AreEqual("Other", _helper.ConvertToHousingStatusCaseflow("other"));
            Assert.AreEqual("Other", _helper.ConvertToHousingStatusCaseflow("Other"));
        }

        [TestMethod]
        public void ConvertFrequencyFromCaseflowTest() 
        {
            Assert.AreEqual(string.Empty, _helper.ConvertFrequencyFromCaseflow(null));
            Assert.AreEqual(string.Empty, _helper.ConvertFrequencyFromCaseflow(""));
            Assert.AreEqual(string.Empty, _helper.ConvertFrequencyFromCaseflow("testing..."));

            Assert.AreEqual("monthly", _helper.ConvertFrequencyFromCaseflow("M"));
            Assert.AreEqual("weekly", _helper.ConvertFrequencyFromCaseflow("W"));
            Assert.AreEqual("fortnightly", _helper.ConvertFrequencyFromCaseflow("F"));
            Assert.AreEqual("4week", _helper.ConvertFrequencyFromCaseflow("4"));
            Assert.AreEqual("Quarterly", _helper.ConvertFrequencyFromCaseflow("Q"));
            Assert.AreEqual("Last Day of the Month", _helper.ConvertFrequencyFromCaseflow("L"));
            Assert.AreEqual("Annually", _helper.ConvertFrequencyFromCaseflow("A"));

            Assert.AreEqual("monthly", _helper.ConvertFrequencyFromCaseflow("m"));
            Assert.AreEqual("weekly", _helper.ConvertFrequencyFromCaseflow("w"));
            Assert.AreEqual("fortnightly", _helper.ConvertFrequencyFromCaseflow("f"));
            Assert.AreEqual("4week", _helper.ConvertFrequencyFromCaseflow("4"));
            Assert.AreEqual("Quarterly", _helper.ConvertFrequencyFromCaseflow("q"));
            Assert.AreEqual("Last Day of the Month", _helper.ConvertFrequencyFromCaseflow("l"));
            Assert.AreEqual("Annually", _helper.ConvertFrequencyFromCaseflow("a"));
        }

        [TestMethod]
        public void ConvertFrequencyToInitialTest() 
        {
            Assert.AreEqual(string.Empty, _helper.ConvertFrequencyToInitial(null));
            Assert.AreEqual(string.Empty, _helper.ConvertFrequencyToInitial(""));
            Assert.AreEqual(string.Empty, _helper.ConvertFrequencyToInitial("testing..."));

            Assert.AreEqual("M", _helper.ConvertFrequencyToInitial("monthly"));
            Assert.AreEqual("W", _helper.ConvertFrequencyToInitial("weekly"));
            Assert.AreEqual("F", _helper.ConvertFrequencyToInitial("fortnightly"));
            Assert.AreEqual("4", _helper.ConvertFrequencyToInitial("4week"));
            Assert.AreEqual("Q", _helper.ConvertFrequencyToInitial("quarterly"));
            Assert.AreEqual("L", _helper.ConvertFrequencyToInitial("last day of the month"));
            Assert.AreEqual("A", _helper.ConvertFrequencyToInitial("annually"));

            Assert.AreEqual("M", _helper.ConvertFrequencyToInitial("Monthly"));
            Assert.AreEqual("W", _helper.ConvertFrequencyToInitial("Weekly"));
            Assert.AreEqual("F", _helper.ConvertFrequencyToInitial("Fortnightly"));
            Assert.AreEqual("4", _helper.ConvertFrequencyToInitial("4week"));
            Assert.AreEqual("Q", _helper.ConvertFrequencyToInitial("Quarterly"));
            Assert.AreEqual("L", _helper.ConvertFrequencyToInitial("Last Day of the Month"));
            Assert.AreEqual("A", _helper.ConvertFrequencyToInitial("Annually"));
        }

        [TestMethod]
        public void MapRegularPaymentTest() 
        {
            _calculatorService.Setup(x => x.ConvertToMonthly(500, "Q")).Returns(1);
            RegularPayment quarterly = _helper.MapRegularPayment(500, "Q");
            Assert.AreEqual("monthly", quarterly.Frequency);
            Assert.AreEqual(1, quarterly.Amount);

            _calculatorService.Setup(x => x.ConvertToMonthly(500, "L")).Returns(1);
            RegularPayment lastDayOfMonth = _helper.MapRegularPayment(500, "L");
            Assert.AreEqual("monthly", lastDayOfMonth.Frequency);
            Assert.AreEqual(1, lastDayOfMonth.Amount);

            _calculatorService.Setup(x => x.ConvertToMonthly(500, "A")).Returns(1);
            RegularPayment annually = _helper.MapRegularPayment(500, "A");
            Assert.AreEqual("monthly", annually.Frequency);
            Assert.AreEqual(1, annually.Amount);

            RegularPayment monthly = _helper.MapRegularPayment(123.45M, "M");
            Assert.AreEqual("monthly", monthly.Frequency);
            Assert.AreEqual(123.45M, monthly.Amount);

            RegularPayment weekly = _helper.MapRegularPayment(123.45M, "W");
            Assert.AreEqual("weekly", weekly.Frequency);
            Assert.AreEqual(123.45M, weekly.Amount);

            RegularPayment fortnightly = _helper.MapRegularPayment(123.45M, "F");
            Assert.AreEqual("fortnightly", fortnightly.Frequency);
            Assert.AreEqual(123.45M, fortnightly.Amount);

            RegularPayment fourWeekly = _helper.MapRegularPayment(123.45M, "4");
            Assert.AreEqual("4week", fourWeekly.Frequency);
            Assert.AreEqual(123.45M, fourWeekly.Amount);

            RegularPayment invalid = _helper.MapRegularPayment(123.45M, "testing...");
            Assert.AreEqual("", invalid.Frequency);
            Assert.AreEqual(123.45M, invalid.Amount);
        }

        [TestMethod]
        public void MapOutgoingTest() 
        {
            _calculatorService.Setup(x => x.ConvertToMonthly(500, "Q")).Returns(1);
            Outgoing quarterly = _helper.MapOutgoing(500, "Q", 123.45M);
            Assert.AreEqual("monthly", quarterly.Frequency);
            Assert.AreEqual(1, quarterly.Amount);
            Assert.AreEqual(123.45M, quarterly.ArrearsAmount);
            Assert.AreEqual(true, quarterly.InArrears);

            _calculatorService.Setup(x => x.ConvertToMonthly(500, "L")).Returns(1);
            Outgoing lastDayOfMonth = _helper.MapOutgoing(500, "L", 123.45M);
            Assert.AreEqual("monthly", lastDayOfMonth.Frequency);
            Assert.AreEqual(1, lastDayOfMonth.Amount);
            Assert.AreEqual(123.45M, lastDayOfMonth.ArrearsAmount);
            Assert.AreEqual(true, lastDayOfMonth.InArrears);

            _calculatorService.Setup(x => x.ConvertToMonthly(500, "A")).Returns(1);
            Outgoing annually = _helper.MapOutgoing(500, "A", 123.45M);
            Assert.AreEqual("monthly", annually.Frequency);
            Assert.AreEqual(1, annually.Amount);
            Assert.AreEqual(123.45M, annually.ArrearsAmount);
            Assert.AreEqual(true, annually.InArrears);

            Outgoing monthly = _helper.MapOutgoing(123.45M, "M", 0);
            Assert.AreEqual("monthly", monthly.Frequency);
            Assert.AreEqual(123.45M, monthly.Amount);
            Assert.AreEqual(0, monthly.ArrearsAmount);
            Assert.AreEqual(false, monthly.InArrears);

            Outgoing weekly = _helper.MapOutgoing(123.45M, "W", 50);
            Assert.AreEqual("weekly", weekly.Frequency);
            Assert.AreEqual(123.45M, weekly.Amount);
            Assert.AreEqual(50, weekly.ArrearsAmount);
            Assert.AreEqual(true, weekly.InArrears);

            Outgoing fortnightly = _helper.MapOutgoing(123.45M, "F", 0);
            Assert.AreEqual("fortnightly", fortnightly.Frequency);
            Assert.AreEqual(123.45M, fortnightly.Amount);
            Assert.AreEqual(0, fortnightly.ArrearsAmount);
            Assert.AreEqual(false, fortnightly.InArrears);

            Outgoing fourWeekly = _helper.MapOutgoing(123.45M, "4", 0);
            Assert.AreEqual("4week", fourWeekly.Frequency);
            Assert.AreEqual(123.45M, fourWeekly.Amount);
            Assert.AreEqual(0, fourWeekly.ArrearsAmount);
            Assert.AreEqual(false, fourWeekly.InArrears);

            Outgoing invalid = _helper.MapOutgoing(123.45M, "testing...", 0);
            Assert.AreEqual("", invalid.Frequency);
            Assert.AreEqual(123.45M, invalid.Amount);
            Assert.AreEqual(0, invalid.ArrearsAmount);
            Assert.AreEqual(false, invalid.InArrears);
        }

        [TestMethod]
        public void CreateOtherDebtsTest_NoOtherDebts() 
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure()
            {
                CourtFines = 0,
                CourtFinesArrears = 0,
                CourtFinesFrequency = string.Empty,
                CCJs = 0,
                CCJsArrears = 0,
                CCJsFrequency = string.Empty,
                OtherExpenditure = 0,
                OtherExpenditureFrequency = string.Empty,
            };

            List<SaveOtherDebtsApiModel> result = _helper.CreateOtherDebts(iAndE);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CreateOtherDebtsTest_CourtFines()
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure()
            {
                CourtFines = 123.45M,
                CourtFinesArrears = 100,
                CourtFinesFrequency = "monthly",
                CCJs = 0,
                CCJsArrears = 0,
                CCJsFrequency = string.Empty,
                OtherExpenditure = 0,
                OtherExpenditureFrequency = string.Empty,
            };

            List<SaveOtherDebtsApiModel> result = _helper.CreateOtherDebts(iAndE);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(123.45M, result[0].Amount);
            Assert.AreEqual(100, result[0].Arrears);
            Assert.AreEqual(false, result[0].CountyCourtJudgement);
            Assert.AreEqual("M", result[0].Frequency);
        }

        [TestMethod]
        public void CreateOtherDebtsTest_CCJs()
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure()
            {
                CourtFines = 0,
                CourtFinesArrears = 0,
                CourtFinesFrequency = string.Empty,
                CCJs = 123.45M,
                CCJsArrears = 100,
                CCJsFrequency = "weekly",
                OtherExpenditure = 0,
                OtherExpenditureFrequency = string.Empty,
            };

            List<SaveOtherDebtsApiModel> result = _helper.CreateOtherDebts(iAndE);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(123.45M, result[0].Amount);
            Assert.AreEqual(100, result[0].Arrears);
            Assert.AreEqual(true, result[0].CountyCourtJudgement);
            Assert.AreEqual("W", result[0].Frequency);
        }

        [TestMethod]
        public void CreateOtherDebtsTest_OtherExpenditure()
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure()
            {
                CourtFines = 0,
                CourtFinesArrears = 0,
                CourtFinesFrequency = string.Empty,
                CCJs = 0,
                CCJsArrears = 0,
                CCJsFrequency = string.Empty,
                OtherExpenditure = 123.45M,
                OtherExpenditureFrequency = "fortnightly",
            };

            List<SaveOtherDebtsApiModel> result = _helper.CreateOtherDebts(iAndE);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(123.45M, result[0].Amount);
            Assert.AreEqual(0, result[0].Arrears);
            Assert.AreEqual(false, result[0].CountyCourtJudgement);
            Assert.AreEqual("F", result[0].Frequency);
        }

        [TestMethod]
        public void CreateOtherDebtsTest_All()
        {
            IncomeAndExpenditure iAndE = new IncomeAndExpenditure()
            {
                CourtFines = 111.11M,
                CourtFinesArrears = 100,
                CourtFinesFrequency = "monthly",
                CCJs = 222.22M,
                CCJsArrears = 200,
                CCJsFrequency = "weekly",
                OtherExpenditure = 333.33M,
                OtherExpenditureFrequency = "fortnightly",
            };

            List<SaveOtherDebtsApiModel> result = _helper.CreateOtherDebts(iAndE);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(111.11M, result[0].Amount);
            Assert.AreEqual(100, result[0].Arrears);
            Assert.AreEqual(false, result[0].CountyCourtJudgement);
            Assert.AreEqual("M", result[0].Frequency);

            Assert.AreEqual(222.22M, result[1].Amount);
            Assert.AreEqual(200, result[1].Arrears);
            Assert.AreEqual(true, result[1].CountyCourtJudgement);
            Assert.AreEqual("W", result[1].Frequency);

            Assert.AreEqual(333.33M, result[2].Amount);
            Assert.AreEqual(0, result[2].Arrears);
            Assert.AreEqual(false, result[2].CountyCourtJudgement);
            Assert.AreEqual("F", result[2].Frequency);
        }

    }
}

