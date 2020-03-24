using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Interfaces;
using FinancialPortal.Web.Services.Interfaces.Utility;
using FinancialPortal.Web.Services.Models;
using FinancialPortal.Web.Settings;

namespace FinancialPortal.Web.Services
{
    public class TriggerFigureService : ITriggerFigureService
    {
        private readonly IRestClient _restClient;
        private readonly PortalSetting _portalSetting;
        private readonly IMapper _mapper;

        public TriggerFigureService(IRestClient restClient,
            PortalSetting portalSetting,
            IMapper mapper)
        {
            _restClient = restClient;
            _portalSetting = portalSetting;
            _mapper = mapper;
        }

        public async Task<ExpenditureMetrics> GetExpenditureMetrics(IApplicationSessionState sessionState)
        {
            ExpenditureMetrics expenditureMetrics = sessionState.GetExpenditureMetrics();

            if (expenditureMetrics == null)
            {
                var innerUrl = $"{_portalSetting.GatewayEndpoint}api/BudgetCalculator/GetTriggerFigures";

                ExpenditureMetricsApiModel apiExpenditureMetrics = await _restClient.GetAsync<ExpenditureMetricsApiModel>(innerUrl);
                expenditureMetrics = _mapper.Map<ExpenditureMetrics>(apiExpenditureMetrics);

                sessionState.SaveExpenditureMetrics(expenditureMetrics);
            }

            return expenditureMetrics;
        }

        public decimal CalculateTriggerFigure(ExpenditureMetric expenditureMetric, int adultsInHousehold, int childrenUnder16, int childrenOver16)
        {
            var result = expenditureMetric.Adult +
                        (expenditureMetric.AdditionalAdult * (adultsInHousehold - 1) +
                        (expenditureMetric.ChildrenUnder14 * childrenUnder16) +
                        (expenditureMetric.Children14To18 * childrenOver16));

            return result;
        }

        public decimal CalculateMaxTriggerValue(decimal value)
        {
            return value * 1.2M;
        }

        public decimal CalculateMinTriggerValue(decimal value)
        {
            return value * 0.8M;
        }

    }
}
