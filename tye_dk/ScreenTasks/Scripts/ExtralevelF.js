var speed = 1;
var speeds = new Array(6000,5000,4000,3000,2000,1000);
var images = new Array();
var interval;

var img1 = null;
var img2 = null;
var img3 = null;

function Stop(){
	var btn = document.getElementById("btn");
	if(btn.value == "Start"){
		btn.value = "Stop";
		StartLevel(); 
		SwitchPictures();
		StartMotion();
	} else {
		StopLevel();
		attValueField.value = speed;
		timeSpentField.value = time.GetInterval();
		seconds = timeSpentField.value;
		var alertStr  = SplitString(alertEndField.value,level,score,seconds,errors);
		alert(alertStr);
		document.Form1.submit();
	}
}

function SwitchPictures(){
	
	img3.src = img2.src;
	img2.src = img1.src;		
	img1.src = "Images/ExtralevelF/" + parseInt(Math.random()*6) + ".gif";
}

function SetPictures(){
	img1 = document.getElementById("img1");
	img2 = document.getElementById("img2");
	img3 = document.getElementById("img3");
	images[0] = parseInt(Math.random()*6);
	images[1] = parseInt(Math.random()*6);
	images[2] = parseInt(Math.random()*6);
	img1.src = "Images/ExtralevelF/" + images[0] + ".gif";
	img2.src = "Images/ExtralevelF/" + images[1] + ".gif";
	img3.src = "Images/ExtralevelF/" + images[2] + ".gif";
}

function StartMotion(){
	interval = window.setInterval("SwitchPictures()", speeds[speed]);
}

function SpeedUp(){
	
	if(speed < 5){
		speed += 1;
		SetSpeedImage("on");
		clearInterval(interval);
		interval = window.setInterval("SwitchPictures()", speeds[speed]);
	}
	
}

function SpeedDown(){
	if(speed > 1){
		speed -= 1;
		SetSpeedImage("off");
		clearInterval(interval);
		interval = window.setInterval("SwitchPictures()", speeds[speed]);
	}
}

function SetSpeedImage(pic){
	var img = null;
	if(pic == "on"){
		img = document.getElementById("speed" + speed);
		img.src = "Images/ExtralevelF/speed" + pic + ".gif";
	} else {
		var num = speed+1;
		img = document.getElementById("speed" + num);
		img.src = "Images/ExtralevelF/speed" + pic + ".gif";
	}
}