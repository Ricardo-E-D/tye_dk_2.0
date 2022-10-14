using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class activationCodeReset : PageBase {

	int OpticianUserID = 0;
	
	PropertyMapper PM = null;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA, 
			tye.Data.User.UserType.Administrator }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");

	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
		PM = new PropertyMapper(new object());

		int.TryParse(VC.RqValue("OpticianUserID"), out OpticianUserID);

		using (var ipa = statics.GetApi()) {
			var codes = ipa.ActivationCodeGetCollection(OpticianUserID);

			foreach (var code in codes) { 
			
				string text = "Kode: " + code.Code;
				if(code.ClientUserID.HasValue) {
					var user = ipa.UserGetSingle(code.ClientUserID.Value);
					if(user != null) {
						text += ". Anvendes af klient: " + user.FullName;
					}
				}
				pnlCodes.Controls.Add(new LiteralControl(text + "<br />"));

				LinkButton lnkResetCode = new LinkButton();
				lnkResetCode.ID = "re" + code.ID;
				lnkResetCode.Text = "Nulstil " + code.Code;
				lnkResetCode.OnClientClick = "return confirm('Sikker?');";
				lnkResetCode.CommandArgument = code.ID.ToString();
				lnkResetCode.Click += new EventHandler(lnkResetCode_Click);
				pnlCodes.Controls.Add(lnkResetCode);

				pnlCodes.Controls.Add(new LiteralControl("<br /><br />"));
			}
		}
	}

	void lnkResetCode_Click(object sender, EventArgs e) {
		int iTry = 0;

		if (!int.TryParse(((LinkButton)sender).CommandArgument.ToString(), out iTry))
			Response.Redirect("activationCodeReset.aspx");

		using (var ipa = statics.GetApi()) {
			var code = ipa.ActivationCodeGetSingle(iTry);
			if (code.ClientUserID.HasValue) {
				var user = ipa.UserGetSingle(code.ClientUserID.Value);
				if (user != null) {
					ipa.UserDeletePermanently(user.ID);
				}
				code.ClientUserID = null;
				code.ActivationDate = null;
				code.ExpirationDate = null;
				ipa.ActivationCodeSave(code);
			}
		}
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	private void redir() {
		Response.Redirect(VC.QueryStringStripNoTrail("id"));
	}
}