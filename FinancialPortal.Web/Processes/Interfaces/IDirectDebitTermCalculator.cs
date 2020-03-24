namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IDirectDebitTermCalculator
    {
        int CalculateTermInMonths(decimal balance, decimal paymentAmount, string paymentFrequency);
    }
}
