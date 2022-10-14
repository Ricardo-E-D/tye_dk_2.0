<%@ Control Language="C#" AutoEventWireup="true" CodeFile="saccade.ascx.cs" Inherits="controls_eyeTestScreen_saccade" %>
<input type="hidden" runat="server" id="level" />

<div style="text-align:center;">
	<input type="button" value="Start" class="link positivesmall" id="btnStart" onclick="Start();" />
    <input type="button" value="Stop" class="hidden link negativesmall" id="btnStop" onclick="Stop();" />
    <br />
	<div id="timer" style="font-weight:bold; text-align:center; font-size:20px;">
		60
	</div>
</div>

<table id="crossed" border="0">
    <tr>
        <td class="left">
            <div class="cross left"><img src="/img/eyetest/saccade/times.jpg" /></div>
        </td>
        <td class="right">
            <div class="cross right"><img src="/img/eyetest/saccade/times.jpg" /></div>
        </td>
    </tr>
</table>
<style type="text/css">
    body {
        background-color: white;
        color: black;
    }
    table {
        width: 100%;
    }
    td.right {
        text-align:right;
    }
    .cross.right {
        margin-right: 30%;
    }
    .cross.left {
        margin-left: 30%;
    }
    .cross img {
        width: 74px;
    }
</style>

<script type="text/javascript">
	$(function () {
		<asp:Literal runat="server" ID="litScript" />
    });

    var t = 60, testRunning = false;
    function itsTheFinalCountdown() {
        if (t >= 0) {
            $('#timer').html(t);
            setTimeout(function () {
                itsTheFinalCountdown();
            }, 1000);
        } else {
            $('.cross').hide();
            Stop(); //eyeTestScreen.stop();
        }
        t -= 1;
    }

    function resize() {
        var h = $(document).height();
        $('#crossed').css('height', (h - 80) + 'px');
    }

    function Start() {
        if (testRunning) {
            return;
        }
        $('.cross').show();
        $('#btnStart').hide();
        $('#btnStop').show();
        testRunning = true;
        eyeTestScreen.start();
        t = 60;
        itsTheFinalCountdown();
    }

    function Stop() {
        if (!testRunning) {
            return;
        }
        $('#btnStart').show();
        $('#btnStop').hide();
        testRunning = false;
        eyeTestScreen.stop();
        t = 0;
    }

	$(window).ready(function() {
        $(window).on('resize', function () { resize(); });
        resize();
	});
</script>