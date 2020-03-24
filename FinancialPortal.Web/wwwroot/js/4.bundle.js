(window["webpackJsonp"] = window["webpackJsonp"] || []).push([[4],{

/***/ 486:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

exports.default = function () {
    var household = {
        circumstances: '',
        adults: 1,
        children_0_16: 0,
        children_17_18: 0,
        housing: ''
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
        children: false
    },
        outgoings = {
        household: 0.00,
        expenditure: 0.00,
        other: 0.00,
        total: 0.00
    },
        min = void 0,
        max = void 0,
        staticOutgoings = {},
        numberalFields = [];

    var numberalFieldOptions = {
        numeral: true,
        onValueChanged: function onValueChanged(e) {

            // Update user payment amount
            ValueChanged(this.element);
        }
    };

    function ValueChanged(inputField) {

        // Update user payment amount
        var $this = (0, _jquery2.default)(inputField),
            category = $this.data('category'),
            $thisVal = (0, _helpers.getFloatValue)($this.val()),
            calculatedValue = void 0,
            staticValue = void 0,
            frequency = (0, _jquery2.default)(inputField).hasClass('arrears-amount') ? 'monthly' : $this.closest('.js-field-parent').find('.js-payment-frequency').val();

        switch (category) {
            case 'household':
                calculatedValue = (0, _helpers.getFloatValue)(outgoings.household);
                if (staticOutgoings.household) {
                    staticValue = (0, _helpers.getFloatValue)(staticOutgoings.household);
                }
                break;
            case 'expenditure':
                calculatedValue = (0, _helpers.getFloatValue)(outgoings.expenditure);
                if (staticOutgoings.expenditure) {
                    staticValue = (0, _helpers.getFloatValue)(staticOutgoings.expenditure);
                }
                break;
            default:
                calculatedValue = (0, _helpers.getFloatValue)(outgoings.other);
                if (staticOutgoings.other) {
                    staticValue = (0, _helpers.getFloatValue)(staticOutgoings.other);
                }
        }

        if (isNaN($thisVal)) {
            $thisVal = 0;
        }

        if ($thisVal != $this.data('oldvalue')) {
            if ($thisVal == 0) {
                calculatedValue = Object.keys(staticOutgoings).length ? staticValue + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
            } else if ($thisVal > $this.data('oldvalue')) {
                calculatedValue += _calcFromMonthly($thisVal - $this.data('oldvalue'), frequency);
            } else if ($thisVal < $this.data('oldvalue')) {
                calculatedValue -= _calcFromMonthly($this.data('oldvalue') - $thisVal, frequency);
            } else {
                calculatedValue += _calcFromMonthly($thisVal, frequency);
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

    if ((0, _jquery2.default)('.js-min-max').length) {
        min = (0, _helpers.getFloatValue)((0, _jquery2.default)('.js-min-max input[name=min]').val());
        max = (0, _helpers.getFloatValue)((0, _jquery2.default)('.js-min-max input[name=max]').val());
    }

    // Setup
    checkReplay();

    mergeIncome();
    mergeOutgoings();
    setupIncomeInfoCard();
    // END Setup

    // jQuery Listeners
    (0, _jquery2.default)('.js-budget-calc .js-payment-input').each(function (index, element) {
        numberalFields.push(new _cleave2.default(element, numberalFieldOptions));
    });

    // Update user payment amount
    (0, _jquery2.default)('.js-budget-calc').on('blur', '.js-payment-input', function (e) {
        if ((0, _jquery2.default)(e.currentTarget).val() != '') {
            (0, _jquery2.default)(e.currentTarget).val((0, _helpers.parseFloatWithCommas)((0, _jquery2.default)(e.currentTarget).val()));
        }
    });

    (0, _jquery2.default)('.js-budget-calc .js-payment-input').on('keydown', function (e) {
        console.log('KEY DOWN' + e.which);
    });

    (0, _jquery2.default)('.arrears-checkbox').on('change', function (e) {
        if (!e.currentTarget.checked) {

            var amountField = document.getElementById(e.currentTarget.dataset.amountField);
            (0, _jquery2.default)(amountField).val('0.00');
            ValueChanged(amountField);
        }
    });

    (0, _jquery2.default)('.js-payment-input').focus(function () {
        var $this = (0, _jquery2.default)(this);
        if ($this.val() == '0.00') {
            $this.val('');
        }
    });

    (0, _jquery2.default)('.js-payment-input').focusout(function () {
        var $this = (0, _jquery2.default)(this);
        if (!_jquery2.default.trim($this.val())) {
            $this.val('0.00');
        }
    });

    (0, _jquery2.default)('.js-payment-input');

    // Expenditure - Warning Inputs
    (0, _jquery2.default)('.js-budget-calc').on('blur', '.js-payment-input[data-warn]', function (e) {
        var $this = (0, _jquery2.default)(e.currentTarget),
            $formElement = $this.closest('.js-field-parent'),
            $select = $formElement.find('.js-payment-frequency'),
            fieldMin = min,
            fieldMax = max;

        if ($this.data('min') !== undefined && $this.data('min') > 0) {
            fieldMin = $this.data('min');
        }

        if ($this.data('max') !== undefined && $this.data('max') > 0) {
            fieldMax = $this.data('max');
        }

        calcMinMax($this, fieldMin, fieldMax, $select.val());
    });

    // Calculate all fields once the jquery fields have binded
    calculateAllFields();

    (0, _jquery2.default)('.js-budget-calc').on('change', '.js-payment-frequency', function (e) {
        var $this = (0, _jquery2.default)(e.currentTarget),
            $formElement = $this.closest('.js-field-parent'),
            $input = $formElement.find('.js-payment-input'),
            fieldMin = min,
            fieldMax = max;

        if ((0, _helpers.getFloatValue)($input.val()) > 0) {
            var category = $input.data('category');

            switch (category) {
                case 'household':
                    outgoings.household = Object.keys(staticOutgoings).length ? parseFloat(staticOutgoings.household) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
                    break;
                case 'expenditure':
                    outgoings.expenditure = Object.keys(staticOutgoings).length ? parseFloat(staticOutgoings.expenditure) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
                    break;
                default:
                    outgoings.other = Object.keys(staticOutgoings).length ? parseFloat(staticOutgoings.other) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
            }

            updateOutgoingInfoCard();

            if ($input.data('warn')) {
                if ($input.data('min') !== undefined && $input.data('min') > 0) {
                    fieldMin = $input.data('min');
                }

                if ($input.data('max') !== undefined && $input.data('max') > 0) {
                    fieldMax = $input.data('max');
                }

                calcMinMax($input, fieldMin, fieldMax, $this.val());
            }
        }
    });

    (0, _jquery2.default)('.js-budget-calc').on('click', '.form__element__notifcation .jw-icon-cross', function (e) {
        var $this = (0, _jquery2.default)(e.currentTarget),
            $thisWarning = $this.closest('.js-warning'),
            $formElement = $this.closest('.form__element--2row');

        $thisWarning.slideUp();
        $formElement.removeClass('form__element--2row--warning');
    });

    (0, _jquery2.default)('.js-budget-calc #circumstances').on("change", function (e) {
        var $closestFormValidator = (0, _jquery2.default)(this).closest(".form").first().validate();
        // Validate this element, thsi will display errors if invalid.
        $closestFormValidator.element(".js-budget-calc #circumstances");
    });

    (0, _jquery2.default)('.js-budget-calc #housing').on("change", function (e) {
        var $closestFormValidator = (0, _jquery2.default)(this).closest(".form").first().validate();
        // Validate this element, thsi will display errors if invalid.
        $closestFormValidator.element(".js-budget-calc #housing");
    });

    // END Expenditure - Warning Inputs

    // Public Functions
    function checkReplay() {
        if ((0, _jquery2.default)('.js-household').length > 0) {
            mergeHousehold();
        }

        if ((0, _jquery2.default)('.js-income').length > 0) {
            mergeIncome(true);
        }
    }

    function showChildField() {
        // TODO: Cast income.children as a bool
        if (income.children != 'false') {
            (0, _jquery2.default)('.js-budget-calc .js-child-field').removeClass('js-child-field');
        }
    }

    function getTotal() {
        return parseFloat(outgoings.household) + parseFloat(outgoings.expenditure) + parseFloat(outgoings.other);
    }

    function mergeHousehold() {
        (0, _jquery2.default)('.js-household input').each(function (index, element) {
            element.value = element.value || household[element.name];
            household[element.name] = element.value;
        });

        setHouseholdFields();
    }

    function mergeIncome(replay) {
        (0, _jquery2.default)('.js-income input').each(function (index, element) {
            element.value = element.value || income[element.name];
            income[element.name] = element.value;
        });
        showChildField();

        if (replay) {
            setIncomeFields();
        }
    }

    function mergeOutgoings() {
        if ((0, _jquery2.default)('.js-outgoings').length) {
            (0, _jquery2.default)('.js-outgoings input').each(function (index, element) {
                element.value = element.value || outgoings[element.name];
                outgoings[element.name] = element.value;
            });
            if ((0, _jquery2.default)('.js-outgoings input[name="no-replace"]').length) {
                staticOutgoings = Object.assign(staticOutgoings, outgoings);
            }
        }
    }

    function setupIncomeInfoCard() {
        (0, _jquery2.default)('.js-budget-income-info-box').find('.js-budget-total').text('£' + (0, _helpers.parseFloatWithCommas)(income.total));
        (0, _jquery2.default)('.js-budget-income-info-box').find('.js-budget-earnings').text('£' + (0, _helpers.parseFloatWithCommas)(income.earnings));
        (0, _jquery2.default)('.js-budget-income-info-box').find('.js-budget-benefits').text('£' + (0, _helpers.parseFloatWithCommas)(income.benefits));
        (0, _jquery2.default)('.js-budget-income-info-box').find('.js-budget-pensions').text('£' + (0, _helpers.parseFloatWithCommas)(income.pensions));
        (0, _jquery2.default)('.js-budget-income-info-box').find('.js-budget-other').text('£' + (0, _helpers.parseFloatWithCommas)(income.other));
    }

    function updateOutgoingInfoCard() {
        (0, _jquery2.default)('.js-budget-outgoings-info-box').find('.js-budget-bills').text('£' + (0, _helpers.parseFloatWithCommas)(outgoings.household));
        (0, _jquery2.default)('.js-budget-outgoings-info-box').find('.js-budget-expenditure').text('£' + (0, _helpers.parseFloatWithCommas)(outgoings.expenditure));
        (0, _jquery2.default)('.js-budget-outgoings-info-box').find('.js-budget-other').text('£' + (0, _helpers.parseFloatWithCommas)(outgoings.other));
        (0, _jquery2.default)('.js-budget-outgoings-info-box').find('.js-budget-total').text('£' + (0, _helpers.parseFloatWithCommas)(getTotal()));
    }

    function calcMinMax(input, minValue, maxValue, frequency) {
        var _getDurationConstants = (0, _helpers.getDurationConstants)(),
            week = _getDurationConstants.week,
            fortnight = _getDurationConstants.fortnight,
            every4week = _getDurationConstants.every4week;

        var $inputVal = (0, _helpers.getFloatValue)((0, _jquery2.default)(input).val()),
            $formElement = (0, _jquery2.default)(input).closest('.form__element--2row');

        switch (frequency) {
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
        if ($inputVal != 0 && $inputVal < minValue) {
            $formElement.find('.js-warning-value').text('low');
            $formElement.addClass('form__element--2row--warning').find('.js-warning').slideDown();
        } else if ($inputVal > maxValue) {
            $formElement.find('.js-warning-value').text('high');
            $formElement.addClass('form__element--2row--warning').find('.js-warning').slideDown();
        } else {
            $formElement.removeClass('form__element--2row--warning').find('.js-warning').slideUp();
        }
    }

    // Replay Fields
    function setHouseholdFields() {
        (0, _jquery2.default)('.js-budget-calc #circumstances').val(household.circumstances).trigger('change');
        (0, _jquery2.default)('.js-budget-calc #adults').val(household.adults).trigger('change');
        (0, _jquery2.default)('.js-budget-calc #children-0-16').val(household.children_0_16).trigger('change');
        (0, _jquery2.default)('.js-budget-calc #children-17-18').val(household.children_17_18).trigger('change');
        (0, _jquery2.default)('.js-budget-calc #housing').val(household.housing).trigger('change');
    }

    function setIncomeFields() {
        (0, _jquery2.default)('.js-budget-calc #earnings').val(income.earnings).trigger('change');
        (0, _jquery2.default)('.js-budget-calc #benefits').val(income.benefits).trigger('change');
        (0, _jquery2.default)('.js-budget-calc #pensions').val(income.pensions).trigger('change');
        (0, _jquery2.default)('.js-budget-calc #other').val(income.other).trigger('change');

        if (income.earnings_term != '') {
            (0, _jquery2.default)('.js-budget-calc #earnings-select').val(income.earnings_term).trigger('change');
        }

        if (income.benefits_term != '') {
            (0, _jquery2.default)('.js-budget-calc #benefits-select').val(income.benefits_term).trigger('change');
        }

        if (income.pensions_term != '') {
            (0, _jquery2.default)('.js-budget-calc #pensions-select').val(income.pensions_term).trigger('change');
        }

        if (income.other_term != '') {
            (0, _jquery2.default)('.js-budget-calc #other-select').val(income.other_term).trigger('change');
        }
    }

    function calculateAllFields() {
        (0, _jquery2.default)('.js-budget-calc .js-payment-input').each(function (index, element) {
            var $this = (0, _jquery2.default)(element),
                category = $this.data('category');

            switch (category) {
                case 'household':
                    outgoings.household = Object.keys(staticOutgoings).length ? parseFloat(staticOutgoings.household) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
                    break;
                case 'expenditure':
                    outgoings.expenditure = Object.keys(staticOutgoings).length ? parseFloat(staticOutgoings.expenditure) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
                    break;
                default:
                    outgoings.other = Object.keys(staticOutgoings).length ? parseFloat(staticOutgoings.other) + _calculateCategoryTotal(category) : _calculateCategoryTotal(category);
            }

            $this.trigger('blur');
        });

        updateOutgoingInfoCard();
    }
    // END Replay Fields
};

var _jquery = __webpack_require__(11);

var _jquery2 = _interopRequireDefault(_jquery);

var _cleave = __webpack_require__(488);

var _cleave2 = _interopRequireDefault(_cleave);

var _helpers = __webpack_require__(97);

function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : { default: obj };
}

;

// Private functions
function _calculateCategoryTotal(category) {
    var inputs = void 0,
        total = 0;

    switch (category) {
        case 'household':
            inputs = (0, _jquery2.default)('.js-budget-calc .js-payment-input[data-category="household"]');
            inputs.each(function (index, element) {
                var frequency = (0, _jquery2.default)(element).closest('.js-field-parent').find('.js-payment-frequency').val();

                if (!isNaN(parseFloat((0, _jquery2.default)(element).val()))) {
                    total += _calcFromMonthly((0, _helpers.getFloatValue)((0, _jquery2.default)(element).val()), frequency);
                }
            });

            return total;
        case 'expenditure':
            inputs = (0, _jquery2.default)('.js-budget-calc .js-payment-input[data-category="expenditure"]');
            inputs.each(function (index, element) {
                var frequency = (0, _jquery2.default)(element).closest('.js-field-parent').find('.js-payment-frequency').val();

                if (!isNaN(parseFloat((0, _jquery2.default)(element).val()))) {
                    total += _calcFromMonthly((0, _helpers.getFloatValue)((0, _jquery2.default)(element).val()), frequency);
                }
            });

            return total;
        default:
            inputs = (0, _jquery2.default)('.js-budget-calc .js-payment-input[data-category="other"]');
            inputs.each(function (index, element) {
                var frequency = (0, _jquery2.default)(element).closest('.js-field-parent').find('.js-payment-frequency').val();

                if (!isNaN(parseFloat((0, _jquery2.default)(element).val()))) {
                    total += _calcFromMonthly((0, _helpers.getFloatValue)((0, _jquery2.default)(element).val()), frequency);
                }
            });
            return total;
    }
}

function _calcFromMonthly(number, frequency) {
    var _getDurationConstants2 = (0, _helpers.getDurationConstants)(),
        week = _getDurationConstants2.week,
        fortnight = _getDurationConstants2.fortnight,
        every4week = _getDurationConstants2.every4week;

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

/***/ })

}]);
//# sourceMappingURL=4.bundle.js.map