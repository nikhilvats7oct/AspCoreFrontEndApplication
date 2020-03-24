import $ from 'jquery';

let outgoings = [];

if($('.js-chart-settings input[name="outcome"]').val()) {
    $('.js-chart-settings-outgoings input').each((index, element) => {
        outgoings.push(element.value);
    });
}

export default {
    labels: [
        "Household", 
        "Expenditure", 
        "Other", 
        "Recommended payment", 
        "Leftover",
    ],
    datasets: [{
        data: outgoings,
        backgroundColor: [
            "#86CBE3", 
            "#696E72", 
            "#FFCB05",
            "#A6CE42",
            "#EEEEEE"
        ],
        label: [
            'Outgoings'
        ],
        borderWidth: 1,
        borderColor: [
            "#86CBE3",
            "#696E72",
            "#FFCB05",
            "#A6CE42",
            "#EEEEEE"
        ],
        hoverBackgroundColor: [
            "#86CBE3",
            "#696E72",
            "#FFCB05",
            "#A6CE42",
            "#EEEEEE"
        ],
        hoverBorderWidth: 1,
        hoverBorderColor: [
            "#86CBE3",
            "#696E72",
            "#FFCB05",
            "#A6CE42",
            "#EEEEEE"
        ],
    }]
};