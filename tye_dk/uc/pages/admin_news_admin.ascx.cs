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

	public partial class admin_news_admin : uc_pages
	{
		protected TextBox tbHeader = new TextBox();
		protected TextBox tbBody = new TextBox();
		protected ListBox lbLanguage = new ListBox();
		protected CheckBox cbSticky = new CheckBox();
		protected int intId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string strMode = Request.QueryString["mode"];
			intId = Convert.ToInt32(Request.QueryString["id"]);

			// dk, tysk, engelsk, svensk
			if (IntSubmenuId == 87 || IntSubmenuId == 1167) {
				addNews();
			}
			if (IntSubmenuId == 88 || IntSubmenuId == 1168 || IntSubmenuId == 1166) {
				switch (strMode) {
					case "edit":
						editNews();
						break;
					case "delete":
						deleteNews();
						break;
					default:
						listNews();
						break;
				}
			}
		}
			
		private void listNews()
		{
			int intLanguageid = Convert.ToInt32(Request.QueryString["language"]);
				
			if(intLanguageid == 0){
				//intLanguageid = 1;
				intLanguageid = Shared.UserLang();
			}

			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function Delete(intid){\n";
			js.Text += "var confirmDelete = window.confirm('Er du sikker på du vil slette denne nyhed?');\n";
			js.Text += "if(confirmDelete){\n";
			js.Text += "location.href = '?page="+IntPageId+"&submenu="+IntSubmenuId+"&mode=delete&id='+intid;\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>";
			
			Head_ph.Controls.Add(js);

			this.Controls.Add(new LiteralControl("Sprog: "));

			lbLanguage.ID = "lbLanguage";
			lbLanguage.Rows = 1;
			lbLanguage.Attributes["onchange"] = "location.href='?page="+IntPageId+"&submenu="+IntSubmenuId+"&language='+this.value";
			
			Database db = new Database();
			string strSql = "SELECT id,name FROM language " + (Shared.UserIsDist() ? "WHERE id = " + Shared.UserLang() : "") + " ORDER BY name";
			lbLanguage.DataSource = db.select(strSql);
			lbLanguage.DataTextField = "name";
			lbLanguage.DataValueField = "id";
			lbLanguage.DataBind();
			
			db.objDataReader.Close();
			db = null;

			this.Controls.Add(lbLanguage);
			this.Controls.Add(new LiteralControl("<br /><br />"));

			db = new Database();
			strSql = "SELECT DATE_FORMAT(addedtime,'%d-%m-%Y') AS dato ,header,id FROM news WHERE languageid = "+ intLanguageid +" ORDER BY addedtime DESC;";
			
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

			objBc.DataField = "dato";
			objBc.HeaderText = "Dato";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 90;
			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "header";
			objBc.HeaderText = "Overskrift";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 290;
			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=edit&id={0}";
			objHlc.HeaderText = "Ret";
			objHlc.Text = "Ret";
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);
		
			objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "javascript:Delete({0});";
			objHlc.HeaderText = "Slet";
			objHlc.Text = "Slet";
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);

			objDg.DataBind();

			if(!(db.objDataReader.HasRows)){
				this.Controls.Add(new LiteralControl("Der blev ikke fundet nogle nyheder i databasen."));
			}else{
				this.Controls.Add(objDg);
			}

			db.objDataReader.Close();
			db = null;

			lbLanguage.SelectedValue = intLanguageid.ToString();
		}

		private void deleteNews(){
			Database db = new Database();
			string strSql = "DELETE FROM news WHERE id = "+intId;
			db.execSql(strSql);

			db = null;

			listNews();
		}

		private void addNews()
		{
			if(Session["noerror"] != null){
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
				Session["noerror"] = null;
			}

			this.Controls.Add(new LiteralControl("Sprog: * "));
			RequiredFieldValidator reqLanguage = new RequiredFieldValidator();
			reqLanguage.ControlToValidate = "lbLanguage";
			reqLanguage.ErrorMessage = "Dette felt skal udfyldes.";
			reqLanguage.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(reqLanguage);
			this.Controls.Add(new LiteralControl("<br />"));

			lbLanguage.ID = "lbLanguage";
			lbLanguage.Rows = 1;
			
			Database db = new Database();
			string strSql = "SELECT id,name FROM language " + (Shared.UserIsDist() ? "WHERE id = " + Shared.UserLang() : "") + " ORDER BY name";
			lbLanguage.DataSource = db.select(strSql);
			lbLanguage.DataTextField = "name";
			lbLanguage.DataValueField = "id";
			lbLanguage.DataBind();
			
			db.objDataReader.Close();
			db = null;

			if(!Shared.UserIsDist())
				lbLanguage.Items.Insert(0,new ListItem("Vælg...",""));

			this.Controls.Add(lbLanguage);
			this.Controls.Add(new LiteralControl("<br /><br />"));
			this.Controls.Add(new LiteralControl("Overskrift: * "));

			RequiredFieldValidator reqHeader = new RequiredFieldValidator();
			reqHeader.ControlToValidate = "tbHeader";
			reqHeader.ErrorMessage = "Dette felt skal udfyldes.";
			reqHeader.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(reqHeader);
			this.Controls.Add(new LiteralControl("<br />"));

			tbHeader.ID = "tbHeader";
			tbHeader.Columns = 20;
			tbHeader.Style.Add("width","475px");
			tbHeader.MaxLength = 255;
			
			this.Controls.Add(tbHeader);
			this.Controls.Add(new LiteralControl("<br /><br />Nyhedstekst: * "));

			RequiredFieldValidator reqBody = new RequiredFieldValidator();
			reqBody.ControlToValidate = "tbBody";
			reqBody.ErrorMessage = "Dette felt skal udfyldes.";
			reqBody.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(reqBody);
			this.Controls.Add(new LiteralControl("<br />"));

			tbBody.ID = "tbBody";
			tbBody.Columns = 20;
			tbBody.Rows = 10;
			tbBody.Style.Add("height","200px");
            tbBody.Style.Add("width","475px");
			tbBody.TextMode = TextBoxMode.MultiLine;
			
			this.Controls.Add(tbBody);

			this.Controls.Add(new LiteralControl("<br /><br />"));
			
			cbSticky.Text = " Behold denne nyhed øverst.";
			
			this.Controls.Add(cbSticky);

			this.Controls.Add(new LiteralControl("<br />"));

			Button submit = new Button();
			submit.Text = "Gem nyhed";
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","15px");
			submit.Click += new EventHandler(saveNews);
			
			this.Controls.Add(submit);
		}

		private void editNews()
		{
			if(Session["noerror"] != null){
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
				Session["noerror"] = null;
			}

			this.Controls.Add(new LiteralControl("Sprog: * "));
			RequiredFieldValidator reqLanguage = new RequiredFieldValidator();
			reqLanguage.ControlToValidate = "lbLanguage";
			reqLanguage.ErrorMessage = "Dette felt skal udfyldes.";
			reqLanguage.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(reqLanguage);
			this.Controls.Add(new LiteralControl("<br />"));

			lbLanguage.ID = "lbLanguage";
			lbLanguage.Rows = 1;
			
			Database db = new Database();
			string strSql = "SELECT id,name FROM language ORDER BY name";
			lbLanguage.DataSource = db.select(strSql);
			lbLanguage.DataTextField = "name";
			lbLanguage.DataValueField = "id";
			lbLanguage.DataBind();
			
			db.objDataReader.Close();
			db = null;

			lbLanguage.Items.Insert(0,new ListItem("Vælg...",""));

			this.Controls.Add(lbLanguage);
			this.Controls.Add(new LiteralControl("<br /><br />"));
			this.Controls.Add(new LiteralControl("Overskrift: * "));

			RequiredFieldValidator reqHeader = new RequiredFieldValidator();
			reqHeader.ControlToValidate = "tbHeader";
			reqHeader.ErrorMessage = "Dette felt skal udfyldes.";
			reqHeader.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(reqHeader);
			this.Controls.Add(new LiteralControl("<br />"));

			tbHeader.ID = "tbHeader";
			tbHeader.Columns = 20;
			tbHeader.Style.Add("width","475px");
			tbHeader.MaxLength = 255;
			
			this.Controls.Add(tbHeader);
			this.Controls.Add(new LiteralControl("<br /><br />Nyhedstekst: * "));

			RequiredFieldValidator reqBody = new RequiredFieldValidator();
			reqBody.ControlToValidate = "tbBody";
			reqBody.ErrorMessage = "Dette felt skal udfyldes.";
			reqBody.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(reqBody);
			this.Controls.Add(new LiteralControl("<br />"));

			tbBody.ID = "tbBody";
			tbBody.Columns = 20;
			tbBody.Rows = 10;
			tbBody.Style.Add("height","200px");
			tbBody.Style.Add("width","475px");
			tbBody.TextMode = TextBoxMode.MultiLine;
			
			this.Controls.Add(tbBody);

			this.Controls.Add(new LiteralControl("<br /><br />"));
			
			cbSticky.Text = " Behold denne nyhed øverst.";
			
			this.Controls.Add(cbSticky);

			this.Controls.Add(new LiteralControl("<br />"));

			Button submit = new Button();
			submit.Text = "Gem nyhed";
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","15px");
			submit.Click +=new EventHandler(updateNews);
			
			this.Controls.Add(submit);

			db = new Database();

			strSql = "SELECT header,body,sticky,languageid FROM news WHERE id = "+ intId;
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()){
				Wysiwyg wys = new Wysiwyg();

				tbHeader.Text = objDr["header"].ToString();
				tbBody.Text = wys.FromDb(1,objDr["body"].ToString());
				
				if(Convert.ToInt32(objDr["sticky"]) == 1){
					cbSticky.Checked = true;
				}

				lbLanguage.SelectedValue = objDr["languageid"].ToString();
			}

			db.objDataReader.Close();
			db = null;
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

		private void saveNews(object sender, EventArgs e)
		{
			if(Page.IsValid){
				Database db = new Database();
				Wysiwyg wys = new Wysiwyg();
				int intSticky = 0;
				if(cbSticky.Checked) {
					intSticky = 1;
				}

				string strSql = "INSERT INTO news (addedtime,header,body,languageid,sticky,author) VALUES(CURRENT_TIMESTAMP(),'"+ wys.ToDb(2,tbHeader.Text) +"','"+ wys.ToDb(1,tbBody.Text) +"',"+ Convert.ToInt32(lbLanguage.SelectedValue) + ","+ intSticky + "," + ((Admin)Session["user"]).IntUserId +")";
				db.execSql(strSql);

				db = null;

				Session["noerror"] = "<div id='noerror'>Nyheden er tilføjet.</div>";

				Response.Redirect("?page="+ IntPageId);
			}

		}

		private void updateNews(object sender, EventArgs e) {
			if(Page.IsValid){
				Database db = new Database();
				Wysiwyg wys = new Wysiwyg();
				int intSticky = 0;
				if(cbSticky.Checked) {
					intSticky = 1;
				}

				string strSql = "UPDATE news SET header = '"+wys.ToDb(2,tbHeader.Text)+"',body = '"+wys.ToDb(1,tbBody.Text)+"',sticky ="+intSticky+",languageid = "+Convert.ToInt32(lbLanguage.SelectedValue)+" WHERE id = "+intId;
				db.execSql(strSql);

				db = null;

				Session["noerror"] = "<div id='noerror'>Nyheden er nu rettet.</div>";

				Response.Redirect("?page="+ IntPageId +"&submenu="+IntSubmenuId+"&mode=edit&id="+intId);
			}
		}
	}
}
