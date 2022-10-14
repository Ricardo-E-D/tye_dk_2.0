<%@ Page language="c#" Inherits="tye.popups.Preview" CodeFile="Preview.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>Preview</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <meta http-equiv="refresh" content="5" />
    <link href="..\stylesheet\tye.css" rel="stylesheet" type="text/css" >
    
	<asp:PlaceHolder id="head_ph" runat="server" />
	
  </head>
  <body style="background:#A0C2D8;text-align:left;" onload="Decode();">
		
  <div id="content" style="margin-bottom:25px;"></div>		

  <a href="#" onclick="Refresh();">Opdater vinduet</a> | <a href="#" onclick="window.close();">Luk vinduet</a>
		
  </body>
</html>
