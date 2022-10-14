using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using tye.Data;
using monosolutions.Utils;
public partial class loginChangePassword : PageBase {
	
	protected void Page_Init(object sender, EventArgs e) {
		Form.DefaultFocus = tbP.ClientID;
		var test = CurrentUser; // eval logged in
		valiRegExpNewUserPassword.ErrorMessage = "<div class=\"errorInline\">" + DicValue("changePasswordFormatNote") + "</div>";
	}

	protected void eBtnChangePassword_Click(object sender, EventArgs e) {

		using (var ipa = statics.GetApi()) {
			var cu = CurrentUser;
			if (cu.Password != tye.Data.User.EncryptPassword(tbP.Text, statics.App.GetSetting(SettingsKeys.EncryptionKey))) {
				cu.Password = tye.Data.User.EncryptPassword(tbP.Text, statics.App.GetSetting(SettingsKeys.EncryptionKey));
				cu.MustChangePassword = false;
				CurrentUser = cu;
				ipa.UserSave(cu);
			}

			string redir = (VC.RqHasValue("ru") ? HttpUtility.UrlDecode(VC.RqValue("ru")) : "~/default.aspx");
			Response.Redirect(redir);
		}

	}

}