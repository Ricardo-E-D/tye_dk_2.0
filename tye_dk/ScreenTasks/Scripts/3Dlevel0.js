function Stop(){
	StopLevel();
	timeSpentField.value = time.GetInterval();
	var alertStr = unlockedField.value;
	alert(alertStr);
	document.Form1.submit();
//	pausecomp(1000);
//	window.close();
}
