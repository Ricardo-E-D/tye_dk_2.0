<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="clientProgram.aspx.cs" Inherits="clientProgram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<h1>
		<Eav:TransLit runat="server" ID="TransLit6" Key="eyeTestProgram" />
	</h1>

	<%--<div class="note well"><asp:Literal runat="server" ID="litProgramComments" EnableViewState="false" /></div>--%>

	<asp:HyperLink runat="server" ID="lnkToPrint" Target="_blank">
		<img src="/img/print.png" alt="Print" class="vm" /> <Eav:TransLit runat="server" ID="TransLit9" Key="print" /> 
	</asp:HyperLink>

	<asp:Table runat="server" ID="tblProgram" CssClass="std stripe">
		<asp:TableHeaderRow TableSection="TableHeader">
			<asp:TableHeaderCell></asp:TableHeaderCell>
			<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit1" Key="name" /></asp:TableHeaderCell>
			<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit2" Key="highscore" /></asp:TableHeaderCell>
		</asp:TableHeaderRow>
	</asp:Table>
	
	<asp:PlaceHolder runat="server" ID="plhControls">
	</asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
</asp:Content>

