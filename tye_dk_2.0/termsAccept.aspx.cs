// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using tye.API;

public partial class termsAccept : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

		if (CurrentUser.TermsAccepted) {
			Response.Redirect("/default.aspx");
		}
    }


	protected void btnTermsAccept_Click(object sender, EventArgs e)
	{
		using (var ipa = statics.GetApi()) { 
			var user = ipa.UserGetSingle(CurrentUser.ID);
			if (user != null) {
				user.TermsAccepted = true;
				ipa.UserSave(user);
				CurrentUser.TermsAccepted = true;

			}
		}
		Response.Redirect("/default.aspx");
	}

}