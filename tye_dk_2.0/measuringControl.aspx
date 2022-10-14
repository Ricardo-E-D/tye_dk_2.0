<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="measuringControl.aspx.cs" Inherits="measuringControl" %>
<%@ Register TagPrefix="tye" TagName="anamneseMenu" Src="~/controls/anamneseMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<h1>
		<asp:Literal runat="server" ID="litheading"></asp:Literal>
		<asp:HyperLink runat="server" ID="lnkBackToClient" NavigateUrl="?id=0" CssClass="plain" />
	</h1>

	<tye:anamneseMenu runat="server" ID="anamneseMenu" />

	<div class="clear"></div>

	<asp:Panel runat="server" ID="pnlList">
		<a href='?ClientUserID=<%= ClientUserID %>&MeasuringID=0<%= getRu() %>' class="positivesmall">
			<Eav:TransLit runat="server" ID="trnew" Key="createNew" />
		</a>

		<br /><br />
		<asp:Repeater runat="server" ID="repList">
			<HeaderTemplate>
				<table>
			</HeaderTemplate>
			<ItemTemplate>
				<tr clickurl='?ClientUserID=<%= ClientUserID %>&MeasuringID=<%# Eval("ID") %>&Step=1<%= getRu() %>'>
					<td>
						<a href='?ClientUserID=<%= ClientUserID %>&MeasuringID=<%# Eval("ID") %>&Step=1<%= getRu() %>'>
							<%# Eval("Created") %>
						</a>
					</td>
					<td>
						<%# Eval("Step1Changes") %>
					</td>
					<td>
						<%# Eval("Step1Comments")%>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>

	</asp:Panel>

	<asp:Panel runat="server" ID="pnlMeasuringControlStep1" Visible="false">
	
		<Eav:TransLit runat="server" ID="TransLit1" CssClass="fieldTitle" TagName="div" Key="mc_convergence" /> 
		<img class="link tooltip vm" alt="Help" src="/img/question.png" onclick="$(this).next().slideToggle();" />
		<Eav:TransLit runat="server" ID="TransLit20" CssClass="well hidden" TagName="div" Key="convergenceMeasuringInfo" /> 
		
		<div class="well">
			<table>
				<tr>
					<td>1. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit5" Key="mc_convergenceStep1" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMc11">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">0-5 cm</asp:ListItem>
							<asp:ListItem Value="2">5-10 cm</asp:ListItem>
							<asp:ListItem Value="3">10-15 cm</asp:ListItem>
							<asp:ListItem Value="4">15-20 cm</asp:ListItem>
							<asp:ListItem Value="5">20-25 cm</asp:ListItem>
							<asp:ListItem Value="6">25-30 cm</asp:ListItem>
							<asp:ListItem Value="7">30-35 cm</asp:ListItem>
							<asp:ListItem Value="8">35-40 cm</asp:ListItem>
							<asp:ListItem Value="9">&gt; 40 cm</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td>2. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit10" Key="mc_convergenceStep2" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMc12">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">{no}</asp:ListItem>
							<asp:ListItem Value="2">{rightEye}</asp:ListItem>
							<asp:ListItem Value="3">{leftEye}</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td>3. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit12" Key="mc_convergenceStep3" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMc13">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">{yes}</asp:ListItem>
							<asp:ListItem Value="2">{no}</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
			</table>
		</div>

		<Eav:TransLit runat="server" ID="TransLit2" CssClass="fieldTitle" TagName="div" Key="mc_motility" /> 
		<img class="link tooltip vm" alt="Help" src="/img/question.png" onclick="$(this).next().slideToggle();" />
		<Eav:TransLit runat="server" ID="TransLit19" CssClass="well hidden" TagName="div" Key="motilityMeasuringInfo" /> 

		<div class="well">
			<table style="width:auto;">
				<tr>
					<td colspan="3"></td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit3" CssClass="fieldTitle" TagName="div" Key="notes" /> 
					</td>
				</tr>
				<tr>
					<td>1. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit13" Key="mc_motilityStep1" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMotility1">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td style="width:300px;">
						<asp:TextBox runat="server" ID="tbMotility1" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<td>2. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit14" Key="mc_motilityStep2" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMotility2">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbMotility2" MaxLength="150"  />
					</td>
				</tr>
				<tr>
					<td>3. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit16" Key="mc_motilityStep3" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMotility3">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbMotility3" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<td>4. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit17" Key="mc_motilityStep5" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMotility5">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">{no}</asp:ListItem>
							<asp:ListItem Value="2">{yesSlightJitter}</asp:ListItem>
							<asp:ListItem Value="3">{yesHeavyJitter}</asp:ListItem>
							<asp:ListItem Value="4">{motilityEyesJumped}</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbMotility5" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<td>5. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit15" Key="mc_motilityStep4" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMotility4">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">{yes}</asp:ListItem>
							<asp:ListItem Value="2">{no}</asp:ListItem>
							<asp:ListItem Value="3">{dontKnow}</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbMotility4" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<td>6. </td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit18" Key="mc_motilityStep6" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlMotility6">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">{yes}</asp:ListItem>
							<asp:ListItem Value="2">{no}</asp:ListItem>
							<asp:ListItem Value="3">{dontKnow}</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbMotility6" MaxLength="150" />
					</td>
				</tr>
			</table>
		</div>
	
		<asp:PlaceHolder runat="server" ID="plhGenerel">
			<Eav:TransLit runat="server" ID="TransLit4" CssClass="fieldTitle" TagName="div" Key="general" /> 
			<div class="well">
				<Eav:TransLit runat="server" ID="TransLit7" CssClass="fieldTitle" TagName="div" Key="mc_changeSinceLast" /> 
				<asp:TextBox runat="server" ID="tbStep1Changes" TextMode="MultiLine" Height="80"></asp:TextBox>

				<Eav:TransLit runat="server" ID="TransLit8" CssClass="fieldTitle" TagName="div" Key="comments" /> 
				<asp:TextBox runat="server" ID="tbStep1Comments" TextMode="MultiLine" Height="80"></asp:TextBox>
			</div>
		</asp:PlaceHolder>

		<div class="clear">
			<asp:LinkButton runat="server" ID="lnkStep1Next" CssClass="btn positive" OnClick="eLnkNext_Click">
				<Eav:TransLit runat="server" ID="TransLit9" Key="save" />
			</asp:LinkButton>
			<a href='<%= VC.QueryStringStripNoTrail("MeasuringID") %>' class="btn negative"><Eav:TransLit runat="server" ID="TransLit11" Key="cancel" /></a>
		</div>

	</asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/js/stickToBottom.js');
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

