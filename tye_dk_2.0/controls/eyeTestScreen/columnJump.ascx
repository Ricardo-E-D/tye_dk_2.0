<%@ Control Language="C#" AutoEventWireup="true" CodeFile="columnJump.ascx.cs" Inherits="controls_eyeTestScreen_columnJump" %>
<input type="hidden" runat="server" id="level" />


<div style="text-align:center;" class="eyeTestScreenTopMenu">
	<a href="#" id="cols2" onclick="ResetCols(); SetCols(2, 'cols')">2</a>
	 <a href="#" id="cols3" onclick="ResetCols(); SetCols(3, 'cols')">3</a> 
	 <a href="#" id="cols4" onclick="ResetCols(); SetCols(4, 'cols')">4</a> 
	 <a href="#" id="colsrandom" onclick="ResetCols(); SetCols(2, 'random')">Random</a>
</div>

<br />
<br />

<table style="WIDTH: 100%; HEIGHT: 90%">

<tr>
		<td colspan="2">
			<div style="text-align:center;">
				<div style="display:inline-block;" class="eyeTestTime">&nbsp;</div>
				<div style="display:inline-block;margin-left: 20px;text-align:right;">
					<Eav:TransLit runat="server" ID="transScore" Key="score"></Eav:TransLit>: 
				</div>
				<div style="display:inline-block;" class="score">0</div>
			</div>
			<br /><br />
		</td>
	</tr>
				<tr>
					<td align="center" valign="middle">
						<table border="0" cellpadding="3">
							<tr>
								<td style="position:relative;">
									<img src="/img/eyetest/columnJump/spacer.png" id="row1" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row2" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row3" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row4" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row5" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row6" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row7" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row8" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row9" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row10" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row11" alt="" class="arrow" />
									<img src="/img/eyetest/columnJump/spacer.png" id="row12" alt="" class="arrow" />

								</td>
								<td style="WIDTH: 6px">&nbsp;</td>
								<td align="center"><img src="/img/x.png" id="col1"></td>
								<td align="center"><img src="/img/x.png" id="col2"></td>
								<td align="center"><img src="/img/x.png" id="col3"></td>
								<td align="center"><img src="/img/x.png" id="col4"></td>
								<td align="center"><img src="/img/x.png" id="col5"></td>
								<td align="center"><img src="/img/x.png" id="col6"></td>
								<td align="center"><img src="/img/x.png" id="col7"></td>
								<td align="center"><img src="/img/x.png" id="col8"></td>
								<td align="center"><img src="/img/x.png" id="col9"></td>
								<td align="center"><img src="/img/x.png" id="col10"></td>
								<td align="center"><img src="/img/x.png" id="col11"></td>
								<td align="center"><img src="/img/x.png" id="col12"></td>
								<td style="WIDTH: 6px">&nbsp;</td>
							</tr>
							<tr style="height:2px;">
								<td></td>
								<td colspan="14" rowspan="15"><img src="/img/eyetest/columnJump/kolonnehop.png"></td>
							</tr>
							<tr>
								<td style="HEIGHT: 6px">
								</td>
							</tr>
							<%--<tr style="height:33px;">
								<td>
									
								</td>
							</tr>--%>
			<%--				<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row2" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row3" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row4" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row5" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row6" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row7" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row8" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row9" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row10" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row11" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td><img src="/img/eyetest/columnJump/spacer.png" id="row12" alt=""></td>
							</tr>
							<tr style="height:33px;">
								<td style="HEIGHT: 6px"></td>
							</tr>--%>
						</table>
						<br />
						<input type="button" class="positivesmall link" value="Start" onclick="MarkCols()" id="startBtn" />
					</td>
				</tr>
			</table>
<br /><br />
<script type="text/javascript">
	$(function () {
		GeneratePossibleCols();
		ResetCols();
		SetCols(2, 'cols');
		$('div.eyeTestScreenTopMenu a').addClass('positivesmall');
		$('div.eyeTestScreenTopMenu a:first-child').removeClass('positivesmall').addClass('negativesmall');
	});
</script>
<style type="text/css">
	.arrow {
		position:absolute;
		left: 0;
		top: 50px;
	}
</style>