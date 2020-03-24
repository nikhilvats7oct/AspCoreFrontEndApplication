import $ from 'jquery';
import Pikaday from 'pikaday';
import moment from 'moment';

export default () => {
	if($('.js-datepicker').length) { 
		let picker = new Pikaday({
			field: $('.js-datepicker')[0],
			firstDay: 1,
            minDate: new Date(),
            format: 'DD-MM-YYYY',
			showDaysInNextAndPreviousMonths: true,
			keyboardInput: false,
		});
	}
};
