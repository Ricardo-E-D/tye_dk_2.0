// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;
using tye.Data;
using System.Web.Hosting;

public partial class login : System.Web.UI.Page
{
	protected void Page_Init(object sender, EventArgs e) {
	}

	protected void ElnkEmailLoginSubmit_Click(object sender, EventArgs e) {
		pnlMessage.CssClass = "errorInline";
		tye.Data.User user = null;
		using (var ipa = statics.GetApi()) {

			if (String.IsNullOrEmpty(tbOldPwd.Text.Trim()))
				goto error;

			var users = ipa.UserGetCollection();
			var u = users.FirstOrDefault(n => n.OldPassword == tbOldPwd.Text);

			if (u == null || !String.IsNullOrEmpty(u.Email) || !RegExp.IsValidEmail(tbEmail.Text.Trim())) {
				goto error;
			} else {
				u.Email = tbEmail.Text.Trim();
				ipa.UserSave(u);
				goto success;
			}

		error: {
				pnlMessage.Visible = true;
				pnlMessage.Controls.Add(new LiteralControl("Old password not found or email found."));
				return;
			};

		success: {
				var mb = new MasterBase();
				mb.CurrentUser = user;
				//mb.CurrentUser = ipa.UserGetSingle(13531); // sundt syn

				pnlMessage.Visible = true;
				pnlMessage.CssClass = "successInline";
				pnlMessage.Controls.Add(new LiteralControl("Change successful. You may now request a new password from the <a href=\"login.aspx\">login page</a>."));
			};
		}
	}

}