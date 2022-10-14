<%@ Page language="c#" Inherits="tye.ScreenTasks._3DLevel6" CodeFile="3DLevel6.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>3D Level 6 - Blomsten negativ</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/3DLevels.js"></script>
	</HEAD>
	<body onload="init();Start()" onunload="StopLvl();" onbeforeunload="StopLvl();" bgcolor="black">
		<input type="hidden" value="negativ" name="test" id="test">
		<form id="Form1" method="post" runat="server">
			<!--#include file="vars.htm"-->
			<div id="scoreField" style="FONT-WEIGHT:bold; TEXT-ALIGN:center">Score: 0</div>
			<table style="WIDTH: 100%; HEIGHT: 100%; TEXT-ALIGN: center">
				<tr>
					<td valign="middle" align="center">
						<table>
							<tr>
								<td valign="middle"><img src="" id="img3D"></td>
							</tr>
							<tr>
								<td style="TEXT-ALIGN: right" valign="middle"><span id="textGuide" runat="server"></span><img src="Images/3DLevel1/exp.gif" usemap="#starmap" border="0" width="134" height="130"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
		<map name="starmap">
			<area coords="52,26,75,49" shape="RECT" onclick="DisplayImage('1');" style="CURSOR: pointer"
				href="#">
			<area coords="84,43,107,66" shape="RECT" onclick="DisplayImage('2');" href="#">
			<area coords="75,76,98,99" shape="RECT" onclick="DisplayImage('3');" href="#">
			<area coords="48,82,71,105" shape="RECT" onclick="DisplayImage('4');" href="#">
			<area coords="32,55,55,78" shape="RECT" onclick="DisplayImage('5');" href="#">
			<area coords="61,55,84,78" shape="RECT" onclick="DisplayImage('6');" href="#">
		</map>
	</body>
</HTML>
