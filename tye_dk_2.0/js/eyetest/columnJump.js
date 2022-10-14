var possible2 = new Array("1-3", "2-4", "2-5", "2-6", "2-7", "2-8", "2-9", "2-10", "2-11", "3-5", "3-6", "3-7", "3-8", "3-9", "3-10", "3-11", "4-6", "4-7", "4-8", "4-9", "4-10", "4-11", "5-7", "5-8", "5-9", "5-10", "5-11", "6-8", "6-9", "6-10", "6-11", "7-9", "7-10", "7-11", "8-10", "8-11", "9-11");
var possible3 = new Array("2-4-6", "2-4-7", "2-4-8", "2-4-9", "2-4-10", "2-4-11", "2-4-5", "2-4-5");
var cols2Possible = new Array();
var cols3Possible = new Array();
var cols4Possible = new Array();
var timeSpent = 0;
var count = 2;
var type = "cols";
var startPoints = 50;
var points = 1;
var pointDecreaser = 1;
var dtStart = new DateTime();

function MarkCols() {
	ResetCols();
	
	var elem = $('#startBtn');
	if ($(elem).val() == "Start") {
		//StartLevel();
		$(elem).val('Stop').removeClass('positivesmall').addClass('negativesmall');
		eyeTestScreen.start();
		dtStart = new DateTime();

		var arr = GetRandomNumbers(count);
		for (var i = 0; i < arr.length; i++) {
			var img = document.getElementById("col" + arr[i]);
			img.src = "/img/eyetest/columnJump/arrowdown.png";
		}
		if (type == "random") {
			var rand = parseInt(Math.random() * 2);
			var arr = GetRandomNumbers((2 + rand));
			for (var i = 0; i < arr.length; i++) {
				var img = document.getElementById("col" + arr[i]);
				img.src = "/img/eyetest/columnJump/arrowdown.png";
			}
			var rand = parseInt(Math.random() * 3);
			var arr = GetRandomNumbers((2 + rand));
			for (var i = 0; i < arr.length; i++) {
				var img = document.getElementById("row" + arr[i]);
				img.src = "/img/eyetest/columnJump/arrowright.png";
			}
		}
		updatePointsAndTime(); // interval to update osd
	} else {
		var dtEnd = new DateTime();
		var totalSeconds = parseInt(dtEnd.subtractDate(dtStart).totalSeconds(), 10);

		/*if ((totalSeconds - 10) > 0) {
			startPoints -= (totalSeconds - 10)
		} else {
			startPoints += (10 - totalSeconds);
		}*/
		ResetCols();

		eyeTestScreen.setScore(points);
		score = Math.max(0, startPoints);

		eyeTestScreen.stop();
		eyeTestScreen.IsRunning = false;

		dtStart = new DateTime();
		if (type != "random") {
			eyeTestScreen.setLevel(count);
		} else {
			eyeTestScreen.setLevel('random');
		}

		$(elem).val('Start').addClass('positivesmall').removeClass('negativesmall');

		alert("Points: " + points);
		$('div.eyeTestTime').html('00:00');
		$('div.score').html(0);
	
		
//		var hscore = GetStringAtIndex(hiscoreField.value, 0);
//		var alertStr = SplitString(alertEndField.value, level, score, seconds, errors);
//		if (parseInt(hscore) < score) {
//			alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 1);
//		} else {
//			alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 2);
//		}
		//alert(SplitString(alertStr));

	}
}
var points;
function updatePointsAndTime() {
	var dtEnd = new DateTime();
	var totalSeconds = parseInt(dtEnd.subtractDate(dtStart).totalSeconds(), 10);
	points = startPoints;

	if ((totalSeconds - 10) > 0) {
		points -= (totalSeconds - 10)
	} else {
		points += (10 - totalSeconds);
	}
	points = Math.max(0, points);

	$('div.eyeTestTime').html(dtEnd.subtractDate(dtStart).toTimeString('mm:ss'));
	$('div.score').html(points);
	
	var elem = $('#startBtn');
	if ($(elem).val() == "Stop") {
		setTimeout(function () {
			updatePointsAndTime();
		}, 1000);
	}
}

function SetCols(cols, typ) {
	if (eyeTestScreen.IsRunning) {
		MarkCols();
	} else {
		eyeTestScreen.stop();
	}

	$('div.eyeTestScreenTopMenu a').removeClass('negativesmall').addClass('positivesmall');

	count = cols;
	type = typ;
	//var level = document.getElementById("chosenLevel");
	//var button = document.getElementById("startBtn");
	//button.value = "Start";
	if (type == "cols") {
		$('#cols' + cols).addClass('negativesmall');
		eyeTestScreen.setLevel(count);
		if (cols == 2) {
			startPoints = 50;
		} else if (cols == 3) {
			startPoints = 70;
		} else if (cols == 4) {
			startPoints = 90;
		}
	} else {
		$('#colsrandom').addClass('negativesmall');
		eyeTestScreen.setLevel("random");
		startPoints = 100;
	}
}

function GeneratePossibleCols() {
	//logField = document.getElementById("log");
	cols2Possible.push("1-3");
	for (var i = 2; i <= 9; i++) {
		for (var j = i + 2; j <= 11; j++) {
			cols2Possible.push(i + "-" + j);
		}
	}

	for (var i = 2; i <= 9; i++) {
		for (var j = i + 2; j <= 11; j++) {
			for (var h = j + 2; h <= 11; h++) {
				cols3Possible.push(i + "-" + j + "-" + h);
			}
		}
	}

	for (var i = 2; i <= 9; i++) {
		for (var j = i + 2; j <= 11; j++) {
			for (var h = j + 2; h <= 11; h++) {
				for (var k = h + 2; k <= 11; k++) {
					cols4Possible.push(i + "-" + j + "-" + h + "-" + k);
				}
			}
		}
	}
	
	
	var level = $('#hidAttribValue').val();
	if (level) {
		if (level == "random") {
			SetCols(2, "random");
		} else {
			if (level != "0") {
				SetCols(level, "cols");
			} else {
				SetCols(2, "cols");
			}
		}
	} else {
		SetCols(2, "cols");
	}
}

function GetRandomNumbers() {
	var randNum = parseInt(Math.random() * eval("cols" + count + "Possible.length"));
	var qqq = "cols" + count + "Possible.length";
	var str = eval("cols" + count + "Possible[" + randNum + "]");
	return str.split("-");
}

function ResetCols() {
	for (var i = 1; i <= 12; i++) {
		var elem = document.getElementById("col" + i);
		elem.src = "/img/eyetest/columnJump/spacer.png";
		elem = document.getElementById("row" + i);
		elem.src = "/img/eyetest/columnJump/spacer.png";

		elem.style.top = (15 + (36 * i)) + "px";
	}
}

/*
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
*/