using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class clientLog : PageBase {

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Optician,
			tye.Data.User.UserType.Client }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();
	}

}