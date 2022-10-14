<%@ Page Title="Opticians" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true"
	CodeFile="opticians.aspx.cs" Inherits="opticians" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" runat="Server">
	<h1>
		<Eav:TransLit runat="server" ID="TranslationLiteral3" Key="opticians" />
		<asp:HyperLink runat="server" ID="lnkCreateNew" NavigateUrl="?id=0" CssClass="btn positivesmall">
			<Eav:TransLit runat="server" ID="TransLit23" Key="createNew" />
		</asp:HyperLink>
	</h1>

	<asp:Panel runat="server" ID="pnlError" CssClass="errorInline" Visible="false"></asp:Panel>

	<asp:PlaceHolder runat="server" ID="plhList">
		<p>
			<Eav:TransLit runat="server" ID="traCountry" Key="language" />
			:
			<asp:DropDownList runat="server" ID="ddlFilterLanguage" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterLanguage_SelectedIndexChanged"
				CssClass="auto" />
			&nbsp;&nbsp;&nbsp;
			<Eav:TransLit runat="server" ID="TransLit20" Key="search" />
			:
			<input type="text" id="tableFilterPhrase" style="width: 100px;" />
			<img id="tableFilterClear" src="/img/clear.png" class="link" alt="" />
		</p>
		<div class="tabContainer" id="tabContainer">
			<div class="innerTabs">
				<ul>
					<li>
						<Eav:TransLit runat="server" ID="TranslationLiteral2" Key="active" />
						(<asp:Literal runat="server" ID="litActiveCount" />) </li>
					<li>
						<Eav:TransLit runat="server" ID="TranslationLiteral1" Key="inactive" />
						(<asp:Literal runat="server" ID="litInactiveCount" />) </li>
				</ul>
			</div>
			<div class="innerContainer">
				<!-- panel -->
				<div class="tabPanel" style="overflow: hidden;">
					<asp:Repeater runat="server" ID="repOpticiansActive">
						<HeaderTemplate>
							<table class="stripe">
								<thead>
									<tr>
										<th>
										</th>
										<Eav:TransLit runat="server" ID="transHN" Key="name" TagName="th" />
										<Eav:TransLit runat="server" ID="TransLit1" Key="address" TagName="th" />
										<Eav:TransLit runat="server" ID="TransLit2" Key="postalcode" TagName="th" />
										<Eav:TransLit runat="server" ID="TransLit3" Key="city" TagName="th" />
										<Eav:TransLit runat="server" ID="TransLit4" Key="phone" TagName="th" />
									</tr>
								</thead>
								<tbody>
						</HeaderTemplate>
						<ItemTemplate>
							<tr clickurl='?id=<%# Eval("ID") %>'>
								<td>
									<a href='?id=<%# Eval("ID") %>'>
										<img src="/img/arrow.png" alt="" /></a>
								</td>
								<td>
									<%# Eval("FullName") %>
								</td>
								<td>
									<%# Eval("Address") %>
								</td>
								<td>
									<%# Eval("PostalCode") %>
								</td>
								<td>
									<%# Eval("City") %>
								</td>
								<td>
									<%# Eval("Phone") %>
								</td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
							</tbody> </table>
						</FooterTemplate>
					</asp:Repeater>
				</div>
				<!-- panel -->
				<div class="tabPanel" style="overflow: hidden;">
					<asp:Repeater runat="server" ID="repOpticiansInactive">
						<HeaderTemplate>
							<table class="stripe">
								<thead>
									<tr>
										<th>
										</th>
										<Eav:TransLit runat="server" ID="transHN" Key="name" TagName="th" />
										<Eav:TransLit runat="server" ID="TransLit11" Key="address" TagName="th" />
										<Eav:TransLit runat="server" ID="TransLit21" Key="postalcode" TagName="th" />
										<Eav:TransLit runat="server" ID="TransLit31" Key="city" TagName="th" />
										<Eav:TransLit runat="server" ID="TransLit41" Key="phone" TagName="th" />
									</tr>
								</thead>
								<tbody>
						</HeaderTemplate>
						<ItemTemplate>
							<tr clickurl='?id=<%# Eval("ID") %>'>
								<td>
									<a href='?id=<%# Eval("ID") %>'>
										<img src="/img/arrow.png" alt="" /></a>
								</td>
								<td>
									<%# Eval("FullName") %>
								</td>
								<td>
									<%# Eval("Address") %>
								</td>
								<td>
									<%# Eval("PostalCode") %>
								</td>
								<td>
									<%# Eval("City") %>
								</td>
								<td>
									<%# Eval("Phone") %>
								</td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
							</tbody> </table>
						</FooterTemplate>
					</asp:Repeater>
				</div>
			</div>
		</div>
	</asp:PlaceHolder>
	<asp:PlaceHolder runat="server" ID="plhEdit">
		<div class="span nomarg column">
			<Eav:TransLit runat="server" ID="TransLit16" Key="name" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbName" MaxLength="50" />
			<asp:RequiredFieldValidator runat="server" ID="reqName" ControlToValidate="tbName"
				Display="Dynamic">
				<Eav:TransLit runat="server" ID="TransLit19" Key="required" TagName="div" CssClass="errorInline" />
			</asp:RequiredFieldValidator>
			<Eav:TransLit runat="server" ID="TransLit15" Key="address" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbAddress" MaxLength="100" />
			<Eav:TransLit runat="server" ID="TransLit14" Key="postalcode" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbPostalCode" MaxLength="50" />
			<Eav:TransLit runat="server" ID="TransLit13" Key="city" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbCity" MaxLength="50" />
			<Eav:TransLit runat="server" ID="TransLit12" Key="state" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbState" MaxLength="50" />
			<Eav:TransLit runat="server" ID="TransLit10" Key="country" TagName="div" CssClass="fieldTitle" />
			<asp:DropDownList runat="server" ID="ddlCountry" />
		</div>
		<div class="span column">
			<div class="fieldTitle">
				E-mail</div>
			<asp:TextBox runat="server" ID="tbEmail" MaxLength="200" />
			<asp:RequiredFieldValidator runat="server" ID="reqTbName" ControlToValidate="tbEmail"
				Display="Dynamic">
				<Eav:TransLit runat="server" ID="TransLit17" Key="required" TagName="div" CssClass="errorInline" />
			</asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="regExpEmail" runat="server" ControlToValidate="tbEmail"
				Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
				<Eav:TransLit runat="server" ID="TransLit18" Key="enterValidEmail" TagName="div"
					CssClass="errorInline" />
			</asp:RegularExpressionValidator>
			<Eav:TransLit runat="server" ID="TransLit9" Key="mobilePhone" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbMobilePhone" MaxLength="50" />
			<Eav:TransLit runat="server" ID="TransLit8" Key="phone" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbPhone" MaxLength="50" />
			<Eav:TransLit runat="server" ID="TransLit7" Key="active" TagName="div" CssClass="fieldTitle" />
			<asp:CheckBox runat="server" ID="chkEnabled" />
			<div class="fieldTitle">Show on map?</div>
			<asp:CheckBox runat="server" ID="chkShowOnMap" />
			<Eav:TransLit runat="server" ID="TransLit6" Key="language" TagName="div" CssClass="fieldTitle" />
			<asp:DropDownList runat="server" ID="ddlLanguage" />
		</div>
		<div class="span column" runat="server" id="adminOptions">
			<div class="clear infobox">
				<asp:LinkButton runat="server" ID="lnkDeleteOptician" CssClass="btn negativesmall"
					OnClientClick="return confirm(tye.dicValue('confirm_deleteOptician'));"
					OnClick="eLnkDeleteOptician_Click">
					<Eav:TransLit runat="server" ID="TransLit161" Key="delete" />
				</asp:LinkButton>
				<br />
				<asp:LinkButton runat="server" ID="lnkImpersonate" CssClass="btn positivesmall" OnClick="eLnkImpersonate_Click"
					CausesValidation="false">
				Impersonate this user
				</asp:LinkButton>
				<br />
				<asp:LinkButton runat="server" ID="lnkImpersonateWithLanguage" CssClass="btn positivesmall"
					OnClick="eLnkImpersonate_Click" CausesValidation="false">
				Impersonate with language
				</asp:LinkButton>
			</div>
			<div class="clear"><br /></div>
			<asp:Panel runat="server" ID="pnlOpticianCodes" Visible="true" CssClass="infobox">
				<h2>Activation codes</h2>
				<asp:Literal runat="server" ID="litRemainingCodes" />
				<div>
					<asp:HyperLink runat="server" ID="lnkPrintCodes" Visible="false" 
					 Target="_blank" NavigateUrl="activationCodePrint.aspx" Text="Print Codes" />
				</div>
				<p>
					<strong>Create more codes</strong>
				</p>
				Quantity:
				<asj:NumericTextBox runat="server" ID="ntbActivationCodesQuantity" NumberType="Integer"
					MinValue="1" MaxValue="50" Width="50" />
				<asp:LinkButton runat="server" ID="lnkCreateNewCodes" CssClass="btn positivesmall" OnClick="eLnkCreateNewCodes_Click" OnClientClick="tye.showOverlay();">
					<Eav:TransLit runat="server" ID="TransLit22" Key="save" />
				</asp:LinkButton>

				<br />
				<asp:HyperLink runat="server" ID="lnkActivationCodeReset" EnableViewState="false">Nulstil kode</asp:HyperLink>
			</asp:Panel>
		
			<div class="clear"><br /></div>
			<asp:Panel runat="server" ID="pnlOpticianOptions" Visible="true" CssClass="infobox">
				<h2>Options</h2>
				<asp:LinkButton runat="server" ID="lnkSendNewPassword" OnClientClick="return confirm(tye.dicValue('confirm_sure'));"  OnClick="ElnkSendNewPassword_Click" CausesValidation="false">Create and email new password to optician</asp:LinkButton>
			</asp:Panel>
		</div>
		<div class="clear">
			<asp:LinkButton runat="server" ID="lnkSave" CssClass="btn positive" OnClick="eLnkSave_Click">
				<Eav:TransLit runat="server" ID="TransLit61" Key="save" />
			</asp:LinkButton>
			<asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn positive" OnClick="eLnkSaveAndClose_Click">
				<Eav:TransLit runat="server" ID="transSave" Key="saveAndClose" />
			</asp:LinkButton>
			<a href="/opticians.aspx" class="btn negative">
				<Eav:TransLit runat="server" ID="TransLit5" Key="cancel" />
			</a>
		</div>
	</asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" runat="Server">
	include('/js/monoTabs.js', function() { $('#tabContainer').monoTabs( { cookie_name:	'opticians' } ); }); 
	include('/js/jquery.tablesorter.min.js', function() { 
		$('#tabContainer table')
			.addClass('tablesorter')
			.tablesorter()
			.bind("sortEnd", function(tbl) { 
				jquery_stripeTables({
					onlyVisible: true 
				}); 
			}); 
	}); 
	include('/js/jquery.doTimeout.js'); 
	include('/js/monoTableFilter.js');
	//onload_methods.push(function() { includerDebug('this is a test onload_method'); }); 
	onload_methods.push(function() { 
		$('#tabContainer table').monoTableFilter('#tableFilterPhrase',
			{ elementClearFilter: $('#tableFilterClear'), 
				cookieName: 'opticians', 
				eventAfterFilter:	function() { jquery_stripeTables({ onlyVisible: true }); }, 
				autoSetFocusToInput: true 
			}); 
		}
	);
</asp:Content>
