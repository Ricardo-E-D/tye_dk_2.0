namespace tye.uc.pages {
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.IO;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using exceptions;
	using tye.File_info;

	public partial class admin_key_admin : uc_pages {
		protected TextBox tb = new TextBox();
		protected ListBox lbCountry = new ListBox();
		protected ListBox lbChain = new ListBox();
		protected ListBox lbOptician = new ListBox();
		protected int intStep;
		protected string strMode;
		protected int intCountry;
		protected int intChain;
		protected int intOptician;
		protected Button submit = new Button();
		protected string strLanguage;
		protected string strChain;
		protected string strOptician;
		protected string strAddedTime;
		DateTime datAddedTime = DateTime.Now;

		private Admin currentUser = null;
		private Translation trans = null;
		private string strOpticianForNewlyCreatedCodes = "";


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
		}
		#endregion

		protected void Page_Load(object sender, System.EventArgs e) {
			currentUser = (Admin)Session["user"];

			trans = new Translation(Server.MapPath("uc\\translation.xml"), this.GetType().BaseType.ToString(), Translation.DbLangs[currentUser.IntLanguageId - 1].ToString());

			strMode = Request.QueryString["mode"];
			strAddedTime = Request.QueryString["addedtime"];
			intStep = Convert.ToInt32(Request.QueryString["step"]);
			intCountry = Convert.ToInt32(Request.QueryString["country"]);
			intChain = Convert.ToInt32(Request.QueryString["chain"]);
			intOptician = Convert.ToInt32(Request.QueryString["optician"]);

			if(intCountry < 1)
				intCountry = (currentUser.IsDistributor ? currentUser.IntLanguageId : 1);

			if (IntSubmenuId == 166 || IntSubmenuId == 1232 || IntSubmenuId == 1233 || IntSubmenuId == 1234) {
				switch (strMode) {
					case "details":
						detailsPage();
						break;
					default:
						archivePage();

						if (intCountry != 0) {
							lbChain.Enabled = true;
							lbCountry.SelectedValue = intCountry.ToString();
						}
						if (intChain != 0) {
							lbOptician.Enabled = true;
							lbChain.SelectedValue = intChain.ToString();
						}
						if (intOptician != 0) {
							submit.Enabled = true;
							lbOptician.SelectedValue = intOptician.ToString();
						}
						break;
				}
			} else {
				codeCleanup();
				switch (intStep) {
					case 2:
						drawAddPageStep2();
						break;
					case 3:
						drawAddPageStep3();
						break;
					default:
						drawAddPageStep1();

						if (intCountry != 0) {
							lbChain.Enabled = true;
							lbCountry.SelectedValue = intCountry.ToString();
						}
						if (intChain != 0) {
							lbOptician.Enabled = true;
							lbChain.SelectedValue = intChain.ToString();
						}
						if (intOptician != 0) {
							submit.Enabled = true;
							lbOptician.SelectedValue = intOptician.ToString();
						}
						break;
				}

			}
		}
		private void codeCleanup() {
			try {
				DirectoryInfo dir = new DirectoryInfo(Files.strServerSavePath + "Jh760gdjkLL99");

				foreach (FileInfo fil in dir.GetFiles()) {
					fil.Delete();
				}

				dir = null;
			}
			catch (Exception e) {
				LogInfo.writeLogMessage(e.ToString());
			}
		}

		private void detailsPage() {
			this.Controls.Add(new LiteralControl("<div class='page_subheader'>" + trans.GetString("chosenOptician") + ":</div>"));

			Database db = new Database();
			string strSql = "SELECT name,address,zipcode,city FROM users INNER JOIN user_optician ON userid = users.id WHERE users.id = " + intOptician;
			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read()) {
				this.Controls.Add(new LiteralControl(objDr["name"].ToString() + "<br/>" + objDr["address"].ToString() + "<br/>" + objDr["zipcode"].ToString() + " " + objDr["city"].ToString() + "<br/><br/>"));
			}

			db.objDataReader.Close();
			db = null;

			HtmlTable htList = new HtmlTable();
			htList.CellPadding = 0;
			htList.CellSpacing = 0;
			htList.Style.Add("width", "480px");
			htList.Style.Add("border-collapse", "collapse");
			htList.Attributes["class"] = "data_table";

			HtmlTableRow trList = new HtmlTableRow();
			HtmlTableCell tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width", "110px");
			tcList.Controls.Add(new LiteralControl(trans.GetGeneral("date")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width", "75px");
			tcList.Style.Add("text-align", "center");
			tcList.Controls.Add(new LiteralControl(trans.GetGeneral("numberOfKeys")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width", "75px");
			tcList.Style.Add("text-align", "center");
			tcList.Controls.Add(new LiteralControl(trans.GetGeneral("printed")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width", "110px");
			tcList.Style.Add("text-align", "center");
			tcList.Controls.Add(new LiteralControl(trans.GetGeneral("activate")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width", "110px");
			tcList.Style.Add("text-align", "right");
			tcList.Controls.Add(new LiteralControl(trans.GetGeneral("print")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();
			htList.Controls.Add(trList);

			db = new Database();
			strSql = "SELECT addedtime,isprinted FROM log_keys WHERE opticianid = " + intOptician + " GROUP BY addedtime ORDER BY addedtime DESC;";
			objDr = db.select(strSql);

			while (objDr.Read()) {

				trList = new HtmlTableRow();
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.InnerHtml = Convert.ToDateTime(objDr["addedtime"]).ToString("dd-MM-yyyy");

				trList.Controls.Add(tcList);
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("text-align", "center");

				Database db1 = new Database();
				string strSql1 = "SELECT COUNT(*) AS found FROM log_keys WHERE opticianid = " + intOptician + " AND addedtime = '" + Convert.ToDateTime(objDr["addedtime"]).ToString("yyyy-MM-dd HH:mm:ss") + "';";
				int intCount = Convert.ToInt32(db1.scalar(strSql1));
				db1 = null;

				tcList.InnerHtml = intCount.ToString();

				trList.Controls.Add(tcList);
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("text-align", "center");

				if (Convert.ToInt32(objDr["isprinted"]) == 1) {
					tcList.InnerHtml = trans.GetGeneral("yes");
				} else {
					tcList.InnerHtml = trans.GetGeneral("no");
				}

				trList.Controls.Add(tcList);

				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("text-align", "center");
				tcList.InnerHtml = "<a href='javascript:void(0);' onclick=\"window.open('popups/ActivateKeys.aspx?date=" + Convert.ToDateTime(objDr["addedtime"]).ToString("yyyy-MM-dd_HH:mm:ss") + "&id=" + intOptician + "','actovate','width=300,height=580,toolbar=no,scrollbars=yes,resizeable=no');\">" + trans.GetGeneral("activate") + "</a>";
				trList.Controls.Add(tcList);
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("text-align", "right");
				tcList.InnerHtml = "<a href='javascript:void(0);' onclick=\"window.open('popups/print.aspx?mode=keycards&addedtime=" + Convert.ToDateTime(objDr["addedtime"]).ToString("yyyy-MM-dd_HH:mm:ss") + "&id=" + intOptician + "','print','width=440,height=580,toolbar=no,scrollbars=yes,resizeable=no');\">" + trans.GetGeneral("print") + "</a>";

				trList.Controls.Add(tcList);
				tcList = new HtmlTableCell();
				htList.Controls.Add(trList);
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(htList);
		}

		private void archivePage() {

			this.Controls.Add(new LiteralControl("<div style='width:237px;float:left;height:140px;'>1. " + trans.GetGeneral("language") + ": * <br />"));

			lbCountry.ID = "lbCountry";
			lbCountry.Rows = 10;
			lbCountry.SelectionMode = ListSelectionMode.Single;
			lbCountry.Style.Add("width", "200px");
			lbCountry.Attributes["onchange"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&country='+this.value;";

			Database db = new Database();
			string strSql = "SELECT id,name FROM language WHERE isactive = 1 " + (currentUser.IsDistributor ? " AND id = " + currentUser.IntLanguageId : "") + " ORDER BY name;";

			lbCountry.DataSource = db.select(strSql);
			lbCountry.DataTextField = "name";
			lbCountry.DataValueField = "id";
			lbCountry.DataBind();

			db.objDataReader.Close();

			this.Controls.Add(lbCountry);

			this.Controls.Add(new LiteralControl("</div><div style='width:237px;float:left;height:140px;'>2. " + trans.GetGeneral("chain") + ": * <br/>"));

			lbChain.ID = "lbChain";
			lbChain.Rows = 10;
			lbChain.SelectionMode = ListSelectionMode.Single;
			lbChain.Style.Add("width", "200px");
			lbChain.Enabled = false;
			lbChain.Attributes["onchange"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&country=" + intCountry + "&chain='+this.value;";

			db = new Database();
			strSql = "SELECT optician_chain.id,optician_chain.name FROM" +
					 " optician_chain INNER JOIN" +
					 " optician_code ON optician_chain.id = optician_code.chainid" + 
					 " WHERE languageid = " + intCountry + 
					 " GROUP BY optician_code.chainid ORDER BY optician_chain.name;";

			lbChain.DataSource = db.select(strSql);
			lbChain.DataTextField = "name";
			lbChain.DataValueField = "id";
			lbChain.DataBind();

			db.objDataReader.Close();

			this.Controls.Add(lbChain);

			this.Controls.Add(new LiteralControl("</div><br style='clear:both;' /><br/>3. " + trans.GetGeneral("optician") + ": * <br/>"));

			lbOptician.ID = "lbOptician";
			lbOptician.Rows = 10;
			lbOptician.SelectionMode = ListSelectionMode.Single;
			lbOptician.Style.Add("width", "440px");
			lbOptician.Enabled = false;
			lbOptician.Attributes["onchange"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&country=" + intCountry + "&chain=" + intChain + "&optician='+this.value;";

			db = new Database();
			strSql = "SELECT users.id,CONCAT(user_optician.name,', ',users.address,', ',users.zipcode,' ',users.city) AS stroptician FROM users INNER JOIN user_optician ON users.id = user_optician.userid";
			strSql += " INNER JOIN optician_code ON opticiancodeid = optician_code.id WHERE isactive = 1 AND languageid = " + intCountry + " AND chainid = " + intChain + " ORDER BY user_optician.name;";

			lbOptician.DataSource = db.select(strSql);
			lbOptician.DataTextField = "stroptician";
			lbOptician.DataValueField = "id";
			lbOptician.DataBind();

			db.objDataReader.Close();

			this.Controls.Add(lbOptician);

			submit.ID = "submit";
			submit.Text = trans.GetGeneral("createdKeysForChosenOptician");
			submit.Style.Add("width", "440px");
			submit.Style.Add("margin-top", "20px");
			submit.Enabled = false;
			submit.Click += new EventHandler(goToDetails);

			this.Controls.Add(submit);
		}

		private void drawAddPageStep1() {
			this.Controls.Add(new LiteralControl("<div style='width:237px;float:left;height:140px;'>1. " + trans.GetGeneral("language") + ": * <br />"));

			lbCountry.ID = "lbCountry";
			lbCountry.Rows = 10;
			lbCountry.SelectionMode = ListSelectionMode.Single;
			lbCountry.Style.Add("width", "200px");
			lbCountry.Attributes["onchange"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&country='+this.value;";

			Database db = new Database();
			
			string strSql = "SELECT id, name FROM language WHERE isactive = 1 " + (currentUser.IsDistributor ? " AND id = " + currentUser.IntLanguageId : "") + " ORDER BY name;";

			lbCountry.DataSource = db.select(strSql);
			lbCountry.DataTextField = "name";
			lbCountry.DataValueField = "id";
			lbCountry.DataBind();

			db.objDataReader.Close();

			this.Controls.Add(lbCountry);

			this.Controls.Add(new LiteralControl("</div><div style='width:237px;float:left;height:140px;'>2. " + trans.GetGeneral("chain") + ": * <br/>"));

			lbChain.ID = "lbChain";
			lbChain.Rows = 10;
			lbChain.SelectionMode = ListSelectionMode.Single;
			lbChain.Style.Add("width", "200px");
			lbChain.Enabled = false;
			lbChain.Attributes["onchange"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&country=" + intCountry + "&chain='+this.value;";

			db = new Database();
			strSql = "SELECT optician_chain.id,optician_chain.name FROM optician_chain INNER JOIN optician_code ON optician_chain.id = optician_code.chainid WHERE languageid = " + intCountry + " GROUP BY optician_code.chainid ORDER BY optician_chain.name;";

			lbChain.DataSource = db.select(strSql);
			lbChain.DataTextField = "name";
			lbChain.DataValueField = "id";
			lbChain.DataBind();

			db.objDataReader.Close();

			this.Controls.Add(lbChain);

			this.Controls.Add(new LiteralControl("</div><br style='clear:both;' /><br/>3. " + trans.GetGeneral("optician") + ": * <br/>"));

			lbOptician.ID = "lbOptician";
			lbOptician.Rows = 10;
			lbOptician.SelectionMode = ListSelectionMode.Single;
			lbOptician.Style.Add("width", "440px");
			lbOptician.Enabled = false;
			lbOptician.Attributes["onchange"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&country=" + intCountry + "&chain=" + intChain + "&optician='+this.value;";

			db = new Database();
			strSql = "SELECT users.id,CONCAT(user_optician.name,', ',users.address,', ',users.zipcode,' ',users.city) AS stroptician FROM users INNER JOIN user_optician ON users.id = user_optician.userid";
			strSql += " INNER JOIN optician_code ON opticiancodeid = optician_code.id WHERE isactive = 1 AND languageid = " + intCountry + " AND chainid = " + intChain + " ORDER BY user_optician.name;";

			lbOptician.DataSource = db.select(strSql);
			lbOptician.DataTextField = "stroptician";
			lbOptician.DataValueField = "id";
			lbOptician.DataBind();

			db.objDataReader.Close();

			this.Controls.Add(lbOptician);

			submit.ID = "submit";
			submit.Text = trans.GetGeneral("approveAndProceedToStepTwo");
			submit.Style.Add("width", "440px");
			submit.Style.Add("margin-top", "20px");
			submit.Enabled = false;
			submit.Click += new EventHandler(goToStep2);

			this.Controls.Add(submit);

		}

		private void drawAddPageStep2() {
			Database db = new Database();
			string strSql = "SELECT user_optician.name,address,zipcode,city,email,phone FROM users INNER JOIN user_optician ON users.id = user_optician.userid WHERE users.id = " + intOptician;
			MySqlDataReader objDr = db.select(strSql);

			if (!(objDr.HasRows)) {
				db.objDataReader.Close();
				db.Dispose();
				db = null;
				throw new NoDataFound();
			}

			if (objDr.Read()) {
				string strTmpText;

				strTmpText = trans.GetString("chosenOptician") + ":<br/><br/><div class='bold_text'>" + objDr["name"].ToString() + "</div>" + objDr["address"].ToString();
				strTmpText += "<br/>" + objDr["zipcode"].ToString() + " " + objDr["city"].ToString() + "<br/><a href='mailto:" + objDr["email"].ToString() + "'>" + objDr["email"].ToString() + "</a>";
				strTmpText += "<br/>" + objDr["phone"].ToString() + "<br/><br/>" + trans.GetGeneral("create") + " ";

				this.Controls.Add(new LiteralControl(strTmpText));

				strOpticianForNewlyCreatedCodes = objDr["name"].ToString() + "\n" +
												  objDr["address"].ToString() + "\n" +
												  objDr["zipcode"].ToString() + " " +
												  objDr["city"].ToString() + "\n" +
												  objDr["email"].ToString() + "\n" +
												  objDr["phone"].ToString() + "\n";

				tb.ID = "tbAmount";
				tb.Width = 5;
				tb.Style.Add("width", "40px");

				this.Controls.Add(tb);

				this.Controls.Add(new LiteralControl(" " + trans.GetGeneral("codes") + ". "));

				RangeValidator rv = new RangeValidator();
				rv.MinimumValue = "1";
				rv.MaximumValue = "100";
				rv.ControlToValidate = "tbAmount";
				rv.Type = ValidationDataType.Integer;
				rv.ID = "rv";
				rv.ErrorMessage = trans.GetString("error_numberBetweenOneAndHundred");// "Der skal indtastes et tal mellem 1 og 100.";
				rv.Display = ValidatorDisplay.Dynamic;

				this.Controls.Add(rv);

				RequiredFieldValidator rfv = new RequiredFieldValidator();
				rfv.ControlToValidate = "tbAmount";
				rfv.ID = "rfv";
				rfv.ErrorMessage = trans.GetString("error_numberBetweenOneAndHundred");
				rfv.Display = ValidatorDisplay.Dynamic;

				this.Controls.Add(rfv);

				this.Controls.Add(new LiteralControl("<br/>"));

				submit.ID = "submit";
				submit.Text = trans.GetGeneral("create") + " " + trans.GetGeneral("codes"); //"Opret koder";
				submit.Style.Add("width", "120px");
				submit.Click += new EventHandler(saveKeys);
				submit.Style.Add("margin-top", "20px");

				this.Controls.Add(submit);
			}

			//rettet
			db.objDataReader.Close();
			db = null;
		}

		private void drawAddPageStep3() {
			Database db = new Database();
			string strSql = "SELECT user_optician.name,address,zipcode,city,email,phone,tyeid,optician_chain.name as chainname,optician FROM users INNER JOIN user_optician ON users.id = user_optician.userid ";
			strSql += "INNER JOIN optician_code ON user_optician.opticiancodeid = optician_code.id INNER JOIN optician_chain ON optician_code.chainid = optician_chain.id ";
			strSql += "INNER JOIN language ON languageid = language.id WHERE users.id = " + intOptician;
			MySqlDataReader objDr = db.select(strSql);

			if (!(objDr.HasRows)) {
				db.objDataReader.Close();
				db.Dispose();
				db = null;
				throw new NoDataFound();
			}

			if (objDr.Read()) {
				string strTmpText;

				strTmpText = "<div class='page_subheader'>" + trans.GetString("chosenOptician") + ":</div><br/><div class='bold_text'>" + objDr["name"].ToString() + "</div>" + objDr["address"].ToString();
				strTmpText += "<br/>" + objDr["zipcode"].ToString() + " " + objDr["city"].ToString() + "<br/><a href='mailto:" + objDr["email"].ToString() + "'>" + objDr["email"].ToString() + "</a>";
				strTmpText += "<br/>" + objDr["phone"].ToString() + "<br/><br/><div class='page_subheader'>" + trans.GetGeneral("created") + " " + trans.GetGeneral("codes") + ":</div><br/>";

				this.Controls.Add(new LiteralControl(strTmpText));

				strLanguage = objDr["tyeid"].ToString();
				strChain = objDr["chainname"].ToString();
				strOptician = objDr["optician"].ToString();
			}

			db.objDataReader.Close();

			db = new Database();
			strSql = "SELECT password FROM optician_keys WHERE addedtime = '" + strAddedTime.Replace("%20", " ") + "';";
			objDr = db.select(strSql);

			while (objDr.Read()) {
				this.Controls.Add(new LiteralControl("<span style='width:158px;float:left;'>" + strLanguage + "-" + strChain + "-" + strOptician + "-" + objDr["password"].ToString() + "</span>"));
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(new LiteralControl("<div style='margin-top:20px;clear:both;'><a href='javascript:void(0);' onclick=\"window.open('popups/print.aspx?mode=keycards&addedtime=" + Convert.ToDateTime(strAddedTime.Replace("%20", " ")).ToString("yyyy-MM-dd_HH:mm:ss") + "&id=" + intOptician + "','print','width=400,height=580,toolbar=no,scrollbars=yes,resizeable=no');\">" + trans.GetGeneral("printCodeCard") + "</a>"));
		}

		private void goToStep2(object sender, EventArgs e) {
			Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&step=2&optician=" + intOptician);
		}

		private void goToDetails(object sender, EventArgs E) {
			Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=details&optician=" + intOptician);
		}

		private void saveKeys(object sender, EventArgs e) {
			string strKeysCreated = "";
			if (Page.IsValid) {
				Database db = new Database();
				Admin admin = new Admin();

				for (int i = 0; i < Convert.ToInt32(tb.Text.ToString()); i++) {
					string strKey = admin.generatePassword();
					strKeysCreated += strKey + '\n'; // add to mail variable

					string strSql = "INSERT INTO optician_keys(password,opticianid,addedtime,authorid) VALUES('";
					strSql += strKey + "'," + intOptician + ",'" + datAddedTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + ((Admin)Session["user"]).IntUserId + ");";
					db.execSql(strSql);

					LogKeys lk = new LogKeys(((Admin)Session["user"]).IntUserId, intOptician, strKey, Request.UserHostAddress.ToString(), datAddedTime);
					lk = null;
				}
				db.dbDispose();
				db = null;
				admin = null;

				try {
					if (currentUser.IsDistributor) {
						Email em = new Email();
						em.SenderEmail = "noreply@trainyoureyes.com";
						em.RecipientEmail = Shared.MariaMail;
						em.Subject = "Distributøraktivitet - kodeoprettelse";
						string strBodyText = "Distributør " + ((Admin)Session["user"]).StrName + " har til\n\n" +
							strOpticianForNewlyCreatedCodes + "\n" +
							"oprettet flg. kode(r):\n\n" + strKeysCreated;
						em.Body = strBodyText;
						em.Send();
					}
				}
				catch(Exception) {}
				
				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&step=3&optician=" + intOptician + "&addedtime=" + datAddedTime.ToString("yyyy-MM-dd HH:mm:ss"));
			}
		}
	}
}
