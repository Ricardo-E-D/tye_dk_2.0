function Start(elem){
	if(elem.value == "Start"){
		StartLevel();
		elem.value = "Stop";
	} else if(elem.value == "Stop"){
		StopLevel();
		seconds = timeSpentField.value;
		alert(SplitString(alertEndField.value, level, score, seconds,errors) + "\r");
		document.Form1.submit();		
	}
}