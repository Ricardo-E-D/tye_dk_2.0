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

	public partial class admin_encyclopedia : uc_pages
	{
		protected string strMode;

		protected ListBox languageId = new ListBox();
		protected TextBox word = new TextBox();
		protected TextBox description = new TextBox();
		protected CheckBox access_surfer = new CheckBox();
		protected CheckBox access_client = new CheckBox();
		protected CheckBox access_optician = new CheckBox();
		protected int intAccessSurfer = 0;
		protected int intAccessClient = 0;
		protected int intAccessOptician = 0;
		protected int intId;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			strMode = Request.QueryString["mode"];
			intId = Convert.ToInt32(Request.QueryString["id"]);
			
			try {
				if(IntSubmenuId == 159 || IntSubmenuId == 1188) {
					drawAddForm();
				}
				if (IntSubmenuId == 160 || IntSubmenuId == 1243) {
					switch (strMode) {
						case "edit":
							drawEditForm();
							break;
						case "delete":
							delete();
							break;
						default:
							drawList();
							break;
					}
				}
			}
			catch(NoDataFound ndf)
			{
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId).ToString()));
			}
		}

		private void drawAddForm()
		{
			if(Session["noerror"] != null) {
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
				Session["noerror"] = null;
			}

			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";

			js.Text += "function addHtml(htmlBefore, htmlAfter){\n";
			js.Text += "if (document.selection.createRange().text != '' && document.selection.createRange().text.indexOf(htmlBefore) == -1) {\n"; 
			js.Text += "sT = document.selection.createRange();\n";
			js.Text += "sTxt = htmlBefore + sT.text + htmlAfter;\n";
			js.Text += "sT.text = sTxt\n";
			js.Text += "} else {\n"; 
			js.Text += "sT = document.selection.createRange();\n";
			js.Text += "sTxt = sT.text.replace(htmlBefore,'');\n";
			js.Text += "sTxt = sTxt.replace(htmlAfter,'');\n";    
			js.Text += "sT.text = sTxt\n";
			js.Text += "}\n";
			js.Text += "}\n\n";
	
			js.Text += "function addList(liststart,listend){\n";
			js.Text += "var input = document.selection.createRange().text;\n";
			js.Text += "if(input != '' && document.selection.createRange().text.indexOf('[liste:') == -1) {\n";
			js.Text += "var inputarr = input.split('\\n');\n";
			js.Text += "var temptxt = '';\n";				
			js.Text += "for(var i = 0;i < inputarr.length;i++)\n";
			js.Text += "{\n";
			js.Text += @"temptxt = temptxt + '[punkt]' + inputarr[i].replace(/\r\n|\r/g,'') + '[/punkt]\n';";
			js.Text += "\n}\n";
			js.Text += "temptxt = liststart + '\\n' + temptxt + listend;\n";
			js.Text += "document.selection.createRange().text = temptxt;\n";
			js.Text += "} else {\n";
			js.Text += "sT = document.selection.createRange();\n";
			js.Text += "sTxt = sT.text.replace('[liste:tal]','');\n";
			js.Text += "sTxt = sTxt.replace('[/liste:tal]','');\n";
			js.Text += "sTxt = sTxt.replace('[liste:prik]','');\n";
			js.Text += "sTxt = sTxt.replace('[/liste:prik]','');\n";
			js.Text += @"sTxt = sTxt.replace(/\[punkt\]/g,'');";
			js.Text += "\n";
			js.Text += @"sTxt = sTxt.replace(/\[\/punkt\]/g,'');";	   
			js.Text += "\n";
			js.Text += "sT.text = sTxt;\n";
			js.Text += "}\n";
			js.Text += "input = '';\n";
			js.Text += "arrinput = '';\n";
			js.Text += "temptxt = '';\n";
			js.Text += "sT = '';\n";
			js.Text += "sTxt = '';\n";
			js.Text += "}\n";
			js.Text += "</script>";

			Head_ph.Controls.Add(js);

			this.Controls.Add(new LiteralControl("Sprog: * "));

			RequiredFieldValidator language_val = new RequiredFieldValidator();

			language_val.ID = "language_val";
			language_val.ControlToValidate = "languageId";
			language_val.ErrorMessage = "Vælg et sprog.";
			language_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(language_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			Database db = new Database();

			string strSql = "SELECT id,name FROM language" + (Shared.UserIsDist() ? " WHERE language.id = " + Shared.UserLang() : "") + " ORDER BY name;";

			languageId.ID = "languageid";

			languageId.DataSource = db.select(strSql);
			languageId.DataTextField = "name";
			languageId.DataValueField = "id";
			languageId.Rows = 1;

			languageId.DataBind();

			db.objDataReader.Close();
			db = null;
			if(!Shared.UserIsDist()) {
				ListItem objLi = new ListItem();
				objLi.Text = "Vælg:";
				objLi.Value = "";
				objLi.Selected = true;
				languageId.Items.Insert(0,objLi);
			}

			this.Controls.Add(languageId);

			this.Controls.Add(new LiteralControl("<br/><br/>Ord: * "));

			RequiredFieldValidator word_val = new RequiredFieldValidator();

			word_val.ID = "word_val";
			word_val.ControlToValidate = "word";
			word_val.ErrorMessage = "Dette felt skal udfyldes.";
			word_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(word_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			word.ID = "word";
			word.Width = 200;
			word.Style.Add("width","250px");

			this.Controls.Add(word);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			HtmlAnchor bold_text_btn = new HtmlAnchor();

			bold_text_btn.ID = "bold_text_btn";
			bold_text_btn.Attributes["class"] = "page_admin_btn";
			bold_text_btn.HRef = "javascript:addHtml('[fed]','[/fed]');";
			bold_text_btn.InnerHtml = "[Fed]";

			this.Controls.Add(bold_text_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor italic_text_btn = new HtmlAnchor();

			italic_text_btn.ID = "italic_text_btn";
			italic_text_btn.Attributes["class"] = "page_admin_btn";
			italic_text_btn.HRef = "javascript:addHtml('[kursiv]','[/kursiv]');";
			italic_text_btn.InnerHtml = "[Kursiv]";

			this.Controls.Add(italic_text_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor olist = new HtmlAnchor();

			olist.ID = "olist";
			olist.Attributes["class"] = "page_admin_btn";
			olist.HRef = "javascript:addList('[liste:tal]','[/liste:tal]');";
			olist.InnerHtml = "[liste:tal]";

			this.Controls.Add(olist);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor ulist = new HtmlAnchor();

			ulist.ID = "ulist";
			ulist.Attributes["class"] = "page_admin_btn";
			ulist.HRef = "javascript:addList('[liste:prik]','[/liste:prik]');";
			ulist.InnerHtml = "[liste:prik]";

			this.Controls.Add(ulist);


			this.Controls.Add(new LiteralControl("<br/><br/>Beskrivelse: * "));

			RequiredFieldValidator description_val = new RequiredFieldValidator();

			description_val.ID = "description_val";
			description_val.ControlToValidate = "description";
			description_val.ErrorMessage = "Dette felt skal udfyldes.";
			description_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(description_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			description.ID = "description";
			description.TextMode = TextBoxMode.MultiLine;
			description.Width = 400;
			description.Rows = 10;
			description.Style.Add("width","475px");
			description.Style.Add("height","100px");

			this.Controls.Add(description);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			access_surfer.ID = "access_surfer";
			access_surfer.Style.Add("border","0px;");
			access_surfer.Text = " Adgang for surfer?";

			this.Controls.Add(access_surfer);
			
			this.Controls.Add(new LiteralControl("<br/><br/>"));

			access_client.ID = "access_client";
			access_client.Style.Add("border","0px;");
			access_client.Text = " Adgang for slutbruger?";

			this.Controls.Add(access_client);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			access_optician.ID = "access_optician";
			access_optician.Style.Add("border","0px;");
			access_optician.Text = " Adgang for optiker?";

			this.Controls.Add(access_optician);

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = "Gem ordet";
			submit.Width = 400;
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","30px");
			submit.Click += new EventHandler(saveWord);

			this.Controls.Add(submit);

		}

		private void drawEditForm()
		{
			if(Session["noerror"] != null) {
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));
				Session["noerror"] = null;
			}

			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";

			js.Text += "function addHtml(htmlBefore, htmlAfter){\n";
			js.Text += "if (document.selection.createRange().text != '' && document.selection.createRange().text.indexOf(htmlBefore) == -1) {\n"; 
			js.Text += "sT = document.selection.createRange();\n";
			js.Text += "sTxt = htmlBefore + sT.text + htmlAfter;\n";
			js.Text += "sT.text = sTxt\n";
			js.Text += "} else {\n"; 
			js.Text += "sT = document.selection.createRange();\n";
			js.Text += "sTxt = sT.text.replace(htmlBefore,'');\n";
			js.Text += "sTxt = sTxt.replace(htmlAfter,'');\n";    
			js.Text += "sT.text = sTxt\n";
			js.Text += "}\n";
			js.Text += "}\n\n";
	
			js.Text += "function addList(liststart,listend){\n";
			js.Text += "var input = document.selection.createRange().text;\n";
			js.Text += "if(input != '' && document.selection.createRange().text.indexOf('[liste:') == -1) {\n";
			js.Text += "var inputarr = input.split('\\n');\n";
			js.Text += "var temptxt = '';\n";				
			js.Text += "for(var i = 0;i < inputarr.length;i++)\n";
			js.Text += "{\n";
			js.Text += @"temptxt = temptxt + '[punkt]' + inputarr[i].replace(/\r\n|\r/g,'') + '[/punkt]\n';";
			js.Text += "\n}\n";
			js.Text += "temptxt = liststart + '\\n' + temptxt + listend;\n";
			js.Text += "document.selection.createRange().text = temptxt;\n";
			js.Text += "} else {\n";
			js.Text += "sT = document.selection.createRange();\n";
			js.Text += "sTxt = sT.text.replace('[liste:tal]','');\n";
			js.Text += "sTxt = sTxt.replace('[/liste:tal]','');\n";
			js.Text += "sTxt = sTxt.replace('[liste:prik]','');\n";
			js.Text += "sTxt = sTxt.replace('[/liste:prik]','');\n";
			js.Text += @"sTxt = sTxt.replace(/\[punkt\]/g,'');";
			js.Text += "\n";
			js.Text += @"sTxt = sTxt.replace(/\[\/punkt\]/g,'');";	   
			js.Text += "\n";
			js.Text += "sT.text = sTxt;\n";
			js.Text += "}\n";
			js.Text += "input = '';\n";
			js.Text += "arrinput = '';\n";
			js.Text += "temptxt = '';\n";
			js.Text += "sT = '';\n";
			js.Text += "sTxt = '';\n";
			js.Text += "}\n";
			js.Text += "</script>";

			Head_ph.Controls.Add(js);

			this.Controls.Add(new LiteralControl("Sprog: * "));

			RequiredFieldValidator language_val = new RequiredFieldValidator();

			language_val.ID = "language_val";
			language_val.ControlToValidate = "languageId";
			language_val.ErrorMessage = "Vælg et sprog.";
			language_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(language_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			Database db = new Database();

			string strSql = "SELECT id,name FROM language" + (Shared.UserIsDist() ? " WHERE language.id = " + Shared.UserLang() : "") + " ORDER BY name;";

			languageId.ID = "languageid";

			languageId.DataSource = db.select(strSql);
			languageId.DataTextField = "name";
			languageId.DataValueField = "id";
			languageId.Rows = 1;

			languageId.DataBind();

			db.objDataReader.Close();
			db = null;

			if(!Shared.UserIsDist()) {
				ListItem objLi = new ListItem();
				objLi.Text = "Vælg:";
				objLi.Value = "";
				objLi.Selected = true;
				languageId.Items.Insert(0,objLi);
			}
			this.Controls.Add(languageId);

			this.Controls.Add(new LiteralControl("<br/><br/>Ord: * "));

			RequiredFieldValidator word_val = new RequiredFieldValidator();

			word_val.ID = "word_val";
			word_val.ControlToValidate = "word";
			word_val.ErrorMessage = "Dette felt skal udfyldes.";
			word_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(word_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			word.ID = "word";
			word.Width = 200;
			word.Style.Add("width","250px");

			this.Controls.Add(word);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			HtmlAnchor bold_text_btn = new HtmlAnchor();

			bold_text_btn.ID = "bold_text_btn";
			bold_text_btn.Attributes["class"] = "page_admin_btn";
			bold_text_btn.HRef = "javascript:addHtml('[fed]','[/fed]');";
			bold_text_btn.InnerHtml = "[Fed]";

			this.Controls.Add(bold_text_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor italic_text_btn = new HtmlAnchor();

			italic_text_btn.ID = "italic_text_btn";
			italic_text_btn.Attributes["class"] = "page_admin_btn";
			italic_text_btn.HRef = "javascript:addHtml('[kursiv]','[/kursiv]');";
			italic_text_btn.InnerHtml = "[Kursiv]";

			this.Controls.Add(italic_text_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor olist = new HtmlAnchor();

			olist.ID = "olist";
			olist.Attributes["class"] = "page_admin_btn";
			olist.HRef = "javascript:addList('[liste:tal]','[/liste:tal]');";
			olist.InnerHtml = "[liste:tal]";

			this.Controls.Add(olist);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor ulist = new HtmlAnchor();

			ulist.ID = "ulist";
			ulist.Attributes["class"] = "page_admin_btn";
			ulist.HRef = "javascript:addList('[liste:prik]','[/liste:prik]');";
			ulist.InnerHtml = "[liste:prik]";

			this.Controls.Add(ulist);


			this.Controls.Add(new LiteralControl("<br/><br/>Beskrivelse: * "));

			RequiredFieldValidator description_val = new RequiredFieldValidator();

			description_val.ID = "description_val";
			description_val.ControlToValidate = "description";
			description_val.ErrorMessage = "Dette felt skal udfyldes.";
			description_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(description_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			description.ID = "description";
			description.TextMode = TextBoxMode.MultiLine;
			description.Width = 400;
			description.Rows = 10;
			description.Style.Add("width","475px");
			description.Style.Add("height","100px");

			this.Controls.Add(description);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			access_surfer.ID = "access_surfer";
			access_surfer.Style.Add("border","0px;");
			access_surfer.Text = " Adgang for surfer?";

			this.Controls.Add(access_surfer);
			
			this.Controls.Add(new LiteralControl("<br/><br/>"));

			access_client.ID = "access_client";
			access_client.Style.Add("border","0px;");
			access_client.Text = " Adgang for slutbruger?";

			this.Controls.Add(access_client);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			access_optician.ID = "access_optician";
			access_optician.Style.Add("border","0px;");
			access_optician.Text = " Adgang for optiker?";

			this.Controls.Add(access_optician);

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = "Gem ordet";
			submit.Width = 400;
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","30px");
			submit.Click += new EventHandler(updateWord);

			this.Controls.Add(submit);

			//Fylder indhold i felterne

			db = new Database();
			strSql = "SELECT languageid,word,description,access_surfer,access_optician,access_client FROM encyclopedia WHERE id = " + intId;
			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db.Dispose();
				db = null;
				throw new NoDataFound();
			}

			if(objDr.Read())
			{
				Wysiwyg wys = new Wysiwyg();

				languageId.SelectedValue = objDr["languageid"].ToString();
				word.Text = objDr["word"].ToString();
				description.Text = wys.Encode(wys.FromDb(1,objDr["description"].ToString()));

				if(Convert.ToInt32(objDr["access_surfer"]) == 1)
				{
					access_surfer.Checked = true;
				}
				if(Convert.ToInt32(objDr["access_client"]) == 1)
				{
					access_client.Checked = true;
				}
				if(Convert.ToInt32(objDr["access_optician"]) == 1)
				{
					access_optician.Checked = true;
				}

				wys = null;
			}

			db.objDataReader.Close();
			db = null;
		}

		private void delete()
		{
			Database db = new Database();
			string strSql = "DELETE FROM encyclopedia WHERE id = " + intId;
			db.execSql(strSql);

			Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId);
		}

		private void drawList()
		{
			Literal js = new Literal();

			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function deleteW(id){\n";
			js.Text += "var confirm = window.confirm('Er du sikker på at du vil slette dette ord/sætning?');\n";
			js.Text += "if(confirm){\n";
			js.Text += "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=delete&id='+id;\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			Head_ph.Controls.Add(js);

			Database db = new Database();
			string strSql = "SELECT encyclopedia.id,encyclopedia.word,language.name as lname,access_surfer,access_client,access_optician" +
							" FROM encyclopedia" + 
							" INNER JOIN language ON languageid = language.id" +
							(Shared.UserIsDist() ? " WHERE language.id = " + Shared.UserLang() : "") + " ORDER BY word;";

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

			objBc.DataField = "word";
			objBc.HeaderText = "Ord";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 240;

			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "lname";
			objBc.HeaderText = "Sprog";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 90;

			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "access_surfer";
			objBc.HeaderText = "S";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 30;

			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "access_client";
			objBc.HeaderText = "C";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 30;

			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "access_optician";
			objBc.HeaderText = "O";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 30;

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
			objHlc.DataNavigateUrlFormatString = "javascript:deleteW({0});";
			objHlc.Text = "Slet";
			objDg.Columns.Add(objHlc);

			objDg.DataBind();

			if(!(db.objDataReader.HasRows))
			{
				db.objDataReader.Close();
				throw new NoDataFound();
			}

			this.Controls.Add(objDg);

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

		private void saveWord(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				if(access_surfer.Checked)
				{
					intAccessSurfer = 1;
				}

				if(access_client.Checked)
				{
					intAccessClient = 1;
				}

				if(access_optician.Checked)
				{
					intAccessOptician = 1;
				}

				Database db = new Database();
				Wysiwyg wys = new Wysiwyg();

				string strSql = "INSERT INTO encyclopedia (languageid,word,description,access_surfer,access_client,access_optician) VALUES(";
				strSql += Convert.ToInt32(languageId.SelectedValue) + ",'" + wys.ToDb(2,word.Text) + "','" + wys.ToDb(1,wys.Decode(description.Text)) + "',";
				strSql += intAccessSurfer + "," + intAccessClient + "," + intAccessOptician + ");";

				db.execSql(strSql);

				db = null;
				wys = null;

				Session["noerror"] = "<div id='noerror'>Ordet er nu gemt.</div>";

				Response.Redirect("?page=" + IntPageId);
			}
		}

		private void updateWord(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				if(access_surfer.Checked)
				{
					intAccessSurfer = 1;
				}

				if(access_client.Checked)
				{
					intAccessClient = 1;
				}

				if(access_optician.Checked)
				{
					intAccessOptician = 1;
				}

				Database db = new Database();
				Wysiwyg wys = new Wysiwyg();

				string strSql = "UPDATE encyclopedia SET languageid = " + Convert.ToInt32(languageId.SelectedValue) + ",word = '" + wys.ToDb(2,word.Text) + "',";
				strSql += "description = '" + wys.ToDb(1,wys.Decode(description.Text)) + "',access_surfer = " + intAccessSurfer + ",access_optician = " + intAccessOptician + ",";
				strSql += "access_client = " + intAccessClient + " WHERE id = " + intId;

				db.execSql(strSql);

				db = null;
				wys = null;

				Session["noerror"] = "<div id='noerror'>Ordet er nu opdateret.</div>";

				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=edit&id=" + intId);
			}
		}
	}
}
