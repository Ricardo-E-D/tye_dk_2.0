<%@ Page language="c#" Inherits="tye.ScreenTasks.ExtralevelB" CodeFile="ExtralevelB.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ExtralevelB</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/ExtralevelB.js"></script>
		<script type="text/javascript">
			var blnCall = true;
			function setCall(obj) {
				if(obj.value.toLowerCase() != "start") {
					blnCall = false;
				}
			}
			function closingWindow() {
				if(blnCall)
					MarkCols(document.getElementById("startBtn"));
			}
		</script>
	</HEAD>
	<body onunload="closingWindow();">
		<form id="Form1" method="post" runat="server">
			<!--#include file="vars.htm"-->
			<div ID="labyrintMenu" Runat="server" class="LabyrintMenu" style="WIDTH: 100%; TEXT-ALIGN: center">
				<a href="#" onclick="ResetCols(); SetCols(2, 'cols')">2</a> | <a href="#" onclick="ResetCols(); SetCols(3, 'cols')">
					3</a> | <a href="#" onclick="ResetCols(); SetCols(4, 'cols')">4</a> | <a href="#" onclick="ResetCols(); SetCols(2, 'random')">
					Random</a>
				<!--
				<a onclick="blnSaveUponExit=false;" href="ExtralevelB.aspx?cols=2&mode=cols">2</a> | 
				<a onclick="blnSaveUponExit=false;" href="ExtralevelB.aspx?cols=3&mode=cols">3</a> | 
				<a onclick="blnSaveUponExit=false;" href="ExtralevelB.aspx?cols=4&mode=cols">4</a> | 
				<a onclick="blnSaveUponExit=false;" href="ExtralevelB.aspx?cols=2&mode=random">Random</a>
				-->
				
				<br><br>
				<span style="FONT-WEIGHT: bold; FONT-SIZE: 14px; FONT-FAMILY: arial">Valgt level: </span>
				<span id="chosenLevel" style="FONT-WEIGHT: bold; FONT-SIZE: 14px; FONT-FAMILY: arial">
				</span>
			</div>
			<table style="WIDTH: 100%; HEIGHT: 90%">
				<tr>
					<td align="center" valign="middle">
						<table border="0" cellpadding="3">
							<tr>
								<td></td>
								<td style="WIDTH: 6px">&nbsp;</td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col1"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col2"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col3"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col4"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col5"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col6"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col7"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col8"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col9"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col10"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col11"></td>
								<td align="center"><img src="Images/ExtralevelB/spacer.png" id="col12"></td>
								<td style="WIDTH: 6px">&nbsp;</td>
							</tr>
							<tr style="height:2px;">
								<td></td>
								<td colspan="14" rowspan="15"><img src="Images/ExtralevelB/kolonnehop.png"></td>
							</tr>
							<tr>
								<td style="HEIGHT: 6px"></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row1" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row2" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row3" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row4" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row5" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row6" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row7" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row8" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row9" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row10" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row11" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td><img src="Images/ExtralevelB/spacer.png" id="row12" alt=""></td>
							</tr>
							<tr style="height:31px;">
								<td style="HEIGHT: 6px"></td>
							</tr>
						</table>
						<br>
						<input type="button" value="Start" onclick="setCall(this);MarkCols(this)" id="startBtn">
					</td>
				</tr>
			</table>
			<asp:Literal runat="server" ID="litJavascript" />
		</form>
	</body>
</HTML>
