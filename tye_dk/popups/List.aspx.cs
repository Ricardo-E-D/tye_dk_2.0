using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;

namespace tye.popups
{
	public partial class List : System.Web.UI.Page
	{
		protected HtmlGenericControl tab_content = new HtmlGenericControl("div");
		protected HtmlGenericControl insert_span = new HtmlGenericControl("span");
		protected HtmlGenericControl admin_span = new HtmlGenericControl("span");
		private int intLanguageId;
		protected HtmlGenericControl tab_focus = new HtmlGenericControl();
		protected string strMode;
		protected MySqlDataReader objDr;

		private tye.Admin currentUser = null;
		private Translation trans = null;

		protected void Page_Load(object sender, System.EventArgs e) {

			if(((Login)Session["login"]).BlnSucces == true)
			{
				currentUser = (tye.Admin)Session["user"];
				trans = new Translation(Server.MapPath("..\\uc\\translation.xml"), this.GetType().BaseType.ToString(), Translation.DbLangs[currentUser.IntLanguageId - 1].ToString());

				strMode = Request.QueryString["mode"];

				drawTabs();

				switch(strMode)
				{
					case "image":
						drawImageList();
						break;
					case "region":
						drawRegionList();
						break;
					case "file":
						drawFileList();
						break;
				}
			}
		}

		private void drawTabs()
		{
			tab_focus.ID = "tab_focus";
			list_body.Controls.Add(tab_focus);

			insert_span.ID = "insert_span";
			insert_span.Attributes["class"] = "tab_notfocus";

			list_body.Controls.Add(insert_span);
			
			admin_span.ID = "admin_span";
			admin_span.Attributes["class"] = "tab_notfocus";
		
			list_body.Controls.Add(admin_span);

			tab_content.ID = "tab_content";
			list_body.Controls.Add(tab_content);
		}

		private void drawImageList()
		{
			tab_focus.InnerHtml = "Indsæt billede";
			
			tab_content.Controls.Add(new LiteralControl("Billederne vises her i 100x100px, men de originale vil have den korrekte skalering.<br/><br/>"));
		
			HtmlAnchor upload_link = new HtmlAnchor();

			upload_link.ID = "upload_link";
			upload_link.HRef = "UploadFile.aspx?mode=image";
			upload_link.InnerHtml = "Upload billede";

			insert_span.Controls.Add(upload_link);

			HtmlAnchor admin_link = new HtmlAnchor();

			admin_link.ID = "admin_link";
			admin_link.HRef = "Admin.aspx?mode=image";
			admin_link.InnerHtml = "Slet billede";

			admin_span.Controls.Add(admin_link);

			Database db = new Database();

			string strSql = "SELECT id,name,path,description FROM file_image ORDER BY addedtime DESC;";

			objDr = db.select(strSql);	

			if (objDr.HasRows != true)
			{
				tab_content.Controls.Add(new LiteralControl("Der er ingen billeder i databasen."));
			}
			
			while(objDr.Read())
			{
				HtmlImage image = new HtmlImage();

				image.Alt = objDr["description"].ToString();
				image.ID = objDr["id"].ToString();
				image.Src = Files.strServerFilePath + "thumb_" + objDr["path"].ToString();
				image.Attributes["onclick"] = "addImage('[billede: " + objDr["name"].ToString() + " /]')";
				image.Attributes["class"] = "img_span";

				tab_content.Controls.Add(image);
			}


			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			Literal js = new Literal();
			js.Text = "<script type=\"text/javascript\">\n";
			js.Text += "function addImage(imgstr) {\n";
			js.Text += "var textstr = opener.parent.document.selection.createRange();\n";
			js.Text += "if (imgstr != '' && textstr.text.indexOf(imgstr) == -1) {\n";
			js.Text += "var caretPos = opener.parent.document.main_form._"+Files.strCtl+"_content.caretPos;\n";
			js.Text += "caretPos.text =\n";
			js.Text += "caretPos.text.charAt(caretPos.text.length - 1) == ' ' ?\n";
			js.Text += "imgstr + ' ' : imgstr;\n";
			js.Text += "window.close();\n";
			js.Text += "}else{\n";
			js.Text += "var sT = textstr;\n";
            js.Text += "var sTxt = sT.text.replace(imgstr,'');\n";
            js.Text += "sT.text = sTxt\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			head_ph.Controls.Add(js);
		}

		private void drawFileList()
		{
			tab_focus.InnerHtml = "Indsæt fil";
			
			//tab_content.Controls.Add(new LiteralControl("Billederne vises her i 100x100px, men de originale vil have den korrekte skalering.<br/><br/>"));

			HtmlAnchor upload_link = new HtmlAnchor();

			upload_link.ID = "upload_link";
			upload_link.HRef = "UploadFile.aspx?mode=file";
			upload_link.InnerHtml = "Upload fil";

			insert_span.Controls.Add(upload_link);

			HtmlAnchor admin_link = new HtmlAnchor();

			admin_link.ID = "admin_link";
			admin_link.HRef = "Admin.aspx?mode=file";
			admin_link.InnerHtml = "Slet fil";

			admin_span.Controls.Add(admin_link);

			Database db = new Database();

            string strSql = "SELECT file_files.id,file_files.name,path,description,language.name as lname FROM file_files INNER JOIN language ON languageid = language.id ORDER BY language.name, file_files.name;";

			objDr = db.select(strSql);	

			if (objDr.HasRows != true)
			{
				tab_content.Controls.Add(new LiteralControl("Der er ingen filer i databasen."));
			}
			else
			{
				DataGrid objDg = new DataGrid();
				objDg.ID = "data_table";
				objDg.AutoGenerateColumns = false;
				objDg.DataSource = objDr;
				objDg.CellPadding = 0;
				objDg.CellSpacing = 0;
				objDg.BorderWidth = 1;
				objDg.CssClass = "data_table";
				objDg.GridLines = GridLines.None;
				objDg.Width = 350;

				BoundColumn objBc = new BoundColumn();

				objBc.DataField = "name";
				objBc.HeaderText = "Navn:";
				objBc.HeaderStyle.CssClass = "data_table_header";
				objBc.ItemStyle.CssClass = "data_table_item";

				objDg.Columns.Add(objBc);

				objBc = new BoundColumn();

				objBc.DataField = "lname";
				objBc.HeaderText = "Sprog:";
				objBc.HeaderStyle.Width = 70;
				objBc.HeaderStyle.CssClass = "data_table_header";
				objBc.ItemStyle.CssClass = "data_table_item";

				objDg.Columns.Add(objBc);

				HyperLinkColumn objHlc = new HyperLinkColumn();
				objHlc.DataNavigateUrlField = "name";
				objHlc.DataNavigateUrlFormatString = "javascript:addFile('[fil: {0} /]')";
				objHlc.HeaderText = "Indsæt fil:";
				objHlc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
				objHlc.Text = "Indsæt";
				objHlc.HeaderStyle.CssClass = "data_table_header";
				objHlc.ItemStyle.CssClass = "data_table_item";
				objHlc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
				objHlc.HeaderStyle.Width = 100;

				objDg.Columns.Add(objHlc);

				objDg.DataBind();

				tab_content.Controls.Add(objDg);
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			Literal js = new Literal();
			js.Text = "<script type=\"text/javascript\">\n";
			js.Text += "function addFile(filestr) {\n";
			js.Text += "var textstr = opener.parent.document.selection.createRange();\n";
			js.Text += "if (filestr != '' && textstr.text.indexOf(filestr) == -1) {\n";
			js.Text += "var caretPos = opener.parent.document.main_form._"+Files.strCtl+"_content.caretPos;\n";
			js.Text += "caretPos.text =\n";
			js.Text += "caretPos.text.charAt(caretPos.text.length - 1) == ' ' ?\n";
			js.Text += "filestr + ' ' : filestr;\n";
			js.Text += "window.close();\n";
			js.Text += "}else{\n";
			js.Text += "var sT = textstr;\n";
			js.Text += "var sTxt = sT.text.replace(filestr,'');\n";
			js.Text += "sT.text = sTxt\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			head_ph.Controls.Add(js);
		}

		private void drawRegionList()
		{
			intLanguageId = Convert.ToInt32(Request.QueryString["language"]);

			tab_focus.InnerHtml = "Vælg region";
					
			if(!currentUser.IsDistributor) {

				HtmlAnchor insert_link = new HtmlAnchor();

				insert_link.ID = "insert_link";
				insert_link.HRef = "Insert.aspx?mode=region&language="+intLanguageId;
				insert_link.InnerHtml = "Tilføj ny region";

				insert_span.Controls.Add(insert_link);
	            
				HtmlAnchor admin_link = new HtmlAnchor();

				admin_link.ID = "admin_link";
				admin_link.HRef = "Admin.aspx?mode=region&language="+intLanguageId;
				admin_link.InnerHtml = "Administrer";

				admin_span.Controls.Add(admin_link);
			}
			Database db = new Database();

			string strSql = "SELECT map_region.name,CONCAT(map_region.id,',',map_region.name) AS idname FROM map_region INNER JOIN map ON mapid = map.languageid WHERE languageid = " + intLanguageId;

			DataGrid objDg = new DataGrid();
			objDg.ID = "regions";
			objDg.AutoGenerateColumns = false;
			objDg.DataSource = db.select(strSql);
			objDg.CellPadding = 0;
			objDg.CellSpacing = 0;
			objDg.BorderWidth = 0;
			objDg.GridLines = GridLines.None;
			objDg.Width = 365;

			BoundColumn objBc = new BoundColumn();

			objBc.DataField = "name";
			objBc.HeaderText = "";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 315;

			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "idname";
			objHlc.DataNavigateUrlFormatString = "javascript:setValue('{0}');window.close();";
			objHlc.HeaderText = "";
			objHlc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			objHlc.Text = "Indsæt";
			objHlc.ItemStyle.CssClass = "data_table_item";
			objHlc.HeaderStyle.Width = 50;

			objDg.Columns.Add(objHlc);

			objDg.DataBind();

			tab_content.Controls.Add(objDg);

			if (db.objDataReader.HasRows != true)
			{
				tab_content.Controls.Add(new LiteralControl("Der er ingen regioner i databasen."));
			}

			db.objDataReader.Close();
            db.dbDispose();
			db = null;

			Literal js = new Literal();

			js.Text += "<script type='text/javascript'>\n";
			js.Text += "function setValue(strValues){\n";

			js.Text += "var region = opener.document.getElementById('_"+Files.strCtl+"_region');\n";
			js.Text += "var regionid = opener.document.getElementById('_"+Files.strCtl+"_regionid');\n";

			js.Text += "arrValues = new Array();\n";
			js.Text += "arrValues = strValues.split(',');\n";

			js.Text += "region.value = arrValues[1];\n";
			js.Text += "regionid.value = arrValues[0];\n";

			js.Text += "}\n";

			js.Text += "</script>";

			head_ph.Controls.Add(js);
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
