<%@ Page Title="Opticians" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true"
	CodeFile="umbService.aspx.cs" Inherits="umbService" %>
<%@ Register TagPrefix="umb" TagName="page" Src="~/controls/umbPageControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" runat="Server">
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" runat="Server">
	<umb:page runat="server" ID="umbPage" UmbracoPageID="1256" />
</asp:Content>
