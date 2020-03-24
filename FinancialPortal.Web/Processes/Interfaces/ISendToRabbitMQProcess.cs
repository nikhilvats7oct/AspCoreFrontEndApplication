using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Processes.Interfaces
{
    public interface ISendToRabbitMQProcess
    {
        Task SendToRabbitMQ(WebActionDto web_action);

        Task SendToRabbitMQAnonymous(WebActionDto webAction);
    }
}
