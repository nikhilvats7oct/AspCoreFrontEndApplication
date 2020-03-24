using System.Globalization;
using AutoMapper;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.ViewModels;

namespace FinancialPortal.Web.Maps
{
    public class TransactionToTransactionVmConverter : ITypeConverter<Transaction, TransactionVm>
    {
        public TransactionVm Convert(Transaction source, TransactionVm destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                destination = new TransactionVm();
            }

            destination.DateText = source.Date.ToString("dd MMM y", CultureInfo.CurrentCulture);
            destination.AmountText = source.Amount.ToString("C", CultureInfo.CurrentCulture);
            destination.Description = source.Description;
            destination.RollingBalanceText = source.RollingBalance.ToString("C", CultureInfo.CurrentCulture);

            return destination;
        }
    }
}
