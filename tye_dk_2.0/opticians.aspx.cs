using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class opticians : PageBase {

	int EditID = 0;
	bool Editing = false;
	PropertyMapper PM = null;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA, 
			tye.Data.User.UserType.Administrator, 
			tye.Data.User.UserType.Distributor }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");

	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		PM = new PropertyMapper(new object());

		if (CurrentUser.Type == tye.Data.User.UserType.Distributor) {
			lnkImpersonate.Visible = false;
			lnkImpersonateWithLanguage.Visible = false;
			lnkActivationCodeReset.Visible = false;
		}

		PM.AddMapping(tbAddress, "Address");
		PM.AddMapping(tbCity, "City");
		PM.AddMapping(tbEmail, "Email");
		PM.AddMapping(tbMobilePhone, "MobilePhone");
		PM.AddMapping(tbName, "FirstName");
		PM.AddMapping(tbPhone, "Phone");
		PM.AddMapping(tbPostalCode, "PostalCode");
		PM.AddMapping(tbState, "State");
		PM.AddMapping(ddlCountry, "CountryID");
		PM.AddMapping(ddlLanguage, "LanguageID");
		//PM.AddMapping(ddlUserType, "Type");
		PM.AddMapping(chkEnabled, "Enabled");
		PM.AddMapping(chkShowOnMap, "ShowOnMap");

		int.TryParse(VC.RqValue("ID"), out EditID);
		Editing = VC.RqHasValue("ID");

		plhEdit.Visible = Editing;
		plhList.Visible = !plhEdit.Visible;

		if (Editing) {

			using (var ipa = statics.GetApi()) {
				ddlCountry.DataSource = ipa.CountryGetCollection();
				ddlCountry.DataTextField = "Name";
				ddlCountry.DataValueField = "ID";
				ddlCountry.DataBind();

				ddlLanguage.DataSource = ipa.LanguageGetCollection();
				ddlLanguage.DataTextField = "Name";
				ddlLanguage.DataValueField = "ID";
				ddlLanguage.DataBind();

			}
			lnkCreateNew.Visible = false;
			populateData();
		} else {
			using (var ipa = statics.GetApi()) {
				if (!IsPostBack) {
					ddlFilterLanguage.DataSource = ipa.LanguageGetCollection();
					ddlFilterLanguage.DataTextField = "Name";
					ddlFilterLanguage.DataValueField = "ID";
					ddlFilterLanguage.DataBind();
					populateOpticianLists();
				}
			}
		}
	}

	protected void ddlFilterLanguage_SelectedIndexChanged(object sender, EventArgs e) {
		populateOpticianLists();
	}

	protected void eLnkCreateNewCodes_Click(object sender, EventArgs e) {
		if (!CurrentUserIsAdmin())
			redir();

		int CodeLength = 6;
		using (var ipa = statics.GetApi()) {
			int iTry = 0;
			if (int.TryParse(ntbActivationCodesQuantity.Text, out iTry) && iTry > 0 && iTry <= 50) {
				for (int i = 0; i < iTry; i++) {
					string code = Strings.CreateRandomString(CodeLength, false);

					while (code.Contains("0") || code.Contains("O") || code.Contains("o") || code.Contains("1") || code.Contains("l") || code.Contains("I")) {
						code = Strings.CreateRandomString(CodeLength, false);
					}

					var newCode = new tye.Data.ActivationCode() { 
						ActivationDate = null,
						ClientUserID = null,
						Code = code,
						ExpirationDate = null,
						ID = 0,
						OpticianUserID = EditID,
						Printed = false
					};
					
					while(ipa.ActivationCodeGetSingle(newCode.Code) != null) {
						newCode.Code = Strings.CreateRandomString(CodeLength, false);
						while (newCode.Code.Contains("0") || newCode.Code.Contains("O") || newCode.Code.Contains("o") || newCode.Code.Contains("1") || newCode.Code.Contains("l") || newCode.Code.Contains("I")) {
							newCode.Code = Strings.CreateRandomString(CodeLength, false);
						}
					}

					ipa.ActivationCodeSave(newCode);
				}
			} // if
		} // using

		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}
	
	protected void eLnkDeleteOptician_Click(object sender, EventArgs e) {
		if (!CurrentUserIsAdmin())
			redir();

		using (var ipa = statics.GetApi()) {
			var user = ipa.UserGetSingle(EditID);
			if (user != null && user.Type == tye.Data.User.UserType.Optician) {
				// todo: delete activation codes first - 27273
				var codes = ipa.ActivationCodeGetCollection(user.ID);
				foreach (var code in codes) {
					ipa.ActivationCodeDelete(code.ID);
				}

				var clients = ipa.UserGetCollectionByOptician(user.ID);

				ipa.UserDeletePermanently(user.ID);

				foreach (var client in clients) {
					ipa.UserDeletePermanently(client.ID);
				}
			}
		}

		redir();
	}

	protected void eLnkImpersonate_Click(object sender, EventArgs e) {
		LinkButton snd = (LinkButton)sender;

		if (CurrentUser.Type != tye.Data.User.UserType.SBA & CurrentUser.Type != tye.Data.User.UserType.Administrator) {
			return;
		}

		using (var ipa = statics.GetApi()) {
			var imp = ipa.UserGetSingle(EditID);
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
		AddJavascript("setTimeout(function() { window.location.href='" + VC.QueryStringStripNoTrail("ID") + "'}, 1000);");
		
	}

	protected void eLnkSaveAndClose_Click(object sender, EventArgs e) {
		save();
		Response.Redirect(VC.QueryStringStripNoTrail("id"));
	}
	
	protected void eLnkSave_Click(object sender, EventArgs e) {
		Response.Redirect(VC.QueryStringStrip("id") + "id=" + save());
	}

	protected void ElnkSendNewPassword_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			var opt = ipa.UserGetSingle(EditID);
			if (opt != null && opt.Type == tye.Data.User.UserType.Optician) { 
				if(!RegExp.IsValidEmail(opt.Email)) {
					pnlError.Controls.Add(new LiteralControl("Cannot create new password. Email address missing!<br /><br />"));
					pnlError.Visible = true;
					return;
				}
				string newPassword = Strings.CreateRandomString(8, false);
				opt.Password = tye.Data.User.EncryptPassword(newPassword, statics.App.GetSetting(SettingsKeys.EncryptionKey));
				opt.MustChangePassword = true;
				ipa.UserSave(opt);

				//statics.SendEmailDefaultTemplate(opt.Email, "New password for trainyoureyes.com", "New password", "Your new password is: " + newPassword);
				statics.SendEmail(opt.Email, "New password for trainyoureyes.com", "Your new password for trainyoureyes.com is: " + newPassword);

				pnlError.CssClass = "successInline";
				pnlError.Controls.Add(new LiteralControl("Email sent! New password is: " + newPassword + "<br /><br />"));
				pnlError.Visible = true;
			}
		}
	}

	private int save() {
		tye.Data.User user = new tye.Data.User();
		using (var ipa = statics.GetApi()) {
			if (EditID > 0) { // existing
				user = ipa.UserGetSingle(EditID);
			} else {
				user.CreatedOn = DateTime.Now;
			}
			if (user != null) {
				PM.Object = user;
				PM.MapToProperties();
				user.MiddleName = "";
				user.LastName = "";
				user.Type = tye.Data.User.UserType.Optician;
				//user.Type = (tye.Data.User.UserType)Convert.ToInt32(ddlUserType.SelectedValue);
				
				ipa.UserSave(user);
			}
		}
		return user.ID;		
	}

	private void populateData() {
		using (var ipa = statics.GetApi()) {
			if (EditID > 0) { // existing
				var user = ipa.UserGetSingle(EditID);
				if (user == null || user.Type != tye.Data.User.UserType.Optician)
					redir();

				PM.Object = user;
				PM.MapToControls();

				
				var codes = ipa.ActivationCodeGetCollection(EditID);
				if (codes.Any(/*n => !n.Printed*/)) {
					lnkPrintCodes.Visible = true;
					lnkPrintCodes.NavigateUrl = "activationCodePrint.aspx?OpticianID=" + EditID;
					lnkActivationCodeReset.NavigateUrl = "activationCodeReset.aspx?OpticianUserID=" + EditID;
				}

				litRemainingCodes.Text = "Optician has <strong>"
					+ ipa.ActivationCodesRemaining(EditID).Count + "</strong> remaining codes.";
			} else {
				adminOptions.Visible = false;
			}
		}
	}

	private void populateOpticianLists() {
		int intLanguageID = 1;
		if (!int.TryParse(ddlFilterLanguage.SelectedValue, out intLanguageID))
			intLanguageID = 1;

		using (var ipa = statics.GetApi()) {
			repOpticiansActive.DataSource = ipa.UserGetCollection(tye.Data.User.UserType.Optician,
				intLanguageID)
				.Where(n => n.Enabled);
			repOpticiansActive.DataBind();

			repOpticiansInactive.DataSource = ipa.UserGetCollection(tye.Data.User.UserType.Optician,
				intLanguageID)
				.Where(n => !n.Enabled);
			repOpticiansInactive.DataBind();

			litActiveCount.Text = repOpticiansActive.Items.Count.ToString();
			litInactiveCount.Text = repOpticiansInactive.Items.Count.ToString();
		}
	}

	private void redir() {
		Response.Redirect(VC.QueryStringStripNoTrail("id"));
	}
}