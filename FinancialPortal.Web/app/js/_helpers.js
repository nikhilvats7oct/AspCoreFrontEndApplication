import $ from 'jquery';

export const resizeTimeout = (func,delay) => {
	let timeout;
	$(window).resize(() => {
		clearTimeout(timeout);
		timeout = setTimeout(func, delay);
	});
};

export const keepDropdownBelowSelect = (e) => {
    // Height of select box and the height of the browsers position
    var requireHeight = $(e.currentTarget).offset().top;
    var viewportBottom = $(window).scrollTop() + $(window).height();

    // console.log((viewportBottom - requireHeight), (requireHeight - 200)); 

    if($(window).width() < 960) {
        // If the height of the element and the dropdown are less than the browsers current position
        // 200 = Max height of the dropdown container
        if((viewportBottom - requireHeight) <= requireHeight - 200 ) {
            $('html, body').animate({
                scrollTop: $(e.currentTarget).offset().top - 100
            }, 750);
        }
    }
};

export const getFloatValue = (string) => {
    if(typeof string == 'number') {
        string = string.toString();
    }

    return parseFloat(string.replace(/,/g, ''));
}

export const parseFloatWithCommas = (val) => {
	let numberWithCommas = function(x) {
        return x.toString().replace(/(\d)(?=(\d{3})+\b)/g, "$1,");
    };
    
    return numberWithCommas(getFloatValue(val).toFixed(2));
};

export const getDurationConstants = () => {
    const week = 4.33,
        fortnight = 2.17,
        every4week = 1.08;

    return { week, fortnight, every4week };
}