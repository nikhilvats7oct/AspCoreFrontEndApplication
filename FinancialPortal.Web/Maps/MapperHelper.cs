using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.Web.Maps
{
    public class MapperHelper : IMapperHelper
    {
        private readonly PortalSetting _portalSettings;
        private readonly ICalculatorService _calculatorService;

        public MapperHelper(PortalSetting portalSettings, ICalculatorService calculatorService)
        {
            _portalSettings = portalSettings;
            _calculatorService = calculatorService;
        }

        public string AppendNonNullWithSpace(string stringBefore, string stringToAdd, string inBetweenText = "")
        {
            if (string.IsNullOrEmpty(stringToAdd)) return stringBefore;

            // Add space if already populated
            if (stringBefore != "") stringBefore += " ";

            return stringBefore + inBetweenText + stringToAdd;
        }

        public string DeriveDiscountDescription(Account account)
        {
            if (account.DiscountOfferAmount != null && account.DiscountOfferExpiry != null)
            {
                string saveAmount = account.DiscountOfferAmount.Value.ToString("C", CultureInfo.CurrentCulture);
                string untilDate = account.DiscountOfferExpiry.Value.ToString("d", CultureInfo.CurrentCulture);
                string willPayAmount = (account.OutstandingBalance - account.DiscountOfferAmount.Value).ToString("C", CultureInfo.CurrentCulture);

                return $"Save {saveAmount} until {untilDate}. You will pay {willPayAmount}. To accept, set up a Direct Debit Plan or Pay In Full.";
            }

            return null;
        }

        public string DerivePlanDescription(Account account)
        {
            string planDescription = "";

            if (account.PaymentPlanAmount.HasValue && account.PaymentPlanAmount.Value > 0)
            {
                planDescription += account.PaymentPlanAmount.Value.ToString("C", CultureInfo.CurrentCulture);
            }

            planDescription = AppendNonNullWithSpace(planDescription, account.PaymentPlanFrequency);
            planDescription = AppendNonNullWithSpace(planDescription, account.PaymentPlanMethod, "by ");

            if (planDescription == "")
            {
                planDescription = null;
            }
            else
            {
                planDescription = ": " + planDescription;
            }

            return planDescription;
        }

        public string DeriveArrearsDetail(decimal? paymentPlanArrearsAmount, bool paymentPlanIsAutomated)
        {
            if (paymentPlanArrearsAmount > 0)
            {
                string amountString = paymentPlanArrearsAmount.Value.ToString("C", CultureInfo.CurrentCulture);
                var talkToUslink = _portalSettings.TalkToUsUrl;

                if (paymentPlanIsAutomated)
                {
                    return $"Unfortunately, your last payment didn’t go through as planned and your account has fallen in to arrears.<br><br>If you are able to you can get your plan up to date by making a one-off payment of {amountString}.<br><br>After that, your next payment will be taken as normal. Don’t worry if you need to change your plan, you can do it after you’ve made this payment. <a target=\"_blank\" href=\"{talkToUslink}\">talk to us</a> or call us on 0333 556 5550 and we’ll be happy to help.";
                }

                return $"Unfortunately, your last payment didn’t go through as planned and your account has fallen in to arrears.<br><br>If you are able to you can get your plan up to date by making a one-off payment of {amountString}.<br><br>After that, your next payment will be taken as normal. Don’t worry if you need to change your plan, you can do it after you’ve made this payment. <a target=\"_blank\" href=\"{talkToUslink}\">talk to us</a> or call us on 0333 556 5550 and we’ll be happy to help.";
            }

            return null;
        }

        public bool AccountHasDiscountOffer(AccountSummary account)
        {
            return account.DiscountOfferAmount != null && account.DiscountOfferExpiry != null;
        }

        public string DeriveDiscountDescription(AccountSummary account)
        {
            if (AccountHasDiscountOffer(account))
            {
                string saveAmount = account.DiscountOfferAmount.Value.ToString("C", CultureInfo.CurrentCulture);
                string untilDate = account.DiscountOfferExpiry.Value.ToString("d", CultureInfo.CurrentCulture);

                return $"Save {saveAmount} until {untilDate}";
            }

            return null;
        }

        public string DerivePlanDescription(AccountSummary account)
        {
            string planDescription = "";

            if (account.PaymentPlanAmount.HasValue && account.PaymentPlanAmount.Value > 0)
            {
                planDescription += account.PaymentPlanAmount.Value.ToString("C", CultureInfo.CurrentCulture);
            }

            planDescription = AppendNonNullWithSpace(planDescription, account.PaymentPlanFrequency);

            if (planDescription == "")
            {
                planDescription = null;
            }
            else
            {
                planDescription = ": " + planDescription;
            }

            return planDescription;
        }

        public string DeriveArrearsSummary(decimal? paymentPlanArrearsAmount)
        {
            if (paymentPlanArrearsAmount > 0)
            {
                string amountString = paymentPlanArrearsAmount.Value.ToString("C", CultureInfo.CurrentCulture);
                var talkToUslink = _portalSettings.TalkToUsUrl;
                return $"In arrears<br>If you are able to, please make your payment today or <a target=\"_blank\" href=\"{talkToUslink}\">talk to us</a> for help.";
            }

            return null;
        }

        public string DerivePlanTransferredFromMessage(List<string> accountNumbers)
        {
            string message = "You've paid off account";

            if (accountNumbers == null || accountNumbers.Count == 0) { return ""; }
            if (accountNumbers.Count == 1) { return $"{message} {accountNumbers[0]}. The payment has been auto transferred to this account."; }

            message = $"{message} {string.Join(", ", accountNumbers.Take(accountNumbers.Count - 1))}";
            message = $"{message} and {accountNumbers.Last()}";
            message = $"{message}. The payment has been auto transferred to this account.";

            return message;
        }

        public string DerivePlanTransferredFromAccountsFormatted(List<string> accountNumbers)
        {
            if (accountNumbers == null || accountNumbers.Count == 0) { return ""; }
            if (accountNumbers.Count == 1) { return $"account ref: {accountNumbers[0]}"; }

            string accountsString = string.Join(", ", accountNumbers.Take(accountNumbers.Count - 1));
            accountsString = $"{accountsString} and {accountNumbers.Last()}";
            accountsString = $"accounts {accountsString}";

            return accountsString;
        }

        public string ConvertEmploymentStatusFromCaseflow(string employmentStatus)
        {
            if (String.IsNullOrEmpty(employmentStatus)) { return string.Empty; }

            switch (employmentStatus.ToLower())
            {
                case "full-time": return "employed-full-time";
                case "part-time": return "employed-part-time";
                case "self-employed": return "self-employed";
                case "unemployed": return "unemployed";
                case "student": return "student";
                case "carer": return "carer";
                case "retired": return "retired";
                case "illness/disability related": return "illness-disability-related";
                case "other": return "other";
                default: return string.Empty;
            }
        }

        public string ConvertToEmploymentStatusCaseflow(string status)
        {
            if (string.IsNullOrEmpty(status)) { return string.Empty; }

            switch (status.ToLower())
            {
                case "employed-full-time": return "Full-Time";
                case "employed-part-time": return "Part-Time";
                case "self-employed": return "Self-Employed";
                case "unemployed": return "Unemployed";
                case "student": return "Student";
                case "carer": return "Carer";
                case "retired": return "Retired";
                case "illness-disability-related": return "Illness/Disability Related";
                case "other": return "Other";
                default: return string.Empty;
            }
        }

        public string ConvertHousingStatusFromCaseflow(string housingStatus)
        {
            if (string.IsNullOrEmpty(housingStatus)) { return string.Empty; }

            switch (housingStatus.ToLower())
            {
                case "owner": return "homeowner";
                case "mortgage": return "mortgage";
                case "homeless": return "homeless";
                case "tenant - private": return "tenant-private";
                case "tenant - social": return "tenant-social";
                case "living with parents": return "living-with-parents";
                case "illness/disability related": return "Illness/Disability related";
                case "other": return "other";
                default: return string.Empty;
            }
        }

        public string ConvertToHousingStatusCaseflow(string status)
        {
            if (string.IsNullOrEmpty(status)) { return string.Empty; }

            switch (status.ToLower())
            {
                case "homeowner": return "Owner";
                case "mortgage": return "Mortgage";
                case "homeless": return "HomeLess";
                case "tenant-private": return "Tenant - Private";
                case "tenant-social": return "Tenant - Social";
                case "living-with-parents": return "Living With Parents";
                case "illness/disability related": return "Illness/Disability Related";
                case "other": return "Other";
                default: return string.Empty;
            }
        }

        public string ConvertFrequencyFromCaseflow(string frequency) 
        {
            if (string.IsNullOrEmpty(frequency)) { return string.Empty; }

            switch (frequency.ToLower())
            {
                case "m": return "monthly";
                case "w": return "weekly";
                case "f": return "fortnightly";
                case "4": return "4week";
                case "q": return "Quarterly";
                case "l": return "Last Day of the Month";
                case "a": return "Annually";
                default: return string.Empty;
            }
        }

        public string ConvertFrequencyToInitial(string initial)
        {
            if (string.IsNullOrEmpty(initial)) { return string.Empty; }

            switch (initial.ToLower())
            {
                case "monthly": return "M";
                case "weekly": return "W";
                case "fortnightly": return "F";
                case "4week": return "4";
                case "quarterly": return "Q";
                case "last day of the month": return "L";
                case "annually": return "A";
                default: return string.Empty;
            }
        }

        public RegularPayment MapRegularPayment(decimal amount, string frequency)
        {
            if (frequency == "Q" || frequency == "L" || frequency == "A" || string.Equals(frequency,"monthly",StringComparison.OrdinalIgnoreCase))
            {
                return new RegularPayment 
                {
                    Amount = _calculatorService.ConvertToMonthly(amount, frequency), 
                    Frequency = "monthly" 
                };
            }
            else 
            {
                return new RegularPayment
                {
                    Amount = amount,
                    Frequency = ConvertFrequencyFromCaseflow(frequency)
                };
            }
        }

        public Outgoing MapOutgoing(decimal amount, string frequency, decimal arrearsAmount)
        {
            if (frequency == "Q" || frequency == "L" || frequency == "A" || string.Equals(frequency, "monthly", StringComparison.OrdinalIgnoreCase))
            {
                return new Outgoing()
                {
                    Amount = _calculatorService.ConvertToMonthly(amount, frequency),
                    ArrearsAmount = arrearsAmount,
                    Frequency = "monthly",
                    InArrears = arrearsAmount > 0.00M
                };
            }
            else
            {
                return new Outgoing() 
                {
                    Amount = amount,
                    ArrearsAmount = arrearsAmount,
                    Frequency = ConvertFrequencyFromCaseflow(frequency),
                    InArrears = arrearsAmount > 0.00M
                };
            }
        }

        public List<SaveOtherDebtsApiModel> CreateOtherDebts(IncomeAndExpenditure iAndE)
        {
            var otherDebts = new List<SaveOtherDebtsApiModel>();

            if (iAndE.OtherDebts!=null && iAndE.OtherDebts.Any())
            {
                iAndE.OtherDebts.ForEach(i =>
                {
                    otherDebts.Add(new SaveOtherDebtsApiModel()
                    {
                        Amount = i.Amount,
                        Arrears = i.Arrears,
                        Frequency = i.Frequency,
                        CountyCourtJudgement = false
                    });
                });               
            }


            if (iAndE.CourtFines > 0.00M)
            {
                otherDebts.Add(new SaveOtherDebtsApiModel()
                {
                    Amount = iAndE.CourtFines,
                    Arrears = iAndE.CourtFinesArrears,
                    Frequency = ConvertFrequencyToInitial(iAndE.CourtFinesFrequency),
                    CountyCourtJudgement = false
                });
            }

            if (iAndE.CCJs > 0.00M)
            {
                otherDebts.Add(new SaveOtherDebtsApiModel()
                {
                    Amount = iAndE.CCJs,
                    Arrears = iAndE.CCJsArrears,
                    Frequency = ConvertFrequencyToInitial(iAndE.CCJsFrequency),
                    CountyCourtJudgement = true
                });
            }
            
            if (iAndE.OtherExpenditure > 0.00M)
            {
                otherDebts.Add(new SaveOtherDebtsApiModel()
                {
                    Amount = iAndE.OtherExpenditure,
                    Arrears = 0.00M,
                    Frequency = ConvertFrequencyToInitial(iAndE.OtherExpenditureFrequency),
                    CountyCourtJudgement = false
                });
            }

            return otherDebts;
        }

    }
}
