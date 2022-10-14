namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Web.UI;
	using MySql.Data.MySqlClient;
	using tye.exceptions;

	public partial class page_admin : uc_pages
	{
		protected ListBox language_lb = new ListBox();
		protected int intEditPageId;
		protected Label old_header = new Label();
		protected TextBox new_header = new TextBox();
		protected TextBox content = new TextBox();
		protected MySqlDataReader objDr;
		protected string strSql;
		protected int intUserControlId;
		protected int intLanguageId;
		protected string strMode;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

			try
			{
				strMode = Request.QueryString["mode"];

				if (strMode == null) {
					drawListPage();
				}
				else if (strMode == "edit") {
					drawEditPage();
				}


			} catch(FileNotFound fnf) {
				this.Controls.Clear();

				Label nl = new Label();
                nl.Text = fnf.Message(((tye.Menu)Session["menu"]).IntLanguageId);
				this.Controls.Add(nl);
				this.Controls.Add(new LiteralControl("<br/><br/>"));
				HtmlAnchor back_link = new HtmlAnchor();

				back_link.ID = "back_link";
				back_link.HRef = "javascript:history.go(-1)";
				back_link.InnerHtml = "Tilbage til den forrige side";
				this.Controls.Add(back_link);
			}
		}

		protected void drawEditPage()
		{
			intEditPageId = Convert.ToInt32(Request.QueryString["id"]);

			// Tilføjer javascript til head

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
	
			js.Text += "function storeCaret(textEl) {\n";
			js.Text += "if (textEl.createTextRange){\n";
			js.Text += "textEl.caretPos = document.selection.createRange().duplicate();\n";
			js.Text += "}\n";
			js.Text += "}\n\n";

			js.Text += "function addLink(){\n";
			js.Text += "var linkstr = window.prompt('Indtast linket her:','');\n\n";
			js.Text += "if (linkstr.indexOf('http://') == -1){\n";
			js.Text += "linkstr = 'http://' + linkstr;\n";
			js.Text += "}\n\n";
			js.Text += "linkstr = '[link,' + linkstr + ',ekstern]';\n";
			js.Text += "var textsel = document.selection.createRange().text;\n\n";
			js.Text += "if (linkstr != null && document.selection.createRange().text != '' && document.selection.createRange().text.indexOf(linkstr) == -1) {\n";
			js.Text += "sT = document.selection.createRange();\n";  
			js.Text += "sTxt = linkstr + sT.text + '[/link]';\n"; 
			js.Text += "sT.text = sTxt;\n"; 
			js.Text += "} else {\n";
			js.Text += "sT = document.selection.createRange();\n";
			js.Text += "sTxt = sT.text.replace(linkstr,'');\n";
			js.Text += "sTxt = sTxt.replace('[/link]','');\n";	    
			js.Text += "sT.text = sTxt;\n";
			js.Text += "}\n";
			js.Text += "}\n";

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


			this.Controls.Add(new LiteralControl("Nuværende overskrift:<br/>"));

			old_header.CssClass = "page_subheader";
			old_header.ID = "old_header";

			this.Controls.Add(old_header);

			this.Controls.Add(new LiteralControl("<br/><br/>Sprog:<br/>"));

			language_lb.ID = "language_lb";
			language_lb.Rows = 1;
			language_lb.SelectionMode = ListSelectionMode.Single;
			language_lb.Attributes["onchange"] = "window.location = '?page=" + IntPageId + "&mode=edit&id=' + this.value;";

			this.Controls.Add(language_lb);
			
			this.Controls.Add(new LiteralControl("<br/><br/>Ny overskrift: *"));
			
			RequiredFieldValidator new_header_val = new RequiredFieldValidator();

			new_header_val.ID="new_header_val";
			new_header_val.ErrorMessage = "Dette felt skal udfyldes!";
			new_header_val.ControlToValidate = "new_header";

			this.Controls.Add(new_header_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			new_header.ID = "new_header";
			new_header.Columns = 25;
			new_header.Width = 237;
			new_header.Attributes["style"] = "width:237px;";

			this.Controls.Add(new_header);

			this.Controls.Add(new LiteralControl("<br/><br/><br/>"));

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

			HtmlAnchor page_subheader_btn = new HtmlAnchor();

			page_subheader_btn.ID = "page_subheader_btn";
			page_subheader_btn.Attributes["class"] = "page_admin_btn";
			page_subheader_btn.HRef = "javascript:addHtml('[overskrift]','[/overskrift]');";
			page_subheader_btn.InnerHtml = "[Overskrift]";

			this.Controls.Add(page_subheader_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor internal_link_btn = new HtmlAnchor();

			internal_link_btn.ID = "internal_link_btn";
			internal_link_btn.Attributes["class"] = "page_admin_btn";
			internal_link_btn.HRef = "../../#";
			internal_link_btn.Attributes["onclick"] = "javascript:window.open('popups/Internal_link.aspx?pageid='+ main_form." + language_lb.ClientID + ".value,'Internt_link','scrollbars=yes,resizeable=no,toolbars=no,width=400,height=600');";
			internal_link_btn.InnerHtml = "[Internt link]";

			this.Controls.Add(internal_link_btn);
			
			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor external_link_btn = new HtmlAnchor();

			external_link_btn.ID = "external_link_btn";
			external_link_btn.Attributes["class"] = "page_admin_btn";
			external_link_btn.HRef = "javascript:addLink();";
			external_link_btn.InnerHtml = "[Eksternt link]";

			this.Controls.Add(external_link_btn);

			this.Controls.Add(new LiteralControl("<div style='margin-top:10px;'>"));

			HtmlAnchor image_btn = new HtmlAnchor();

			image_btn.ID = "image_btn";
			image_btn.Attributes["class"] = "page_admin_btn";
			image_btn.Attributes["onclick"] = "javascript:window.open('popups/List.aspx?mode=image','Indsæt_billede','scrollbars=yes,resizeable=no,toolbars=no,width=400,height=600');";
			image_btn.HRef = "../../#";
			image_btn.InnerHtml = "[Billede]";

			this.Controls.Add(image_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));	
		
			HtmlAnchor file_btn = new HtmlAnchor();

			file_btn.ID = "file_btn";
			file_btn.Attributes["class"] = "page_admin_btn";
			file_btn.Attributes["onclick"] = "javascript:window.open('popups/List.aspx?mode=file','Indsæt_fil','scrollbars=yes,resizeable=no,toolbars=no,width=400,height=600');";
			file_btn.HRef = "../../#";
			file_btn.InnerHtml = "[Fil]";

			this.Controls.Add(file_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));	

			HtmlAnchor object_left_btn = new HtmlAnchor();

			object_left_btn.ID = "object_left_btn";
			object_left_btn.Attributes["class"] = "page_admin_btn";
			object_left_btn.HRef = "javascript:addHtml('[venstre stillet]','[/venstre stillet]');";
			object_left_btn.InnerHtml = "[Vestre stillet]";

			this.Controls.Add(object_left_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor center_btn = new HtmlAnchor();

			center_btn.ID = "center_btn";
			center_btn.Attributes["class"] = "page_admin_btn";
			center_btn.HRef = "javascript:addHtml('[centreret]','[/centreret]');";
			center_btn.InnerHtml = "[Centreret]";

			this.Controls.Add(center_btn);

			this.Controls.Add(new LiteralControl("&nbsp;"));

			HtmlAnchor object_right_btn = new HtmlAnchor();

			object_right_btn.ID = "object_right_btn";
			object_right_btn.Attributes["class"] = "page_admin_btn";
			object_right_btn.HRef = "javascript:addHtml('[højre stillet]','[/højre stillet]');";
			object_right_btn.InnerHtml = "[Højre stillet]";

			this.Controls.Add(object_right_btn);

			this.Controls.Add(new LiteralControl("</div>"));

			this.Controls.Add(new LiteralControl("<div style='margin-top:10px;'>"));

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


			this.Controls.Add(new LiteralControl("</div>"));

			this.Controls.Add(new LiteralControl("<br/><br/>Indhold: *"));

			RequiredFieldValidator content_val = new RequiredFieldValidator();

			content_val.ID="content_val";
			content_val.ErrorMessage = "Dette felt skal udfyldes!";
			content_val.ControlToValidate = "content";

			this.Controls.Add(content_val);

			this.Controls.Add(new LiteralControl("<br/>"));
									  
			content.ID = "content";
			content.Columns = 50;
			content.Rows = 15;
			content.Attributes["style"] = "width:475px;height:180px;";
			content.TextMode = TextBoxMode.MultiLine;
			content.Attributes["onchange"] ="storeCaret(this);";
			content.Attributes["onkeyup"] ="storeCaret(this);";
			content.Attributes["onkeydown"] = "storeCaret(this);";
			content.Attributes["onclick"] = "storeCaret(this);";
			content.Attributes["onselect"] = "storeCaret(this);";

			this.Controls.Add(content);			
			
			this.Controls.Add(new LiteralControl("<br/><br/>"));

			HtmlAnchor preview_link = new HtmlAnchor();

			preview_link.ID = "preview_link";
			preview_link.Attributes["class"] = "page_admin_btn";
			preview_link.Attributes["onclick"] = "javascript:window.open('popups/Preview.aspx','Preview','scrollbars=yes,resizeable=no,toolbars=no,width=500,height=450');";
			preview_link.InnerHtml = "Se sidelayout (Gemmer ikke siden)";
			preview_link.HRef = "../../#";
			preview_link.Attributes["style"] = "width:100%;";

			this.Controls.Add(preview_link);

			this.Controls.Add(new LiteralControl("<br/><br/><br/><br/>"));

			Button submit = new Button();

			submit.ID = "submit";
			submit.Click += new EventHandler(savePage);
			submit.Text = "Gem siden";
			submit.Attributes["style"] = "width:475px;";

			this.Controls.Add(submit);

			this.Controls.Add(new LiteralControl("<br/><br/>"));

			HtmlAnchor back_link = new HtmlAnchor();

			back_link.ID = "back_link";
			back_link.HRef = "../../?page=" + IntPageId;
			back_link.InnerHtml = "Tilbage til den forrige side";

			this.Controls.Add(back_link);


			//Henter sidenavn ud fra databasen og kaster exception hvis ikke der findes noget

			Database db_content = new Database();

			strSql = "SELECT name,body,usercontrolid,language FROM menu INNER JOIN content ON menu.id = content.menuid WHERE menu.id = " + intEditPageId;

			MySqlDataReader objDr_content = db_content.select(strSql);

			if (objDr_content.Read())
			{
				Wysiwyg wys = new Wysiwyg();

				old_header.Text = objDr_content["name"].ToString();
				new_header.Text = objDr_content["name"].ToString();
				content.Text = wys.Encode(wys.FromDb(1,objDr_content["body"].ToString()));

				intUserControlId = Convert.ToInt32(objDr_content["usercontrolid"]);
				intLanguageId = Convert.ToInt32(objDr_content["language"]);

				db_content.objDataReader.Close();
				db_content = null;
				wys = null;
				objDr_content = null;
			}
			else
			{
				db_content.objDataReader.Close();
				db_content = null;
				objDr_content = null;

				throw new FileNotFound();
			}

			//rettet
			// Jb Rettet;
			//db_content.objDataReader.Close();
			//db_content = null;

			if (!(Page.IsPostBack))
			{
				//Henter sprog ud til listbox
			
				Database db = new Database();

				strSql = "SELECT menu.id, language.name FROM menu" +
					" INNER JOIN language ON menu.language = language.id AND usercontrolid = " + intUserControlId + 
					" INNER JOIN content ON menuid = menu.id " +
					(Shared.UserIsDist() ? "WHERE language.id = " + Shared.UserLang() : "") + 
					" ORDER BY language.id;";

				language_lb.DataSource = db.select(strSql);
				language_lb.DataTextField = "name";
				language_lb.DataValueField = "id";
				language_lb.DataBind();
		
				db.objDataReader.Close();
				db = null;

			}

			language_lb.SelectedValue = intEditPageId.ToString();
		}

		private void drawListPage()
		{
			Database db = new Database();

			string strSql = "SELECT id,name FROM menu WHERE language = " + (Shared.UserIsDist() ? Shared.UserLang() + "" : "1") + "  AND iseditable = 1 ORDER BY name";

			DataGrid objDg = new DataGrid();
			objDg.ID = "data_table";
			objDg.AutoGenerateColumns = false;
			objDg.DataSource = db.select(strSql);
			objDg.CellPadding = 0;
			objDg.CellSpacing = 0;
			objDg.BorderWidth = 1;
			objDg.CssClass = "data_table";
			objDg.GridLines = GridLines.None;
			objDg.Width = 480;

			BoundColumn objBc = new BoundColumn();

			objBc.DataField = "name";
			objBc.HeaderText = "Navn:";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "../../?page=" + Request.QueryString["page"].ToString() + "&mode=edit&id={0}";
			objHlc.HeaderText = "Ret indhold:";
			objHlc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			objHlc.Text = "Ret";
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";
			objHlc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
			objHlc.HeaderStyle.Width = 100;

			objDg.Columns.Add(objHlc);

			objDg.DataBind();

			this.Controls.Add(objDg);

			db.objDataReader.Close();

			db = null;
		}


		//Opdaterer databasen hvis de krævede felter er udfyldt

		public void savePage(object Sender, EventArgs E)
		{
			if (Page.IsValid)
			{
				Database db = new Database();
				Wysiwyg wys = new Wysiwyg();

				strSql = "UPDATE menu SET name = '" + wys.ToDb(2,new_header.Text) + "' WHERE id = " + intEditPageId;
			
				db.execSql(strSql);

				strSql = "UPDATE content SET body = '" + wys.ToDb(1,wys.Decode(content.Text)) + "' WHERE menuid = " + intEditPageId;
			
				db.execSql(strSql);

				db = null;
				wys = null;

				LogPageEdit objPe = new LogPageEdit();

				objPe.IntAuthorId = ((Admin)Session["user"]).IntUserId;
				objPe.IntContentId = intEditPageId;
				objPe.StrIp = Request.UserHostAddress;

				objPe.Insert(objPe);

				objPe = null;

				Response.Redirect("?page=95&mode=edit&id=" + intEditPageId);
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
