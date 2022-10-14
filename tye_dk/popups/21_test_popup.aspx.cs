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
using tye.exceptions;

namespace tye.popups
{
	public partial class _21_test_popup : System.Web.UI.Page
	{
		protected string strId = "";
		protected string strSql = "";

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				strId = Request.QueryString["id"];

				Database db = new Database();
				strSql = "SELECT tyeid,header,body FROM measuring_directions WHERE tyeid = '" + strId + "' AND languageid = " + ((User)Session["user"]).IntLanguageId;
				MySqlDataReader objDr = db.select(strSql);

				if(objDr.Read()){
					main_body.Controls.Add(new LiteralControl("<div class='bold_text' style='margin-bottom:5px;margin-top:15px;'>#" + objDr["tyeid"].ToString() + " " + objDr["header"].ToString() + ":</div>"));
					main_body.Controls.Add(new LiteralControl("<div>"+ objDr["body"].ToString() +"</div>"));
				}else{
					main_body.Controls.Add(new LiteralControl("aha..."));
				}

				db.objDataReader.Close();
                db.dbDispose();
                objDr.Close();
				db = null;
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
	}
}
