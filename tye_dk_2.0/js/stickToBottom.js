onload_methods.push(function () {
	var s = $("div.stickToBottom");
	if (s.length == 0)
		return;

	var pos = s.offset(), stickyHeight = s.height();
	var onscroll = function () {
		var windowpos = $(window).scrollTop();
		var windowHeight = $(window).innerHeight();
		var removeStick = (windowpos + windowHeight - stickyHeight) > pos.top;
		if (removeStick) {
			s.removeClass("isSticky span12");
		} else {
			s.addClass("isSticky span12");
		}
	};
	$(window).scroll(onscroll);
	onscroll.apply();
});