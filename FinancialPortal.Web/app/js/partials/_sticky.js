import $ from 'jquery';

export default function() {
    if($('.js-sticky').length) {
        $(window).on('load scroll resize orientationchange', () => {
            $('.js-sticky').each((index, element) => {
                let $this = $(element),
                    $windowTop = $(window).scrollTop(),
                    stickyWidth = $this.siblings('.js-sticky-width').innerWidth(),
                    elementTop = $this.offset().top,
                    elementHeight = $this.outerHeight(),
                    elementBottom = $this.offset().top + elementHeight,
                    parentHeight = $this.parent().outerHeight(),
                    containerTop = $this.closest('.container').offset().top,
                    headerHeight = 0;

                if($('.js-sticky-header').length) {
                    headerHeight = $('.js-sticky-header').outerHeight();
                    $windowTop = $windowTop + headerHeight;
                }

                if($(window).width() > 992) {
                    if($this.hasClass('sticky') && containerTop > $windowTop ) {
                        $this.css({width: 'auto'});
                        $this.removeClass('sticky');
                        
                        if(headerHeight > 0) {
                            $this.css({top: 0});
                        }
                    }

                    if(elementTop - $windowTop < 0) {
                        $this.css({width: stickyWidth});
                        $this.addClass('sticky');

                        if(headerHeight > 0 && !($this.hasClass('sticky--absolute'))) {
                            $this.css({top: headerHeight});
                        }
                    }

                    if($this.hasClass('sticky--absolute') && elementTop > $windowTop) {
                        $this.removeClass('sticky--absolute');
                        
                        if (headerHeight > 0) {
                            $this.css({
                                top: headerHeight
                            })
                        }
                        else {
                            $this.css({
                                top: 0,
                            });
                        }
                    }

                    if(elementBottom - containerTop - parentHeight > 0) {
                        $this.addClass('sticky--absolute');
                        
                        $this.css({
                            top: (parentHeight - elementHeight),
                        });
                    }
                    
                }
                else {
                    $this.css({width: 'auto'});
                    $this.removeClass('sticky');
                    $this.removeClass('sticky--absolute');
                    
                    if(headerHeight > 0) {
                        $this.css({top: 0});
                    }
                }
            })
        });
    }

    if($('.js-sticky-header').length) {
        $(window).on('load scroll resize orientationchange', () => {
            let elementTop = $('.js-sticky-header').offset().top,
                elementHeight = $('.js-sticky-header').outerHeight();

            if($('.js-sticky-header').hasClass('sticky') && $(window).scrollTop() == 0) {
                $('body').css({'padding-top': 0});
                $('.js-sticky-header').removeClass('sticky');
            }
            if(elementTop - $(window).scrollTop() < 0) {
                $('body').css({'padding-top': elementHeight});
                $('.js-sticky-header').addClass('sticky');
            }
        });
    }
};