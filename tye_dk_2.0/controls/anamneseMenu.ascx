<%@ Control Language="C#" AutoEventWireup="true" CodeFile="anamneseMenu.ascx.cs" Inherits="controls_anamneseMenu" %>
<div>
	<asp:HyperLink runat="server" ID="lnkToAnamnese">
		<Eav:TransLit runat="server" ID="TransLit1" Key="anamnese" />
	</asp:HyperLink>
	&nbsp;
	<asp:HyperLink runat="server" ID="lnkToStartMeasuringControl">
		<Eav:TransLit runat="server" ID="TransLit2" Key="startMeasuring" />
	</asp:HyperLink>
	&nbsp;
	<asp:HyperLink runat="server" ID="lnkToMeasuringControl">
		<Eav:TransLit runat="server" ID="transM" Key="mc_namePlural" />
	</asp:HyperLink>
	&nbsp;
	<asp:HyperLink runat="server" ID="lnkToMeasuring21">
		<Eav:TransLit runat="server" ID="TransLit11" Key="mc21_name" />
	</asp:HyperLink>
</div>
<br />