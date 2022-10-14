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

	public partial class Insert : System.Web.UI.Page
	{
		protected HtmlGenericControl tab_content = new HtmlGenericControl("div");
		protected HtmlGenericControl list_span = new HtmlGenericControl("span");
		protected HtmlGenericControl admin_span = new HtmlGenericControl("span");
		protected TextBox name = new TextBox();
		protected TextBox map_name = new TextBox();
		protected HtmlInputHidden map_id = new HtmlInputHidden();
		private int intLanguageId;
		protected HtmlGenericControl tab_focus = new HtmlGenericControl();
		protected string strMode;
		protected MySqlDataReader objDr;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				strMode = Request.QueryString["mode"];
				
				drawTabs();

				switch(strMode)
				{
					case "region":
						drawInsertForm();
						break;
					
				}
			}
		}

		private void drawTabs()
		{
			list_span.ID = "list_span";
			list_span.Attributes["class"] = "tab_notfocus";

			insert_form.Controls.Add(list_span);

			tab_focus.ID = "tab_focus";
			insert_form.Controls.Add(tab_focus);

			admin_span.ID = "admin_span";
			admin_span.Attributes["class"] = "tab_notfocus";
		
			insert_form.Controls.Add(admin_span);

			tab_content.ID = "tab_content";
			insert_form.Controls.Add(tab_content);
		}

		private void drawInsertForm()
		{
			intLanguageId = Convert.ToInt32(Request.QueryString["language"]);

			tab_focus.InnerHtml = "Tilføj ny region";
					
			HtmlAnchor list_link = new HtmlAnchor();

			list_link.ID = "list_link";
			list_link.HRef = "List.aspx?mode=region&language="+intLanguageId;
			list_link.InnerHtml = "Vælg region";

			list_span.Controls.Add(list_link);
            
			HtmlAnchor admin_link = new HtmlAnchor();

			admin_link.ID = "admin_link";
			admin_link.HRef = "Admin.aspx?mode=region&language="+intLanguageId;
			admin_link.InnerHtml = "Administrer";

			admin_span.Controls.Add(admin_link);

			if(Session["noerror"] != null)
			{
				tab_content.Controls.Add(new LiteralControl(Session["noerror"].ToString()));

				Session["noerror"] = null;
			}

			Database db = new Database();

			string strSql = "SELECT name,id FROM map WHERE languageid = " + intLanguageId;

			objDr = db.select(strSql);

			if (objDr.Read())
			{
				map_name.Text = objDr["name"].ToString();
				map_id.Value = objDr["id"].ToString();
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

			tab_content.Controls.Add(map_id);

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
			submit.Click +=new EventHandler(submit_Click);

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

		private void submit_Click(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				Database db = new Database();
				Wysiwyg wys = new Wysiwyg();

				string strSql = "INSERT INTO map_region (mapid,name) VALUES('"+map_id.Value+"','"+wys.ToDb(2,name.Text)+"')";

				db.execSql(strSql);
                db.dbDispose();
                
				Session["noerror"] = "<div id='noerror'>Der er nu oprettet en ny region.</div>";

				Response.Redirect("Insert.aspx?mode=region&language="+intLanguageId);
			}
		}
	}


}
