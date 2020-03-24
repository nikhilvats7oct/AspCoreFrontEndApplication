import $ from 'jquery';
import moment from 'moment';
import cleave from 'cleave.js';
import Pikaday from 'pikaday';
import 'select2';
import { getFloatValue, parseFloatWithCommas, getDurationConstants } from '../_helpers';

export default () => {
    if ($('.js-payment-calc').length > 0) {

        $('.js-payment-select').on('change', (e) => {

            var $this = $(e.currentTarget);
            var $closestFormValidator = $this.closest('.form').first().validate();
            $closestFormValidator.element('select.js-payment-select');

            paymentSettings.paymentType = $this.val();
            ShowHideNextButton();
        });

        function ResetDDOptions() {
            $('#otherPaymentOption').removeClass('d-none');

            if (paymentSettings.iAndELessThanOrIs12MonthsOld && paymentSettings.monthlyDisposableIncomePerAccount > 0) {
                $('#disposableIncomeOption').removeClass('d-none');
                $('#averagePaymentOption').addClass('d-none');
            }

            if (paymentSettings.iAndELessThanOrIs12MonthsOld && paymentSettings.monthlyDisposableIncomePerAccount <= 0) {
                $('#disposableIncomeOption').addClass('d-none');
                $('#averagePaymentOption').addClass('d-none');
            }

            if (!paymentSettings.iAndELessThanOrIs12MonthsOld) {
                $('#disposableIncomeOption').addClass('d-none');
                $('#averagePaymentOption').removeClass('d-none');
            }

            $('#DisposableIncome').prop('checked', false);
            $('#OtherPaymentOffer').prop('checked', false);
            $('#AverageSetupValue').prop('checked', false);

            $('input[name=DirectDebitAmount]').val('');

            updateNumberPayment(0);
            RefreshDDTermInfoBox();
        }

        $('#btnPlanReset').on('click', (e) => {
            ResetDDOptions();
        });

        $('.js-source-of-funds-select').on('change', (e) => {
            var $this = $(e.currentTarget);
            var $closestFormValidator = $this.closest('.form').first().validate();
            $closestFormValidator.element('select[id=PartialPaymentSelectedSourceOfFunds]');
            $closestFormValidator.element('select[id=FullPaymentSelectedSourceOfFunds]');
        });

        $(':radio[id=DisposableIncome]').change(function () {
            DisposableIncomeSelected();
            $('#PlanSetupOptionValMsg').addClass('d-none'); //Remove any validation errors for SetupPlanOption
        });
        
        $(':radio[id=AverageSetupValue]').change(function () {
            AverageSetupValueSelected();
            $('#PlanSetupOptionValMsg').addClass('d-none'); //Remove any validation errors for SetupPlanOption
        });
        
        $(':radio[id=OtherPaymentOffer]').change(function () {
            OtherPaymentOfferSelected();
            $('#PlanSetupOptionValMsg').addClass('d-none'); //Remove any validation errors for SetupPlanOption
        });

        function DisposableIncomeSelected() {
            $('#otherPaymentOption').addClass('d-none');
            $('#averagePaymentOption').addClass('d-none');
            $('input[name=DirectDebitAmount]').val($('#disposableIncome').val());
            updateNumberPayment($('#disposableIncome').val());
            RefreshDDTermInfoBox();           
        }

        function AverageSetupValueSelected() {
            $('#otherPaymentOption').addClass('d-none');
            $('#disposableIncomeOption').addClass('d-none');
            $('input[name=DirectDebitAmount]').val($('#averagePayment').val());
            updateNumberPayment($('#averagePayment').val());
            RefreshDDTermInfoBox();
        }

        function OtherPaymentOfferSelected() {
            $('#disposableIncomeOption').addClass('d-none');
            $('#averagePaymentOption').addClass('d-none');
            RefreshDDTermInfoBox();
        }

        let paymentSettings = {
            account: 'Account Name',
            balance: '',
            discountBalance: '',
            paymentName: '',
            paymentAmount: '',
            term: '',
            paymentDate: '',
            minDateRange: moment().format('DD-MM-YYYY'),
            maxDateRange: moment().add(6, 'months').format('DD-MM-YYYY'),
            iAndELessThanOrIs12MonthsOld: '',
            paymentType: '',
            monthlyDisposableIncomePerAccount: '',
            averageMonthlyPayment: '',
            directDebitAmount: ''
        },
            $activePaymentSettings = $('.payment-active .js-payment-settings-type'),
            discountActive = false,
            numberalFields = [];

        if ($('.discount-already-selected').length > 0) {
            discountActive = true;
            paymentSettings.discountBalance = $('.discount-already-selected').text();

            $('.discount-already-selected').hide();
        }

        // Setup
        mergeAllPaymentSettings();
        setupInfoCard();
        setFullPaymentHiddenInput();
        updateDatePicker(true);
        calculateDDTerm();
        updatePaymentInfo(_getPaymentLength(), paymentSettings.paymentDate);

        let numberalFieldOptions = {
            numeral: true,
            onValueChanged: function (e) {
                // Update user payment amount
                if (e.target.value == "" || e.target.value <= 0 || e.target.value == ".") {
                    $(this.element).addClass('input-validation-error');
                    $(this.element).siblings('.js-pyament-input-message').text(paymentSettings.fieldEmpty);
                    updateNumberPayment('0.00');
                }
                else if (getFloatValue(e.target.value) > getFloatValue(getBalanceAmount())) {
                    $(this.element).addClass('input-validation-error');
                    $(this.element).siblings('.js-pyament-input-message').text(paymentSettings.fieldExceeded);
                    updateNumberPayment(getBalanceAmount());
                }
                else {
                    $(this.element).removeClass('input-validation-error');
                    $(this.element).siblings('.js-pyament-input-message').text('');
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
        $('.js-payment-calc .js-payment-select').on('change', (e) => {
            updateActiveSettings();
            setupInfoCard();
            updateDatePicker(true);
        });

        // Update user payment amount
        $('.js-payment-calc').on('blur', '.payment-active .js-payment-input', (e) => {
            if ($(e.currentTarget).val() != '') {
                $(e.currentTarget).val(parseFloatWithCommas($(e.currentTarget).val()));
            }
        });

        $('.js-payment-calc').on('change', '.payment-active .js-payment-input', (e) => {
            RefreshDDTermInfoBox();
        });

        // Change the date for Direct Debt
        $('.js-payment-calc').on('change', '.payment-active .js-datepicker-payment', (e) => {
            let dateSelected = $(e.currentTarget).val();
            updatePaymentInfo(_getPaymentLength(), dateSelected);
            changeStartDate(dateSelected);
        });

        // Update info based on payment length (monthly, weekly etc.)
        $('.js-payment-calc .js-payment-length').on('change', () => {
            FrequencyChanged();
        });

        $('.js-apply-dicount').on('click', () => {
            discountActive = !discountActive;
            $('.js-discount-info').stop(false, false).slideToggle();
            $('#discount-apply-confirm').attr('disabled', function (i, v) { return !v; });

            let $this = $('.js-hide-option').parent();
            $('.js-hide-option').attr('disabled', function (i, v) { return !v; });
            $this.select2('destroy');

            $this.select2({
                theme: 'lowell',
                width: '100%',
                minimumResultsForSearch: -1,
                selectOnClose: true,
            });

            $this.val('').trigger('change');

            ResetDDOptions();

            updateFullPayment();
            setupInfoCard(); // The on change event on the select will take care of this. refer to line 73
            calculateDDTerm();
            setDDField();
            RefreshDDTermInfoBox();
        });

        if ($('.js-apply-dicount').is(':checked')) {
            discountActive = !discountActive;
            $('.js-discount-info').slideToggle();
            $('#discount-apply-confirm').attr('disabled', function (i, v) { return !v; });

            let $this = $('.js-hide-option').parent();
            $('.js-hide-option').attr('disabled', function (i, v) { return !v; });
            $this.select2('destroy');

            $this.select2({
                theme: 'lowell',
                width: '100%',
                minimumResultsForSearch: -1,
                selectOnClose: true,
            });

            $('.js-payment-amount').each((index, element) => {
                element.value = 0;
            });

            updateFullPayment();
            setupInfoCard();
            calculateDDTerm();
            RefreshDDTermInfoBox();
        }
        // END jQuery Listeners

        // Public Functions
        function FrequencyChanged()
        {
            let paymentFrequency = _getPaymentLength();

            if (paymentFrequency == 'monthly') {
                updateDatePicker(true);
            }
            else {
                updateDatePicker();
            }

            const { week, fortnight, every4week } = getDurationConstants();

            let disposableIncome = $('#disposableIncome');
            let averagePayment = $('#averagePayment');

            switch (paymentFrequency) {
                case 'weekly':
                    disposableIncome.val(parseFloatWithCommas(paymentSettings.monthlyDisposableIncomePerAccount / week));
                    averagePayment.val(parseFloatWithCommas(paymentSettings.averageMonthlyPayment / week));
                    break;
                case 'fortnightly':
                    disposableIncome.val(parseFloatWithCommas(paymentSettings.monthlyDisposableIncomePerAccount / fortnight));
                    averagePayment.val(parseFloatWithCommas(paymentSettings.averageMonthlyPayment / fortnight));
                    break;
                case '4week':
                    disposableIncome.val(parseFloatWithCommas(paymentSettings.monthlyDisposableIncomePerAccount / every4week));
                    averagePayment.val(parseFloatWithCommas(paymentSettings.averageMonthlyPayment / every4week));
                    break;
                case 'every 4 weeks':
                    disposableIncome.val(parseFloatWithCommas(paymentSettings.monthlyDisposableIncomePerAccount / every4week));
                    averagePayment.val(parseFloatWithCommas(paymentSettings.averageMonthlyPayment / every4week));
                    break;
                case 'monthly':
                    disposableIncome.val(parseFloatWithCommas(paymentSettings.monthlyDisposableIncomePerAccount));
                    averagePayment.val(parseFloatWithCommas(paymentSettings.averageMonthlyPayment));
                    break;
            }

            let ddAmount = _getDDPaymentAmount();

            if ($('#DisposableIncome').prop('checked')) {
                ddAmount = disposableIncome.val();
            }

            if ($('#AverageSetupValue').prop('checked')) {
                ddAmount = averagePayment.val();
            }

            updateNumberPayment(ddAmount);
            updateDirectDebtInfo(ddAmount);

            RefreshDDTermInfoBox();
        }

        function mergeAllPaymentSettings() {
            $('.js-payment-calc .js-general-payment-settings input').each((index, element) => {
                element.value = element.value || paymentSettings[element.name];
                paymentSettings[element.name] = element.value;
            });

            $('.payment-active').find('.js-payment-settings-type input').each((index, element) => {
                element.value = element.value || paymentSettings[element.name];
                paymentSettings[element.name] = element.value;
            });

            //Convert string to bool
            paymentSettings.iAndELessThanOrIs12MonthsOld = JSON.parse(paymentSettings.iAndELessThanOrIs12MonthsOld);
        }

        function setupInfoCard() {
            if ($('.payment-active').data('form-fields') == 'direct-debit') {
                $('.js-payment-info-box').find('.js-payment-info').show();
            }
            else {
                $('.js-payment-info-box').find('.js-payment-info').hide();
            }

            if (discountActive) {
                $('.js-payment-info-box').find('.js-payment-balance').text('£' + parseFloatWithCommas(paymentSettings.discountBalance));
                $('.js-payment-info-box').find('.js-payment-balance-name').text('Discounted balance');
                // Fix for - TFS 86359
                $('.js-payment-info-box').find('.js-payment-amount').text('£' + parseFloatWithCommas(paymentSettings.discountBalance));
            }
            else {
                $('.js-payment-info-box').find('.js-payment-balance').text('£' + parseFloatWithCommas(paymentSettings.balance));
                $('.js-payment-info-box').find('.js-payment-balance-name').text('Balance');
                $('.js-payment-info-box').find('.js-payment-amount').text('£' + parseFloatWithCommas(paymentSettings.paymentAmount));
            }

            $('.js-payment-info-box').find('.js-payment-account').text(paymentSettings.account);
            $('.js-payment-info-box').find('.js-payment-name').text(paymentSettings.paymentName);
            $('.js-payment-info-box').find('.js-payment-term').text(paymentSettings.term);
            $('.js-payment-info-box').find('.js-payment-date').text(moment(paymentSettings.paymentDate, 'DD-MM-YYYY').format('Do MMMM YYYY'));
        }

        function updateFullPayment() {
            $('.js-payment-amount').val(discountActive
                ? parseFloatWithCommas(paymentSettings.discountBalance)
                : parseFloatWithCommas(paymentSettings.balance));
        }

        function setFullPaymentHiddenInput() {
            $('input[type=hidden].js-payment-amount').val(parseFloatWithCommas(paymentSettings.paymentAmount));
        }

        function updateDirectDebtInfo(amount) {
            let termLength = _calcLengthOfDD(getBalanceAmount(), amount, _getPaymentLength());
            $activePaymentSettings.find('input[name=term]').val(termLength);
            $('.js-payment-info-box').find('.js-payment-term').text(termLength);
            $('.js-payment-info-box').find('.js-payment-name').text($('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val());
        }

        function changeStartDate(dateSelected) {
            $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentDate]').val(dateSelected);
            $('.js-payment-info-box').find('.js-payment-date').text(moment(dateSelected, 'DD-MM-YYYY').format('Do MMMM YYYY'));
        }

        function updateActiveSettings() {
            $activePaymentSettings = $('.payment-active .js-payment-settings-type');

            $('.payment-active').find('.js-payment-settings-type input').each((index, element) => {
                element.value = element.value || paymentSettings[element.name];
                paymentSettings[element.name] = element.value;
            });
        }

        function setNumberalFields() {
            $('.js-payment-calc .js-payment-input').each((index, element) => {
                numberalFields.push(new cleave(element, numberalFieldOptions));
            });
        }

        function updateNumberPayment(value) {
            let formattedValue = parseFloatWithCommas(value);

            $activePaymentSettings.find('input[name=paymentAmount]').val(formattedValue);
            $('.js-payment-info-box').find('.js-payment-amount').text('£' + formattedValue);

            if ($('.payment-active').data('form-fields') == 'direct-debit') {
                updateDirectDebtInfo(formattedValue);
            }
        }

        function calculateDDTerm() {
            $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=term]').val(
                _calcLengthOfDD(
                    getBalanceAmount(),
                    _getDDPaymentAmount(),
                    _getPaymentLength()
                )
            );
        }

        function setDDField() {
            if (getFloatValue($('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentAmount]').val()) >
                getFloatValue(getBalanceAmount()))
            {
                $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentAmount]').val('1.00');
                $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-input').val('1.00');
            }
        }

        function getBalanceAmount() {
            return discountActive ? paymentSettings.discountBalance : paymentSettings.balance;
        }

        function updateDatePicker(monthly = false) {
            if (window.picker != null) {
                window.picker.destroy();
            }

            let picker = new Pikaday({
                field: $('.js-datepicker-payment')[0],
                firstDay: 1,
                format: 'DD-MM-YYYY',
                minDate: moment(paymentSettings.minDateRange, 'DD-MM-YYYY').toDate(),
                maxDate: moment(paymentSettings.maxDateRange, 'DD-MM-YYYY').toDate(),
                disableWeekends: false, // Fix for Bug 86362
                showDaysInNextAndPreviousMonths: true,
                keyboardInput: false
            });

            if (paymentSettings.paymentDate != '' && monthly) {
                picker.setDate(moment(paymentSettings.paymentDate, 'DD-MM-YYYY'));
            }
            else {
                picker.setDate(moment(paymentSettings.minDateRange, 'DD-MM-YYYY').toDate());
            }

            window.picker = picker;
        }

        function updatePaymentInfo(legnth, date) {

            let weekendClause = '';
            let dayOfWeek = moment(date, 'DD-MM-YYYY').day();

            if (dayOfWeek == 6 || dayOfWeek == 0)
            {
                weekendClause = ' You have selected a weekend, however, your payment will be taken on the next working day.'
            }

            switch (legnth) {
                case 'monthly':
                    $('.js-payment-info').html('Payments will be taken on the <span>' + moment(date, 'DD-MM-YYYY').format('Do') + ' of each month</span>, in cases that this falls on a bank holiday or a weekend, the payment will be taken on the next working day.');
                    break;
                case 'weekly':
                    $('.js-payment-info').html('Payments will be taken on the <span>' + _getWeekDay(dayOfWeek) + '</span> ' + _getPaymentLength() + '.' + weekendClause);
                    break;
                case 'fortnightly':
                    $('.js-payment-info').html('Payments will be taken each <span>' + _getWeekDay(dayOfWeek) + '</span>, on a fortnightly basis.' + weekendClause);
                    break;
                case '4week':
                case 'every 4 weeks':
                    $('.js-payment-info').html('Your first payment will be on the <span>' + moment(date, 'DD-MM-YYYY').format('Do of MMM') + '</span> and then every <span>4th ' + _getWeekDay(dayOfWeek) + '</span> afterwards.' + weekendClause);
                    break;
            }
        }

        function RefreshDDTermInfoBox()
        {
            let ddPaymentAmount = getFloatValue(_getDDPaymentAmount());
            let ddPaymentFrequency = _getPaymentLength();
            let balance = getFloatValue(getBalanceAmount());

            if (ddPaymentAmount && ddPaymentFrequency && balance) {

                //Show DD Term InfoBox
                let ddTermTotalMonths = _calcDDTotalMonths(balance, ddPaymentAmount, ddPaymentFrequency);
                let ddTermMonths = (ddTermTotalMonths % 12);
                let ddTermYears = Math.floor(ddTermTotalMonths / 12);

                if (ddTermMonths <= 0 && ddTermYears <= 0) {
                    ddTermMonths = ddTermMonths + 1;
                }

                $('#dd-term-years').text(ddTermYears);
                $('#dd-term-months').text(ddTermMonths);
                $('#dd-term-message').removeClass('d-none');

                if (paymentSettings.iAndELessThanOrIs12MonthsOld && paymentSettings.monthlyDisposableIncomePerAccount <= 0) {
                    $('#dd-term-debt-advice').removeClass('d-none');
                }
                else {
                    $('#dd-term-debt-advice').addClass('d-none');
                }
            }
            else {
                //Hide DD Term InfoBox
                $('#dd-term-message').addClass('d-none');
            }
        }

        function ShowHideNextButton() {
            if ($('.js-payment-select').val() === 'direct-debit') {
                $('#btnNext').addClass('d-none');
            }
            else {
                $('#btnNext').removeClass('d-none');
            }
        }
        
        // END Public Functions
    }
};

// Private Functions
function _calcDDTotalMonths(balance, ddAmount, ddFrequency) {

    const { week, fortnight, every4week } = getDurationConstants();
    
    switch (ddFrequency) {
        case 'monthly':
            return Math.ceil(balance / ddAmount);
        case 'weekly':
            return Math.ceil((balance / ddAmount) / week);
        case 'fortnightly':
            return Math.ceil((balance / ddAmount) / fortnight);
        case '4week':
        case 'every 4 weeks':
            return Math.ceil((balance / ddAmount) / every4week);
    }
}

function _calcLengthOfDD(balance, amount, length) {
    const { week, fortnight, every4week } = getDurationConstants();
    let numberBalance = getFloatValue(balance)
    let numberAmount = getFloatValue(amount);

    let term;
    switch (length) {
        case 'monthly':
            term = Math.ceil(numberBalance / numberAmount);
            $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val('Monthly payment');
            break;
        case 'weekly':
            term = Math.ceil((numberBalance / numberAmount) / week);
            $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val('Weekly payment');
            break;
        case 'fortnightly':
            term = Math.ceil((numberBalance / numberAmount) / fortnight);
            $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val('Fortnightly payment');
            break;
        case '4week':
        case 'every 4 weeks':
            term = Math.ceil((numberBalance / numberAmount) / every4week);
            $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentName]').val('Four Weekly payment');
            break;
    }

    if (!(isFinite(term))) {
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
    if ($('.js-payment-calc .js-payment-length').val() == '4week') {
        return 'every 4 weeks';
    }
    return $('.js-payment-calc .js-payment-length').val();
}

function _getDDPaymentAmount() {
    return $('.js-payment-calc div[data-form-fields=direct-debit] .js-payment-settings-type input[name=paymentAmount]').val();
}

// END Private Functions