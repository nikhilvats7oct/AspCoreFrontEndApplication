using FinancialPortal.Web.Processes;
using FinancialPortal.Web.Processes.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialPortal.UnitTests.Processes
{
    [TestClass]
    public class DirectDebitFrequencyTranslatorTests
    {
        private IDirectDebitFrequencyTranslator _directDebitFrequencyTranslator;

        [TestInitialize]
        public void Initialise()
        {
            _directDebitFrequencyTranslator = new DirectDebitFrequencyTranslator();
        }

        [TestMethod]
        [DataRow(1, "Every 4 weeks", "4week")]
        [DataRow(2, "evERy 4 wEEks", "4week")]      // don't care about case
        public void TranslateDescriptionToClientScriptCompatibleValue_ConvertsDescriptionsRequiringSpecialTranslation(
            int testNumber, string testDescription, string expectedClientScriptCompatibleValue)
        {
            Assert.AreEqual(expectedClientScriptCompatibleValue, _directDebitFrequencyTranslator.TranslateDescriptionToClientScriptCompatibleValue(testDescription));
        }

        [TestMethod]
        [DataRow(1, "AbC", "abc")]
        [DataRow(2, "Weekly", "weekly")]
        [DataRow(3, "Monthly", "monthly")]
        [DataRow(4, "Fortnightly", "fortnightly")]
        [DataRow(5, "every 4 weeks ", "every 4 weeks ")]     // must exactly match, aside from case (note space at end)
        public void TranslateDescriptionToClientScriptCompatibleValue_ConvertsOtherDescriptionsToLowerCase(
            int testNumber, string testDescription, string expectedClientScriptCompatibleValue)
        {
            Assert.AreEqual(expectedClientScriptCompatibleValue, _directDebitFrequencyTranslator.TranslateDescriptionToClientScriptCompatibleValue(testDescription));
        }

        [TestMethod]
        [DataRow(1, "4week", "every 4 weeks")]      // lower case for consistency
        [DataRow(2, "4WeeK", "every 4 weeks")]      // ignores case of value
        public void TranslateClientScriptCompatibleValueToDescription_ConvertsSpecialTranslationsBackToDescriptions(
            int testNumber, string testClientScriptCompatibleValue, string expectedDescription)
        {
            Assert.AreEqual(expectedDescription, _directDebitFrequencyTranslator.TranslateClientScriptCompatibleValueToDescription(testClientScriptCompatibleValue));
        }

        [TestMethod]
        [DataRow(1, "abc", "abc")]
        [DataRow(2, "aBc", "abc")]          // changes to lowercase for constency. In practice should already be made lowercase by other method.
        [DataRow(3, "4week ", "4week ")]    // must exactly match, aside from case (note space at end)
        public void TranslateClientScriptCompatibleValueToDescription_ConvertsOtherValuesBackToDescriptions(
            int testNumber, string testClientScriptCompatibleValue, string expectedDescription)
        {
            Assert.AreEqual(expectedDescription, _directDebitFrequencyTranslator.TranslateClientScriptCompatibleValueToDescription(testClientScriptCompatibleValue));
        }

        [TestMethod]
        [DataRow(1, "Every 4 weeks", "4week", "every 4 weeks")]     // loses uppercase letters
        [DataRow(2, "Weekly", "weekly", "weekly")]
        [DataRow(3, "Monthly", "monthly", "monthly")]
        [DataRow(4, "Fortnightly", "fortnightly", "fortnightly")]
        public void Translate_BothMethodsRoundTrip_ComesBackWithJustCaseLost(
            int testNumber, string testDescription, string expectedClientScriptCompatibleValue,
            string expectedRoundTripDescription)
        {
            string valueReturned = _directDebitFrequencyTranslator.TranslateDescriptionToClientScriptCompatibleValue(testDescription);
            Assert.AreEqual(expectedClientScriptCompatibleValue, valueReturned);

            Assert.AreEqual(expectedRoundTripDescription, _directDebitFrequencyTranslator.TranslateClientScriptCompatibleValueToDescription(valueReturned));
        }
    }
}
