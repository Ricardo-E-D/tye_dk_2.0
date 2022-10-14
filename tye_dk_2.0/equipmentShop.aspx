<%@ Page Title="" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="equipmentShop.aspx.cs" Inherits="equipmentShop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<div class="shoppingCartSmall well">
		<asp:Literal runat="server" ID="litShoppingCart" />
		<asp:LinkButton runat="server" ID="lnkShowCart" OnClick="ElnkShowCart_Click" CssClass="positivesmall">
			<Eav:TransLit runat="server" ID="tr9" Key="showCart" />
		</asp:LinkButton>
	</div>

	<asp:PlaceHolder runat="server" ID="plhItems">
		<asp:Repeater runat="server" ID="repItems">
			<HeaderTemplate>
				<table class="sortTable stripe">
				<tbody>
			</HeaderTemplate>
			<ItemTemplate>
				<tr clickurl='<%# VC.QueryStringStrip("EquipmentID") + "EquipmentID=" + Eval("ID") %>'>
					<td>
						<a href='<%# VC.QueryStringStrip("EquipmentID") + "EquipmentID=" + Eval("ID") %>'>
							<strong>
								<%# Eval("Name") %>
							</strong>
						</a>
					</td>
					<td>
						<%# Eval("Description") %>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</tbody></table>
			</FooterTemplate>
		</asp:Repeater>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhVariants" Visible="false">
		<h1>
			<asp:Literal runat="server" ID="litEquipmentName" EnableViewState="false" />
		</h1>

		<asp:Repeater runat="server" ID="repVariants">
			<HeaderTemplate>
				<table width="100%">
					<thead>
						<tr>
							<Eav:TransLit runat="server" ID="tr9" Key="description" TagName="th" />
							<Eav:TransLit runat="server" ID="TransLit1" Key="price" TagName="th" />
							<Eav:TransLit runat="server" ID="TransLit2" Key="quantity" TagName="th" />
						</tr>
					</thead>
				<tbody>
			</HeaderTemplate>
			<ItemTemplate>

				<tr>
					<td><%# Eval("Description") %></td>
					<td><%# Eval("Price") %></td>
					<td>
						<asj:NumericTextBox runat="server" ID='ntbQuantity' NumberType="Integer" MinValue="1" MaxValue="20" Text="1" />
					</td>
					<td class="nowrap">
						<asp:LinkButton runat="server" ID="lnkAddToCart" CommandArgument='<%# Eval("ID") %>' CssClass="positivesmall" OnClick="ElnkAddToCart_Click">
							<Eav:TransLit runat="server" ID="transAddToCart" Key="addToCart" CssClass="positive" />
						</asp:LinkButton>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</tbody></table>
			</FooterTemplate>
		</asp:Repeater>


		<br /><br />

		<a href="equipmentShop.aspx" class="negative">
			<Eav:TransLit runat="server" ID="trans1" Key="cancel" />
		</a>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhCart" Visible="false">
		
		<asp:Repeater runat="server" ID="repCart">
			<HeaderTemplate>
				<table width="100%">
					<thead>
						<tr>
							<Eav:TransLit runat="server" ID="tr9" Key="description" TagName="th" />
							<Eav:TransLit runat="server" ID="TransLit1" Key="price" TagName="th" />
							<th></th>
							<Eav:TransLit runat="server" ID="TransLit2" Key="quantity" TagName="th" />
							<th></th>
							<Eav:TransLit runat="server" ID="TransLit3" Key="subTotal" TagName="th" />
						</tr>
					</thead>
				<tbody>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<%# Eval("EquipmentName")%>
						<div class="note">
							<%# Eval("EquipmentItemDescription")%>
						</div>
					</td>
					<td><%# Eval("EquipmentPrice")%></td>
					<td style="text-align:right;">
						<asp:LinkButton runat="server" ID="lnkOneLess" CommandArgument='<%# Eval("EquipmentItemID")%>' CssClass="negativesmall" OnClick="ElnkOneLessMore_Click" Text="-" />
					</td>
					<td style="text-align:center;"><%# Eval("Quantity")%></td>
					<td style="text-align:left;">
						<asp:LinkButton runat="server" ID="lnkOneMore" CommandArgument='<%# Eval("EquipmentItemID")%>' CssClass="positivesmall" OnClick="ElnkOneLessMore_Click" Text="+" />
					</td>

					<td><%# Eval("SubTotal")%></td>
					<td class="delete">
						<asp:LinkButton runat="server" ID="lnkDelCartItem" CommandArgument='<%# Eval("EquipmentItemID")%>' CssClass="negativesmall" OnClientClick="return confirm(tye.dicValue('confirm_delete'));" OnClick="ElnkDelCartItem_Click">
							<Eav:TransLit runat="server" ID="TransLit5" Key="delete" />
						</asp:LinkButton>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</tbody></table>
			</FooterTemplate>
		</asp:Repeater>

		<br />
		<strong>
			<asp:Literal runat="server" ID="litCartTotal"></asp:Literal>
		</strong>

		<br /><br />

		<Eav:TransLit runat="server" ID="TransLit6" Key="remarks" /> (<Eav:TransLit runat="server" ID="TransLit7" Key="optional" />)
		<asp:TextBox runat="server" ID="tbOrderNote" TextMode="MultiLine" Height="80"></asp:TextBox>
		<br /><br />

		<div class="buttons">
			<asp:LinkButton runat="server" ID="lnkCompletePurchase" CssClass="positive" OnClientClick="return confirm(tye.dicValue('confirm_sure'));" OnClick="ElnkCompletePurchase_Click">
				<Eav:TransLit runat="server" ID="TransLit3" Key="cartPurchaseNow" />
			</asp:LinkButton>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<a href="equipmentShop.aspx" class="negative">
				<Eav:TransLit runat="server" ID="TransLit4" Key="back" />
			</a>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<asp:LinkButton runat="server" ID="lnkClearShoppingCart" CssClass="negative" OnClick="ElnkClearShoppingCart_Click">
				<Eav:TransLit runat="server" ID="TransLit5" Key="cartClear" />
			</asp:LinkButton>
		</div>
	</asp:PlaceHolder>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	$(window).ready(function() { 
		//sortTable($('table.sortTable'), 'td:first-child a strong'); 
	});
</asp:Content>

