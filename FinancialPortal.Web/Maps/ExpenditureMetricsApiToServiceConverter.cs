using AutoMapper;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialPortal.Web.Maps
{
    public class ExpenditureMetricsApiToServiceConverter : ITypeConverter<ExpenditureMetricsApiModel, ExpenditureMetrics>
    {
        private IMapper _mapper;
        public ExpenditureMetricsApiToServiceConverter(IMapper mapper) 
        {
            _mapper = mapper;
        }

        public ExpenditureMetrics Convert(ExpenditureMetricsApiModel source, ExpenditureMetrics destination, ResolutionContext context)
        {
            if (source == null) { return null; }
            if (destination == null) { destination = new ExpenditureMetrics(); }

            foreach (ExpenditureMetricApiModel metric in source.GuideLines)
            {
                switch (metric.Name)
                {
                    case "Comms and Leisure":
                        destination.CommsAndLeisure = _mapper.Map<ExpenditureMetric>(metric);
                        break;
                    case "Food and Housekeeping":
                        destination.FoodAndHousekeeping = _mapper.Map<ExpenditureMetric>(metric);
                        break;
                    case "Personal Costs":
                        destination.PersonalCosts = _mapper.Map<ExpenditureMetric>(metric);
                        break;
                }
            }

            return destination;
        }
    }
}
