<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="clients.aspx.cs" Inherits="clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	
	<h1>
		<Eav:TransLit runat="server" ID="TranslationLiteral3" Key="clients" />
	</h1>

	<asp:PlaceHolder runat="server" ID="plhList">
	<p>
		<Eav:TransLit runat="server" ID="traCountry" Key="country" Visible="false" /> <asp:DropDownList runat="server" ID="ddlFilterCountry" 
			AutoPostBack="true" onselectedindexchanged="ddlFilterCountry_SelectedIndexChanged" Visible="false" />
			<Eav:TransLit runat="server" ID="TransLit30" Key="search" />: <input type="text" id="tableFilterPhrase" style="width:100px;" /> <img id="tableFilterClear" src="/img/clear.png" class="link" alt="" />

			&nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal runat="server" ID="litRemainingCodes" /> 
			<asp:DropDownList runat="server" ID="ddlUseCode" Width="100" />
			<asp:LinkButton runat="server" ID="lnkCreateNew" CssClass="positivesmall" OnClick="eLnkCreateNew_Click">
				<Eav:TransLit runat="server" ID="TransLit23" Key="useCodeAndCreateClient" />
			</asp:LinkButton>
	</p>
	<div class="tabContainer" id="tabContainer">
      <div class="innerTabs">
            <ul>
                  <li>
							<Eav:TransLit runat="server" ID="TranslationLiteral2" Key="active" />
							(<asp:Literal runat="server" ID="litActiveCount" />)
						</li>
                  <li>
							<Eav:TransLit runat="server" ID="TranslationLiteral1" Key="inactive" />
							(<asp:Literal runat="server" ID="litInactiveCount" />)
						</li>
            </ul>
      </div>
      <div class="innerContainer">
			<!-- panel -->
         <div class="tabPanel" style="overflow: hidden;">
				<asp:Repeater runat="server" ID="repClientsActive">
					<HeaderTemplate>
						<table class="stripe">
							<thead>
								<tr>
									<th></th>
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
								<a href='?id=<%# Eval("ID") %>'><img src="/img/arrow.png" alt="" /></a>
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
							</tbody>
						</table>
					</FooterTemplate>
				</asp:Repeater>
			</div>
			<!-- panel -->
         <div class="tabPanel" style="overflow: hidden;">
				<asp:Repeater runat="server" ID="repClientsInactive">
					<HeaderTemplate>
						<table class="stripe">
							<thead>
								<tr>
									<th></th>
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
								<a href='?id=<%# Eval("ID") %>'><img src="/img/arrow.png" alt="" /></a>
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
							</tbody>
						</table>
					</FooterTemplate>
				</asp:Repeater>
			</div>
		</div>
	</div>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhEdit">
		<script type="text/javascript" src="js/navigateaway.js"></script>
		
		<div class="clear"></div>
		
		<div class="span nomarg column">
			<Eav:TransLit runat="server" ID="TransLit16" Key="firstName" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbFirstName" MaxLength="50" />
			<asp:RequiredFieldValidator runat="server" ID="rqFn" ControlToValidate="tbFirstName" Display="Dynamic">
				<Eav:TransLit runat="server" ID="TransLit24" Key="required" TagName="div" CssClass="errorInline" />
			</asp:RequiredFieldValidator>

			<Eav:TransLit runat="server" ID="TransLit19" Key="middleName" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbMiddleName" MaxLength="50" />

			<Eav:TransLit runat="server" ID="TransLit20" Key="lastName" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbLastName" MaxLength="50" />
			<asp:RequiredFieldValidator runat="server" ID="reqLn" ControlToValidate="tbLastName" Display="Dynamic">
				<Eav:TransLit runat="server" ID="TransLit25" Key="required" TagName="div" CssClass="errorInline" />
			</asp:RequiredFieldValidator>

			<Eav:TransLit runat="server" ID="TransLit22" Key="birthday" TagName="div" CssClass="fieldTitle" />
			<asj:DateTimePicker runat="server" ID="dpBirthday" AllowClear="false" DisplayType="TextBoxOnly" Mode="Date" OnClientShow="function(tb,cal) { fixDtpPosition(this,tb,cal); }" ShowYearsBefore="80" ShowYearsAfter="0" CssClass="ignorenavigateaway" />
			<asp:RequiredFieldValidator runat="server" ID="reqBd" ControlToValidate="dpBirthday" Display="Dynamic">
				<Eav:TransLit runat="server" ID="TransLit26" Key="required" TagName="div" CssClass="errorInline" />
			</asp:RequiredFieldValidator>

			<Eav:TransLit runat="server" ID="TransLit15" Key="address" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbAddress" MaxLength="100" />
			<%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="tbAddress" Display="Dynamic">
				<Eav:TransLit runat="server" ID="TransLit27" Key="required" TagName="div" CssClass="errorInline" />
			</asp:RequiredFieldValidator>
			--%>
			<Eav:TransLit runat="server" ID="TransLit14" Key="postalcode" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbPostalCode" MaxLength="50" />
			<%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="tbPostalCode" Display="Dynamic">
				<Eav:TransLit runat="server" ID="TransLit29" Key="required" TagName="div" CssClass="errorInline" />
			</asp:RequiredFieldValidator>--%>

			<Eav:TransLit runat="server" ID="TransLit13" Key="city" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbCity" MaxLength="50" />
			<%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="tbCity" Display="Dynamic">
				<Eav:TransLit runat="server" ID="TransLit28" Key="required" TagName="div" CssClass="errorInline" />
			</asp:RequiredFieldValidator>--%>
		</div>
		<div class="span column">
			<Eav:TransLit runat="server" ID="TransLit12" Key="state" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbState" MaxLength="50" />
		
			<Eav:TransLit runat="server" ID="TransLit10" Key="country" TagName="div" CssClass="fieldTitle" />
			<asp:DropDownList runat="server" ID="ddlCountry" />

			<div class="fieldTitle">E-mail</div>
			<asp:TextBox runat="server" ID="tbEmail" MaxLength="200" />
			

			<Eav:TransLit runat="server" ID="TransLit9" Key="mobilePhone" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbMobilePhone" MaxLength="50" />

			<Eav:TransLit runat="server" ID="TransLit8" Key="phone" TagName="div" CssClass="fieldTitle" />
			<asp:TextBox runat="server" ID="tbPhone" MaxLength="50" />

			<Eav:TransLit runat="server" ID="TransLit7" Key="active" TagName="div" CssClass="fieldTitle" />
			<asp:CheckBox runat="server" ID="chkEnabled" />

			<Eav:TransLit runat="server" ID="TransLit6" Key="language" TagName="div" CssClass="fieldTitle" />
			<asp:DropDownList runat="server" ID="ddlLanguage" />
		</div>
		<div class="span column">
			<asp:Panel CssClass="clear infobox" runat="server" ID="clientTools">
				<asp:LinkButton runat="server" ID="lnkDeleteClient" CssClass="btn negativesmall" OnClientClick="return confirm(tye.dicValue('confirm_delete'));" CausesValidation="false" Visible="false"> ! virker ikke ! 
					<Eav:TransLit runat="server" ID="TransLit161" Key="delete" />
				</asp:LinkButton>
				<asp:LinkButton runat="server" ID="lnkImpersonate" CssClass="btn positivesmall" OnClick="eLnkImpersonate_Click" CausesValidation="false" Visible="false">
					Impersonate this user
				</asp:LinkButton>
			</asp:Panel>
			<div class="clear"><br /></div>
			<div class="clear infobox">

				<asp:HyperLink runat="server" ID="lnkToStartMeasuring">
				</asp:HyperLink>
				<br />

				<asp:HyperLink runat="server" ID="lnkToAnamnese">
				</asp:HyperLink>
				<br />
				
				<asp:HyperLink runat="server" ID="lnkToMeasuring21"></asp:HyperLink>
				<br />
				
				<asp:HyperLink runat="server" ID="lnkToMeasuringControl">
				</asp:HyperLink>
				<br />
				<br />
				<asp:PlaceHolder runat="server" ID="pnlCreateClientProgram">
					<asp:LinkButton runat="server" ID="lnkCreateClientProgram" OnClick="ElnkCreateClientProgram_Click">
						<Eav:TransLit runat="server" ID="TransLit32" Key="generateClientProgram" />
					</asp:LinkButton>
				
					<Eav:TransLit runat="server" ID="transClientGenerateProgramWarning" Key="generateClientProgramAnamneseMissing" TagName="div" CssClass="note" Visible="false" />

					<br /><br />
				</asp:PlaceHolder>
				
				<asp:HyperLink runat="server" ID="lnkToClientLog">
					<Eav:TransLit runat="server" ID="TransLit34" Key="clientLog" />
				</asp:HyperLink>
				<br />
				<asp:HyperLink runat="server" ID="lnkToProgram">
					<Eav:TransLit runat="server" ID="t1" Key="eyeTestProgram" />
				</asp:HyperLink>
				<br /><br />

				<asp:Literal runat="server" ID="litCode"></asp:Literal>
				<br />
				
				<Eav:TransLit runat="server" ID="TransLit35119" Key="createdOn"></Eav:TransLit>: 
				<asp:Literal runat="server" ID="litCreatedOn" EnableViewState="false" />. 
				<br />
				<asp:PlaceHolder runat="server" ID="plhExpiresOn">
					<Eav:TransLit runat="server" ID="TransLit35" Key="expiresOn" />
					<asj:DateTimePicker runat="server" ID="dpExpirationDate" DisplayType="TextBoxOnly" Mode="Date" OnClientShow="calendarBelowTextbox" />
				</asp:PlaceHolder>

			</div>
		</div>
		<div class="clear stickToBottom">
			<asp:LinkButton runat="server" ID="lnkSave" CssClass="nonavigate btn positive" OnClick="eLnkSave_Click">
				<Eav:TransLit runat="server" ID="TransLit61" Key="save" TagName="th" />
			</asp:LinkButton>
			<asp:LinkButton runat="server" ID="LinkButton1" CssClass="nonavigate btn positive" OnClick="eLnkSaveAndClose_Click">
				<Eav:TransLit runat="server" ID="transSave" Key="saveAndClose" TagName="th" />
			</asp:LinkButton>
			
			<a href="/clients.aspx" class="nonavigate btn negative"><Eav:TransLit runat="server" ID="TransLit5" Key="cancel" /></a>
		</div>
	</asp:PlaceHolder>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/js/stickToBottom.js');
	include('/js/monoTabs.js', function() { $('#tabContainer').monoTabs( { cookie_name: 'clients' } ); });
		include('/js/jquery.tablesorter.min.js', function() { 
		$('#tabContainer table').addClass('tablesorter').tablesorter().bind("sortEnd", function(tbl) { 
			jquery_stripeTables({ onlyVisible: true });
    });
	});
	include('/js/jquery.doTimeout.js');
	include('/js/monoTableFilter.js');
	include('/js/page/clients.js');
	onload_methods.push(function() { 
		$('#tabContainer table').monoTableFilter('#tableFilterPhrase', { 
			elementClearFilter: $('#tableFilterClear'), 
			cookieName: 'clients',
			eventAfterFilter: function() { jquery_stripeTables({ onlyVisible: true }); },
			autoSetFocusToInput: true
		});
	});
</asp:Content>

