(window["webpackJsonp"] = window["webpackJsonp"] || []).push([[5],{

/***/ 485:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _jquery = __webpack_require__(11);

var _jquery2 = _interopRequireDefault(_jquery);

var _moment = __webpack_require__(0);

var _moment2 = _interopRequireDefault(_moment);

var _cleave = __webpack_require__(488);

var _cleave2 = _interopRequireDefault(_cleave);

var _pikaday = __webpack_require__(98);

var _pikaday2 = _interopRequireDefault(_pikaday);

__webpack_require__(263);

var _helpers = __webpack_require__(97);

function _interopRequireDefault(obj) {
    return obj && obj.__esModule ? obj : { default: obj };
}

exports.default = function () {
    if ((0, _jquery2.default)('.js-payment-calc').length > 0) {
        var ResetDDOptions = function ResetDDOptions() {
            (0, _jquery2.default)('#otherPaymentOption').removeClass('d-none');

            if (paymentSettings.iAndELessThanOrIs12MonthsOld && paymentSettings.monthlyDisposableIncomePerAccount > 0) {
                (0, _jquery2.default)('#disposableIncomeOption').removeClass('d-none');
                (0, _jquery2.default)('#averagePaymentOption').addClass('d-none');
            }

            if (paymentSettings.iAndELessThanOrIs12MonthsOld && paymentSettings.monthlyDisposableIncomePerAccount <= 0) {
                (0, _jquery2.default)('#disposableIncomeOption').addClass('d-none');
                (0, _jquery2.default)('#averagePaymentOption').addClass('d-none');
            }

            if (!paymentSettings.iAndELessThanOrIs12MonthsOld) {
                (0, _jquery2.default)('#disposableIncomeOption').addClass('d-none');
                (0, _jquery2.default)('#averagePaymentOption').removeClass('d-none');
            }

            (0, _jquery2.default)('#DisposableIncome').prop('checked', false);
            (0, _jquery2.default)('#OtherPaymentOffer').prop('checked', false);
            (0, _jquery2.default)('#AverageSetupValue').prop('checked', false);

            (0, _jquery2.default)('input[name=DirectDebitAmount]').val('');

            updateNumberPayment(0);
            RefreshDDTermInfoBox();
        };

        var DisposableIncomeSelected = function DisposableIncomeSelected() {
            (0, _jquery2.default)('#otherPaymentOption').addClass('d-none');
            (0, _jquery2.default)('#averagePaymentOption').addClass('d-none');
            (0, _jquery2.default)('input[name=DirectDebitAmount]').val((0, _jquery2.default)('#disposableIncome').val());
            updateNumberPayment((0, _jquery2.default)('#disposableIncome').val());
            RefreshDDTermInfoBox();
        };

        var AverageSetupValueSelected = function AverageSetupValueSelected() {
            (0, _jquery2.default)('#otherPaymentOption').addClass('d-none');
            (0, _jquery2.default)('#disposableIncomeOption').addClass('d-none');
            (0, _jquery2.default)('input[name=DirectDebitAmount]').val((0, _jquery2.default)('#averagePayment').val());
            updateNumberPayment((0, _jquery2.default)('#averagePayment').val());
            RefreshDDTermInfoBox();
        };

        var OtherPaymentOfferSelected = function OtherPaymentOfferSelected() {
            (0, _jquery2.default)('#disposableIncomeOption').addClass('d-none');
            (0, _jquery2.default)('#averagePaymentOption').addClass('d-none');
            RefreshDDTermInfoBox();
        };

        // END jQuery Listeners

        // Public Functions
        var FrequencyChanged = function FrequencyChanged() {
            var paymentFrequency = _getPaymentLength();

            if (paymentFrequency == 'monthly') {
                updateDatePicker(true);
            } else {
                updateDatePicker();
            }

            var _getDurationConstants = (0, _helpers.getDurationConstants)(),
                week = _getDurationConstants.week,
                fortnight = _getDurationConstants.fortnight,
                every4week = _getDurationConstants.every4week;

            var disposableIncome = (0, _jquery2.default)('#disposableIncome');
            var averagePayment = (0, _jquery2.default)('#averagePayment');

            switch (paymentFrequency) {
                case 'weekly':
                    disposableIncome.val((0, _helpers.parseFloatWithCommas)(paymentSettings.monthlyDisposableIncomePerAccount / week));
                    averagePayment.val((0, _helpers.parseFloatWithCommas)(paymentSettings.averageMonthlyPayment / week));
                    break;
                case 'fortnightly':
                    disposableIncome.val((0, _helpers.parseFloatWithCommas)(paymentSettings.monthlyDisposableIncomePerAccount / fortnight));
                    averagePayment.val((0, _helpers.parseFloatWithCommas)(paymentSettings.averageMonthlyPayment / fortnight));
                    break;
                case '4week':
                    disposableIncome.val((0, _helpers.parseFloatWithCommas)(paymentSettings.monthlyDisposableIncomePerAccount / every4week));
                    averagePayment.val((0, _helpers.parseFloatWithCommas)(paymentSettings.averageMonthlyPayment / every4week));
                    break;
                case 'every 4 weeks':
                    disposableIncome.val((0, _helpers.parseFloatWithCommas)(paymentSettings.monthlyDisposableIncomePerAccount / every4week));
                    averagePayment.val((0, _helpers.parseFloatWithCommas)(paymentSettings.averageMonthlyPayment / every4week));
                    break;
                case 'monthly':
                    disposableIncome.val((0, _helpers.parseFloatWithCommas)(paymentSettings.monthlyDisposableIncomePerAccount));
                    averagePayment.val((0, _helpers.parseFloatWithCommas)(paymentSettings.averageMonthlyPayment));
                    break;
            }

            var ddAmount = _getDDPaymentAmount();

            if ((0, _jquery2.default)('#DisposableIncome').prop('checked')) {
                ddAmount = disposableIncome.val();
            }

            if ((0, _jquery2.default)('#AverageSetupValue').prop('checked')) {
                ddAmount = averagePayment.val();
            }

            updateNumberPayment(ddAmount);
            updateDirectDebtInfo(ddAmount);

            RefreshDDTermInfoBox();
        };

        var mergeAllPaymentSettings = function mergeAllPaymentSettings() {
            (0, _jquery2.default)('.js-payment-calc .js-general-payment-settings input').each(function (index, element) {
                element.value = element.value || paymentSettings[element.name];
                paymentSettings[element.name] = element.value;
            });

            (0, _jquery2.default)('.payment-active').find('.js-payment-settings-type input').each(function (index, element) {
                element.value = element.value || paymentSettings[element.name];
                paymentSettings[element.name] = element.value;
            });

            //Convert string to bool
            paymentSettings.iAndELessThanOrIs12MonthsOld = JSON.parse(paymentSettings.iAndELessThanOrIs12MonthsOld);
        };

        var setupInfoCard = function setupInfoCard() {
            if ((0, _jquery2.default)('.payment-active').data('form-fields') == 'direct-debit') {
                (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-info').show();
            } else {
                (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-info').hide();
            }

            if (discountActive) {
                (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-balance').text('£' + (0, _helpers.parseFloatWithCommas)(paymentSettings.discountBalance));
                (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-balance-name').text('Discounted balance');
                // Fix for - TFS 86359
                (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-amount').text('£' + (0, _helpers.parseFloatWithCommas)(paymentSettings.discountBalance));
            } else {
                (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-balance').text('£' + (0, _helpers.parseFloatWithCommas)(paymentSettings.balance));
                (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-balance-name').text('Balance');
                (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-amount').text('£' + (0, _helpers.parseFloatWithCommas)(paymentSettings.paymentAmount));
            }

            (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-account').text(paymentSettings.account);
            (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-name').text(paymentSettings.paymentName);
            (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-term').text(paymentSettings.term);
            (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-date').text((0, _moment2.default)(paymentSettings.paymentDate, 'DD-MM-YYYY').format('Do MMMM YYYY'));
        };

        var updateFullPayment = function updateFullPayment() {
            (0, _jquery2.default)('.js-payment-amount').val(discountActive ? (0, _helpers.parseFloatWithCommas)(paymentSettings.discountBalance) : (0, _helpers.parseFloatWithCommas)(paymentSettings.balance));
        };

        var setFullPaymentHiddenInput = function setFullPaymentHiddenInput() {
            (0, _jquery2.default)('input[type=hidden].js-payment-amount').val((0, _helpers.parseFloatWithCommas)(paymentSettings.paymentAmount));
        };

        var updateDirectDebtInfo = function updateDirectDebtInfo(amount) {
            var termLength = _calcLengthOfDD(getBalanceAmount(), amount, _getPaymentLength());
            $activePaymentSettings.find('input[name=term]').val(termLength);
            (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-term').text(termLength);
            (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-name').text((0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val());
        };

        var changeStartDate = function changeStartDate(dateSelected) {
            (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentDate]').val(dateSelected);
            (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-date').text((0, _moment2.default)(dateSelected, 'DD-MM-YYYY').format('Do MMMM YYYY'));
        };

        var updateActiveSettings = function updateActiveSettings() {
            $activePaymentSettings = (0, _jquery2.default)('.payment-active .js-payment-settings-type');

            (0, _jquery2.default)('.payment-active').find('.js-payment-settings-type input').each(function (index, element) {
                element.value = element.value || paymentSettings[element.name];
                paymentSettings[element.name] = element.value;
            });
        };

        var setNumberalFields = function setNumberalFields() {
            (0, _jquery2.default)('.js-payment-calc .js-payment-input').each(function (index, element) {
                numberalFields.push(new _cleave2.default(element, numberalFieldOptions));
            });
        };

        var updateNumberPayment = function updateNumberPayment(value) {
            var formattedValue = (0, _helpers.parseFloatWithCommas)(value);

            $activePaymentSettings.find('input[name=paymentAmount]').val(formattedValue);
            (0, _jquery2.default)('.js-payment-info-box').find('.js-payment-amount').text('£' + formattedValue);

            if ((0, _jquery2.default)('.payment-active').data('form-fields') == 'direct-debit') {
                updateDirectDebtInfo(formattedValue);
            }
        };

        var calculateDDTerm = function calculateDDTerm() {
            (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=term]').val(_calcLengthOfDD(getBalanceAmount(), _getDDPaymentAmount(), _getPaymentLength()));
        };

        var setDDField = function setDDField() {
            if ((0, _helpers.getFloatValue)((0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentAmount]').val()) > (0, _helpers.getFloatValue)(getBalanceAmount())) {
                (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentAmount]').val('1.00');
                (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-input').val('1.00');
            }
        };

        var getBalanceAmount = function getBalanceAmount() {
            return discountActive ? paymentSettings.discountBalance : paymentSettings.balance;
        };

        var updateDatePicker = function updateDatePicker() {
            var monthly = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : false;

            if (window.picker != null) {
                window.picker.destroy();
            }

            var picker = new _pikaday2.default({
                field: (0, _jquery2.default)('.js-datepicker-payment')[0],
                firstDay: 1,
                format: 'DD-MM-YYYY',
                minDate: (0, _moment2.default)(paymentSettings.minDateRange, 'DD-MM-YYYY').toDate(),
                maxDate: (0, _moment2.default)(paymentSettings.maxDateRange, 'DD-MM-YYYY').toDate(),
                disableWeekends: false, // Fix for Bug 86362
                showDaysInNextAndPreviousMonths: true,
                keyboardInput: false
            });

            if (paymentSettings.paymentDate != '' && monthly) {
                picker.setDate((0, _moment2.default)(paymentSettings.paymentDate, 'DD-MM-YYYY'));
            } else {
                picker.setDate((0, _moment2.default)(paymentSettings.minDateRange, 'DD-MM-YYYY').toDate());
            }

            window.picker = picker;
        };

        var updatePaymentInfo = function updatePaymentInfo(legnth, date) {

            var weekendClause = '';
            var dayOfWeek = (0, _moment2.default)(date, 'DD-MM-YYYY').day();

            if (dayOfWeek == 6 || dayOfWeek == 0) {
                weekendClause = ' You have selected a weekend, however, your payment will be taken on the next working day.';
            }

            switch (legnth) {
                case 'monthly':
                    (0, _jquery2.default)('.js-payment-info').html('Payments will be taken on the <span>' + (0, _moment2.default)(date, 'DD-MM-YYYY').format('Do') + ' of each month</span>, in cases that this falls on a bank holiday or a weekend, the payment will be taken on the next working day.');
                    break;
                case 'weekly':
                    (0, _jquery2.default)('.js-payment-info').html('Payments will be taken on the <span>' + _getWeekDay(dayOfWeek) + '</span> ' + _getPaymentLength() + '.' + weekendClause);
                    break;
                case 'fortnightly':
                    (0, _jquery2.default)('.js-payment-info').html('Payments will be taken each <span>' + _getWeekDay(dayOfWeek) + '</span>, on a fortnightly basis.' + weekendClause);
                    break;
                case '4week':
                case 'every 4 weeks':
                    (0, _jquery2.default)('.js-payment-info').html('Your first payment will be on the <span>' + (0, _moment2.default)(date, 'DD-MM-YYYY').format('Do of MMM') + '</span> and then every <span>4th ' + _getWeekDay(dayOfWeek) + '</span> afterwards.' + weekendClause);
                    break;
            }
        };

        var RefreshDDTermInfoBox = function RefreshDDTermInfoBox() {
            var ddPaymentAmount = (0, _helpers.getFloatValue)(_getDDPaymentAmount());
            var ddPaymentFrequency = _getPaymentLength();
            var balance = (0, _helpers.getFloatValue)(getBalanceAmount());

            if (ddPaymentAmount && ddPaymentFrequency && balance) {

                //Show DD Term InfoBox
                var ddTermTotalMonths = _calcDDTotalMonths(balance, ddPaymentAmount, ddPaymentFrequency);
                var ddTermMonths = ddTermTotalMonths % 12;
                var ddTermYears = Math.floor(ddTermTotalMonths / 12);

                if (ddTermMonths <= 0 && ddTermYears <= 0) {
                    ddTermMonths = ddTermMonths + 1;
                }

                (0, _jquery2.default)('#dd-term-years').text(ddTermYears);
                (0, _jquery2.default)('#dd-term-months').text(ddTermMonths);
                (0, _jquery2.default)('#dd-term-message').removeClass('d-none');

                if (paymentSettings.iAndELessThanOrIs12MonthsOld && paymentSettings.monthlyDisposableIncomePerAccount <= 0) {
                    (0, _jquery2.default)('#dd-term-debt-advice').removeClass('d-none');
                } else {
                    (0, _jquery2.default)('#dd-term-debt-advice').addClass('d-none');
                }
            } else {
                //Hide DD Term InfoBox
                (0, _jquery2.default)('#dd-term-message').addClass('d-none');
            }
        };

        var ShowHideNextButton = function ShowHideNextButton() {
            if ((0, _jquery2.default)('.js-payment-select').val() === 'direct-debit') {
                (0, _jquery2.default)('#btnNext').addClass('d-none');
            } else {
                (0, _jquery2.default)('#btnNext').removeClass('d-none');
            }
        };

        // END Public Functions


        (0, _jquery2.default)('.js-payment-select').on('change', function (e) {

            var $this = (0, _jquery2.default)(e.currentTarget);
            var $closestFormValidator = $this.closest('.form').first().validate();
            $closestFormValidator.element('select.js-payment-select');

            paymentSettings.paymentType = $this.val();
            ShowHideNextButton();
        });

        (0, _jquery2.default)('#btnPlanReset').on('click', function (e) {
            ResetDDOptions();
        });

        (0, _jquery2.default)('.js-source-of-funds-select').on('change', function (e) {
            var $this = (0, _jquery2.default)(e.currentTarget);
            var $closestFormValidator = $this.closest('.form').first().validate();
            $closestFormValidator.element('select[id=PartialPaymentSelectedSourceOfFunds]');
            $closestFormValidator.element('select[id=FullPaymentSelectedSourceOfFunds]');
        });

        (0, _jquery2.default)(':radio[id=DisposableIncome]').change(function () {
            DisposableIncomeSelected();
            (0, _jquery2.default)('#PlanSetupOptionValMsg').addClass('d-none'); //Remove any validation errors for SetupPlanOption
        });

        (0, _jquery2.default)(':radio[id=AverageSetupValue]').change(function () {
            AverageSetupValueSelected();
            (0, _jquery2.default)('#PlanSetupOptionValMsg').addClass('d-none'); //Remove any validation errors for SetupPlanOption
        });

        (0, _jquery2.default)(':radio[id=OtherPaymentOffer]').change(function () {
            OtherPaymentOfferSelected();
            (0, _jquery2.default)('#PlanSetupOptionValMsg').addClass('d-none'); //Remove any validation errors for SetupPlanOption
        });

        var paymentSettings = {
            account: 'Account Name',
            balance: '',
            discountBalance: '',
            paymentName: '',
            paymentAmount: '',
            term: '',
            paymentDate: '',
            minDateRange: (0, _moment2.default)().format('DD-MM-YYYY'),
            maxDateRange: (0, _moment2.default)().add(6, 'months').format('DD-MM-YYYY'),
            iAndELessThanOrIs12MonthsOld: '',
            paymentType: '',
            monthlyDisposableIncomePerAccount: '',
            averageMonthlyPayment: '',
            directDebitAmount: ''
        },
            $activePaymentSettings = (0, _jquery2.default)('.payment-active .js-payment-settings-type'),
            discountActive = false,
            numberalFields = [];

        if ((0, _jquery2.default)('.discount-already-selected').length > 0) {
            discountActive = true;
            paymentSettings.discountBalance = (0, _jquery2.default)('.discount-already-selected').text();

            (0, _jquery2.default)('.discount-already-selected').hide();
        }

        // Setup
        mergeAllPaymentSettings();
        setupInfoCard();
        setFullPaymentHiddenInput();
        updateDatePicker(true);
        calculateDDTerm();
        updatePaymentInfo(_getPaymentLength(), paymentSettings.paymentDate);

        var numberalFieldOptions = {
            numeral: true,
            onValueChanged: function onValueChanged(e) {
                // Update user payment amount
                if (e.target.value == "" || e.target.value <= 0 || e.target.value == ".") {
                    (0, _jquery2.default)(this.element).addClass('input-validation-error');
                    (0, _jquery2.default)(this.element).siblings('.js-pyament-input-message').text(paymentSettings.fieldEmpty);
                    updateNumberPayment('0.00');
                } else if ((0, _helpers.getFloatValue)(e.target.value) > (0, _helpers.getFloatValue)(getBalanceAmount())) {
                    (0, _jquery2.default)(this.element).addClass('input-validation-error');
                    (0, _jquery2.default)(this.element).siblings('.js-pyament-input-message').text(paymentSettings.fieldExceeded);
                    updateNumberPayment(getBalanceAmount());
                } else {
                    (0, _jquery2.default)(this.element).removeClass('input-validation-error');
                    (0, _jquery2.default)(this.element).siblings('.js-pyament-input-message').text('');
                    updateNumberPayment(e.target.value);
                }
            }
        };

        setNumberalFields();
        FrequencyChanged();
        ResetDDOptions();
        ShowHideNextButton();

        // END Setup

        // jQuery Listeners
        // Change type of payment dropdown
        (0, _jquery2.default)('.js-payment-calc .js-payment-select').on('change', function (e) {
            updateActiveSettings();
            setupInfoCard();
            updateDatePicker(true);
        });

        // Update user payment amount
        (0, _jquery2.default)('.js-payment-calc').on('blur', '.payment-active .js-payment-input', function (e) {
            if ((0, _jquery2.default)(e.currentTarget).val() != '') {
                (0, _jquery2.default)(e.currentTarget).val((0, _helpers.parseFloatWithCommas)((0, _jquery2.default)(e.currentTarget).val()));
            }
        });

        (0, _jquery2.default)('.js-payment-calc').on('change', '.payment-active .js-payment-input', function (e) {
            RefreshDDTermInfoBox();
        });

        // Change the date for Direct Debt
        (0, _jquery2.default)('.js-payment-calc').on('change', '.payment-active .js-datepicker-payment', function (e) {
            var dateSelected = (0, _jquery2.default)(e.currentTarget).val();
            updatePaymentInfo(_getPaymentLength(), dateSelected);
            changeStartDate(dateSelected);
        });

        // Update info based on payment length (monthly, weekly etc.)
        (0, _jquery2.default)('.js-payment-calc .js-payment-length').on('change', function () {
            FrequencyChanged();
        });

        (0, _jquery2.default)('.js-apply-dicount').on('click', function () {
            discountActive = !discountActive;
            (0, _jquery2.default)('.js-discount-info').stop(false, false).slideToggle();
            (0, _jquery2.default)('#discount-apply-confirm').attr('disabled', function (i, v) {
                return !v;
            });

            var $this = (0, _jquery2.default)('.js-hide-option').parent();
            (0, _jquery2.default)('.js-hide-option').attr('disabled', function (i, v) {
                return !v;
            });
            $this.select2('destroy');

            $this.select2({
                theme: 'lowell',
                width: '100%',
                minimumResultsForSearch: -1,
                selectOnClose: true
            });

            $this.val('').trigger('change');

            ResetDDOptions();

            updateFullPayment();
            setupInfoCard(); // The on change event on the select will take care of this. refer to line 73
            calculateDDTerm();
            setDDField();
            RefreshDDTermInfoBox();
        });

        if ((0, _jquery2.default)('.js-apply-dicount').is(':checked')) {
            discountActive = !discountActive;
            (0, _jquery2.default)('.js-discount-info').slideToggle();
            (0, _jquery2.default)('#discount-apply-confirm').attr('disabled', function (i, v) {
                return !v;
            });

            var $this = (0, _jquery2.default)('.js-hide-option').parent();
            (0, _jquery2.default)('.js-hide-option').attr('disabled', function (i, v) {
                return !v;
            });
            $this.select2('destroy');

            $this.select2({
                theme: 'lowell',
                width: '100%',
                minimumResultsForSearch: -1,
                selectOnClose: true
            });

            (0, _jquery2.default)('.js-payment-amount').each(function (index, element) {
                element.value = 0;
            });

            updateFullPayment();
            setupInfoCard();
            calculateDDTerm();
            RefreshDDTermInfoBox();
        }
    }
};

// Private Functions


function _calcDDTotalMonths(balance, ddAmount, ddFrequency) {
    var _getDurationConstants2 = (0, _helpers.getDurationConstants)(),
        week = _getDurationConstants2.week,
        fortnight = _getDurationConstants2.fortnight,
        every4week = _getDurationConstants2.every4week;

    switch (ddFrequency) {
        case 'monthly':
            return Math.ceil(balance / ddAmount);
        case 'weekly':
            return Math.ceil(balance / ddAmount / week);
        case 'fortnightly':
            return Math.ceil(balance / ddAmount / fortnight);
        case '4week':
        case 'every 4 weeks':
            return Math.ceil(balance / ddAmount / every4week);
    }
}

function _calcLengthOfDD(balance, amount, length) {
    var _getDurationConstants3 = (0, _helpers.getDurationConstants)(),
        week = _getDurationConstants3.week,
        fortnight = _getDurationConstants3.fortnight,
        every4week = _getDurationConstants3.every4week;

    var numberBalance = (0, _helpers.getFloatValue)(balance);
    var numberAmount = (0, _helpers.getFloatValue)(amount);

    var term = void 0;
    switch (length) {
        case 'monthly':
            term = Math.ceil(numberBalance / numberAmount);
            (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val('Monthly payment');
            break;
        case 'weekly':
            term = Math.ceil(numberBalance / numberAmount / week);
            (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val('Weekly payment');
            break;
        case 'fortnightly':
            term = Math.ceil(numberBalance / numberAmount / fortnight);
            (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val('Fortnightly payment');
            break;
        case '4week':
        case 'every 4 weeks':
            term = Math.ceil(numberBalance / numberAmount / every4week);
            (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val('Four Weekly payment');
            break;
    }

    if (!isFinite(term)) {
        return 'None';
    }

    if (term == 0) {
        term = term + 1;
    }

    return term + ' months';
}

function _getWeekDay(dayNumber) {
    switch (dayNumber) {
        case 1:
            return 'Monday';
        case 2:
            return 'Tuesday';
        case 3:
            return 'Wednesday';
        case 4:
            return 'Thursday';
        case 5:
            return 'Friday';
        case 6:
            return 'Saturday';
        case 0:
            return 'Sunday';
        default:
            return '';
    }
}

function _getPaymentLength() {
    if ((0, _jquery2.default)('.js-payment-calc .js-payment-length').val() == '4week') {
        return 'every 4 weeks';
    }
    return (0, _jquery2.default)('.js-payment-calc .js-payment-length').val();
}

function _getDDPaymentAmount() {
    return (0, _jquery2.default)('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentAmount]').val();
}

// END Private Functions

/***/ })

}]);
//# sourceMappingURL=5.bundle.js.map