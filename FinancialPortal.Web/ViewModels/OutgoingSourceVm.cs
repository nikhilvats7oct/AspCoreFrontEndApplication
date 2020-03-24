using FinancialPortal.Web.ViewModels.Interfaces;

namespace FinancialPortal.Web.ViewModels
{
    public class OutgoingSourceVm : IOutgoingSourceVm
    {
        public decimal Amount { get; set; }
        public string Frequency { get; set; }
        public bool InArrears { get; set; }
        public decimal ArrearsAmount { get; set; }
    }
}
