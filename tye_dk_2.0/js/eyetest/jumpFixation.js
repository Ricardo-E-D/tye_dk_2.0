
var images,
	maxTime = 15000,
	level = 0,
	imgDir,
	interval = null,
	counter = 0,
	score = 0,
	completed = false,
	jsDic = null,
	hits = 0,
	wrongAnswers = 0,
	currentPic = 0;

/*
	findTheFigure !!!!
*/

function LevelBack() {
	if (counter == 1) {
		level -= 1;
		currentPic = parseInt((Math.random() * 39));
		counter = 0;
	} else {
		counter = 1;
	}
	score -= 3;
	score = Math.max(score, 0);
	level = Math.max(level, 0);

	var img = document.getElementById("img3D");
	var rand = 0;
	if (img.src.indexOf(images[currentPic][rand]) != -1) {
		img.src = imgDir + images[currentPic][1];
	} else {
		img.src = imgDir + images[currentPic][0];
	}

	//document.getElementById("img3D").src = imgDir + images[level][0];
	
	eyeTestScreen.setAttrib("level", level);
	updateScore();
	
	window.clearInterval(interval);
	interval = window.setInterval(function () { LevelBack(); }, maxTime);
}

function updateScore() {
	$('#scoreField').html("Score: " + score);
	// if test already completed, and user continues to train resulting in sub-pass score....don't update it
	if (completed && score < eyeTestScreen.getScoreRequired())
		return;
	eyeTestScreen.setScore(score);
}

function DisplayImage(str) {
	window.clearInterval(interval);
	interval = window.setInterval(function () { LevelBack(); }, 15000);
	var img = document.getElementById("img3D");
	hits += 1;

	if (img.src.indexOf("-" + str) != -1) {
		level++;
		score += 5;
		var rand = parseInt((Math.random() * 2));
		var rand2 = parseInt((Math.random() * 39));
		currentPic = rand2;
		img.src = imgDir + images[rand2][rand];
	} else {
		wrongAnswers += 1;
		level = Math.max(--level, 0);
		if (score >= 5)
			score -= 5;
		else
			score = 0;

		var rand = parseInt((Math.random() * 2));
		img.src = imgDir + images[level][rand];
	}
		
	// !
	eyeTestScreen.setAttrib("level", level);
	updateScore(score);

	if (eyeTestScreen.getScoreRequired() <= score && !completed) {
		completed = true;
		eyeTestScreen.logEnd();
		alert(tye.dicValue('eyeTest_3dtestCompleted'));
	}
		
}
