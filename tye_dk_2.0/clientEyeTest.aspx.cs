using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class clientEyeTest : PageBase {

	protected int ProgramEyeTestID = 0;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Client }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();

		if (!int.TryParse(VC.RqValue("ID"), out ProgramEyeTestID)) {
			redir();
		}
		
		using (var ipa = statics.GetApi()) {
			var pet = ipa.ProgramEyeTestGetSingle(ProgramEyeTestID);
			if (pet == null)
				redir();

			var program = ipa.ProgramGetSingle(pet.ProgramID);
			if (program == null || program.ClientUserID != CurrentUser.ID)
				redir();

			var eyetest = ipa.EyeTestGetSingle(pet.EyeTestID);
			if (eyetest == null)
				redir();

			var activeTest = program.ProgramEyeTests.Where(n => n.EyeTestID == eyetest.ID).FirstOrDefault();
			if (activeTest == null || activeTest.Locked || activeTest.LockedByOptician)
				redir();

			var user = CurrentUser;
			
			string[] metroTests = new string[] { 
				"vestibularTraining", 
				"vestibularTrainingWithBar", 
				"reflexTraining", 
				"balanceBoard", 
				"coordinationTraining", 
				"anglesInTheSnow", 
				"swimOnFloor", 
				"jumpRope", 
				"thumb", 
				"ninjaTraining", 
			};

			lnkToMetronome.Visible = metroTests.Contains(eyetest.InternalName);
			lnkToPrint.NavigateUrl = "eyeTestPrint.aspx?EyeTestID=" + eyetest.ID;
			
			renderEyeTestInfos(eyetest);
            renderEyeTestLinks(eyetest.ID, ipa.OpticianClientGetOptician(CurrentUser.ID).ID);
			plhStartEyeTestText.Visible = !(plhStartEyeTestScreen.Visible = (eyetest.ScreenTest));


		}
	}
    
    private void renderEyeTestLinks(int TestID, int OpticianID) {
        using(var ipa = statics.GetApi()) {

            var links = ipa.EyeTestLinkGetCollection(OpticianID, TestID);
            if(links.Any()) {
                plhLinks.Visible=true;
                foreach(var link in links) {
                    var literal = new LiteralControl();
                    literal.EnableViewState = false;
                    literal.Text = "<a href=\"" + link.LinkUrl + "\" target=\"_blank\"><i class=\"fa fa-external-link\"></i>&nbsp;" + link.LinkName + "</a><br />";
                    plhLinks.Controls.Add(literal);
                }
                plhLinks.Controls.Add(new LiteralControl() { Text = "<br /><br />" });
            }
        }
    }

	private void renderEyeTestInfos(tye.Data.EyeTest eyetest) {
		

		var name = eyetest.EyeTestInfos.Where(n => n.InfoType == "Name").FirstOrDefault();
		var intro = eyetest.EyeTestInfos.Where(n => n.InfoType == "Intro").FirstOrDefault();
		var purpose = eyetest.EyeTestInfos.Where(n => n.InfoType == "Purpose").FirstOrDefault();
		var steps = eyetest.EyeTestInfos.Where(n => n.InfoType == "Step" && n.LanguageID == CurrentLanguage.ID);
		var important = eyetest.EyeTestInfos.Where(n => n.InfoType == "Important").FirstOrDefault();


		if (name != null) {
			eyeTestName.Text = eyetest.InfoValue("Name", CurrentLanguage);
		}

		if (purpose != null) {
			litTabs.Text += "<li>" + DicValue("purpose") + "</li>";
			litTabPanels.Text += "<div class=\"tabPanel\"><h3>" + DicValue("purpose") + "</h3>" + eyetest.InfoValue("Purpose", CurrentLanguage) + "</div>";
		}
		if (intro != null) {
			litTabs.Text += "<li>" + DicValue("intro") + "</li>";
			litTabPanels.Text += "<div class=\"tabPanel\"><h3>" + DicValue("intro") + "</h3>" + eyetest.InfoValue("Intro", CurrentLanguage) + "</div>";
		}
		if (steps.Any()) {
			foreach (var step in steps.OrderBy(n => n.Priority)) {
				litTabs.Text += "<li>" + step.Priority + "</li>";
				litTabPanels.Text += "<div class=\"tabPanel\"><h3>" + step.Priority + "</h3>" + step.InfoText + "</div>";
			}
		}
		if (important != null) {
			litTabs.Text += "<li>" + DicValue("important") + "</li>";
			litTabPanels.Text += "<div class=\"tabPanel\"><h3>" + DicValue("important") + "</h3>" + eyetest.InfoValue("Important", CurrentLanguage) + "</div>";
		}
	}

	private void redir() {
		if (VC.RqHasValue("ru"))
			Response.Redirect(VC.RqValue("ru"));
		else
			Response.Redirect("clientProgram.aspx");
	}
}