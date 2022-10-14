var speed = 1;
var speeds = new Array(6000,5000,4000,3000,2000,1000);
var images = new Array();
var interval;
var dtStart = new DateTime();
var img1 = null,
	img2 = null,
	img3 = null;

function Stop(){
	var btn = document.getElementById("btn");
	if(btn.value == "Start"){
		btn.value = "Stop";
		$(btn).addClass("negativesmall").removeClass("positivesmall");
		eyeTestScreen.start();
		dtStart = new DateTime();
		SwitchPictures();
		StartMotion();
	} else {
		// stop eyetest
		
		$(btn).addClass("positivesmall").removeClass("negativesmall");
		btn.value = "Start";
		var dtEnd = new DateTime();
		var totalSeconds = parseInt(dtEnd.subtractDate(dtStart).totalSeconds(), 10);

		eyeTestScreen.stop();
		eyeTestScreen.IsRunning = false;
		clearInterval(interval);
	}
}

function updateHidSpeed() {
	$('#hidAttribName').val('speed');
	$('#hidAttribValue').val(speed);
}

function SwitchPictures() {
	img3.src = img2.src;
	img2.src = img1.src;
	img1.src = "/img/eyetest/sunStar/" + parseInt(Math.random() * 6) + ".gif";
}

function SetPictures(){
	img1 = document.getElementById("img1");
	img2 = document.getElementById("img2");
	img3 = document.getElementById("img3");
	images[0] = parseInt(Math.random()*6);
	images[1] = parseInt(Math.random()*6);
	images[2] = parseInt(Math.random()*6);
	img1.src = "/img/eyetest/sunStar/" + images[0] + ".gif";
	img2.src = "/img/eyetest/sunStar/" + images[1] + ".gif";
	img3.src = "/img/eyetest/sunStar/" + images[2] + ".gif";
}

function StartMotion(){
	interval = window.setInterval(function () { SwitchPictures(); }, speeds[speed]);
}

function SpeedUp(){
	if(speed < 5){
		speed += 1;
		SetSpeedImage("on");
		clearInterval(interval);
		if(eyeTestScreen.IsRunning)
			interval = window.setInterval(function () { SwitchPictures(); }, speeds[speed]);
	}
	updateHidSpeed();
}

function SpeedDown(){
	if(speed > 1){
		speed -= 1;
		SetSpeedImage("off");
		clearInterval(interval);
		if (eyeTestScreen.IsRunning)
			interval = window.setInterval(function () { SwitchPictures(); }, speeds[speed]);
	}
	updateHidSpeed();
}

function SetSpeedImage(pic){
	var img = null;
	if(pic == "on"){
		img = document.getElementById("speed" + speed);
		img.src = "/img/eyetest/sunStar/speed" + pic + ".gif";
	} else {
		var num = speed+1;
		img = document.getElementById("speed" + num);
		img.src = "/img/eyetest/sunStar/speed" + pic + ".gif";
	}
}