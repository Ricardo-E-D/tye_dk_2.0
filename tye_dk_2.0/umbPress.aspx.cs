using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbConsole;
using Umbraco.Core;
using monosolutions.Utils;

public partial class umbPress : PageBase {

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
		switch (CurrentLanguage.ID) { 
			case 1: // Danish
				umbPage.UmbracoPageID = 1060;
				break;
			case 2: // Norweigian
				umbPage.UmbracoPageID = 1186;
				break;
			case 3: // English
				umbPage.UmbracoPageID = 1210;
				break;
			case 4: // German
				umbPage.UmbracoPageID = 1234;
				break;
		}
		checkPermissions();
	}

}