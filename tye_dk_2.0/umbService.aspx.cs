using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbConsole;
using Umbraco.Core;
using monosolutions.Utils;

public partial class umbService : PageBase {

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.SBA, 
			tye.Data.User.UserType.Administrator, 
			tye.Data.User.UserType.Distributor, 
			tye.Data.User.UserType.Optician }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();

		switch (VC.RqValue("lang")) { 
			case "DE":
				umbPage.UmbracoPageID = 1714;
				break;
			case "GB":
				umbPage.UmbracoPageID = 1715;
				break;
			case "NO":
				umbPage.UmbracoPageID = 1716;
				break;
		}
	}

}