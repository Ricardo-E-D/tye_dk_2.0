<%@ Control Language="C#" AutoEventWireup="true" CodeFile="3dBalls.ascx.cs" Inherits="controls_eyeTestScreen_3dBalls" %>
<input type="hidden" runat="server" id="level" />

<table style="width: 100%; height: 100%; text-align: center; margin-top:20%;">
	<tr>
		<td valign="middle" align="center">
			<table>
				<tr>
					<td align="center"><img src="/img/eyetest/3dBalls/bouncing.gif" width="292" height="138" alt="" /></td>
				</tr>
				<tr>
					<td>
						<Eav:TransLit runat="server" ID="litInfo" Key="eyetest_3dBalls_info" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

<script type="text/javascript">
	onload_methods.push(function () { $('#hidScore').val('50'); eyeTestScreen.start(); });
</script>