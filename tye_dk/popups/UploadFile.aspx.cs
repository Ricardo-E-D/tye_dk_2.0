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
using tye.exceptions;
using MySql.Data.MySqlClient;
using System.IO;

namespace tye.popups
{
	public partial class UploadFile : System.Web.UI.Page
	{
		protected string modeStr;
		protected HtmlInputFile hif = new HtmlInputFile();
		protected TextBox filename = new TextBox();
		protected TextBox fdescription = new TextBox();
		protected ListBox languageId = new ListBox();
		protected HtmlGenericControl tab_content = new HtmlGenericControl("div");
		protected HtmlGenericControl list_span = new HtmlGenericControl("span");
		protected HtmlGenericControl admin_span = new HtmlGenericControl("span");
		protected HtmlGenericControl tab_focus = new HtmlGenericControl();
		protected Label error = new Label();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				Server.ScriptTimeout = 600;
				
				modeStr = Request.QueryString["mode"];

				drawTabs();

				switch(modeStr)
				{
					case "image":
						drawImageUpload();
						break;
					case "file":
						drawFileUpload();
						break;
				}

				Label lblDemands = new Label();

				lblDemands.Text  = "<br/><br/>Krav til filer: <br/><br/>";
				lblDemands.Text += "Billeder:<br/>";
				lblDemands.Text += " - Typer: gif eller jpeg<br/>";
				lblDemands.Text += " - Dimentioner: max 640x480px<br/>";
				lblDemands.Text += " - Størrelse: max 500KB.<br/>";
				lblDemands.Text += "<br/>PDF'er:<br/>";
				lblDemands.Text += " - Typer: pdf<br/>";
				lblDemands.Text += "<br/>Dokumenter:<br/>";
				lblDemands.Text += " - Typer: doc<br/>";
				lblDemands.Text += " - Størrelse: max 500KB.<br/><br/><br/>";
					
				tab_content.Controls.Add(lblDemands);
			}
		}

		private void drawTabs()
		{
			list_span.ID = "list_span";
			list_span.Attributes["class"] = "tab_notfocus";

			upload_form.Controls.Add(list_span);

			tab_focus.ID = "tab_focus";
			upload_form.Controls.Add(tab_focus);

			admin_span.ID = "admin_span";
			admin_span.Attributes["class"] = "tab_notfocus";
		
			upload_form.Controls.Add(admin_span);

			tab_content.ID = "tab_content";
			upload_form.Controls.Add(tab_content);

			error.ID = "errormsg";
			tab_content.Controls.Add(error);

		}

		private void drawFileUpload()
		{
			HtmlAnchor upload_link = new HtmlAnchor();

			upload_link.ID = "upload_link";
			upload_link.HRef = "List.aspx?mode=file";
			upload_link.InnerHtml = "Indsæt fil";

			list_span.Controls.Add(upload_link);

			tab_focus.InnerHtml = "Upload fil";

			HtmlAnchor admin_link = new HtmlAnchor();

			admin_link.ID = "admin_link";
			admin_link.HRef = "Admin.aspx?mode=file";
			admin_link.InnerHtml = "Slet fil";

			admin_span.Controls.Add(admin_link);

			if (Session["noerror"] != null)
			{
				tab_content.Controls.Add(new LiteralControl(Session["noerror"].ToString()));

				Session["noerror"] = null;
			}
				
			tab_content.Controls.Add(new LiteralControl("Vælg fil: * "));

			RequiredFieldValidator inputfile_val = new RequiredFieldValidator();

			inputfile_val.ID = "inputfile_val";
			inputfile_val.ControlToValidate = "inputfile";
			inputfile_val.ErrorMessage = "Du skal vælge en fil!";
					
			tab_content.Controls.Add(inputfile_val);

			tab_content.Controls.Add(new LiteralControl("<br/>"));

			hif.ID = "inputfile";
			hif.Size = 43;
			hif.Attributes["style"] = "width:300px;";
				                
			tab_content.Controls.Add(hif);

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));
			
			tab_content.Controls.Add(new LiteralControl("Sprog: * <br/>"));

			Database db = new Database();

			string strSql = "SELECT id,name FROM language ORDER BY id;";

			languageId.DataSource = db.select(strSql);
			languageId.DataTextField = "name";
			languageId.DataValueField = "id";

			languageId.ID = "languageid";
			languageId.Rows = 1;
			
			languageId.DataBind();

			tab_content.Controls.Add(languageId);
		
			db.objDataReader.Close();
            db.dbDispose();
			db = null;

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));

			tab_content.Controls.Add(new LiteralControl("Filnavn: * "));

			RequiredFieldValidator filename_val = new RequiredFieldValidator();

			filename_val.ID = "filename_val";
			filename_val.ControlToValidate = "filename";
			filename_val.ErrorMessage = "Dette felt skal udfyldes!";
			filename_val.Display = ValidatorDisplay.Dynamic;

			tab_content.Controls.Add(filename_val);

			CustomValidator filename_exist = new CustomValidator();

			filename_exist.ControlToValidate = "filename";
			filename_exist.ID = "filename_exist";
			filename_exist.ServerValidate += new ServerValidateEventHandler(filename_exist_ServerValidate);
			filename_exist.ErrorMessage = "Filnavnet eksisterer i forevejen, vælg et andet!";

			tab_content.Controls.Add(filename_exist);

			tab_content.Controls.Add(new LiteralControl("<br/>"));

			filename.ID = "filename";
			filename.Width = 300;
			filename.Attributes["style"] = "width:300px;";

			tab_content.Controls.Add(filename);

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));

			tab_content.Controls.Add(new LiteralControl("Beskrivelse: * "));

			RequiredFieldValidator fdescription_val = new RequiredFieldValidator();

			fdescription_val.ID = "fdescription_val";
			fdescription_val.ControlToValidate = "fdescription";
			fdescription_val.ErrorMessage = "Dette felt skal udfyldes!";
			fdescription_val.Display = ValidatorDisplay.Dynamic;
			
			tab_content.Controls.Add(fdescription_val);

			tab_content.Controls.Add(new LiteralControl("<br/>"));

			fdescription.ID = "fdescription";
			fdescription.Columns = 30;
			fdescription.Rows = 10;
			fdescription.Attributes["style"] = "width:300px;height:100px;";

			tab_content.Controls.Add(fdescription);

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));

			Button submit = new Button();
					
			submit.ID = "submit";
			submit.Text = "Upload filen";
			submit.Click += new EventHandler(doFileUpload);
			submit.Attributes["style"] = "width:300px;";
						
			tab_content.Controls.Add(submit);
		}

		private void drawImageUpload()
		{
			HtmlAnchor upload_link = new HtmlAnchor();

			upload_link.ID = "upload_link";
			upload_link.HRef = "List.aspx?mode=image";
			upload_link.InnerHtml = "Indsæt billede";

			list_span.Controls.Add(upload_link);

			tab_focus.InnerHtml = "Upload billede";

			HtmlAnchor admin_link = new HtmlAnchor();

			admin_link.ID = "admin_link";
			admin_link.HRef = "Admin.aspx?mode=image";
			admin_link.InnerHtml = "Slet billede";

			admin_span.Controls.Add(admin_link);

			if (Session["noerror"] != null)
			{
				tab_content.Controls.Add(new LiteralControl(Session["noerror"].ToString()));

				Session["noerror"] = null;
			}
				
			tab_content.Controls.Add(new LiteralControl("Vælg fil: * "));

			RequiredFieldValidator inputfile_val = new RequiredFieldValidator();

			inputfile_val.ID = "inputfile_val";
			inputfile_val.ControlToValidate = "inputfile";
			inputfile_val.ErrorMessage = "Du skal vælge en fil!";
					
			tab_content.Controls.Add(inputfile_val);

			tab_content.Controls.Add(new LiteralControl("<br/>"));

			hif.ID = "inputfile";
			hif.Size = 43;
			hif.Attributes["style"] = "width:300px;";
				                
			tab_content.Controls.Add(hif);

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));

			tab_content.Controls.Add(new LiteralControl("Filnavn: * "));

			RequiredFieldValidator filename_val = new RequiredFieldValidator();

			filename_val.ID = "filename_val";
			filename_val.ControlToValidate = "filename";
			filename_val.ErrorMessage = "Dette felt skal udfyldes!";
			filename_val.Display = ValidatorDisplay.Dynamic;

			tab_content.Controls.Add(filename_val);

			CustomValidator filename_exist = new CustomValidator();

			filename_exist.ControlToValidate = "filename";
			filename_exist.ID = "filename_exist";
			filename_exist.ServerValidate += new ServerValidateEventHandler(imgname_exist_ServerValidate);
			filename_exist.ErrorMessage = "Filnavnet eksisterer i forevejen, vælg et andet!";

			tab_content.Controls.Add(filename_exist);

			tab_content.Controls.Add(new LiteralControl("<br/>"));

			filename.ID = "filename";
			filename.Width = 300;
			filename.Attributes["style"] = "width:300px;";

			tab_content.Controls.Add(filename);

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));

			tab_content.Controls.Add(new LiteralControl("Beskrivelse: * "));

			RequiredFieldValidator fdescription_val = new RequiredFieldValidator();

			fdescription_val.ID = "fdescription_val";
			fdescription_val.ControlToValidate = "fdescription";
			fdescription_val.ErrorMessage = "Dette felt skal udfyldes!";
			fdescription_val.Display = ValidatorDisplay.Dynamic;
			
			tab_content.Controls.Add(fdescription_val);

			tab_content.Controls.Add(new LiteralControl("<br/>"));

			fdescription.ID = "fdescription";
			fdescription.Columns = 30;
			fdescription.Rows = 10;
			fdescription.Attributes["style"] = "width:300px;height:100px;";

			tab_content.Controls.Add(fdescription);

			tab_content.Controls.Add(new LiteralControl("<br/><br/>"));

			Button submit = new Button();
					
			submit.ID = "submit";
			submit.Text = "Upload filen";
			submit.Click += new EventHandler(doImgUpload);
			submit.Attributes["style"] = "width:300px;";
						
			tab_content.Controls.Add(submit);
		}

		public void doImgUpload(object Sender, EventArgs E)
		{
			if (Page.IsValid)
			{
				Files file = new Files();

				file.ObjFileInput = hif.PostedFile;
				file.StrDescription = fdescription.Text;
				file.StrName = filename.Text;
				file.StrFileType = "image";

				try
				{
					file.upload();

					file = null;

					Response.Redirect("UploadFile.aspx?mode=image");
				}
				catch(UploadError ue)
				{
					file = null;

					error.Text = ue.Message(((Menu)Session["menu"]).IntLanguageId).ToString();
				}
			}

		}

		public void doFileUpload(object Sender, EventArgs E)
		{
			if (Page.IsValid)
			{
				Files file = new Files();

				file.ObjFileInput = hif.PostedFile;
				file.StrDescription = fdescription.Text;
				file.StrName = filename.Text;
				file.IntLanguageId = Convert.ToInt32(languageId.SelectedValue);
				file.StrFileType = "application";

				try
				{
					file.upload();

					file = null;

					Response.Redirect("UploadFile.aspx?mode=file");
				}
				catch(UploadError ue)
				{
					file = null;

					error.Text = ue.Message(((Menu)Session["menu"]).IntLanguageId).ToString();
				}
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

		private void imgname_exist_ServerValidate(object source, ServerValidateEventArgs args)
		{
			Database db = new Database();

			string strSql = "SELECT COUNT(*) AS found FROM file_image WHERE name = '" + filename.Text + "';";

			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read())
			{
				args.IsValid = (Convert.ToInt32(objDr["found"]) == 0);
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

		}

		private void filename_exist_ServerValidate(object source, ServerValidateEventArgs args)
		{
			Database db = new Database();

			string strSql = "SELECT COUNT(*) AS found FROM file_files WHERE name = '" + filename.Text + "';";

			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read())
			{
				args.IsValid = (Convert.ToInt32(objDr["found"]) == 0);
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

		}
	}
}
