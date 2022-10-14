<%@ Page Title="Print" Language="C#" MasterPageFile="~/masterClean.master" AutoEventWireup="true" CodeFile="programPrint.aspx.cs" Inherits="programPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<h1>
		<asp:HyperLink runat="server" ID="lnkBackToClient" NavigateUrl="?id=0" CssClass="plain" />
	</h1>

	<div class="note"><asp:Literal runat="server" ID="litProgramComments" EnableViewState="false" /></div>

	<asp:Table runat="server" ID="tblProgram" CssClass="std stripe">
		<asp:TableHeaderRow TableSection="TableHeader">
			<asp:TableHeaderCell></asp:TableHeaderCell>
			<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit1" Key="name" /></asp:TableHeaderCell>
		</asp:TableHeaderRow>
	</asp:Table>
	
	<asp:PlaceHolder runat="server" ID="plhControls">
	</asp:PlaceHolder>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	$(function() { window.print(); });
</asp:Content>