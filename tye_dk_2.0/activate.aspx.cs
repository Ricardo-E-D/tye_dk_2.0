using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class activate : PageBase {
	protected void Page_Init(object sender, EventArgs e) {

		if (CurrentUser.Type != tye.Data.User.UserType.Client) {
			Response.Redirect("default.aspx");
		}

		using (var ipa = statics.GetApi()) {
			var clientCodes = ipa.ActivationCodeGetCollectionByClient(CurrentUser.ID);
			if (!clientCodes.Any()) {
				// let user know that time's up
				// response.redirect("expired.aspx");
				plhExpired.Visible = true;
				plhActivate.Visible = false;
			}
			if (clientCodes.Where(n => n.ActivationDate.HasValue && n.ExpirationDate < DateTime.Now).Any()) {
				// user has an active code
				SessionDataValueSet(SessionDataKeys.ClientCodeIsValid, "true");
			} else if (clientCodes.Where(n => !n.ActivationDate.HasValue).Any()) {
				// user has unused activation code
				// redirect to activation page

				foreach (var code in clientCodes.Where(n => !n.ActivationDate.HasValue)) {
					LinkButton lnkActivate = new LinkButton();
					lnkActivate.CommandArgument = code.ID.ToString();
					lnkActivate.Text = DicValue("codeActivate") + " (" + code.Code + ")<br />";
					lnkActivate.Click += delegate(object sender1, EventArgs e1) {
						using (var ipa1 = statics.GetApi()) {
							int itryCodeID = 0;
							if (int.TryParse(((LinkButton)sender1).CommandArgument, out itryCodeID)) {
								var codeActivate = ipa1.ActivationCodeGetSingle(itryCodeID);
								if (codeActivate != null && codeActivate.ClientUserID == CurrentUser.ID) {
									SessionDataValueSet(SessionDataKeys.ClientCodeIsValid, "true");
									codeActivate.ActivationDate = DateTime.Now;
									codeActivate.ExpirationDate = DateTime.Now.AddMonths(6);
									ipa1.ActivationCodeSave(codeActivate);
									
									Response.Redirect("/");
								}
							}
						}
					};
					plhActivate.Controls.Add(lnkActivate);
				}

			}
		}

	}

	
}