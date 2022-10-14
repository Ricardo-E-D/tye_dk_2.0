// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class master : MasterBase {
	protected void Page_Init(object sender, EventArgs e) {
		var cu = CurrentUser; // eval redir to login if required

		litUserInfo.Text = cu.FullName;

		if (Impersonating && (cu.ID == CurrentBaseUser.ID))
			Impersonating = false;

		if (Impersonating) {
			if ((CurrentBaseUser.Type == tye.Data.User.UserType.SBA || CurrentBaseUser.Type == tye.Data.User.UserType.Administrator)) {
				lnkStopImpersonating.Visible = true;
			} else if (CurrentBaseUser.Type == tye.Data.User.UserType.Optician && CurrentUser.Type == tye.Data.User.UserType.Client) {
				lnkStopImpersonating.Visible = true;
			}
		}

		string pressurl = "";
		string serviceurl = "";
		var langID = CurrentLanguage.ID;
		switch (langID) { 
			case 1: //DK
				pressurl = "http://tye.dk/presse/artikler.aspx";
				serviceurl = "/umbService.aspx?lang=DK";
				break;
			case 4: //DE
				pressurl = "http://tye.dk/de/presse/artikel.aspx";
				serviceurl = "/umbService.aspx?lang=DE";
				break;
			case 3: //UK
				pressurl = "http://tye.dk/uk/press/articles.aspx";
				serviceurl = "/umbService.aspx?lang=GB";
				break;
			case 2: //NO
				pressurl = "http://www.tye.dk/no/presse/artikler.aspx";
				serviceurl = "/umbService.aspx?lang=NO";
				break;
		}
		lnkToPressAdmin.NavigateUrl = lnkToPressDistributor.NavigateUrl = lnkToPressOptician.NavigateUrl = pressurl;
		lnkMenuToServiceAdmin.NavigateUrl = lnkMenuToServiceDist.NavigateUrl = lnkMenuToServiceOptician.NavigateUrl = serviceurl;

		switch (cu.Type) { 
			case tye.Data.User.UserType.Distributor:
				plhMenuAdmin.Visible = false;
				plhMenuOptician.Visible = false;
				plhMenuClient.Visible = false;
				plhMenuDistributor.Visible = true;
				break;
			case tye.Data.User.UserType.Optician:
				plhMenuAdmin.Visible = false;
				plhMenuDistributor.Visible = false;
				plhMenuClient.Visible = false;
				plhMenuOptician.Visible = true;
				break;
			case tye.Data.User.UserType.Client:
				plhMenuAdmin.Visible = false;
				plhMenuOptician.Visible = false;
				plhMenuDistributor.Visible = false;
				plhMenuClient.Visible = true;

				string codeValid = SessionDataValueGet(SessionDataKeys.ClientCodeIsValid);
				if (String.IsNullOrEmpty(codeValid)) {
					string url = HttpContext.Current.Request.RawUrl.ToString().ToLower();
					using (var ipa = statics.GetApi()) {
						var clientCodes = ipa.ActivationCodeGetCollectionByClient(CurrentUser.ID);
						if(!clientCodes.Any()) {
							// let user know that time's up
							// response.redirect("expired.aspx");
							// ...which is done in activate.aspx
							if (!url.Contains("activate.aspx"))
								Response.Redirect("activate.aspx");
						}
						if (clientCodes.Where(n => n.ActivationDate.HasValue && n.ExpirationDate < DateTime.Now).Any()) { 
							// user has an active code
							codeValid = "true";
							SessionDataValueSet(SessionDataKeys.ClientCodeIsValid, "true");
						} else if (clientCodes.Where(n => !n.ActivationDate.HasValue).Any()) {
							// user has unused activation code
							// redirect to activation page
							if(!url.Contains("activate.aspx"))
								Response.Redirect("activate.aspx");
						}
					}
				}
				if (codeValid != "true") {
					SessionDataValueSet(SessionDataKeys.ClientCodeIsValid, "false");
				}
				break;
		}
	}

	protected void eLnkLogout_Click(object sender, EventArgs e) {
		Session.Abandon();
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	protected void eLnkStopImpersonating_Click(object sender, EventArgs e) {
		Impersonating = false;
		var cu = CurrentUser;
		cu.ImpersonatingUser = null;
		CurrentUser = cu;
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}
}
