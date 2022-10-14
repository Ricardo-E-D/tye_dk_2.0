<%@ Page language="c#" Inherits="tye.ScreenTasks.ExtralevelA" CodeFile="ExtralevelA.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
  <head>
    <title>ExtralevelA</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
    <link href="Styles/Styles.css" rel="stylesheet" type="text/css">
    <script language="javascript" src="Scripts/Classes/Time.js"></script>
    <script language="javascript" src="Scripts/ExtralevelA.js"></script>
    <script type="text/javascript">
		var blnCall = true;
		function setCall(obj) {
			if(obj.value.toLowerCase() != "start") {
				blnCall = false;
			}
		}
		function closingWindow() {
			if(blnCall)
				Start(document.getElementById("btnStart"));
		}
	</script>
  </head>
  <body onload="" bgcolor="black" onunload="closingWindow();">
    <form id="Form1" method="post" runat="server">
		<!--#include file="vars.htm"-->
		<div ID="labyrintMenu" Runat="server" class="LabyrintMenu" style="width: 100%; text-align:center"></div><br /><br />
		<center><asp:Image Visible="false" CssClass="LabyrintImage" Runat="server" ID="labyrintPicture" ImageAlign="Middle" ImageUrl="Images/ExtralevelA/1.png"></asp:Image></center>
		<center><input type="button" value="Start" runat="server" id="btnStart" onclick="setCall(this);Start(this)"></center>
     </form>
  </body>
</html>
