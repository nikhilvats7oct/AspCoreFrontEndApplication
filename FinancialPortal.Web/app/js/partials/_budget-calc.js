import $ from 'jquery';
import cleave from 'cleave.js';
import {getFloatValue, parseFloatWithCommas, getDurationConstants} from '../_helpers';

export default function() {
    let household = {
            circumstances: '',
            adults: 1,
            children_0_16: 0,
            children_17_18: 0,
            housing: '',
        },
        income = {
            earnings: 0,
            benefits: 0,
            pensions: 0,
            other: 0,
            earnings_term: '',
            benefits_term: '',
            pensions_term: '',
            other_term: '',
            total: 0,
            children: false,
        },
        outgoings = {
            household: 0.00,
            expenditure: 0.00,
            other: 0.00,
            total: 0.00
        },
        min,
        max,
        staticOutgoings = {},
        numberalFields = [];

    let numberalFieldOptions = { 
        numeral: true,
        onValueChanged: function (e) {
            
            // Update user payment amount
            ValueChanged(this.element) 
        }
    };

    function ValueChanged(inputField) {

        // Update user payment amount
        let $this = $(inputField),
            category = $this.data('category'),
            $thisVal = getFloatValue($this.val()),
            calculatedValue,
            staticValue,
            frequency = $(inputField).hasClass('arrears-amount') ? 'monthly' : $this.closest('.js-field-parent').find('.js-payment-frequency').val();

        switch (category) {
            case 'household':
                calculatedValue = getFloatValue(outgoings.household);
                if (staticOutgoings.household) {
                    staticValue = getFloatValue(staticOutgoings.household);
                }
                break;
            case 'expenditure':
                calculatedValue = getFloatValue(outgoings.expenditure);
                if (staticOutgoings.expenditure) {
                    staticValue = getFloatValue(staticOutgoings.expenditure);
                }
                break;
            default:
                calculatedValue = getFloatValue(outgoings.other);
                if (staticOutgoings.other) {
                    staticValue = getFloatValue(staticOutgoings.other);
                }
        }

        if (isNaN($thisVal)) {
            $thisVal = 0;
        }

        if ($thisVal != $this.data('oldvalue')) {
            if ($thisVal == 0) {
                calculatedValue = (Object.keys(staticOutgoings).length) ? staticValue + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
            }
            else if ($thisVal > $this.data('oldvalue')) {
                calculatedValue += _calcFromMonthly(
                    ($thisVal - $this.data('oldvalue')),
                    frequency
                );
            }
            else if ($thisVal < $this.data('oldvalue')) {
                calculatedValue -= _calcFromMonthly(
                    ($this.data('oldvalue') - $thisVal),
                    frequency
                );
            }
            else {
                calculatedValue += _calcFromMonthly(
                    $thisVal,
                    frequency
                );
            }
        }

        switch (category) {
            case 'household':
                outgoings.household = calculatedValue;
                break;
            case 'expenditure':
                outgoings.expenditure = calculatedValue;
                break;
            default:
                outgoings.other = calculatedValue;
        }

        $this.data('oldvalue', $thisVal);
        updateOutgoingInfoCard();
    }

    if($('.js-min-max').length) {
        min = getFloatValue($('.js-min-max input[name=min]').val());
        max = getFloatValue($('.js-min-max input[name=max]').val());
    }

    // Setup
    checkReplay();

    mergeIncome();
    mergeOutgoings();
    setupIncomeInfoCard();
    // END Setup

    // jQuery Listeners
    $('.js-budget-calc .js-payment-input').each((index, element) => {
        numberalFields.push(new cleave(element, numberalFieldOptions)); 
    });

    // Update user payment amount
    $('.js-budget-calc').on('blur', '.js-payment-input', (e) => {
        if($(e.currentTarget).val() != '') {
            $(e.currentTarget).val(parseFloatWithCommas($(e.currentTarget).val()));
        }
    });

    $('.js-budget-calc .js-payment-input').on('keydown', (e) => {
        console.log('KEY DOWN' + e.which);
    });

    $('.arrears-checkbox').on('change', (e) => {
        if (!e.currentTarget.checked) {

            let amountField = document.getElementById(e.currentTarget.dataset.amountField);
            $(amountField).val('0.00');
            ValueChanged(amountField);
        }
    });

    $('.js-payment-input').focus(function () {
        let $this = $(this);
        if ($this.val() == '0.00') {
            $this.val('');
        }
    });

    $('.js-payment-input').focusout(function () {
        let $this = $(this);
        if (!$.trim($this.val())) {
            $this.val('0.00');
        }
    });

    $('.js-payment-input')

    // Expenditure - Warning Inputs
    $('.js-budget-calc').on('blur', '.js-payment-input[data-warn]', (e) => {
        let $this = $(e.currentTarget),
            $formElement = $this.closest('.js-field-parent'),
            $select = $formElement.find('.js-payment-frequency'),
            fieldMin = min,
            fieldMax = max;

        if($this.data('min') !== undefined && $this.data('min') > 0) {
            fieldMin = $this.data('min');
        }

        if($this.data('max') !== undefined && $this.data('max') > 0) {
            fieldMax = $this.data('max');
        }

        calcMinMax($this, fieldMin, fieldMax, $select.val());
    });

    // Calculate all fields once the jquery fields have binded
    calculateAllFields();

    $('.js-budget-calc').on('change', '.js-payment-frequency', (e) => {
        let $this = $(e.currentTarget),
            $formElement = $this.closest('.js-field-parent'),
            $input = $formElement.find('.js-payment-input'),
            fieldMin = min,
            fieldMax = max;
        
        if(getFloatValue($input.val()) > 0) { 
            let category = $input.data('category');

            switch(category) {
                case 'household':
                    outgoings.household = (Object.keys(staticOutgoings).length) ? parseFloat(staticOutgoings.household) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
                    break;
                case 'expenditure':
                    outgoings.expenditure = (Object.keys(staticOutgoings).length) ? parseFloat(staticOutgoings.expenditure) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
                    break;
                default:
                    outgoings.other = (Object.keys(staticOutgoings).length) ? parseFloat(staticOutgoings.other) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
            }

            updateOutgoingInfoCard();

            if($input.data('warn')) {
                if($input.data('min') !== undefined && $input.data('min') > 0) {
                    fieldMin = $input.data('min');
                }
        
                if($input.data('max') !== undefined && $input.data('max') > 0) {
                    fieldMax = $input.data('max');
                }

                calcMinMax($input, fieldMin, fieldMax, $this.val());
            }
        }
    });

    $('.js-budget-calc').on('click', '.form__element__notifcation .jw-icon-cross', (e) => {
        let $this = $(e.currentTarget),
            $thisWarning = $this.closest('.js-warning'),
            $formElement = $this.closest('.form__element--2row');

        $thisWarning.slideUp();
        $formElement.removeClass('form__element--2row--warning');
    });

    $('.js-budget-calc #circumstances').on("change", function (e) {
        let $closestFormValidator = $(this).closest(".form").first().validate();
        // Validate this element, thsi will display errors if invalid.
        $closestFormValidator.element(".js-budget-calc #circumstances");
    });

    $('.js-budget-calc #housing').on("change", function (e) {
        let $closestFormValidator = $(this).closest(".form").first().validate();
        // Validate this element, thsi will display errors if invalid.
        $closestFormValidator.element(".js-budget-calc #housing");
    });

    // END Expenditure - Warning Inputs

    // Public Functions
    function checkReplay() {
        if($('.js-household').length > 0) {
            mergeHousehold();
        }

        if($('.js-income').length > 0) {
            mergeIncome(true);
        }
    }

    function showChildField() {
        // TODO: Cast income.children as a bool
        if(income.children != 'false') {
            $('.js-budget-calc .js-child-field').removeClass('js-child-field');
        }
    }

    function getTotal() {
        return parseFloat(outgoings.household) + parseFloat(outgoings.expenditure) + parseFloat(outgoings.other);
    }

    function mergeHousehold() {
        $('.js-household input').each((index, element) => {
            element.value = element.value || household[element.name];
            household[element.name] = element.value;
        });

        setHouseholdFields();
    }

    function mergeIncome(replay) {
        $('.js-income input').each((index, element) => {
            element.value = element.value || income[element.name];
            income[element.name] = element.value;
        });
        showChildField();

        if(replay) {
            setIncomeFields();
        }
    }

    function mergeOutgoings() {
        if($('.js-outgoings').length) {
            $('.js-outgoings input').each((index, element) => {
                element.value = element.value || outgoings[element.name];
                outgoings[element.name] = element.value;
            });
            if($('.js-outgoings input[name="no-replace"]').length) {
                staticOutgoings = Object.assign(staticOutgoings, outgoings);
            }
        }
    }

    function setupIncomeInfoCard() {
        $('.js-budget-income-info-box').find('.js-budget-total').text('£' + parseFloatWithCommas(income.total));
        $('.js-budget-income-info-box').find('.js-budget-earnings').text('£' + parseFloatWithCommas(income.earnings));
        $('.js-budget-income-info-box').find('.js-budget-benefits').text('£' + parseFloatWithCommas(income.benefits));
        $('.js-budget-income-info-box').find('.js-budget-pensions').text('£' + parseFloatWithCommas(income.pensions));
        $('.js-budget-income-info-box').find('.js-budget-other').text('£' + parseFloatWithCommas(income.other));
    }

    function updateOutgoingInfoCard() {
        $('.js-budget-outgoings-info-box').find('.js-budget-bills').text('£' + parseFloatWithCommas(outgoings.household));
        $('.js-budget-outgoings-info-box').find('.js-budget-expenditure').text('£' + parseFloatWithCommas(outgoings.expenditure));
        $('.js-budget-outgoings-info-box').find('.js-budget-other').text('£' + parseFloatWithCommas(outgoings.other));
        $('.js-budget-outgoings-info-box').find('.js-budget-total').text('£' + parseFloatWithCommas(getTotal()));
    }

    function calcMinMax(input, minValue, maxValue, frequency) {
        const { week, fortnight, every4week } = getDurationConstants();
        let $inputVal = getFloatValue($(input).val()),
            $formElement = $(input).closest('.form__element--2row');
    
        switch(frequency) {
            case 'monthly':
                checkWarnFields($inputVal, $formElement, minValue, maxValue);
                break;
            case 'weekly':
                checkWarnFields($inputVal, $formElement, minValue / week, maxValue / week);
                break;
            case 'fortnightly':
                checkWarnFields($inputVal, $formElement, minValue / fortnight, maxValue / fortnight);
                break;
            case '4week':
            case 'every 4 weeks':
                checkWarnFields($inputVal, $formElement, minValue / every4week, maxValue / every4week);
                break;
        }
    }

    function checkWarnFields($inputVal, $formElement, minValue, maxValue) {
        if($inputVal != 0 && $inputVal < minValue){
            $formElement.find('.js-warning-value').text('low');
            $formElement.addClass('form__element--2row--warning').find('.js-warning').slideDown();
        }
        else if($inputVal > maxValue){
            $formElement.find('.js-warning-value').text('high');
            $formElement.addClass('form__element--2row--warning').find('.js-warning').slideDown();
        }
        else {
            $formElement.removeClass('form__element--2row--warning').find('.js-warning').slideUp();
        }
    }

    // Replay Fields
    function setHouseholdFields() {
        $('.js-budget-calc #circumstances').val(household.circumstances).trigger('change');
        $('.js-budget-calc #adults').val(household.adults).trigger('change');
        $('.js-budget-calc #children-0-16').val(household.children_0_16).trigger('change');
        $('.js-budget-calc #children-17-18').val(household.children_17_18).trigger('change');
        $('.js-budget-calc #housing').val(household.housing).trigger('change');
    }

    function setIncomeFields() {
        $('.js-budget-calc #earnings').val(income.earnings).trigger('change');
        $('.js-budget-calc #benefits').val(income.benefits).trigger('change');
        $('.js-budget-calc #pensions').val(income.pensions).trigger('change');
        $('.js-budget-calc #other').val(income.other).trigger('change');

        if(income.earnings_term != '') {
            $('.js-budget-calc #earnings-select').val(income.earnings_term).trigger('change');
        }

        if(income.benefits_term != '') {
            $('.js-budget-calc #benefits-select').val(income.benefits_term).trigger('change');
        }

        if(income.pensions_term != '') {
            $('.js-budget-calc #pensions-select').val(income.pensions_term).trigger('change');
        }

        if(income.other_term != '') {
            $('.js-budget-calc #other-select').val(income.other_term).trigger('change');
        }
    }

    function calculateAllFields() {
        $('.js-budget-calc .js-payment-input').each((index, element) => {
            let $this = $(element),
                category = $this.data('category');

            switch(category) {
                case 'household':
                    outgoings.household = (Object.keys(staticOutgoings).length) ? parseFloat(staticOutgoings.household) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
                    break;
                case 'expenditure':
                    outgoings.expenditure = (Object.keys(staticOutgoings).length) ? parseFloat(staticOutgoings.expenditure) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
                    break;
                default:
                    outgoings.other = (Object.keys(staticOutgoings).length) ? parseFloat(staticOutgoings.other) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
            }

            $this.trigger('blur');
        });

        updateOutgoingInfoCard();
    }
    // END Replay Fields

};

// Private functions
function _calculateCategoryTotal(category) {
    let inputs,
        total = 0;

    switch(category) {
        case 'household':
            inputs = $('.js-budget-calc .js-payment-input[data-category="household"]');
            inputs.each((index, element) => {
                let frequency = $(element).closest('.js-field-parent').find('.js-payment-frequency').val();

                if(!isNaN(parseFloat($(element).val()))){
                    total += _calcFromMonthly(
                        getFloatValue($(element).val()),
                        frequency
                    )
                }
            });

            return total;
        case 'expenditure': 
            inputs = $('.js-budget-calc .js-payment-input[data-category="expenditure"]');
            inputs.each((index, element) => {
                let frequency = $(element).closest('.js-field-parent').find('.js-payment-frequency').val();

                if(!isNaN(parseFloat($(element).val()))){
                    total += _calcFromMonthly(
                        getFloatValue($(element).val()),
                        frequency
                    )
                }
            });

            return total;
        default:
            inputs = $('.js-budget-calc .js-payment-input[data-category="other"]');
            inputs.each((index, element) => {
                let frequency = $(element).closest('.js-field-parent').find('.js-payment-frequency').val();

                if(!isNaN(parseFloat($(element).val()))){
                    total += _calcFromMonthly(
                        getFloatValue($(element).val()),
                        frequency
                    )
                }
            });
            return total;
    }
}

function _calcFromMonthly(number, frequency) {
    const { week, fortnight, every4week } = getDurationConstants();

    switch (frequency) {
        case 'monthly':
            return number;
        case 'weekly':
            return number * week;
        case 'fortnightly':
            return number * fortnight;
        case '4week':
        case 'every 4 weeks':
            return number * every4week;
    }
}