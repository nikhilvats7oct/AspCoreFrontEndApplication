using System.Globalization;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Settings;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Processes
{
    public class ArrearsDescriptionProcess : IArrearsDescriptionProcess
    {
        private readonly ILogger<ArrearsDescriptionProcess> _logger;
        private readonly PortalSetting _portalSetting;

        public ArrearsDescriptionProcess(ILogger<ArrearsDescriptionProcess> logger,
            PortalSetting portalSetting)
        {
            _logger = logger;
            _portalSetting = portalSetting;
        }

        public string DeriveArrearsSummary(decimal? paymentPlanArrearsAmount)
        {
            if (paymentPlanArrearsAmount > 0)
            {
                string amountString = paymentPlanArrearsAmount.Value.ToString("C", CultureInfo.CurrentCulture);
                var talkToUslink = _portalSetting.TalkToUsUrl;
                return $"In arrears<br>If you are able to, please make your payment today or <a target=\"_blank\" href=\"{talkToUslink}\">talk to us</a> for help.";
            }

            return null;
        }

        public string DeriveArrearsDetail(decimal? paymentPlanArrearsAmount, bool paymentPlanIsAutomated)
        {
            if (paymentPlanArrearsAmount > 0)
            {
                string amountString = paymentPlanArrearsAmount.Value.ToString("C", CultureInfo.CurrentCulture);
                var talkToUslink = _portalSetting.TalkToUsUrl;
                if (paymentPlanIsAutomated)
                    return $"Unfortunately, your last payment didn’t go through as planned and your account has fallen in to arrears.<br><br>If you are able to you can get your plan up to date by making a one-off payment of {amountString}.<br><br>After that, your next payment will be taken as normal. Don’t worry if you need to change your plan, you can do it after you’ve made this payment. <a target=\"_blank\" href=\"{talkToUslink}\">talk to us</a> or call us on 0333 556 5550 and we’ll be happy to help.";
                return $"Unfortunately, your last payment didn’t go through as planned and your account has fallen in to arrears.<br><br>If you are able to you can get your plan up to date by making a one-off payment of {amountString}.<br><br>After that, your next payment will be taken as normal. Don’t worry if you need to change your plan, you can do it after you’ve made this payment. <a target=\"_blank\" href=\"{talkToUslink}\">talk to us</a> or call us on 0333 556 5550 and we’ll be happy to help.";


            }


            return null;
        }

    }
}
