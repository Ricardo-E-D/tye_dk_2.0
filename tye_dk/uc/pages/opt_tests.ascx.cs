namespace tye.uc.pages
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using exceptions;
	using tye;


	public partial class opt_tests : uc_pages
	{
		protected string strMode;
		protected int intId;
		protected int intStepId;
		protected int intImportant;
		protected int intCatCount;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			strMode = Request.QueryString["mode"];
			try {
				switch(strMode) {
					case "details":
						drawDetailsPage();
						break;
					default:
						drawTestList();
						break;
				}
			}
			catch(NoDataFound ndf) {
				this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId).ToString()));
			}
		}

		private void drawTestList()
		{
			Database db = new Database();

			string strSql = "SELECT id,tyename,name,isscreen FROM tests WHERE languageid = " + ((tye.Menu)Session["menu"]).IntLanguageId + " ORDER BY priority;";

			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;
				throw new NoDataFound();
			}
			
			HtmlTable objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt.Attributes["class"] = "data_table";
			objHt.ID = "data_table";

			while(objDr.Read())
			{
				HtmlTableRow objHtr = new HtmlTableRow();

				objHtr.Attributes["onmouseover"] = "hoverRow(this,'in');";
				objHtr.Attributes["onmouseout"] = "hoverRow(this,'out');";
				objHtr.Attributes["onclick"] = "location.href='?page=" + IntPageId + "&mode=details&id=" + objDr["id"].ToString() + "';";

				HtmlTableCell objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:170px;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["tyename"].ToString();

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:245px;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["name"].ToString();

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:20px;text-align:center;";
				objHtc.Attributes["class"] = "data_table_item";
									
				HtmlImage objI = new HtmlImage();

				if(Convert.ToInt32(objDr["isscreen"]) == 1)
				{
					objI.ID = "monitor_test_" + objDr["id"].ToString();
					objI.Src = "../../gfx/monitor_test.gif";
					objI.Alt = "Dette er en skærmøvelse";
					objI.Attributes["title"] = "Dette er en skærmøvelse";
				}
				else
				{
					objI.ID = "printed_test_" + objDr["id"].ToString();
					objI.Src = "../../gfx/printed_test.gif";
					objI.Alt = "Dette er en trykt øvelse";
					objI.Attributes["title"] = "Dette er en trykt øvelse";
				}
				
				objHtc.Controls.Add(objI);

				objHtr.Controls.Add(objHtc);
	
				objHt.Controls.Add(objHtr);
			}

			db.objDataReader.Close();
			db = null;

			this.Controls.Add(objHt);
		}

		private void drawDetailsPage()
		{
			intId = Convert.ToInt32(Request.QueryString["id"]);
			intStepId = Convert.ToInt32(Request.QueryString["step"]);
			intImportant = Convert.ToInt32(Request.QueryString["important"]);

			Database db = new Database();

			string strSql = "SELECT test_info_cat.id, tests_infos.body" +
				" FROM tests_infos" +
				" INNER JOIN test_info ON tests_infos.id = test_info.infoid" +
				" INNER JOIN test_info_cat ON test_info_cat.id = tests_infos.catid" +
				" WHERE test_info.testid = " + intId;

			MySqlDataReader objDr = db.select(strSql);

			Hashtable hashInfos= new Hashtable();

			while(objDr.Read())
			{
				if(!hashInfos.ContainsKey(Convert.ToInt32(objDr["id"])))
                {
                    hashInfos.Add(Convert.ToInt32(objDr["id"]),objDr["body"].ToString()); 
                }
			}

			db.objDataReader.Close();

			db = new Database();

			strSql = "SELECT name,tyename,purpose,isscreen,intro,important,requirement,priority,path FROM tests LEFT JOIN test_files ON fileid = test_files.id WHERE tests.id = " + Convert.ToInt32(Request.QueryString["id"]);

			objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;
				throw new FileNotFound();
			}

			if(objDr.Read())
			{
				if(!(Page.IsPostBack))
				{
					Page_header.InnerHtml = objDr["name"].ToString();
				}
			
				Session["tests"] = new Tests(objDr["name"].ToString(),intId,0,Convert.ToInt32(objDr["requirement"]),hashInfos,Request.UserHostAddress.ToString(),false,Convert.ToInt32(objDr["priority"]),-1);
				
				HtmlGenericControl purpose_div = new HtmlGenericControl("div");

				purpose_div.ID = "purpose_div";
				purpose_div.InnerHtml = objDr["purpose"].ToString();
				purpose_div.Attributes["class"] = "purpose_div";

				this.Controls.Add(purpose_div);
				
				HtmlGenericControl container_div_left = new HtmlGenericControl("div");

				container_div_left.ID = "container_div_left";
				container_div_left.Style.Add("float","left");
				container_div_left.Style.Add("margin-right","10px");
				
				this.Controls.Add(container_div_left);
				
				HtmlGenericControl panel_div = new HtmlGenericControl("div");

				panel_div.ID = "panel_div";
				panel_div.Attributes["class"] = "panel_div";
				panel_div.Style.Add("background","#8DACC0");

				HtmlAnchor intro_link = new HtmlAnchor();

				intro_link.ID = "intro_link";
				intro_link.HRef = "../../?page=" + IntPageId + "&mode=details&id=" + intId;
				if(hashInfos.Count > 2)
					intro_link.InnerHtml = hashInfos[2].ToString();
				
				panel_div.Controls.Add(intro_link);

				panel_div.Controls.Add(new LiteralControl(" | "));

				Database db_steps = new Database();

				string strSql_steps = "SELECT priority FROM tests_steps WHERE testid = " + intId + " ORDER BY priority;";

				MySqlDataReader objDr_steps = db_steps.select(strSql_steps);

				while(objDr_steps.Read())
				{
					HtmlAnchor step_link = new HtmlAnchor();

					step_link.ID = objDr_steps["priority"].ToString();
					step_link.HRef = "../../?page=" + IntPageId + "&mode=details&id=" + intId + "&step=" + Convert.ToInt32(objDr_steps["priority"]);
					step_link.InnerHtml = objDr_steps["priority"].ToString();

					panel_div.Controls.Add(step_link);

					panel_div.Controls.Add(new LiteralControl(" | "));
				}

				db_steps.objDataReader.Close();
				db_steps = null;

				HtmlAnchor important_link = new HtmlAnchor();

				important_link.ID = "important_link";
				important_link.HRef = "../../?page=" + IntPageId + "&mode=details&id=" + intId + "&important=1";
				if (hashInfos.Count > 3) 
					important_link.InnerHtml = hashInfos[3].ToString();
				
				panel_div.Controls.Add(important_link);				

				container_div_left.Controls.Add(panel_div);
				
				HtmlGenericControl content_div = new HtmlGenericControl("div");

				content_div.ID = "content_div";
				content_div.Attributes["class"] = "content_div";
				
				if(intStepId == 0 && intImportant == 0)
				{
					content_div.InnerHtml = objDr["intro"].ToString();
				}
				else if(intStepId != 0 && intImportant == 0)
				{
					Database db_step = new Database();

					string strSql_step = "SELECT body FROM tests_steps WHERE priority = " + intStepId + " AND testid = " + intId;

					MySqlDataReader objDr_step = db_step.select(strSql_step);

					if(objDr_step.Read())
					{
						content_div.InnerHtml = objDr_step["body"].ToString();
					}

					db_step.objDataReader.Close();
					db_step = null;
				}
				else if(intStepId == 0 && intImportant == 1)
				{
					content_div.InnerHtml = objDr["important"].ToString();
				}

				container_div_left.Controls.Add(content_div);				

				HtmlGenericControl container_div_right = new HtmlGenericControl("div");

				container_div_right.ID = "container_div_right";
				container_div_right.Style.Add("float","left");
				container_div_right.Style.Add("width","150px");

				this.Controls.Add(container_div_right);
				
				HtmlGenericControl links_div = new HtmlGenericControl("div");
				links_div.Style.Add("background","#B2CDDF");
				links_div.Style.Add("border","1px solid #819DAF");
				links_div.Style.Add("padding","5px");

				container_div_right.Controls.Add(links_div);

				HtmlAnchor back_link = new HtmlAnchor();

				back_link.ID = "back_link";
				if (hashInfos.Count > 1)
					back_link.InnerHtml = "<ul><li>" + hashInfos[1].ToString() + "</li>";
				back_link.HRef = "../../?page=" + IntPageId;

				links_div.Controls.Add(back_link);

				links_div.Controls.Add(new LiteralControl("&nbsp;"));

				HtmlAnchor start_link = new HtmlAnchor();

				start_link.ID = "start_link";
				if (hashInfos.Count > 5)
					start_link.InnerHtml = "<li>" + hashInfos[4].ToString() + "</li>";
				start_link.HRef = "javascript:void(0);";
				string strExerciseId = "intExerciseIdNo=" + intId;
				if (objDr["path"].ToString().IndexOf("?") > -1) {
					strExerciseId = "&" + strExerciseId;
				} else {
					strExerciseId = "?" + strExerciseId;
				}
				start_link.Attributes["onclick"] = "window.open('ScreenTasks/" + objDr["path"].ToString() + strExerciseId + "','','fullscreen=yes');";
				
				links_div.Controls.Add(start_link);

				if(hashInfos[24] != null) {
					HtmlAnchor metronom_link = new HtmlAnchor();

					metronom_link.ID = "metronom_link";
					metronom_link.InnerHtml = "<li>" + hashInfos[24].ToString() + "</li>";
					metronom_link.HRef = "http://www.tye.dk/download/Metronome.exe";
					//start_link.Attributes["onclick"] = "window.open('ScreenTasks/"+objDr["path"].ToString()+"','','fullscreen=yes');";
				
					links_div.Controls.Add(metronom_link);
				}

				if(Convert.ToInt32(objDr["isscreen"]) == 0)
				{
					start_link.Visible = false;
				}

				HtmlAnchor print_link = new HtmlAnchor();

				print_link.ID = "print_link";
				print_link.InnerHtml = "<li>Print</li></ul>";
				print_link.Attributes["onclick"] = "window.open('popups/Print.aspx?mode=instructions&type=single&id="+intId+"','print','width=630,height=580,toolbar=no,scrollbars=yes,resizeable=false');";
				print_link.HRef = "javascript:void(0)";
				links_div.Controls.Add(print_link);

				// exception for test "Harry's Blocks"
				if (intId == 290 || intId == 291 || intId == 292 || intId == 293) {
					HtmlAnchor link_AG = new HtmlAnchor();
					link_AG.ID = "link_AG";
					link_AG.InnerHtml = "<ul><li>";
					//((User)HttpContext.Current.Session["user"]).IntLanguageId;
					int intLang = ((tye.Menu)Session["menu"]).IntLanguageId;
					switch (intLang) {
						case (int)Shared.Language.Danish:
							link_AG.InnerHtml += "Print grafikker (A-G)";
							break;
						case (int)Shared.Language.German:
							link_AG.InnerHtml += "Druck Grafiken (A-G)";
							break;
						case (int)Shared.Language.English:
							link_AG.InnerHtml += "Print graphics (A-G)";
							break;
						case (int)Shared.Language.Norwegian:
							link_AG.InnerHtml += "Skriv ut grafikk (A-G)";
							break;
					}
					link_AG.InnerHtml += "</li></ul>";
					link_AG.Attributes["onclick"] = "window.open('popups/HarrysBlocks.aspx?lang=" + intLang + "','harry','width=630,height=580,toolbar=no,scrollbars=yes,resizeable=false');";
					link_AG.HRef = "javascript:void(0)";
					links_div.Controls.Add(link_AG);
				}
				// exception ends

			}
			hashInfos = null;

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
