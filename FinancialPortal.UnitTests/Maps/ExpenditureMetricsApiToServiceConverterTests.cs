using AutoMapper;
using FinancialPortal.Web.Maps;
using FinancialPortal.Web.Services.ApiModels;
using FinancialPortal.Web.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialPortal.UnitTests.Maps
{
    [TestClass]
    public class ExpenditureMetricsApiToServiceConverterTests
    {
        private Mock<IMapper> _mapper;
        private ExpenditureMetricsApiToServiceConverter _converter;

        [TestInitialize]
        public void TestInitialse() 
        {
            this._mapper = new Mock<IMapper>(MockBehavior.Strict);
            this._converter = new ExpenditureMetricsApiToServiceConverter(this._mapper.Object);
        }

        [TestMethod]
        public void ConvertTest_SourceNull() 
        {
            ExpenditureMetricsApiModel source = null;
            ExpenditureMetrics destination = new ExpenditureMetrics();

            Assert.IsNull(_converter.Convert(source, destination, null));
        }

        [TestMethod]
        public void ConvertTest()
        {
            ExpenditureMetricApiModel sourceCommsAndLeisure = new ExpenditureMetricApiModel()
            {
                Name = "Comms and Leisure"
            };

            ExpenditureMetricApiModel sourceFoodAndHousekeeping = new ExpenditureMetricApiModel()
            {
                Name = "Food and Housekeeping"
            };

            ExpenditureMetricApiModel sourcePersonalCosts = new ExpenditureMetricApiModel() 
            {
                Name = "Personal Costs"
            };

            ExpenditureMetric commsAndLeisure = new ExpenditureMetric()
            {
                Name = "Comms and Leisure",
                AdditionalAdult = 1,
                Children14To18 = 2,
                AdjustmentPercentage = 55,
                Adult = 2,
                ChildrenUnder14 = 4,
                Vehicle = 2
            };

            ExpenditureMetric foodAndHousekeeping = new ExpenditureMetric() 
            {
                Name = "Food and Housekeeping",
                AdditionalAdult = 2,
                Children14To18 = 1,
                AdjustmentPercentage = 44,
                Adult = 3,
                ChildrenUnder14 = 3,
                Vehicle = 1
            };

            ExpenditureMetric personalCosts = new ExpenditureMetric() 
            {
                Name = "Personal Costs",
                AdditionalAdult = 3,
                Children14To18 = 4,
                AdjustmentPercentage = 33,
                Adult = 1,
                ChildrenUnder14 = 2,
                Vehicle = 0
            };

            _mapper.Setup(x => x.Map<ExpenditureMetric>(It.Is<ExpenditureMetricApiModel>(m => m.Name == "Comms and Leisure")))
                .Returns(commsAndLeisure);
            _mapper.Setup(x => x.Map<ExpenditureMetric>(It.Is<ExpenditureMetricApiModel>(m => m.Name == "Food and Housekeeping")))
                .Returns(foodAndHousekeeping);
            _mapper.Setup(x => x.Map<ExpenditureMetric>(It.Is<ExpenditureMetricApiModel>(m => m.Name == "Personal Costs")))
                .Returns(personalCosts);

            ExpenditureMetricsApiModel source = new ExpenditureMetricsApiModel()
            {
                GuideLines = new List<ExpenditureMetricApiModel>()
                {
                    sourceCommsAndLeisure,
                    sourceFoodAndHousekeeping,
                    sourcePersonalCosts,
                }
            };

            //Create a copy of source for later
            ExpenditureMetricsApiModel sourceCopy = Utilities.DeepCopy(source);

            ExpenditureMetrics destination = new ExpenditureMetrics();
            ExpenditureMetrics expected = new ExpenditureMetrics()
            {
                CommsAndLeisure = commsAndLeisure,
                FoodAndHousekeeping = foodAndHousekeeping,
                PersonalCosts = personalCosts
            };

            ExpenditureMetrics result = _converter.Convert(source, destination, null);

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));

            //Check that result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

        [TestMethod]
        public void ConvertTest_DestinationNull()
        {
            ExpenditureMetricApiModel sourceCommsAndLeisure = new ExpenditureMetricApiModel()
            {
                Name = "Comms and Leisure"
            };

            ExpenditureMetricApiModel sourceFoodAndHousekeeping = new ExpenditureMetricApiModel()
            {
                Name = "Food and Housekeeping"
            };

            ExpenditureMetricApiModel sourcePersonalCosts = new ExpenditureMetricApiModel()
            {
                Name = "Personal Costs"
            };

            ExpenditureMetric commsAndLeisure = new ExpenditureMetric()
            {
                Name = "Comms and Leisure",
                AdditionalAdult = 1,
                Children14To18 = 2,
                AdjustmentPercentage = 55,
                Adult = 2,
                ChildrenUnder14 = 4,
                Vehicle = 2
            };

            ExpenditureMetric foodAndHousekeeping = new ExpenditureMetric()
            {
                Name = "Food and Housekeeping",
                AdditionalAdult = 2,
                Children14To18 = 1,
                AdjustmentPercentage = 44,
                Adult = 3,
                ChildrenUnder14 = 3,
                Vehicle = 1
            };

            ExpenditureMetric personalCosts = new ExpenditureMetric()
            {
                Name = "Personal Costs",
                AdditionalAdult = 3,
                Children14To18 = 4,
                AdjustmentPercentage = 33,
                Adult = 1,
                ChildrenUnder14 = 2,
                Vehicle = 0
            };

            _mapper.Setup(x => x.Map<ExpenditureMetric>(It.Is<ExpenditureMetricApiModel>(m => m.Name == "Comms and Leisure")))
                .Returns(commsAndLeisure);
            _mapper.Setup(x => x.Map<ExpenditureMetric>(It.Is<ExpenditureMetricApiModel>(m => m.Name == "Food and Housekeeping")))
                .Returns(foodAndHousekeeping);
            _mapper.Setup(x => x.Map<ExpenditureMetric>(It.Is<ExpenditureMetricApiModel>(m => m.Name == "Personal Costs")))
                .Returns(personalCosts);
           
            ExpenditureMetricsApiModel source = new ExpenditureMetricsApiModel()
            {
                GuideLines = new List<ExpenditureMetricApiModel>()
                {
                    sourceCommsAndLeisure,
                    sourceFoodAndHousekeeping,
                    sourcePersonalCosts,
                }
            };

            //Create a copy of source for later
            ExpenditureMetricsApiModel sourceCopy = Utilities.DeepCopy(source);

            ExpenditureMetrics destination = null;
            ExpenditureMetrics expected = new ExpenditureMetrics()
            {
                CommsAndLeisure = commsAndLeisure,
                FoodAndHousekeeping = foodAndHousekeeping,
                PersonalCosts = personalCosts
            };

            ExpenditureMetrics result = _converter.Convert(source, destination, null);

            //Check that source hasn't been modified
            Assert.IsTrue(Utilities.DeepCompare(source, sourceCopy));

            //Check that result is as expected
            Assert.IsTrue(Utilities.DeepCompare(expected, result));
        }

    }
}
