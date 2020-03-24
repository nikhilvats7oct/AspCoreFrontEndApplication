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
        "Left over",
    ],
    datasets: [{
        data: outgoings,
        backgroundColor: [
            "#86CBE3",
            "#696E72",
            "#CB490D",
        ],
        label: [
            'Outgoings'
        ],
        borderWidth: 1,
        borderColor: [
            "#86CBE3",
            "#696E72",
            "#CB490D",
        ],
        hoverBackgroundColor: [
            "#86CBE3",
            "#696E72",
            "#CB490D",
        ],
        hoverBorderWidth: 1,
        hoverBorderColor: [
            "#86CBE3",
            "#696E72",
            "#CB490D",
        ],
    }]
};