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
			tye.Data.User.UserType.Optician,
			tye.Data.User.UserType.Client }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		if (!int.TryParse(VC.RqValue("ClientUserID"), out EditID) || EditID == 0)
			redir();

		using (var ipa = statics.GetApi()) {
			var user = ipa.UserGetSingle(EditID);
			if (user == null)
				redir();

            var optician = ipa.OpticianClientGetOptician(EditID);
			if (CurrentUser.Type == tye.Data.User.UserType.Client && EditID != CurrentUser.ID)
				redir();
			if (CurrentUser.Type == tye.Data.User.UserType.Optician && optician.ID != CurrentUser.ID)
				redir();

			lnkBackToClient.NavigateUrl = "clients.aspx?id=" + EditID;
			lnkBackToClient.Text = user.FullName;

			AddJavascript("addCheckEvent()");

			var program = ipa.ProgramGetSingleByUserID(EditID);
			if (program == null)
				return;

			// disabled 2014-05-21
			//if(!String.IsNullOrEmpty(program.Comment))
			//   litProgramComments.Text = Server.HtmlDecode(program.Comment);

			var EyeTests = ipa.EyeTestGetCollection();
            EyeTests.AddRange(ipa.EyeTestGetCollection(optician.ID));

            foreach (var activeTest in program.ProgramEyeTests.Where(m => m.Active).OrderBy(m => m.Priority))
            {
                
                var EyeTest = EyeTests.FirstOrDefault(m => m.ID == activeTest.EyeTestID);
                if(EyeTest == null) {
                    continue;
                }
                TableRow row = new TableRow();
                TableCell tdName = new TableCell();
                TableCell tdHighScore = new TableCell();

                tdName.Text = EyeTest.InfoValue("Name", CurrentLanguage);
                tdHighScore.Text = "Highscore: ";

                bool EyeTestLockedByDefault = EyeTest.ScreenTest && EyeTest.ScoreRequired > 0;
                if (activeTest != null)
                    EyeTestLockedByDefault = activeTest.Locked;

                row.Cells.Add(tdName);

                var logs = ipa.ClientEyeTestLogGetCollection(EditID, EyeTest.ID);
                if (logs.Any())
                {
                    var high = logs.Where(n => n.HighScore).OrderByDescending(n => n.Score).FirstOrDefault();
                    if (high != null)
                    {
                        tdHighScore.Text += high.Score.ToString();
                    }
                }
                row.Cells.Add(tdHighScore);
                row.Cells.Add(new TableCell() { Text = "<img src=\"/img/" + (EyeTest.ScreenTest ? "screen" : "text") + ".png\" alt=\"\" />" });

                tblProgram.Rows.Add(row);
            }

            //foreach (var EyeTest in EyeTests) {
            //    var activeTest = program.ProgramEyeTests.Where(n => n.EyeTestID == EyeTest.ID).FirstOrDefault();

            //    if (activeTest == null)
            //        continue;

            //    if (!activeTest.Active) {
            //        return;
            //    }

            //    TableRow row = new TableRow();
            //    TableCell tdName = new TableCell();
            //    TableCell tdHighScore = new TableCell();

            //    tdName.Text = EyeTest.InfoValue("Name", CurrentLanguage);
            //    tdHighScore.Text = "Highscore: ";

            //    bool EyeTestLockedByDefault = EyeTest.ScreenTest && EyeTest.ScoreRequired > 0;
            //    if (activeTest != null)
            //        EyeTestLockedByDefault = activeTest.Locked;

            //    row.Cells.Add(tdName);

            //    var logs = ipa.ClientEyeTestLogGetCollection(EditID, EyeTest.ID);
            //    if (logs.Any()) {
            //        var high = logs.Where(n => n.HighScore).OrderByDescending(n => n.Score).FirstOrDefault();
            //        if (high != null) {
            //            tdHighScore.Text += high.Score.ToString();
            //        }
            //    }
            //    row.Cells.Add(tdHighScore);
            //    row.Cells.Add(new TableCell() { Text = "<img src=\"/img/" + (EyeTest.ScreenTest ? "screen" : "text") + ".png\" alt=\"\" />" });
				

            //    tblProgram.Rows.Add(row);
            //}
		}
		
	}


private void redir() {
		if (VC.RqHasValue("ru"))
			Response.Redirect(VC.RqValue("ru"));
		else
			Response.Redirect("clients.aspx");
	}
}