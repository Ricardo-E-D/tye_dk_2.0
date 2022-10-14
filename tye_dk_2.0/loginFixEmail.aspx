<%@ Page Title="" Language="C#" MasterPageFile="~/masterGrand.master" AutoEventWireup="true" CodeFile="loginFixEmail.aspx.cs" Inherits="login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHmasterHead" Runat="Server">
<style type="text/css">
	.tight { margin-right: 0; }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHmasterContent" Runat="Server">
	
	<div style="text-align:center;width:422px;margin:auto;">
		<div style="text-align:left;">
			<asp:Panel runat="server" ID="pnlMessage" CssClass="errorInline info" Visible="False" />
		</div>

		<div id="profhelp" class="infobox hidden" style="width:392px;margin-bottom: 30px;text-align:left;">
						<h2>info for professionals logging on to the new TrainYourEyes for the first time</h2>
						The new trainyoureyes.com requires you to have an email registered in order to login.<br />
						If your email was already registered in the old system, you'll be able to 
						<ul>
							<li>enter that email here</li>
							<li>click the "forgot password?" button</li>
							<li>and receive a new password via email.</li>
						</ul><br />
						If the system doesn't know your email (or the forgot password features says it cannot find it) please to to <a href="loginFixEmail.aspx">this page</a> and enter your password for the <strong>old trainyoureyes.com</strong> and the <strong>new email</strong> you wish to use when logging in.<br />After that you may return here and use the "forgot password" feature to get a new password.
		</div>

		<table style="width:100%;">
			<tr>
				<td style="width:100%;">
						Your old password for trainyoureyes.com:
						<br />
						<asp:TextBox runat="server" ID="tbOldPwd" TextMode="Password"></asp:TextBox>

						<br /><br />
						Your email
						<asp:TextBox runat="server" ID="tbEmail"></asp:TextBox>

						<div class="buttons">
							<asp:LinkButton ID="lnkEmailLoginSubmit" runat="server" Text="send" CssClass="tight btn positive"
								OnClick="ElnkEmailLoginSubmit_Click" />
						</div>
						<br /><br /><br />
				</td>
			</tr>
		</table>
	</div>
</asp:Content>