<%@ Page Title="Print" Language="C#" MasterPageFile="~/masterClean.master" AutoEventWireup="true" CodeFile="eyeTestPrint.aspx.cs" Inherits="programPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<h1>
		<asp:Literal runat="server" ID="litEyeTestName"></asp:Literal>
	</h1>

	<asp:PlaceHolder runat="server" ID="plhControls">
	</asp:PlaceHolder>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	$(function() { window.print(); });
</asp:Content>