using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialPortal.Web.Services
{
    public class CalculatorService : ICalculatorService
    {
        public MonthlyIncome CalculateMonthlyIncome(IncomeAndExpenditure iAndE)
        {
            if (iAndE != null)
            {
                var salary = ConvertToMonthly(iAndE.Salary, iAndE.SalaryFrequency);
                var benefits = ConvertToMonthly(iAndE.BenefitsTotal, iAndE.BenefitsTotalFrequency);
                var pension = ConvertToMonthly(iAndE.Pension, iAndE.PensionFrequency);
                var other = ConvertToMonthly(iAndE.OtherIncome, iAndE.OtherincomeFrequency);

                var total = salary + benefits + other + pension;

                return new MonthlyIncome()
                {
                    Salary = salary,
                    Benefits = benefits,
                    Other = other,
                    Pension = pension,
                    Total = total,
                };
            }

            return new MonthlyIncome()
            {
                Salary = 0.00M,
                Benefits = 0.00M,
                Other = 0.00M,
                Pension = 0.00M,
                Total = 0.00M
            };
        }

        public MonthlyOutgoings CalculateMonthlyOutgoings(IncomeAndExpenditure iAndE)
        {
            decimal householdBills = 0.00M;
            decimal arrears = 0.00M;

            if (iAndE != null)
            {
                householdBills = CalculateMonthlyBillsAndOutgoings(iAndE);
                arrears = AddArrears(iAndE);

                decimal expenditure = CalculateMonthlyExpenditure(iAndE);


                decimal total = householdBills + expenditure + arrears;

                return new MonthlyOutgoings()
                {
                    HouseholdBills = householdBills + arrears,
                    Expenditures = expenditure,
                    Total = total
                };
            }

            return new MonthlyOutgoings()
            {
                HouseholdBills = householdBills + arrears,
                Total = householdBills + arrears
            };

        }

        public decimal CalculateDisposableIncome(decimal income, decimal expenditure)
        {
            var disposable = income - expenditure;

            return Math.Round(disposable, 2, MidpointRounding.AwayFromZero);
        }

        public decimal CalculateMonthlyExpenditure(IncomeAndExpenditure iAndE)
        {
            var totalDebts = CalculateOtherDebts(iAndE);

            var expenditure = ConvertToMonthly(iAndE.Housekeeping, iAndE.HousekeepingFrequency) +
                   ConvertToMonthly(iAndE.PersonalCosts, iAndE.PersonalCostsFrequency) +
                   ConvertToMonthly(iAndE.Leisure, iAndE.LeisureFrequency) +
                   ConvertToMonthly(iAndE.Travel, iAndE.TravelFrequency) +
                   ConvertToMonthly(iAndE.Healthcare, iAndE.HealthcareFrequency) +
                   ConvertToMonthly(iAndE.PensionInsurance, iAndE.PensionInsuranceFrequency) +
                   ConvertToMonthly(iAndE.SchoolCosts, iAndE.SchoolCostsFrequency) +
                   ConvertToMonthly(iAndE.ProfessionalCosts, iAndE.ProfessionalCostsFrequency) +
                   ConvertToMonthly(iAndE.SavingsContributions, iAndE.SavingsContributionsFrequency) +
                   ConvertToMonthly(iAndE.OtherExpenditure, iAndE.OtherExpenditureFrequency);

            return expenditure + totalDebts;
        }

        private decimal AddArrears(IncomeAndExpenditure iAndE)
        {
            var arrears = CalculateArrears(iAndE.MortgageArrears, iAndE.MortgageArrears > 0) +
                   CalculateArrears(iAndE.RentArrears, iAndE.RentArrears > 0) +
                   CalculateArrears(iAndE.SecuredloansArrears, iAndE.SecuredloansArrears > 0) +
                   CalculateArrears(iAndE.CouncilTaxArrears, iAndE.CouncilTaxArrears > 0) +
                   CalculateArrears(iAndE.RentalArrears, iAndE.RentalArrears > 0) +
                   CalculateArrears(iAndE.TvLicenceArrears, iAndE.TvLicenceArrears > 0) +
                   CalculateArrears(iAndE.GasArrears, iAndE.GasArrears > 0) +
                   CalculateArrears(iAndE.OtherUtilitiesArrears, iAndE.OtherUtilitiesArrears > 0) +
                   CalculateArrears(iAndE.ElectricityArrears, iAndE.ElectricityArrears > 0) +
                   CalculateArrears(iAndE.WaterArrears, iAndE.WaterArrears > 0) +
                   CalculateArrears(iAndE.ChildMaintenanceArrears, iAndE.ChildMaintenanceArrears > 0) +
                   CalculateArrears(iAndE.CourtFinesArrears, iAndE.CourtFinesArrears > 0) +
                   CalculateArrears(iAndE.CCJsArrears, iAndE.CCJsArrears > 0);

            var otherDebtsArrears = iAndE.OtherDebts==null? 0.00M:  iAndE.OtherDebts.Sum(x => x.Arrears);

            return arrears + otherDebtsArrears;
        }

        public decimal CalculateMonthlyBillsAndOutgoings(IncomeAndExpenditure iAndE)
        {
            return ConvertToMonthly(iAndE.Mortgage, iAndE.MortgageFrequency) +
                   ConvertToMonthly(iAndE.Rent, iAndE.RentFrequency) +
                   ConvertToMonthly(iAndE.SecuredLoans, iAndE.SecuredLoansFrequency) +
                   ConvertToMonthly(iAndE.CouncilTax, iAndE.CouncilTaxFrequency) +
                   ConvertToMonthly(iAndE.Rental, iAndE.RentalFrequency) +
                   ConvertToMonthly(iAndE.TvLicence, iAndE.TvLicenceFrequency) +
                   ConvertToMonthly(iAndE.Gas, iAndE.GasFrequency) +
                   ConvertToMonthly(iAndE.OtherUtilities, iAndE.OtherUtilitiesFrequency) +
                   ConvertToMonthly(iAndE.Electricity, iAndE.ElectricityFrequency) +
                   ConvertToMonthly(iAndE.Water, iAndE.WaterFrequency) +
                   ConvertToMonthly(iAndE.ChildMaintenance, iAndE.ChildMaintenanceFrequency) +
                   ConvertToMonthly(iAndE.CourtFines, iAndE.CourtFinesFrequency) +
                   ConvertToMonthly(iAndE.CCJs, iAndE.CCJsFrequency);
        }

        public decimal ConvertToMonthly(decimal amount, string frequency)
        {
            if (string.IsNullOrEmpty(frequency)) { return 0; }

            switch (frequency.ToLower())
            {
                case "m": return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
                case "monthly": return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
                case "w": return Math.Round(amount * 4.33M, 2, MidpointRounding.AwayFromZero);
                case "weekly": return Math.Round(amount * 4.33M, 2, MidpointRounding.AwayFromZero);
                case "f": return Math.Round(amount * 2.17M, 2, MidpointRounding.AwayFromZero);
                case "fortnightly": return Math.Round(amount * 2.17M, 2, MidpointRounding.AwayFromZero);
                case "4": return Math.Round(amount * 1.08M, 2, MidpointRounding.AwayFromZero);
                case "4week": return Math.Round(amount * 1.08M, 2, MidpointRounding.AwayFromZero);
                case "q": return Math.Round(amount / 3, 2, MidpointRounding.AwayFromZero);
                case "quarterly": return Math.Round(amount / 3, 2, MidpointRounding.AwayFromZero);
                case "l": return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
                case "a": return Math.Round(amount / 12, 2, MidpointRounding.AwayFromZero);
                case "annually": return Math.Round(amount / 12, 2, MidpointRounding.AwayFromZero);
                default: return 0;
            }
        }

        private decimal CalculateArrears(decimal? amount, bool inArrears)
        {
            if (inArrears && amount > 0.00M)
            {
                return Math.Round(amount ?? 0.00M, 2, MidpointRounding.AwayFromZero);
            }

            return 0.00M;
        }

        private decimal CalculateOtherDebts(List<SaveOtherDebts> otherDebts )
        {
            var totalDebts = 0.00M;

            otherDebts.ForEach(debt =>
            {
                if (debt.Amount > 0.00M)
                {
                    var monthlyDebt = ConvertToMonthly(debt.Amount, debt.Frequency);

                    totalDebts += monthlyDebt;
                }
            });

            return Math.Round(totalDebts, 2, MidpointRounding.AwayFromZero);
        }

        public bool InArrears(IncomeAndExpenditure iAndE)
        {
            return (iAndE.RentalArrears > 0 ||
                    iAndE.CCJsArrears > 0 ||
                    iAndE.ChildMaintenanceArrears > 0 ||
                    iAndE.CouncilTaxArrears > 0 ||
                    iAndE.CourtFinesArrears > 0 ||
                    iAndE.ElectricityArrears > 0 ||
                    iAndE.GasArrears > 0 ||
                    iAndE.MortgageArrears > 0 ||
                    iAndE.OtherUtilitiesArrears > 0 ||
                    iAndE.RentArrears > 0 ||
                    iAndE.SecuredloansArrears > 0 ||
                    iAndE.TvLicenceArrears > 0 ||
                    iAndE.WaterArrears > 0);
        }

        public decimal CalculateOtherDebts(IncomeAndExpenditure iAndE)
        {
            if (iAndE.OtherDebts != null && iAndE.OtherDebts.Any())
            {
                return CalculateOtherDebts(iAndE.OtherDebts);
            }

            return 0.00M;
        }
    }
}