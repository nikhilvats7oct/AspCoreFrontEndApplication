

var GTMModule = (function () {

    //-----------------------------------
    // PRIVATE VARIABLE
    //-----------------------------------
    
    var globalVars = {

        gtmProperties: {
            accountRef: 'account_ref',
            guid: 'guid',
            result: 'result',
            errorMessage: 'error_message',
            evnt: 'event',
            step: 'step',
            actionTaken: 'action_taken',
            paymentType: 'payment_type',
            paymentAmount: 'payment_amount',
            discountAvailable: 'discount_available',
            balanceSelected: 'balance_selected',
            instalmentStartDate: 'instalment_start_date',
            userStatus: 'user_status',
            planStatus: 'plan_status',
            planType: 'plan_type',
            paymentDetail: 'payment_detail',
            monthlyIncome: 'monthly_income',
            dependants: 'dependants',
            priorityHouseholdBills: 'priority_household_bills',
            otherPriorityExpenditure: 'other_priority_expenditure',
            otherHouseholdBills: 'other_household_bills',
            travel: 'travel',
            otherOutgoings: 'other_outgoings',
            monthlyExpences: 'monthly_expences',
            monthlyDisposableIncome: 'monthly_disposable_income',
            payPredictorBalance: 'pay_predictor_balance',
            payPredictorNumberOfInstalments: 'pay_predictor_number_of_instalments',
            payPredictorMonthlyPayment: 'pay_predictor_payment_amount',  
            payPredictorInstalmentPeriod: 'pay_predictor_instalment_period',
            housingStatus: 'housing_status',
            employmentStatus: 'employment_status',
            payment_option_chosen: 'payment_option_chosen'
        }
    };

    // send data to Tag Manager
    callGTM = function (dataLayer, data) {
        dataLayer.push(data);
    }

    return {

    };

})();

