using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;

namespace tye
{
	/// <summary>
	/// Summary description for Error.
	/// </summary>
	public partial class Error : System.Web.UI.Page
	{
		Errors objError = new Errors();
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			Label lblHeader = new Label();
			Label lblTechHeader = new Label();
			Label lblDescription = new Label();
			Label lblTechDescription = new Label();
			Label lblToDo = new Label();
			
			int intLanguageId = Convert.ToInt32(Session["language_id"]);

			if(intLanguageId == 0)
			{
				intLanguageId = 1;
			}
			
			if (objError == null)
			{
				

				objError.IntErrorType = 5;
				objError.StrTechHeader = "TechHeader";
				objError.StrTechDescription = "TechDescription";
				objError.IntLine = 0;
				objError.IntReferer = 0;
			}
			
			Database db = new Database();

			string strSql = "SELECT header,description,todo FROM errors WHERE languageid = " + intLanguageId + " AND error_typeid = "+ objError.IntErrorType + ";";
			
			MySqlDataReader objDr = db.select(strSql);
			
			if (objDr.Read())
			{
				objError.StrHeader = objDr["header"].ToString();
				objError.StrDescription = objDr["description"].ToString();
				objError.StrToDo = objDr["todo"].ToString();
				lblHeader.Text = objError.StrHeader;
				lblTechHeader.Text = objError.StrTechHeader;
				lblDescription.Text = objError.StrDescription;
				lblTechDescription.Text = objError.StrTechDescription;
				lblToDo.Text = objError.StrToDo;

				error_ph.Controls.Add(lblHeader);
				error_ph.Controls.Add(lblTechHeader);
				error_ph.Controls.Add(lblDescription);
				error_ph.Controls.Add(lblTechDescription);
				error_ph.Controls.Add(lblToDo);
			}

			//rettet
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
