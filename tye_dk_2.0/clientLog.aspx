<%@ Page Title="" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="clientLog.aspx.cs" Inherits="clientLog" %>
<%@ Register TagPrefix="tye" TagName="ClientLogUc" Src="~/controls/ClientLog.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<tye:ClientLogUc runat="server" ID="clientLogControl" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
</asp:Content>

