
var images,
	maxTime = 30000,
	level = 0,
	imgDir,
	interval = null,
	counter = 0,
	score = 0,
	completed = false,
	jsDic = null,
	nrOfOks = 0,
	wrongAnswers = 0,
	wrongAnswers2 = 0;

/*
	findTheNumbers !!!!
*/

function SwitchPictures() {
	window.clearInterval(interval);
	var img = document.getElementById("img3D");
	var circles = document.getElementById("circlesImg");
	var rand = parseInt((Math.random() * 12) + 1);
	var rand2 = parseInt((Math.random()) * 2);

	img.src = "/img/eyetest/findTheNumbers/still/" + rand + ".jpg";
	circles.src = "/img/eyetest/findTheNumbers/circles/" + images[rand][rand2];

	var numbers = (images[rand][rand2]);
	numbers = numbers.substring(0, numbers.indexOf("."));
	numbers = numbers.substring(numbers.lastIndexOf("-") + 1);
	if (numbers.length != 3) {
		alert("Critical error. Please report the following to TrainYourEyes.com:\n\"Find tallene\": img string length not equal to expected integer!");
	}
	
	// add numbers to dictionary object
	jsDic = new jsDictionary();
	for (var k = 0; k < numbers.length; k++) {
		var n = numbers.substr(k, 1);
		if (!jsDic.Exists(n)) {
			jsDic.Add(n, "0");
		}
	}

	for (var i = 1; i <= 6; i++) {
		var image = document.getElementById("rw" + i);
		image.src = "/img/eyetest/findTheNumbers/rightwrong/" + rand + "-0.gif";
	}
	curImg = rand;
	curImg2 = rand2;

	interval = window.setInterval(function () { LevelBack(); }, 15000);
}


function LevelBack() {
	if (counter == 1) {
		level -= 1;
		counter = 0;
	} else
		counter = 1;
	
	score -= 3;
	score = Math.max(score, 0);
	level = Math.max(level, 0);
	/*if (level <= 0) level = 0;
	if (score <= 0) score = 0;*/

	SwitchPictures();
	eyeTestScreen.setAttrib("level", level);
	updateScore();
	
	window.clearInterval(interval);
	interval = window.setInterval(function () { LevelBack(); }, maxTime);
}



function CheckBox(str) {
	window.clearInterval(interval);
	interval = window.setInterval(function () { LevelBack(); }, maxTime);
	var scoreF = document.getElementById("scoreField");
	var tempStr = images[curImg][curImg2];
	var tempArr = tempStr.split("-");
	// eval if correct key has been pressed
	// old code kept (commented) for reference

	//if(tempArr[2].indexOf(str) != -1){
	if (jsDic.Exists(str)) { // jsDic populated in SwitchPictures()
		jsDic.Delete(str);
		var img = document.getElementById("rw" + str);
		img.src = "/img/eyetest/findtheNumbers/rightwrong/" + (curImg) + "-2.gif";
		score += 5;
		scoreF.innerHTML = "Score: " + score;
		nrOfOks += 1;
	} else {
		var img = document.getElementById("rw" + str);
		img.src = "/img/eyetest/findtheNumbers/rightwrong/" + (curImg) + "-1.gif";
		if (score != 0) {
			score -= 5;
		}
		wrongAnswers2 += 1;
		wrongAnswers += 1;
		updateScore();
	}
	if (wrongAnswers2 == 2) {
		wrongAnswers2 = 0;
		nrOfOks = 0;
		SwitchPictures();
	}
	if (nrOfOks == 3) {
		nrOfOks = 0;
		wrongAnswers2 = 0;
		level++;
		
		if (eyeTestScreen.getScoreRequired() <= score && !completed) {
			alert(tye.dicValue('eyeTest_3dtestCompleted'));
			completed = true;
		}
		/*
		if (level >= 30) {
			level = 0;
			StopLevel();
			scoreField.value = score;
			errorsField.value = wrongAnswers;
			errors = wrongAnswers;

			var hscore = GetStringAtIndex(hiscoreField.value, 0);
			var alertStr = SplitString(alertEndField.value, level, score, seconds, errors);
			if (parseInt(hscore) < score) {
				alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 1);
			} else {
				alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 2);
			}
			if (level == 30) {
				alertStr += "\n" + unlockedField.value;
			}
			alert(alertStr);
			document.Form1.submit();
		}*/
		SwitchPictures();
	}
	updateScore();
}

function updateScore() {
	$('#scoreField').html("Score: " + score);
	
	// if test already completed, and user continues to train resulting in sub-pass score....don't update it
	if (completed && score < eyeTestScreen.getScoreRequired())
		return;
	
	eyeTestScreen.setScore(score);
}

function DisplayImage(str) {
	// method used in 3DLevel1, 3DLevel2, 3DLevel6, 3DLevel7
	window.clearInterval(interval);

//	if (level <= 40)
//		return;

	interval = window.setInterval(function () { LevelBack(); }, maxTime);

	var img = document.getElementById("img3D");
	var rand = parseInt((Math.random() * 2));
	if (img.src.indexOf("-" + str) != -1) {
		level++;
		score += 5;
	} else {
		level = Math.max(level - 1, 0);
		score = (score <= 2 ? 0 : score - 5);
	}
	img.src = imgDir + images[level][rand];
	/*
	if (level < 40 && numberOrTries <= 50) {
		numberOrTries++;
		var img = document.getElementById("img3D");
		var rand = parseInt((Math.random() * 2));
		if (img.src.indexOf("-" + str) != -1) {
			level++;
			img.src = imgDir + images[level][rand];
			score += 5;
			scoreField.innerHTML = "Score: " + score;
		} else {
			level = Math.max(level - 1, 0);
			score = (score <= 2 ? 0 : score - 5);
			img.src = imgDir + images[level][rand];
			scoreField.innerHTML = "Score: " + score;
		}
	} else {
		eyeTestScreen.stop();
		level = 0;
		score = 0;
		numberOrTries = 0;
	}
	*/

	eyeTestScreen.setAttrib("level", level);
	updateScore(score);

	if (eyeTestScreen.getScoreRequired() <= score && !completed) {
		alert(tye.dicValue('eyeTest_3dtestCompleted'));
		completed = true;
		eyeTestScreen.logEnd();
	}
	
}
