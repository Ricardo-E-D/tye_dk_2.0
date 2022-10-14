<%@ Control Language="C#" AutoEventWireup="true" CodeFile="letterHunt.ascx.cs" Inherits="controls_eyeTestScreen_letterHunt" %>
<input type="hidden" runat="server" id="level" />


<div style="text-align:center;" class="eyeTestScreenTopMenu">
	<asp:Literal runat="server" ID="litMenu" EnableViewState="false"></asp:Literal>
</div>

<br /><br />

<table align="center">
	<tr>
		<td colspan="2">
			<div style="float:left;"><a style="cursor:pointer;" onclick="toggleColorize(this);">Color off</a></div>
			<div style="float:right;text-align:right;">
				<div style="display:inline-block;" class="eyeTestTime">&nbsp;</div>
				<div style="display:inline-block;margin-left: 20px;text-align:right;">
					<Eav:TransLit runat="server" ID="transScore" Key="score"></Eav:TransLit>: 
				</div>
				<div style="display:inline-block;" class="score">0</div>
			</div>
			<br /><br />
		</td>
	</tr>
	<tr>
		<td><div ID="textLabel" Runat="server"></div></td>
		<td><div id="rightMenuLabel" class="rightMenuText" style="line-height: 14px;"><asp:Literal runat="server" ID="litRightMenu" EnableViewState="false" /></div></td>
	</tr>
</table>
<input type="hidden" id="hideColor" value="off" />

<br /><br />
