import $ from 'jquery';
import _debounce from 'lodash.debounce';

export default function() {
    $('.faq__title').on('keypress click', _debounce((e) => {
        e.preventDefault();
        let $this = $(e.currentTarget);

        if (e.which === 13 || e.which === 32 || e.type === 'click') {
            if($this.hasClass('faq__title--active')) {
                closeAllActive($this.siblings('.faq__body'));
            }

            $this.toggleClass('faq__title--active');
            $this.siblings('.faq__body').toggleClass('faq__body--active').slideToggle();

            if($this.hasClass('js-pull-out')) {
                let $pullOut = $this.siblings('.faq__body').find('.pull-out');

                $pullOut.find('a').attr('tabindex', '-1');
                $pullOut.toggleClass('pull-out--show');
            }
        }
    }, 250, {
        'leading': true,
        'trailing': false
    }) );
};

function closeAllActive($this) {
    let allOpen = $this.find('.faq__body--active');

    allOpen.each((index, element) => {
        $(element).find('.pull-out').removeClass('pull-out--show'); 
        $(element).toggleClass('faq__body--active').slideToggle();
        $(element).siblings('.faq__title--active').removeClass('faq__title--active');
        
    });
}