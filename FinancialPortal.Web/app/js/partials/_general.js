import $ from 'jquery';
import 'imagesloaded';
import 'jquery-match-height';
import {resizeTimeout} from '../_helpers';

export default function() {
    // Maintain scroll breaks on ios
    if (!navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
        $.fn.matchHeight._maintainScroll = true;
    }

    // Clear heights of all matched elements before updating
    $.fn.matchHeight._beforeUpdate = function (event, groups) {
        let elementsArr = groups
            .map(group => group.elements.get())
            .reduce((collection, elements) => collection.concat(elements), []);

        $(elementsArr).height('');
    };

    $('[data-mh]').matchHeight();

    // Allow for changes to dom during load
    setTimeout(() => {
        $.fn.matchHeight._update();
    }, 100);

    // Force update on window resize
    resizeTimeout(() => {
        $.fn.matchHeight._update();
    }, 1000);

    $('a[href="#top"]').on('click', (e) => {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 'slow');
    });

    if(_getIOSVersion <= 10) {
        $('body').addClass('ios10');
    }

    $('.js-toggle').on('click', (e) => {
        let $this = $(e.currentTarget);

        $this.parent().siblings('.js-toggle-visible').stop(false, false).slideToggle();
    });

    $('.js-checkbox-toggle').find('input').on('click', (e) => {
        $('.contprefmobile').stop(false, false).slideToggle();
    }); 

    if ($('#AllowContactBySms').length > 0)
    {       
        if ($('#AllowContactBySms').is(':checked')) {
            $('.contprefmobile').slideDown();
        }        
    }    

    $('.js-field').on('click', (e) => {
        if(!($(e.currentTarget).is('input'))){
            e.preventDefault();
        }
        let $this = $(e.currentTarget),
            $target = $this.closest('.js-field-parent').find('.js-field-visible');

        $target.stop(false, false).slideToggle();

        if($target.hasClass('form')) {
            $target.toggleClass('js-show-onload');
        }
    });

    $('.js-field').each((index, element) => {
        if($(element).is(':checked')) {
            $(element).closest('.js-field-parent').find('.js-field-visible').slideDown();
        }
    });

    $('.js-show-onload').each((index, element) => {
        $(element).slideDown();
    });

    $('.js-sibling').on('click', (e) => {
        e.preventDefault();
        let $this = $(e.currentTarget);

        $this.toggleClass('active');

        $this.siblings('.js-toggle-visible').stop(false, false).slideToggle();
    });

    $('.js-transcript-button').on('click', (e) => {
        e.preventDefault();
        let $this = $(e.currentTarget);

        $this.toggleClass('active');

        $this.closest('.video-card').find('.js-toggle-visible').stop(false, false).slideToggle();
    });

    $(window).on('load scroll resize orientationchange', () => {
        let availableHeight = window.innerHeight - ($('.header').outerHeight() + $('.footer').outerHeight());

        if(availableHeight > 400) {
            $('.js-error').height( availableHeight - 100 );
        }
        else {
            $('.js-error').height('auto');
        }
    });

    $('.js-payment-input').on('keydown',
        function (evt) {
            var allow = false;
            if (evt.which >= 48 && evt.which <= 57) { // 0-9 keys above alphabets
                allow = true;
            } else if (evt.which >= 96 && evt.which <= 105) { // 0-9 numberpad keys
                allow = true;
            } else if (evt.which === 190 || evt.which === 110) { // . next to shift and . in number pad
                allow = true;
            } else if (evt.which === 8 || evt.which === 46) { // back space and delete
                allow = true;
            } else if (evt.which === 9) { // tab
                allow = true;
            } else if (evt.which === 116) { // f5
                allow = true;
            }


            if (!allow) {
                evt.preventDefault();
            }
        });
};

function _getIOSVersion() {
    if (/iP(hone|od|ad)/.test(navigator.platform)) {
        var v = (navigator.appVersion).match(/OS (\d+)_(\d+)_?(\d+)?/);
        return parseInt(v[1], 10);
    }
}