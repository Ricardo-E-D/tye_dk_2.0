<%@ Page language="c#" Inherits="tye.ScreenTasks.Level1A" CodeFile="Level1A.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Level1A</title>
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
		<script language="javascript" src="Scripts/Level1A.js"></script>
		  <script type="text/javascript">
			var blnCall = true;
			function setCall(obj) {
				if(obj.value.toLowerCase() != "start") {
					blnCall = false;
				}
			}
			function closingWindow() {
				if(blnCall)
					Stop();
			}
		</script>
	</HEAD>
	<body bgcolor="black" onload="" onunload="closingWindow();">
		<form id="Form1" method="post" runat="server">
			<!--#include file="vars.htm"-->
			<div ID="letterMenu" Runat="server" class="LabyrintMenu" style="WIDTH: 100%; TEXT-ALIGN: center">
				<asp:HyperLink Runat="server" ID="vertical">Vertical</asp:HyperLink>
				|
				<asp:HyperLink Runat="server" ID="horizontal">Horizontal</asp:HyperLink>
				|
				<asp:HyperLink Runat="server" ID="circular">Circular</asp:HyperLink>
				|
				<asp:HyperLink Runat="server" ID="diverse">Skrå</asp:HyperLink>
				|
				<asp:HyperLink Runat="server" ID="random">Random</asp:HyperLink>
			</div>
			<table style="WIDTH: 100%; HEIGHT: 90%">
				<tr>
					<td>
						<table align="center">
							<tr>
								<td valign="middle">
									<div style="WIDTH: 700px; POSITION: relative; HEIGHT: 500px">
										<img src="Images/Level1A/eye.jpg" id="eye" style="LEFT: 0px; POSITION: absolute; TOP: 0px" width="54" height="36">
										<img src="Images/Level1A/aim.gif" id="aim" style="LEFT: 350px; POSITION: absolute; TOP: 250px" width="21" height="21">
									</div>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<center><input type="button" id="btnStopExercise" value="Stop" onclick="setCall(this);Stop()"></center>
		</form>
	</body>
</HTML>
