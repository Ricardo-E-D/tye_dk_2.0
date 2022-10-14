<%@ Page Title="Clients" Language="C#" MasterPageFile="~/master.master" AutoEventWireup="true" CodeFile="program.aspx.cs" Inherits="program" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHhead" Runat="Server">
	<link rel="Stylesheet" href="/css/monoTabs.js.css" />
	<link rel="Stylesheet" href="/css/jquery.tablesorter.blue.css" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <style type="text/css">
        .sort-handle
        {
            margin-left: 20px;
            display:none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHcontent" Runat="Server">
	<h1>
		<Eav:TransLit runat="server" ID="TransLit6" Key="clients" />
		<asp:HyperLink runat="server" ID="lnkBackToClient" NavigateUrl="?id=0" CssClass="plain" />
	</h1>
	
	<%--program comments--%>
	<div class="note well"><asp:Literal runat="server" ID="litProgramComments" EnableViewState="false" /></div>
	<div class="note link" onclick="tye.showOverlay();$('#pnlEditProgramComment').css('z-index', '200').fadeIn();">
		<a href="#"><Eav:TransLit runat="server" ID="TransLit14" Key="editProgramComments"></Eav:TransLit></a>
		<br /><br />
	</div>

	<%--program comments editor--%>
	<asp:Panel runat="server" ID="pnlEditProgramComment" CssClass="well dialog" ClientIDMode="Static">
		<Eav:TransLit runat="server" ID="trspnlEditProgramComment" Key="addComment"></Eav:TransLit>
		<asp:TextBox runat="server" ID="tbEditProgramComment" ClientIDMode="Static" TextMode="MultiLine" Rows="5" />
		
		<br /><br />
		<div class="buttons">
			<asp:LinkButton runat="server" ID="lnkEditProgramCommentSave" OnClick="ElnkEditProgramCommentSave_Click" CssClass="positive">
				<Eav:TransLit runat="server" ID="TransLit12" Key="save" />
			</asp:LinkButton>
			<a class="negative link" onclick="$('#pnlEditProgramComment').fadeOut();tye.hideOverlay();">
				<Eav:TransLit runat="server" ID="TransLit13" Key="cancel" />
			</a>
		</div>
	</asp:Panel>
    <div id="toolbarDefault">
	    <img class="tooltip vm" src="/img/question.png" alt="Help" />
	    <div class="hidden">
		    <Eav:TransLit runat="server" ID="transHoverCheck" Key="tooltip_hoverCheck" /> 
	    </div>
	    <img class="tooltip vm" src="/img/warning.png" alt="Warning" />
	    <div class="hidden">
		    <Eav:TransLit runat="server" ID="TransLit3" Key="tooltip_warnRemoveEyeTest" /> 
	    </div>

        <span class="spacer-inline"></span>

	    <asp:HyperLink runat="server" ID="lnkToPrint" Target="_blank">
		    <img src="/img/print.png" alt="Print" class="vm" /> <Eav:TransLit runat="server" ID="TransLit9" Key="print" /> 
	    </asp:HyperLink>
    
        <span class="spacer-inline"></span>

        <a id="lnkSortEnable" class="link"><i class="fa fa-sort"></i> Sort tests</a>
    </div>
    <div id="toolbarSorting" class="hidden">
         
        <a id="lnkSortSave" class="positive link">Save custom sort order</a>
        <a id="lnkSortCancel"class="negative link">Cancel sorting</a>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink runat="server" ID="lnkResetSortOrder" CssClass="negativesmall">
		    Reset to default sort order
	    </asp:HyperLink>

        <div><br /></div>
    </div>

	<div class="clear"></div>
	
	<asp:Panel runat="server" ID="pnlConfirmEyeTestRemoval" CssClass="errorInline" Visible="false">
		<Eav:TransLit runat="server" ID="TransLit7" Key="tooltip_warnRemoveEyeTest" />
		<br />
		<Eav:TransLit runat="server" ID="TransLit8" Key="areYouSureDelete" />
		<br />
		<asp:LinkButton runat="server" ID="lnkSaveWithConfirmation" CssClass="btn positive" OnClick="eLnkSaveAndClose_Click">
			<Eav:TransLit runat="server" ID="TransLit4" Key="saveAndClose" />
		</asp:LinkButton>
	</asp:Panel>

	<asp:Table runat="server" ID="tblProgram" ClientIDMode="Static" CssClass="std stripe">
		<asp:TableHeaderRow TableSection="TableHeader" CssClass="sort-hide">
			<asp:TableHeaderCell></asp:TableHeaderCell>
			<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit2" Key="active" /></asp:TableHeaderCell>
			<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit10" Key="opticianLock" /></asp:TableHeaderCell>
			<asp:TableHeaderCell><Eav:TransLit runat="server" ID="transLock" Key="programLock" /></asp:TableHeaderCell>
			<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit1" Key="name" /></asp:TableHeaderCell>
			<asp:TableHeaderCell><Eav:TransLit runat="server" ID="TransLit11" Key="highscore" /></asp:TableHeaderCell>
		</asp:TableHeaderRow>
	</asp:Table>
	
	<asp:PlaceHolder runat="server" ID="plhControls">
	</asp:PlaceHolder>
	
	<div class="buttons clear stickToBottom">
		<asp:LinkButton runat="server" ID="lnkSave" CssClass="btn positive" OnClick="eLnkSave_Click">
			<Eav:TransLit runat="server" ID="TransLit61" Key="save" />
		</asp:LinkButton>
		<asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn positive" OnClick="eLnkSaveAndClose_Click">
			<Eav:TransLit runat="server" ID="transSave" Key="saveAndClose" />
		</asp:LinkButton>
			
		<asp:HyperLink runat="server" ID="lnkCancel" NavigateUrl="/clients.aspx"  CssClass="btn negative">
			<Eav:TransLit runat="server" ID="TransLit5" Key="cancel" />
		</asp:HyperLink>
	</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHscript" Runat="Server">
	include('/js/stickToBottom.js');
    include('/js/programEyeTestSorting.js');
    
	onload_methods.push(function() {
		$('img.tooltip').each(function() {
         $(this).qtip({
             content: {
                 text: $(this).next('.hidden')
             }
         });
     });

	  var dim = function() {
	   var t = $(this);
		 if(t.is(':checked')) {
			t.parents('tr').removeClass('dimmed');
		 } else {
			t.parents('tr').addClass('dimmed');
		 }
	  }

	  $('#tblProgram').on('click', 'span.eyeTestActive input', function() {
		dim.call($(this));
	  });
	  $('#tblProgram span.eyeTestActive input').each(function() {
		dim.call($(this));
	  });
	});
</asp:Content>

