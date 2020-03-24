using System.Collections.Generic;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IGetDaysForDoBProcess
    {
        List<KeyValuePair<int?, string>> Build();
    }
}
