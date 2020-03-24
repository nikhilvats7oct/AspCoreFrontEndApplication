namespace FinancialPortal.Web.Services.Models.OpenWrks
{
    public class OpenWrksArrearsAndFine
    {
        public string Type { get; set; }

        public bool IsPaying { get; set; }

        public decimal? MonthlyAmount { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal? MonthlyHouseholdAmount { get; set; }

        public string Category { get; set; }
    }
}