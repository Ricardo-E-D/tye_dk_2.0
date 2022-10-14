<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="eyeTest.aspx.cs" Inherits="eyeTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	
	<asp:PlaceHolder runat="server" ID="plhList" Visible="false">
        <asp:PlaceHolder runat="server" ID="plhOwnTests" Visible="false">
        <h2>Your own tests <a href="eyeTestOpticianCreate.aspx" class="positivesmall">Create new eye test</a></h2>
        <asp:Table runat="server" ID="tblOwnTests" CssClass="std stripe">
			<asp:TableHeaderRow TableSection="TableHeader">
				<asp:TableHeaderCell></asp:TableHeaderCell>
				<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit3" Key="name" /></asp:TableHeaderCell>
			</asp:TableHeaderRow>
		</asp:Table>
        </asp:PlaceHolder>

        <h2>System tests</h2>
		<asp:Table runat="server" ID="tblProgram" CssClass="std stripe">
			<asp:TableHeaderRow TableSection="TableHeader">
				<asp:TableHeaderCell></asp:TableHeaderCell>
				<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit1" Key="name" /></asp:TableHeaderCell>
			</asp:TableHeaderRow>
		</asp:Table>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhDetails" Visible="false">
		<h1>
			<a href="clientProgram.aspx"><Eav:TransLit runat="server" ID="TransLit2" Key="eyeTestProgram" /></a>
			- 
			<asp:Literal runat="server" ID="eyeTestName" EnableViewState="false" />
		</h1>
		
         <asp:PlaceHolder runat="server" ID="plhLinks" Visible="false">
            <Eav:TransLit runat="server" ID="smth" Key="usefulLinks" TagName="h3"></Eav:TransLit>
        </asp:PlaceHolder>

		<asp:HyperLink runat="server" ID="lnkToMetronome" Target="_blank" NavigateUrl="http://www.webmetronome.com/">
			<img src="/img/metronome.png" alt="Metronome" class="vm" width="24" /> Metronome
		</asp:HyperLink>

		<asp:HyperLink runat="server" ID="lnkToPrint" Target="_blank">
			<img src="/img/print.png" alt="Print" class="vm" /> <Eav:TransLit runat="server" ID="TransLit9" Key="print" /> 
		</asp:HyperLink>
		<br /><br />

		<asp:PlaceHolder runat="server" ID="plhStartEyeTestScreen" Visible="false">
		<script type="text/javascript">
			var linkUrl = "clientEyeTestScreen.aspx?ID=<%= EditID %>&IgnoreProgram=true",
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
	</asp:PlaceHolder>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/css/monoTabs.js.css');
	include('/js/monoTabs.js', function() { $('#tabContainer').monoTabs(); });
</asp:Content>

