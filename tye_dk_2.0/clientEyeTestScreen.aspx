<%@ Page Title="Screen test" Language="C#" MasterPageFile="~/masterEyeTestScreen.master" AutoEventWireup="true" CodeFile="clientEyeTestScreen.aspx.cs" Inherits="clientEyeTestScreen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<meta http-equiv="Expires" content="Fri, Jun 12 1981 08:20:00 GMT" />
	<meta http-equiv="Pragma" content="no-cache" />
	<meta http-equiv="Cache-Control" content="no-cache\" />

	<link rel="Stylesheet" href="/css/master.css" />
	<link rel="Stylesheet" href="/css/buttons.css" />
	<link rel="Stylesheet" href="/css/eyeTestScreen.css" />
	<script type="text/javascript" src="/js/datetime.1-1.js"></script>
	<script type="text/javascript" src="/js/eyetest/global/eyeTestScreen.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<asp:ScriptManager runat="server" ID="sm1">
		<Services>
			<asp:ServiceReference Path="~/wsTye.asmx" />
		</Services>
	</asp:ScriptManager>

	<div style="">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
			<tr>
				<td>
					<strong>
						<asp:Literal runat="server" ID="litEyeTestName" EnableViewState="false" />
					</strong>
				</td>
				<td style="text-align:right;" class="closebuttons">
					<a href="#" class="positivesmall" onclick="window.location.reload(true);">
						<Eav:TransLit runat="server" ID="TransLit1" Key="restart" />
					</a>
					<a href="#" class="negativesmall" onclick="eyeTestScreen.stop();window.close();">
						<Eav:TransLit runat="server" ID="litCloseWindow" Key="closeWindow" />
					</a>
				</td>
			</tr>
		</table>
	</div>

	<div id="eyetestContainer" style="position:relative;margin:auto;">
		<asp:PlaceHolder runat="server" ID="plhUc" />
	</div>

	<script type="text/javascript">
		include('/js/eyetest/global/Animate.js');
		include('/js/eyetest/global/Bezier.js');
		$(window).ready(function () {
			eyeTestScreen.EyeTestProgramID = <%= ProgramEyeTestID %>;
		});
	</script>
	
	<asp:HiddenField runat="server" ID="hidProgramEyeTestID" ClientIDMode="Static" />
	<asp:HiddenField runat="server" ID="hidStartTime" ClientIDMode="Static" />
	<asp:HiddenField runat="server" ID="hidEndTime" ClientIDMode="Static" />
	<asp:HiddenField runat="server" ID="hidScore" Value="0" ClientIDMode="Static" />
	<asp:HiddenField runat="server" ID="hidScoreRequired" Value="0" ClientIDMode="Static" />
	<asp:HiddenField runat="server" ID="hidAttribName" ClientIDMode="Static" />
	<asp:HiddenField runat="server" ID="hidAttribValue" ClientIDMode="Static" />
	<asp:HiddenField runat="server" ID="hidPreviousHighscore" ClientIDMode="Static" />
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	
</asp:Content>

