using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Services.Models
{
    public class IncomeAndExpenditure
    {
        public string LowellReference { get; set; }

        public string User { get; set; }
        public DateTime Created { get; set; }
        public bool HasArrears { get; set; }

        public int AdultsInHousehold { get; set; }
        public int ChildrenUnder16 { get; set; }
        public int Children16to18 { get; set; }

        public decimal Salary { get; set; }
        public string SalaryFrequency { get; set; }

        public decimal Pension { get; set; }
        public string PensionFrequency { get; set; }

        public decimal EarningsTotal { get; set; }
        public string EarningsTotalFrequency { get; set; }

        public decimal BenefitsTotal { get; set; }
        public string BenefitsTotalFrequency { get; set; }

        public decimal OtherIncome { get; set; }
        public string OtherincomeFrequency { get; set; }

        public decimal Mortgage { get; set; }
        public string MortgageFrequency { get; set; }
        public decimal MortgageArrears { get; set; }

        public decimal Rent { get; set; }
        public string RentFrequency { get; set; }
        public decimal RentArrears { get; set; }

        public decimal SecuredLoans { get; set; }
        public string SecuredLoansFrequency { get; set; }
        public decimal SecuredloansArrears { get; set; }

        public decimal CouncilTax { get; set; }
        public string CouncilTaxFrequency { get; set; }
        public decimal CouncilTaxArrears { get; set; }

        public decimal Rental { get; set; }
        public string RentalFrequency { get; set; }
        public decimal RentalArrears { get; set; }

        public decimal TvLicence { get; set; }
        public string TvLicenceFrequency { get; set; }
        public decimal TvLicenceArrears { get; set; }

        public decimal HomeContents { get; set; }
        public string HomeContentsFrequency { get; set; }
        public decimal HomeContentsArrears { get; set; }

        public decimal Gas { get; set; }
        public string GasFrequency { get; set; }
        public decimal GasArrears { get; set; }

        public decimal OtherUtilities { get; set; }
        public string OtherUtilitiesFrequency { get; set; }
        public decimal OtherUtilitiesArrears { get; set; }

        public decimal Electricity { get; set; }
        public string ElectricityFrequency { get; set; }
        public decimal ElectricityArrears { get; set; }

        public decimal Water { get; set; }
        public string WaterFrequency { get; set; }
        public decimal WaterArrears { get; set; }

        public decimal UtilitiesTotal { get; set; }
        public string UtilitiesTotalFrequency { get; set; }
        public decimal UtilitiesTotalArrears { get; set; }

        public decimal ChildMaintenance { get; set; }
        public string ChildMaintenanceFrequency { get; set; }
        public decimal ChildMaintenanceArrears { get; set; }
        public decimal CCJs { get; set; }
        public string CCJsFrequency { get; set; }
        public decimal CCJsArrears { get; set; }
        public decimal CourtFines { get; set; }
        public string CourtFinesFrequency { get; set; }
        public decimal CourtFinesArrears { get; set; }

        public decimal Housekeeping { get; set; }
        public string HousekeepingFrequency { get; set; }

        public decimal PersonalCosts { get; set; }
        public string PersonalCostsFrequency { get; set; }

        public decimal Leisure { get; set; }
        public string LeisureFrequency { get; set; }

        public decimal Travel { get; set; }
        public string TravelFrequency { get; set; }

        public decimal Healthcare { get; set; }
        public string HealthcareFrequency { get; set; }

        public decimal PensionInsurance { get; set; }
        public string PensionInsuranceFrequency { get; set; }

        public decimal SchoolCosts { get; set; }
        public string SchoolCostsFrequency { get; set; }

        public decimal ProfessionalCosts { get; set; }
        public string ProfessionalCostsFrequency { get; set; }

        public decimal SavingsContributions { get; set; }
        public string SavingsContributionsFrequency { get; set; }
        public decimal OtherExpenditure { get; set; }
        public string OtherExpenditureFrequency { get; set; }

        public decimal IncomeTotal { get; set; }
        public decimal ExpenditureTotal { get; set; }
        public decimal DisposableIncome { get; set; }

        public string HousingStatus { get; set; }
        public string EmploymentStatus { get; set; }
        public List<SaveOtherDebts> OtherDebts { get; set; }

        public string BudgetSource { get; set; }
    }

    public class SaveOtherDebts
    {
        public decimal Amount { get; set; }
        public string Frequency { get; set; }
        public bool CountyCourtJudgement { get; set; }
        public decimal Arrears { get; set; }
    }
}
