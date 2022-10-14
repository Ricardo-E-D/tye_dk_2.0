
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
	wrongAnswers = 0;

/*
	findTheFigure !!!!
*/

function LevelBack() {
	level -= 1;
	score -= 3;
	score = Math.max(score, 0);
	level = Math.max(level, 0);

	var rand = parseInt((Math.random()*2));
	document.getElementById("img3D").src = imgDir + images[level][rand];
	
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
	} else {
		wrongAnswers += 1;
		level = Math.max(--level, 0);
		if (score >= 5)
			score -= 5;
		else
			score = 0;
	}
	var rand = parseInt((Math.random()*2));
	img.src = imgDir + images[level][rand];
		
	// !
	eyeTestScreen.setAttrib("level", level);
	updateScore(score);

	if (eyeTestScreen.getScoreRequired() <= score && !completed) {
		alert(tye.dicValue('eyeTest_Last3dtestCompleted'));
		completed = true;
		window.clearInterval(interval);
		eyeTestScreen.logEnd();
	}
	
}
