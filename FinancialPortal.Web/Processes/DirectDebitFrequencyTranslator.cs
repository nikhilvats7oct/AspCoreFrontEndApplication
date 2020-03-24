using FinancialPortal.Web.Processes.Interfaces;

namespace FinancialPortal.Web.Processes
{
    //
    // Translates frequency descriptions used by Service and Portal into
    // values that are compatible with JayWing scripts
    //
    // TODO: Long term the scripts should be changed to be data driven or at least
    // TODO: use the same descriptions as the server side
    //
    public class DirectDebitFrequencyTranslator : IDirectDebitFrequencyTranslator
    {
        private const string SpecialDescription4Weeks = "every 4 weeks";
        private const string SpecialValue4Weeks = "4week";
        private const string CaseFlowValue4Weeks = "four weekly";
        public string TranslateDescriptionToClientScriptCompatibleValue(string frequencyDescription)
        {
            // TODO: instead of this, perhaps script could be changed on Payment Options so that display names are consistent with values
            if (frequencyDescription.ToLower() == SpecialDescription4Weeks) return SpecialValue4Weeks;
            return frequencyDescription.ToLower();

        }

        public string TranslateClientScriptCompatibleValueToDescription(string clientScriptCompatibleValue)
        {
            if (clientScriptCompatibleValue.ToLower() == SpecialValue4Weeks) return SpecialDescription4Weeks;
            return clientScriptCompatibleValue.ToLower();
        }

        public string TranslateCaseFlow4WeeklyValueToClientScriptValue(string caseFlow4WeeklyValue)
        {
            if (caseFlow4WeeklyValue.ToLower() == CaseFlowValue4Weeks) return SpecialValue4Weeks;
            return caseFlow4WeeklyValue.ToLower();
        }
    }
}
