using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FinancialPortal.Web.Models.DataTransferObjects;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinancialPortal.Web.Controllers
{
    [AllowAnonymous]
    public class HeartBeatController : BaseController
    {
        private readonly IApiGatewayHeartbeatService _apiGatewayHeartbeatService;
        private readonly HeartbeatSetting _heartbeatSetting;

        public HeartBeatController(ILogger<BaseController> logger,
            IApiGatewayHeartbeatService apiGatewayHeartbeatService,
            HeartbeatSetting heartbeatSetting,
            IDistributedCache distributedCache,
            IApplicationSessionState sessionState,
            IConfiguration configuration)
            : base(logger, distributedCache, sessionState, configuration)
        {
            _apiGatewayHeartbeatService = apiGatewayHeartbeatService;
            _heartbeatSetting = heartbeatSetting;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var greenThreshold = _heartbeatSetting.GreenThreshold;
            var redThreshold = _heartbeatSetting.RedThreshold;

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            stopWatch.Stop();

            var result = new HeartBeatDto
            {
                ServiceName = "Financial Portal",
                TotalElapsedTime = stopWatch.Elapsed,
                RunningElapsedTime = stopWatch.Elapsed
            };

            result.SetStatus(greenThreshold, redThreshold);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> CheckAllServices()
        {
            var result = await GetHeartBeat();
            return View(result);
        }

        [HttpGet]
        public async Task<PartialViewResult> GetServiceHeartBeats()
        {
            var result = await GetHeartBeat();
            return PartialView("Partials/CheckServices", result);
        }

        private async Task<HeartBeatDto> GetHeartBeat()
        {
            var stopWatch = new Stopwatch();
            var greenThreshold = _heartbeatSetting.GreenThreshold;
            var redThreshold = _heartbeatSetting.RedThreshold;

            stopWatch.Start();
            var apiResult = await _apiGatewayHeartbeatService.CallHeartbeatAsync();
            stopWatch.Stop();

            var result = new HeartBeatDto
            {
                ServiceName = "Financial Portal",
                TotalElapsedTime = stopWatch.Elapsed,
                RunningElapsedTime = stopWatch.Elapsed - apiResult.TotalElapsedTime
            };

            result.SetStatus(greenThreshold, redThreshold);
            result.ServiceStatusHeartBeatResults.Add(apiResult);
            SetOverallStatus(result);
            return result;
        }

        private void SetOverallStatus(HeartBeatDto heartbeatResults)
        {
            var result = string.Empty;
            var gatewayApi = (HeartBeatDto)heartbeatResults.ServiceStatusHeartBeatResults.Single();
            var statues = new List<string>
            {
                heartbeatResults.Status,
                gatewayApi.Status
            };

            foreach (var service in gatewayApi.ServiceStatusHeartBeatResults)
            {
                statues.Add(service.Status);
                if (service.ChildHeartBeat != null)
                {
                    statues.Add(service.ChildHeartBeat.Status);
                }
            }

            if (statues.Any(x => x.Contains("RED")))
            {
                result = "RED";
            }
            else if (statues.Any(x => x.Contains("AMBER")))
            {
                result = "AMBER";
            }
            else if (statues.Any(x => x.Contains("GREEN")))
            {
                result = "GREEN";
            }

            heartbeatResults.OverallStatus = result;
        }


    }
}