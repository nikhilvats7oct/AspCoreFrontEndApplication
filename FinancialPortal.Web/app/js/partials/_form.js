import $ from 'jquery';
import capsLock from 'capslock';

export default () => {
    $('.js-show-password').change( (e) => {
        let $this = $(e.currentTarget);
        
        if( $this.is(':checked') ) {
            $('.js-password').attr('type', 'text');
        } else {
            $('.js-password').attr('type', 'password');
        }
    });

    $('.js-postcode').on('blur',(e) => {
        let $this = $(e.currentTarget);

        var ucase = $this.val().toUpperCase();
        $this.val(ucase);
    });

    capsLock.observe((status) => {
        if (status) {
            $(`<p class="capsLockWarning">
                    <span>
                        <img src="/assets/images/icons/caps-lock-button.svg" />
                    </span>
                    Caps lock is currently active
                </p>`).insertAfter('.js-password');
            return;
        }

        $('.capsLockWarning').remove();
    });

    $('.js-password--disable-paste').each((index, element) => { 
        $(element).bind('cut copy paste', function(e) {
            e.preventDefault();
            return false;
        });
    });

    $('.js-checkbox').on('change', (e) => {
        let $this = $(e.currentTarget),
            target = $this.data('cookie-level');
        
        if( $this.is(':checked') ) {
            $this.siblings('label').text('Currently active');
            $('.js-cookie-list').find(`li[data-cookie-level="${target}"]`).each((index, element) => {
                $(element).removeClass('cross');
            })
        } else {
            $this.siblings('label').text('Currently disabled');
            $('.js-cookie-list').find(`li[data-cookie-level="${target}"]`).each((index, element) => {
                $(element).addClass('cross');
            })
        }
    });
};