<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	 <link rel="Stylesheet" href="/css/master.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    	<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
			 CellPadding="4" DataKeyNames="ID" DataSourceID="SqlDataSource1" 
			 ForeColor="#333333" GridLines="None">
			<AlternatingRowStyle BackColor="White" />
			<Columns>
				<asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
					ReadOnly="True" SortExpression="ID" />
				<asp:BoundField DataField="ProgramEyeTestID" HeaderText="ProgramEyeTestID" 
					SortExpression="ProgramEyeTestID" />
				<asp:BoundField DataField="StartTime" HeaderText="StartTime" 
					SortExpression="StartTime" />
				<asp:BoundField DataField="EndTime" HeaderText="EndTime" 
					SortExpression="EndTime" />
				<asp:BoundField DataField="Score" HeaderText="Score" SortExpression="Score" />
				<asp:CheckBoxField DataField="HighScore" HeaderText="HighScore" 
					SortExpression="HighScore" />
				<asp:BoundField DataField="AttribName" HeaderText="AttribName" 
					SortExpression="AttribName" />
				<asp:BoundField DataField="AttribValue" HeaderText="AttribValue" 
					SortExpression="AttribValue" />
				<asp:BoundField DataField="Comment" HeaderText="Comment" 
					SortExpression="Comment" />
				<asp:BoundField DataField="UpdateToken" HeaderText="UpdateToken" 
					SortExpression="UpdateToken" />
			</Columns>
			<EditRowStyle BackColor="#7C6F57" />
			<FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
			<HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
			<PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
			<RowStyle BackColor="#E3EAEB" />
			<SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
			<SortedAscendingCellStyle BackColor="#F8FAFA" />
			<SortedAscendingHeaderStyle BackColor="#246B61" />
			<SortedDescendingCellStyle BackColor="#D4DFE1" />
			<SortedDescendingHeaderStyle BackColor="#15524A" />
		 </asp:GridView>
		 <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
			 ConnectionString="<%$ ConnectionStrings:tempConnectionString %>" 
			 SelectCommand="SELECT * FROM [ClientEyeTestLog] ORDER BY [StartTime] DESC">
		 </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
