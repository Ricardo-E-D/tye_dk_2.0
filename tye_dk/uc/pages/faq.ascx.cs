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

	public partial class faq : uc_pages
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				Database db = new Database();
                string strSql = "SELECT question,answer,header FROM faq WHERE languageid = " + ((tye.Menu)Session["menu"]).IntLanguageId + " AND " + ((tye.Menu)Session["menu"]).StrAccess + " = 1 ORDER BY id;";
				MySqlDataReader objDr = db.select(strSql);

				if(!(objDr.HasRows))
				{
					throw new NoDataFound();
				}

				while(objDr.Read())
				{
					this.Controls.Add(new LiteralControl("<p><div class='page_subheader' style='margin-bottom: 15px;'>"+ objDr["header"].ToString() +"</div><div class='bold_text' style='width:475px;width:475px;'>" + objDr["question"] + "</div><div style='line-height:5px;'>&nbsp;</div><div class='italic_text' style='width:475px;'>" + objDr["answer"] + "</div></p><hr />"));
				}

				db.objDataReader.Close();
				db = null;
			}
			catch(NoDataFound ndf)
			{
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
