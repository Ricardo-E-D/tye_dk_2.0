using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;
using System.Reflection;

public partial class measuring21 : PageBase {

	protected int ClientUserID = 0;
	int MeasuringID = 0;
	int StepNumber = 1;
	bool Editing = false;
	PropertyMapper PM = null;
	string[] nullables = new string[] { "ntb21_9", "ntb21_16a", "ntb21_17a" };

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

		foreach (DropDownList ddl in VC.FindControlTypeRecursive(this.Form, typeof(DropDownList))) {
			var items = ddl.Items.Cast<ListItem>().Where(n => n.Text.StartsWith("{")).ToList();
			if (items.Any()) {
				foreach (var item in items) {
					item.Text = DicValue(item.Text.Remove(0, 1).Replace("}", ""));
				}
			}
		}

		PM = new PropertyMapper(null);
		PM.AddMapping(ntb21_3, "ntb21_3");
		PM.AddMapping(ddlCoverTestFar, "CoverTestFar");
		PM.AddMapping(ddlCoverTestNear, "CoverTestNear");
		PM.AddMapping(ddlDominans, "Dominans");
		PM.AddMapping(ddlVisusBinocular, "VisusBinocular");
		PM.AddMapping(ddlVisusLeft, "VisusLeft");
		PM.AddMapping(ddlVisusRight, "VisusRight");
		PM.AddMapping(tbFixation, "FixationParity");
		PM.AddMapping(tbPupilReflex, "PupilReflex");
		PM.AddMapping(tbSettingsAreaFrom, "SettingsAreaFrom");
		PM.AddMapping(tbSettingsAreaTo, "SettingsAreaTo");
		PM.AddMapping(tbStereopsisFar, "StereopsisFar");
		PM.AddMapping(tbStereopsisNear, "StereopsisNear");
		PM.AddMapping(tbVisusBinocularComment, "VisusBinocularComment");
		PM.AddMapping(tbVisusLeftComment, "VisusLeftComment");
		PM.AddMapping(tbVisusRightComment, "VisusRightComment");
		PM.AddMapping(ddlHabitualVisusBinocular, "HabituelBinocular");
		PM.AddMapping(tbHabitualVisusBinocularComment, "HabituelBinocularComment");
		PM.AddMapping(ddlHabitualVisusLeft, "HabituelLeft");
		PM.AddMapping(tbHabitualVisusLeftComment, "HabituelLeftComment");
		PM.AddMapping(ddlHabitualVisusRight, "HabituelRight");
		PM.AddMapping(tbHabitualVisusRightComment, "HabituelRightComment");

		PM.AddMapping(tbVisionField, "VisionField");

		PM.AddMapping(ddlPinHoleBinocular, "PinholevisusBinocular");
		PM.AddMapping(ddlPinHoleLeft, "PinholevisusLeft");
		PM.AddMapping(ddlPinHoleRight, "PinholevisusRight");

		PM.AddMapping(tbPinHoleBinocularComment, "PinholevisusBinocularComment");
		PM.AddMapping(tbPinHoleLeftComment, "PinholevisusLeftComment");
		PM.AddMapping(tbPinHoleRightComment, "PinholevisusRightComment");


		
		PM.AddMapping(ntb21_13a, "ntb21_13a");
		PM.AddMapping(ntb21_4Hsf, "ntb21_4Hsf");
		PM.AddMapping(ntb21_4Hcyl1, "ntb21_4Hcyl1");
		PM.AddMapping(ntb21_4Hcyl2, "ntb21_4Hcyl2");
		PM.AddMapping(ntb21_4Hvisus, "ntb21_4Hvisus");
		PM.AddMapping(ntb21_4Vsf, "ntb21_4Vsf");
		PM.AddMapping(ntb21_4Vcyl1, "ntb21_4Vcyl1");
		PM.AddMapping(ntb21_4Vcyl2, "ntb21_4Vcyl2");
		PM.AddMapping(ntb21_4Vvisus, "ntb21_4Vvisus");
		PM.AddMapping(ntb21_5Hsf, "ntb21_5Hsf");
		PM.AddMapping(ntb21_5Hlag, "ntb21_5Hlag");
		PM.AddMapping(ntb21_5Hnetto, "ntb21_5Hnetto");
		PM.AddMapping(ntb21_5Vsf, "ntb21_5Vsf");
		PM.AddMapping(ntb21_5Vlag, "ntb21_5Vlag");
		PM.AddMapping(ntb21_5Vnetto, "ntb21_5Vnetto");
		PM.AddMapping(ntb21_7Hsf, "ntb21_7Hsf");
		PM.AddMapping(ntb21_7Hcyl1, "ntb21_7Hcyl1");
		PM.AddMapping(ntb21_7Hcyl2, "ntb21_7Hcyl2");
		PM.AddMapping(ntb21_7Hvisus, "ntb21_7Hvisus");
		PM.AddMapping(ntb21_7Vsf, "ntb21_7Vsf");
		PM.AddMapping(ntb21_7Vcyl1, "ntb21_7Vcyl1");
		PM.AddMapping(ntb21_7Vcyl2, "ntb21_7Vcyl2");
		PM.AddMapping(ntb21_7Vvisus, "ntb21_7Vvisus");
		PM.AddMapping(ntb21_7aHsf, "ntb21_7aHsf");
		PM.AddMapping(ntb21_7aHcyl1, "ntb21_7aHcyl1");
		PM.AddMapping(ntb21_7aHcyl2, "ntb21_7aHcyl2");
		PM.AddMapping(ntb21_7aHvisus, "ntb21_7aHvisus");
		PM.AddMapping(ntb21_7aVsf, "ntb21_7aVsf");
		PM.AddMapping(ntb21_7aVcyl1, "ntb21_7aVcyl1");
		PM.AddMapping(ntb21_7aVcyl2, "ntb21_7aVcyl2");
		PM.AddMapping(ntb21_7aVvisus, "ntb21_7aVvisus");
		PM.AddMapping(ntb21_8, "ntb21_8");
		PM.AddMapping(ntb21_9, "ntb21_9");
		PM.AddMapping(ntb21_10_1, "ntb21_10_1");
		PM.AddMapping(ntb21_10_2, "ntb21_10_2");
		PM.AddMapping(ntb21_11_1, "ntb21_11_1");
		PM.AddMapping(ntb21_11_2, "ntb21_11_2");
		PM.AddMapping(ntb21_12Hs, "ntb21_12Hs");
		PM.AddMapping(ntb21_12Hi, "ntb21_12Hi");
		PM.AddMapping(ntb21_12Vs, "ntb21_12Vs");
		PM.AddMapping(ntb21_12Vi, "ntb21_12Vi");
		PM.AddMapping(ntb21_13b, "ntb21_13b");
		PM.AddMapping(ntb21_14aHsf, "ntb21_14aHsf");
		PM.AddMapping(ntb21_14aHlag, "ntb21_14aHlag");
		PM.AddMapping(ntb21_14aHnetto, "ntb21_14aHnetto");
		PM.AddMapping(ntb21_14aVsf, "ntb21_14aVsf");
		PM.AddMapping(ntb21_14aVlag, "ntb21_14aVlag");
		PM.AddMapping(ntb21_14aVnetto, "ntb21_14aVnetto");
		PM.AddMapping(ntb21_15a, "ntb21_15a");
		PM.AddMapping(ntb21_14bHsf, "ntb21_14bHsf");
		PM.AddMapping(ntb21_14bHlag, "ntb21_14bHlag");
		PM.AddMapping(ntb21_14bHnetto, "ntb21_14bHnetto");
		PM.AddMapping(ntb21_14bVsf, "ntb21_14bVsf");
		PM.AddMapping(ntb21_14bVlag, "ntb21_14bVlag");
		PM.AddMapping(ntb21_14bVnetto, "ntb21_14bVnetto");
		PM.AddMapping(ntb21_15b, "ntb21_15b");
		PM.AddMapping(ntb21_16a, "ntb21_16a");
		PM.AddMapping(ntb21_16b_1, "ntb21_16b_1");
		PM.AddMapping(ntb21_16b_2, "ntb21_16b_2");
		PM.AddMapping(ntb21_17a, "ntb21_17a");
		PM.AddMapping(ntb21_17b_1, "ntb21_17b_1");
		PM.AddMapping(ntb21_17b_2, "ntb21_17b_2");
		PM.AddMapping(ntb21_18Hs, "ntb21_18Hs");
		PM.AddMapping(ntb21_18Hi, "ntb21_18Hi");
		PM.AddMapping(ntb21_18Vs, "ntb21_18Vs");
		PM.AddMapping(ntb21_18Vi, "ntb21_18Vi");
		PM.AddMapping(ntb21_19right, "ntb21_19right");
		PM.AddMapping(ntb21_19left, "ntb21_19left");
		PM.AddMapping(ntb21_19both, "ntb21_19both");
		PM.AddMapping(ntb21_20, "ntb21_20");
		PM.AddMapping(ntb21_21, "ntb21_21");
		
		PM.AddMapping(ddl21_3, "ddl21_3");
		PM.AddMapping(ddl21_13a, "ddl21_13a");
		PM.AddMapping(ddl21_4H, "ddl21_4H");
		PM.AddMapping(ddl21_4V, "ddl21_4V");
		PM.AddMapping(ddl21_5H, "ddl21_5H");
		PM.AddMapping(ddl21_5V, "ddl21_5V");
		PM.AddMapping(ddl21_7H, "ddl21_7H");
		PM.AddMapping(ddl21_7V, "ddl21_7V");
		PM.AddMapping(ddl21_7aH, "ddl21_7aH");
		PM.AddMapping(ddl21_7aV, "ddl21_7aV");
		PM.AddMapping(ddl21_8, "ddl21_8");
		PM.AddMapping(ddl21_10, "ddl21_10");
		PM.AddMapping(ddl21_11, "ddl21_11");
		PM.AddMapping(ddl21_12, "ddl21_12");
		PM.AddMapping(ddl21_13b, "ddl21_13b");
		PM.AddMapping(ddl21_14aH, "ddl21_14aH");
		PM.AddMapping(ddl21_14aV, "ddl21_14aV");
		PM.AddMapping(ddl21_15a, "ddl21_15a");
		PM.AddMapping(ddl21_14bH, "ddl21_14bH");
		PM.AddMapping(ddl21_14bV, "ddl21_14bV");
		PM.AddMapping(ddl21_15b, "ddl21_15b");
		PM.AddMapping(ddl21_16b, "ddl21_16b");
		PM.AddMapping(ddl21_17b, "ddl21_17b");
		PM.AddMapping(ddl21_18, "ddl21_18");

		PM.AddMapping(rb21_12H, "rb21_12H");
		PM.AddMapping(rb21_12V, "rb21_12V");
		PM.AddMapping(rb21_18H, "rb21_18H");
		PM.AddMapping(rb21_18V, "rb21_18V");

		/*
		string qqq = "";
		string qqq2 = "";
		foreach (var control in VC.FindControlTypeRecursive(pnlMeasuring21Step1, typeof(monosolutions.Controls.NumericTextBox))) {
			qqq += "PM.AddMapping(" + control.ID + ", \"" + control.ID + "\");<br />";
			qqq2 += "public decimal " + control.ID + " { get; set; }<br />";
		}
		foreach (var control in VC.FindControlTypeRecursive(pnlMeasuring21Step1, typeof(DropDownList))) {
			qqq += "PM.AddMapping(" + control.ID + ", \"" + control.ID + "\");<br />";
			qqq2 += "public string " + control.ID + " { get; set; }<br />";
		}
		Response.Write(qqq);
		Response.Write(qqq2);
		*/

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

				var pnlVisible = (Panel)pnlList.Parent.FindControl("pnlMeasuring21Step" + StepNumber);
				if (pnlVisible != null)
					pnlVisible.Visible = true;

				var mc = ipa.Measuring21GetSingle(MeasuringID);

				if (mc != null) {
					PM.Object = mc;
					PM.MapToControls();
					if (mc.ntb21_9.HasValue)
						ntb21_9.Text = mc.ntb21_9.ToString();//.Replace(".", ",");
					if (mc.ntb21_16a.HasValue)
						ntb21_16a.Text = mc.ntb21_16a.ToString();//.Replace(".", ",");
					if (mc.ntb21_17a.HasValue)
						ntb21_17a.Text = mc.ntb21_17a.ToString();//.Replace(".", ",");
					//lnkStep1Next.Visible = false;
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

			repList.DataSource = ipa.Measuring21GetCollection(ClientUserID);
			repList.DataBind();
		}
	}

	private int save(string senderLinkButtonID) {
		using (var ipa = statics.GetApi()) {
			tye.Data.Measuring21 mc = new tye.Data.Measuring21() { ClientUserID = ClientUserID };
			if (MeasuringID > 0)
				mc = ipa.Measuring21GetSingle(MeasuringID);
			if (mc == null)
				mc = new tye.Data.Measuring21() { ClientUserID = ClientUserID };

			PM.Object = mc;
			PM.MapToProperties();

			// decimal values doesn't map correctly - do it "manually"
			foreach (PropertyInfo prop in mc.GetType().GetProperties()) {
				if (prop.PropertyType.Name == "Decimal") {
					string w = "";
					monosolutions.Controls.NumericTextBox ntb = (monosolutions.Controls.NumericTextBox)pnlMeasuring21Step1.FindControl(prop.Name);
					if (ntb != null) {
						decimal dTry = 0;
						if (decimal.TryParse(ntb.Text.Replace(".", ","), out dTry)) {
							prop.SetValue(mc, dTry, null);
						}
					}
				}
			}

			if (string.IsNullOrEmpty(ntb21_9.Text)) {
				mc.ntb21_9 = null;
			} else {
				mc.ntb21_9 = decimal.Parse(ntb21_9.Text.Replace(".", ","));
			}



			if (string.IsNullOrEmpty(ntb21_16a.Text)) {
				mc.ntb21_16a = null;
			} else {
				mc.ntb21_16a = decimal.Parse(ntb21_16a.Text.Replace(".", ","));
			}



			if (string.IsNullOrEmpty(ntb21_17a.Text)) {
				mc.ntb21_17a = null;
			} else {
				mc.ntb21_17a = decimal.Parse(ntb21_17a.Text.Replace(".", ","));
			}

			ipa.Measuring21Save(mc);
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