<%@ Page Title="Opticians" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true"
	CodeFile="activationCodeReset.aspx.cs" Inherits="activationCodeReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" runat="Server">
	<h1>
		Nulstil kode
	</h1>
	
	<asp:Panel runat="server" ID="pnlError" CssClass="errorInline" Visible="false"></asp:Panel>

	
	<asp:PlaceHolder runat="server" ID="plhEdit">
		<div class="span">
			<asp:PlaceHolder runat="server" ID="pnlCodes"></asp:PlaceHolder>
		</div>
		
	</asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" runat="Server">

</asp:Content>
