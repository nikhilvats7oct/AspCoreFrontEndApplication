using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Processes.Interfaces;
using FinancialPortal.Web.Proxy.Interfaces;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.Web.Processes
{
    public class SendToRabbitMQProcess : ISendToRabbitMQProcess
    {
        private readonly IApiGatewayProxy _proxy;

        private readonly IRestClient _restClient;

        private readonly PortalSetting _portalSetting;

        public SendToRabbitMQProcess(IApiGatewayProxy proxy,
            PortalSetting portalSetting,
            IRestClient restClient)
        {
            _proxy = proxy;
            _restClient = restClient;
            _portalSetting = portalSetting;
        }
        public async Task SendToRabbitMQ(WebActionDto web_action)
        {
            await _proxy.SendToRabbitMQ(web_action);
        }

        public async Task SendToRabbitMQAnonymous(WebActionDto webAction)
        {
            var uri = "api/WebActivity/SendToRabbitMQ";
            await _restClient.PostAsync<object, Task>($"{_portalSetting.GatewayEndpoint}{uri}", new
            {
                webAction.LowellReference,
                webAction.Company,
                webAction.DateTime,
                WebActionID = (int)webAction.WebActionType,
                webAction.Guid
            });
        }
    }
}
