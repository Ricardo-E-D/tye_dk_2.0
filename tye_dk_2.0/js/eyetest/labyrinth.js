var level = 1,
	dtStart = new DateTime(),
	intervalTimeSpent = null;

function Start() {
	var elem = $('#btnStart');
	if (elem.val() == "Start") {
		intervalTimeSpent = setInterval(function () {
			var dtEnd = new DateTime();
			$('div.eyeTestTime').html(dtEnd.subtractDate(dtStart).toTimeString('mm:ss'));
		}, 1000);
		elem.val("Stop");
		$(elem).addClass('negativesmall').removeClass('positivesmall');
		eyeTestScreen.start();
		dtStart = new DateTime();
		updateHidLevel();

	} else {
		clearInterval(intervalTimeSpent);

		$(elem).addClass('positivesmall').removeClass('negativesmall').val("Start");
		var dtEnd = new DateTime();
		var totalSeconds = parseInt(dtEnd.subtractDate(dtStart).totalSeconds(), 10);

		eyeTestScreen.stop();
		eyeTestScreen.IsRunning = false;
	}
}

function getLevel() {
	return parseInt($('#hidAttribValue').val(), 10);
}

function setLevel(obj, lv) {
	if (eyeTestScreen.IsRunning) {
		Start();
	} else {
		eyeTestScreen.stop();
	}
	updateHidLevel();
	level = lv;
	$('.eyeTestScreenTopMenu a').addClass('positivesmall').removeClass('negativesmall');
	$(obj).removeClass('positivesmall').addClass('negativesmall');
	$('#imgLabyrinth').attr('src', '/img/eyetest/labyrinth/' + lv + '.png');
	
	$('#imgLabyrinth').css('height', 'auto');
	setTimeout(function () { resizeLabyrinth(); }, 500);

//	if (!eyeTestScreen.IsRunning) {
//		Start();
//	}
}

function updateHidLevel() {
	$('#hidAttribName').val('level');
	$('#hidAttribValue').val(level);
}

function resizeLabyrinth() {
	var t = $('#imgLabyrinth'), top = t.position().top, height = t.height(), windowHeight = $(window).innerHeight();
	

	if (top + height > windowHeight) {
		//		t.css('height', (windowHeight - top - 30) + 'px');
		t.animate({ height: (windowHeight - top - 30) + 'px' });
	} else {
		t.css('height', 'auto');
	}


}