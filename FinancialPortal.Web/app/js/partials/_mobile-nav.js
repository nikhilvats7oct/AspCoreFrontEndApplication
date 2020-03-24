import $ from 'jquery';
import _debounce from 'lodash.debounce';

export default () => {
    // Show nav
    $('.header__mobile-options .hamburger').on('click', _debounce((e) => {
        toggleNav();
    }, 250, {
        'leading': true,
        'trailing': false
    }) );

    // Accordion nav menu items
    $('.js-has-menu').on('click', (e) => {
        e.preventDefault();

        let $this = $(e.currentTarget);
        $this.toggleClass('has-menu--active');
        $this.siblings('ul').slideToggle();
    });

    // Close nav if user clicks the main element
    $('.jw-wrapper').on('touchstart', () => {
        if ($('body').hasClass('noscroll')) {
            toggleNav();
        }
    });
};

function toggleNav() {
    toggleClass();
    toggleScroll();
    checkHighlightMenu();

    $('.mobile-header__nav').slideToggle();
}

function toggleClass() {
    $('.header__mobile-options .hamburger i').toggleClass('jw-icon-hamburger');
    $('.header__mobile-options .hamburger i').toggleClass('jw-icon-cross');
}

function toggleScroll() {
    $('body').toggleClass('noscroll');
    $('.mobile-header').toggleClass('mobile-header--nav-active');
}

function checkHighlightMenu() {
    if ( !($('.mobile-header__highlight').hasClass('mobile-header__highlight--hidden'))) {
        $('.mobile-header__highlight').slideToggle();
    }
}