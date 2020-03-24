namespace FinancialPortal.Web.ViewModels.Interfaces
{
    public interface IExpenditureSource
    {
        decimal Amount { get; set; }
        string Frequency { get; set; }
    }
}
