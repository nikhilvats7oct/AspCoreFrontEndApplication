import $ from 'jquery';

let overBudget = [],
    outgoings = [];

if($('.js-chart-settings input[name="outcome"]').val()) {
    overBudget.push($('.js-chart-settings input[name="monthly-spare"]').val());
    overBudget.push($('.js-chart-settings input[name="outgoing-total"]').val());

    // Empty values to match with the over budget dataset
    outgoings.push(0, 0);

    $('.js-chart-settings-outgoings input').each((index, element) => {
        // Skip over leftover and recommended
        if(element.name == 'leftover') {
            return;
        }
        outgoings.push(element.value);
    });
}

export default {
    labels: [
        "Left over", 
        "", 
        "Household", 
        "Expenditure", 
    ],
    datasets: [{
        data: overBudget,
        backgroundColor: [
            "#CB490D", 
            "transparent", 
        ],
        label: [
            'Overflow'
        ],
        hoverBackgroundColor: [
            "#CB490D", 
            "transparent", 
        ],
        borderWidth: 1,
        borderColor: [
            "#CB490D", 
            "transparent", 
        ],
        hoverBorderWidth: 1,
        hoverBorderColor: [
            "#CB490D", 
            "transparent", 
        ],
    },
    {
        data: outgoings,
        backgroundColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        label: [
            'Outgoings'
        ],
        borderWidth: 1,
        borderColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        hoverBackgroundColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        hoverBorderWidth: 1,
        hoverBorderColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
    },
    {
        data: outgoings,
        backgroundColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        label: [
            'Outgoings'
        ],
        borderWidth: 1,
        borderColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        hoverBackgroundColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        hoverBorderWidth: 1,
        hoverBorderColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
    },
    {
        data: outgoings,
        backgroundColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        label: [
            'Outgoings'
        ],
        borderWidth: 1,
        borderColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        hoverBackgroundColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
        hoverBorderWidth: 1,
        hoverBorderColor: [
            'transparent',
            'transparent',
            "#86CBE3",
            "#696E72", 
        ],
    }]
};