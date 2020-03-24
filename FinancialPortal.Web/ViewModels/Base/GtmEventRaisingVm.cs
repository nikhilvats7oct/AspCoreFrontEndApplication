using System;
using System.Collections.Generic;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Models.Interfaces;

namespace FinancialPortal.Web.ViewModels.Base
{
    // Used when a View Model needs to contain information for Google Analytics
    // Can be populated via prior controller actions, so that it can be consumed by scripts on the receiving page
    [Serializable()]
    public class GtmEventRaisingVm : IGtmEventRaisingVm
    {
        public List<GtmEvent> GtmEvents { get; set; } = new List<GtmEvent>();
    }
}
