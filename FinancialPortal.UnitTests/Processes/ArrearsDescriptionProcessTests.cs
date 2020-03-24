using System;
using System.Collections.Generic;
using System.Text;
using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class ArrearsDescriptionProcessTests
    {
        private Mock<ILogger<ArrearsDescriptionProcess>> _mockLogger;
        private IArrearsDescriptionProcess _arrearsDescriptionService;

        [TestInitialize]
        public void Initialise()
        {
            _mockLogger = new Mock<ILogger<ArrearsDescriptionProcess>>();
            _arrearsDescriptionService = new ArrearsDescriptionProcess(_mockLogger.Object,new PortalSetting());
        }

        [TestMethod]

        //
        // Not in arrears - suppress message
        //
        [DataRow(null, null)]
        [DataRow(null, null)]

        // Non-positive values also suppress message
        [DataRow(0.0d, null)]
        [DataRow(-0.01d, null)]

        //
        // Arrears - displays appropriate message
        //
        [DataRow(1.0d, "In arrears<br>If you are able to, please make your payment today or <a target=\"_blank\" href=\"\">talk to us</a> for help.")]

        // Various values
        [DataRow(0.01d, "In arrears<br>If you are able to, please make your payment today or <a target=\"_blank\" href=\"\">talk to us</a> for help.")]
        [DataRow(9999.99d, "In arrears<br>If you are able to, please make your payment today or <a target=\"_blank\" href=\"\">talk to us</a> for help.")]

        [DataRow(1000.00d, "In arrears<br>If you are able to, please make your payment today or <a target=\"_blank\" href=\"\">talk to us</a> for help.")]
        [DataRow(12345678.09d, "In arrears<br>If you are able to, please make your payment today or <a target=\"_blank\" href=\"\">talk to us</a> for help.")]

        public void DeriveArrearsSummary_GivenArrearsInputFields_DerivesShortDescription(
            double? testPaymentPlanArrearsAmount,
            string testArrearsMessage)
        {
            string description = _arrearsDescriptionService.DeriveArrearsSummary((decimal?)testPaymentPlanArrearsAmount);

            Assert.AreEqual(testArrearsMessage, description);
        }


        [TestMethod]
        //
        // Not in arrears - suppress message, regardless of 'Is Automated' flag
        //
        [DataRow(null, false, null)]
        [DataRow(null, true, null)]

        // Non-positive values also suppress message
        [DataRow(0.0d, true, null)]
        [DataRow(-0.01d, true, null)]

        //
        // Arrears - displays appropriate message according to 'Is Automated'
        //
        [DataRow(1.0d, false, "We noticed your account isn’t up to date. Missed payments mean you risk defaulting on your plan.<br> Make a payment of £10.00 today and get back on track.<br><br><b>Catch up with a one-off payment.</b><br>You can make an extra payment, just use our one-off payment options. It’s a good way to pay the arrears and put your mind at rest. Any amount above the arrears value will lower your balance and reduce the number of instalments you need to pay. The one-off payment won’t affect your normal agreed payment plan which will continue as usual. ")]
        [DataRow(1.0d, true, "We noticed your account isn’t up to date. Missed payments mean you risk defaulting on your plan.<br> Make a payment of £10.00 today and get back on track.<br><br><b>Catch up with a one-off payment.</b><br>You can make an extra payment, just use our one-off payment options. It’s a good way to pay the arrears and put your mind at rest. Any amount above the arrears value will lower your balance and reduce the number of instalments you need to pay. The one-off payment won’t affect your normal agreed payment plan which will continue as usual. ")]

        //// Various values
        [DataRow(0.01d, false, "We noticed your account isn’t up to date. Missed payments mean you risk defaulting on your plan.<br> Make a payment of £10.00 today and get back on track.<br><br><b>Catch up with a one-off payment.</b><br>You can make an extra payment, just use our one-off payment options. It’s a good way to pay the arrears and put your mind at rest. Any amount above the arrears value will lower your balance and reduce the number of instalments you need to pay. The one-off payment won’t affect your normal agreed payment plan which will continue as usual. ")]
        [DataRow(9999.99d, false, "We noticed your account isn’t up to date. Missed payments mean you risk defaulting on your plan.<br> Make a payment of £10.00 today and get back on track.<br><br><b>Catch up with a one-off payment.</b><br>You can make an extra payment, just use our one-off payment options. It’s a good way to pay the arrears and put your mind at rest. Any amount above the arrears value will lower your balance and reduce the number of instalments you need to pay. The one-off payment won’t affect your normal agreed payment plan which will continue as usual. ")]

        [DataRow(1000.00d, true, "We noticed your account isn’t up to date. Missed payments mean you risk defaulting on your plan.<br> Make a payment of £10.00 today and get back on track.<br><br><b>Catch up with a one-off payment.</b><br>You can make an extra payment, just use our one-off payment options. It’s a good way to pay the arrears and put your mind at rest. Any amount above the arrears value will lower your balance and reduce the number of instalments you need to pay. The one-off payment won’t affect your normal agreed payment plan which will continue as usual. ")]
        [DataRow(12345678.09d, true, "We noticed your account isn’t up to date. Missed payments mean you risk defaulting on your plan.<br> Make a payment of £10.00 today and get back on track.<br><br><b>Catch up with a one-off payment.</b><br>You can make an extra payment, just use our one-off payment options. It’s a good way to pay the arrears and put your mind at rest. Any amount above the arrears value will lower your balance and reduce the number of instalments you need to pay. The one-off payment won’t affect your normal agreed payment plan which will continue as usual. ")]

        public void DeriveArrearsDetail_GivenArrearsInputFields_DerivesShortDescription(
            double? testPaymentPlanArrearsAmount, bool testPaymentPlanIsAutomated,
            string testArrearsMessage)
        {
            Assert.Inconclusive("Needs refactoring since content updates. Should not rely on config. 'Talk to Us' link should be provided from calling service, sourcing it from: IApplicationSettingsReader");

            string description = _arrearsDescriptionService.DeriveArrearsDetail((decimal?)testPaymentPlanArrearsAmount, testPaymentPlanIsAutomated);

            Assert.AreEqual(testArrearsMessage, description);
        }

    }
}
