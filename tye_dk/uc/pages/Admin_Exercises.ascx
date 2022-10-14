<%@ Reference Control="~/uc/pages/uc_pages.ascx" %>
<%@ Control Language="c#" Inherits="tye.uc.pages.Admin_Exercises" CodeFile="Admin_Exercises.ascx.cs" CodeFileBaseClass="tye.uc.pages.uc_pages" %>


        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <!-- <h5>Rediger tekst til øvelse...</h5> -->
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
