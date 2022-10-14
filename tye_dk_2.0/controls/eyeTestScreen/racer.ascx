<%@ Control Language="C#" AutoEventWireup="true" CodeFile="racer.ascx.cs" Inherits="controls_eyeTestScreen_racer" %>

<style type="text/css">
	.hideCursor {
    cursor: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAFBhaW50Lk5FVCB2My41LjbQg61aAAAADUlEQVQYV2P4//8/IwAI/QL/+TZZdwAAAABJRU5ErkJggg=='),
    url('/img/x.png'),
    none !important;
}
#maps { cursor:none !important; }
</style>
<meta name="viewport" content="user-scalable=1.0,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0">
<meta name="apple-mobile-web-app-capable" content="yes">
<meta name="format-detection" content="telephone=no">
<div id="debugger" style="display:none;position:absolute;top: 0;left: 0; width:250px;color:#ffffff;border: 1px solid #ffffff;"></div>

<div style="text-align:center;" class="eyeTestScreenTopMenu" style="z-index:100000;">
	<a class="positivesmall link map1a" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map1a'>1</a>
	<a class="positivesmall link map1b" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map1b'>2</a>
	<a class="positivesmall link map2a" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map2a'>3</a>
	<a class="positivesmall link map2b" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map2b'>4</a>
	<a class="positivesmall link map3a" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map3a'>5</a>
	<a class="positivesmall link map3b" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map3b'>6</a>
	<a class="positivesmall link map4a" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map4a'>7</a>
	<a class="positivesmall link map4b" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map4b'>8</a>
	<a class="positivesmall link map5a" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map5a'>9</a>
	<a class="positivesmall link map5b" href='?ID=<%= ProgramEyeTestID %>&IgnoreProgram=<%= VC.RqValue("IgnoreProgram") %>&map=map5b'>10</a>
</div>

<div style="position: absolute;width:100%; height: 100%" id="Div1">
	<input type="hidden" id="mapNum" value="map1a" runat="server" clientidmode="Static" />
	<div ID="letterMenu" runat="server" class="LabyrintMenu" style="WIDTH: 100%; TEXT-ALIGN: center">
				
	</div>
	<img src="/img/eyetest/racer/car/10.png" id="car" style="position:absolute; z-index:10000;border:0px solid #ffffff;" alt="" />
	<img src="/img/eyetest/racer/map/Level1.png" id="maps" style="position:absolute;border:0px solid #ffffff;" alt="" />
	<input type="hidden" id="exNo" name="exNo" value="-1" />
</div>

<script type="text/javascript">
	include('/js/eyetest/global/uri.js');
	//include('/js/eyetest/global/Time.js');
	onload_methods.push(
		function () {
			$('#hidAttribName').val('level');
			$('#hidAttribValue').val('1');
			DetectScreenSize();
			eyeTestScreen.messages.offTrack = '<%= DicValue("eyeTest_racer_offTrack").JsEncode() %>';
			eyeTestScreen.messages.congratulations = '<%= DicValue("eyeTest_racer_congratulations").JsEncode() %>';
			//setTimeout(function () { eyeTestScreen.start(); }, 2000);
			//setTimeout(function () { eyeTestScreen.stop(); }, 13000);
			//$('.eyeTestScreenTopMenu a').on('click', function () {
				$('.eyeTestScreenTopMenu a').addClass('positivesmall');
				$('.' + $('#mapNum').val()).removeClass('positivesmall').addClass('negativesmall');
			//});
		}
	);
</script>