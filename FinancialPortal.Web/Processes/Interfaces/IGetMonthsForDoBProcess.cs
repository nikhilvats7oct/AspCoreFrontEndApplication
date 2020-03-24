using System.Collections.Generic;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IGetMonthsForDoBProcess
    {
        List<KeyValuePair<string, string>> Build();
    }
}
