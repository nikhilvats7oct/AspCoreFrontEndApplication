using System.Collections.Generic;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IBuildFrequencyListProcess
    {
        List<DirectDebitPaymentFrequencyVm> BuildFrequencyList(IList<string> stringList);
    }
}