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
using System.IO;

namespace tye.popups
{

	public partial class Admin : System.Web.UI.Page
	{
		protected HtmlGenericControl tab_content = new HtmlGenericControl("div");
		protected HtmlGenericControl insert_span = new HtmlGenericControl("span");
		protected HtmlGenericControl list_span = new HtmlGenericControl("span");
		protected TextBox name = new TextBox();
		protected TextBox map_name = new TextBox();
		private int intLanguageId;
		protected HtmlGenericControl tab_focus = new HtmlGenericControl();
		protected string strMode;
		protected string strFileName;
		protected string strFilePath;
		protected string strFileDesc;
		protected MySqlDataReader objDr;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				strMode = Request.QueryString["mode"];
				strFileName = Request.QueryString["filename"];

				drawTabs();

				switch(strMode)
				{
					case "image":
						drawImageAdmin();
						break;
					case "file":
						drawFileAdmin();
						break;
					case "region":
						drawRegionAdmin();
						break;
					case "deleteregion":
						deleteRegion();
						break;
					case "editregion":
						editRegion();
						break;
				}
			}
		}

		private void drawTabs()
		{
			list_span.ID = "list_span";
			list_span.Attributes["class"] = "tab_notfocus";
		
			admin_form.Controls.Add(list_span);

			insert_span.ID = "insert_span";
			insert_span.Attributes["class"] = "tab_notfocus";

			admin_form.Controls.Add(insert_span);

			tab_focus.ID = "tab_focus";
			admin_form.Controls.Add(tab_focus);

			tab_content.ID = "tab_content";
			admin_form.Controls.Add(tab_content);
		}

		private void drawImageAdmin()
		{	
			tab_content.Controls.Add(new LiteralControl("Klik på det billede du vil slette.<br/><br/>"));

			if (strFileName != null)
			{
				RemoveImgPerm(RemoveImgReference("[billede: " + strFileName + " /]",strFileName));
			}

			Literal js = new Literal();
			
			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function removeFile(filename)\n";
			js.Text += "{\n";
			js.Text += "var confirmed = window.confirm('Er du sikker på at du vil slette billedet?');\n";
			js.Text += "if (confirmed == true){\n";
			js.Text += "var textstr = opener.parent.document.main_form._"+Files.strCtl+"_content.value;\n";
			js.Text += "if (filename != '' && textstr != '')\n";
			js.Text += "{\n";
			js.Text += "var sT = textstr;\n";
			js.Text += "opener.parent.document.main_form._"+Files.strCtl+"_content.value = textstr.replace('[billede: '+filename+' /]','');\n";
			js.Text += "window.location.href = '?mode=image&amp;filename=' + filename;\n"; 
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			head_ph.Controls.Add(js);

			HtmlAnchor image_list = new HtmlAnchor();

			image_list.ID = "image_link";
			image_list.HRef = "List.aspx?mode=image";
			image_list.InnerHtml = "Indsæt billede";

			list_span.Controls.Add(image_list);

			HtmlAnchor upload_link = new HtmlAnchor();

			upload_link.ID = "upload_link";
			upload_link.HRef = "UploadFile.aspx?mode=image";
			upload_link.InnerHtml = "Upload billede";

			insert_span.Controls.Add(upload_link);
		
			tab_focus.InnerHtml = "Slet billede";	
		
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
				image.Attributes["onclick"] = "removeFile(\""+objDr["name"].ToString()+"\")";
				image.Attributes["class"] = "img_span";

				tab_content.Controls.Add(image);
			}
			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;
		}

		private void drawFileAdmin()
		{
			if (strFileName != null)
			{
				RemoveFilePerm(RemoveFileReference("[fil: " + strFileName + " /]",strFileName));
			}

			Literal js = new Literal();
				
			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function removeFile(filename)\n";
			js.Text += "{\n";
			js.Text += "var confirmed = window.confirm('Er du sikker på at du vil slette filen?');\n";
			js.Text += "if (confirmed == true){\n";
			js.Text += "var textstr = opener.parent.document.main_form._"+Files.strCtl+"_content.value;\n";
			js.Text += "if (filename != '' && textstr != '')\n";
			js.Text += "{\n";
			js.Text += "var sT = textstr;\n";
			js.Text += "opener.parent.document.main_form._"+Files.strCtl+"_content.value = textstr.replace('[fil: '+filename+' /]','');\n";
			js.Text += "window.alert(filename);\n";
			js.Text += "window.location.href = '?mode=file&amp;filename=' + filename;\n"; 
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			head_ph.Controls.Add(js);

			HtmlAnchor list_link = new HtmlAnchor();

			list_link.ID = "list_link";
			list_link.HRef = "List.aspx?mode=file";
			list_link.InnerHtml = "Indsæt fil";

			list_span.Controls.Add(list_link);

			HtmlAnchor insert_link = new HtmlAnchor();

			insert_link.ID = "insert_link";
			insert_link.HRef = "UploadFile.aspx?mode=file";
			insert_link.InnerHtml = "Upload fil";

			insert_span.Controls.Add(insert_link);
			
			tab_focus.InnerHtml = "Slet fil";

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
				objHlc.DataNavigateUrlFormatString = "javascript:removeFile(\"{0}\")";
				objHlc.HeaderText = "Slet fil:";
				objHlc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
				objHlc.Text = "Slet";
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
		}




		private string RemoveImgReference(string strTag,string strFileName) 
		{
			Database db = new Database();

			string strSql = "SELECT path,description FROM file_image WHERE name = '" + strFileName + "';";

			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read())
			{
				strFilePath = objDr["path"].ToString();	
				strFileDesc = objDr["description"].ToString();
			}

			db.objDataReader.Close();
            db.dbDispose();
            
			strSql = "SELECT id,body FROM content WHERE body like '%<img%';";

			objDr = db.select(strSql);

			while (objDr.Read())
			{
				Wysiwyg wys = new Wysiwyg();

				string strTempBody = objDr["body"].ToString().Replace("<img src='" + Files.strServerFilePath + strFilePath + "' id='" + strFileName + "' alt='" + strFileDesc + "' style='border: 0px;' />","");
							   
				Database db_update = new Database();

				string strSql_update = "UPDATE content SET body = '" + wys.ToDb(1,strTempBody) + "' WHERE id = " + Convert.ToInt32(objDr["id"]);
			
				db_update.execSql(strSql_update);

				wys = null;
                db_update.dbDispose();
				db_update = null;
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return strFilePath;

		}

		private void RemoveImgPerm(string strFilePath)
		{
			File.Delete(Files.strServerSavePath + strFilePath);

			File.Delete(Files.strServerSavePath + "thumb_" + strFilePath);

			Database db = new Database();

			string strSql = "DELETE FROM file_image WHERE path = '" + strFilePath + "';";

			db.execSql(strSql);
            db.dbDispose();
			db = null;
		}

		private string RemoveFileReference(string strTag,string strFileName) 
		{
			Response.Write(strFileName);

			Database db = new Database();

			string strSql = "SELECT name,path,description FROM file_files WHERE name = \"" + strFileName + "\";";

			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read())
			{
				strFilePath = objDr["path"].ToString();
				strFileName = objDr["name"].ToString();
				strFileDesc = objDr["description"].ToString();
			}

			db.objDataReader.Close();

			strSql = "SELECT id,body FROM content WHERE body like \"%<span id=\'" + strFileName + "\' class=\'file_link\'>%\";";

			objDr = db.select(strSql);

			while (objDr.Read())
			{
				Wysiwyg wys = new Wysiwyg();

				string strTempBody = objDr["body"].ToString().Replace("<span id='" + strFileName + "' class='file_link'><img src='gfx/pdf_icon.gif' alt='" + strFileDesc + "' id='" + strFileName + "' style='border:0px;vertical-align:middle;padding:3px;' /> <a href='" + Files.strServerFilePath + strFilePath + "'>" + strFileName + "</a></span>","");
				strTempBody = strTempBody.Replace("<span id='" + strFileName + "' class='file_link'><img src='gfx/doc_icon.gif' alt='" + strFileDesc + "' id='" + strFileName + "' style='border:0px;vertical-align:middle;padding:3px;' /> <a href='" + Files.strServerFilePath + strFilePath + "'>" + strFileName + "</a></span>","");
							   
				Database db_update = new Database();

				string strSql_update = "UPDATE content SET body = '" + wys.ToDb(1,strTempBody) + "' WHERE id = " + Convert.ToInt32(objDr["id"]);
			
				db_update.execSql(strSql_update);

				wys = null;	
				db_update = null;
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return strFilePath;

		}

		private void RemoveFilePerm(string strFilePath)
		{
			File.Delete(Files.strServerSavePath + strFilePath);

			Database db = new Database();

			string strSql = "DELETE FROM file_files WHERE path = '" + strFilePath + "';";

			db.execSql(strSql);
            db.dbDispose();
            
			db = null;

			Response.Redirect("Admin.aspx?mode=file");
		}

		private void drawRegionAdmin()
		{
			intLanguageId = Convert.ToInt32(Request.QueryString["language"]);
			
			tab_content.Controls.Add(new LiteralControl("Her kan du rette eller slette en region. (<span class='bold_text'>OBS:</span> hvis du sletter en region vil de optikere der er tilknytter denne region være uden en region.)<br/><br/>"));
			HtmlAnchor region_list = new HtmlAnchor();

			region_list.ID = "region_link";
			region_list.HRef = "List.aspx?mode=region&language="+intLanguageId;
			region_list.InnerHtml = "Vælg region";

			list_span.Controls.Add(region_list);

			HtmlAnchor insert_link = new HtmlAnchor();

			insert_link.ID = "insert_link";
			insert_link.HRef = "Insert.aspx?mode=region&language="+intLanguageId;
			insert_link.InnerHtml = "Tilføj ny region";

			insert_span.Controls.Add(insert_link);
			
			tab_focus.InnerHtml = "Administrer";

			Database db = new Database();

			string strSql = "SELECT map_region.id,map_region.name FROM map INNER JOIN map_region ON mapid = map.id WHERE languageid = "+intLanguageId;

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

			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "?mode=editregion&language="+intLanguageId+"&id={0}";
			objHlc.HeaderText = "";
			objHlc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			objHlc.Text = "Ret";
			objHlc.ItemStyle.CssClass = "data_table_item";
			objHlc.HeaderStyle.Width = 30;

			objDg.Columns.Add(objHlc);
			
			objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "?mode=deleteregion&language="+intLanguageId+"&id={0}";
			objHlc.HeaderText = "";
			objHlc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			objHlc.Text = "Slet";
			objHlc.ItemStyle.CssClass = "data_table_item";
			objHlc.HeaderStyle.Width = 30;

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
		}

		private void deleteRegion()
		{
			Database db = new Database();

			string strSql = "UPDATE user_optician SET regionid = 0 WHERE regionid = "+Convert.ToInt32(Request.QueryString["id"]);

			db.execSql(strSql);

			strSql = "DELETE FROM map_region WHERE id = "+Convert.ToInt32(Request.QueryString["id"]);

			db.execSql(strSql);
            db.dbDispose();
            db = null;

			Response.Redirect("Admin.aspx?mode=region&language="+Convert.ToInt32(Request.QueryString["language"]));
		}

		private void editRegion()
		{
			intLanguageId = Convert.ToInt32(Request.QueryString["language"]);
					
			HtmlAnchor list_link = new HtmlAnchor();

			list_link.ID = "list_link";
			list_link.HRef = "List.aspx?mode=region&language="+intLanguageId;
			list_link.InnerHtml = "Vælg region";

			list_span.Controls.Add(list_link);
            
			HtmlAnchor insert_link = new HtmlAnchor();

			insert_link.ID = "insert_link";
			insert_link.HRef = "Insert.aspx?mode=region&language="+intLanguageId;
			insert_link.InnerHtml = "Administrer";

			insert_span.Controls.Add(insert_link);

			tab_focus.InnerHtml = "Administrer";

			if(Session["noerror"] != null)
			{
				tab_content.Controls.Add(new LiteralControl(Session["noerror"].ToString()));

				Session["noerror"] = null;
			}

			Database db = new Database();

			string strSql = "SELECT map.name,map_region.name AS regname FROM map INNER JOIN map_region ON mapid = map.id WHERE map_region.id = " + Convert.ToInt32(Request.QueryString["id"]);

			objDr = db.select(strSql);

			if (objDr.Read())
			{
				map_name.Text = objDr["name"].ToString();
				name.Text = objDr["regname"].ToString();
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;
			
			tab_content.Controls.Add(new LiteralControl("Kortnavn: <br/>"));
	
			map_name.ID = "map_name";
			map_name.Width = 65;
			map_name.Attributes["style"] = "width:200px;";
			map_name.MaxLength = 255;
			map_name.ReadOnly = true;

			tab_content.Controls.Add(map_name);

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));

			tab_content.Controls.Add(new LiteralControl("Navn: * "));

			RequiredFieldValidator name_val = new RequiredFieldValidator();
			name_val.ID = "name_val";
			name_val.ControlToValidate = "name";
			name_val.ErrorMessage = "Feltet skal udfyldes.";
			name_val.Display = ValidatorDisplay.Dynamic;

			tab_content.Controls.Add(name_val);

			tab_content.Controls.Add(new LiteralControl("<br/>"));

			name.ID = "name";
			name.Width = 65;
			name.Attributes["style"] = "width:200px;";
			name.MaxLength = 255;

			tab_content.Controls.Add(name);

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = "Gem regionen";
			submit.Width = 65;
			submit.Attributes["style"] = "width:200px;";
			submit.Click +=new EventHandler(edit_region_submit);

			tab_content.Controls.Add(submit);
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

		private void edit_region_submit(object sender, EventArgs e)
		{
			Database db = new Database();
			Wysiwyg wys = new Wysiwyg();	

			string strSql = "UPDATE map_region SET name = '"+wys.ToDb(2,name.Text)+"' WHERE id = "+Convert.ToInt32(Request.QueryString["id"]);
			
			db.execSql(strSql);
            db.dbDispose();
			db = null;
			wys = null;

			Session["noerror"] = "<div id='noerror'>Du har nu rettet regionen.</div>";

			Response.Redirect("Admin.aspx?mode=editregion&language="+intLanguageId+"&id="+Convert.ToInt32(Request.QueryString["id"]));

		}
	}
}
