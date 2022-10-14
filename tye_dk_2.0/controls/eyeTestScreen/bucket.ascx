<%@ Control Language="C#" AutoEventWireup="true" CodeFile="bucket.ascx.cs" Inherits="controls_eyeTestScreen_bucket" %>

<div style="text-align:center;">
	

	<table style="width: 100%; height: 100%; text-align: center">
		<tr>
			<td>
				<div style="text-align:center;" class="eyeTestScreenTopMenu">
					<a class="negativesmall link" onclick="setLevel(this, 1);">1</a>
					<a class="positivesmall link" onclick="setLevel(this, 2);">2</a>
				</div>
				<br />
			</td>
		</tr>
		<tr>
			<td valign="middle" align="center">
				<div class="eyeTestTime">&nbsp;</div>
				<br />
				<img src="/img/eyetest/bucket/1.png" id="img3d" alt="" />
			</td>
		</tr>
		<tr>
			<td>
				<br /><br />	
				<input type="button" class="positivesmall link" value="Start" onclick="Start();" id="btnStart" />
			</td>
		</tr>
	</table>
	
	
</div>

<script type="text/javascript">
	<asp:Literal runat="server" ID="litScript" />
	$(function () { });
	
</script>