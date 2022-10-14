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
	public class AdminFile : System.Web.UI.Page
	{
		protected string strMode;
		protected string strFileName;
		private string strFilePath;
		private string strFileDesc;
		protected HtmlGenericControl image_body;

		protected HtmlGenericControl tab_content = new HtmlGenericControl("div");
		protected HtmlGenericControl insert_span = new HtmlGenericControl("span");
		protected HtmlGenericControl list_span = new HtmlGenericControl("span");
		protected HtmlGenericControl tab_focus = new HtmlGenericControl();

		protected MySqlDataReader objDr;
		protected PlaceHolder head_ph;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				strMode = Request.QueryString["mode"];
				strFileName = Request.QueryString["filename"];

				drawTabs();

				switch(strMode)
				{
					case "image":
						drawImgAdmin();
						break;
					case "file":
						drawFileAdmin();
						break;
				}
			}
		}

		private void drawTabs()
		{
			list_span.ID = "list_span";
			list_span.Attributes["class"] = "tab_notfocus";
		
			image_body.Controls.Add(list_span);

			insert_span.ID = "insert_span";
			insert_span.Attributes["class"] = "tab_notfocus";

			image_body.Controls.Add(insert_span);

			tab_focus.ID = "tab_focus";
			image_body.Controls.Add(tab_focus);

			tab_content.ID = "tab_content";
			image_body.Controls.Add(tab_content);
		}

		private void drawImgAdmin()
		{
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
			js.Text += "var textstr = opener.parent.document.main_form._ctl0_content.value;\n";
			js.Text += "if (filename != '' && textstr != '')\n";
			js.Text += "{\n";
			js.Text += "var sT = textstr;\n";
			js.Text += "opener.parent.document.main_form._ctl0_content.value = textstr.replace('[billede: '+filename+' /]','');\n";
			js.Text += "window.location.href = '?mode=image&amp;filename=' + filename;\n"; 
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			head_ph.Controls.Add(js);

			HtmlAnchor list_link = new HtmlAnchor();

			list_link.ID = "list_link";
			list_link.HRef = "List.aspx?mode=image";
			list_link.InnerHtml = "Indsæt billede";

			list_span.Controls.Add(list_link);

			HtmlAnchor insert_link = new HtmlAnchor();

			insert_link.ID = "insert_link";
			insert_link.HRef = "UploadFile.aspx?mode=file";
			insert_link.InnerHtml = "Upload billede";

			insert_span.Controls.Add(insert_link);
			
			tab_focus.InnerHtml = "Slet billede";

			Database db = new Database();

			string strSql = "SELECT id,name,path,description FROM file_image ORDER BY addedtime DESC;";

			objDr = db.select(strSql);	

			while(objDr.Read())
			{
				HtmlImage image = new HtmlImage();

				image.Alt = objDr["description"].ToString();
				image.ID = objDr["id"].ToString();
				image.Src = Files.strServerFilePath + "thumb_" + objDr["path"].ToString();
				image.Attributes["onclick"] = "removeFile('"+objDr["name"].ToString()+"')";
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
			js.Text += "var textstr = opener.parent.document.main_form._ctl0_content.value;\n";
			js.Text += "if (filename != '' && textstr != '')\n";
			js.Text += "{\n";
			js.Text += "var sT = textstr;\n";
			js.Text += "opener.parent.document.main_form._ctl0_content.value = textstr.replace('[fil: '+filename+' /]','');\n";
			js.Text += "window.alert(filename);\n";
			js.Text += "window.location.href = '?mode=file&amp;filename=' + filename;\n"; 
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>\n";

			head_ph.Controls.Add(js);

			HtmlAnchor list_link = new HtmlAnchor();

			list_link.ID = "list_link";
			list_link.HRef = "List.aspx?mode=image";
			list_link.InnerHtml = "Indsæt fil";

			list_span.Controls.Add(list_link);

			HtmlAnchor insert_link = new HtmlAnchor();

			insert_link.ID = "insert_link";
			insert_link.HRef = "UploadFile.aspx?mode=file";
			insert_link.InnerHtml = "Upload fil";

			insert_span.Controls.Add(insert_link);
			
			tab_focus.InnerHtml = "Slet fil";

			Database db = new Database();

			string strSql = "SELECT file_files.id,file_files.name,path,description,language.name as lname FROM file_files INNER JOIN language ON languageid = language.id ORDER BY addedtime DESC;";

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
				objHlc.DataNavigateUrlFormatString = "javascript:removeFile('{0}')";
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
            objDr.Close();

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

			string strSql = "SELECT path,description FROM file_files WHERE name = '" + strFileName + "';";

			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read())
			{
				strFilePath = objDr["path"].ToString();	
				strFileDesc = objDr["description"].ToString();
			}

			if(!(objDr.HasRows))
			{
				Response.Write("HEST!");
			}

			db.objDataReader.Close();

			strSql = "SELECT id,body FROM content WHERE body like '%<img%';";

			objDr = db.select(strSql);

			while (objDr.Read())
			{
				//Wysiwyg wys = new Wysiwyg();

				//string strTempBody = objDr["body"].ToString().Replace("<img src='" + Files.strServerFilePath + strFilePath + "' id='" + strFileName + "' alt='" + strFileDesc + "' style='border: 0px;' />","");
							   
				//Database db_update = new Database();

				//string strSql_update = "UPDATE content SET body = '" + wys.ToDb(1,strTempBody) + "' WHERE id = " + Convert.ToInt32(objDr["id"]);
			
				//db_update.execSql(strSql_update);

				//wys = null;	
				//db_update = null;
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

			Response.Redirect("AdminFile.aspx?mode=file");
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}

}
