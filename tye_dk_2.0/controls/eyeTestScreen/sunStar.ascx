<%@ Control Language="C#" AutoEventWireup="true" CodeFile="sunStar.ascx.cs" Inherits="controls_eyeTestScreen_sunStar" %>
	
	<br />
	<br />
	<table align="center">
		<tr>
			<td width="230" valign="top"><div style="position: relative;">
				<div style="z-index: 100; position:absolute"><img src="" border="0" id="img1" alt="" /></div>
				<div style="background-image: url('/img/eyetest/sunStar/dither.gif'); width: 220px; height:220px; position:absolute; z-index: 1000">&nbsp;</div>
				</div></td>
			<td  width="230"><img src="" border="0" id="img2" alt="" /></td>
			<td  width="230" valign="top"><div style="position: relative;">
				<div style="z-index: 100; position:absolute"><img src="" border="0" id="img3" alt="" /></div>
				<div style="background-image: url('/img/eyetest/sunStar/dither.gif'); width: 220px; height:220px; position:absolute; z-index: 1000">&nbsp;</div>
				</div></td>
		</tr>
		<tr>
			<td colspan="3" align="center" valign="middle"><br>
				<br />
				<Eav:TransLit runat="server" ID="t1" Key="slower" TagName="span"></Eav:TransLit>
				<span id="slowerDiv" runat="server"></span> <img src="/img/eyetest/sunStar/speedleftbtn.gif" onclick="SpeedDown()" style="cursor: pointer;" width="35" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed1" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed2" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed3" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed4" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedoff.gif" id="speed5" width="16" height="20" alt="" /><img src="/img/eyetest/sunStar/speedrightbtn.gif" onclick="SpeedUp()" style="cursor: pointer;" width="37" height="20" alt="" /> <Eav:TransLit runat="server" ID="TransLit1" Key="faster" TagName="span"></Eav:TransLit></td>
		</tr>
	</table><br>
	<center>
		<input type="button" class="positivesmall link" value="Start" onclick="Stop();" id="btn" />
	</center>


<script type="text/javascript">
	$(function () { SetSpeedImage('on'); SetPictures(); updateHidSpeed() });
</script>