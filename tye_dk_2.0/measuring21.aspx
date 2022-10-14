<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="measuring21.aspx.cs" Inherits="measuring21" %>
<%@ Register TagPrefix="tye" TagName="anamneseMenu" Src="~/controls/anamneseMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<h1>
		<Eav:TransLit runat="server" ID="TransLit6" Key="mc21_name" />
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
							<%# ((DateTime)Eval("Created")).ToLocalTime() %>
						</a>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>

	</asp:Panel>

	<asp:Panel runat="server" ID="pnlMeasuring21Step1" Visible="false">
	
		<Eav:TransLit runat="server" ID="TransLit1" CssClass="fieldTitle" TagName="div" Key="mc_convergence" /> 
		<div class="well">
			<table>
				<tr>
					<td></td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit5" Key="mc21_dominance" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlDominans">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">{rightEye}</asp:ListItem>
							<asp:ListItem Value="2">{leftEye}</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit10" Key="mc21_coverTestFar" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlCoverTestFar">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">{ortoforia}</asp:ListItem>
							<asp:ListItem Value="2">{esoforia}</asp:ListItem>
							<asp:ListItem Value="3">{exoforia}</asp:ListItem>
							<asp:ListItem Value="4">{occasionallyEsotropia}</asp:ListItem>
							<asp:ListItem Value="5">{occasionallyExotropia}</asp:ListItem>
							<asp:ListItem Value="6">{constantRightEsotropia}</asp:ListItem>
							<asp:ListItem Value="7">{constantLeftEsotropia}</asp:ListItem>
							<asp:ListItem Value="8">{constantRightExotropia}</asp:ListItem>
							<asp:ListItem Value="9">{constantLeftExotropia}</asp:ListItem>
							<asp:ListItem Value="10">{alternatingEsotropia}</asp:ListItem>
							<asp:ListItem Value="11">{alternatingExotropia}</asp:ListItem>
							<asp:ListItem Value="12">{hyperforia}</asp:ListItem>
							<asp:ListItem Value="13">{hypoforia}</asp:ListItem>
							<asp:ListItem Value="14">{rightHypotropia}</asp:ListItem>
							<asp:ListItem Value="15">{leftHypotropia}</asp:ListItem>
							<asp:ListItem Value="16">{rightHypertropia}</asp:ListItem>
							<asp:ListItem Value="17">{leftHypertropia}</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit12" Key="mc21_coverTestNear" /> 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlCoverTestNear">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="1">{ortoforia}</asp:ListItem>
							<asp:ListItem Value="2">{esoforia}</asp:ListItem>
							<asp:ListItem Value="3">{exoforia}</asp:ListItem>
							<asp:ListItem Value="4">{occasionallyEsotropia}</asp:ListItem>
							<asp:ListItem Value="5">{occasionallyExotropia}</asp:ListItem>
							<asp:ListItem Value="6">{constantRightEsotropia}</asp:ListItem>
							<asp:ListItem Value="7">{constantLeftEsotropia}</asp:ListItem>
							<asp:ListItem Value="8">{constantRightExotropia}</asp:ListItem>
							<asp:ListItem Value="9">{constantLeftExotropia}</asp:ListItem>
							<asp:ListItem Value="10">{alternatingEsotropia}</asp:ListItem>
							<asp:ListItem Value="11">{alternatingExotropia}</asp:ListItem>
							<asp:ListItem Value="12">{hyperforia}</asp:ListItem>
							<asp:ListItem Value="13">{hypoforia}</asp:ListItem>
							<asp:ListItem Value="14">{rightHypotropia}</asp:ListItem>
							<asp:ListItem Value="15">{leftHypotropia}</asp:ListItem>
							<asp:ListItem Value="16">{rightHypertropia}</asp:ListItem>
							<asp:ListItem Value="17">{leftHypertropia}</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
			</table>
		</div>

		<div class="well">
			<table style="width:auto;">
				<tr>
					<td>
						<Eav:TransLit runat="server" ID="TransLit13" Key="mc21_fixationDisparity" /> 
					</td>
					<td style="width:300px;">
						<asp:TextBox runat="server" ID="tbFixation" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<td>
						<Eav:TransLit runat="server" ID="TransLit14" Key="mc21_stereopsisFar" /> 
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbStereopsisFar" MaxLength="150"  />
					</td>
				</tr>
				<tr>
					<td>
						<Eav:TransLit runat="server" ID="TransLit16" Key="mc21_stereopsisNear" /> 
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbStereopsisNear" MaxLength="150"  />
					</td>
				</tr>
				<tr>
					<td>
						<Eav:TransLit runat="server" ID="TransLit15" Key="mc21_pupilReflex" /> 
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbPupilReflex" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<td>
						<Eav:TransLit runat="server" ID="TransLit17" Key="mc21_visionField" /> 
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbVisionField" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<td>
						<Eav:TransLit runat="server" ID="TransLit18" Key="mc21_settingsAreaFrom" /> 
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbSettingsAreaFrom" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<td>
						<Eav:TransLit runat="server" ID="TransLit19" Key="mc21_settingsAreaTo" /> 
					</td>
					<td>
						<asp:TextBox runat="server" ID="tbSettingsAreaTo" MaxLength="150" />
					</td>
				</tr>
			</table>
		</div>
	
		<Eav:TransLit runat="server" ID="TransLit23" TagName="div" CssClass="fieldTitle" Key="mc21_visus" />
		<div class="well">
			<table>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit2" TagName="td" Key="right" />
					<td>
						<asp:DropDownList runat="server" ID="ddlVisusRight">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit38" Key="other" />: 
						<asp:TextBox runat="server" ID="tbVisusRightComment" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit24" TagName="td" Key="left" />
					<td>
						<asp:DropDownList runat="server" ID="ddlVisusLeft">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit37" Key="other" />: 
						<asp:TextBox runat="server" ID="tbVisusLeftComment" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit20" TagName="td" Key="binocular" />
					<td>
						<asp:DropDownList runat="server" ID="ddlVisusBinocular">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit36" Key="other" />: 
						<asp:TextBox runat="server" ID="tbVisusBinocularComment" MaxLength="150" />
					</td>
				</tr>
			</table>
		</div>

		<Eav:TransLit runat="server" ID="TransLit3" TagName="div" CssClass="fieldTitle" Key="mc21_habitualVisusCorrection" />
		<div class="well">
			<table>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit21" TagName="td" Key="right" />
					<td>
						<asp:DropDownList runat="server" ID="ddlHabitualVisusRight">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit35" Key="other" />: 
						<asp:TextBox runat="server" ID="tbHabitualVisusRightComment" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit22" TagName="td" Key="left" />
					<td>
						<asp:DropDownList runat="server" ID="ddlHabitualVisusLeft">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit34" Key="other" />: 
						<asp:TextBox runat="server" ID="tbHabitualVisusLeftComment" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit25" TagName="td" Key="binocular" />
					<td>
						<asp:DropDownList runat="server" ID="ddlHabitualVisusBinocular">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit33" Key="other" />: 
						<asp:TextBox runat="server" ID="tbHabitualVisusBinocularComment" MaxLength="150" />
					</td>
				</tr>
			</table>
		</div>

		<Eav:TransLit runat="server" ID="TransLit26" TagName="div" CssClass="fieldTitle" Key="mc21_pinholevisus" />
		<div class="well">
			<table>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit27" TagName="td" Key="right" />
					<td>
						<asp:DropDownList runat="server" ID="ddlPinHoleRight">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit32" Key="other" />: 
						<asp:TextBox runat="server" ID="tbPinHoleRightComment" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit28" TagName="td" Key="left" />
					<td>
						<asp:DropDownList runat="server" ID="ddlPinHoleLeft">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit31" Key="other" />: 
						<asp:TextBox runat="server" ID="tbPinHoleLeftComment" MaxLength="150" />
					</td>
				</tr>
				<tr>
					<Eav:TransLit runat="server" ID="TransLit29" TagName="td" Key="binocular" />
					<td>
						<asp:DropDownList runat="server" ID="ddlPinHoleBinocular">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="0,1">0,1</asp:ListItem>
							<asp:ListItem Value="0,2">0,2</asp:ListItem>
							<asp:ListItem Value="0,3">0,3</asp:ListItem>
							<asp:ListItem Value="0,4">0,4</asp:ListItem>
							<asp:ListItem Value="0,5">0,5</asp:ListItem>
							<asp:ListItem Value="0,6">0,6</asp:ListItem>
							<asp:ListItem Value="0,7">0,7</asp:ListItem>
							<asp:ListItem Value="0,8">0,8</asp:ListItem>
							<asp:ListItem Value="0,9">0,9</asp:ListItem>
							<asp:ListItem Value="1">1,0</asp:ListItem>
							<asp:ListItem Value="1,1">1,1</asp:ListItem>
							<asp:ListItem Value="1,2">1,2</asp:ListItem>
							<asp:ListItem Value="1,3">1,3</asp:ListItem>
							<asp:ListItem Value="1,4">1,4</asp:ListItem>
							<asp:ListItem Value="1,5">1,5</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td class="nowrap">
						<Eav:TransLit runat="server" ID="TransLit30" Key="other" />: 
						<asp:TextBox runat="server" ID="tbPinHoleBinocularComment" MaxLength="150" />
					</td>
				</tr>
			</table>
		</div>

		<div class="well">
			<table>
				<tr class="separate">
					<td>
						<a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#3</a>
					</td>
					<td><asj:NumericTextBox runat="server" ID="ntb21_3" MaxValue="360" Width="50" NumberType="Decimal" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_3">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="eso">eso</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="exo">exo</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="trans21001" Key="21point_3" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#13A</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_13a" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_13a">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="eso">eso</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="exo">exo</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit39" Key="21point_13A" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
				<tr class="separateSkipFirst">
					<td rowspan="2"><a class="link" onclick="$(this).parents('tr').next().next().fadeToggle()">#4</a></td>
					<td>H:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_4H">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_4Hsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						cyl: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_4Hcyl1" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_4Hcyl2" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						Visus: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_4Hvisus" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="separate">
					<td>V:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_4V">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_4Vsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						cyl: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_4Vcyl1" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_4Vcyl2" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						Visus: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_4Vvisus" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit40" Key="21point_4" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
				<tr class="separateSkipFirst">
					<td rowspan="2"><a class="link" onclick="$(this).parents('tr').next().next().fadeToggle()">#5</a></td>
					<td>H:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_5H">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_5Hsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						lag: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_5Hlag" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td></td>
					<td>
						netto: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_5Hnetto" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="separate">
					<td>V:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_5V">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_5Vsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						lag: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_5Vlag" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td></td>
					<td>
						netto: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_5Vnetto" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit41" Key="21point_5" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
				<tr class="separateSkipFirst">
					<td rowspan="2"><a class="link" onclick="$(this).parents('tr').next().next().fadeToggle()">#7</a></td>
					<td>H:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_7H">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7Hsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						cyl: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7Hcyl1" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7Hcyl2" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						Visus: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7Hvisus" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="separate">
					<td>V:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_7V">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7Vsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						cyl: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7Vcyl1" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7Vcyl2" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						Visus: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7Vvisus" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit42" Key="21point_7" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
				<tr class="separateSkipFirst">
					<td rowspan="2"><a class="link" onclick="$(this).parents('tr').next().next().fadeToggle()">#7A</a></td>
					<td>H:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_7aH">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7aHsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						cyl: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7aHcyl1" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7aHcyl2" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						Visus: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7aHvisus" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="separate">
					<td>V:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_7aV">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7aVsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						cyl: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7aVcyl1" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7aVcyl2" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						Visus: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_7aVvisus" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit43" Key="21point_7A" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#8</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_8" MinValue="-360" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_8">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="eso">eso</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="exo">exo</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit48" Key="21point_8" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#9</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" AllowNullValue="true" ID="ntb21_9" MinValue="-360" MaxValue="360" Width="50" /></td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit49" Key="21point_9" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#10</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_10_1" MinValue="-360" MaxValue="360" Width="50" />&nbsp; /</td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_10_2" MinValue="-360" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_10">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="SI">SI</asp:ListItem>
							<asp:ListItem Value="LO">LO</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit50" Key="21point_10" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#11</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_11_1" MinValue="-360" MaxValue="360" Width="50" />&nbsp; /</td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_11_2" MinValue="-360" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_11">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="SI">SI</asp:ListItem>
							<asp:ListItem Value="LO">LO</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit51" Key="21point_11" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="">
					<td rowspan="3"><a class="link" onclick="$(this).parents('tr').next().next().next().fadeToggle()">#12</a></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_12">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="eso">eso</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="exo">exo</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="separateSkipFirst">
					<td>
						H:
						<asp:RadioButton GroupName="21_12" runat="server" ID="rb21_12H"  />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_12Hs" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_12Hi" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="separate">
					<td>
						V:
						<asp:RadioButton GroupName="21_12" runat="server" ID="rb21_12V"  />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_12Vs" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_12Vi" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit44" Key="21point_12" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#13B</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_13b" MinValue="-360" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_13b">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="eso">eso</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="exo">exo</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit52" Key="21point_13B" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separateSkipFirst">
					<td rowspan="2"><a class="link" onclick="$(this).parents('tr').next().next().fadeToggle()">#14A</a></td>
					<td>H:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_14aH">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14aHsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						lag: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14aHlag" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td></td>
					<td>
						netto: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14aHnetto" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="separate">
					<td>V:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_14aV">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14aVsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						lag: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14aVlag" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td></td>
					<td>
						netto: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14aVnetto" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit45" Key="21point_14A" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#15A</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_15a" MinValue="-360" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_15a">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="eso">eso</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="exo">exo</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit53" Key="21point_15A" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
						
				<tr class="separateSkipFirst">
					<td rowspan="2"><a class="link" onclick="$(this).parents('tr').next().next().fadeToggle()">#14B</a></td>
					<td>H:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_14bH">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14bHsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						lag: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14bHlag" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td></td>
					<td>
						netto: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14bHnetto" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="separate">
					<td>V:</td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_14bV">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="+">+</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="-">-</asp:ListItem>
						</asp:DropDownList>
					</td>
					<td>
						sf: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14bVsf" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						lag: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14bVlag" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td></td>
					<td>
						netto: <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_14bVnetto" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit46" Key="21point_14B" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#15B</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_15b" MinValue="-360" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_15b">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="eso">eso</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="exo">exo</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit54" Key="21point_15B" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#16A</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" AllowNullValue="true" runat="server" ID="ntb21_16a" MinValue="-360" MaxValue="360" Width="50" /></td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit55" Key="21point_16A" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#16B</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_16b_1" MinValue="-360" MaxValue="360" Width="50" />&nbsp; /</td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_16b_2" MinValue="-360" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_16b">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="SI">SI</asp:ListItem>
							<asp:ListItem Value="LO">LO</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit56" Key="21point_16B" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#17A</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" AllowNullValue="true" runat="server" ID="ntb21_17a" MinValue="-360" MaxValue="360" Width="50" /></td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit57" Key="21point_17A" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#17B</a></td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_17b_1" MinValue="-360" MaxValue="360" Width="50" />&nbsp; /</td>
					<td><asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_17b_2" MinValue="-360" MaxValue="360" Width="50" /></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_17b">
							<asp:ListItem Value="0">{choose}</asp:ListItem>
							<asp:ListItem Value="SI">SI</asp:ListItem>
							<asp:ListItem Value="LO">LO</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit58" Key="21point_17B" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="">
					<td rowspan="3"><a class="link" onclick="$(this).parents('tr').next().next().next().fadeToggle()">#18</a></td>
					<td>	
						<asp:DropDownList runat="server" ID="ddl21_18">
							<asp:ListItem Value="-1">{choose}</asp:ListItem>
							<asp:ListItem Value="eso">eso</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="exo">exo</asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
				<tr class="separateSkipFirst">
					<td>
						H:
						<asp:RadioButton GroupName="21_18" runat="server" ID="rb21_18H"  />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_18Hs" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_18Hi" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="separate">
					<td>
						V:
						<asp:RadioButton GroupName="21_18" runat="server" ID="rb21_18V"  />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_18Vs" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_18Vi" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit47" Key="21point_18" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#19</a></td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit4" Key="right" />
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_19right" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit7" Key="left" />
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_19left" MinValue="-360" MaxValue="360" Width="50" />
					</td>
					<td>
						<Eav:TransLit runat="server" ID="TransLit8" Key="both" />
						<asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_19both" MinValue="-360" MaxValue="360" Width="50" />
					</td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit59" Key="21point_19" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#20</a></td>
					<td>- <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_20" MinValue="-360" MaxValue="360" Width="50" /></td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit60" Key="21point_20" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>

				<tr class="separate">
					<td><a class="link" onclick="$(this).parents('tr').next().fadeToggle()">#21</a></td>
					<td>+ <asj:NumericTextBox NumberType="Decimal" runat="server" ID="ntb21_21" MinValue="-360" MaxValue="360" Width="50" /></td>
				</tr>
				<tr class="hidden">
					<td colspan="7"><Eav:TransLit runat="server" ID="TransLit61" Key="21point_21" TagName="div" CssClass="well" IgnoreLineBreaks="true" /></td>
				</tr>
			</table>
		</div>



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

