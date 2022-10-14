<%@ Page Title="" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="equipment.aspx.cs" Inherits="equipment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<style type="text/css">
		div.eyeTestInfo { display:none; margin-bottom: 10px; }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">

	<asp:PlaceHolder runat="server" ID="plhList">
		<a href="?EquipmentID=0" class="positive">
			New equipment
		</a>
		<br /><br />

		<asp:Repeater runat="server" ID="repList">
			<HeaderTemplate>
				<table class="stripe">
					<thead>
						<tr>
							<th>Active</th>
							<th>Internal name</th>
						</tr>
					</thead>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td style='color:<%# ((bool)Eval("Active") ? "#00ff00" : "#bb0000") %>;'>
						<%# Eval("Active") %>
					</td>
					<td>
						<%# Eval("Name") %>
					</td>
					<td>
						<a href='?EquipmentID=<%# Eval("ID") %>'>Edit description</a>
					</td>
					<td>
						<a href='?EquipmentID=<%# Eval("ID") %>&view=items'>Edit variants</a>
					</td>
					<td class="delete">
						<asp:LinkButton runat="server" ID="linkDeleteEquipment" CssClass="negativesmall" OnClientClick="return confirm(tye.dicValue('confirm_delete'));" CommandArgument='<%# Eval("ID") %>' OnClick="lnkDeleteEquipment_Click">
							<Eav:TransLit runat="server" ID="litDel" Key="delete" />
						</asp:LinkButton>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate></table></FooterTemplate>
		</asp:Repeater>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhEditText" Visible="false">
		<h1>
			<asp:Literal runat="server" ID="litEquipmentName" />
		</h1>
		
		<asp:CheckBox runat="server" ID="chkActive" Text="Active on web site?" />
		<br /><br />

		<div class="fieldTitle">Internal name</div>
		<asp:TextBox runat="server" ID="tbInternalName" MaxLength="50" />
		<br /><br />

		<div class="tabContainer" id="tabContainer">
			<div class="innerTabs">
				<ul>
					<asp:Literal runat="server" ID="litTabs" />
				</ul>
			</div>
			<div class="innerContainer">
				<asp:PlaceHolder runat="server" ID="plhTabs" />
			</div>
		</div>
		<asp:TextBox runat="server" ID="tbName" MaxLength="100" Visible="false" />
		<br /><br />
		<div class="buttons">
			<asp:LinkButton runat="server"  CssClass="positive" ID="lnkEquipentSave" OnClick="eLnkEquipmentSave_Click">
				Save
			</asp:LinkButton>
			<asp:LinkButton runat="server"  CssClass="positive" ID="lnkEquipentSaveAndClose" OnClick="eLnkEquipmentSave_Click">
				Save and close
			</asp:LinkButton>
			<a class="negative" href="equipment.aspx">
				Cancel
			</a>
		</div>
	</asp:PlaceHolder>



	<asp:PlaceHolder runat="server" ID="plhEquipmentItem" Visible="false">
		<asp:PlaceHolder runat="server" ID="plhEquipmentItemList">

			<h1>
				<asp:Literal runat="server" ID="litEquipmentName2" EnableViewState="false" />
			</h1>
			<asp:LinkButton runat="server" ID="lnkAddVariant" CssClass="positive" OnClick="eLnkAddVariant_Click">
				Add variant
			</asp:LinkButton>
			
			<br /><br />

			<div class="tabContainer" id="tabContainerItems">
				<div class="innerTabs">
					<ul>
						<asp:Literal runat="server" ID="litItemTabs" />
					</ul>
				</div>
				<div class="innerContainer">
					<asp:PlaceHolder runat="server" ID="plhItemTabs" />
				</div>
			</div>
		</asp:PlaceHolder>
		<br /><br />
		<div class="buttons">
			<asp:LinkButton runat="server"  CssClass="positive" ID="lnkSave" OnClick="eLnkEquipmentItemSave_Click">
				Save
			</asp:LinkButton>
			<asp:LinkButton runat="server"  CssClass="positive" ID="lnkSaveAndClose" OnClick="eLnkEquipmentItemSave_Click">
				Save and close
			</asp:LinkButton>
			<a class="negative" href="equipment.aspx">
				Cancel
			</a>
		</div>
	</asp:PlaceHolder>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/css/monoTabs.js.css');
	include('/js/jquery.cookie.js');
	include('/js/monoTabs.js', function() { $('#tabContainer, #tabContainerItems').monoTabs({ cookie_name: 'adminEquipment'}); });

</asp:Content>

