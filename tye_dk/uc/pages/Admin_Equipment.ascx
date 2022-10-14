<%@ Reference Control="~/uc/pages/uc_pages.ascx" %>
<%@ Control Language="c#" Inherits="tye.uc.pages.Admin_Equipment" CodeFile="Admin_Equipment.ascx.cs" CodeFileBaseClass="tye.uc.pages.uc_pages" %>

<style type="text/css">
	img { border: 0px; }
	.vt { vertical-align: top; }
	.pis td { border: #000 0px solid; }
</style>
	<asp:Panel ID="pnlNewEqupiment" runat="server" Visible="false">
		<asp:LinkButton runat="server" ID="lnkBackToList" Text="Tilbage til oversigt" OnClick="lnkBackToList_Click" />
		<br /><br />
		<h3>Opret nyt udstyr...</h3>
		<b>Navn på udstyr</b>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td><img src="gfx/danish_flag.gif" alt="" />&nbsp;&nbsp;</td>
				<td><asp:TextBox ID="tbNewEquipmentName_DK" runat="server" Width="200" /></td>
			</tr>
			<tr>
				<td><img src="gfx/norwegian_flag.gif" alt="" /></td>
				<td><asp:TextBox ID="tbNewEquipmentName_NO" runat="server" Width="200" /></td>
			</tr>
			<tr>
				<td><img src="gfx/english_flag.gif" alt="" /></td>
				<td><asp:TextBox ID="tbNewEquipmentName_UK" runat="server" Width="200" /></td>
			</tr>
			<tr>
				<td><img src="gfx/german_flag.gif" alt="" /></td>
				<td><asp:TextBox ID="tbNewEquipmentName_DE" runat="server" Width="200" /></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2"><b>Beskrivelser</b></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/danish_flag.gif" alt="" /></td>
				<td runat="server" id="tdDesc_DK"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/norwegian_flag.gif" alt="" /></td>
				<td runat="server" id="tdDesc_NO"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/english_flag.gif" alt="" /></td>
				<td runat="server" id="tdDesc_UK"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/german_flag.gif" alt="" /></td>
				<td runat="server" id="tdDesc_DE">
				</td>
			</tr>
			<tr>
				<td></td>
				<td><asp:CheckBox runat="server" ID="chkActive" Text="Aktiv på hjemmesiden?" Checked="true" /></td>
			</tr>
			<tr>
				<td>Billede</td>
				<td runat="server" id="tdNewEquipmentImage"><asp:FileUpload runat="server" ID="fileUplNewImage" /></td>
			</tr>
			<tr>
				<td></td>
				<td>
					<asp:Button ID="btnNewEquipmentSubmit" runat="server" Text="Gem" OnClick="btnNewEquipmentSubmit_Click" /><br />
				</td>
			</tr>
		</table>
	</asp:Panel>
	
	
	<asp:Panel ID="pnlEditEqupiment" runat="server" Visible="false">
		<asp:LinkButton runat="server" ID="lnkBackToListFromEdit" Text="Tilbage til oversigt" OnClick="lnkBackToList_Click" />
		<br /><br />
		<h3>Rediger udstyr...</h3>
		<b>Navn på udstyr</b>
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td><img src="gfx/danish_flag.gif" alt="" />&nbsp;&nbsp;</td>
				<td><asp:TextBox ID="tbEditEquipmentName_DK" runat="server" Width="200" /></td>
			</tr>
			<tr>
				<td><img src="gfx/norwegian_flag.gif" alt="" /></td>
				<td><asp:TextBox ID="tbEditEquipmentName_NO" runat="server" Width="200" /></td>
			</tr>
			<tr>
				<td><img src="gfx/english_flag.gif" alt="" /></td>
				<td><asp:TextBox ID="tbEditEquipmentName_UK" runat="server" Width="200" /></td>
			</tr>
			<tr>
				<td><img src="gfx/german_flag.gif" alt="" /></td>
				<td><asp:TextBox ID="tbEditEquipmentName_DE" runat="server" Width="200" /></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2"><b>Beskrivelser</b></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/danish_flag.gif" alt="" /></td>
				<td runat="server" id="tdEditDesc_DK"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/norwegian_flag.gif" alt="" /></td>
				<td runat="server" id="tdEditDesc_NO"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/english_flag.gif" alt="" /></td>
				<td runat="server" id="tdEditDesc_UK"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/german_flag.gif" alt="" /></td>
				<td runat="server" id="tdEditDesc_DE">
				</td>
			</tr>
			<tr>
				<td></td>
				<td><asp:CheckBox runat="server" ID="chkEditActive" Text="Aktiv på hjemmesiden?" Checked="true" /></td>
			</tr>
			<tr>
				<td class="vt">Billede</td>
				<td runat="server" id="tdEditEquipmentImage">
					<asp:Image Width="100" runat="server" ID="imgExistingImage" Visible="false" />
					<asp:ImageButton BorderWidth="0" runat="server" ID="ibtnDeleteExistingImage" Visible="false" ImageUrl="../../gfx/delete.gif" OnClientClick="return sureDelete('equipmentImage');" ToolTip="Slet det tilknyttede billede" OnClick="ibtnDeleteExistingImage_Click" />
					<asp:FileUpload runat="server" ID="fileUplEditImage" />
				
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
					<asp:Button ID="btnEditEquipmentSubmit" runat="server" Text="Gem" OnClick="btnEditEquipmentSubmit_Click" /><br />
				</td>
			</tr>
		</table>
	</asp:Panel>
	
	<asp:Panel ID="pnlNewVariant" runat="server" Visible="false">
		<asp:LinkButton runat="server" ID="lnkNewVariantBackToList" Text="Tilbage til oversigt" OnClick="lnkBackToList_Click" />
		<br /><br />
		<h3>Opret ny variant...</h3>
		...til produkt <asp:Literal ID="litNewVariantParentEquipment" runat="server" />
		<br />
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td colspan="2"><b>Beskrivelser</b></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/danish_flag.gif" alt="" /></td>
				<td runat="server" id="tdNewVariantDesc_DK"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/norwegian_flag.gif" alt="" /></td>
				<td runat="server" id="tdNewVariantDesc_NO"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/english_flag.gif" alt="" /></td>
				<td runat="server" id="tdNewVariantDesc_UK"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/german_flag.gif" alt="" /></td>
				<td runat="server" id="tdNewVariantDesc_DE">
				</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2"><b>Priser</b></td>
			</tr>
			<tr>
				<td><img src="gfx/danish_flag.gif" alt="" />&nbsp;&nbsp;</td>
				<td>DKK <asp:TextBox ID="tbNewVariantPrice_DK" runat="server" Width="200" onblur="do_check_price(this, false);" /></td>
			</tr>
			<tr>
				<td><img src="gfx/norwegian_flag.gif" alt="" /></td>
				<td>NOK <asp:TextBox ID="tbNewVariantPrice_NO" runat="server" Width="200" onblur="do_check_price(this, false);" /></td>
			</tr>
			<tr>
				<td><img src="gfx/english_flag.gif" alt="" /></td>
				<td>GBP <asp:TextBox ID="tbNewVariantPrice_UK" runat="server" Width="200" onblur="do_check_price(this, false);" /></td>
			</tr>
			<tr>
				<td><img src="gfx/german_flag.gif" alt="" /></td>
				<td>Euro <asp:TextBox ID="tbNewVariantPrice_DE" runat="server" Width="200" onblur="do_check_price(this, false);" /></td>
			</tr>
			<tr>
				<td></td>
				<td><asp:CheckBox runat="server" ID="chkNewVariantActive" Text="Aktiv på hjemmesiden?" Checked="true" /></td>
			</tr>
			<tr>
				<td></td>
				<td>
					<asp:Button ID="btnNewVariantSubmit" runat="server" Text="Gem" OnClick="btnNewVariantSubmit_Click" /><br />
				</td>
			</tr>
		</table>
	</asp:Panel>
	
	<asp:Panel ID="pnlEditVariant" runat="server" Visible="false">
		<asp:LinkButton runat="server" ID="lnkEditVariantBackToList" Text="Tilbage til oversigt" OnClick="lnkBackToList_Click" />
		<br /><br />
		<h3>Rediger variant...</h3>
		...til produkt <asp:Literal ID="litEditVariantParentEquipment" runat="server" />
		<br />
		<table cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td colspan="2"><b>Beskrivelser</b></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/danish_flag.gif" alt="" /></td>
				<td runat="server" id="tdEditVariantDesc_DK"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/norwegian_flag.gif" alt="" /></td>
				<td runat="server" id="tdEditVariantDesc_NO"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/english_flag.gif" alt="" /></td>
				<td runat="server" id="tdEditVariantDesc_UK"></td>
			</tr>
			<tr>
				<td class="vt"><img src="gfx/german_flag.gif" alt="" /></td>
				<td runat="server" id="tdEditVariantDesc_DE">
				</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2"><b>Priser</b></td>
			</tr>
			<tr>
				<td><img src="gfx/danish_flag.gif" alt="" />&nbsp;&nbsp;</td>
				<td>DKK <asp:TextBox ID="tbEditVariantPrice_DK" runat="server" Width="200" onblur="do_check_price(this, false);" /></td>
			</tr>
			<tr>
				<td><img src="gfx/norwegian_flag.gif" alt="" /></td>
				<td>NOK <asp:TextBox ID="tbEditVariantPrice_NO" runat="server" Width="200" onblur="do_check_price(this, false);" /></td>
			</tr>
			<tr>
				<td><img src="gfx/english_flag.gif" alt="" /></td>
				<td>GBP <asp:TextBox ID="tbEditVariantPrice_UK" runat="server" Width="200" onblur="do_check_price(this, false);" /></td>
			</tr>
			<tr>
				<td><img src="gfx/german_flag.gif" alt="" /></td>
				<td>Euro <asp:TextBox ID="tbEditVariantPrice_DE" runat="server" Width="200" onblur="do_check_price(this, false);" /></td>
			</tr>
			<tr>
				<td></td>
				<td><asp:CheckBox runat="server" ID="chkEditVariantActive" Text="Aktiv på hjemmesiden?" Checked="true" /></td>
			</tr>
			<tr>
				<td></td>
				<td>
					<asp:Button ID="btnEditVariantSubmit" runat="server" Text="Gem" OnClick="btnEditVariantSubmit_Click" /><br />
				</td>
			</tr>
		</table>
	</asp:Panel>
	
	<asp:Panel ID="pnlExistingEquipment" runat="server" Visible="true">
		<asp:LinkButton runat="server" ID="lnkMakeNewEquipment" Text="Opret nyt udstyr" OnClick="lnkMakeNewEquipment_Click" />
		<br /><br />
		<h3>Eksisterende udstyr</h3>
		<asp:Literal ID="litExistingEquipment" runat="server" />
		<asp:Table runat="server" ID="tblExistingEquipment" Width="100%" CssClass="pis" />
	</asp:Panel>
		
	<asp:HiddenField ID="hideEquipmentItem" runat="server" Value="false" />
	<asp:HiddenField ID="hideEquipmentId" runat="server" />
	<asp:HiddenField ID="hideVariantParentEquipment" runat="server" />
	