using System.Threading.Tasks;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Models;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface ITriggerFigureService
    {
        Task<ExpenditureMetrics> GetExpenditureMetrics(IApplicationSessionState sessionState);
        decimal CalculateTriggerFigure(ExpenditureMetric expenditureMetric, int adultsInHousehold, int childrenUnder16, int childrenOver16);
        decimal CalculateMaxTriggerValue(decimal value);
        decimal CalculateMinTriggerValue(decimal value);

    }
}
