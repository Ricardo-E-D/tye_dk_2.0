<%@ Control Language="C#" AutoEventWireup="true" CodeFile="labyrinth.ascx.cs" Inherits="controls_eyeTestScreen_labyrinth" %>
<input type="hidden" runat="server" id="level" />


<div style="text-align:center;" class="eyeTestScreenTopMenu">
	<a class="negativesmall link" onclick="setLevel(this, 1);">1</a>
	<a class="positivesmall link" onclick="setLevel(this, 2);">2</a>
	<a class="positivesmall link" onclick="setLevel(this, 3);">3</a>
	<a class="positivesmall link" onclick="setLevel(this, 4);">4</a>
	<a class="positivesmall link" onclick="setLevel(this, 5);">5</a>
	<a class="positivesmall link" onclick="setLevel(this, 6);">6</a>
	<a class="positivesmall link" onclick="setLevel(this, 7);">7</a>
	<a class="positivesmall link" onclick="setLevel(this, 8);">8</a>
	<a class="positivesmall link" onclick="setLevel(this, 9);">9</a>
	<a class="positivesmall link" onclick="setLevel(this, 10);">10</a>
	<a class="positivesmall link" onclick="setLevel(this, 11);">11</a>
	<a class="positivesmall link" onclick="setLevel(this, 12);">12</a>
	<a class="positivesmall link" onclick="setLevel(this, 13);">13</a>
	<a class="positivesmall link" onclick="setLevel(this, 14);">14</a>
	<a class="positivesmall link" onclick="setLevel(this, 15);">15</a>
	<a class="positivesmall link" onclick="setLevel(this, 16);">16</a>
	<a class="positivesmall link" onclick="setLevel(this, 17);">17</a>
	<a class="positivesmall link" onclick="setLevel(this, 18);">18</a>
	<a class="positivesmall link" onclick="setLevel(this, 19);">19</a>
	<a class="positivesmall link" onclick="setLevel(this, 20);">20</a>
</div>

<br />
<div style="text-align:center;">
	<input type="button" value="Start" class="link positivesmall" id="btnStart" onclick="Start();" />
</div>

<div style="text-align:center;">
	<div class="eyeTestTime">&nbsp;</div>
	<img src="/img/eyetest/labyrinth/1.png" alt="" id="imgLabyrinth" />
</div>

<br /><br />

<script type="text/javascript">
	include('/js/jquery.doTimeout.js');
	$(function () {
		updateHidLevel();
		resizeLabyrinth();
		$(window).on('resize', function () {
			$.doTimeout('labyrinth', 500, function () {
				resizeLabyrinth();
			});
		});
	});
</script>