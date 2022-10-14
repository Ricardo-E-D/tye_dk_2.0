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
	public partial class Internal_link : System.Web.UI.Page
	{

		private int intLanguageId;
		protected System.Web.UI.WebControls.PlaceHolder ph;
		private int intPageId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				intPageId = Convert.ToInt32(Request.QueryString["pageid"]);

				try
				{
					if (Session["login"] == null)
					{
						throw new NoAccess();
					} 
					else
					{
						Login objLogin = (Login)Session["login"];

						if (!(objLogin.IntUserType == 4))
						{
							objLogin = null;
							throw new NoAccess();
						}
						objLogin = null;
					}
					

					Database db = new Database();

					string strSql = "SELECT language FROM menu WHERE id = " + intPageId;

					MySqlDataReader objDr = db.select(strSql);

					if (objDr.Read())
					{
						intLanguageId = Convert.ToInt32(objDr["language"]);
					}

					db.objDataReader.Close();
                    db.dbDispose();
                    objDr.Close();
					db = null;

					if (!(Page.IsPostBack))

					{
						Database db_pages = new Database();

						strSql = "SELECT id,name FROM menu WHERE isactive = 1 AND language = " + intLanguageId + " AND submenu = 0 AND access_surfer = 1;";

						MySqlDataReader objDr_pages  = db_pages .select(strSql);
					
						while (objDr_pages.Read())
						{
							HtmlAnchor na = new HtmlAnchor();

							na.HRef = "javascript:addLink('?page=" + objDr_pages["id"] + "');";	
							na.InnerHtml = "+ " + objDr_pages["name"].ToString();
							na.Attributes["class"] = "bold_text";

							il_body.Controls.Add(na);

							Literal nl = new Literal();

							nl.Text = "<br/>";

							il_body.Controls.Add(nl);

							Database db_subpages = new Database();

							strSql = "SELECT id,name FROM menu WHERE submenu = " + objDr_pages["id"];

							MySqlDataReader objDr_subpages = db_subpages.select(strSql);

							while (objDr_subpages.Read())
							{
								HtmlAnchor nasp = new HtmlAnchor();

								nasp.HRef = "javascript:addLink('?page=" + objDr_pages["id"] + "&submenu=" + objDr_subpages["id"] + "');";
								nasp.InnerHtml = "- " + objDr_subpages["name"].ToString();
								nasp.Attributes["class"] = "italic_text";

								il_body.Controls.Add(nasp);

								Literal nlsp = new Literal();

								nlsp.Text = "<br/>";

								il_body.Controls.Add(nlsp);
							}

							db_subpages.objDataReader.Close();
							db_subpages = null;
							objDr_subpages = null;

							nl = new Literal();

							nl.Text = "<br/><br/>";

							il_body.Controls.Add(nl);

						}


						db_pages.objDataReader.Close();
                        db_pages.dbDispose();
                        objDr_pages.Close();
						db_pages  = null;

					}
				
				}
				
				catch(NoAccess na)
				{
					Label nl = new Label();

					nl.Text = na.Message(((Menu)Session["menu"]).IntLanguageId);

					il_body.Controls.Add(nl);
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
	}
}
