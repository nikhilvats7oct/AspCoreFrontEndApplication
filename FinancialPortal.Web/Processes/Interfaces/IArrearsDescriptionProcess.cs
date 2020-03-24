namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface IArrearsDescriptionProcess
    {
        string DeriveArrearsSummary(decimal? paymentPlanArrearsAmount);

        string DeriveArrearsDetail(decimal? paymentPlanArrearsAmount, bool paymentPlanIsAutomated);
    }
}
