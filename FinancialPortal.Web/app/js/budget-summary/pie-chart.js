import $ from 'jquery';
import Chart from 'chart.js';
import tinycolor from 'tinycolor2';

import {getDurationConstants} from '../_helpers';

import configAffordable from './_config-afforable';
import configUnaffordable from './_config-unaffordable';
import configNegative from './_config-negative';

export default () => {
    let dataConfig,
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
        centerIndex;

    // Setup 
    $('.js-chart-settings-income input').each((index, element) => {
        element.value = element.value || income[element.name];
        income[element.name] = element.value;
    });
    $('.js-chart-settings-outgoings input').each((index, element) => {
        element.value = element.value || outgoings[element.name];
        outgoings[element.name] = element.value;
    });

    switch($('.js-chart-settings input[name="outcome"]').val()) {
        case 'affordable':
        case 'postive':
            dataConfig = configAffordable;
            pieOutcome = true;
            break;
        case 'unaffordable':
        case 'not-reccomended':
            pieOutcome = true;
            dataConfig = configUnaffordable;
            break;
        case 'negative':
        default:
            dataConfig = configNegative;
            outgoings.leftover = Math.abs(outgoings.leftover);
    }
    
    centerIndex = dataConfig.labels.indexOf('Left over');

    updateIncome();
    updateOutgoings();

    const ctx = document.getElementById('myChart');

    if (ctx) {
        ctx.addEventListener('mouseout', (e) => {
            triggerPieInfo(dataConfig.datasets[0].backgroundColor[centerIndex], dataConfig.labels[centerIndex], dataConfig.datasets[0].data[centerIndex]);
        });

        window.myDoughnut = new Chart(ctx, {
            type: 'doughnut',
            data: dataConfig,
            options: {
                responsive: true,
                legendCallback: function (chart) {

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
                    onComplete() {
                        _showCenter();
                    },
                },
                cutoutPercentage: 50,
                onHover(e, t) {
                    if (t.length) {
                        let index = t[0]._index,
                            datasetsIndex = t[0]._datasetIndex;

                        if (dataConfig.labels[index] != '') {
                            triggerPieInfo(dataConfig.datasets[datasetsIndex].backgroundColor[index], dataConfig.labels[index], dataConfig.datasets[datasetsIndex].data[index]);
                        }
                    }
                },
                onClick(e, t) {
                    if (t.length) {
                        let index = t[0]._index,
                            datasetsIndex = t[0]._datasetIndex;

                        if (dataConfig.labels[index] != '') {
                            triggerPieInfo(dataConfig.datasets[datasetsIndex].backgroundColor[index], dataConfig.labels[index], dataConfig.datasets[datasetsIndex].data[index]);
                        }
                    }
                },
            }
        });

        $('.chart-legend').html(window.myDoughnut.generateLegend());
    }

    $('.js-budget-frequency').on('change', (e) => {
        let $this = $(e.currentTarget),
            $thisVal = $this.val();

        $('.js-spare-money').text('£' +
            _calcFromMonthly(
                parseFloat($('.js-chart-settings input[name="monthly-spare"]').val()),
                $thisVal
            ).toFixed(2)
        );
        $('.js-income-total').text('£' + 
            _calcFromMonthly(
                parseFloat($('.js-chart-settings input[name="income-total"]').val()), 
                $thisVal
            ).toFixed(2)
        );
        $('.js-outgoings-total').text('£' + 
            _calcFromMonthly(
                parseFloat($('.js-chart-settings input[name="outgoing-total"]').val()), 
                $thisVal
            ).toFixed(2)
        );

        $('.js-money-frequency').text(_getFormattedFrequency($thisVal));

        updateIncome($thisVal);
        updateOutgoings($thisVal);
        updatePieData();
        triggerPieInfo(dataConfig.datasets[0].backgroundColor[centerIndex], dataConfig.labels[centerIndex], dataConfig.datasets[0].data[centerIndex]);
        $('.chart-legend').html(window.myDoughnut.generateLegend());
    });

    // Public Functions
    function triggerPieInfo(backgroundColor, label, price) {
        let checkifColorIsDark = tinycolor(backgroundColor);

        if (checkifColorIsDark.isDark()) {
            $('.js-chart-info').css({
                backgroundColor: backgroundColor,
                color: 'white'
            });
        } else {
            $('.js-chart-info').css({
                backgroundColor: backgroundColor,
                color: 'black'
            });
        }

        $('.js-chart-heading').text(label + ':');

        if (!pieOutcome && label == 'Left over') {
            $('.js-chart-price').text('-£' + parseFloat(price).toFixed(2));
        } else {
            $('.js-chart-price').text('£' + parseFloat(price).toFixed(2));
        }
    }

    function updateIncome(frequency = 'monthly') {
        for (let elem in income) {
            activeIncome[elem] = _calcFromMonthly(parseFloat(income[elem]), frequency).toFixed(2);
        }

        $('.js-income-salary').text('£' + activeIncome.salary);
        $('.js-income-benefits').text('£' + activeIncome.benefits);
        $('.js-income-other').text('£' + activeIncome.other);
    }

    function updateOutgoings(frequency = 'monthly') {
        for (let elem in outgoings) {
            activeOutgoings[elem] = _calcFromMonthly(parseFloat(outgoings[elem]), frequency).toFixed(2);
        }

        if(!pieOutcome) {
            activeOutgoings.monthlySpare = _calcFromMonthly(parseFloat($('.js-chart-settings input[name="monthly-spare"]').val()), frequency).toFixed(2);
            activeOutgoings.outgoingTotal = _calcFromMonthly(parseFloat($('.js-chart-settings input[name="outgoing-total"]').val()), frequency).toFixed(2);
            $('.js-outgoings-leftover').text('-£' + activeOutgoings.leftover);
        }
        else {
            $('.js-outgoings-leftover').text('£' + activeOutgoings.leftover);
        }

        $('.js-outgoings-household').text('£' + activeOutgoings.household);
        $('.js-outgoings-expenditure').text('£' + activeOutgoings.expenditure);
        $('.js-outgoings-other').text('£' + activeOutgoings.other);
        $('.js-outgoings-recommended').text('£' + activeOutgoings.recommended);   
    }

    function updatePieData() {
        window.myDoughnut.options.animation.duration = 375;

        if(pieOutcome) {
            window.myDoughnut.data.datasets.forEach((element) =>{
                element.data = [];
                element.data.push(
                    activeOutgoings.household,
                    activeOutgoings.expenditure,
                );

                if(parseFloat(activeOutgoings.recommended) > 0) {
                    element.data.push(activeOutgoings.recommended);
                }

                element.data.push(activeOutgoings.leftover);
            });
        }
        else {
            window.myDoughnut.data.datasets.forEach((element) =>{
                element.data = [];

                if(element.label == 'Overflow') {
                    element.data.push(
                        activeOutgoings.monthlySpare,
                        activeOutgoings.outgoingTotal
                    );
                }
                else {
                    element.data.push(
                        0,
                        0,
                        activeOutgoings.household,
                        activeOutgoings.expenditure,
                    );
                }
            });
        }

        $('.js-chart').removeClass('chart--is-animating');
        window.myDoughnut.update();
    }
}

// Private Functions
function _showCenter() {
    $('.js-chart').addClass('chart--is-animating');
}

function _calcFromMonthly(number, frequency) {
    const { week, fortnight, every4week } = getDurationConstants();

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