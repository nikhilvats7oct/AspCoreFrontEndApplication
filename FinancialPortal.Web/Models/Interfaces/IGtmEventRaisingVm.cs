using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Models.Interfaces
{
    public interface IGtmEventRaisingVm
    {
        List<GtmEvent> GtmEvents { get; set; }
    }
}
