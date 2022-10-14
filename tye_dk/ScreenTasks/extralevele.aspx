<%@ Page language="c#" Inherits="tye.ScreenTasks.ExtralevelE" CodeFile="ExtralevelE.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Extralevel E</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/Timer.js"></script>
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/ExtralevelE.js"></script>
		<script type="text/javascript" src="Scripts/_postback.js"></script>
	</HEAD>
	<body bgcolor="black" onload="StartLevel();SetPictures()">
		<form id="Form1" method="post" runat="server">
			<input type="hidden" runat="server" id="level" />
			<!--#include file="vars.htm"-->
			<div ID="letterMenu" Runat="server" class="LabyrintMenu" style="WIDTH: 100%; TEXT-ALIGN: center"></div>
			<br>
			<br>
			<table align="center">
				<tr>
					<td width="230" valign="top"><div style="position: relative;">
						<div style="z-index: 100; position:absolute"><img src="" border="0" id="img1"></div>
						<div style="background-image: url(Images/ExtralevelE/dither.gif); width: 220px; height:220px; position:absolute; z-index: 1000">&nbsp;</div>
						</div></td>
					<td  width="230"><img src="" border="0" id="img2"></td>
					<td  width="230" valign="top"><div style="position: relative;">
						<div style="z-index: 100; position:absolute"><img src="" border="0" id="img3"></div>
						<div style="background-image: url(Images/ExtralevelE/dither.gif); width: 220px; height:220px; position:absolute; z-index: 1000">&nbsp;</div>
						</div></td>
				</tr>	
				<tr>
					<td colspan="3" align="center">
					<br>
					<span id="slowerDiv" runat="server"></span><img src="Images/ExtralevelF/speedleftbtn.gif" onclick="SpeedDown()" style="CURSOR: pointer"
						width="35" height="20"><img src="Images/ExtralevelF/speedoff.gif" id="speed1" width="16" height="20"><img src="Images/ExtralevelF/speedoff.gif" id="speed2" width="16" height="20"><img src="Images/ExtralevelF/speedoff.gif" id="speed3" width="16" height="20"><img src="Images/ExtralevelF/speedoff.gif" id="speed4" width="16" height="20"><img src="Images/ExtralevelF/speedoff.gif" id="speed5" width="16" height="20"><img src="Images/ExtralevelF/speedrightbtn.gif" onclick="SpeedUp()" style="CURSOR: pointer"
						width="37" height="20"> <span id="fasterDiv" runat="server"></span></TD>
				</tr>
				<tr>
					<td colspan="3" align="center"></td>
				</tr>
			</table><br><br />
			<!--<center><input type="button" value="Start" onclick="Stop()" id="btn"></center>-->
		</form>
	</body>
</HTML>
