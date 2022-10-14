<%@ Page Title="" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="eyeTestOpticianCreate.aspx.cs" Inherits="eyeTestOpticianCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<style type="text/css">
		div.eyeTestInfo { display:x-none; margin-bottom: 10px; }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">


    <h1>Create new test</h1>
    <strong>Name</strong>
            <br />
    
            <asp:TextBox runat="server" ID="tbEyeTestName"></asp:TextBox>
            <asp:Button runat="server" ID="btnNameSubmit" Text="Create" CssClass="positivesmall" OnClick="eBtnNameSubmit_Click" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
</asp:Content>

