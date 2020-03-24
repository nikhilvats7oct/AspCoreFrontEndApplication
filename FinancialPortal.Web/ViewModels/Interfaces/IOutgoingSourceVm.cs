namespace FinancialPortal.Web.ViewModels.Interfaces
{
    public interface IOutgoingSourceVm
    {
        decimal Amount { get; set; }
        string Frequency { get; set; }
        bool InArrears { get; set; }
        decimal ArrearsAmount { get; set; }
    }
}
