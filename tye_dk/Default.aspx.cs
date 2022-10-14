using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;
using tye.exceptions;
using System.Globalization;

namespace tye {
	public partial class _Default : System.Web.UI.Page {
		protected TextBox login;
		protected Button login_btn;
		protected TextBox login_tb;
		protected TextBox h4xbox;
		protected PlaceHolder error_ph;
		protected HtmlGenericControl page_header = new HtmlGenericControl("div");

		private int intPageId;
		private int intSubmenuId;

		protected void Page_Load(object sender, System.EventArgs e) {
			Files.strServerSavePath = Request.PhysicalApplicationPath + "upload\\";

			if (Session["menu"] == null) {
				Menu objMenu = new Menu();
				objMenu.IntLanguageId = 1;
				objMenu.StrAccess = "access_surfer";
				Session["menu"] = objMenu;
				objMenu = null;
				evalLanguage();
			}
			try {
				//if (Request.Browser.EcmaScriptVersion.Major < 1) {
				//   throw new NoJs();
				//}

				if (Session["login"] == null) {
					Login objLogin = new Login();

					objLogin.BlnSucces = false;
					objLogin.IntUserType = 1;
					objLogin.StrIp = Request.UserHostAddress.ToString();

					Session["login"] = objLogin;

					objLogin = null;
				}

				if (((Login)Session["login"]).BlnSucces == true) {
					Button logout_btn = new Button();

					logout_btn.ID = "logout_btn";
					logout_btn.Text = "LOGOUT";
					logout_btn.Click += new EventHandler(doLogOut);
					logout_btn.CausesValidation = false;

					login_div.Controls.Add(logout_btn);
				} else {
					main_body.Attributes["onload"] = "main_form.login_tb.focus();";

					Button login_btn = new Button();

					login_btn.ID = "login_btn";
					login_btn.Text = "LOGIN";
					login_btn.Click += new EventHandler(doLogin);

					login_div.Controls.Add(login_btn);

					Label login_arrows = new Label();

					login_arrows.ID = "login_arrows";
					login_arrows.Text = "» ";

					login_div.Controls.Add(login_arrows);
					//lefttop_div.Controls.Add(login_arrows);

					TextBox login_tb = new TextBox();

					login_tb.ID = "login_tb";
					login_tb.MaxLength = 10;
					login_tb.TextMode = TextBoxMode.Password;

					lefttop_div.Controls.Add(login_tb);

					TextBox h4xbox = new TextBox();

					h4xbox.ID = "h4xbox";
					h4xbox.Enabled = false;

					login_div.Controls.Add(h4xbox);
				}

				intSubmenuId = Convert.ToInt32(Request.QueryString["submenu"]);
				intPageId = Convert.ToInt32(Request.QueryString["page"]);

				// Redir til redigeringsside
				//if (intPageId == 1210) { intPageId = 1208; }
				if (intPageId == 1210) {
					switch (intSubmenuId) {
						case 1208:
							Response.Redirect("Admin_Exercises.aspx?page=1210&submenu=1208");
							break;
						case 1209:
							Response.Redirect("Admin_Equipment.aspx?page=1210&submenu=1209");
							break;
					}
				}

				intPageId = ((Menu)Session["menu"]).getFirstId(intPageId);

				if (!(intSubmenuId > 0)) {
					intSubmenuId = ((Menu)Session["menu"]).getFirstSubmenuId(intPageId);
				}

				// tilføj flag til sprogvalg til siden (hvis man ikke er logget ind)
				if (((Menu)Session["menu"]).StrAccess == "access_surfer") {
					DrawFlags();
				}

				((Menu)Session["menu"]).FillMenu(intPageId, menu_td);
				((Menu)Session["menu"]).FillSubmenu(intPageId, intSubmenuId, submenu_td);

				bool isFound = false;

				if (intPageId == 1) {
					isFound = true;
				} else {
					for (int i = 0; i < ((Menu)Session["menu"]).ArrPageAccess.Length; i++) {
						int id = ((Menu)Session["menu"]).ArrPageAccess[i];
						if (intPageId == ((Menu)Session["menu"]).ArrPageAccess[i]) {
							isFound = true;
							break;
						}
					}
				}
				if (isFound == false) {
					throw new NoAccess();
				}

				//Tilføjer nyhedstd
				news_td.Controls.Add(new tye.uc.pages.news().getLastNews(((Menu)Session["menu"]).IntLanguageId));

				//Tilføjer indhold til siden
				Database db = new Database();

				string strSql;

				if (intPageId > 0 && intSubmenuId == 0) {
					strSql = "SELECT usercontrols.name, menu.name AS pagename" +
							 " FROM menu" +
							 " INNER JOIN usercontrols ON menu.usercontrolid = usercontrols.id" +
							 " WHERE menu.id = " + intPageId + ";";
				} else {
					strSql = "SELECT usercontrols.name, menu.name AS pagename" +
							 " FROM menu" +
							 " INNER JOIN usercontrols ON menu.usercontrolid = usercontrols.id" +
							 " WHERE menu.id = " + intSubmenuId + ";";
				}

				MySqlDataReader objDr = db.select(strSql);

				if (objDr.Read() == true) {
					page_header.ID = "page_header";
					page_header.InnerHtml = objDr["pagename"].ToString();
					content_div.Controls.Add(page_header);
					IncludeUc(objDr["name"].ToString());
					string strTempName = objDr["name"].ToString();
					//Response.Write(strTempName);
				} else {
					db.objDataReader.Close();
					db = null;
					throw new FileNotFound();
				}

				db.objDataReader.Close();
				db = null;

				//Tilføjer banner til sitet
				getBanner();
			} catch (FileNotFound fnf) {
				Label error_lbl = new Label();

				error_lbl.Text = fnf.Message(((Menu)Session["menu"]).IntLanguageId);
				error_lbl.ID = "error_lbl";

				//content_div.Controls.Clear();
				content_div.Controls.AddAt(0, error_lbl);
			} catch (NoAccess na) {
				Label error_lbl = new Label();

				error_lbl.Text = na.Message(((Menu)Session["menu"]).IntLanguageId);
				error_lbl.ID = "error_lbl";

				content_div.Controls.Clear();
				content_div.Controls.Add(error_lbl);
			}
			// check if session tests is something (a test has been initiated)
			if (Session["tests"] != null && !IsPostBack) {
				Tests T = (Tests)Session["tests"];
				if (!T.IsScreenExercise) { // if this i not a screen test (ei: a text test)
					if (T.DblSeconds < 1) { // if no time has been recorded
						TimeSpan ts = DateTime.Now.Subtract(T.DatStarttime);
						T.DblSeconds = (ts.Minutes * 60) + ts.Seconds; // calc time spent
						T.saveLog(); // and save log
					}
				}
				Session["tests"] = null; // nullify session object to avoid further logging
			}
		}

		private void getBanner() {
			string strSql = "SELECT banner.id,path,website,alt FROM banner INNER JOIN banner_language ON banner.id = banner_language.bannerid WHERE languageid = " + ((Menu)Session["menu"]).IntLanguageId + " AND isactive = 1";
			Database db = new Database();
			MySqlDataReader objDr = db.select(strSql);

			while (objDr.Read()) {
				HtmlImage banner = new HtmlImage();
				banner.ID = "banner_" + objDr["id"];
				banner.Src = Files.strServerFilePath + objDr["path"].ToString();
				banner.Alt = objDr["alt"].ToString();
				banner.Attributes["title"] = objDr["alt"].ToString();
				banner.Attributes["onclick"] = "window.open('" + objDr["website"].ToString() + "');";
				banner.Attributes["onmouseover"] = "this.style.cursor = 'hand';";

				banner_td.Controls.Add(banner);

				banner_td.Controls.Add(new LiteralControl("<br/><br/>"));
			}

			db.objDataReader.Close();
			db = null;
		}

		private void DrawFlags() {
			Database db = new Database();
			string strSql = "SELECT id,name,imgpath,imgalt FROM language WHERE isactive = 1 ORDER BY id;";
			MySqlDataReader objDr = db.select(strSql);
			while (objDr.Read() == true) {
				ImageButton ib = new ImageButton();

				ib.CssClass = "language_btn";
				ib.ID = objDr["name"] + "_flag";
				ib.ImageUrl = "gfx/" + objDr["imgpath"];
				ib.AlternateText = objDr["imgalt"].ToString();
				string test = "language_" + objDr["name"];

				switch (Convert.ToInt32(objDr["id"])) {
					case 1:
						ib.Click += new ImageClickEventHandler(languageDk);
						break;
					case 2:
						ib.Click += new ImageClickEventHandler(languageN);
						break;
					case 3:
						ib.Click += new ImageClickEventHandler(languageUk);
						break;
					case 4:
						ib.Click += new ImageClickEventHandler(languageDe);
						break;
				}

				language_div.Controls.Add(ib);
				language_div.Controls.Add(new LiteralControl("&nbsp;"));
			}
			db.objDataReader.Close();
			db = null;

			ImageButton ibAus = new ImageButton();
			ibAus.CssClass = "language_btn";
			ibAus.ID = "aus_flag";
			ibAus.ImageUrl = "gfx/flag_australia.gif";
			ibAus.Click += new ImageClickEventHandler(languageUk);
			language_div.Controls.AddAt(6, ibAus);
			language_div.Controls.AddAt(7, new LiteralControl("&nbsp;"));

			ImageButton ibBrit = new ImageButton();
			ibBrit.CssClass = "language_btn";
			ibBrit.ID = "brit_flag";
			ibBrit.ImageUrl = "gfx/flag_british.gif";
			ibBrit.Click += new ImageClickEventHandler(languageUk);
			language_div.Controls.AddAt(8, ibBrit);
			language_div.Controls.AddAt(9, new LiteralControl("&nbsp;"));

			ImageButton ibAustria = new ImageButton();
			ibAustria.CssClass = "language_btn";
			ibAustria.ID = "austria_flag";
			ibAustria.ImageUrl = "gfx/flag_austria.gif";
			ibAustria.Click += new ImageClickEventHandler(languageDe);
			language_div.Controls.AddAt(12, ibAustria);
			language_div.Controls.AddAt(13, new LiteralControl("&nbsp;"));

			ImageButton ibSchwitzerland = new ImageButton();
			ibSchwitzerland.CssClass = "language_btn";
			ibSchwitzerland.ID = "switcherland_flag";
			ibSchwitzerland.ImageUrl = "gfx/flag_switcherland.gif";
			ibSchwitzerland.Click += new ImageClickEventHandler(languageDe);
			language_div.Controls.AddAt(14, ibSchwitzerland);
			language_div.Controls.AddAt(15, new LiteralControl("&nbsp;"));
		}

		protected void btnDummyToPostBack_Click(object sender, EventArgs e) {
			// save log of text exercise from modal popup window
			if (Session["tests"] != null) {
				Tests T = (Tests)Session["tests"];
				if (T.IsScreenExercise) {
					TimeSpan ts = DateTime.Now.Subtract(T.DatStarttime);
					T.DblSeconds = (ts.Minutes * 60) + ts.Seconds;
					T.saveLog();
				}
				Session["tests"] = null;
			}
		}

		/// <summary>
		/// Returns the postback options
		/// </summary>
		/// <returns>PostBackOptions object</returns>
		private PostBackOptions GetbtnDummyPostOptions() {
			return new PostBackOptions(this.btnDummyToPostBack);
		}

		/// <summary>
		/// Overridden Render method solely to do registration for event validation
		/// </summary>
		/// <param name="writer">HtmlTextWriter object</param>
		protected override void Render(HtmlTextWriter writer) {
			PostBackOptions options = GetbtnDummyPostOptions();
			if (options != null) {
				options.PerformValidation = true;
				Page.ClientScript.RegisterForEventValidation(options);
				litPostBack.Text = Page.ClientScript.GetPostBackEventReference(options);
			}
			base.Render(writer);
		}

		// Eventhandlers

		public void doLogin(object Sender, System.EventArgs E) {
			((Login)Session["login"]).StrPassword = Request.Form["login_tb"].Replace("'", "");
			((Login)Session["login"]).StrPassword = ((Login)Session["login"]).StrPassword.Trim();

			LogLogin objLl = new LogLogin();
			objLl.StrIp = Request.UserHostAddress;
			objLl.StrPassword = ((Login)Session["login"]).StrPassword;

			try {
				((Login)Session["login"]).ValidateLogin();
				objLl.Insert(objLl);

				Response.Redirect(Request.RawUrl);
			} catch (WrongPassword wp) { //show an error message saying that the account is locked
				Label error_lbl = new Label();

				error_lbl.Text = wp.Message(((Menu)Session["menu"]).IntLanguageId);
				error_lbl.ID = "error_lbl";

				content_div.Controls.Clear();
				content_div.Controls.Add(error_lbl);

				objLl.Insert(objLl);
			} catch (LicenceExpired le) { //show an error message saying that the account is locked
				Label error_lbl = new Label();

				error_lbl.Text = le.Message(((Menu)Session["menu"]).IntLanguageId);
				error_lbl.ID = "error_lbl";

				content_div.Controls.Clear();
				content_div.Controls.Add(error_lbl);

				objLl.Insert(objLl);
				Session.Abandon();
			}

			objLl = null;
		}

		public void doLogOut(object Sender, System.EventArgs E) {
			Session.Clear();
			Session.Abandon();
			Response.Redirect("Default.aspx");
		}

		private void IncludeUc(string strFileName) {
			UserControl uc = (UserControl)LoadControl("uc/pages/" + strFileName + ".ascx");
			if (Shared.debug) {
				Literal l = new Literal();
				l.Text = "Current usercontrol: '" + strFileName + ".ascx'<br /><br />";
				content_div.Controls.Add(l);
			}
			content_div.Controls.Add(uc);
			uc.GetType().GetProperty("Head_ph").SetValue(uc, head_ph, null);
			uc.GetType().GetProperty("IntPageId").SetValue(uc, intPageId, null);
			uc.GetType().GetProperty("IntSubmenuId").SetValue(uc, intSubmenuId, null);
			uc.GetType().GetProperty("Page_header").SetValue(uc, page_header, null);
			uc.GetType().GetProperty("Page_body").SetValue(uc, main_body, null);
			uc.GetType().GetProperty("Main_form").SetValue(uc, main_form, null);
		}

		public void SetLanguage(int intLanguageId) {
			((Menu)Session["menu"]).IntLanguageId = intLanguageId;
			if (((User)Session["user"]) != null) {
				((User)Session["user"]).IntLanguageId = intLanguageId;
			}
			if (((Optician)Session["user"]) != null) {
				((Optician)Session["user"]).IntLanguageId = intLanguageId;
			}

			Response.Redirect(".");
		}

		public void languageDk(object Sender, System.Web.UI.ImageClickEventArgs E) {
			SetLanguage(1);
		}

		public void languageN(object Sender, System.Web.UI.ImageClickEventArgs E) {
			SetLanguage(2);
		}

		public void languageUk(object Sender, System.Web.UI.ImageClickEventArgs E) {
			SetLanguage(3);
		}
		public void languageDe(object Sender, System.Web.UI.ImageClickEventArgs E) {
			SetLanguage(4);
		}
		// jb tysk slut		

		private void evalLanguage() {
			if (Session["languageChecked"] != null)
				return;
			Session["languageChecked"] = "yup";

			// else - try to read browser value and map accordingly
			if (Request.UserLanguages != null) {
				if (Request.UserLanguages.Length > 0) {
					string ul = Request.UserLanguages[0].ToLower();
					if (ul.StartsWith("da"))
						SetLanguage(1);
					if (ul.StartsWith("en"))
						SetLanguage(3);
					if (ul.StartsWith("de"))
						SetLanguage(4);
					if (ul.StartsWith("nb") || ul.StartsWith("nn") || ul.StartsWith("no"))
						SetLanguage(2);
				} // if
			} // if
		} // method

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() { }
		#endregion
	}
}
