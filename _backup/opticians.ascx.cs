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

	public partial class opticians : uc_pages
	{

		private ListBox lbRegions = new ListBox();
		private ListBox lbOpticians = new ListBox();
		private string strRegionId;
		private int intOpticianId;
		private string strMode;
		protected string[] arrInfos;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			strRegionId = Request.QueryString["region"];
			intOpticianId = Convert.ToInt32(Request.QueryString["optician"]);
			strMode = Request.QueryString["mode"];

			try
			{
				Database db = new Database();

				string strSql = "SELECT body FROM content WHERE menuid = " + IntSubmenuId;

				MySqlDataReader objDr = db.select(strSql);

				if(objDr.Read())
				{
					arrInfos = objDr["body"].ToString().Split(Convert.ToChar(","));
				}

				db.objDataReader.Close();
				db = null;

				switch(strMode)
				{
					case "details":
						drawDetails();
						break;
					default:
						drawMap();
						break;
				}
			}
			catch(NoDataFound ndf)
			{
				this.Controls.Clear();
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId)));
			}

		}

		private void drawDetails()
		{
			Database db = new Database();

			string strSql = "SELECT name,address,zipcode,city,phone,email FROM users INNER JOIN user_optician ON users.id = userid WHERE users.id = " + intOpticianId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				this.Controls.Add(new LiteralControl("<p><span class='bold_text'>" + objDr["name"] + "</span><br/>" + objDr["address"] + "<br/>" + objDr["zipcode"] + " " + objDr["city"] + "<br/>" + objDr["phone"] + "<br/>" + objDr["email"] + "</p>"));

				HtmlAnchor back_link = new HtmlAnchor();

				back_link.ID = "back_link";
				back_link.HRef = "../../?page=" + IntPageId + "&region=" + strRegionId;
				back_link.InnerHtml = arrInfos[4].ToString();

				this.Controls.Add(back_link);
			}

			db.objDataReader.Close();
			db = null;
		}

		private void drawMap()
		{
			Database db = new Database();

            string strSql = "SELECT mappath,mapalt,isactive FROM language WHERE id = " + ((tye.Menu)Session["menu"]).IntLanguageId;

			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;

				throw new NoDataFound();
			}

			if(objDr.Read())
			{
				if(Convert.ToInt32(objDr["isactive"]) == 0)
				{
					this.Controls.Add(new LiteralControl(arrInfos[5].ToString()));
				}
				else
				{
					HtmlImage objMap = new HtmlImage();

					objMap.Src = "../../gfx/" + objDr["mappath"].ToString();
					objMap.Alt = objDr["mapalt"].ToString();
					objMap.Attributes["title"] = objDr["mapalt"].ToString();
					objMap.ID = "map";
					objMap.Attributes["class"] = "object_left";

					this.Controls.Add(objMap);

					HtmlGenericControl listbox_div = new HtmlGenericControl("div");

					listbox_div.ID = "listbox_div";
					listbox_div.Style.Add("width","200px");
					listbox_div.Style.Add("margin-left","50px");
					listbox_div.Attributes["class"] = "object_left";

					this.Controls.Add(listbox_div);
					
					listbox_div.Controls.Add(new LiteralControl(arrInfos[0].ToString() + "<br/>"));

					lbRegions.ID = "lbRegions";
					lbRegions.Rows = 1;
					lbRegions.Attributes["onchange"] = "window.location='?page=" + IntPageId + "&region=' + this.value";
				
					Database db_lb = new Database();

					// Ændring jb. Task 9.

                    string strSql_lb = "SELECT map_region.id,map_region.name FROM map_region INNER JOIN map ON mapid = map.id WHERE languageid = " + ((tye.Menu)Session["menu"]).IntLanguageId + " AND map_region.name <> 'Udenfor region' ORDER BY map_region.name;";

					lbRegions.DataSource = db_lb.select(strSql_lb);
					lbRegions.DataTextField = "name";
					lbRegions.DataValueField = "id";
					lbRegions.DataBind();

					ListItem objLi = new ListItem();

					objLi.Value = "0";
					objLi.Text = arrInfos[2].ToString();
					objLi.Selected = true;

					lbRegions.Items.Insert(0,objLi);

					objLi = null;

					listbox_div.Controls.Add(lbRegions);

					db_lb.objDataReader.Close();

					listbox_div.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[1].ToString() + "<br/>"));

					lbOpticians.ID = "lbOpticians";
					lbOpticians.Rows = 1;
					lbOpticians.Attributes["onchange"] = "window.location='?page=" + IntPageId + "&mode=details&region=" + strRegionId + "&optician=' + this.value";

					if(Convert.ToInt32(strRegionId) > 0)
					{
						lbRegions.SelectedValue = strRegionId;
					
						db_lb = new Database();

						int currLanguage = 0;
                  try { currLanguage = ((tye.Menu)Session["menu"]).IntLanguageId; }
                  catch { } 
                  
                  if (currLanguage == 4)
                  {
					  strSql_lb = "SELECT users.id,CONCAT(users.zipcode, ' ', users.city, ', ', user_optician.name) AS optname FROM users INNER JOIN user_optician ON users.id = user_optician.userid WHERE regionid = " + Convert.ToInt32(strRegionId) + " ORDER BY zipcode";
                  }
                  else
                  {
                      strSql_lb = "SELECT users.id,CONCAT(user_optician.name,', ',users.city) AS optname FROM users INNER JOIN user_optician ON users.id = user_optician.userid WHERE regionid = " + Convert.ToInt32(strRegionId) + " ORDER BY users.city, user_optician.name";
                  }

						lbOpticians.DataSource = db_lb.select(strSql_lb);
						lbOpticians.DataTextField = "optname";
						lbOpticians.DataValueField = "id";
						lbOpticians.DataBind();

						objLi = new ListItem();

						objLi.Value = "0";
						objLi.Text = arrInfos[3].ToString();
						objLi.Selected = true;

						lbOpticians.Items.Insert(0,objLi);

						db_lb.objDataReader.Close();
						db_lb = null;
					}
					else
					{
						lbOpticians.Enabled = false;
						lbOpticians.Style.Add("width","105px");
					}

					listbox_div.Controls.Add(lbOpticians);
				}
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
