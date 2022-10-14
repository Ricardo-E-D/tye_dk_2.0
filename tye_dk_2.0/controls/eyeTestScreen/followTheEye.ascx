<%@ Control Language="C#" AutoEventWireup="true" CodeFile="followTheEye.ascx.cs" Inherits="controls_eyeTestScreen_racer" %>

<div style="text-align:center;" class="eyeTestScreenTopMenu">
	<a class="positivesmall link" onclick="AnimateCurve('vertical');">
		<Eav:TransLit runat="server" ID="tl1" Key="vertical" />
	</a>
	<a class="positivesmall link" onclick="AnimateCurve('horizontal');">
		<Eav:TransLit runat="server" ID="TransLit1" Key="horizontal" />
	</a>
	<a class="positivesmall link" onclick="AnimateCurve('circular');">
		<Eav:TransLit runat="server" ID="TransLit2" Key="circular" />
	</a>
	<a class="positivesmall link" onclick="AnimateCurve('diagonal');">
		<Eav:TransLit runat="server" ID="TransLit3" Key="diagonal" />
	</a>
	<a class="positivesmall link" onclick="AnimateCurve('random');">
		<Eav:TransLit runat="server" ID="TransLit4" Key="random" />
	</a>
</div>

<div style="width: 700px; position: relative; height: 500px; margin:auto;">
		<img src="/img/eyetest/followTheEye/eye.jpg" id="eye" style="left: 0px; position: absolute; top: 0px" width="54" height="36" />
		<img src="/img/eyetest/followTheEye/aim.gif" id="aim" style="left: 350px; position: absolute; top: 250px" width="21" height="21" />
</div>