var browser = navigator.appName.toLowerCase();
var level = 0;
var score = 0;
var wrongAnswers = 0;
var maxHits;
var hits = 0;
var interval = null;
var wrongAnswers2 = 0;
var counter = 0;

function Start(){
	StartLevel();
	logField.value = "1";
	maxHits = parseInt(requirementField.value);
}

function init(){
	var img = document.getElementById("img3D");
	img.src = "Images/3DLevel4/" + images[0][0];
}

function LevelBack(){
	level -= 1;
	score -= 3;
	if(level <= 0){
		level = 0;
	}
	var img = document.getElementById("img3D");
	var rand = 0;
	img.src = imgDir + images[level][0];	
	if(score <= 0){
		score = 0;
	}
	var scoreF = document.getElementById("scoreField");
	scoreF.innerHTML = "Score: " + score;
	wrongAnswers += 1;
	window.clearInterval(interval);
	interval = window.setInterval("LevelBack()", 15000);
}

function DisplayImage(str){
	window.clearInterval(interval);
	if(level < 40 && hits <= maxHits){
		hits += 1;
		var scoreF = document.getElementById("scoreField");
		var img = document.getElementById("img3D");
		if(img.src.indexOf("-" + str) != -1){
			interval = window.setInterval("LevelBack()", 15000);
			level++;
			img.src = "Images/3DLevel4/" + images[level][0];
			score += 5;
			scoreF.innerHTML = "Score: " + score;
		} else {
			wrongAnswers += 1;
			interval = window.setInterval("LevelBack()", 15000);
			level -= 1;
			if(level == -1){
				level = 0;
			}
			if(score >= 5){
				score -= 5;
			} else {
				score = 0;
				scoreF.innerHTML = "Score: " + score;
			}
			img.src = imgDir + images[level][0];
			
			scoreF.innerHTML = "Score: " + score;
		}
	} else {
		logField.value = "1";
		StopLvl();
		pausecomp(400);
		Start();
		level = 0;
		score = 0;
		wrongAnswers = 0;
		hits = 0;
	}
	
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
		if(level >= 40){
			alertStr += "\n" + unlockedField.value;
			boolCompletedField.value = "true";
		}
		
		if(logField.value == "1"){
			alert(alertStr);
			logField.value = "0";
			pausecomp(400);
			document.Form1.submit();
			
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
	if(Key == "1" || Key == "2" || Key == "3" || Key == "4"){
		DisplayImage(Key);
	}
}
//Step 3: Hook the two up
document.onkeypress = processkey
