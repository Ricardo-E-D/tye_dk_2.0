function Stop(){
	var btn = document.getElementById("btn");
	if(btn.value == "Start"){
		btn.value = "Stop";
		StartLevel(); 
	} else {
		StopLevel();
		timeSpentField.value = time.GetInterval();
		seconds = timeSpentField.value;
		var alertStr  = SplitString(alertEndField.value,level,score,seconds,errors);
	
		alert(alertStr);
		document.Form1.submit();
	}
}
