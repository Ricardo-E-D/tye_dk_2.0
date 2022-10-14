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

	public partial class frontpage : uc_pages
	{
		protected int intPageId;
		protected Menu menu;	

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Database db = new Database();

			string strSql = "SELECT body FROM content WHERE menuid = " + IntPageId;

			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read())
			{
				this.Controls.Add(new LiteralControl(objDr["body"].ToString()));
			}
			else
			{
				this.Controls.Add(new LiteralControl("Intet indhold fundet i databasen..."));
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
	}
}