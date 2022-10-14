<%@ Page language="c#" Inherits="tye.ScreenTasks.Test" CodeFile="Test.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>Test</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <script language="javascript">
var browser = navigator.appName.toLowerCase();
if(browser.indexOf("netscape") != -1) document.captureEvents(Event.MOUSEMOVE);
if(browser.indexOf("netscape") != -1) document.captureEvents(Event.CLICK);
if(browser.indexOf("netscape") != -1) document.captureEvents(Event.DOUBLECLICK);


var coords = new Array();
function setCoordinate(e){
	var posx = 0;
	var posy = 0;
	if (!e) var e = window.event;
	if (e.pageX || e.pageY)
	{
		posx = e.pageX;
		posy = e.pageY;
	}
	else if (e.clientX || e.clientY)
	{
		posx = e.clientX + document.body.scrollLeft;
		posy = e.clientY + document.body.scrollTop;
		
	}
	coords.push(posx);
	coords.push(posy);
}

function endCoordinate(e){
	var elem = document.getElementById("letterMenu");
	for(var i = 0; i<= coords.length; i++){
		elem.innerHTML += coords[i] + ",";
	}
}
//Step 3: Hook the two up

window.document.onclick = setCoordinate;
window.document.onkeypress = endCoordinate;

    </script>
  </head>
  <body leftmargin="0" topmargin="0" style="cursor: crosshair">
	
    <form id="Form1" method="post" runat="server">
		<img src="Images/ExtralevelD/map3a.png" id="map" style="position:absolute;">
		<div id="letterMenu" style="position: absolute; top: 400px;width:400px"></div>
     </form>
	
  </body>
</html>
