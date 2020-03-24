namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IDirectDebitFrequencyTranslator
    {
        string TranslateDescriptionToClientScriptCompatibleValue(string frequencyDescription);
        string TranslateClientScriptCompatibleValueToDescription(string clientScriptCompatibleValue);
        string TranslateCaseFlow4WeeklyValueToClientScriptValue(string caseFlow4WeeklyValue);
    }
}
