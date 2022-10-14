<%@ Page language="c#" Inherits="tye.ScreenTasks._3DLevel0" CodeFile="3DLevel0.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>3D Level 0</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Fri, Jun 12 1981 08:20:00 GMT">
		<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
		<META HTTP-EQUIV="Cache-Control" CONTENT="no-cache\">
		<link href="Styles/Styles.css" rel="stylesheet" type="text/css">
		<script language="javascript" src="Scripts/Classes/Time.js"></script>
		<script language="javascript" src="Scripts/3DLevel0.js"></script>
		<script type="text/javascript">
		var blnMakeParentPostback = true;
		function closewindow() {
		  Stop();
		  if(!blnMakeParentPostback)
			return;
			
		  if (window.dialogArguments) {
			// Calling the method (given as argument) to cause the postback
			window.dialogArguments.doPostBack();
		  }
		  else{
			opener.doPostBack();
		  }
			window.close();
		}
		window.onunload = closewindow;
		window.onbeforeunload = closewindow;
		</script>
	</HEAD>
	<body onload="StartLevel()" bgcolor="black">
		<form id="Form1" method="post" runat="server">
			<!--#include file="vars.htm"-->
			<table style="WIDTH: 100%; HEIGHT: 100%; TEXT-ALIGN: center">
				<tr>
					<td valign="middle" align="center">
						<table>
							<tr>
								<td align="center"><img src="Images/3DLevel0/bouncing.gif" width="292" height="138"></td>
							</tr>
							<tr>
								<td><span id="textGuide" runat="server"></span></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
