using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class clientAnamnese : PageBase {

	PropertyMapper PM = new PropertyMapper(null);
	int EditID = 0;
	protected int ClientUserID = 0;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Optician 
			//,tye.Data.User.UserType.Administrator
			//,tye.Data.User.UserType.SBA
		}
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();

		int.TryParse(VC.RqValue("ID"), out EditID);
		int.TryParse(VC.RqValue("ClientUserID"), out ClientUserID);

		using (var ipa = statics.GetApi()) {
			var client = ipa.UserGetSingle(ClientUserID);
			if (client != null)
				litClientName.Text = " - <a href=\"clients.aspx?id=" + client.ID + "\">" + client.FullName + "</a>";

			if (EditID > 0 || VC.RqValue("ID") == "0") {
				// form
				tye.Data.Anamnese anam = null;
				if (EditID > 0)
					anam = ipa.AnamneseGetSingle(EditID);

				plhList.Visible = false;
				plhEdit.Visible = true;

				if (anam != null) {
					var optician = ipa.OpticianClientGetOptician(anam.ClientUserID);
					if (optician == null || optician.ID != CurrentUser.ID)
						redir();

					//tbComment.Text = anam.Comments;
					//tbInjuries.Text = anam.Injuries;
					tbMedicine.Text = anam.Medication;
					tbSickness.Text = anam.Sicknesses;
					ntbHoursNear.Text = anam.DailyCloseRangeWork.ToString();
					ntbReadingHours.Text = anam.MaxReadingHours.ToString();
				}

				for (int i = 1; i <= 20; i++) {
					TableRow tr = new TableRow();
					tr.ID = "row" + i;
					tr.Cells.Add(new TableCell() { Text = i + "." });
					tr.Cells.Add(new TableCell() { Text = DicValue("anam_Q" + i) });
					int val = -1;
					if (anam != null) {
						int.TryParse(anam.GetType().GetProperty("Q" + i).GetValue(anam, null).ToString(), out val);
					}

					for (int j = 0; j <= 5; j++) {
						CheckBox chk = new CheckBox() { ID = "chk" + i + j };
						var cell = new TableCell();
						cell.Controls.Add(chk);
						tr.Cells.Add(cell);
						if (val == j)
							chk.Checked = true;
					}
					tblProgram.Rows.Add(tr);
				}
			} else {
				// list
				plhList.Visible = true;
				plhEdit.Visible = false;

				foreach (var anam in ipa.AnamneseGetCollection(ClientUserID)) {
					var l = new LiteralControl("");
					l.EnableViewState = false;
					l.Text = "<a class=\"btn\" href=\"" + VC.QueryStringStrip("") + "ID=" + anam.ID + "\">" + anam.Created.ToString("d") + "</a><br />";
					plhListList.Controls.Add(l);
				}
			}
		}
	}

	protected void eLnkSave_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {

			var anam = new tye.Data.Anamnese() { ID = 0, ClientUserID = ClientUserID, Created = DateTime.Now };
			if (EditID > 0) {
				anam = ipa.AnamneseGetSingle(EditID);
			}

			if (anam != null) {
				//anam.Comments = tbComment.Text;
				//anam.Injuries = tbInjuries.Text;
				anam.Medication = tbMedicine.Text;
				anam.Sicknesses = tbSickness.Text;
				anam.Created = DateTime.Now;

				int itry = 0;
				int.TryParse(ntbHoursNear.Text, out itry);
				anam.DailyCloseRangeWork = itry;

				int.TryParse(ntbReadingHours.Text, out itry);
				anam.MaxReadingHours = itry;

				for (int i = 1; i <= 20; i++) {
					TableRow tr = (TableRow)tblProgram.FindControl("row" + i);
					if (tr == null)
						continue;

					for (int j = 0; j <= 5; j++) {
						CheckBox chk = (CheckBox)VC.FindControlRecursive(tr, "chk" + i + j);
						if (chk == null || !chk.Checked)
							continue;
						anam.GetType().GetProperty("Q" + i).SetValue(anam, j, null);
					}
				}
				ipa.AnamneseSave(anam);
			}
		}
		eLnkCancel_Click(lnkCancel, new EventArgs());
	}

	protected void eLnkCancel_Click(object sender, EventArgs e) {
		if (getRu() != "")
			Response.Redirect(Server.UrlDecode(VC.RqValue("ru")));
		else
			Response.Redirect("clientAnamnese.aspx?ClientUserID=" + VC.RqValue("ClientUserID"));
	}

	private void redir() {
		if (VC.RqHasValue("ru"))
			Response.Redirect(VC.RqValue("ru"));
		else
			Response.Redirect("clients.aspx");
	}

	protected string getRu() {
		if (VC.RqHasValue("ru"))
			return "&ru=" + Server.UrlEncode(VC.RqValue("ru"));
		else
			return "";
	}

}