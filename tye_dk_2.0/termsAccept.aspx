<%@ Page Title="" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="termsAccept.aspx.cs" Inherits="termsAccept" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">

	<h1>Personal data proction policies</h1>

	<Eav:TransLit runat="server" ID="oneoneone" Key="opticianTermsIntro"></Eav:TransLit>
	<br />
	<br />

	<a href="https://www.tye.dk/personal-data-proction-policies.aspx" target="_blank">Personal data proction policies</a>

	<br />

	<asp:Button runat="server" ID="btnTermsAccept" CssClass="btn positive" Text="Accept" OnClick="btnTermsAccept_Click" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
</asp:Content>

