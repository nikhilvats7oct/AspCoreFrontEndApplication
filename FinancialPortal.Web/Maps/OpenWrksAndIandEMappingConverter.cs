using AutoMapper;
using FinancialPortal.Web.Models;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Services.Models.OpenWrks;
using System;
using System.Collections.Generic;

namespace FinancialPortal.Web.Maps
{
    public class OpenWrksAndIandEMappingConverter : ITypeConverter<OpenWrksBudget, IncomeAndExpenditure>
    {
        private const string _monthly = "monthly";

        public IncomeAndExpenditure Convert(OpenWrksBudget source, IncomeAndExpenditure destination, ResolutionContext context)
        {
            if (source == null) { return null; }
            
            if (destination == null)
            {
                destination = new IncomeAndExpenditure();
            }

            if (destination.OtherDebts == null)
            {
                destination.OtherDebts = new List<SaveOtherDebts>();
            }

            destination = MapHousehold(source, destination);
            destination = MapIncome(source, destination);
            destination = MapExpenditure(source, destination);
            destination = MapArrearsAndFines(source, destination);
            destination = MapCardsAndLoans(source, destination); 
   
            return destination;
        }

        private IncomeAndExpenditure MapHousehold(OpenWrksBudget source, IncomeAndExpenditure destination)
        {
            destination.AdultsInHousehold = source.Household.LivesWithPartner ? 2 : 1;
            destination.AdultsInHousehold += source.Household.DependentAdults + source.Household.NonDependentCohabitants;
            destination.Children16to18 = source.Household.DependentYoungAdults;
            destination.ChildrenUnder16 = source.Household.DependentChildren;
            return destination;            
        }

        private IncomeAndExpenditure MapIncome(OpenWrksBudget source, IncomeAndExpenditure destination)
        {
            foreach (var income in source.Income)
            {
                switch (income.SfsCategory.ToUpper())
                {
                    case Constants.OpenWrksIncomeAndExpenditures.Salary:
                        {
                            destination.Salary = income.MonthlyAmount;
                            destination.SalaryFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.PartnerSalary:
                    case Constants.OpenWrksIncomeAndExpenditures.OtherEarnings:
                    case Constants.OpenWrksIncomeAndExpenditures.OtherIncome:
                    case Constants.OpenWrksIncomeAndExpenditures.StudentGrants:
                    case Constants.OpenWrksIncomeAndExpenditures.ChildSupport:
                    case Constants.OpenWrksIncomeAndExpenditures.OtherPensions:
                        {
                            destination.OtherIncome += income.MonthlyAmount;
                            destination.OtherincomeFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.DisabilityBenefits:
                    case Constants.OpenWrksIncomeAndExpenditures.CarersAllowance:
                    case Constants.OpenWrksIncomeAndExpenditures.EmploymentSupport:
                    case Constants.OpenWrksIncomeAndExpenditures.IncomeSupport:
                    case Constants.OpenWrksIncomeAndExpenditures.JobSeekersAllowance:
                    case Constants.OpenWrksIncomeAndExpenditures.LocalHousingAllowance:
                    case Constants.OpenWrksIncomeAndExpenditures.OtherBenefits:
                    case Constants.OpenWrksIncomeAndExpenditures.UniversalCredit:
                    case Constants.OpenWrksIncomeAndExpenditures.WorkingTaxCredit:                  
                 
                        {
                            destination.BenefitsTotal += income.MonthlyAmount;
                            destination.BenefitsTotalFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.StatePensions:
                    case Constants.OpenWrksIncomeAndExpenditures.PensionCredit:
                    case Constants.OpenWrksIncomeAndExpenditures.PrivateOrWorkPension:
                        {
                            destination.Pension += income.MonthlyAmount;
                            destination.PensionFrequency = _monthly;
                            break;
                        }
                  
                }
            }

            destination.EarningsTotal = destination.Salary + destination.BenefitsTotal + destination.Pension + destination.OtherIncome;
            destination.EarningsTotalFrequency = _monthly;
            destination.IncomeTotal = destination.EarningsTotal;

            return destination;
        }

        private IncomeAndExpenditure MapExpenditure(OpenWrksBudget source, IncomeAndExpenditure destination)
        {
            foreach (var expenditure in source.Expenditure)
            {
                switch (expenditure.SfsCategory.ToUpper())
                {
                    case Constants.OpenWrksIncomeAndExpenditures.Mortgage:
                        {
                            destination.Mortgage = expenditure.MonthlyHouseholdAmount;
                            destination.MortgageFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.Rent:
                        {
                            destination.Rent += expenditure.MonthlyHouseholdAmount;
                            destination.RentFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.CouncilTax:
                        {
                            destination.CouncilTax += expenditure.MonthlyHouseholdAmount;
                            destination.CouncilTaxFrequency = _monthly;
                            break;
                        }

                    case Constants.OpenWrksIncomeAndExpenditures.TvLicence:
                        {
                            destination.TvLicence += expenditure.MonthlyHouseholdAmount;
                            destination.TvLicenceFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.SecuredLoans:
                        {
                            destination.SecuredLoans += expenditure.MonthlyHouseholdAmount;
                            destination.SecuredLoansFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.ApplianceRental:
                        {
                            destination.Rental += expenditure.MonthlyHouseholdAmount;
                            destination.RentalFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.Energy:

                        {
                            destination.OtherUtilities += expenditure.MonthlyHouseholdAmount;
                            destination.OtherUtilitiesFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.WaterSupply:
                        {
                            destination.Water += expenditure.MonthlyHouseholdAmount;
                            destination.WaterFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.ChildSupport:
                    case Constants.OpenWrksIncomeAndExpenditures.ChildcareCosts:
                        {
                            destination.ChildMaintenance += expenditure.MonthlyHouseholdAmount;
                            destination.ChildMaintenanceFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.AdultCareCosts:
                    case Constants.OpenWrksIncomeAndExpenditures.PrescriptsAndMedicines:
                    case Constants.OpenWrksIncomeAndExpenditures.OtherHealthCosts:
                        {
                            destination.Healthcare += expenditure.MonthlyHouseholdAmount;
                            destination.HealthcareFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.BreakdownCover:
                    case Constants.OpenWrksIncomeAndExpenditures.CarInsurance:
                    case Constants.OpenWrksIncomeAndExpenditures.FuelAndParkingCharges:
                    case Constants.OpenWrksIncomeAndExpenditures.RoadTax:
                    case Constants.OpenWrksIncomeAndExpenditures.VehicleMaintenance:
                    case Constants.OpenWrksIncomeAndExpenditures.HirePurchase:
                    case Constants.OpenWrksIncomeAndExpenditures.PublicTransport:
                    case Constants.OpenWrksIncomeAndExpenditures.OtherTransportAndTravelCosts:
                        {
                            destination.Travel += expenditure.MonthlyHouseholdAmount;
                            destination.TravelFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.SchoolCosts:
                        {
                            destination.SchoolCosts += expenditure.MonthlyHouseholdAmount;
                            destination.SchoolCostsFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.PensionPayments:
                    case Constants.OpenWrksIncomeAndExpenditures.LifeInsurance:
                    case Constants.OpenWrksIncomeAndExpenditures.HealthInsurance:
                    case Constants.OpenWrksIncomeAndExpenditures.BuildingInsurance:
                    case Constants.OpenWrksIncomeAndExpenditures.OtherPensionAndInsuranceCosts:
                        {
                            destination.PensionInsurance += expenditure.MonthlyHouseholdAmount;
                            destination.PensionInsuranceFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.ProfessionCosts:
                        {
                            destination.ProfessionalCosts += expenditure.MonthlyHouseholdAmount;
                            destination.ProfessionalCostsFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.Mobile:
                    case Constants.OpenWrksIncomeAndExpenditures.Gifts:
                    case Constants.OpenWrksIncomeAndExpenditures.HomePhoneAndInternet:
                    case Constants.OpenWrksIncomeAndExpenditures.HobbiesAndSport:
                    case Constants.OpenWrksIncomeAndExpenditures.GymsAndExecrises:
                    case Constants.OpenWrksIncomeAndExpenditures.PubsAndCafes:
                    case Constants.OpenWrksIncomeAndExpenditures.EatingOutAndTakeaways:
                        {
                            destination.Leisure += expenditure.MonthlyHouseholdAmount;
                            destination.LeisureFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.Groceries:
                    case Constants.OpenWrksIncomeAndExpenditures.HouseRepairs:
                    case Constants.OpenWrksIncomeAndExpenditures.PetInsurance:
                        {
                            destination.Housekeeping += expenditure.MonthlyHouseholdAmount;
                            destination.HousekeepingFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.PersonalCosts:
                       {
                            destination.PersonalCosts += expenditure.MonthlyHouseholdAmount;
                            destination.PersonalCostsFrequency = _monthly;
                            break;
                        }                   
                    case Constants.OpenWrksIncomeAndExpenditures.Savings:
                        {
                            destination.SavingsContributions += expenditure.MonthlyHouseholdAmount;
                            destination.SavingsContributionsFrequency = _monthly;
                            break;
                        }
                    case Constants.OpenWrksIncomeAndExpenditures.NonPriorityDebts:
                    case Constants.OpenWrksIncomeAndExpenditures.PriorityDebts:
                        {
                            destination.OtherDebts.Add(new SaveOtherDebts { Amount = expenditure.MonthlyHouseholdAmount, Frequency = _monthly });
                            break;
                        }
                }
            }

            destination.ExpenditureTotal = destination.Mortgage + destination.Rent + destination.SecuredLoans + destination.CouncilTax +
                                           destination.Rental + destination.TvLicence + destination.Gas + destination.OtherUtilities +
                                           destination.ChildMaintenance +
                                           destination.Housekeeping + destination.PersonalCosts + destination.Leisure + destination.Travel +
                                           destination.Healthcare + destination.PensionInsurance + destination.SchoolCosts +
                                           destination.ProfessionalCosts + destination.SavingsContributions;


            return destination;
        }

        private IncomeAndExpenditure MapArrearsAndFines(OpenWrksBudget source, IncomeAndExpenditure destination)
        {
            var monthlyArrearsPayments = 0.00M;

            foreach (var entry in source.ArrearsAndFines)
            {
                if (!entry.IsPaying) { continue; }
                if (!entry.MonthlyHouseholdAmount.HasValue) { continue; }

                switch (entry.Type.ToString().ToUpper())
                {
                    case Constants.OpenWorksArrears.Energy:
                        {
                            if (destination.OtherUtilities > 0.00M)
                            {
                                destination.OtherUtilitiesArrears += entry.MonthlyHouseholdAmount.Value;
                            }
                            else
                            {
                                AddArrearsToOtherDebts(entry, destination);
                            }

                            break;
                        }
                    case Constants.OpenWorksArrears.Water:
                        {
                            if (destination.Water > 0.00M)
                            {
                                destination.WaterArrears += entry.MonthlyHouseholdAmount.Value;
                            }
                            else
                            {
                                AddArrearsToOtherDebts(entry, destination);
                            }

                            break;
                        }
                    case Constants.OpenWorksArrears.ChildMaintanence:
                        if (destination.ChildMaintenance > 0.00M)
                        {
                            destination.ChildMaintenanceArrears += entry.MonthlyHouseholdAmount.Value;
                        }
                        else
                        {
                            AddArrearsToOtherDebts(entry, destination);
                        }
                        break;

                    case Constants.OpenWorksArrears.BenefitRepayment:
                        AddArrearsToOtherDebts(entry, destination);
                        break;

                    case Constants.OpenWorksArrears.TvLicence:
                        if (destination.TvLicence > 0.00M)
                        {
                            destination.TvLicenceArrears += entry.MonthlyHouseholdAmount.Value;
                        }
                        else
                        {
                            AddArrearsToOtherDebts(entry, destination);
                        }
                        break;
                    case Constants.OpenWorksArrears.CouncilTax:
                        if (destination.CouncilTax > 0.00M)
                        {
                            destination.CouncilTaxArrears += entry.MonthlyHouseholdAmount.Value;
                        }
                        else
                        {
                            AddArrearsToOtherDebts(entry, destination);
                        }
                        break;
                    case Constants.OpenWorksArrears.Tax:
                        AddArrearsToOtherDebts(entry, destination);
                        break;
                    case Constants.OpenWorksArrears.Rent:
                        if (destination.Rent > 0.00M)
                        {
                            destination.RentArrears += entry.MonthlyHouseholdAmount.Value;
                        }
                        else
                        {
                            AddArrearsToOtherDebts(entry, destination);
                        }
                        break;
                    case Constants.OpenWorksArrears.Mortgage:
                        if (destination.Mortgage > 0.00M)
                        {
                            destination.MortgageArrears += entry.MonthlyHouseholdAmount.Value;

                        }
                        else
                        {
                            AddArrearsToOtherDebts(entry, destination);
                        }
                        break;
                    case Constants.OpenWorksArrears.HirePurchase:
                        AddArrearsToOtherDebts(entry, destination);
                        break;
                    case Constants.OpenWorksArrears.CourtFines:
                        AddArrearsToOtherDebts(entry, destination);
                        break;
                    case Constants.OpenWorksArrears.MobilePhone:
                        AddArrearsToOtherDebts(entry, destination);
                        break;

                }

                monthlyArrearsPayments += entry.MonthlyHouseholdAmount.Value;
            }

            destination.ExpenditureTotal += monthlyArrearsPayments;
            destination.HasArrears = monthlyArrearsPayments > 0.00m;

            return destination;
        }

        private IncomeAndExpenditure MapCardsAndLoans(OpenWrksBudget source, IncomeAndExpenditure destination)
        {
            foreach (var entry in source.CardsAndLoans)
            {
                destination.OtherDebts.Add(new SaveOtherDebts { Amount = entry.MonthlyAmount, Frequency = _monthly });
            }

            return destination;
        }

        private void AddArrearsToOtherDebts(OpenWrksArrearsAndFine arrears, IncomeAndExpenditure destination)
        {
            destination.OtherDebts.Add(new SaveOtherDebts { Amount = arrears.MonthlyHouseholdAmount.Value, Frequency = _monthly});
        }
    }
}
