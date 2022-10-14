<%@ Page Title="" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="eyeTestInfo.aspx.cs" Inherits="eyeTestInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<style type="text/css">
		div.eyeTestInfo { display:none; margin-bottom: 10px; }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">

	<asp:PlaceHolder runat="server" ID="plhList">
		<asp:Repeater runat="server" ID="repList">
			<HeaderTemplate>
				<table>
			</HeaderTemplate>
			<ItemTemplate>
				<tr clickurl='?EyeTestID=<%# Eval("Name") %>'>
					<td>
						<%# createEditLinks((int)DataBinder.Eval(Container.DataItem, "ID")) %>
					</td>
					<td>
						<a href='?EyeTestID=<%# Eval("ID") %>&lang=Dk'></a><%# Eval("Name") %>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate></table></FooterTemplate>
		</asp:Repeater>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhTextTypes" Visible="false">
		<h1>
			<asp:Literal runat="server" ID="litEyeTestName" />
		</h1>

		<asp:Literal runat="server" ID="litLangLinks"></asp:Literal>

		<a class="link" onclick="$('div.eyeTestInfo').slideToggle();">Toggle texts</a>
		<br />
		<br /><br />
		<asp:LinkButton runat="server" CssClass="positivesmall" ID="lnkAddStep" OnClick="eLnkAddText_Click">Add step</asp:LinkButton>	
		<asp:LinkButton runat="server" CssClass="positivesmall" ID="lnkAddIntro" OnClick="eLnkAddText_Click">Add "Intro"</asp:LinkButton>	
		<asp:LinkButton runat="server" CssClass="positivesmall" ID="lnkAddImportant" OnClick="eLnkAddText_Click">Add "Important"</asp:LinkButton>	
		<asp:LinkButton runat="server" CssClass="positivesmall" ID="lnkAddPurpose" OnClick="eLnkAddText_Click">Add "Purpose"</asp:LinkButton>	
		
		<br /><br />
		<asp:PlaceHolder runat="server" ID="plhEyeTestInfos"></asp:PlaceHolder>
		<br /><br />
		<div class="buttons">
			<a class="negative" href="eyeTestInfo.aspx">Back to list</a>
		</div>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhEditText" Visible="false">
		<h1>
			<asp:Literal runat="server" ID="litEditTextName" />
			<asp:LinkButton runat="server" ID="lnkDeleteEyeTestInfo" OnClick="elnkDeleteEyeTestInfo_Click" OnClientClick="return confirm(tye.dicValue('confirm_delete'));" CssClass="negativesmall">
				<Eav:TransLit runat="server" ID="sdkf2" Key="delete"></Eav:TransLit>
			</asp:LinkButton>
		</h1>

		<ck:CKEditorControl runat="server" ID="ckEditor"></ck:CKEditorControl>
		<asp:TextBox runat="server" ID="tbName" MaxLength="100" Visible="false" />
		<br /><br />
		<div class="buttons">
			<asp:LinkButton runat="server"  CssClass="positive" ID="lnkSaveEyeTestInfo" OnClick="eLnkSaveEyeTestInfo_Click">
				Save
			</asp:LinkButton>
			<asp:HyperLink runat="server" CssClass="negative"  ID="lnkCancelEyeTestInfo">
				Cancel
			</asp:HyperLink>
		</div>
	</asp:PlaceHolder>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
</asp:Content>

