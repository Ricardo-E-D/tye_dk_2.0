namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using exceptions;

	public partial class admin_faq_admin : uc_pages
	{
		protected ListBox language = new ListBox();
		protected TextBox question = new TextBox();
		protected TextBox header = new TextBox();
		protected TextBox answer = new TextBox();
		protected CheckBox access_surfer = new CheckBox();
		protected CheckBox access_optician = new CheckBox();
		protected CheckBox access_client = new CheckBox();

		protected string strMode;
		protected int intId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			strMode = Request.QueryString["mode"];
			intId = Convert.ToInt32(Request.QueryString["id"]);

			try
			{
				switch(IntSubmenuId)
				{
					case 162:
						drawAddPage();
						break;
					case 163:
					switch(strMode)
					{
						case "edit":
							drawEditPage();
							break;
						case "delete":
							delete();
							break;
						default:
							drawArchivePage();
							break;
					}
						break;
				}
			}
			catch(NoDataFound ndf)
			{
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId).ToString()));
			}
		}

		private void delete()
		{
			Database db = new Database();
			string strSql = "DELETE FROM faq WHERE id = " + intId;
			db.execSql(strSql);

			Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId);
		}

		private void drawEditPage()
		{
			if(Session["noerror"] != null)
			{
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
				Session["noerror"] = null;
			}

			this.Controls.Add(new LiteralControl("Sprog: * "));

			RequiredFieldValidator language_val = new RequiredFieldValidator();
			language_val.ID = "language_val";
			language_val.ControlToValidate = "language";
			language_val.ErrorMessage = "Dette felt skal udfyldes.";
			language_val.Display = ValidatorDisplay.Dynamic;
			this.Controls.Add(language_val);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			language.ID = "language";
			language.Rows = 1;

			Database db = new Database();
			string strSql = "SELECT id,name FROM language " + (Shared.UserIsDist() ? "WHERE language.id = " + Shared.UserLang() : "") + " ORDER BY name";

			language.DataSource = db.select(strSql);
			language.DataTextField = "name";
			language.DataValueField = "id";
			language.DataBind();

			if(!Shared.UserIsDist()) {
				ListItem objLi = new ListItem();
				objLi.Text = "Vælg...";
				objLi.Value = "";
				objLi.Selected = true;
				language.Items.Insert(0,objLi);
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(language);

			this.Controls.Add(new LiteralControl("<br/><br/>Overskrift: * "));
	
			RequiredFieldValidator header_val = new RequiredFieldValidator();
			header_val.ID = "header_val";
			header_val.ControlToValidate = "header";
			header_val.ErrorMessage = "Dette felt skal udfyldes.";
			header_val.Display = ValidatorDisplay.Dynamic;
			this.Controls.Add(header_val);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			header.ID = "header";
			header.Width = 475;
			header.Style.Add("width","475px");
			header.TextMode = TextBoxMode.SingleLine;

			this.Controls.Add(header);

			this.Controls.Add(new LiteralControl("<br/><br/>Spørgsmål: * "));
	
			RequiredFieldValidator question_val = new RequiredFieldValidator();
			question_val.ID = "question_val";
			question_val.ControlToValidate = "question";
			question_val.ErrorMessage = "Dette felt skal udfyldes.";
			question_val.Display = ValidatorDisplay.Dynamic;
			this.Controls.Add(question_val);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			question.ID = "question";
			question.Width = 475;
			question.Rows = 10;
			question.Style.Add("width","475px");
			question.Style.Add("height","100px");
			question.TextMode = TextBoxMode.MultiLine;
			

			this.Controls.Add(question);

			this.Controls.Add(new LiteralControl("<br/><br/>Svar: * "));

			RequiredFieldValidator answer_val = new RequiredFieldValidator();
			answer_val.ID = "answer_val";
			answer_val.ControlToValidate = "answer";
			answer_val.ErrorMessage = "Dette felt skal udfyldes.";
			answer_val.Display = ValidatorDisplay.Dynamic;
			this.Controls.Add(answer_val);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			answer.ID = "answer";
			answer.Rows = 10;
			answer.Columns = 200;
			answer.Style.Add("width","475px");
			answer.Style.Add("height","100px");
			answer.TextMode = TextBoxMode.MultiLine;

			this.Controls.Add(answer);

			this.Controls.Add(new LiteralControl("<br/><br/>Tilføjes til: * "));

			this.Controls.Add(new LiteralControl("<br/>"));

			access_surfer.ID = "access_surfer";
			access_surfer.Text = " Surfer";

			this.Controls.Add(access_surfer);

			this.Controls.Add(new LiteralControl("<br/>"));

			access_optician.ID = "access_optician";
			access_optician.Text = " Optiker";

			this.Controls.Add(access_optician);

			this.Controls.Add(new LiteralControl("<br/>"));

			access_client.ID = "access_client";
			access_client.Text = " Slutbruger";

			this.Controls.Add(access_client);

			this.Controls.Add(new LiteralControl("<br/>"));

			Button submit = new Button();
			submit.ID = "submit";
			submit.Text = "Gem FAQ";
			submit.Style.Add("margin-top","20px");
			submit.Style.Add("width","475px");
			submit.Click += new EventHandler(updateFAQ);
			this.Controls.Add(submit);

			db = new Database();
			strSql = "SELECT languageid,question,answer,access_surfer,access_optician,access_client FROM faq WHERE id = " + intId;
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				Wysiwyg wys = new Wysiwyg();

				language.SelectedValue = objDr["languageid"].ToString();
				question.Text = wys.FromDb(1,objDr["question"].ToString());
				answer.Text = wys.FromDb(1,objDr["answer"].ToString());
				if(Convert.ToInt32(objDr["access_surfer"]) == 1)
				{
					access_surfer.Checked = true;
				}
				if(Convert.ToInt32(objDr["access_optician"]) == 1)
				{
					access_optician.Checked = true;
				}
				if(Convert.ToInt32(objDr["access_client"]) == 1)
				{
					access_client.Checked = true;
				}
				wys = null;
			}

			db.objDataReader.Close();
			db = null;
			
		}

		private void drawArchivePage()
		{
			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function deleteQ(id){\n";
			js.Text += "var confirm = window.confirm('Er du sikker på at du vil slette dette spørgsmål?');\n";
			js.Text += "if(confirm){\n";
			js.Text += "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=delete&id='+id;\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			Head_ph.Controls.Add(js);

			Database db = new Database();
			string strSql = "SELECT faq.id,LEFT(question,40) AS question,language.name" + 
				" FROM faq" + 
				" INNER JOIN language ON languageid = language.id" +
				(Shared.UserIsDist() ? " WHERE language.id = " + Shared.UserLang() : "") + " ORDER BY question;";

			DataGrid objDg = new DataGrid();
			objDg.ID = "data_table";
			objDg.AutoGenerateColumns = false;
			objDg.DataSource = db.select(strSql);
			objDg.CellPadding = 0;
			objDg.CellSpacing = 0;
			objDg.BorderWidth = 1;
			objDg.Width = 480;
			objDg.CssClass = "data_table";
			objDg.GridLines = GridLines.None;

			BoundColumn objBc = new BoundColumn();

			objBc.DataField = "question";
			objBc.HeaderText = "Spørgsmål";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 345;

			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "name";
			objBc.HeaderText = "Sprog";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 70;

			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();

			objHlc.DataNavigateUrlField = "id";
			objHlc.HeaderText = "Ret";
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";
			objHlc.HeaderStyle.Width = 30;
			objHlc.DataNavigateUrlFormatString = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=edit&id={0}";
			objHlc.Text = "Ret";

			objDg.Columns.Add(objHlc);
		
			objHlc = new HyperLinkColumn();

			objHlc.DataNavigateUrlField = "id";
			objHlc.HeaderText = "Slet";
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";
			objHlc.HeaderStyle.Width = 30;
			objHlc.DataNavigateUrlFormatString = "javascript:deleteQ({0});";
			objHlc.Text = "Slet";
			objDg.Columns.Add(objHlc);

			objDg.DataBind();

			if(!(db.objDataReader.HasRows))
			{
				throw new NoDataFound();
			}

			this.Controls.Add(objDg);

			db.objDataReader.Close();
			db = null;
		}

		private void drawAddPage()
		{
			if(Session["noerror"] != null)
			{
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
				Session["noerror"] = null;
			}

			this.Controls.Add(new LiteralControl("Sprog: * "));

			RequiredFieldValidator language_val = new RequiredFieldValidator();
			language_val.ID = "language_val";
			language_val.ControlToValidate = "language";
			language_val.ErrorMessage = "Dette felt skal udfyldes.";
			language_val.Display = ValidatorDisplay.Dynamic;
			this.Controls.Add(language_val);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			language.ID = "language";
			language.Rows = 1;

			Database db = new Database();
			string strSql = "SELECT id,name FROM language" + (Shared.UserIsDist() ? " WHERE language.id = " + Shared.UserLang() : "") + " ORDER BY name";

			language.DataSource = db.select(strSql);
			language.DataTextField = "name";
			language.DataValueField = "id";
			language.DataBind();

			if(!Shared.UserIsDist()) {
				ListItem objLi = new ListItem();
				objLi.Text = "Vælg...";
				objLi.Value = "";
				objLi.Selected = true;
				language.Items.Insert(0,objLi);
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(language);

			this.Controls.Add(new LiteralControl("<br/><br/>Overskrift: * "));
	
			RequiredFieldValidator header_val = new RequiredFieldValidator();
			header_val.ID = "header_val";
			header_val.ControlToValidate = "header";
			header_val.ErrorMessage = "Dette felt skal udfyldes.";
			header_val.Display = ValidatorDisplay.Dynamic;
			this.Controls.Add(header_val);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			header.ID = "header";
			header.Width = 475;
			header.Style.Add("width","475px");
			header.TextMode = TextBoxMode.SingleLine;

			this.Controls.Add(header);

			this.Controls.Add(new LiteralControl("<br/><br/>Spørgsmål: * "));
	
			RequiredFieldValidator question_val = new RequiredFieldValidator();
			question_val.ID = "question_val";
			question_val.ControlToValidate = "question";
			question_val.ErrorMessage = "Dette felt skal udfyldes.";
			question_val.Display = ValidatorDisplay.Dynamic;
			this.Controls.Add(question_val);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			question.ID = "question";
			question.Width = 475;
			question.Rows = 10;
			question.Style.Add("width","475px");
			question.Style.Add("height","100px");
			question.TextMode = TextBoxMode.MultiLine;

			this.Controls.Add(question);

			this.Controls.Add(new LiteralControl("<br/><br/>Svar: * "));

			RequiredFieldValidator answer_val = new RequiredFieldValidator();
			answer_val.ID = "answer_val";
			answer_val.ControlToValidate = "answer";
			answer_val.ErrorMessage = "Dette felt skal udfyldes.";
			answer_val.Display = ValidatorDisplay.Dynamic;
			this.Controls.Add(answer_val);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			answer.ID = "answer";
			answer.Rows = 10;
			answer.Columns = 200;
			answer.Style.Add("width","475px");
			answer.Style.Add("height","100px");
			answer.TextMode = TextBoxMode.MultiLine;

			this.Controls.Add(answer);

			this.Controls.Add(new LiteralControl("<br/><br/>Tilføjes til: * "));

			this.Controls.Add(new LiteralControl("<br/>"));

			access_surfer.ID = "access_surfer";
			access_surfer.Text = " Surfer";

			this.Controls.Add(access_surfer);

			this.Controls.Add(new LiteralControl("<br/>"));

			access_optician.ID = "access_optician";
			access_optician.Text = " Optiker";

			this.Controls.Add(access_optician);

			this.Controls.Add(new LiteralControl("<br/>"));

			access_client.ID = "access_client";
			access_client.Text = " Slutbruger";

			this.Controls.Add(access_client);

			this.Controls.Add(new LiteralControl("<br/>"));

			Button submit = new Button();
			submit.ID = "submit";
			submit.Text = "Gem FAQ";
			submit.Style.Add("margin-top","20px");
			submit.Style.Add("width","475px");
			submit.Click += new EventHandler(saveFAQ);
			this.Controls.Add(submit);
		}

		private void saveFAQ(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				Wysiwyg wys = new Wysiwyg();

				int intAccessS = 0;
				int intAccessO = 0;
				int intAccessC = 0;

				if(access_surfer.Checked == true)
				{
					intAccessS = 1;
				}

				if(access_optician.Checked == true)
				{
					intAccessO = 1;
				}

				if(access_client.Checked == true)
				{
					intAccessC = 1;
				}

				Database db = new Database();
				string strSql = "INSERT INTO faq (languageid,addedtime,author,question,answer,access_surfer,access_optician,access_client,header) VALUES(";
				strSql += Convert.ToInt32(language.SelectedValue) + ",CURRENT_TIMESTAMP()," + ((Admin)Session["user"]).IntUserId + ",'" + wys.ToDb(1,question.Text) + "','" + wys.ToDb(1,answer.Text) + "',";
				strSql += intAccessS + "," + intAccessO + "," + intAccessC + ",'"+ wys.ToDb(2,header.Text) +"');";
				db.execSql(strSql);

				db = null;
				wys = null;

				Session["noerror"] = "<div id='noerror'>Spørgsmålet er nu gemt.</div>";

				Response.Redirect("?page=" + IntPageId);
			}
		}

		private void updateFAQ(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				Wysiwyg wys = new Wysiwyg();

				int intAccessS = 0;
				int intAccessO = 0;
				int intAccessC = 0;

				if(access_surfer.Checked == true)
				{
					intAccessS = 1;
				}

				if(access_optician.Checked == true)
				{
					intAccessO = 1;
				}

				if(access_client.Checked == true)
				{
					intAccessC = 1;
				}

				Database db = new Database();
				string strSql = "UPDATE faq SET languageid = " + Convert.ToInt32(language.SelectedValue) + ",addedtime = CURRENT_TIMESTAMP(),author = " + ((Admin)Session["user"]).IntUserId + ",question = '" + wys.ToDb(1,question.Text) + "'";
				strSql += ",answer = '" + wys.ToDb(1,answer.Text) + "',access_surfer = " + intAccessS + ",access_optician = " + intAccessO + ",access_client = " + intAccessC + ",header = '"+wys.ToDb(2,header.Text) + "'";
				strSql += " WHERE id = " + intId;
				db.execSql(strSql);

				db = null;
				wys = null;

				Session["noerror"] = "<div id='noerror'>Spørgsmålet er nu opdateret.</div>";

				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=edit&id=" + intId);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
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
		private void InitializeComponent()
		{
		}
		#endregion
	}
}
