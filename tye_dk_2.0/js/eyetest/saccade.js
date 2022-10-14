var speed = 0,
	speeds = new Array(6000, 5000, 4000, 3000, 2000, 1000),
	images = new Array(),
	dtStart = new DateTime(),
	interval,
	level = 1;
images[0] = new Array("1-0.gif", "1-1.gif");
images[1] = new Array("2-0.gif", "2-1.gif");
images[2] = new Array("3-0.gif", "3-1.gif");
images[3] = new Array("4-0.gif", "4-1.gif");
images[4] = new Array("5-0.gif", "5-1.gif", "5-2.gif");


var img1 = null,
		img2 = null,
		img3 = null;

function Stop() {
	var btn = document.getElementById("btn");
	if (btn.value == "Start") {
		btn.value = "Stop";
		$(btn).addClass('negativesmall').removeClass('positivesmall');
		eyeTestScreen.start();
		dtStart = new DateTime();
		StartMotion();
		updateHidSpeed();
	} else {
		// stop eyetest
		clearInterval(interval);

		$(btn).addClass('positivesmall').removeClass('negativesmall').val("Start");
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
		Stop();
	} else {
		eyeTestScreen.stop();
	}
	level = lv;
	$('.eyeTestScreenTopMenu a').addClass('positivesmall').removeClass('negativesmall');
	$(obj).removeClass('positivesmall').addClass('negativesmall');
}

function updateHidSpeed() {
	$('#hidAttribName').val('level');
	$('#hidAttribValue').val(level);
}

function SwitchPictures() {
	img3.src = img2.src;
	img2.src = img1.src;
	img1.src = "/img/eyetest/clapStomp/" + this.GetRandomPicture(getLevel());
}

function GetRandomPicture(level) {
	try {
		var index = parseInt(Math.random() * level)
		var index2;
		if (index != 4) {
			index2 = parseInt(Math.random() * 2);
		} else {
			index2 = parseInt(Math.random() * 3);
		}
		return images[index][index2];
	} catch (err) { alert(level); }
}

function SetPictures() {
	img1 = document.getElementById("img1");
	img2 = document.getElementById("img2");
	img3 = document.getElementById("img3");

	if (level == parseInt(0)) {
		level = 5;
	}

	img1.src = "/img/eyetest/clapStomp/" + this.GetRandomPicture(level);
	img2.src = "/img/eyetest/clapStomp/" + this.GetRandomPicture(level);
	img3.src = "/img/eyetest/clapStomp/" + this.GetRandomPicture(level);
}

function StartMotion() {
	interval = window.setInterval(function () { SwitchPictures(); }, speeds[speed]);
}

function SpeedUp() {
	if (speed < 5) {
		speed += 1;
		SetSpeedImage("on");
		clearInterval(interval);
		if(eyeTestScreen.IsRunning)
			interval = window.setInterval(function () { SwitchPictures(); }, speeds[speed]);
	}
	updateHidSpeed();
}

function SpeedDown() {
	if (speed > 0) {
		speed -= 1;
		SetSpeedImage("off");
		clearInterval(interval);
		if (eyeTestScreen.IsRunning)
			interval = window.setInterval(function () { SwitchPictures(); }, speeds[speed]);
	}
//	else if (speed <= 1) {
//		speed -= 1;
//		SetSpeedImage("off");
//		clearInterval(interval);
//		//StopLevel();
//		timeSpentField.value = time.GetInterval();
//	}
	updateHidSpeed();
}

function SetSpeedImage(pic) {
	var img = null;
	if (pic == "on") {
		img = document.getElementById("speed" + speed);
		img.src = "/img/eyetest/sunStar/speed" + pic + ".gif";
	} else {
		var num = speed + 1;
		img = document.getElementById("speed" + num);
		img.src = "/img/eyetest/sunStar/speed" + pic + ".gif";
	}
}