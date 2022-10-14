var speed = 0;
var speeds = new Array(6000,5000,4000,3000,2000,1000);
var images = new Array();
images[0] = new Array("1-0.gif", "1-1.gif");
images[1] = new Array("2-0.gif", "2-1.gif");
images[2] = new Array("3-0.gif", "3-1.gif");
images[3] = new Array("4-0.gif", "4-1.gif");
images[4] = new Array("5-0.gif", "5-1.gif", "5-2.gif");
var interval;

var img1 = null;
var img2 = null;
var img3 = null;

function Stop(){
	var btn = document.getElementById("btn");
	if(btn.value == "Start"){
		btn.value = "Stop";
		StartLevel(); 
		//SwitchPictures();
		StartMotion();
	} else {
		StopLevel();
		timeSpentField.value = time.GetInterval();		
	
		var level = document.getElementById("level").value;
		if(level == 5) {
			alert(alertEndField.value);
			document.Form1.submit();	
		}
		else {
			document.Form1.submit();	
		}
	}
}

function SwitchPictures(){
	img3.src = img2.src;
	img2.src = img1.src;	

	var level = document.getElementById("level").value;
	img1.src = "Images/ExtralevelE/" + this.GetRandomPicture(level);
}

function GetRandomPicture(level){
		var index = parseInt(Math.random()*level)
		var index2;
		if(index != 4){
			index2 = parseInt(Math.random()*2);
		} else {
			index2 = parseInt(Math.random()*3);
		}
		
		return images[index][index2];
}

function SetPictures(){
	img1 = document.getElementById("img1");
	img2 = document.getElementById("img2");
	img3 = document.getElementById("img3");

	var level = document.getElementById("level").value;
	if(level == parseInt(0)){
		level = 5;
	} 

	img1.src = "Images/ExtralevelE/" + this.GetRandomPicture(level);
	img2.src = "Images/ExtralevelE/" + this.GetRandomPicture(level);
	img3.src = "Images/ExtralevelE/" + this.GetRandomPicture(level);
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
	else if(speed <= 1) {
		//alert("Speed før minus: "+speed);
		speed -= 1;
		SetSpeedImage("off");
		clearInterval(interval);
		StopLevel();
		timeSpentField.value = time.GetInterval();
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