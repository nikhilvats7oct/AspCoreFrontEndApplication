import $ from 'jquery';
import Pikaday from 'pikaday';
import 'select2';
// import * as moment from 'moment/moment';
import { keepDropdownBelowSelect } from '../_helpers';
const moment = require('moment');

export default () => {

    window.moment = moment;

    $('.js-select-dob').select2({
        theme: 'lowell',
        width: '100%',
        selectOnClose: true,
    }).on('select2:open', (e) => {
        // keepDropdownBelowSelect(e);
    }).on('change', (e) => {
        let $this = $(e.currentTarget),
            $dobField = $('input[type=text].date-of-birth-text'),
            day = $('.dropdown-day').val(),
            month = $('.dropdown-month').val(),
            year = $('.dropdown-year').val();

        console.log('day' + day);
        console.log('month' + month);
        console.log('year' + year);

        if (day && month && year) {
            let $textValue = day + '-' + month + '-' + year;
            $dobField.attr('value', $textValue);
            $dobField.val($textValue);

            let tmpDt = moment($textValue, "DD-MMMM-YYYY");
            let isValid = tmpDt.isValid();

            console.log('date isvalid()=' + isValid);

        } else {
            $dobField.val('');
        }

        let $closestFormValidator = $this.closest('.form').first().validate();
        // Validate this element, thsi will display errors if invalid.
        if ($('input[type=text].date-of-birth-text').length > 0)
            $closestFormValidator.element('input[type=text].date-of-birth-text');

        console.log($dobField.val());
    });

    //callback date    
    if ($('.js-datepicker-callback').length) {
        if (window.picker != null) {
            window.picker.destroy();
        }

        let noofdays = 4;
        var d = new Date();

        for (var i = 0; i <= 4; i++)
		{
	        if (d.getDay() == 0)
		        noofdays = 5;
	        d.setDate(d.getDate() + 1);
        }

        var startDayIndex = 0;
        if ($('#SlotsAvailableForCurrentDay').length > 0) {

            var value = $('#SlotsAvailableForCurrentDay').val();

            if (value == "1") {
                startDayIndex = 1;
            }
        }

        var startDate = moment().add(startDayIndex, 'days').format('DD-MM-YYYY');
        var endDate = moment().add(noofdays + startDayIndex, 'days').format('DD-MM-YYYY');

        let callbackSettings = {
            minDateRange: startDate,
            maxDateRange: endDate
        }

        let picker = new Pikaday({
            field: $('.js-datepicker-callback')[0],
            firstDay: 1,
            format: 'DD-MM-YYYY',
            minDate: moment(callbackSettings.minDateRange, 'DD-MM-YYYY').toDate(),
            maxDate: moment(callbackSettings.maxDateRange, 'DD-MM-YYYY').toDate(),
            disableWeekends: false,
            disableDayFn: function (date) {
                // Disable Sunday
                return (date.getDay() === 0);
            },
            showDaysInNextAndPreviousMonths: true,
            keyboardInput: false
        });

        picker.setDate(moment(callbackSettings.minDateRange, 'DD-MM-YYYY').toDate());

        window.picker = picker;
    };

    //callback timeslot
    $('.js-datepicker-callback').on('change', (e) => {
        let dateSelected = $(e.currentTarget).val();

            $('#TimeSloFirstDay').val('');
            $('#TimeSlotWeekday').val('');
            $('#TimeSlotSaturday').val('');

        if ($('#TimeSlotSunday').length > 0) {

            let $timeslotField = $('input[type=text].time-slot-text');
            $timeslotField.attr('value', '');
            $timeslotField.val('');

            let tmpDt = moment(dateSelected, "DD-MM-YYYY");
            let callbackDt = new Date(tmpDt);
            let CurrentDate = new Date($("#FirstAvailableDate").val());
            let curr = callbackDt.getDay();

            if (!tmpDt.isValid() || curr == 0) {
                $('#dvsundaytimeslots').show();
                $('#dvtodaytimeslots').hide();
                $('#dvweekdaytimeslots').hide();
                $('#dvsaturdaytimeslots').hide();
            }
            else {
                if (callbackDt.getDate() == CurrentDate.getDate()
                    && callbackDt.getMonth() == CurrentDate.getMonth()
                    && callbackDt.getFullYear() == CurrentDate.getFullYear()) {
                    $('#dvsundaytimeslots').hide();
                    $('#dvtodaytimeslots').show();
                    $('#dvweekdaytimeslots').hide();
                    $('#dvsaturdaytimeslots').hide();
                }
                else if (curr == 6) {
                    $('#dvsundaytimeslots').hide();
                    $('#dvtodaytimeslots').hide();
                    $('#dvweekdaytimeslots').hide();
                    $('#dvsaturdaytimeslots').show();
                }
                else {
                    $('#dvsundaytimeslots').hide();
                    $('#dvtodaytimeslots').hide();
                    $('#dvweekdaytimeslots').show();
                    $('#dvsaturdaytimeslots').hide();
                }
            }
        }
    });

    //callback timeslot
    $('.js-select-timeslot').on('change', (e) => {
        let $timeslotField = $('input[type=text].time-slot-text'),
            valtoday = $('#TimeSloFirstDay').val(),
            valweekday = $('#TimeSlotWeekday').val(),
            valsaturday = $('#TimeSlotSaturday').val();

        var timeslot = "";

        if (valtoday != undefined && valtoday != "") {
            timeslot = valtoday;
            $('#TimeSlotWeekday').val("");
            $('#TimeSlotSaturday').val("");
        } else if (valweekday != undefined && valweekday != "") {
            timeslot = valweekday;
            $('#TimeSlotSaturday').val("");
            $('#TimeSloFirstDay').val("");

        } else if (valsaturday != undefined && valsaturday != '') {
            timeslot = valsaturday;
            $('#TimeSlotWeekday').val("");
            $('#TimeSloFirstDay').val("");
        }

        $timeslotField.attr('value', timeslot);
        $timeslotField.val(timeslot);
    });

    //callback callmenow
    $('.clscallmenow').on('click', (e) => {       
        $('.clscallmenowmsg').removeClass('d-none');        
        $('.clscallbackdate').addClass('d-none');
        $('.clscallbacktime').addClass('d-none');
    });

    //callback callmenow new 
    $('.CallMeNow').on('click', (e) => {
	    $('.clscallmenowmsg').removeClass('d-none');
	    $('.clscallbackdate').addClass('d-none');
        $('.clscallbacktime').addClass('d-none');
    });

    //appointment callmenow new 
    $('.Appointment').on('click', (e) => {
        $('.clscallmenowmsg').addClass('d-none');
        $('.clscallbackdate').removeClass('d-none');
        $('.clscallbacktime').removeClass('d-none');
    });

    $('.js-select-style-only').select2({
        theme: 'lowell',
        width: '100%',
        selectOnClose: true,
    }).on('select2:open', (e) => {
        keepDropdownBelowSelect(e);
    });

    $('.js-select-style-only-no-search').select2({
        theme: 'lowell',
        width: '100%',
        minimumResultsForSearch: -1,
        selectOnClose: true,
    }).on('select2:open', (e) => {
        keepDropdownBelowSelect(e);
    });

    $('.js-select').each((index, element) => {
        $(element).select2({
            theme: 'lowell',
            width: '100%',
            minimumResultsForSearch: -1,
            selectOnClose: true,
        }).on('change', (e) => {
            let $this = $(e.currentTarget),
                target = $this.data('target'),
                val = $this.find('option:selected').data('form'),
                $form = $this.closest('form');

            _findFormFields($form, target, val);
        }).on('select2:open', (element) => {
            keepDropdownBelowSelect(element);
        }).trigger('change');
    });
};

function _findFormFields($form, target, name) {
    $form.find(`[data-target-element='${target}']`).slideUp().removeClass('payment-active');
    $form.find(`[data-target-element='${target}'] :input:not(.js-disabled)`).attr('disabled', true);
    $form.find(`[data-target-element='${target}'][data-form-fields='${name}']`).slideDown().addClass('payment-active');
    $form.find(`[data-target-element='${target}'][data-form-fields='${name}'] :input:not(.js-disabled)`).attr('disabled', false);
}