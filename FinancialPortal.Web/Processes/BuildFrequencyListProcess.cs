using System.Collections.Generic;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Processes
{
    public class BuildFrequencyListProcess : IBuildFrequencyListProcess
    {
        public List<DirectDebitPaymentFrequencyVm> BuildFrequencyList(IList<string> stringList)
        {
            List<DirectDebitPaymentFrequencyVm> vmList = new List<DirectDebitPaymentFrequencyVm>();

            foreach (string s in stringList)
            {
                vmList.Add(new DirectDebitPaymentFrequencyVm()
                {
                    Value = FrequencyStringIntoValue(s),
                    DisplayedText = s
                });
            }

            return vmList;
        }

        string FrequencyStringIntoValue(string s)
        {
            // TODO: instead of this, perhaps script could be changed on Payment Options so that display names are consistent with values
            if (s == "Every 4 weeks") return "4week";
            return s.ToLower();
        }
    }
}