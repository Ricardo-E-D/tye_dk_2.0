﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="star.ascx.cs" Inherits="controls_eyeTestScreen_star" %>
<input type="hidden" runat="server" id="level" />

<div style="text-align:center;">

	<div id="scoreField" style="font-weight:bold; text-align:center">
		Score: 0
	</div>
	<table style="width: 100%; height: 100%; text-align: center">
		<tr>
			<td valign="middle" align="center">
				<table>
					<tr>
						<td valign="middle"><img src="" id="img3D"></td>
					</tr>
					<tr>
						<td style="text-align: right" valign="middle"><span id="textGuide" runat="server"></span><img src="/img/eyetest/star/exp.gif" usemap="#starmap" border="0" width="134" height="130"></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	 <map name="starmap">
		<area coords="52,26,75,49" shape="rectangle" onclick="DisplayImage('1');" style="cursor: pointer" href="#">
		<area coords="84,43,107,66" shape="rectangle" onclick="DisplayImage('2');"  href="#">
		<area coords="75,76,98,99" shape="rectangle" onclick="DisplayImage('3');" href="#">
		<area coords="48,82,71,105" shape="rectangle" onclick="DisplayImage('4');" href="#">
		<area coords="32,55,55,78" shape="rectangle" onclick="DisplayImage('5');" href="#">
		<area coords="61,55,84,78" shape="rectangle" onclick="DisplayImage('6');" href="#">
     </map>
	
	<br /><br />
	<input style="display:none;" type="button" value="Start" class="link positivesmall" id="btnStart" onclick="Start();" />
</div>

<br /><br />

<script type="text/javascript">
	$(function () {
		<asp:Literal runat="server" ID="litScript" />
		var img = document.getElementById("img3D");
		var rand = parseInt((Math.random()*2));
		img.src = imgDir + images[0][rand];

		$(document).on('keyup', function(ev) {
			var Key = String.fromCharCode(ev.keyCode);
			var KeyPad = String.fromCharCode(ev.keyCode - 48);
			
			if(Key == "1" || Key == "2" || Key == "3" || Key == "4" || Key == "5" || Key == "6"){
				DisplayImage(Key);
			} else if(KeyPad == "1" || KeyPad == "2" || KeyPad == "3" || KeyPad == "4" || KeyPad == "5" || KeyPad == "6"){
				DisplayImage(KeyPad);
			}
		});
	});
	$(window).ready(function() {
		setTimeout(function() { eyeTestScreen.start(); }, 1000);
	});
</script>