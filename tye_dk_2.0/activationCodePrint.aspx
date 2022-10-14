<%@ Page Title="Clients" Language="C#" MasterPageFile="~/masterClean.master" AutoEventWireup="true" CodeFile="activationCodePrint.aspx.cs" Inherits="activationCodePrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
	<style type="text/css">
		@media print
		{
			div.notprintable { display:none; }
			body { margin: 0mm 0mm 0mm 0mm; }
		}
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<asp:HyperLink runat="server" ID="lnkShowAll" CssClass="positivesmall" Visible="false">Vis alle koder - inkl. brugte</asp:HyperLink><br /><br />

	<asp:Panel EnableViewState="false" runat="server" ID="pnlChecks" ClientIDMode="Static" CssClass="notprintable">
	
	</asp:Panel>
	<asp:Panel EnableViewState="false" runat="server" ID="Panel1" CssClass="notprintable">
		<br />
		<br />
		<asp:Button runat="server" ID="btnMarkAsPrinted" Text="Mark selected codes as printed" CssClass="positivesmall" OnClick="eBtnMarkAsPrinted_Click" />

		<br />
		<br />

		
		<asp:Button runat="server" ID="btnCreatePdf" Text="Lav PDF" CssClass="positivesmall" OnClick="eBtnCreatePdf_Click" />
	</asp:Panel>

	<asp:PlaceHolder runat="server" ID="list"></asp:PlaceHolder>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/js/js.js');
	onload_methods.push(function() {
		$('.checker').on('click', 'input', function() {
			var ischecked = $(this).is(':checked');
			if(ischecked)
				$('#' + $(this).attr('id').replace('chk', '')).slideDown();
			else
				$('#' + $(this).attr('id').replace('chk', '')).slideUp();

		});
		addCheckEvent();
	});
	/*onload_methods.push(function() {
		$('img.tooltip').each(function() {
         $(this).qtip({
             content: {
                 text: $(this).next('.hidden')
             }
         });
     });
	});*/
</asp:Content>

