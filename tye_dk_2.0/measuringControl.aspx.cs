using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class measuringControl : PageBase {

	protected int ClientUserID = 0;
	int MeasuringID = 0;
	int StepNumber = 1;
	bool IsFakeStartMeasuring = false;
	bool Editing = false;
	PropertyMapper PM = null;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Optician,
			tye.Data.User.UserType.Client }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		if (!int.TryParse(VC.RqValue("ClientUserID"), out ClientUserID) || ClientUserID == 0)
			redir();

		if (!int.TryParse(VC.RqValue("Step"), out StepNumber))
			StepNumber = 1;

		if (!int.TryParse(VC.RqValue("MeasuringID"), out MeasuringID))
			MeasuringID = -1;

		IsFakeStartMeasuring = (VC.RqValue("isStart") == "true");

		litheading.Text = (IsFakeStartMeasuring ? DicValue("startMeasuring") : DicValue("mc_namePlural"));
		plhGenerel.Visible = !IsFakeStartMeasuring;

		foreach (DropDownList ddl in VC.FindControlTypeRecursive(this.Form, typeof(DropDownList))) {
			var items = ddl.Items.Cast<ListItem>().Where(n => n.Text.StartsWith("{")).ToList();
			if (items.Any()) {
				foreach (var item in items) {
					item.Text = DicValue(item.Text.Remove(0, 1).Replace("}", ""));
				}
			}
		}

		PM = new PropertyMapper(null);
		PM.AddMapping(ddlMc11, "Convergence1");
		PM.AddMapping(ddlMc12, "Convergence2");
		PM.AddMapping(ddlMc13, "Convergence3");
		PM.AddMapping(ddlMotility1, "MotilityPointsRightEye");
		PM.AddMapping(ddlMotility2, "MotilityPointsLeftEye");
		PM.AddMapping(ddlMotility3, "MotilityPointsBothEyes");
		PM.AddMapping(ddlMotility4, "MotilityHeadMovements");
		PM.AddMapping(ddlMotility5, "MotilityHorizontalEyeMovements");
		PM.AddMapping(ddlMotility6, "MotilityDidClientSway");
		PM.AddMapping(tbMotility1, "NoteMotilityPointsRightEye");
		PM.AddMapping(tbMotility2, "NoteMotilityPointsLeftEye");
		PM.AddMapping(tbMotility3, "NoteMotilityPointsBothEyes");
		PM.AddMapping(tbMotility4, "NoteMotilityHeadMovements");
		PM.AddMapping(tbMotility5, "NoteMotilityHorizontalEyeMovements");
		PM.AddMapping(tbMotility6, "NoteMotilityDidClientSway");
		PM.AddMapping(tbStep1Changes, "Step1Changes");
		PM.AddMapping(tbStep1Comments, "Step1Comments");

		using (var ipa = statics.GetApi()) {
			var user = ipa.UserGetSingle(ClientUserID);
			var optician = ipa.OpticianClientGetOptician(ClientUserID);
			
			// user and optician must be valid data. Optician must "own" user.
			if (user == null || optician == null || (optician.ID != CurrentUser.ID))
				redir();
			
			lnkBackToClient.NavigateUrl = "clients.aspx?id=" + user.ID;
			lnkBackToClient.Text = " - " + user.FullName;

			if (MeasuringID > -1) {
				pnlList.Visible = false;

				var pnlVisible = (Panel)pnlList.Parent.FindControl("pnlMeasuringControlStep" + StepNumber);
				if (pnlVisible != null)
					pnlVisible.Visible = true;

				var mc = ipa.MeasuringControlGetSingle(MeasuringID);
				if (mc != null && mc.ClientUserID != ClientUserID) // measuring must belong to user
					redir();

				if (mc != null) {
					PM.Object = mc;
					PM.MapToControls();
					lnkStep1Next.Visible = false;
				}
			} else {
				populateList();
			}
		}
	}

	protected void eLnkImpersonate_Click(object sender, EventArgs e) {
		LinkButton snd = (LinkButton)sender;

		if (!CurrentUserIsAdmin()) {
			return;
		}

		using (var ipa = statics.GetApi()) {
			var imp = ipa.UserGetSingle(ClientUserID);
			if (imp != null && (imp.Type != tye.Data.User.UserType.SBA)) {
				if (snd.ID != "lnkImpersonateWithLanguage") {
					imp.Language = CurrentUser.Language;
				}
				Impersonating = true;

				var cu = CurrentUser;
				cu.ImpersonatingUser = imp;
				CurrentUser = cu;
			}
		}
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	protected void eLnkSaveAndClose_Click(object sender, EventArgs e) {
		int intSave = save(((LinkButton)sender).ID);
		//if (intSave <= 0)
		//   return;

		if (StepNumber >= 1)
			redir();
		else
			Response.Redirect(VC.QueryStringStrip("Step") + "Step=" + ++StepNumber);
	}

	protected void eLnkNext_Click(object sender, EventArgs e) {
		int intSave = save(((LinkButton)sender).ID);
		if (StepNumber >= 1)
			redir();
		else
			Response.Redirect(VC.QueryStringStrip("Step") + "Step=" + ++StepNumber);
	}

	protected string getRu() {
		if (VC.RqHasValue("ru"))
			return "&ru=" + VC.RqValue("ru");
		else
			return "";
	}

	private void populateList() {
		using (var ipa = statics.GetApi()) {
			var user = ipa.UserGetSingle(ClientUserID);
			var optician = ipa.OpticianClientGetOptician(ClientUserID);

			if (optician == null || optician.ID != CurrentUser.ID)
				redir();

			// skip one for the fake start measure
			repList.DataSource = ipa.MeasuringControlGetCollection(ClientUserID)
				.OrderBy(n => n.Created)
				.Skip(1)
				.ToList()
				.OrderByDescending(n => n.Created);
			repList.DataBind();
		}
	}

	private int save(string senderLinkButtonID) {
		using (var ipa = statics.GetApi()) {
			tye.Data.MeasuringControl mc = new tye.Data.MeasuringControl() { ClientUserID = ClientUserID };
			if (MeasuringID > 0)
				mc = ipa.MeasuringControlGetSingle(MeasuringID);
			if (mc == null)
				mc = new tye.Data.MeasuringControl() { ClientUserID = ClientUserID };

			PM.Object = mc;
			PM.MapToProperties();

			ipa.MeasuringControlSave(mc);
		}
		return 0;
	}

	private void redir() {
		//if (VC.RqHasValue("ru"))
		//   Response.Redirect(VC.RqValue("ru"));
		//else
		Response.Redirect(VC.QueryStringStripNoTrail("MeasuringID"));
	}
}