<%@ Page language="c#" Inherits="tye.ScreenTasks.ExtralevelD" CodeFile="ExtralevelD.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Extralevel D</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/uri.js"></script>
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/ExtralevelD.js"></script>
	</HEAD>
	<body bgcolor="black" onload="DetectScreenSize()" onresize="DetectScreenSize()" leftmargin="0" topmargin="0">
		<form id="Form1" method="post" runat="server">
		<div style="position: absolute;width:100%; height: 100%" id="container">
			<input type="hidden" id="mapNum" value="map1a" runat="server">
			<!--#include file="vars.htm"-->
			<div ID="letterMenu" runat="server" class="LabyrintMenu" style="WIDTH: 100%; TEXT-ALIGN: center">
				
			</div>
			<img src="Images/ExtralevelD/Cars/spacer.png" id="car" style="position: absolute; z-index: 10000;">
			<img src="Images/ExtralevelD/map1a.png" id="maps" style="position:absolute;">
			<input type="hidden" id="exNo" name="exNo" runat="server" value="-1" />
		</div>
		</form>
	</body>
</HTML>
