namespace FinancialPortal.Web.Services.Models.OpenWrks
{
    public class OpenWrksCardAndLoan
    {
        public string Type { get; set; }

        public string Description { get; set; }

        public decimal MonthlyAmount { get; set; }
        public decimal MonthlyHouseholdAmount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}