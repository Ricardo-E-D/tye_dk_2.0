<%@ Page language="c#" Inherits="tye.ScreenTasks.Circel" CodeFile="circel.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Circel</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/Classes/Animate.js"></script>
		<script language="javascript" src="Scripts/Classes/Bezier.js"></script>
		<script language="javascript" src="Scripts/circel.js"></script>
	</HEAD>
	<body bgcolor="black" onload="AnimateCurve()">
		<form id="Form1" method="post" runat="server">
									<div style="WIDTH: 600px; POSITION: relative; HEIGHT: 600px">
										<img src="Images/Level1A/eye.jpg" id="eye" style="LEFT: 0px; POSITION: absolute; TOP: 0px" width="54" height="36">
										<img src="Images/Level1A/aim.gif" id="aim" style="LEFT: 250px; POSITION: absolute; TOP: 250px" width="21" height="21">
									</div>
			<center><input type="button" value="Stop" onclick="Stop()"></center>
		</form>
	</body>
</HTML>
