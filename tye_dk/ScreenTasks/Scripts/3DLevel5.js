var browser = navigator.appName.toLowerCase();
var maxTime = 15;
var level = 0;
var score = 0;
var imgDir;
var wrongAnswers = 0;
var maxHits;
var hits = 0;
var interval = null;
var wrongAnswers2 = 0;
var currentPic = 0;
var counter = 0;

function Start(){	
	StartLevel();
	maxHits = parseInt(requirementField.value);
}

function init(){
	var img = document.getElementById("img3D");
	var rand = parseInt((Math.random()*2));
	img.src = imgDir + images[0][rand];
	currentPic = 0;
}

function LevelBack(){
	if(counter == 1){
		level -= 1;
		currentPic = parseInt((Math.random()*39));
		counter = 0;
	} else {
		counter = 1;
	}
	score -= 3;
	if(level <= 0){
		level = 0;
	}
	var img = document.getElementById("img3D");
	var rand = 0;
	if(img.src.indexOf(images[currentPic][rand]) != -1){
		img.src = imgDir + images[currentPic][1];	
	} else {
		img.src = imgDir + images[currentPic][0];	
	}
	
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
	if(hits == 0){
		logField.value = "1";
	}
	if(level < 40 && hits <= maxHits){	
		hits += 1;
		var scoreF = document.getElementById("scoreField");
		var img = document.getElementById("img3D");
		if(img.src.indexOf("-" + str) != -1) {
			interval = window.setInterval("LevelBack()", 15000);
			var rand = parseInt((Math.random()*2));
			level++;
			var rand2 = parseInt((Math.random()*39));
			currentPic = rand2;
			img.src = imgDir + images[rand2][rand];
			score += 5;
			scoreF.innerHTML = "Score: " + score;
		} else {
			
			
			wrongAnswers += 1;
			interval = window.setInterval("LevelBack()", 15000);
			level -= 1;
			if(level == -1){
				level = 0;
			}
			if(score <= 2){
				score = 0;
				scoreF.innerHTML = "Score: " + score;
				
			} else {
				score -= 5;
			}
			var rand = parseInt((Math.random()*2));
			img.src = imgDir + images[level][rand];
			scoreF.innerHTML = "Score: " + score;
			//timeSpent.value = score;
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
		if(level >= 39){
			alertStr += "\n" + unlockedField.value;
			boolCompletedField.value = "true";
		}
		
		if(logField.value == "1"){
			alert(alertStr);
			logField.value = "0";
			document.Form1.submit();
			pausecomp(400);
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
		DisplayImage(Key);
	}
}
//Step 3: Hook the two up
document.onkeypress = processkey
