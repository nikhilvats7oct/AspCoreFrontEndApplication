namespace FinancialPortal.Web.Services.Models
{
    public class ExpenditureMetrics
    {
        public ExpenditureMetric CommsAndLeisure { get; set; } = new ExpenditureMetric();
        public ExpenditureMetric FoodAndHousekeeping { get; set; } = new ExpenditureMetric();
        public ExpenditureMetric PersonalCosts { get; set; } = new ExpenditureMetric();


    }
}