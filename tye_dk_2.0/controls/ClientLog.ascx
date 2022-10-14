<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientLog.ascx.cs" Inherits="controls_ClientLog" %>

	<h1>
		<asp:Literal runat="server" ID="litClientName" />
	</h1>

	<Eav:TransLit runat="server" ID="lit" Key="dateFilter" />: 
	<asj:DateTimePicker runat="server" ID="dtpStart" Mode="Date" Width="120" />
	<asj:DateTimePicker runat="server" ID="dtpEnd" Mode="Date" Width="120" />
	
	<Eav:TransLit runat="server" ID="TransLit3" Key="groupBy" />: 
	<asp:DropDownList runat="server" ID="ddlGroupBy" Width="150">
	
	</asp:DropDownList>
	<asp:LinkButton runat="server" ID="lnkShow" CssClass="positivesmall" OnClick="eLnkShow_Click">
		<Eav:TransLit runat="server" ID="TransLit2" Key="show" />
	</asp:LinkButton>

	<asp:Panel runat="server" ID="pnlClientLogAddComment" CssClass="well dialog" ClientIDMode="Static">
		<Eav:TransLit runat="server" ID="trspnlClientLogAddComment" Key="addComment"></Eav:TransLit>
		<asp:TextBox runat="server" ID="tbClientLogAddComment" ClientIDMode="Static" TextMode="MultiLine" Rows="5" />
		<asp:HiddenField runat="server" ID="hidClientLogAddCommentEntryID" ClientIDMode="Static" />
		<br /><br />
		<div class="buttons">
			<asp:LinkButton runat="server" ID="lnkClientLogAddCommentSave" OnClick="ElnkClientLogAddCommentSave_Click" CssClass="positive">
				<Eav:TransLit runat="server" ID="TransLit4" Key="save" />
			</asp:LinkButton>
			<a class="negative link" onclick="$('#pnlClientLogAddComment').fadeOut();tye.hideOverlay();">
				<Eav:TransLit runat="server" ID="TransLit5" Key="cancel" />
			</a>
		</div>
	</asp:Panel>

	<br /><br />

	<asp:PlaceHolder runat="server" ID="plhControls" EnableViewState="false">
		<a class="link" onclick="$('div.logHeading').each(function() { $(this).nextUntil('div.logHeading').fadeOut(); })">
			<Eav:TransLit runat="server" ID="TransLit1" Key="collapseAll" />
		</a>
		&nbsp;&nbsp;&nbsp;
		<a class="link" onclick="$('div.logHeading').each(function() { $(this).nextUntil('div.logHeading').fadeIn(); })">
			<Eav:TransLit runat="server" ID="transExpand" Key="expandAll" />
		</a>
	</asp:PlaceHolder>
	<asp:PlaceHolder runat="server" ID="plhLog" EnableViewState="false" />

	<script type="text/javascript">
		$('div.logHeading').on('click', function () {
			$(this).nextUntil('div.logHeading').fadeToggle();
		});
		$('img.clientLogEditComment').on('click', function () {
			var t = $(this);
			tye.showOverlay();
			$('#pnlClientLogAddComment').fadeIn().center();
			$('#hidClientLogAddCommentEntryID').val(t.attr('entryid'));
			$('#tbClientLogAddComment').val(t.parent().text());
		});
	</script>
