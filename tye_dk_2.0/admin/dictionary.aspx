<%@ Page Title="Dictionary" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="dictionary.aspx.cs" Inherits="admin_dictionary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" runat="server">
	<script type="text/javascript" src="/js/picnet.table.filter.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" runat="Server">
	
	<h1>Dictionary
		<asp:HyperLink runat="server" ID="lnkNew" CssClass="btn positivesmall" NavigateUrl="dictionary.aspx?ID=0">
			<Eav:TransLit runat="server" ID="TranslationLiteral1" Key="createNew" />
		</asp:HyperLink>
		<asp:LinkButton runat="server" ID="lnkDelete" 
			OnClick="eLnkDelete_Click" OnClientClick="return confirm('Sure?');" CssClass="btn negativesmall link" Visible="false">
			<Eav:TransLit runat="server" ID="t2000" Key="delete" />
		</asp:LinkButton>
	</h1>
	

	<asp:Panel runat="server" ID="pnlExistingData" Visible="true">
		<Eav:TransLit runat="server" ID="TranslationLiteral2" Key="quickFind" TagName="div" CssClass="fieldLabel" />
		<input type="text" id="inpQuickFind" /><br /><br />

		<asp:Table ID="tblEntries" ClientIDMode="Static" runat="server" CssClass="stripe" CellPadding="0" CellSpacing="0">
		</asp:Table>
	</asp:Panel>
	
	<asp:Panel runat="server" ID="pnlAddEntry" Visible="false" DefaultButton="btnSave">
		<div class="fieldLabel">
			Key
		</div>
		<asp:TextBox runat="server" ID="tbKey" MaxLength="50"></asp:TextBox>

		<asp:Table runat="server" ID="tblAddEntry"></asp:Table>
		<asp:PlaceHolder runat="server" ID="plhAddEntry"></asp:PlaceHolder>
		<br />
		<div class="buttons stickToBottom">
			<asp:LinkButton runat="server" ID="btnSave" CssClass="btn positive" OnClick="eBtnSave_Click">
				<Eav:TransLit runat="server" ID="TranslationLiteral3" Key="saveAndClose" />
			</asp:LinkButton>
			<a href="dictionary.aspx" class="btn negative">
			<Eav:TransLit runat="server" ID="t3000" Key="cancel" />
			</a>
		</div>
	</asp:Panel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" runat="Server">
	include('/js/stickToBottom.js');
	$(function() {
		var tableFilterOptions = {
				additionalFilterTriggers: [$('#inpQuickFind')]
			};
		$('#tblEntries').tableFilter(tableFilterOptions);
	});
	
</asp:Content>