using System.Collections.Generic;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IGetYearsForDoBProcess
    {
        List<KeyValuePair<int?, string>> Build(int minYear, int maxYear);
    }
}
