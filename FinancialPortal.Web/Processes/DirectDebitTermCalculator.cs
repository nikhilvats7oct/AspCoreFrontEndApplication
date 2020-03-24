using System;
using FinancialPortal.Web.Processes.Interfaces;

namespace FinancialPortal.Web.Processes
{
    public class DirectDebitTermCalculator : IDirectDebitTermCalculator
    {
        public int CalculateTermInMonths(decimal balance, decimal paymentAmount, string paymentFrequency)
        {
            decimal monthlyPaymentAmount = paymentAmount;
            switch (paymentFrequency.ToLower())
            {
                case "weekly":
                    monthlyPaymentAmount = monthlyPaymentAmount * 4.33M;
                    break;
                case "fortnightly":
                    monthlyPaymentAmount = monthlyPaymentAmount * 2.17M;
                    break;
                case "4week":
                case "4weekly":
                case "every 4 weeks":
                    monthlyPaymentAmount = monthlyPaymentAmount * 1.08M;
                    break;
            }

            return decimal.ToInt32(Math.Ceiling(balance / monthlyPaymentAmount));
        }

    }
}
