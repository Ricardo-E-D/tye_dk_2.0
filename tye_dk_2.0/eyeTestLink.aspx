<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="eyeTestLink.aspx.cs" Inherits="eyeTestLink" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	
    <asp:Panel runat="server" ID="pnlError" CssClass="errorInline" Visible="false">
        <asp:Literal runat="server" ID="litError"></asp:Literal>
    </asp:Panel>
	
        <h1></h1>
		<h1>
			
		<asp:Literal runat="server" ID="eyeTestName" EnableViewState="false" />
		</h1>
		<div>Links are only visible to your own clients</div>
        <br />
        <asp:Table runat="server" ID="tblLinks" CssClass="std stripe">
		</asp:Table>

    <br /><br />
        <h1>Add link</h1>

        <label for="tbNewLinkUrl">URL</label>
        <asp:TextBox runat="server" ID="tbNewLinkUrl"></asp:TextBox>

        <label for="tbNewLinkUrl">Link name</label>
        <asp:TextBox runat="server" ID="tbNewLinkName"></asp:TextBox>
        <br /><br />
         <asp:Button runat="server" ID="btnNewLinkSubmit" Text="Save" CssClass="positivesmall" OnClick="btnNewLinkSubmit_Click" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/css/monoTabs.js.css');
	include('/js/monoTabs.js', function() { $('#tabContainer').monoTabs(); });
</asp:Content>

