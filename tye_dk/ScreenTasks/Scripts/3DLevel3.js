var browser = navigator.appName.toLowerCase();
var level = 0;
var score = 0;
var curImg = 0;
var curImg2 = 0;
var nrOfOks = 0;
var wrongAnswers = 0;
var maxHits;
var hits = 0;
var interval = 0;
var wrongAnswers2 = 0;
var jsDic = null;

function Start(){
	StartLevel();
	logField.value = "1";
}

function init() {
	SwitchPictures();
}

function SwitchPictures(){
	window.clearInterval(interval);
	var img = document.getElementById("img3D");
	var circles = document.getElementById("circlesImg");
	var rand = parseInt((Math.random()*12)+1);
	
	var rand2 = parseInt((Math.random())*2);
	img.src = "Images/3DLevel3/still/" + rand + ".jpg";
	circles.src = "Images/3DLevel3/circles/" + imagesCircles[rand][rand2];
	
	var numbers = (imagesCircles[rand][rand2]);
	numbers = numbers.substring(0, numbers.indexOf("."));
	numbers = numbers.substring(numbers.lastIndexOf("-") + 1);
	if(numbers.length != 3) {
		alert("Critical error. Please report the following to TrainYourEyes.com:\n\"Find tallene\": img string length not equal to expected integer!");
	}
	//alert(numbers);
	// add numbers to dictionary object
	jsDic = new jsDictionary();
	for(var k = 0; k < numbers.length; k++) {
		var n = numbers.substr(k, 1);
		if(!jsDic.Exists(n)) {
			jsDic.Add(n, "0");
		}
	}
	
	for(var i = 1; i <= 6; i++){
		var image = document.getElementById("rw" + i);
		image.src = "Images/3DLevel3/rightwrong/" + rand + "-0.gif";
	}
	curImg = rand;
	curImg2 = rand2;
	
	interval = window.setInterval("LevelBack()", 15000);
}


function LevelBack(){
	level -= 1;
	score -= 3;
	if(level <= 0){
		level = 0;
	}
	SwitchPictures();
	var rand = parseInt((Math.random()*2));
	if(score <= 0){
		score = 0;
	}
	var scoreF = document.getElementById("scoreField");
	scoreF.innerHTML = "Score: " + score;
	wrongAnswers += 1;
	window.clearInterval(interval);
}


function StopLvl(){
		StopLevel();
		seconds = timeSpentField.value;
		scoreField.value = score;
		errorsField.value = wrongAnswers;
		errors = wrongAnswers;
		
		var hscore = GetStringAtIndex(hiscoreField.value, 0);
		var alertStr  = SplitString(alertEndField.value,level,score,seconds,errors);
		
		if(parseInt(hscore) < score){
			alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 1);
		} else {
			alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 2);
		}
		
		if(score >= 160){
			alertStr += "\n" + unlockedField.value;
			boolCompletedField.value = "true";
		}
		
		if(logField.value == "1"){
			alert(alertStr);
			logField.value = "0";
			pausecomp(400);
			document.Form1.submit();
			pausecomp(400);
		}	
}
//function DisplayImage(str){
//	alert("DisplayImage");
//	window.clearInterval(interval);
//	var scoreField = document.getElementById("scoreField");
//	var str = imagesCircles[curImg][curImg2];
//	var tempArr = str.split("-");
//	if(tempArr[2].indexOf(str) != -1){
//		interval = window.setInterval("LevelBack()", 15000);
//		var rand = parseInt((Math.random()*2));
//		level++;
//		img.src = "Images/3DLevel3/" + images[level][rand];
//		score += 5;
//		scoreField.innerHTML = "Score: " + score;
//	} else {
//		if(score <= 2){
//			level = 0;
//			score = 0;
//			scoreF.innerHTML = "Score: " + score;
//			init();
//		} else {
//			score -= 5;
//		}
//		scoreField.innerHTML = "Score: " + score;
//	}
//}

function CheckBox(str){
	window.clearInterval(interval);
	interval = window.setInterval("LevelBack()", 15000);
	var scoreF = document.getElementById("scoreField");
	var timeSpent = document.getElementById("timeSpent");
	var tempStr = imagesCircles[curImg][curImg2];
	var tempArr = tempStr.split("-");
	// eval if correct key has been pressed
	// old code kept (commented) for reference
	
	//if(tempArr[2].indexOf(str) != -1){
	if(jsDic.Exists(str)) { // jsDic populated in SwitchPictures()
		jsDic.Delete(str);
		var img = document.getElementById("rw" + str);
		img.src = "Images/3DLevel3/rightwrong/" + (curImg) + "-2.gif";
		score += 5;
		scoreF.innerHTML = "Score: " + score;
		nrOfOks += 1;
		timeSpent.value = score;
	} else {	
		var img = document.getElementById("rw" + str);
		img.src = "Images/3DLevel3/rightwrong/" + (curImg) + "-1.gif";
		if(score != 0){
			score -= 5;
		}
		wrongAnswers2 += 1;
		wrongAnswers += 1;
		scoreF.innerHTML = "Score: " + score;
		timeSpent.value = score;
	}
	if(wrongAnswers2 == 2){
		wrongAnswers2 = 0;
		nrOfOks = 0;
		
		SwitchPictures();
	}
	if(nrOfOks == 3){
		nrOfOks = 0;
		wrongAnswers2 = 0;
		level++;
		if(level >= 30){
			level = 0;
			StopLevel();
			seconds = timeSpentField.value;
			scoreField.value = score;
			errorsField.value = wrongAnswers;
			errors = wrongAnswers;
			
			var hscore = GetStringAtIndex(hiscoreField.value, 0);
			var alertStr  = SplitString(alertEndField.value,level,score,seconds,errors);
			if(parseInt(hscore) < score){
				alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 1);
			} else {
				alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 2);
			}
			if(level == 30){
				alertStr += "\n" + unlockedField.value;
			}
			alert(alertStr);
			document.Form1.submit();
		}
		SwitchPictures();
	}
}

if(browser.indexOf("netscape") != -1) document.captureEvents(Event.KEYDOWN);
//Step 2: Create the reacting function
function processkey(e){
	var Key
	
	if (browser.indexOf("netscape") != -1){
		Key = String.fromCharCode(e.which);
	} else{
        Key = String.fromCharCode(window.event.keyCode);
        
	}
	if(Key == "1" || Key == "2" || Key == "3" || Key == "4" || Key == "5" || Key == "6"){
		CheckBox(Key);
	}
}
//Step 3: Hook the two up
document.onkeypress = processkey
