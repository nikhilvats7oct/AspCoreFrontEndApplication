namespace FinancialPortal.Web.Services.Models.OpenWrks
{
    public class OpenWrksBudget
    {
        public string CustomerReference { get; set; }

        public decimal DisposableIncome { get; set; }

        public decimal MoneyLeft { get; set; }

        public OpenWrksIncome[] Income { get; set; }

        public OpenWrksExpenditure[] Expenditure { get; set; }

        public OpenWrksCardAndLoan[] CardsAndLoans { get; set; }

        public OpenWrksArrearsAndFine[] ArrearsAndFines { get; set; }

        public OpenWrksHousehold Household { get; set; }

        public string Message { get; set; }
    }
}
