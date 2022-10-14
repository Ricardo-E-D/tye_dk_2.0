using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class controls_ClientLog : UserControlBase {

	int clientID = 0;
	string groupby = "eyetest"; // eyetest

	protected void Page_Init(object sender, EventArgs e) {

		bool blnIsOptician = CurrentUser.Type == tye.Data.User.UserType.Optician;
		bool blnIsClient = CurrentUser.Type == tye.Data.User.UserType.Client;
		bool blnIsAdmin = (!blnIsOptician && !blnIsClient);

		clientID = CurrentUser.ID; // blnIsClient

		if (blnIsOptician || blnIsAdmin) {
			int.TryParse(VC.RqValue("ClientUserID"), out clientID);
		}
		if (clientID == 0) {
			closeWindow();
			return;
		}

		switch (CurrentLanguage.ID) {
			case 1: // Danish
				dtpStart.Culture = dtpEnd.Culture = new System.Globalization.CultureInfo("da-DK");
				break;
			case 2: // Norsk
				dtpStart.Culture = dtpEnd.Culture = new System.Globalization.CultureInfo("nb-NO");
				break;
			case 3: // English
				dtpStart.Culture = dtpEnd.Culture = new System.Globalization.CultureInfo("en-GB");
				break;
			case 4: // German
				dtpStart.Culture = dtpEnd.Culture = new System.Globalization.CultureInfo("de-DE");
				break;
		}

		dtpStart.LabelToday = dtpEnd.LabelToday = DicValue("todayIs");


		Calculations calcs = new Calculations();
		calcs.EvalHighscores(clientID);

		tye.Data.User client = null;
		using (var ipa = statics.GetApi()) { 
			client = ipa.UserGetSingle(clientID);
			if (client == null) { // get lost if client id is bogus
				closeWindow();
				return;
			}
			if (blnIsOptician) {
				var optician = ipa.OpticianClientGetOptician(clientID);
				// also get lost if client doesn't belong to optician
				if (optician == null || optician.ID != CurrentUser.ID) {
					closeWindow();
					return;
				}
			}
		}

		if (client != null) {
			litClientName.Text = DicValue("clientLog") + " - ";
			if (CurrentUser.Type == tye.Data.User.UserType.Optician) {
				litClientName.Text += "<a href=\"clients.aspx?id=" + client.ID + "\">" + client.FullName + "</a>";
			} else {
				litClientName.Text += client.FullName;
			}
		}

		// everything's good. Show log.
		if (!IsPostBack) {
			dtpStart.SelectedDate = DateTime.Now.AddDays(-30);
			dtpEnd.SelectedDate = DateTime.Now;
		}

		ddlGroupBy.Items.Clear();
		ddlGroupBy.Items.Add(new ListItem() { Text = DicValue("eyeTest"), Value = "eyetest" });
		ddlGroupBy.Items.Add(new ListItem() { Text = DicValue("date"), Value = "date" });

		showLog();
		plhControls.Visible = (plhLog.Controls.Count > 1);
	}

	protected void eLnkShow_Click(object sender, EventArgs e) {
		groupby = ddlGroupBy.SelectedValue;
		showLog();
	}

	protected void ElnkClientLogAddCommentSave_Click(object sender, EventArgs e) {
		int iTry = 0;
		if (!int.TryParse(hidClientLogAddCommentEntryID.Value, out iTry))
			Response.Redirect(VC.QueryStringStripNoTrail(""));

		using (var ipa = statics.GetApi()) {
			var logentry = ipa.ClientEyeTestLogGetSingle(iTry);
			if(logentry == null)
				Response.Redirect(VC.QueryStringStripNoTrail(""));

			var program = ipa.ProgramGetSingle(logentry.ProgramEyeTest.ProgramID);
			if(program == null)
				Response.Redirect(VC.QueryStringStripNoTrail(""));

			bool okToComment = false;
			if (CurrentUser.Type == tye.Data.User.UserType.Client) {
				if (program.ClientUserID == CurrentUser.ID)
					okToComment = true;
			}
			if (CurrentUser.Type == tye.Data.User.UserType.Optician) {
				var clients = ipa.UserGetCollectionByOptician(CurrentUser.ID);
				okToComment = (clients.Any(n => n.ID == program.ClientUserID));
			}
			if (okToComment) {
				logentry.Comment = tbClientLogAddComment.Text;
				ipa.ClientEyeTestLogSave(logentry);
			}
		}
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	private void showLog() {
		plhLog.Controls.Clear();
		using (var ipa = statics.GetApi()) { 
			DateTime dtStart = (dtpStart.SelectedDate.HasValue ? (DateTime)dtpStart.SelectedDate : DateTime.Now.AddDays(-14));
			dtStart = dtStart.Subtract(dtStart.TimeOfDay);

			DateTime dtEnd = dtpEnd.SelectedDateOrNow;
			dtEnd = dtEnd.Subtract(dtEnd.TimeOfDay).AddDays(1);

			var logs = ipa.ClientEyeTestLogGetCollection(clientID, dtStart, dtEnd);

			Dictionary<int, tye.Data.EyeTest> tests = new Dictionary<int, tye.Data.EyeTest>();

			DateTime dtTemp = DateTime.MaxValue;
			string eyeTestTemp = "";
			string dtFormat = (groupby == "date" ? "HH:mm:ss" : "g");

			if (groupby == "date") { 
				foreach (var log in logs) {
					if (log.StartTime.Date < dtTemp.Date) {
						dtTemp = log.StartTime.Date;
						output("<div class=\"logHeading\">" + dtTemp.ToString("yyyy-MM-dd") + "</div>");
						output("<div class=\"logTh\">");
						
						output("<div>" + DicValue("eyeTest") + "</div>");
						output("<div>" + DicValue("time") + "</div>");
						output("<div>" + DicValue("timeSpent") + "</div>");
						
						output("<div>" + DicValue("score") + "</div>");
						output("<div>Highscore</div>");
						output("<div>" + DicValue("attribute") + "</div>");
						output("<div>" + DicValue("attributeValue") + "</div>");
						output("<div>" + DicValue("comment") + "</div>");
						output("</div>");
					}
					string eyeTestName = "";
					if (!tests.ContainsKey(log.ProgramEyeTest.EyeTestID)) {
						tests.Add(log.ProgramEyeTest.EyeTestID, ipa.EyeTestGetSingle(log.ProgramEyeTest.EyeTestID));
					}
					if (tests.ContainsKey(log.ProgramEyeTest.EyeTestID)) {
						eyeTestName = tests[log.ProgramEyeTest.EyeTestID].InfoValue("Name", CurrentLanguage);
					}

					TimeSpan timeSpent = TimeSpan.FromSeconds(0);
					string strTS = "";
					if (log.EndTime.HasValue) {
						timeSpent = log.EndTime.Value.Subtract(log.StartTime);
						strTS = timeSpent.Minutes.ToString().PadLeft(2, '0') + ":" + timeSpent.Seconds.ToString().PadLeft(2, '0');
					}

					output("<div class=\"logEntry\">"
						+ "<div>" + eyeTestName + "</div>"
						+ "<div class=\"nowrap\">" + log.StartTime.ToString(dtFormat) + "</div>"
						+ "<div class=\"nowrap\">" + (log.EndTime.HasValue ? strTS : "") + "</div>"
						+ "<div>" + log.Score + "</div>"
						+ "<div>" + (log.HighScore ? "<img src=\"/img/star.png\" alt=\"high score\" />" : "&nbsp;") + "</div>"
						+ "<div>&nbsp;" + log.AttribName + "</div>"
						+ "<div>&nbsp;" + log.AttribValue + "</div>"
						+ "<div><img src=\"/img/comment.png\" class=\"clientLogEditComment\" title=\"" + ("addComment") + "\" alt=\"" + DicValue("addComment") + "\" entryid=\"" + log.ID + "\" />" + Server.HtmlEncode(log.Comment ?? "").Replace("\n", "<br />") + "</div>"
						+ "</div>");
				} // foreach
			}
			else if (groupby == "eyetest") {
				foreach (var log in logs.OrderBy(n => n.ProgramEyeTestID).ThenByDescending(n => n.StartTime)) {
					string eyeTestName = "";
					if (!tests.ContainsKey(log.ProgramEyeTest.EyeTestID)) {
						tests.Add(log.ProgramEyeTest.EyeTestID, ipa.EyeTestGetSingle(log.ProgramEyeTest.EyeTestID));
					}
					if (tests.ContainsKey(log.ProgramEyeTest.EyeTestID)) {
						eyeTestName = tests[log.ProgramEyeTest.EyeTestID].InfoValue("Name", CurrentLanguage);
					}

					if (eyeTestTemp != eyeTestName) {
						eyeTestTemp = eyeTestName;
						output("<div class=\"logHeading\">" + eyeTestTemp + "</div>");
						output("<div class=\"logTh\">");
						output("<div>" + DicValue("date") + "</div>");
						output("<div>" + DicValue("timeSpent") + "</div>");
						output("<div>" + DicValue("score") + "</div>");
						output("<div>Highscore</div>");
						output("<div>" + DicValue("attribute") + "</div>");
						output("<div>" + DicValue("attributeValue") + "</div>");
						output("<div>" + DicValue("comment") + "</div>");
						output("</div>");
					}

					TimeSpan timeSpent = TimeSpan.FromSeconds(0);
					string strTS = "";
					if (log.EndTime.HasValue) {
						timeSpent = log.EndTime.Value.Subtract(log.StartTime);
						strTS = timeSpent.Minutes.ToString().PadLeft(2, '0') + ":" + timeSpent.Seconds.ToString().PadLeft(2, '0');
					}

					output("<div class=\"logEntry\">"
						+ "<div class=\"nowrap\">" + log.StartTime.ToString("g") + "</div>"
						+ "<div class=\"nowrap\">" + (log.EndTime.HasValue ? strTS : "") + "</div>"
						+ "<div>" + log.Score + "</div>"
						+ "<div>" + (log.HighScore ? "<img src=\"/img/star.png\" alt=\"high score\" />" : "&nbsp;") + "</div>"
						+ "<div>&nbsp;" + log.AttribName + "</div>"
						+ "<div>&nbsp;" + log.AttribValue + "</div>"
						+ "<div><img src=\"/img/comment.png\" class=\"clientLogEditComment\" title=\"" + ("addComment") + "\" alt=\"" + DicValue("addComment") + "\" entryid=\"" + log.ID + "\" />" + Server.HtmlEncode(log.Comment ?? "").Replace("\n", "<br />") + "</div>"
						+ "</div>");
				} // foreach
			}
		}
	}

	private void output(string s) {
		plhLog.Controls.Add(new LiteralControl() { EnableViewState = false, Text = s });
	}

	private void closeWindow() {
		AddJavascript("$(function() { window.close(); });");
	}

}