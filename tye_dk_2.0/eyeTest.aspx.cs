using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class eyeTest : PageBase {

	public int EditID = 0;
	bool Editing = false;
	PropertyMapper PM = null;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA,
			tye.Data.User.UserType.Administrator,
				tye.Data.User.UserType.Distributor,
			tye.Data.User.UserType.Optician
		}
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();

		if (!int.TryParse(VC.RqValue("ID"), out EditID) || EditID < 1) {
			List();
		} else {
			Details();
			lnkToPrint.NavigateUrl = "eyeTestPrint.aspx?EyeTestID=" + EditID;
			plhDetails.Visible = true;
			tblProgram.Visible = false;
		}
	}
	
	private void Details() {
		using (var ipa = statics.GetApi()) {
			var eyetest = ipa.EyeTestGetSingle(EditID);
			if (eyetest == null)
				Response.Redirect("/");

            if (eyetest.OpticianID.HasValue && eyetest.OpticianID.Value != CurrentUser.ID) {
                Response.Redirect("eyeTest.aspx");
            }

			renderEyeTestInfos(eyetest);
            renderEyeTestLinks(eyetest.ID, CurrentUser.ID);

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

			plhStartEyeTestScreen.Visible = (eyetest.ScreenTest);

		}


	}

    private void renderEyeTestLinks(int TestID, int OpticianID)
    {
        using (var ipa = statics.GetApi())
        {

            var links = ipa.EyeTestLinkGetCollection(OpticianID, TestID);
            if (links.Any())
            {
                plhLinks.Visible = true;
                foreach (var link in links)
                {
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

		if (purpose != null && !String.IsNullOrEmpty(eyetest.InfoValue("Purpose", CurrentLanguage))) {
			litTabs.Text += "<li>" + DicValue("purpose") + "</li>";
			litTabPanels.Text += "<div class=\"tabPanel\"><h3>" + DicValue("purpose") + "</h3>" + eyetest.InfoValue("Purpose", CurrentLanguage) + "</div>";
		}
		if (intro != null && !String.IsNullOrEmpty(eyetest.InfoValue("Intro", CurrentLanguage))) {
			litTabs.Text += "<li>" + DicValue("intro") + "</li>";
			litTabPanels.Text += "<div class=\"tabPanel\"><h3>" + DicValue("intro") + "</h3>" + eyetest.InfoValue("Intro", CurrentLanguage) + "</div>";
		}
		if (steps.Any()) {
			foreach (var step in steps.OrderBy(n => n.Priority)) {
				litTabs.Text += "<li>" + step.Priority + "</li>";
				litTabPanels.Text += "<div class=\"tabPanel\"><h3>" + step.Priority + "</h3>" + step.InfoText + "</div>";
			}
		}
		if (important != null && !String.IsNullOrEmpty(eyetest.InfoValue("Important", CurrentLanguage))) {
			litTabs.Text += "<li>" + DicValue("important") + "</li>";
			litTabPanels.Text += "<div class=\"tabPanel\"><h3>" + DicValue("important") + "</h3>" + eyetest.InfoValue("Important", CurrentLanguage) + "</div>";
		}
	}

	private void List() {
        plhList.Visible = true;

		using (var ipa = statics.GetApi()) {
			var EyeTests = ipa.EyeTestGetCollection();
			foreach (var EyeTest in EyeTests) {
				TableRow row = new TableRow();

				TableCell tdName = new TableCell();
                TableCell tdLinks = new TableCell();

				tdName.Text = EyeTest.InfoValue("Name", CurrentLanguage);

				tdName.Text = "<a href=\"eyeTest.aspx?ID=" + EyeTest.ID + "\">" + tdName.Text + "</a>";

                tdLinks.Text = "<a href=\"eyeTestLink.aspx?EyeTestID=" + EyeTest.ID + "\"><i class=\"fa fa-pencil\"></i>&nbsp;Add links" + "" + "</a>";

				row.Cells.Add(new TableCell() { Text = "<img src=\"/img/" + (EyeTest.ScreenTest ? "screen" : "text") + ".png\" alt=\"\" />" });
				row.Cells.Add(tdName);
                row.Cells.Add(tdLinks);

				tblProgram.Rows.Add(row);
			}

            if (CurrentUser.Type == tye.Data.User.UserType.Optician) {
                plhOwnTests.Visible = true;
                
                var customTests = ipa.EyeTestGetCollection(CurrentUser.ID);
                foreach (var EyeTest in customTests)
                {
                    TableRow row = new TableRow();

                    TableCell tdName = new TableCell();
                    TableCell tdEdit = new TableCell();
                    TableCell tdLinks = new TableCell();

                    tdName.Text = EyeTest.InfoValue("Name", CurrentLanguage);

                    tdName.Text = "<a href=\"eyeTest.aspx?ID=" + EyeTest.ID + "\">" + tdName.Text + "</a>";

                    tdEdit.Text = "<a href=\"eyeTestOptician.aspx?EyeTestID=" + EyeTest.ID + "\"><i class=\"fa fa-pencil\"></i>&nbsp;Edit" + "" + "</a>";
                    tdLinks.Text = "<a href=\"eyeTestLink.aspx?EyeTestID=" + EyeTest.ID + "\"><i class=\"fa fa-pencil\"></i>&nbsp;Add links" + "" + "</a>";

                    row.Cells.Add(new TableCell() { Text = "<img src=\"/img/" + (EyeTest.ScreenTest ? "screen" : "text") + ".png\" alt=\"\" />" });
                    row.Cells.Add(tdName);
                    row.Cells.Add(tdEdit);
                    row.Cells.Add(tdLinks);

                    tblOwnTests .Rows.Add(row);
                }
                if (!customTests.Any()) {
                    tblOwnTests.Visible = false;
                }
            }

		}
	}

}