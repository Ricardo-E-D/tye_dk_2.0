<%@ Page language="c#" Inherits="tye.popups.Internal_link" CodeFile="Internal_link.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
    <title>Internal_link</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
	<link href="..\stylesheet\tye.css" rel="stylesheet" type="text/css" >
	
	<script type="text/javascript">

		function addLink(linkstr){
		
		var textstr = opener.parent.document.selection.createRange();
			
			if (textstr.text != '' && linkstr != '' && textstr.text.indexOf(linkstr) == -1) {
				sT = textstr;
				sTxt = "[link," + linkstr + ",intern]" + sT.text + "[/link]";
				sT.text = sTxt
				
				window.close();
				
			} else if (textstr.text != '' && textstr.text.indexOf(linkstr) != -1) {
				sT = textstr;
				sTxt = sT.text.replace("[link," + linkstr + ",intern]",'');
				sTxt = sTxt.replace("[/link]",'');
				sT.text = sTxt
			}
		}
		
		function storeCaret(textEl){
  		var theform = opener.document.forms["main_form"]; 
    		if (textEl.createTextRange){
				textEl.caretPos = theform.selection.createRange().duplicate();
			} 
		}
		
	</script>
</head>  
  <body id="il_body" runat="server" style="text-align:left;">

  </body>
</html>
