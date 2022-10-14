<%@ Page language="c#" Inherits="tye.ScreenTasks.ExtralevelH" CodeFile="ExtralevelH.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Extralevel H</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/ExtralevelH.js"></script>
	</HEAD>
	<body bgcolor="black" onload="StartLevel()" onunload="Stop();">
		<form id="Form1" method="post" runat="server">
			<!--#include file="vars.htm"-->
			<div ID="letterMenu" Runat="server" class="LabyrintMenu" style="WIDTH: 100%; TEXT-ALIGN: center"></div>
			<br>
			<br>
			<table align="center">
				<tr>
					<td><asp:Image ID="img" Runat="server" ImageUrl="Images/ExtralevelH/1.png"></asp:Image>
					</td>
				</tr>
			</table>
			<center><input type="button" value="Start" onclick="Stop()" id="btn"></center>
		</form>
	</body>
</HTML>

</html>
