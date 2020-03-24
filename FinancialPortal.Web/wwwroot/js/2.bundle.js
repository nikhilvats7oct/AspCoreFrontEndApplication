(window["webpackJsonp"] = window["webpackJsonp"] || []).push([[2],{

/***/ 487:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _jquery = __webpack_require__(11);

var _jquery2 = _interopRequireDefault(_jquery);

var _chart = __webpack_require__(489);

var _chart2 = _interopRequireDefault(_chart);

var _tinycolor = __webpack_require__(490);

var _tinycolor2 = _interopRequireDefault(_tinycolor);

var _helpers = __webpack_require__(97);

var _configAfforable = __webpack_require__(491);

var _configAfforable2 = _interopRequireDefault(_configAfforable);

var _configUnaffordable = __webpack_require__(492);

var _configUnaffordable2 = _interopRequireDefault(_configUnaffordable);

var _configNegative = __webpack_require__(493);

var _configNegative2 = _interopRequireDefault(_configNegative);

function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : { default: obj };
}

exports.default = function () {
    var dataConfig = void 0,
        pieOutcome = false,
        income = {
        salary: 0,
        benefits: 0,
        other: 0
    },
        activeIncome = {},
        outgoings = {
        household: 0,
        expenditure: 0,
        other: 0,
        recommended: 0,
        leftover: 0
    },
        activeOutgoings = {},
        centerIndex = void 0;

    // Setup 
    (0, _jquery2.default)('.js-chart-settings-income input').each(function (index, element) {
        element.value = element.value || income[element.name];
        income[element.name] = element.value;
    });
    (0, _jquery2.default)('.js-chart-settings-outgoings input').each(function (index, element) {
        element.value = element.value || outgoings[element.name];
        outgoings[element.name] = element.value;
    });

    switch ((0, _jquery2.default)('.js-chart-settings input[name="outcome"]').val()) {
        case 'affordable':
        case 'postive':
            dataConfig = _configAfforable2.default;
            pieOutcome = true;
            break;
        case 'unaffordable':
        case 'not-reccomended':
            pieOutcome = true;
            dataConfig = _configUnaffordable2.default;
            break;
        case 'negative':
        default:
            dataConfig = _configNegative2.default;
            outgoings.leftover = Math.abs(outgoings.leftover);
    }

    centerIndex = dataConfig.labels.indexOf('Left over');

    updateIncome();
    updateOutgoings();

    var ctx = document.getElementById('myChart');

    if (ctx) {
        ctx.addEventListener('mouseout', function (e) {
            triggerPieInfo(dataConfig.datasets[0].backgroundColor[centerIndex], dataConfig.labels[centerIndex], dataConfig.datasets[0].data[centerIndex]);
        });

        window.myDoughnut = new _chart2.default(ctx, {
            type: 'doughnut',
            data: dataConfig,
            options: {
                responsive: true,
                legendCallback: function legendCallback(chart) {

                    var legendHtml = [];
                    legendHtml.push('<ul class="list-inline" style="display: inline-flex; list-style:none; position: absolute; top: 0; width: 100%; justify-content: space-around;" >');
                    var item = chart.data.datasets[0];
                    for (var i = 0; i < item.data.length; i++) {
                        legendHtml.push('<li class="list-inline-item" style="margin-right: 10px; text-align: center;" >');
                        legendHtml.push('<div class="chart-legend" style="background-color:' + item.backgroundColor[i] + '; height:20px; width:20px; margin: 0 auto;"></div>');
                        legendHtml.push('<div class="chart-legend-label-text">' + chart.data.labels[i] + '<br/> <strong>£' + item.data[i] + '</strong></div>');
                        legendHtml.push('</li>');
                    }

                    legendHtml.push('</ul>');
                    return legendHtml.join("");
                },
                legend: {
                    display: false
                },
                tooltips: {
                    enabled: false
                },
                animation: {
                    animateScale: true,
                    animateRotate: true,
                    onComplete: function onComplete() {
                        _showCenter();
                    }
                },
                cutoutPercentage: 50,
                onHover: function onHover(e, t) {
                    if (t.length) {
                        var index = t[0]._index,
                            datasetsIndex = t[0]._datasetIndex;

                        if (dataConfig.labels[index] != '') {
                            triggerPieInfo(dataConfig.datasets[datasetsIndex].backgroundColor[index], dataConfig.labels[index], dataConfig.datasets[datasetsIndex].data[index]);
                        }
                    }
                },
                onClick: function onClick(e, t) {
                    if (t.length) {
                        var index = t[0]._index,
                            datasetsIndex = t[0]._datasetIndex;

                        if (dataConfig.labels[index] != '') {
                            triggerPieInfo(dataConfig.datasets[datasetsIndex].backgroundColor[index], dataConfig.labels[index], dataConfig.datasets[datasetsIndex].data[index]);
                        }
                    }
                }
            }
        });

        (0, _jquery2.default)('.chart-legend').html(window.myDoughnut.generateLegend());
    }

    (0, _jquery2.default)('.js-budget-frequency').on('change', function (e) {
        var $this = (0, _jquery2.default)(e.currentTarget),
            $thisVal = $this.val();

        (0, _jquery2.default)('.js-spare-money').text('£' + _calcFromMonthly(parseFloat((0, _jquery2.default)('.js-chart-settings input[name="monthly-spare"]').val()), $thisVal).toFixed(2));
        (0, _jquery2.default)('.js-income-total').text('£' + _calcFromMonthly(parseFloat((0, _jquery2.default)('.js-chart-settings input[name="income-total"]').val()), $thisVal).toFixed(2));
        (0, _jquery2.default)('.js-outgoings-total').text('£' + _calcFromMonthly(parseFloat((0, _jquery2.default)('.js-chart-settings input[name="outgoing-total"]').val()), $thisVal).toFixed(2));

        (0, _jquery2.default)('.js-money-frequency').text(_getFormattedFrequency($thisVal));

        updateIncome($thisVal);
        updateOutgoings($thisVal);
        updatePieData();
        triggerPieInfo(dataConfig.datasets[0].backgroundColor[centerIndex], dataConfig.labels[centerIndex], dataConfig.datasets[0].data[centerIndex]);
        (0, _jquery2.default)('.chart-legend').html(window.myDoughnut.generateLegend());
    });

    // Public Functions
    function triggerPieInfo(backgroundColor, label, price) {
        var checkifColorIsDark = (0, _tinycolor2.default)(backgroundColor);

        if (checkifColorIsDark.isDark()) {
            (0, _jquery2.default)('.js-chart-info').css({
                backgroundColor: backgroundColor,
                color: 'white'
            });
        } else {
            (0, _jquery2.default)('.js-chart-info').css({
                backgroundColor: backgroundColor,
                color: 'black'
            });
        }

        (0, _jquery2.default)('.js-chart-heading').text(label + ':');

        if (!pieOutcome && label == 'Left over') {
            (0, _jquery2.default)('.js-chart-price').text('-£' + parseFloat(price).toFixed(2));
        } else {
            (0, _jquery2.default)('.js-chart-price').text('£' + parseFloat(price).toFixed(2));
        }
    }

    function updateIncome() {
        var frequency = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : 'monthly';

        for (var elem in income) {
            activeIncome[elem] = _calcFromMonthly(parseFloat(income[elem]), frequency).toFixed(2);
        }

        (0, _jquery2.default)('.js-income-salary').text('£' + activeIncome.salary);
        (0, _jquery2.default)('.js-income-benefits').text('£' + activeIncome.benefits);
        (0, _jquery2.default)('.js-income-other').text('£' + activeIncome.other);
    }

    function updateOutgoings() {
        var frequency = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : 'monthly';

        for (var elem in outgoings) {
            activeOutgoings[elem] = _calcFromMonthly(parseFloat(outgoings[elem]), frequency).toFixed(2);
        }

        if (!pieOutcome) {
            activeOutgoings.monthlySpare = _calcFromMonthly(parseFloat((0, _jquery2.default)('.js-chart-settings input[name="monthly-spare"]').val()), frequency).toFixed(2);
            activeOutgoings.outgoingTotal = _calcFromMonthly(parseFloat((0, _jquery2.default)('.js-chart-settings input[name="outgoing-total"]').val()), frequency).toFixed(2);
            (0, _jquery2.default)('.js-outgoings-leftover').text('-£' + activeOutgoings.leftover);
        } else {
            (0, _jquery2.default)('.js-outgoings-leftover').text('£' + activeOutgoings.leftover);
        }

        (0, _jquery2.default)('.js-outgoings-household').text('£' + activeOutgoings.household);
        (0, _jquery2.default)('.js-outgoings-expenditure').text('£' + activeOutgoings.expenditure);
        (0, _jquery2.default)('.js-outgoings-other').text('£' + activeOutgoings.other);
        (0, _jquery2.default)('.js-outgoings-recommended').text('£' + activeOutgoings.recommended);
    }

    function updatePieData() {
        window.myDoughnut.options.animation.duration = 375;

        if (pieOutcome) {
            window.myDoughnut.data.datasets.forEach(function (element) {
                element.data = [];
                element.data.push(activeOutgoings.household, activeOutgoings.expenditure);

                if (parseFloat(activeOutgoings.recommended) > 0) {
                    element.data.push(activeOutgoings.recommended);
                }

                element.data.push(activeOutgoings.leftover);
            });
        } else {
            window.myDoughnut.data.datasets.forEach(function (element) {
                element.data = [];

                if (element.label == 'Overflow') {
                    element.data.push(activeOutgoings.monthlySpare, activeOutgoings.outgoingTotal);
                } else {
                    element.data.push(0, 0, activeOutgoings.household, activeOutgoings.expenditure);
                }
            });
        }

        (0, _jquery2.default)('.js-chart').removeClass('chart--is-animating');
        window.myDoughnut.update();
    }
};

// Private Functions


function _showCenter() {
    (0, _jquery2.default)('.js-chart').addClass('chart--is-animating');
}

function _calcFromMonthly(number, frequency) {
    var _getDurationConstants = (0, _helpers.getDurationConstants)(),
        week = _getDurationConstants.week,
        fortnight = _getDurationConstants.fortnight,
        every4week = _getDurationConstants.every4week;

    switch (frequency) {
        case 'monthly':
            return number;
        case 'weekly':
            return number / week;
        case 'fortnightly':
            return number / fortnight;
        case '4week':
        case 'every 4 weeks':
            return number / every4week;
    }
}

function _getFormattedFrequency(frequency) {
    switch (frequency) {
        case 'monthly':
            return 'month';
        case 'weekly':
            return 'week';
        case 'fortnightly':
            return 'fortnight';
        case '4week':
        case 'every 4 weeks':
            return 'every 4th week';
    }
}

/***/ }),

/***/ 491:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _jquery = __webpack_require__(11);

var _jquery2 = _interopRequireDefault(_jquery);

function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : { default: obj };
}

var outgoings = [];

if ((0, _jquery2.default)('.js-chart-settings input[name="outcome"]').val()) {
    (0, _jquery2.default)('.js-chart-settings-outgoings input').each(function (index, element) {
        outgoings.push(element.value);
    });
}

exports.default = {
    labels: ["Household", "Expenditure", "Left over"],
    datasets: [{
        data: outgoings,
        backgroundColor: ["#86CBE3", "#696E72", "#CFEAF4"],
        label: ['Outgoings'],
        borderWidth: 1,
        borderColor: ["#86CBE3", "#696E72", "#CFEAF4"],
        hoverBackgroundColor: ["#86CBE3", "#696E72", "#CFEAF4"],
        hoverBorderWidth: 1,
        hoverBorderColor: ["#86CBE3", "#696E72", "#CFEAF4"]
    }]
};

/***/ }),

/***/ 492:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _jquery = __webpack_require__(11);

var _jquery2 = _interopRequireDefault(_jquery);

function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : { default: obj };
}

var outgoings = [];

if ((0, _jquery2.default)('.js-chart-settings input[name="outcome"]').val()) {
    (0, _jquery2.default)('.js-chart-settings-outgoings input').each(function (index, element) {
        outgoings.push(element.value);
    });
}

exports.default = {
    labels: ["Household", "Expenditure", "Left over"],
    datasets: [{
        data: outgoings,
        backgroundColor: ["#86CBE3", "#696E72", "#CB490D"],
        label: ['Outgoings'],
        borderWidth: 1,
        borderColor: ["#86CBE3", "#696E72", "#CB490D"],
        hoverBackgroundColor: ["#86CBE3", "#696E72", "#CB490D"],
        hoverBorderWidth: 1,
        hoverBorderColor: ["#86CBE3", "#696E72", "#CB490D"]
    }]
};

/***/ }),

/***/ 493:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _jquery = __webpack_require__(11);

var _jquery2 = _interopRequireDefault(_jquery);

function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : { default: obj };
}

var overBudget = [],
    outgoings = [];

if ((0, _jquery2.default)('.js-chart-settings input[name="outcome"]').val()) {
    overBudget.push((0, _jquery2.default)('.js-chart-settings input[name="monthly-spare"]').val());
    overBudget.push((0, _jquery2.default)('.js-chart-settings input[name="outgoing-total"]').val());

    // Empty values to match with the over budget dataset
    outgoings.push(0, 0);

    (0, _jquery2.default)('.js-chart-settings-outgoings input').each(function (index, element) {
        // Skip over leftover and recommended
        if (element.name == 'leftover') {
            return;
        }
        outgoings.push(element.value);
    });
}

exports.default = {
    labels: ["Left over", "", "Household", "Expenditure"],
    datasets: [{
        data: overBudget,
        backgroundColor: ["#CB490D", "transparent"],
        label: ['Overflow'],
        hoverBackgroundColor: ["#CB490D", "transparent"],
        borderWidth: 1,
        borderColor: ["#CB490D", "transparent"],
        hoverBorderWidth: 1,
        hoverBorderColor: ["#CB490D", "transparent"]
    }, {
        data: outgoings,
        backgroundColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        label: ['Outgoings'],
        borderWidth: 1,
        borderColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        hoverBackgroundColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        hoverBorderWidth: 1,
        hoverBorderColor: ['transparent', 'transparent', "#86CBE3", "#696E72"]
    }, {
        data: outgoings,
        backgroundColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        label: ['Outgoings'],
        borderWidth: 1,
        borderColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        hoverBackgroundColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        hoverBorderWidth: 1,
        hoverBorderColor: ['transparent', 'transparent', "#86CBE3", "#696E72"]
    }, {
        data: outgoings,
        backgroundColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        label: ['Outgoings'],
        borderWidth: 1,
        borderColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        hoverBackgroundColor: ['transparent', 'transparent', "#86CBE3", "#696E72"],
        hoverBorderWidth: 1,
        hoverBorderColor: ['transparent', 'transparent', "#86CBE3", "#696E72"]
    }]
};

/***/ })

}]);
//# sourceMappingURL=2.bundle.js.map