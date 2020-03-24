import $ from 'jquery';
import 'magnific-popup';

export default () => {
    $('a[data-modal]').each((index, element) => {
        let $link = $(element),
            content =  $link.attr('data-modal'),
            type = $link.attr('data-modal-type');

        $link.magnificPopup({
            closeMarkup: '<i class="jw-icon-cross mfp-close"></i>',
            items: {
                src: content
            },
            type: type,
            callbacks: {
                beforeOpen: () => {
                    $('html').css('overflow', 'hidden');
                },
                open: () => {
                    $('.mfp-wrap').css('overflow',  'hidden auto');
                },
                beforeClose: () => {
                    $('html').css('overflow', 'auto');
                    $('.mfp-wrap').css('overflow',  'auto');
                },
            }
        });
    });
};