using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class clientProgram : PageBase {

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Client }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();

		using (var ipa = statics.GetApi()) {
			var user = CurrentUser;

			var program = ipa.ProgramGetSingleByUserID(user.ID);
			if (program == null || !program.ProgramEyeTests.Any())
				return;

			lnkToPrint.NavigateUrl = "programPrint.aspx?ClientUserID=" + user.ID;

            var optician = ipa.OpticianClientGetOptician(user.ID);
			var EyeTests = ipa.EyeTestGetCollection();
            EyeTests.AddRange(ipa.EyeTestGetCollection(optician.ID)); // custom tests
            
            // for sorting
            var programeyetestids = program.ProgramEyeTests.Where(m => m.Active).OrderBy(m => m.Priority).Select(m => m.EyeTestID).ToList();

			// eval locks for 3D EyeTests
			foreach (var pet in program.ProgramEyeTests.Where(n => n.Locked)) {
				var eyeTest = EyeTests.FirstOrDefault(n => n.ID == pet.EyeTestID);
				if (eyeTest == null)
					continue;

				// eye test found - now get the eyetest that preceeds it, which requires a score
				var preceeding = EyeTests.Where(
						n => n.Priority < eyeTest.Priority
							&& n.ScreenTest
                            && programeyetestids.Contains(n.ID) // test for eval must also be included in client program
							/*&& n.ScoreRequired > 0*/).OrderByDescending(n => n.Priority).FirstOrDefault();
				if (preceeding == null)
					continue;

				// preceeding test found. Now eval score required match
				var logs = ipa.ClientEyeTestLogGetCollection(user.ID, preceeding.ID);
				if (!logs.Any())
					continue;

				// if score required is met...unlock this test
				if (logs.Where(n => n.Score >= preceeding.ScoreRequired).Any()) {
					pet.Locked = false; // important for the displayed list to be correct
					ipa.ProgramEyeTestSave(program.ID, pet.EyeTestID, false, eyeTest.Priority);
				}
			}

			evalHighscores(user.ID);

            foreach (var EyeTest in EyeTests.OrderBy(m => programeyetestids.IndexOf(m.ID)))
            {
				var activeTest = program.ProgramEyeTests.Where(n => n.Active && n.EyeTestID == EyeTest.ID).FirstOrDefault();

				if (activeTest == null) // skip tests not relevant for user
					continue;

				TableRow row = new TableRow();

				TableCell tdName = new TableCell();
				TableCell tdHighscore = new TableCell();

				tdName.Text = EyeTest.InfoValue("Name", CurrentLanguage);

				bool EyeTestLockedByDefault = EyeTest.ScreenTest && EyeTest.ScoreRequired > 0;
				//if (activeTest != null)
				EyeTestLockedByDefault = (activeTest == null ? true : activeTest.Locked);

				if (EyeTestLockedByDefault) {
					tdName.Text = "<img src=\"/img/lock.png\" alt=\"\" /> " + tdName.Text;
				} else {
					if (activeTest != null && !activeTest.LockedByOptician) // activated for current program
						tdName.Text = "<a href=\"clientEyeTest.aspx?ID=" + activeTest.ID + "\">" + tdName.Text + "</a>";
				}

				var highscore = ipa.ClientEyeTestLogGetHighScore(user.ID, EyeTest.ID);
				if (highscore != null)
					tdHighscore.Text = highscore.Score.ToString();

				row.Cells.Add(new TableCell() { Text = "<img src=\"/img/" + (EyeTest.ScreenTest ? "screen" : "text") + ".png\" alt=\"\" />" });
				row.Cells.Add(tdName);
				row.Cells.Add(tdHighscore);

				tblProgram.Rows.Add(row);
			}
		}
	}

	private void evalHighscores(int UserID) {
		Calculations calcs = new Calculations();
		calcs.EvalHighscores(UserID);
	}

	private void redir() {
		if (VC.RqHasValue("ru"))
			Response.Redirect(VC.RqValue("ru"));
		else
			Response.Redirect("clients.aspx");
	}
}