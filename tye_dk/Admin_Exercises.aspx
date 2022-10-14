<%@ Reference Control="~/uc/pages/news.ascx" %>
<%@ Page language="c#" Inherits="tye._Default" CodeFile="Admin_Exercises.aspx.cs"  ValidateRequest="false"%>
<%@ Import Namespace="tye" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>TrainYourEyes.com</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
		<script src="_js.js" type="text/javascript" language="javascript"></script>
		<link href="stylesheet/tye.css" rel="stylesheet" type="text/css" />
		<asp:PlaceHolder id="head_ph" runat="server" />
		<!-- kontrol af javascript -->
		<%--<noscript>
			<meta http-equiv="refresh" content="1; URL=NoJavaScript.aspx">
		</noscript>--%>
		<!-- kontrol slutter -->
	</head>
	<body id="main_body" runat="server">
		<form id="main_form" runat="server">
			<table id="main_table" cellspacing="0" cellpadding="0">
				<tr>
					<td colspan="3" id="top_td" style="background-image:url('gfx/top_bg.gif');">						
						<div id="login_div" runat="server"></div>
						<div id="lefttop_div" runat="server"></div>
						<div id="language_div" runat="server"></div>
					</td>
				</tr>
				<tr>
					<td colspan="3" id="menu_td" runat="server" align="left">&nbsp;</td>
				</tr>
				<tr>	
					<td colspan="3" id="submenu_td" style="background-image:url('gfx/submenu_bg.gif');" runat="server" align="left"></td>
				</tr>
				<tr>
					<td rowspan="2" id="news_td" runat="server"></td>
				</tr>
				<tr>
					<td id="content_td"><div id="content_div" runat="server">
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">
                                <h5>Rediger tekst til øvelse...</h5>
                                <br />
                                <asp:Table ID="ex_table" runat="server">
                                </asp:Table>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <h5><asp:Label ID="ex_editLabel" runat="server" Text="Label"></asp:Label></h5>
                                <asp:Table ID="editNavTable" runat="server"><asp:TableRow ID="editNavTableRowOne" runat="server"></asp:TableRow></asp:Table>
                                <asp:Table ID="editTable" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell VerticalAlign="top"></asp:TableCell>
                                    <asp:TableCell VerticalAlign="top" ID="importantCell">
                                    </asp:TableCell>
                                </asp:TableRow>
                                </asp:Table>
                            </asp:View>
                        </asp:MultiView>
                    </div>
					</td>
					<td id="banner_td" runat="server"></td>
				</tr>
				<tr>
					<td colspan="3" id="bottom_td"><img src="gfx/bottom_arch.gif" id="bottom_arch" alt="Sidens bund" width="780" height="102" /></td>
				</tr>
			</table>
		</form>
	</body>
</html>
