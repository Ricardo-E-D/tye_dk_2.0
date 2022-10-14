<%@ Control Language="C#" AutoEventWireup="true" CodeFile="findTheFigure.ascx.cs" Inherits="controls_eyeTestScreen_findTheFigure" %>

<div style="text-align:center;">

	<div id="scoreField" style="font-weight:bold; text-align:center">
		Score: 0
	</div>

	<table style="width: 100%; height: 100%; text-align: center">
		<tr>
			<td valign="middle" align="center">
				<table>
					<tr>
						<td valign="middle"><img src="" id="img3D" alt="" /></td>
					</tr>
					<tr>
						<td style="text-align: center" valign="middle">
							<img src="/img/eyetest/findTheFigure/exp.gif" usemap="#starmap" border="0" width="407" height="113" alt="" />
							<br />
							<Eav:TransLit runat="server" ID="idTransNote" Key="eyeTest_3dTestDefaultNote" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br /><br />
</div>
	<map name="starmap" id="starmap">
		<area coords="33,28,93,88" shape="rect" onclick="DisplayImage('1');" style="cursor: pointer" href="#" alt="" />
		<area coords="126,28,186,88" shape="rect" onclick="DisplayImage('2');" href="#" alt="" />
		<area coords="224,28,284,88" shape="rect" onclick="DisplayImage('3');" href="#" alt="" />
		<area coords="306,28,366,88" shape="rect" onclick="DisplayImage('4');" href="#" alt="" />
	</map>
<br /><br />

<script type="text/javascript" src="/js/dictionaryObject.js"></script>
<script type="text/javascript">
	<asp:Literal runat="server" ID="litScript" />
	$(function () {
		var img = document.getElementById("img3D");
		img.src = imgDir + images[0][0];

		$(document).on('keyup', function(ev) {
			var Key = String.fromCharCode(ev.keyCode);
			var KeyPad = String.fromCharCode(ev.keyCode - 48);
			
			if(Key == "1" || Key == "2" || Key == "3" || Key == "4"){
				DisplayImage(Key);
			} else if(KeyPad == "1" || KeyPad == "2" || KeyPad == "3" || KeyPad == "4"){
				DisplayImage(KeyPad);
			}
		});
		//DisplayImage("1");
	});
	$(window).ready(function() {
		setTimeout(function() { eyeTestScreen.start(); }, 1000);
	});
</script>