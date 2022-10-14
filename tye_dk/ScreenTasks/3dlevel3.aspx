<%@ Page language="c#" Inherits="tye.ScreenTasks._3DLevel3" CodeFile="3DLevel3.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>3DLevel3 - Find tallene</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/3DLevel3.js"></script>
		<script language="javascript" src="Scripts/_dictionaryObject.js"></script>
	</HEAD>
	<body onload="init(); Start()" onunload="StopLvl('');">
		<form id="Form1" method="post" runat="server">
			<!--#include file="vars.htm"-->
			<div id="scoreField" style="FONT-WEIGHT:bold; TEXT-ALIGN:center">Score: 0</div>
			<table style="WIDTH: 100%; HEIGHT: 90%">
				<tr>
					<td valign="middle" align="center">
						<table align="center">
							<tr>
								<td colspan="2"><img src="Images/3DLevel3/spacer.png" id="img3D"></td>
							</tr>
							<tr>
								<td><img src="Images/3DLevel3/spacer.png" id="circlesImg"></td>
								<td>
									<img src="Images/3DLevel3/spacer.png" id="rw1"><br>
									<img src="Images/3DLevel3/spacer.png" id="rw2"><br>
									<img src="Images/3DLevel3/spacer.png" id="rw3"><br>
									<img src="Images/3DLevel3/spacer.png" id="rw4"><br>
									<img src="Images/3DLevel3/spacer.png" id="rw5"><br>
									<img src="Images/3DLevel3/spacer.png" id="rw6">
								</td>
							</tr>
							<tr>
								<td colspan="2"><span id="textGuide" runat="server"></span></td>
							</tr>

						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
