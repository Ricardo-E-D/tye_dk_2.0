var charArr = new Array("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"),
	checked = new Array("false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false", "false"),
	dtStart = new DateTime(),
	intervalTimeSpent = null,
	points = 1800,
	intLastLetterIndex = 0,
	blnColorize = false;

function GetMilliseconds() {
	return time.GetInterval();
}

// ok = "True" or "False" (string)
// ch = "a" through "z" (string)
// letter = this (the span element)
function SetCharacter(ok, ch, letter) {
	if (!eyeTestScreen.IsRunning) {
		eyeTestScreen.start();
		dtStart = new DateTime();
		intervalTimeSpent = setInterval(function () {
			var dtEnd = new DateTime();
			$('div.eyeTestTime').html(dtEnd.subtractDate(dtStart).toTimeString('mm:ss'));
			var totalSeconds = parseInt(dtEnd.subtractDate(dtStart).totalSeconds(), 10);
			$('div.score').html((points - (totalSeconds * 5)));
		}, 1000);
	}

	if (ok == "True" && Check(ch)) {
		var HTMLelem = document.getElementById("right" + ch.toLowerCase());
		HTMLelem.style.backgroundColor = "#444";
		HTMLelem.style.color = "white";
		colorizeLetters(letter);

		var objL = $(letter), i = 0, delay = 200, oncolor = '#00ff00', offcolor = '#ffffff';
		setTimeout(function () { objL.css('color', oncolor); }, delay * i++);
		setTimeout(function () { objL.css('color', offcolor); }, delay * i++);
		setTimeout(function () { objL.css('color', oncolor); }, delay * i++);
		setTimeout(function () { objL.css('color', offcolor); }, delay * i++);

		if (ch.toLowerCase() == "z") {
			var dtEnd = new DateTime();
			var totalSeconds = parseInt(dtEnd.subtractDate(dtStart).totalSeconds(), 10);

			score = (points - (totalSeconds * 5));
			$('#hidScore').val(score);
			clearInterval(intervalTimeSpent);
			eyeTestScreen.stop();

			//$('div.eyeTestTime').html($('div.eyeTestTime').html() + " - " + score);

			alert("Points: " + score);

//			var hscore = GetStringAtIndex(hiscoreField.value, 0);
//			var alertStr = SplitString(alertEndField.value, level, score, seconds, errors);
//			if (parseInt(hscore) < score) {
//				alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 1);
//			} else {
//				alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 2);
//			}
//			alert(alertStr);
		}
	} else {
		var objL = $(letter), i = 0, delay = 200, oncolor = '#ff0000', offcolor = '#ffffff';
		setTimeout(function () { objL.css('color', oncolor); }, delay * i++);
		setTimeout(function () { objL.css('color', offcolor); }, delay * i++);
		setTimeout(function () { objL.css('color', oncolor); }, delay * i++);
		setTimeout(function () { objL.css('color', offcolor); }, delay * i++);
		setTimeout(function () { objL.css('color', oncolor); }, delay * i++);
		setTimeout(function () { objL.css('color', offcolor); }, delay * i++);
		setTimeout(function () { objL.css('color', oncolor); }, delay * i++);
		setTimeout(function () { objL.css('color', offcolor); }, delay * i++);
	}
}

function toggleColorize(obj) {
	blnColorize = !blnColorize;
	obj.style.backgroundColor = (blnColorize ? "#003765" : "#000000");
	obj.style.color = (blnColorize ? "#666666" : "#ffffff");
	obj.innerHTML = (blnColorize ? "Color on" : "Color off");
	document.getElementById("hideColor").value = (blnColorize ? "on" : "off");
	if (blnColorize)
		return;

	var intC = 0;
	for (var i = 0; i < intLastLetterIndex + 1; i++) {
		if (document.getElementById("Echar" + i)) {
			document.getElementById("Echar" + i).style.backgroundColor = "#000000";
			document.getElementById("Echar" + i).style.color = "#ffffff";
		}
	}
}

function colorizeLetters(letter) {
	if (!blnColorize || !letter)
		return;
	if (letter.id.indexOf("Echar") != 0)
		return;
	var intNo = parseInt(letter.id.replace('Echar', ''));
	if (intNo < 1)
		return;
	
	for (var i = 0; i < intNo + 1; i++) {
		var o = document.getElementById("Echar" + i);
		if (!o)
			continue;
		$(o).css({ backgroundColor: "#003765", color: "#666666" });
	}
	intLastLetterIndex = intNo;
}

function Check(ch) {
	for (var i = 0; i < charArr.length; i++) {
		if (charArr[i] == ch.toLowerCase()) {
			var index = i;
			if (charArr[i] != "a") {
				if (checked[i - 1] == "true" && (checked[i] == "false")) {
					checked[i] = "true";
					return true;
				}
				else {
					return false;
				}
			}
			else {
				if (checked[i] != "true") {
					checked[i] = "true";
					return true;
				}

			}
		}
	}
	return false;
}
 