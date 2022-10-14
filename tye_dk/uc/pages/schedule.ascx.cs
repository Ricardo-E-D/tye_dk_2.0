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

	public partial class schedule : uc_pages
	{

		protected string strMode;
		protected int intId;
		protected int intStepId;
		protected int intImportant;
		protected int intCatCount;
		protected int intLock;
		protected int intProgramId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			strMode = Request.QueryString["mode"];

			try {
				switch(strMode) {
					case "details":
						//if (Shared.debug)
						//    Response.Write("drawDetailsPage()");
						drawDetailsPage();
						break;
					default:
						//if (Shared.debug)
						//    Response.Write("drawTestList()");
						Literal js = new Literal();
						drawTestList();
						break;
				}
			}
			catch(NoDataFound ndf) {
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId).ToString()));
			}
		}

		//private void linkbutton_click_text_exercise(object sender, EventArgs e) {
		//    LinkButton lnk = (LinkButton)sender;
		//    if (lnk == null)
		//        return;
		//    int intIsScreen = Convert.ToInt32(lnk.Attributes["isscreen"].ToString());
		//    string strMode = lnk.Attributes["mode"].ToString();
		//    string strProgram = lnk.Attributes["program"].ToString();
		//    string strId = lnk.Attributes["id"].ToString();
		//    string strPage = lnk.Attributes["page"].ToString();
			
		//    if(intIsScreen == 0) {
		//        Database db = new Database();
		//        string strSql = "SELECT test_info_cat.id, tests_infos.body FROM tests_infos INNER JOIN" +
		//            " test_info ON tests_infos.id = test_info.infoid INNER JOIN" +
		//            " test_info_cat ON test_info_cat.id = tests_infos.catid WHERE test_info.testid = " + strId;
		//        MySqlDataReader objDr = db.select(strSql);
		//        Hashtable hashInfos = new Hashtable();
		//        while (objDr.Read()) {
		//            if (!hashInfos.ContainsKey(Convert.ToInt32(objDr["id"]))) {
		//                hashInfos.Add(Convert.ToInt32(objDr["id"]), objDr["body"].ToString());
		//            }
		//        }
		//        db.objDataReader.Close();
		//        db = new Database();
		//        strSql = "SELECT name,tyename,isscreen,purpose,intro,important,requirement,priority,path FROM" +
		//                " tests LEFT JOIN test_files ON fileid = test_files.id WHERE tests.id = " + strId;
		//        objDr = db.select(strSql);
		//        if(objDr.Read()) {
		//            Tests T = new Tests(objDr["name"].ToString(), Convert.ToInt32(strId), Convert.ToInt32(strProgram), Convert.ToInt32(objDr["requirement"]), hashInfos, Request.UserHostAddress.ToString(), true, Convert.ToInt32(objDr["priority"]), 0);
		//            T.DatStarttime = DateTime.Now;
		//            T.DblSeconds = 30;
		//            Session["tests"] = T;
		//            T.saveLog();
		//            Session["tests"] = null;
		//        }
		//        db.objDataReader.Close();
		//        db.Dispose();
		//        db = null;
		//    }
		//    Response.Redirect("?page=" + strPage + "&mode=" + strMode + "&program=" + strProgram + "&id=" + strId);
		//}

		private void drawTestList()
		{
			Database db = new Database();
			Object cl =  (Object) Session["user"];
			User us = (User) Session["user"];
			int test = us.IntUserId;
			string strSql = "SELECT id,guide FROM test_schedule WHERE isactive = 1 AND clientid = " + test + " order by id DESC limit 1";
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()) {
				this.Controls.Add(new LiteralControl("<p>" + ((Client)Session["user"]).StrName + "<br/>" + ((Client)Session["user"]).StrAddress + "<br/>" + ((Client)Session["user"]).StrZipCode + " " + ((Client)Session["user"]).StrCity + "</p><p>" + objDr["guide"] + "</p>"));
				intProgramId = Convert.ToInt32(objDr["id"]);
				us.IntProgramId = intProgramId;
				Session["user"] = us;
			}

			db.objDataReader.Close();

            int lang_id = ((tye.Menu)Session["menu"]).IntLanguageId;

			strSql = "SELECT body,test_schedule_cat.id FROM test_schedule_infos  "+
					" INNER JOIN test_schedule_cat ON test_schedule_cat.id = catid"+
					" AND languageId = "+lang_id;

			objDr = db.select(strSql);

			Hashtable hashInfos= new Hashtable();

			while(objDr.Read()) {
				hashInfos.Add(Convert.ToInt32(objDr["id"]),objDr["body"].ToString());
			}

			db.objDataReader.Close();

			strSql = "SELECT tests.id,tests.name,tests.isscreen,test_schedule_tests.islocked FROM test_schedule_tests INNER JOIN tests ";
            strSql += "ON test_schedule_tests.testid = tests.id INNER JOIN test_schedule ON test_schedule_tests.scheduleid = test_schedule.id WHERE languageid = " + ((tye.Menu)Session["menu"]).IntLanguageId;
			strSql += " AND test_schedule.clientid = " + ((User)Session["user"]).IntUserId + " and  test_schedule_tests.scheduleid = "+ intProgramId.ToString()+" ORDER BY priority;";

			objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;
				throw new NoDataFound();
			}

			this.Controls.Add(new LiteralControl("<div class='page_subheader'>" + hashInfos[2].ToString() + "</div>"));

			HtmlTable objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt.Attributes["class"] = "data_table";
			objHt.ID = "data_table";

			HtmlTableRow objHtr = new HtmlTableRow();
			HtmlTableCell objHtc = new HtmlTableCell();

			while(objDr.Read())
			{
				intLock = Convert.ToInt32(objDr["islocked"]);

				objHtr = new HtmlTableRow();

				if(intLock == 1) {	
					objHtr.Attributes["onmouseover"] = "hoverRow(this, 'in', true);";
					objHtr.Attributes["onmouseout"] = "hoverRow(this, 'out', true);";
					objHtr.Attributes["onclick"] = "location.href='?page=" + IntPageId + "&mode=details&program=" + intProgramId + "&id=" + objDr["id"].ToString() + "';";
				}

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:30px;";
				objHtc.Attributes["class"] = "data_table_item";

				HtmlImage objI = new HtmlImage();
				
				if(intLock == 1) {
					objI.ID = "no_program_lock_" + objDr["id"].ToString();
					objI.Src = "../../gfx/no_lock.gif";
				} 
				else if(intLock == 0) {
					objI.ID = "program_lock_" + objDr["id"].ToString();
					objI.Src = "../../gfx/program_lock.gif";
					objI.Alt = hashInfos[4].ToString();
					objI.Attributes["title"] = hashInfos[4].ToString();
				}
				else if(intLock == -1) {
					objI.ID = "optician_lock_" + objDr["id"].ToString();
					objI.Src = "../../gfx/optician_lock.gif";
					objI.Alt = hashInfos[5].ToString();
					objI.Attributes["title"] = hashInfos[5].ToString();
				}

				objHtc.Controls.Add(objI);

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:245px;";
				objHtc.Attributes["class"] = "data_table_item";
				if(intLock == 1) {
					//LinkButton lnkEx = new LinkButton();
					//lnkEx.ID = "ex" + objDr["id"].ToString();
					//lnkEx.Attributes.Add("mode", "details");
					//lnkEx.Attributes.Add("program", intProgramId.ToString());
					//lnkEx.Attributes.Add("id", objDr["id"].ToString());
					//lnkEx.Attributes.Add("page", IntPageId.ToString());
					//lnkEx.Attributes.Add("isscreen", objDr["isscreen"].ToString());
					//lnkEx.Style.Add("color", "#000");
					//lnkEx.Style.Add("text-decoration", "none");
					//lnkEx.Text = objDr["name"].ToString();
					//lnkEx.Click += new EventHandler(linkbutton_click_text_exercise);

					string strOnclick = "openDialog(" +
						"'popups/Exercise_text.aspx?page=" + IntPageId.ToString() +
						"&id=" + objDr["id"].ToString() +
						"&program=" + intProgramId.ToString() +
						"&mode=details');"; 
					//, 'something', 'toolbar=0,location=0,directories=0,status=0,menubar=0,width=600,height=600');";
					//lnkEx.Attributes.Add("onclick", strOnclick);
					//objHtc.Controls.Add(lnkEx);
					objHtc.InnerHtml = objDr["name"].ToString();
				} else {
					objHtc.InnerHtml = objDr["name"].ToString();
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:140px;";
				objHtc.Attributes["class"] = "data_table_item";
				
				if(Convert.ToInt32(objDr["isscreen"]) == 1) {
					Database db1 = new Database();
					// Added 10/04-2009: "ORDER BY score DESC". The fucker who wrote this initially...grrrr...I hate the guy
					string strSql1 = "SELECT score FROM log_testresult WHERE testid ="+objDr["id"]+" AND clientid = "+ ((Client)Session["user"]).IntUserId +" AND highscore = 1 ORDER BY score DESC LIMIT 0,1";
					int intHighScore = Convert.ToInt32(db1.scalar(strSql1));
					
					if(intHighScore > 0){
						objHtc.InnerHtml = "Hiscore: "+ intHighScore.ToString();
					}
				}
				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:20px;text-align:center;";
				objHtc.Attributes["class"] = "data_table_item";
									
				objI = new HtmlImage();

				if(Convert.ToInt32(objDr["isscreen"]) == 1)
				{
					objI.ID = "monitor_test_" + objDr["id"].ToString();
					objI.Src = "../../gfx/monitor_test.gif";
					objI.Alt = hashInfos[6].ToString();
					objI.Attributes["title"] = hashInfos[6].ToString();
				}
				else
				{
					objI.ID = "printed_test_" + objDr["id"].ToString();
					objI.Src = "../../gfx/printed_test.gif";
					objI.Alt = hashInfos[7].ToString();
					objI.Attributes["title"] = hashInfos[7].ToString();
				}
				
				objHtc.Controls.Add(objI);
				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
			}

			db.objDataReader.Close();
			db = null;

			objHtr = new HtmlTableRow();
			objHtc = new HtmlTableCell();
			objHtc.ColSpan = 4;

			HtmlAnchor schedulePrint = new HtmlAnchor();
			schedulePrint.HRef = "javascript:void(0);";
			schedulePrint.Attributes["onclick"] = "window.open('popups/print.aspx?mode=schedule&type=optician&id=" + ((Client)Session["user"]).IntUserId + "','print','width=550,height=580,toolbar=no,scrollbars=yes,resizeable=no');";
			schedulePrint.InnerHtml = "<ul><li>Print</li></ul>";

			objHtc.Controls.Add(schedulePrint);
			objHtr.Controls.Add(objHtc);
			objHt.Controls.Add(objHtr);

			this.Controls.Add(objHt);
		}

		private void drawDetailsPage()
		{   // Når vi nu er på vej ind på en ny øvelse, så tjek lige om Session["tests"] = null
            // Hvis ikke, så har brugeren ikke trykket på "Tilbage" - evaluer derfor om datStartTime <> standardværdien.
            // Hvis den er betyder det at brugeren lige har været inde på en skærmøvelse og derfor skal (en kopi af) back_link_text køres.
            // Hvis den ikke er betyder det at brugeren lige har været inde på en skærmøvelse og derfkal (en kopi af) back_link_screen køres
            // Herefter er Session["tests"] igen = null, og der skulle ikke ske flere tekst/log-fejl på øvelserne.
            // [mital]

            string intNoLog = Request.QueryString["noLog"];

            if ((Tests)Session["tests"] != null) {
                Tests t = (Tests)Session["tests"];
                if (t.DatStarttime.ToString() == "01-01-2004 01:01:01") {
                    intNoLog = Request.QueryString["noLog"];
					//Session["tests"] = null;
                    if (intNoLog == null) {
                    } else {
                        switch (((Optician)Session["user"]).IntLanguageId) {
                            case 1:
                                Response.Redirect("?page=103&submenu=105&mode=schedule&id=" + Request.QueryString["scheduleId"]);
                                break;
                            case 2:
                                Response.Redirect("?page=107&submenu=109&mode=schedule&id=" + Request.QueryString["scheduleId"]);
                                break;
                            case 3:
                                Response.Redirect("?page=111&submenu=113&mode=schedule&id=" + Request.QueryString["scheduleId"]);
                                break;
                            case 4:
                                Response.Redirect("?page=1178&submenu=1206&mode=schedule&id=" + Request.QueryString["scheduleId"]);
                                break;
                        }
                    }
                } else {
                    intNoLog = Request.QueryString["noLog"];
                    if (intNoLog == null) {
                        //((Tests)Session["tests"]).DblSeconds = ((TimeSpan)DateTime.Now.Subtract(((Tests)Session["tests"]).DatStarttime)).Seconds;
                        //((Tests)Session["tests"]).saveLog();
                        //Session["tests"] = null;
                    } else {
                        //Session["tests"] = null;
                        switch (((Optician)Session["user"]).IntLanguageId) {
                            case 1:
                                Response.Redirect("?page=103&submenu=105&mode=schedule&id=" + Request.QueryString["scheduleId"]);
                                break;
                            case 2:
                                Response.Redirect("?page=107&submenu=109&mode=schedule&id=" + Request.QueryString["scheduleId"]);
                                break;
                            case 3:
                                Response.Redirect("?page=111&submenu=113&mode=schedule&id=" + Request.QueryString["scheduleId"]);
                                break;
                            case 4:
                                Response.Redirect("?page=1178&submenu=1206&mode=schedule&id=" + Request.QueryString["scheduleId"]);
                                break;
                        }
                    }
                }
            }

            intId = Convert.ToInt32(Request.QueryString["id"]);
			intProgramId = Convert.ToInt32(Request.QueryString["program"]);
			intStepId = Convert.ToInt32(Request.QueryString["step"]);
			intImportant = Convert.ToInt32(Request.QueryString["important"]);

			if(Session["tests"] == null) {
				Tests T = new Tests();
				T = T.GetTestFromId(intId);
				T.DatStarttime = DateTime.Now;
				Session["tests"] = T;
			}

			Database db = new Database();
			string strSql = "SELECT test_info_cat.id, tests_infos.body FROM tests_infos INNER JOIN test_info ON tests_infos.id = test_info.infoid INNER JOIN test_info_cat ON test_info_cat.id = tests_infos.catid WHERE test_info.testid = " + intId;
			MySqlDataReader objDr = db.select(strSql);
			Hashtable hashInfos= new Hashtable();

			while(objDr.Read()) {
				if(!hashInfos.ContainsKey(Convert.ToInt32(objDr["id"]))) {
					hashInfos.Add(Convert.ToInt32(objDr["id"]),objDr["body"].ToString());
				}
			}

			db.objDataReader.Close();
			
			int intScore = -1;
			
			db = new Database();
			if ( intNoLog == null )
				strSql = "SELECT score FROM log_testresult WHERE testid = " + intId + " AND log_testresult.clientid =" + ((User)Session["user"]).IntUserId + " AND highscore = 1 ORDER BY score LIMIT 0,1";
			else 
				strSql = "SELECT score FROM log_testresult WHERE testid = " + intId + " AND log_testresult.clientid =" + 1 + " AND highscore = 1 ORDER BY score LIMIT 0,1";
			
			
			if(db.scalar(strSql) != DBNull.Value)
				intScore = Convert.ToInt32(db.scalar(strSql));
				
			db = null;
			db = new Database();

			strSql = "SELECT name,tyename,isscreen,purpose,intro,important,requirement,priority,path FROM tests LEFT JOIN test_files ON fileid = test_files.id WHERE tests.id = " + Convert.ToInt32(Request.QueryString["id"]);
			objDr = db.select(strSql);
			string strInfoDivsOutput = "";

			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;
				throw new FileNotFound();
			}

			if(objDr.Read()) {
				if(!(Page.IsPostBack)) {
					Page_header.InnerHtml = objDr["name"].ToString();
				}
			    // Nedenstående linje er årsagen til at applikationen nogle gange viser det forkerte navn i toppen af skærmen
                // og logger forkert. Hvis man istedet for at trykke på linket "tilbage" trykker på menu-linket "Træningsprogram"
                // bliver funktionen back_link_screen aldrig kørt, og dermed bliver session["tests"] ikke nulstillet.
                // [mital]
				if(Session["tests"] == null) {
					Session["tests"] = new Tests(objDr["name"].ToString(),intId,intProgramId,Convert.ToInt32(objDr["requirement"]),hashInfos,Request.UserHostAddress.ToString(),true,Convert.ToInt32(objDr["priority"]),intScore);

					if(Convert.ToInt32(objDr["isscreen"]) == 0)
						((Tests)Session["tests"]).DatStarttime = DateTime.Now;
				}
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
				//intro_link.HRef = "../../?page=" + IntPageId + "&mode=details&id=" + intId;
				intro_link.HRef = "javascript:switchDiv('divInfoIntro', 'divTestInfo');";
				intro_link.Attributes.Add("onclick", "colorizeLink(this, 'infoLink', 'a', 'bold');");
				intro_link.Attributes.Add("class", "infoLink");
				intro_link.InnerHtml = hashInfos[2].ToString();
				
				panel_div.Controls.Add(intro_link);
				panel_div.Controls.Add(new LiteralControl(" | "));

				Database db_steps = new Database();
				string strSql_steps = "SELECT priority, body FROM tests_steps WHERE testid = " + intId + " ORDER BY priority;";
				MySqlDataReader objDr_steps = db_steps.select(strSql_steps);

				while(objDr_steps.Read()) {
					HtmlAnchor step_link = new HtmlAnchor();

					step_link.ID = objDr_steps["priority"].ToString();
					//step_link.HRef = "../../?page=" + IntPageId + "&mode=details&id=" + intId + "&step=" + Convert.ToInt32(objDr_steps["priority"]);
					step_link.HRef = "javascript:switchDiv('divInfo" + objDr_steps["priority"].ToString() + "', 'divTestInfo');";
					step_link.Attributes.Add("onclick", "switchDiv('divInfo" + objDr_steps["priority"].ToString() + "');colorizeLink(this, 'infoLink', 'a', 'bold');");
					step_link.Attributes.Add("class", "infoLink");
					step_link.InnerHtml = objDr_steps["priority"].ToString();

					panel_div.Controls.Add(step_link);
					panel_div.Controls.Add(new LiteralControl(" | "));

					strInfoDivsOutput += "<div class=\"divTestInfo\" id=\"divInfo" + objDr_steps["priority"].ToString() + "\" style=\"display:none;\">" + objDr_steps["body"].ToString() + "</div>";
				}

				strInfoDivsOutput += "<div class=\"divTestInfo\" id=\"divInfoIntro\" style=\"display:block;\">" + objDr["intro"].ToString() + "</div>";
				strInfoDivsOutput += "<div class=\"divTestInfo\" id=\"divInfoImportant\" style=\"display:none;\">" + objDr["important"].ToString() + "</div>";
				
				db_steps.objDataReader.Close();
				db_steps = null;

				HtmlAnchor important_link = new HtmlAnchor();

				important_link.ID = "important_link";
				//important_link.HRef = "../../?page=" + IntPageId + "&mode=details&id=" + intId + "&important=1";
				important_link.HRef = "javascript:switchDiv('divInfoImportant', 'divTestInfo');";
				important_link.Attributes.Add("onclick", "colorizeLink(this, 'infoLink', 'a', 'bold');");
				important_link.Attributes.Add("class", "infoLink");
				important_link.InnerHtml = hashInfos[3].ToString();
				
				panel_div.Controls.Add(important_link);				

				container_div_left.Controls.Add(panel_div);
				
				HtmlGenericControl content_div = new HtmlGenericControl("div");

				content_div.ID = "content_div";
				content_div.Attributes["class"] = "content_div";
				
				content_div.InnerHtml = strInfoDivsOutput;

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

				LinkButton back_link = new LinkButton();
				back_link.ID = "back_link";
				back_link.Text = "<ul><li>" + hashInfos[1].ToString() + "</li>";
				if(Convert.ToInt32(objDr["isscreen"]) == 1) {
					back_link.Click +=new EventHandler(back_link_screen);
				} else {
					back_link.Click +=new EventHandler(back_link_text);									
				}

				links_div.Controls.Add(back_link);

				links_div.Controls.Add(new LiteralControl("&nbsp;"));

				HtmlAnchor start_link = new HtmlAnchor();

				start_link.ID = "start_link";
				start_link.InnerHtml = "<li>" + hashInfos[4].ToString() + "</li>";
				start_link.HRef = "javascript:void(0);";
				string strExerciseId = "intExerciseIdNo=" + intId;
				if(objDr["path"].ToString().IndexOf("?") > -1) {
					strExerciseId = "&" + strExerciseId;
				} else {
					strExerciseId = "?" + strExerciseId;
				}
				start_link.Attributes["onclick"] = "window.open('ScreenTasks/" + objDr["path"].ToString() + strExerciseId + "','','fullscreen=yes');";
				
				links_div.Controls.Add(start_link);

				if(Convert.ToInt32(objDr["isscreen"]) == 0) {
					start_link.Visible = false;
				}

				if(hashInfos[24] != null) {
					HtmlAnchor metronom_link = new HtmlAnchor();

					metronom_link.ID = "metronom_link";
					metronom_link.InnerHtml = "<li>" + hashInfos[24].ToString() + "</li>";
					metronom_link.HRef = "http://www.tye.dk/download/Metronome.exe";
				
					links_div.Controls.Add(metronom_link);
				}

				HtmlAnchor print_link = new HtmlAnchor();

				print_link.ID = "print_link";
				print_link.InnerHtml = "<li>Print</li></ul>";
				print_link.Attributes["onclick"] = "window.open('popups/Print.aspx?mode=instructions&type=single&id="+intId+"','print','width=630,height=580,toolbar=no,scrollbars=yes,resizeable=false');";
				print_link.HRef = "javascript:void(0)";
				links_div.Controls.Add(print_link);

				// exception for test "Harry's Blocks"
				if(intId == 290 || intId == 291 || intId == 292 || intId == 293) {
					HtmlAnchor link_AG = new HtmlAnchor();
					link_AG.ID = "link_AG";
					link_AG.InnerHtml = "<ul><li>";
					//((User)HttpContext.Current.Session["user"]).IntLanguageId;
					int intLang = ((tye.Menu)Session["menu"]).IntLanguageId; 
					switch(intLang) {
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

		private void back_link_screen(object sender, EventArgs e)
		{
			// Jb 
			string  intNoLog = Request.QueryString["noLog"];
			if ( intNoLog == null)  {			
				Session["tests"] = null;
				Response.Redirect("?page=" + IntPageId);
			}  else  {					
				Session["tests"] = null;
				switch( ((Optician)Session["user"]).IntLanguageId)  {
					case 1:
						Response.Redirect("?page=103&submenu=105&mode=schedule&id="+Request.QueryString["scheduleId"]);
						break;
					case 2:
						Response.Redirect("?page=107&submenu=109&mode=schedule&id="+Request.QueryString["scheduleId"]);
						break;
					case 3: 
						Response.Redirect("?page=111&submenu=113&mode=schedule&id="+Request.QueryString["scheduleId"]);
						break;
					case 4:
						Response.Redirect("?page=1178&submenu=1206&mode=schedule&id="+Request.QueryString["scheduleId"]);
						break;
				}								
			}
		}

		private void back_link_text(object sender, EventArgs e)
		{
			string  intNoLog = Request.QueryString["noLog"];
			if ( intNoLog == null)  {
				((Tests)Session["tests"]).DblSeconds = ((TimeSpan)DateTime.Now.Subtract(((Tests)Session["tests"]).DatStarttime)).Seconds;
				((Tests)Session["tests"]).saveLog();

				Session["tests"] = null;
				Response.Redirect("?page=" + IntPageId);
			}  else  {					
				Session["tests"] = null;				
				switch( ((Optician)Session["user"]).IntLanguageId)  {
					case 1:
						Response.Redirect("?page=103&submenu=105&mode=schedule&id="+Request.QueryString["scheduleId"]);
						break;
					case 2:
						Response.Redirect("?page=107&submenu=109&mode=schedule&id="+Request.QueryString["scheduleId"]);
						break;
					case 3: 
						Response.Redirect("?page=111&submenu=113&mode=schedule&id="+Request.QueryString["scheduleId"]);
						break;
					case 4:
						Response.Redirect("?page=1178&submenu=1206&mode=schedule&id="+Request.QueryString["scheduleId"]);
						break;
				}
			}
		}
	}
}
