import $ from 'jquery';

export default () => {
    $('.js-close').on('click', (e) => {
        $('.mobile-header__highlight').slideUp();    
        $('.mobile-header__highlight').addClass('mobile-header__highlight--hidden');
    });
};