<%@ Control Language="C#" AutoEventWireup="true" CodeFile="findTheNumbers.ascx.cs" Inherits="controls_eyeTestScreen_findTheNumbers" %>

<div style="text-align:center;">

	<div id="scoreField" style="font-weight:bold; text-align:center">
		Score: 0
	</div>
	<table style="width: 100%; height: 100%; text-align: center">
		<tr>
			<td valign="middle" align="center">
				<table>
					<tr>
						<td valign="middle"><img src="/img/eyetest/findTheNumbers/spacer.png" id="img3D" alt="" /></td>
					</tr>
					<tr>
						<td>
							<img src="/img/eyetest/findTheNumbers/spacer.png" id="circlesImg" alt="" />
						</td>
							<td>
								<img src="/img/eyetest/findTheNumbers/spacer.png" id="rw1" alt="" /><br />
								<img src="/img/eyetest/findTheNumbers/spacer.png" id="rw2" alt="" /><br />
								<img src="/img/eyetest/findTheNumbers/spacer.png" id="rw3" alt="" /><br />
								<img src="/img/eyetest/findTheNumbers/spacer.png" id="rw4" alt="" /><br />
								<img src="/img/eyetest/findTheNumbers/spacer.png" id="rw5" alt="" /><br />
								<img src="/img/eyetest/findTheNumbers/spacer.png" id="rw6" alt="" />
							</td>
					</tr>
					<tr>
						<td colspan="2">
							<Eav:TransLit runat="server" ID="idTransNote" Key="eyeTest_findTheNumbersNote" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br /><br />
</div>

<div id="keypad" class="tablet-keypad" style="display:none;position:absolute;bottom: 0;left: 50%;">
    <div class="keypad-key">1</div>
    <div class="keypad-key">2</div>
    <div class="keypad-key">3</div>
    <div class="keypad-key">4</div>
    <div class="keypad-key">5</div>
    <div class="keypad-key">6</div>
</div>

<br /><br />

<script type="text/javascript" src="/js/dictionaryObject.js"></script>
<script type="text/javascript">
	<asp:Literal runat="server" ID="litScript" />
	$(function () {
		var img = document.getElementById("img3D");
		var rand = parseInt((Math.random()*2));
		img.src = imgDir + images[1][rand];

		$(document).on('keyup input', function(ev) {
			var Key = String.fromCharCode(ev.keyCode);
			var KeyPad = String.fromCharCode(ev.keyCode - 48);
			
			if(Key == "1" || Key == "2" || Key == "3" || Key == "4" || Key == "5" || Key == "6"){
				CheckBox(Key);
				//$('#debug').html($('#debug').html() + " " + Key);
			} else if(KeyPad == "1" || KeyPad == "2" || KeyPad == "3" || KeyPad == "4" || KeyPad == "5" || KeyPad == "6"){
				CheckBox(KeyPad);
				//$('#debug').html($('#debug').html() + " " + KeyPad);
			}
		});
		SwitchPictures();
		if(is_touch_device()) {
		    var keypad = $('#keypad'), w = keypad.width();
		    keypad.show().css('margin-left', (parseInt(w / 2, 10) * -1) + 'px');
		    $('.keypad-key').on('click', function() {
		        var t = $(this);
		        t.addClass('highlight');
		        setTimeout(function() {
		            t.removeClass('highlight');
		        }, 600);

		        var keyvalue = parseInt(t.html(), 10);
		        CheckBox(keyvalue);
		    });
		}
	});
	
	function is_touch_device() {
	    return 'ontouchstart' in window        // works on most browsers 
            || navigator.maxTouchPoints;       // works on IE10/11 and Surface
	};

	$(window).ready(function() {
		setTimeout(function() { eyeTestScreen.start(); }, 1000);
	});
</script>