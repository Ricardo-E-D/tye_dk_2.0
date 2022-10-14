<%@ Page Title="" Language="C#" MasterPageFile="~/masterGrand.master" AutoEventWireup="true" CodeFile="systemjob.aspx.cs" Inherits="systemjob" %>

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
						If the system doesn't know your email (or the forgot password features says it cannot find it) please go to <a href="loginFixEmail.aspx">this page</a> and enter your password for the <strong>old trainyoureyes.com</strong> and the <strong>new email</strong> you wish to use when logging in.<br />After that you may return here and use the "forgot password" feature to get a new password.
		</div>

		<table style="width:100%;">
			<tr>
				<td style="width:50%;">
					<div class="infobox" style="height:320px;width:175px;">
						<h2>keycard login</h2>
						<img src="/img/keycardPrint.png" style="width:169px;" alt="" />
						<asp:TextBox runat="server" ID="tbLoginCode" TextMode="Password" Width="100" />
						<div class="buttons">
						</div>
						<br />
					</div>
				</td>
				<td style="width:50%;">
					<asp:Panel runat="server" ID="pnlOpticianLogin" DefaultButton="lnkEmailLoginSubmit">
						<div class="infobox" style="height:320px;width:175px;">
							<h2>professional login</h2>
					
							<div class="fieldTitle">email</div>
							<asp:TextBox runat="server" ID="tbLoginEmail" Width="100" />
					
							<div class="fieldTitle">password</div>
							<asp:TextBox runat="server" ID="tbLoginPassword" TextMode="Password" Width="100" />
						
							<br /><br />
							<div class="buttons">
								<a href="#" onclick="$('#profhelp').slideToggle()">need help?</a><br />

								
							</div>
							<br /><br /><br />
						</div>
					</asp:Panel>
				</td>
			</tr>
		</table>
		<asp:Panel runat="server" ID="Panel1" DefaultButton="lnkCodeLogin">
			
		</asp:Panel>
	</div>
</asp:Content>