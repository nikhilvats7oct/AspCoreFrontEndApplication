using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;

namespace FinancialPortal.Web.Services.Interfaces
{
    public interface IApiGatewayHeartbeatService
    {
        Task<HeartBeatDto> CallHeartbeatAsync();
    }
}