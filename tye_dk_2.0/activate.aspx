<%@ Page Title="" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="activate.aspx.cs" Inherits="activate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">

	<asp:PlaceHolder runat="server" ID="plhExpired" Visible="false">
		<Eav:TransLit runat="server" ID="lit1" Key="codeExpired"></Eav:TransLit>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder runat="server" ID="plhActivate">
		
        
        <Eav:TransLit runat="server" ID="TransLit2" Key="codeActivateTerms"></Eav:TransLit>
        <br />
		<br />

        <Eav:TransLit runat="server" ID="TransLit1" Key="codeActivationInstruction"></Eav:TransLit>
        <br />
	</asp:PlaceHolder>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
</asp:Content>

