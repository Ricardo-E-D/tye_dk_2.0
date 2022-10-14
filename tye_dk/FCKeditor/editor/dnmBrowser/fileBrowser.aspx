<%@ Page Language="VB" AutoEventWireup="false" CodeFile="fileBrowser.aspx.vb" Inherits="FCKeditor_editor_dnmBrowser_fileBrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Browser by mital</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellpadding=3 cellspacing=0 border=0>
            <tr>
                <td valign="top">
                </td>
                
                <td valign="top">
                    <asp:Table id="browserTable" runat="server">
                    </asp:Table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
