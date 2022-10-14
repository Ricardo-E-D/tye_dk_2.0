function fixDtpPosition(obj, input, calendar) {

	$(calendar).css('top', $(input).position().top);
	$(calendar).css('left', $(input).width());
}