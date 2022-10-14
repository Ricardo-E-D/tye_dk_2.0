<%@ Page language="c#" Inherits="tye.ScreenTasks._3DLevel4" CodeFile="3DLevel4.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>3D Level 4 - Find figuren</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/3DLevel4.js"></script>
	</HEAD>
	<body onload="init();Start()" bgcolor="black" onunload="StopLvl('');">
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
								<td style="TEXT-ALIGN: center" valign="middle"><img src="Images/exp.gif" usemap="#starmap" border="0" width="407" height="113"><br /><span id="textGuide" runat="server"></span></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
		<map name="starmap">
			<area coords="33,28,93,88" shape="RECT" onclick="DisplayImage('1');" style="CURSOR: pointer"
				href="#">
			<area coords="126,28,186,88" shape="RECT" onclick="DisplayImage('2');" href="#">
			<area coords="224,28,284,88" shape="RECT" onclick="DisplayImage('3');" href="#">
			<area coords="306,28,366,88" shape="RECT" onclick="DisplayImage('4');" href="#">
		</map>
	</body>
</HTML>
