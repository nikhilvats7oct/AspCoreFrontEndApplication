import "babel-polyfill";

import $ from 'jquery';
window.jQuery = window.$ = $;

import initForm from './partials/_form';
import initModal from './partials/_modal';
import initMobileNav from './partials/_mobile-nav';
import initMobileHighlight from './partials/_mobile-highlight';
import initNotification from './partials/_notifaction';
import initDatePicker from './partials/_date-picker';
import initGeneral from './partials/_general';
import initSticky from './partials/_sticky';
import initFaqs from './partials/_faqs';
import initSelect2 from './partials/_select2';
import documents from './partials/_documents';


$(function() {
    if($('.js-payment-calc').length > 0) {
        import(/* webpackChunkName: "payment-calculator" */ './partials/_payment-calc').then(initPaymentCalc => {
            initPaymentCalc.default();
        });
    }
});

$(function() {
    if($('.js-budget-calc').length > 0) {
        import(/* webpackChunkName: "budget-calculator" */ './partials/_budget-calc').then(initBudgetCalc => {
            initBudgetCalc.default();
        });
    }
});

$(function() {
    if($('.js-chart-settings').length > 0) {
        import(/* webpackChunkName: "budget-summary" */ './budget-summary/pie-chart').then(initPieChart => {
            initPieChart.default();
        });
    }
});

// This functions removes the modal from accounts page when 'Make a payment' or 'Setup a plan' is clicked
// as these buttons dont actually do anything but close the modal.
$(function () {

    $(".close-modal").each(function(index, value) {
        $(value).click(function () {

            $(".mfp-ready").remove();
            $(".mfp-wrap").remove();
            $('html').css({ 'overflow': '', 'margin-right': ''});
        });
    });
});


initForm();
initModal();
initMobileNav();
initMobileHighlight();
initNotification();
initDatePicker();
initGeneral();
initSticky();
initFaqs();
initSelect2();

var addDob = function () {
    if (window.location.pathname === "/DataProtection") {

        var day = document.querySelector(".dropdown-day").value;
        var month = document.querySelector(".dropdown-month").value;
        var year = document.querySelector(".dropdown-year").value;
        var dob = document.querySelector(".date-of-birth-text");

        if (day != "" && month != "" && year != "") {
            dob.value = day + "-" + month + "-" + year;
        }
    }
}

window.addEventListener("load", addDob, false);