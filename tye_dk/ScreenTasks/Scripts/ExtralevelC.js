var charArr = new Array("a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z");
var checked = new Array("false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false","false");

var points = 1800;

function GetMilliseconds(){
	return time.GetInterval();
}

function SetCharacter(ok, ch, letter){
	var HTMLelem = document.getElementById("right" + ch.toLowerCase());
	if (ok == "True") {
		
		if(Check(ch)){
		HTMLelem.style.backgroundColor = "#444";
		HTMLelem.style.color = "white";
		colorizeLetters(letter);
			if(ch.toLowerCase() == "z"){
				StopLevel();
				var interval = time.GetInterval();

				seconds = interval;
				
				scoreField.value = (points - (parseInt(interval)*5));
				score = (points - (parseInt(interval)*5));
				if(parseInt(score) < 0) {
					score = 0;
					scoreField.value = 0;
				}
				//if(scoreField.value < 0)
				//	scoreField.value = scoreField.value*-1;
					
				//if(score < 0)
				//	score = score*-1;
				
				var hscore = GetStringAtIndex(hiscoreField.value, 0);
				var alertStr  = SplitString(alertEndField.value,level,score,seconds, errors);
				if(parseInt(hscore) < score){
					alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 1);
				} else {
					alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 2);
				}
				
				alert(alertStr);
				
				document.Form1.submit();
			}
		}
	} else {
		alert(alertStartField.value);
	}
}

var intLastLetterIndex = 0;
var blnColorize = false;

function toggleColorize(obj) {
	blnColorize = !blnColorize;
	obj.style.backgroundColor = (blnColorize ? "#003765" : "#000000");
	obj.style.color = (blnColorize ? "#666666" : "#ffffff");
	obj.innerHTML = (blnColorize ? "Color on" : "Color off");
	document.getElementById("hideColor").value = (blnColorize ? "on" : "off");
	if(blnColorize)
		return;
		
	var intC = 0;
	for(var i = 0; i < intLastLetterIndex + 1; i++) {
		if(document.getElementById("Echar" + i)) {
			document.getElementById("Echar" + i).style.backgroundColor = "#000000";
			document.getElementById("Echar" + i).style.color = "#ffffff";
		}
	}
	//intLastLetterIndex = 0;
}

function colorizeLetters(letter) {
	if(!blnColorize || !letter)
		return;
	if(letter.id.indexOf("Echar") != 0)
		return;
	var intNo = parseInt(letter.id.replace('Echar', ''));
	if(intNo < 1)
		return;
	
	for(var i = 0; i < intNo + 1; i++) {
		var o = document.getElementById("Echar" + i);
		if(!o)
			continue;
		o.style.backgroundColor = "#003765";
		o.style.color = "#666666";
	}
	intLastLetterIndex = intNo;
}

function Check(ch){
	for(var i = 0; i < charArr.length; i++)
	{
		if(charArr[i] == ch.toLowerCase())
		{
			var index = i;
			if(charArr[i] != "a"){
				if(checked[i-1] == "true" && (checked[i] == "false"))
				{
					checked[i] = "true";
					return true;
				} 
				else 
				{
					return false;
				}
			} 
			else
			{
				if(checked[i] != "true")
				{
					checked[i] = "true";
					return true;
				} 
				
			}
		}
	}
	return false;
}