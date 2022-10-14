<%@ Page Title="" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="loginChangePassword.aspx.cs" Inherits="loginChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
<table style="margin: auto;" cellspacing="20">
		<tr>
			<td>
				<div id="divLogin">
					<br /><br />
					<asp:Panel runat="server" ID="pnlChangePassword" DefaultButton="btnChangePassword">
						<div class="fieldLabel">
							<Eav:TransLit runat="server" ID="qq23" Key="pleaseChangePassword"></Eav:TransLit>
						</div>
						<asp:TextBox runat="server" ID="tbP" TextMode="Password" />
					
						<asp:RegularExpressionValidator ID="valiRegExpNewUserPassword" runat="server" 
										ControlToValidate="tbP"
										Display="Dynamic" 
										ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$" />
						<br />

						<div class="buttons">
							<asp:LinkButton ID="btnChangePassword" runat="server" CssClass="btn positive"
								OnClick="eBtnChangePassword_Click">
									<Eav:TransLit runat="server" ID="TransLit1" Key="change"></Eav:TransLit>
								</asp:LinkButton>
						</div>
					</asp:Panel>
				</div>
			</td>
		</tr>
	</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
</asp:Content>

