<%@ Control Language="C#" AutoEventWireup="true"  %>
<input type="hidden" runat="server" id="level" />

<div style="text-align:center;" class="eyeTestScreenTopMenu">
	<a class="negativesmall link" onclick="setLevel(this, 1);">1</a>
	<a class="positivesmall link" onclick="setLevel(this, 2);">2</a>
	<a class="positivesmall link" onclick="setLevel(this, 3);">3</a>
	<a class="positivesmall link" onclick="setLevel(this, 4);">4</a>
	<a class="positivesmall link" onclick="setLevel(this, 5);">5</a>
</div>

<br />
<br />
<table align="center">
	<tr>
		<td width="230" valign="top"><div style="position: relative;">
			<div style="z-index: 100; position:absolute"><img src="" border="0" id="img1" alt="" /></div>
			<div style="background-image: url('/img/eyetest/clapStomp/dither.gif'); width: 220px; height:220px; position:absolute; z-index: 1000">&nbsp;</div>
			</div></td>
		<td  width="230"><img src="" border="0" id="img2" alt="" /></td>
		<td  width="230" valign="top"><div style="position: relative;">
			<div style="z-index: 100; position:absolute"><img src="" border="0" id="img3" alt="" /></div>
			<div style="background-image: url('/img/eyetest/clapStomp/dither.gif'); width: 220px; height:220px; position:absolute; z-index: 1000">&nbsp;</div>
			</div></td>
	</tr>	
	<tr>
		<td colspan="3" align="center">
		<br>
		<span id="slowerDiv" runat="server"></span><img src="/img/eyetest/sunStar/speedleftbtn.gif" onclick="SpeedDown()" style="cursor: pointer"
			width="35" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed1" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed2" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed3" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed4" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed5" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedrightbtn.gif" onclick="SpeedUp()" style="cursor: pointer"
			width="37" height="20" alt="" /> <span id="fasterDiv" runat="server"></span></td>
	</tr>
	<tr>
		<td colspan="3" align="center">
			<br />
			<input type="button" class="positivesmall link" value="Start" onclick="Stop();" id="btn" />
		</td>
	</tr>
</table>

<br /><br />
<script type="text/javascript">
	$(function () { updateHidSpeed(); SetPictures(); });
</script>