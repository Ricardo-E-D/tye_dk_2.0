using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class programPrint : PageBase {

	int EditID = 0;
	bool Editing = false;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA,
			tye.Data.User.UserType.Administrator,
			tye.Data.User.UserType.Distributor,
			tye.Data.User.UserType.Optician,
			tye.Data.User.UserType.Client }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		if (!int.TryParse(VC.RqValue("EyeTestID"), out EditID) || EditID == 0)
			redir();

		Details();
	}

	private void Details() {
		using (var ipa = statics.GetApi()) {
			var eyetest = ipa.EyeTestGetSingle(EditID);
			if (eyetest == null)
				Response.Redirect("/");

			renderEyeTestInfos(eyetest);
		}
	}

	private void renderEyeTestInfos(tye.Data.EyeTest eyetest) {
		//litEyeTestName.Text = eyetest.EyeTestInfos.Where(n => n.InfoType == "Name").FirstOrDefault();

		var name = eyetest.EyeTestInfos.Where(n => n.InfoType == "Name").FirstOrDefault();
		var intro = eyetest.EyeTestInfos.Where(n => n.InfoType == "Intro").FirstOrDefault();
		var purpose = eyetest.EyeTestInfos.Where(n => n.InfoType == "Purpose").FirstOrDefault();
		var steps = eyetest.EyeTestInfos.Where(n => n.InfoType == "Step" && n.LanguageID == CurrentLanguage.ID);
		var important = eyetest.EyeTestInfos.Where(n => n.InfoType == "Important").FirstOrDefault();

		if (name != null) {
			litEyeTestName.Text = eyetest.InfoValue("Name", CurrentLanguage);
		}

		if (purpose != null) {
			plhControls.Controls.Add(new LiteralControl("<h3>" + DicValue("purpose") + "</h3>"));
			plhControls.Controls.Add(new LiteralControl(eyetest.InfoValue("Purpose", CurrentLanguage)));
		}
		if (intro != null) {
			plhControls.Controls.Add(new LiteralControl("<h3>" + DicValue("intro") + "</h3>"));
			plhControls.Controls.Add(new LiteralControl(eyetest.InfoValue("Intro", CurrentLanguage)));
		}
		if (steps.Any()) {
			foreach (var step in steps.OrderBy(n => n.Priority)) {
				plhControls.Controls.Add(new LiteralControl("<h3>" + step.Priority + "</h3>"));
				plhControls.Controls.Add(new LiteralControl(step.InfoText));
			}
		}
		if (important != null) {
			plhControls.Controls.Add(new LiteralControl("<h3>" + DicValue("important") + "</h3>"));
			plhControls.Controls.Add(new LiteralControl(eyetest.InfoValue("Important", CurrentLanguage)));
		}
	}

	private void redir() {
		if (VC.RqHasValue("ru"))
			Response.Redirect(VC.RqValue("ru"));
		else
			Response.Redirect("clients.aspx");
	}
}