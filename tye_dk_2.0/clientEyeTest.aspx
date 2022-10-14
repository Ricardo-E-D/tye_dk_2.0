<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="clientEyeTest.aspx.cs" Inherits="clientEyeTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<h1>
		<a href="clientProgram.aspx"><Eav:TransLit runat="server" ID="TransLit6" Key="eyeTestProgram" /></a>
		- 
		<asp:Literal runat="server" ID="eyeTestName" EnableViewState="false" />
	</h1>

	<asp:HyperLink runat="server" ID="lnkToMetronome" Target="_blank" NavigateUrl="http://www.webmetronome.com/">
		<img src="/img/metronome.png" alt="Metronome" class="vm" width="24" /> Metronome
	</asp:HyperLink>

	<asp:HyperLink runat="server" ID="lnkToPrint" Target="_blank">
		<img src="/img/print.png" alt="Print" class="vm" /> <Eav:TransLit runat="server" ID="TransLit9" Key="print" /> 
	</asp:HyperLink>
	<br /><br />
	
    <asp:PlaceHolder runat="server" ID="plhLinks" Visible="false">
        <Eav:TransLit runat="server" ID="smth" Key="usefulLinks" TagName="h3"></Eav:TransLit>
    </asp:PlaceHolder>	

	<asp:PlaceHolder runat="server" ID="plhStartEyeTestScreen" Visible="false">
		<script type="text/javascript">
			var linkUrl = "clientEyeTestScreen.aspx?ID=<%= ProgramEyeTestID %>",
			linkParams = 'width=' + screen.width
				+ ', height=' + screen.height
				+ ', top=0, left=0'
				+ ', fullscreen=yes';
		</script>
		<a class="positive link" onclick="window.open(linkUrl, '', linkParams);">
			<Eav:TransLit runat="server" ID="t01" Key="startEyeTest" />
		</a>
		<br /><br />
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhStartEyeTestText" Visible="false">
		<asp:ScriptManagerProxy runat="server" ID="smp1">
			<Services>
				<asp:ServiceReference Path="~/wsTye.asmx" />
			</Services>
		</asp:ScriptManagerProxy>
		<script type="text/javascript">
			include('/js/eyetest/global/eyeTestText.js', 
				function() { 
					eyeTestStartText(<%= ProgramEyeTestID %>); 
				}
			);
		</script>
	</asp:PlaceHolder>

	<div id="tabContainer" class="tabContainer">
		<div class="innerTabs">
			<ul>
				<asp:Literal runat="server" ID="litTabs" EnableViewState="false" />
			</ul>
		</div>
		<div class="innerContainer">
			<asp:Literal runat="server" ID="litTabPanels" EnableViewState="false" />
		</div>
	</div>
	<asp:PlaceHolder runat="server" ID="plhControls">
	</asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/css/monoTabs.js.css');
	include('/js/monoTabs.js', function() { $('#tabContainer').monoTabs(); });
</asp:Content>

