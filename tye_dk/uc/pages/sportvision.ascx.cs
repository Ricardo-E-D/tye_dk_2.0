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

	public partial class sportvision : uc_pages
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Database db = new Database();

				string strSql = "SELECT body FROM content WHERE menuid = " + IntSubmenuId;

				MySqlDataReader objDr = db.select(strSql);

				if(!(objDr.HasRows))
				{
					db.objDataReader.Close();
					db = null;
					throw new NoDataFound();
				}

				if(objDr.Read())
				{
					this.Controls.Add(new LiteralControl(objDr["body"].ToString()));
				}

				//rettet
				db.objDataReader.Close();
				db = null;
			}
			catch(NoDataFound ndf)
			{
				this.Controls.Clear();
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId).ToString()));
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
