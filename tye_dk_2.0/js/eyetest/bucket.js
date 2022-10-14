var level = 1,
	dtStart = new DateTime(),
	intervalTimeSpent = null;

function setLevel(obj, lv) {
	if (eyeTestScreen.IsRunning) {
		Stop();
	} else {
		eyeTestScreen.stop();
	}
	level = lv;
	$('.eyeTestScreenTopMenu a').addClass('positivesmall').removeClass('negativesmall');
	$(obj).removeClass('positivesmall').addClass('negativesmall');
}


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
	$('#img3d').attr('src', '/img/eyetest/bucket/' + lv + '.png');
}

function updateHidLevel() {
	$('#hidAttribName').val('level');
	$('#hidAttribValue').val(level);
}