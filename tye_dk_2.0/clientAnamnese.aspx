<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="clientAnamnese.aspx.cs" Inherits="clientAnamnese"
	ValidateRequest="false" %>
	<%@ Register TagPrefix="tye" TagName="anamneseMenu" Src="~/controls/anamneseMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
	<style type="text/css">
		.rotate270 { width: 60px; }
		checkbox {border: 10px solid #000000 !important; }
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">

	<h1>
		<Eav:TransLit runat="server" ID="TransLit6" Key="anamnese" />
		<asp:Literal runat="server" ID="litClientName" EnableViewState="false"></asp:Literal>
	</h1>

	<tye:anamneseMenu runat="server" ID="anamneseMenu" />

	<asp:PlaceHolder runat="server" ID="plhList" EnableViewState="false">
		<a href='?ClientUserID=<%= ClientUserID %>&ID=0<%= getRu() %>' class="positivesmall">
			<Eav:TransLit runat="server" ID="trnew" Key="createNew" />
		</a>
		<br />
		<br />
		<asp:PlaceHolder runat="server" ID="plhListList" EnableViewState="false"></asp:PlaceHolder>
	</asp:PlaceHolder>

	<asp:PlaceHolder runat="server" ID="plhEdit">

	<%--<Eav:TransLit runat="server" ID="TransLit1" Key="anam_injuries" TagName="div" CssClass="fieldTitle" />
	<asp:TextBox runat="server" ID="tbInjuries" MaxLength="200" Width="100%" />
--%>
	<Eav:TransLit runat="server" ID="TransLit8" Key="anam_medicine" TagName="div" CssClass="fieldTitle"  />
	<asp:TextBox runat="server" ID="tbMedicine" MaxLength="200" />

	<Eav:TransLit runat="server" ID="TransLit9" Key="anam_sickness" TagName="div" CssClass="fieldTitle"  />
	<asp:TextBox runat="server" ID="tbSickness" MaxLength="200" />
	
	<asp:Table runat="server" ID="tblProgram" ClientIDMode="Static" CssClass="std stripe">
		<asp:TableHeaderRow TableSection="TableHeader">
			<asp:TableHeaderCell></asp:TableHeaderCell>
			<asp:TableHeaderCell></asp:TableHeaderCell>

			<asp:TableHeaderCell CssClass="rotate270 nowrap vb">
				<Eav:TransLit runat="server" ID="t100" Key="never" /><br />0
			</asp:TableHeaderCell>
			<asp:TableHeaderCell CssClass="rotate270 nowrap vb">
				<Eav:TransLit runat="server" ID="TransLit2" Key="rarely" /><br />1
			</asp:TableHeaderCell>
			<asp:TableHeaderCell CssClass="rotate270 vb">
				<Eav:TransLit runat="server" ID="TransLit3" Key="occasionally" /><br />2
			</asp:TableHeaderCell>
			<asp:TableHeaderCell CssClass="rotate270 nowrap vb">
				<Eav:TransLit runat="server" ID="TransLit4" Key="often" /><br />3
			</asp:TableHeaderCell>
			<asp:TableHeaderCell CssClass="rotate270 nowrap vb">
				<Eav:TransLit runat="server" ID="TransLit5" Key="everyDay" /><br />4
			</asp:TableHeaderCell>
			<asp:TableHeaderCell CssClass="rotate270 nowrap vb">
				<Eav:TransLit runat="server" ID="TransLit7" Key="dontKnow" /><br />&nbsp;
			</asp:TableHeaderCell>
		</asp:TableHeaderRow>
	</asp:Table>
	
	<br />

	<div class="fieldTitle clear" style="float:right;">SUM</div>
	<div id="summation" class="clear" style="float:right;">0</div>

	<div class="clear"></div>
	<Eav:TransLit runat="server" ID="TransLit10" Key="anam_readingHours" TagName="div" CssClass="fieldTitle" />
	<asj:NumericTextBox runat="server" ID="ntbReadingHours" MinValue="0" MaxValue="24" NumberType="Integer"></asj:NumericTextBox>

	<Eav:TransLit runat="server" ID="TransLit13" Key="anam_hoursNear" TagName="div" CssClass="fieldTitle" />
	<asj:NumericTextBox runat="server" ID="ntbHoursNear" MinValue="0" MaxValue="24" NumberType="Integer"></asj:NumericTextBox>
	
	<%--<Eav:TransLit runat="server" ID="TransLit14" Key="anam_others" TagName="div" CssClass="fieldTitle" />
	<asp:TextBox runat="server" ID="tbComment" TextMode="MultiLine"></asp:TextBox>
--%>

	<div class="clear stickToBottom">
		<asp:LinkButton runat="server" ID="lnkSave" CssClass="btn positive" OnClick="eLnkSave_Click">
			<Eav:TransLit runat="server" ID="TransLit61" Key="save" />
		</asp:LinkButton>
		<asp:LinkButton runat="server" ID="lnkCancel" CssClass="btn negative" OnClick="eLnkCancel_Click">
			<Eav:TransLit runat="server" ID="TransLit12" Key="cancel" />
		</asp:LinkButton>
	</div>

	</asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/js/stickToBottom.js');
	var summation = function() {
		var tbl = $('#tblProgram'), t = $(this), sum = 0, isChecked = (t.attr('checked') == 'checked');
		
		t.parents('tr').find('input[type=checkbox]').removeAttr('checked');
		if(isChecked) {
			t.attr('checked', 'checked');
		}

		tbl.find('tr').each(function() {
			var row = $(this), boxes = row.find('input[type=checkbox]');
			if(boxes.length == 0)
				return;
				
			for(var i = 0, len = boxes.length; i < len; i++) {
				var e = $(boxes[i]);
				if(e.is(':checked')) {
					var idx = boxes.index(e);
					if(idx >= 0 && idx <= 4) {
						sum += idx;
					}
				}
			}

			return;
				
		}); // tbl.find
		$('#summation').html(sum);
	};

	onload_methods.push(function() { 
		summation.call(); 
		$('#tblProgram').on('click', 'input[type=checkbox]', function() { summation.call(this); });
	});
</asp:Content>

