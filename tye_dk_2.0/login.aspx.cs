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
		this.Form.DefaultFocus = tbLoginCode.ClientID;
		//fileImports();
		//Response.Write(tye.Data.User.EncryptPassword("zxcvbn", statics.App.GetSetting(SettingsKeys.EncryptionKey)));
	}

	protected void eBtnLogin_Click(object sender, EventArgs e) {
		pnlMessage.CssClass = "errorInline";
		tye.Data.User user = null;
		Language lng = new Language();

		using (var ipa = statics.GetApi()) {
			var code = ipa.ActivationCodeGetSingle(tbLoginCode.Text);
			if (code == null) {
				goto error;
			} else {
				if (code.ClientUserID.HasValue) {
					user = ipa.UserGetSingle(code.ClientUserID.Value);
					if (user != null && (!code.ExpirationDate.HasValue || (code.ExpirationDate.HasValue && code.ExpirationDate.Value > DateTime.Now)))
						goto success;
					else {
						if (code.ExpirationDate.HasValue && code.ExpirationDate.Value <= DateTime.Now) {
							lng = user.Language;
							goto errorexpired;
						} else {
							goto error;
						}
					}
				} else
					goto error;
			}

		errorexpired: {
			pnlMessage.Visible = true;
			var dic = ipa.DictionaryGet();
			pnlMessage.Controls.Add(new LiteralControl(dic.GetValue("codeExpired", lng)));
			return;
		};
		error: {
				pnlMessage.Visible = true;
				pnlMessage.Controls.Add(new LiteralControl("Something's wrong.<br />Maybe it's one of these?<ul><li>Incorrect email</li><li>Incorrect password</li><li>Not a registered user</li><li>Account disabled</li></ul>"));
				return;
			};

		success: {
				var mb = new MasterBase();
	
				mb.CurrentUser = user;

				pnlMessage.Visible = true;
				pnlMessage.CssClass = "successInline";
				pnlMessage.Controls.Add(new LiteralControl("Login successful... <script type=\"text/javascript\">setTimeout(function() { window.location.href = '"
					+ (VC.RqHasValue("ru") ? HttpUtility.UrlDecode(VC.RqValue("ru")) : "default.aspx")
					+ "'; }, 1500);</script>"));
			};
		}
	}

	protected void ElnkEmailLoginSubmit_Click(object sender, EventArgs e) {
		pnlMessage.CssClass = "errorInline";
		tye.Data.User user = null;
		using (var ipa = statics.GetApi()) {

            if (string.IsNullOrEmpty(tbLoginEmail.Text) || string.IsNullOrEmpty(tbLoginPassword.Text)) {
                goto error;
            }

			// "real" email login
			var opts = ipa.UserSearch(/*Enabled && */"Email.ToLower() == @0 && Password == @1", new object[] { tbLoginEmail.Text.ToLower(), tye.Data.User.EncryptPassword(tbLoginPassword.Text, statics.App.GetSetting(SettingsKeys.EncryptionKey)) }.ToList());

			if (opts.Count == 1 && opts.First().Enabled) {
				user = opts.First();
				goto success;
			} else {
				if (String.IsNullOrEmpty(tbLoginEmail.Text)) {
					opts = ipa.UserSearch("Enabled && Password == @0", new object[] { tye.Data.User.EncryptPassword(tbLoginPassword.Text, statics.App.GetSetting(SettingsKeys.EncryptionKey)) }.ToList());
					if (opts.Count == 1 && opts.First().Enabled) {
						user = opts.First();
						goto success;
					} else {
						goto error;
					}
				} else {
					if (opts.Count == 1 && !opts.First().Enabled)
						goto errorDisabled;
					else
						goto error;
				}
			}
		error: {
				pnlMessage.Visible = true;
				pnlMessage.Controls.Add(new LiteralControl("Something's wrong.<br />Maybe it's one of these?<ul><li>Incorrect email</li><li>Incorrect password</li><li>Not a registered user</li><li>Account disabled</li></ul>"));
				return;
			};
		errorDisabled: {
				pnlMessage.Visible = true;
				pnlMessage.Controls.Add(new LiteralControl("Account inactive"));
				return;
			};
		success: {
				var mb = new MasterBase();
				mb.CurrentUser = user;
				//mb.CurrentUser = ipa.UserGetSingle(13531); // sundt syn

				string url = (VC.RqHasValue("ru") ? HttpUtility.UrlDecode(VC.RqValue("ru")) : "default.aspx");
				if (!user.TermsAccepted) {
					url = "/termsAccept.aspx";
				}

				pnlMessage.Visible = true;
				pnlMessage.CssClass = "successInline";
				pnlMessage.Controls.Add(new LiteralControl("Login successful... <script type=\"text/javascript\">setTimeout(function() { window.location.href = '"
					+ url
					+ "'; }, 1500);</script>"));
			};
		}
	}

	protected void eBtnForgotPasswordSubmit_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			var user = ipa
				.UserSearch("Email.ToLower().Equals(@0) && Enabled == true", new object[] { tbLoginEmail.Text.ToLower() }.ToList())
				.FirstOrDefault();

			if (user == null || !monosolutions.Utils.RegExp.IsValidEmail(tbLoginEmail.Text)) {
				pnlMessage.Visible = true;
				pnlMessage.Controls.Add(new LiteralControl("Email was not found."));
				return;
			}

			user.Password = monosolutions.Utils.Strings.CreateRandomString(8, false);
			user.MustChangePassword = true;

			
			string strBody = "Your new password for trainyoueyes.com is: " + user.Password;

			user.Password = tye.Data.User.EncryptPassword(user.Password, statics.App.GetSetting(SettingsKeys.EncryptionKey));
			ipa.UserSave(user);

			statics.SendEmail(user.Email, "New password", strBody);
			//statics.SendEmailDefaultTemplate(user.Email, "New password", "Your new password...", strBody);

			//pnlMessage.Visible = true;
			pnlMessage.Controls.Add(new LiteralControl("Please check your inbox."));
			pnlMessage.CssClass = "successInline info";

			tbLoginEmail.Text = user.Email;
			Form.DefaultFocus = tbLoginEmail.ClientID;
		}
	}

}