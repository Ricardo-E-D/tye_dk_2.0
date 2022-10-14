var blnMakeParentPostback = true;
function closewindow() {
  if(!blnMakeParentPostback)
	return;
	
  if (window.dialogArguments) {
    // Calling the method (given as argument) to cause the postback
    window.dialogArguments.doPostBack();
  }
  else{
    opener.doPostBack();
  }
    window.close();
}

window.onunload = closewindow;