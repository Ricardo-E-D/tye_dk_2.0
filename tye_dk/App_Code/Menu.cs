using System;
using System.Data.Odbc;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace tye
{
	public class Menu : System.Web.UI.Page
	{
		private int intLanguageId;
		private string strAccess;
		private int[] arrPageAccess;

		public Menu()
		{
			
		}
	
		public int IntLanguageId
		{
			get
			{
				return intLanguageId;
			}

			set
			{
				intLanguageId = value;
			}
		}

		public string StrAccess
		{
			get
			{
				return strAccess;
			}

			set
			{
				strAccess = value;
			}
		}

		public int[] ArrPageAccess 	{
			get	{ return arrPageAccess; }
			set { arrPageAccess = value; }
		}

		public void FillMenu(int intPageId,HtmlTableCell menu_td)
		{
			Database db = new Database();

			int intSelectLang = intLanguageId; // for distributors. Not all admin menu items have been created for all languages
			if(Session["user"] != null) {
				if (((User)Session["user"]).IsDistributor) // && intSelectLang != (int)Shared.Language.German) // menu items do in fact exist for german
				    intSelectLang = (int)Shared.Language.Danish;
			}
			string strSql = "SELECT id, name, isvisible" +
							" FROM menu" +
							" WHERE language = " + intSelectLang + 
							" AND submenu = 0 AND isactive = 1" +
							" AND " + StrAccess + " = 1" +
							" ORDER BY preference;";		

			MySqlDataReader objDr = db.select(strSql);			

			int i = new int();
			i = 0;

			int[] tempArr = new int[15];

			while (objDr.Read() == true && i < 15)
			{
				if(Convert.ToInt32(objDr["isvisible"]) == 1){				
					HyperLink link = new HyperLink();
					link.ID = "link_" + objDr["id"];
					link.Text = objDr["name"].ToString();
					link.NavigateUrl = "Default.aspx?page="+objDr["id"];

					if (intPageId == Convert.ToInt32(objDr["id"])) {
						link.CssClass = "menu_chosen";
					}
					else {				
						link.CssClass = "menu";
					}

					menu_td.Controls.Add(link);
				}
				tempArr[i] = Convert.ToInt32(objDr["id"]);
				i++;
			
			}

			ArrPageAccess = tempArr;

			tempArr = null;
			
			db.objDataReader.Close();
            db.dbDispose();
			db = null;
		}
	
		public void FillSubmenu(int intPageId,int intSubmenuId, HtmlTableCell submenu_td)
		{
			if (intPageId == 0)
				return;

			int intSelectLang = intLanguageId; // for distributors. Not all admin menu items have been created for all languages
			if (Session["user"] != null) {
				if (((User)Session["user"]).IsDistributor) // && intSelectLang != (int)Shared.Language.German) // menu items do in fact exist for german
					intSelectLang = (int)Shared.Language.Danish;
			}

			Database db = new Database();
			string strSql = "SELECT id,name FROM menu WHERE language = " + intSelectLang + 
							" AND submenu = " + intPageId + 
							" AND isactive = 1" + 
							" AND " + StrAccess + " = 1" + 
							" ORDER BY preference;";		
			MySqlDataReader objDr = db.select(strSql);

			while (objDr.Read() == true)
			{	
				HyperLink link = new HyperLink();
				link.ID = "link_" + objDr["id"];
				link.Text = objDr["name"].ToString();
				link.NavigateUrl = "Default.aspx?page="+intPageId+"&submenu="+objDr["id"];
				
				if (intSubmenuId == Convert.ToInt32(objDr["id"]))
				{
					link.CssClass = "submenu_chosen";
				}
				else
				{				
					link.CssClass = "submenu";
				}

				submenu_td.Controls.Add(link);
			}	
	
			db.objDataReader.Close();
            db.dbDispose();
			db = null;
		}

		public int getFirstId(int intPageId)
		{
			// jb Oversættes til tysk
			if (intPageId == 0)
			{
				switch (Convert.ToInt32(((Login)Session["login"]).IntUserType))
				{
					case 1:
						switch (Convert.ToInt32(((Menu)Session["menu"]).IntLanguageId))
						{
							case 1:
								intPageId = 1;
								break;
							
							case 2:
								intPageId = 29;
								break;
							
							case 3:
								intPageId = 57;
								break;
							case 4:
								intPageId = 1141;
								break;
						}						
						break;

					case 2:
							switch ( ((Menu)Session["menu"]).IntLanguageId)
							{
								case 1:
									intPageId = 100;
									break;
								case 2:
									intPageId = 101;
									break;
								case 3:
									intPageId = 102;
									break;
								case 4:
									intPageId = 1177;
									break;
							}
							break;

					case 3:
						switch (Convert.ToInt32(((Menu)Session["menu"]).IntLanguageId))
						{
							case 1:
								intPageId = 124;
								break;
							case 2:
								intPageId = 125;
								break;
							case 3:
								intPageId = 126;
								break;
							case 4:
								intPageId = 1184;
								break;
						}
							break;
					case 4:
						switch (Convert.ToInt32(((Menu)Session["menu"]).IntLanguageId))
						{
							case 1:
								intPageId = 86;
								break;
							case 2:
								intPageId = 127;
								break;
							case 3:
								intPageId = 141;
								break;
							case 4:
								intPageId = 1196;
								break;
						}
							break;
					case 5:
						switch (Convert.ToInt32(((Menu)Session["menu"]).IntLanguageId)) {
							case 1: // dansk
								intPageId = 92;
								break;
							case 2: // Norsk
								intPageId = 1212;
								break;
							case 3: // Engelsk
								intPageId = 1211;
								break;
							case 4: // Tysk
								intPageId = 1213;
								break;
						}
						break;
				}
			}
			return intPageId;
		}

		public int getFirstSubmenuId(int intPageId)
		{
			Database db = new Database();
			string strSql = "SELECT id" +
						    " FROM menu" +
							" WHERE submenu = " + intPageId + " AND isvisible = 1" + //" AND " + StrAccess + " = 1" +
							" ORDER BY preference LIMIT 0,1";
			int tempId;

			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read())
			{
				tempId = Convert.ToInt32(objDr["id"]);
			}
			else
			{
				tempId = 0;
			}

			db.objDataReader.Close();
            db.dbDispose();
			db = null;
			return tempId;
		}

	}
}
