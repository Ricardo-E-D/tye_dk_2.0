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

	public partial class admin_log : uc_pages
	{
		protected string strMode;
		protected int intId;
		protected ListBox lbLanguage = new ListBox();
		private Admin currentUser = null;
		private Translation trans = null;

		protected void Page_Load(object sender, System.EventArgs e) {
			currentUser = (Admin)Session["user"];

			strMode = Request.QueryString["mode"];
			intId = Convert.ToInt32(Request.QueryString["id"]);
			
			trans = new Translation(Server.MapPath("uc\\translation.xml"), this.GetType().BaseType.ToString(), Translation.DbLangs[currentUser.IntLanguageId - 1].ToString());

			//if (IntSubmenuId == 185 || IntSubmenuId == 1214 || IntSubmenuId == 1215 || IntSubmenuId == 1216) {
			if (IntSubmenuId == 185 && !currentUser.IsDistributor) {
				login();
			}
			else {
				switch (strMode) {
					case "client": //Klienten
						client();
						break;
					case "optician":
						optician();
						break;
					default: //Opticianlist
						drawOpticianList();
						break;
				}
			}
			
		}

		private void login(){
			this.Controls.Add(new LiteralControl("Vælg periode: "));
			string strStartdate = DateTime.Now.ToString("yyyy-MM-dd");
			string strEnddate = DateTime.Now.ToString("yyyy-MM-dd");

			if(Request.QueryString["startdate"] == null || Request.QueryString["enddate"] == null || Request.QueryString["enddate"] == "dd/mm/yyyy" || Request.QueryString["startdate"] == "dd/mm/yyyy"){
				strStartdate = DateTime.Now.Subtract(TimeSpan.FromDays(7.00)).ToString("yyyy-MM-dd");
			}else{
				strStartdate = Convert.ToDateTime(Request.QueryString["startdate"]).ToString("yyyy-MM-dd");
				strEnddate = Convert.ToDateTime(Request.QueryString["enddate"]).ToString("yyyy-MM-dd");
			}

			ListBox lbPeriod = new ListBox();
			lbPeriod.Rows = 1;
			lbPeriod.Attributes["onchange"] = "location.href='?page="+IntPageId+"&submenu="+IntSubmenuId+"&enddate="+DateTime.Now.ToString("yyyy-MM-dd")+"&startdate='+this.value+'&lb=true';";
			lbPeriod.Items.Insert(0,new ListItem("Sidste 7 dage",DateTime.Now.Subtract(TimeSpan.FromDays(7.00)).ToString("yyyy-MM-dd")));
			lbPeriod.Items.Insert(1,new ListItem("Sidste 30 dage",DateTime.Now.Subtract(TimeSpan.FromDays(30.00)).ToString("yyyy-MM-dd")));
			lbPeriod.Items.Insert(2,new ListItem("Sidste 90 dage",DateTime.Now.Subtract(TimeSpan.FromDays(90.00)).ToString("yyyy-MM-dd")));
			lbPeriod.Items.Insert(3,new ListItem("Sidste 180 dage",DateTime.Now.Subtract(TimeSpan.FromDays(180.00)).ToString("yyyy-MM-dd")));
			lbPeriod.DataBind();

			this.Controls.Add(lbPeriod);
			
			if(Request.QueryString["lb"] == "true"){
				lbPeriod.SelectedValue = strStartdate;
			}

			this.Controls.Add(new LiteralControl("<br/><br/>Interval: "));

			TextBox tbFrom = new TextBox();
			tbFrom.ID = "tbfrom";
			tbFrom.Style.Add("width","85px");
			tbFrom.Text = "dd/mm/yyyy";
			tbFrom.Attributes.Add("onfocus","if(this.value == 'dd/mm/yyyy')this.value = '';");
			tbFrom.Attributes.Add("onblur","if(this.value == '')this.value = 'dd/mm/yyyy';");

			this.Controls.Add(tbFrom);

			this.Controls.Add(new LiteralControl(" - "));

			TextBox tbTo = new TextBox();
			tbTo.ID = "tbto";
			tbTo.Style.Add("width","85px");
			tbTo.Text = "dd/mm/yyyy";
			tbTo.Attributes.Add("onfocus","if(this.value == 'dd/mm/yyyy')this.value = '';");
			tbTo.Attributes.Add("onblur","if(this.value == '')this.value = 'dd/mm/yyyy';");

			this.Controls.Add(tbTo);

			this.Controls.Add(new LiteralControl(" "));

			HtmlAnchor aPeriod = new HtmlAnchor();
			aPeriod.Attributes["class"] = "page_admin_btn";
			aPeriod.InnerHtml = "Vis";
			aPeriod.HRef = "../../#";
			aPeriod.Attributes["onclick"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&enddate='+document.getElementById('" + tbTo.UniqueID + "').value+'&startdate='+document.getElementById('" + tbFrom.UniqueID + "').value;";
            this.Controls.Add(aPeriod);
			
			HtmlTable ht = new HtmlTable();
			ht.CellPadding = 0;
			ht.CellSpacing = 0;
			ht.Style.Add("width","475px");
			ht.Style.Add("border-collapse","collapse");
			ht.Style.Add("margin-top","15px");
			ht.Attributes["class"] = "data_table";

			HtmlTableRow tr = new HtmlTableRow();

			HtmlTableCell tc = new HtmlTableCell();
			tc.Style.Add("width","150px");
			tc.Attributes["class"] = "data_table_header";
			tc.InnerHtml = "Tidspunkt";
			tr.Controls.Add(tc);

			tc = new HtmlTableCell();
			tc.Style.Add("width","85px");
			tc.Attributes["class"] = "data_table_header";
			tc.InnerHtml = "Kodeord";
			tr.Controls.Add(tc);

			tc = new HtmlTableCell();
			tc.Style.Add("width","240px");
			tc.Attributes["class"] = "data_table_header";
			tc.InnerHtml = "Bruger";
			tr.Controls.Add(tc);

			ht.Controls.Add(tr);

			Database db = new Database();
			string strSql = "SELECT addedtime,password,success FROM log_login WHERE TO_DAYS('" + strEnddate + "') - TO_DAYS(addedtime) >= 0 AND TO_DAYS(addedtime) - TO_DAYS('" + strStartdate + "') >= 0 ORDER BY addedtime DESC;";

			MySqlDataReader objDr = db.select(strSql);

			while(objDr.Read())	{

				tr = new HtmlTableRow();

				tc = new HtmlTableCell();
				tc.Attributes["class"] = "data_table_item";
				tc.InnerHtml = Convert.ToDateTime(objDr["addedtime"]).ToString("dd-MM-yyyy HH:mm:ss");
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.Attributes["class"] = "data_table_item";
				tc.InnerHtml = objDr["password"].ToString();
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.Attributes["class"] = "data_table_item";

				if(Convert.ToInt32(objDr["success"]) > 0){
					Database db1 = new Database();
					string strSql1 = "SELECT usertypeid FROM users WHERE id = "+objDr["success"].ToString();
					int intUserTypeId = Convert.ToInt32(db1.scalar(strSql1));

					db1 = new Database();
					switch(intUserTypeId){
						case 2:
							strSql1 = "SELECT user_optician.name,usertypes.name AS utname FROM users INNER JOIN user_optician ON userid = users.id INNER JOIN usertypes ON usertypeid = usertypes.id WHERE users.id = " + objDr["success"].ToString();
							break;
						case 3:
							strSql1 = "SELECT CONCAT(firstname,' ',lastname) AS name,usertypes.name AS utname FROM users INNER JOIN user_client ON userid = users.id INNER JOIN usertypes ON usertypeid = usertypes.id WHERE users.id = " + objDr["success"].ToString();
							break;
						case 4:
                            strSql1 = "SELECT CONCAT(firstname,' ',lastname) AS name,usertypes.name AS utname FROM users INNER JOIN user_admin ON userid = users.id INNER JOIN usertypes ON usertypeid = usertypes.id WHERE users.id = " + objDr["success"].ToString();
							break;
					}
					
					MySqlDataReader objDr1 = db1.select(strSql1);

					if(objDr1.Read()){
						tc.InnerHtml =  objDr1["utname"].ToString() + " : " + objDr1["name"].ToString();
					}

					db1.objDataReader.Close();
					db1 = null;
				}else{
					tc.InnerHtml = "Loginfejl";
				}

				tr.Controls.Add(tc);
				ht.Controls.Add(tr);	

				
			}
			db.objDataReader.Close();
			db = null;
			this.Controls.Add(ht);
		}

		private void client(){
			HtmlTable ht = new HtmlTable();
			HtmlTableRow tr = new HtmlTableRow();
			HtmlTableCell tc = new HtmlTableCell();

			Database db = new Database();
			string strSql = "SELECT users.addedtime,password,CONCAT(user_client.firstname,' ',user_client.lastname) AS name,address,zipcode,city,email,phone FROM users INNER JOIN user_client ON userid = users.id WHERE users.id = "+ intId;
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()){
				ht = new HtmlTable();
				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.Style.Add("width","110px");
				tc.InnerHtml = trans.GetGeneral("name") + ":";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.Style.Add("width","300px");
				tc.InnerHtml = (string)objDr["name"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("address") + ":";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["address"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("postalAndCity") + ":";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["zipcode"] + " " + (string)objDr["city"];

				tr.Controls.Add(tc);

				ht.Controls.Add(tr);
				
				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("phone") + ":";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["phone"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = "Email:";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["email"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("password") + ":";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["password"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);
				
				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("created") + ":";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = ((DateTime)objDr["addedtime"]).ToString("dd-MM-yyyy HH:mm:ss");
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				this.Controls.Add(ht);

			}
			db.objDataReader.Close();
			db = null;

			ht = new HtmlTable();
			ht.Style.Add("margin-top","15px");
			tr = new HtmlTableRow();
			tc = new HtmlTableCell();

			tc.Style.Add("width","110px");
			tc.InnerHtml = trans.GetGeneral("numberOfLogins") + ":";
			tr.Controls.Add(tc);

			db = new Database();
			strSql = "SELECT COUNT(*) FROM log_login WHERE success = "+intId;
			int intLogins = Convert.ToInt32(db.scalar(strSql)); 
			tc = new HtmlTableCell();
			tc.Style.Add("width","300px");
			tc.InnerHtml = intLogins.ToString();
			db = null;
			tr.Controls.Add(tc);

			ht.Controls.Add(tr);

			tr = new HtmlTableRow();
			tc = new HtmlTableCell();

			tc.Style.Add("width","110px");
			tc.InnerHtml = trans.GetGeneral("latestLogin") + ":";
			tr.Controls.Add(tc);

			db = new Database();
			strSql = "SELECT addedtime FROM log_login WHERE success = "+intId+" ORDER BY addedtime DESC LIMIT 0,1";
            objDr = db.select(strSql);

			tc = new HtmlTableCell();
			tc.Style.Add("width","300px");

			if(objDr.Read()){
				tc.InnerHtml = objDr["addedtime"].ToString();
			}
			
			db.objDataReader.Close();
			db = null;
			tr.Controls.Add(tc);

			ht.Controls.Add(tr);

			//Nulstil klient!
			tr = new HtmlTableRow();
			tc = new HtmlTableCell();

			tc.InnerHtml = trans.GetGeneral("resetClient") + ":"; 
			tr.Controls.Add(tc);

			tc = new HtmlTableCell();

			LinkButton lb = new LinkButton();
			lb.Text = trans.GetGeneral("resetClient");
			lb.Attributes["onclick"] = "return window.confirm('" + trans.GetGeneral("confirm_sureDeleteClient") + "');";
			lb.CausesValidation = false;
			lb.Click += new EventHandler(resetClient);

			tc.Controls.Add(lb);

			tr.Controls.Add(tc);
			ht.Controls.Add(tr);
			
			this.Controls.Add(ht);
			
			this.Controls.Add(new LiteralControl("<p><div class='page_subheader'>" + trans.GetGeneral("TyeGeneratedProgram") + ":</div></p>"));

			db = new Database();

			ht = new HtmlTable();
			ht.CellPadding = 0;
			ht.CellSpacing = 0;
			ht.Style.Add("width","475px");
			ht.Style.Add("border-collapse","collapse");
			ht.Attributes["class"] = "data_table";

			strSql = "SELECT comments,guide,id FROM log_test_schedule WHERE clientid = "+intId;
			objDr = db.select(strSql);
			int intScheduleId = 0;
			if(objDr.Read()){
				intScheduleId = Convert.ToInt32(objDr["id"]);
				this.Controls.Add(new LiteralControl(trans.GetGeneral("comment") + ":<br/>"+objDr["comments"].ToString()+"<p>" + trans.GetGeneral("timeSpent") +":<br/>"+ objDr["guide"].ToString() +"</p>"));
			}

			db.objDataReader.Close();

			db = new Database();
			strSql = "SELECT tests.id,name,tyename,islocked,isscreen FROM tests INNER JOIN log_test_schedule_tests ON testid = tests.id WHERE scheduleid = "+intScheduleId+" ORDER BY priority";
			objDr = db.select(strSql);

			while(objDr.Read()){
				tr = new HtmlTableRow();
				tc = new HtmlTableCell();
				tc.Attributes["class"] = "data_table_item";
				tc.Style.Add("width","35px");

				HtmlImage objI = new HtmlImage();
					
				if(Convert.ToInt32(objDr["islocked"]) == 1)
				{
					objI.ID = "no_program_lock_" + objDr["id"].ToString();
					objI.Src = "../../gfx/no_lock.gif";
				}
				else if(Convert.ToInt32(objDr["islocked"]) == 0)
				{
					objI.ID = "program_lock_" + objDr["id"].ToString();
					objI.Src = "../../gfx/program_lock.gif";
					objI.Alt = "Programlås";
					objI.Attributes["title"] = "Programlås";
				}
				
				tc.Controls.Add(objI);
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.Attributes["class"] = "data_table_item";
				tc.Style.Add("width","200px");
				tc.InnerHtml = objDr["name"].ToString();
				tr.Controls.Add(tc);
				
				tc = new HtmlTableCell();
				tc.Attributes["class"] = "data_table_item";
				tc.Style.Add("width","200px");
				tc.InnerHtml = objDr["tyename"].ToString();
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.Attributes["class"] = "data_table_item";
				tc.Style.Add("width","40px");
			
				objI = new HtmlImage();

				if(Convert.ToInt32(objDr["isscreen"]) == 1)
				{
					objI.ID = "monitor_test_" + objDr["id"].ToString();
					objI.Src = "../../gfx/monitor_test.gif";
					objI.Alt = "Skærmøvelse";
					objI.Attributes["title"] = "Skærmøvelse";
				}
				else
				{
					objI.ID = "printed_test_" + objDr["id"].ToString();
					objI.Src = "../../gfx/printed_test.gif";
					objI.Alt = "Tekstøvelse";
					objI.Attributes["title"] = "Tekstøvelse";
				}
				
				tc.Controls.Add(objI);
				tr.Controls.Add(tc);
				ht.Controls.Add(tr);
			}
			db.objDataReader.Close();
			db = null;

			this.Controls.Add(ht);

			this.Controls.Add(new LiteralControl("<div style='margin-top:15px;'><a href='?page="+IntPageId+"&submenu="+IntSubmenuId+"&mode=optician&id="+Request.QueryString["opticianid"]+"'>" + trans.GetGeneral("back") + "</a></div>"));
		}

		private void optician(){
			HtmlTable ht = new HtmlTable();
			HtmlTableRow tr = new HtmlTableRow();
			HtmlTableCell tc = new HtmlTableCell();

			Database db = new Database();
			string strSql = "SELECT users.addedtime,password,user_optician.name,address,zipcode,city,email,phone,CONCAT(language.tyeid,'-',optician_chain.name,'-',optician) AS optid FROM users INNER JOIN user_optician ON userid = users.id INNER JOIN optician_code ON opticiancodeid = optician_code.id INNER JOIN language ON optician_code.languageid = language.id INNER JOIN optician_chain ON chainid = optician_chain.id WHERE users.id = "+ intId  + " ORDER BY user_optician.name";
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()){
				ht = new HtmlTable();
				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.Style.Add("width","110px");
				tc.InnerHtml = trans.GetGeneral("name") + ":";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.Style.Add("width","300px");
				tc.InnerHtml = (string)objDr["name"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("address") + ":";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["address"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("postalAndCity") + ":"; //"Post nr. & By:";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["zipcode"] + " " + (string)objDr["city"];

				tr.Controls.Add(tc);

				ht.Controls.Add(tr);
				
				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("phone") + ":"; //"Telefon:";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["phone"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = "Email:";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["email"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("password") + ":"; // "Kodeord:";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["password"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);
				
				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = "Id:";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = (string)objDr["optid"];
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				tr = new HtmlTableRow();
				tc = new HtmlTableCell();

				tc.InnerHtml = trans.GetGeneral("created") + ":"; // "Oprettet:";
				tr.Controls.Add(tc);

				tc = new HtmlTableCell();
				tc.InnerHtml = ((DateTime)objDr["addedtime"]).ToString("dd-MM-yyyy HH:mm:ss");
				tr.Controls.Add(tc);

				ht.Controls.Add(tr);

				this.Controls.Add(ht);

			}
			db.objDataReader.Close();
			db = null;

			ht = new HtmlTable();
			ht.Style.Add("margin-top","15px");
			tr = new HtmlTableRow();
			tc = new HtmlTableCell();

			tc.Style.Add("width","110px");
			tc.InnerHtml = trans.GetGeneral("numberOfLogins") + ":"; // "Antal logins:";
			tr.Controls.Add(tc);

			db = new Database();
			strSql = "SELECT COUNT(*) FROM log_login WHERE success = "+intId;
            int intLogins = Convert.ToInt32(db.scalar(strSql)); 
			tc = new HtmlTableCell();
			tc.Style.Add("width","300px");
			tc.InnerHtml = intLogins.ToString();
			db = null;
			tr.Controls.Add(tc);

			ht.Controls.Add(tr);

			tr = new HtmlTableRow();
			tc = new HtmlTableCell();

			tc.Style.Add("width","110px");
			tc.InnerHtml = trans.GetGeneral("latestLogin") + ":"; //"Sidste login:";
			tr.Controls.Add(tc);

			db = new Database();
			strSql = "SELECT addedtime FROM log_login WHERE success = "+intId+" ORDER BY addedtime DESC LIMIT 0,1";
            objDr = db.select(strSql);

			tc = new HtmlTableCell();
			tc.Style.Add("width","300px");

			if(objDr.Read()){
				tc.InnerHtml = objDr["addedtime"].ToString();
			}
			
			db.objDataReader.Close();
			db = null;
			tr.Controls.Add(tc);

			ht.Controls.Add(tr);
			
			this.Controls.Add(ht);

			this.Controls.Add(new LiteralControl("<p><div class='page_subheader'>" + trans.GetString("relatedClients") + ":</div></p>"));

			db = new Database();

			strSql = "SELECT users.id,CONCAT(firstname,' ',lastname) AS name,enddate,DATE_FORMAT(addedtime,'%d-%m-%Y %I:%i') AS thedate,password FROM users INNER JOIN user_client ON user_client.userid = users.id WHERE opticianid = " + intId + " ORDER BY name;"; //AND isactive = 1

			DataGrid objDg = new DataGrid();
			objDg.ID = "data_table";
			objDg.AutoGenerateColumns = false;
			objDg.DataSource = db.select(strSql);
			objDg.CellPadding = 0;
			objDg.CellSpacing = 0;
			objDg.BorderWidth = 1;
			objDg.Width = 480;
			objDg.CssClass = "data_table";
			objDg.GridLines = GridLines.None;

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=client&id={0}&opticianid="+intId;
			objHlc.HeaderText = trans.GetGeneral("name");
			objHlc.DataTextField = "name";
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);

			BoundColumn objBc = new BoundColumn();

			objBc.DataField = "password";
			objBc.HeaderText = trans.GetGeneral("password");
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 120;
			objBc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			objBc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "thedate";
			objBc.HeaderText = trans.GetGeneral("created");
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 120;
			objBc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			objBc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
			objDg.Columns.Add(objBc);
			
			objDg.DataBind();

			this.Controls.Add(objDg);

			if(!(db.objDataReader.HasRows)){
				this.Controls.Add(new LiteralControl("<p>" + trans.GetString("noClientsFound") + "</p>"));
			}

			db.objDataReader.Close();

			db = null;

			this.Controls.Add(new LiteralControl("<p><div class='page_subheader'>" + trans.GetString("relatedKeys") + ":</div></p>"));

			HtmlTable htList = new HtmlTable();
			htList.CellPadding = 0;
			htList.CellSpacing = 0;
			htList.Style.Add("width","480px");
			htList.Style.Add("border-collapse","collapse");
			htList.Attributes["class"] = "data_table";

            HtmlTableRow trList = new HtmlTableRow();
			HtmlTableCell tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","130px");
			tcList.Controls.Add(new LiteralControl(trans.GetGeneral("date")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","120px");
			tcList.Style.Add("text-align","center");
			tcList.Controls.Add(new LiteralControl(trans.GetString("numberOfKeys")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","120px");
			tcList.Style.Add("text-align","center");
			tcList.Controls.Add(new LiteralControl(trans.GetGeneral("printed")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","110px");
			tcList.Style.Add("text-align","right");
			tcList.Controls.Add(new LiteralControl(trans.GetGeneral("print")));

			trList.Controls.Add(tcList);
			tcList = new HtmlTableCell();
			htList.Controls.Add(trList);

			db = new Database();
			strSql = "SELECT addedtime,isprinted FROM log_keys WHERE opticianid = " + intId + " GROUP BY addedtime ORDER BY addedtime DESC;";
			objDr = db.select(strSql);

			while(objDr.Read()){

				trList = new HtmlTableRow();
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.InnerHtml = Convert.ToDateTime(objDr["addedtime"]).ToString("dd-MM-yyyy");

				trList.Controls.Add(tcList);
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("text-align","center");

				Database db1 = new Database();
				string strSql1 = "SELECT COUNT(*) AS found FROM log_keys WHERE opticianid = " + intId + " AND addedtime = '" + Convert.ToDateTime(objDr["addedtime"]).ToString("yyyy-MM-dd HH:mm:ss") + "';";
				int intCount = Convert.ToInt32(db1.scalar(strSql1));
				db1 = null;

				tcList.InnerHtml = intCount.ToString();

				trList.Controls.Add(tcList);
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("text-align","center");
				
				if(Convert.ToInt32(objDr["isprinted"]) == 1){
					tcList.InnerHtml = trans.GetGeneral("yes");
				}else{
					tcList.InnerHtml = trans.GetGeneral("no");
				}
				
				trList.Controls.Add(tcList);
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("text-align","right");
				tcList.InnerHtml = "<a href='javascript:void(0);' onclick=\"window.open('popups/print.aspx?mode=keycards&addedtime="+Convert.ToDateTime(objDr["addedtime"]).ToString("yyyy-MM-dd_HH:mm:ss") + "&id=" + intId + "','print','width=400,height=580,toolbar=no,scrollbars=yes,resizeable=no');\">" + trans.GetGeneral("print") + "</a>";

				trList.Controls.Add(tcList);
				tcList = new HtmlTableCell();
				htList.Controls.Add(trList);
			}
			this.Controls.Add(htList);

			if(!(objDr.HasRows)){
				this.Controls.Add(new LiteralControl("<p>" + trans.GetString("noKeysFound") + "</p>"));
			}

			db.objDataReader.Close();
			db = null;

			


		}

		private void drawOpticianList(){
			int intLanguageid = Convert.ToInt32(Request.QueryString["language"]);
				
			if(intLanguageid == 0){
				intLanguageid = (currentUser.IsDistributor ? currentUser.IntLanguageId : 1);
			}
			
			Database db = null;
			string strSql = "";

			if(!currentUser.IsDistributor) {
				this.Controls.Add(new LiteralControl("Sprog: "));

				lbLanguage.ID = "lbLanguage";
				lbLanguage.Rows = 1;
				lbLanguage.Attributes["onchange"] = "location.href='?page="+IntPageId+"&submenu="+IntSubmenuId+"&language='+this.value";
				
				db = new Database();
				strSql = "SELECT id, name FROM language" + (currentUser.IsDistributor ? " WHERE id = " + currentUser.IntLanguageId : "") + " ORDER BY name";
				lbLanguage.DataSource = db.select(strSql);
				lbLanguage.DataTextField = "name";
				lbLanguage.DataValueField = "id";
				lbLanguage.DataBind();
				
				db.objDataReader.Close();
				db = null;

				this.Controls.Add(lbLanguage);
				this.Controls.Add(new LiteralControl("<br /><br />"));
			}

			db = new Database();
			strSql = "SELECT users.id,user_optician.name,CONCAT(tyeid,'-',optician_chain.name,'-',optician) AS optid FROM users INNER JOIN user_optician ON userid = users.id INNER JOIN optician_code ON opticiancodeid = optician_code.id";
            strSql += " INNER JOIN language ON optician_code.languageid = language.id INNER JOIN optician_chain ON chainid = optician_chain.id WHERE optician_code.languageid = " + intLanguageid + " AND usertypeid = 2 ORDER BY user_optician.name, optician_chain.name,optician";
            			
			DataGrid objDg = new DataGrid();
			objDg.ID = "data_table";
			objDg.AutoGenerateColumns = false;
			objDg.DataSource = db.select(strSql);
			objDg.CellPadding = 0;
			objDg.CellSpacing = 0;
			objDg.BorderWidth = 1;
			objDg.Width = 480;
			objDg.CssClass = "data_table";
			objDg.GridLines = GridLines.None;

			BoundColumn objBc = new BoundColumn();

			objBc.DataField = "name";
			objBc.HeaderText = trans.GetGeneral("name");
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 250;
			objDg.Columns.Add(objBc);

			objBc = new BoundColumn();

			objBc.DataField = "optid";
			objBc.HeaderText = trans.GetGeneral("opticianid");
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 130;
			objDg.Columns.Add(objBc);

			HyperLinkColumn objHlc = new HyperLinkColumn();
			objHlc.DataNavigateUrlField = "id";
			objHlc.DataNavigateUrlFormatString = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=optician&id={0}";
			objHlc.HeaderText = trans.GetGeneral("readmore");
			objHlc.Text = trans.GetGeneral("details"); // "Detaljer";
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);
		
			objDg.DataBind();

			if(!(db.objDataReader.HasRows)){
				this.Controls.Add(new LiteralControl("Der blev ikke fundet nogle optikere i databasen."));
			}else{
				this.Controls.Add(objDg);
			}

			db.objDataReader.Close();
			db = null;

			lbLanguage.SelectedValue = intLanguageid.ToString();
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

		private void resetClient(object sender, EventArgs e)
		{
			Database db = new Database();
			string strSql = "SELECT password FROM users WHERE id = " + intId + ";";
			string strPassword = Convert.ToString(db.scalar(strSql));
			db = null;

			if(strPassword != "" && strPassword != "0")
			{
				db = new Database();
				strSql = "INSERT INTO optician_keys (password,opticianid,addedtime,authorid,isactive) VALUES('" + strPassword + "'," + Request.QueryString["opticianid"] + ",CURRENT_TIMESTAMP()," + ((Admin)Session["user"]).IntUserId + ",1);";
				db.execSql(strSql);
				db = null;

				db = new Database();
				strSql = "DELETE FROM a_anamnese WHERE clientid = " + intId + ";";
				strSql += "DELETE FROM a_21 WHERE clientid = " + intId + ";";
				strSql += "DELETE FROM a_control WHERE clientid = " + intId + ";";
				strSql += "DELETE FROM a_convergence WHERE clientid = " + intId + ";";
				strSql += "DELETE FROM a_motilitet WHERE clientid = " + intId + ";";
				db.execSql(strSql);
				db = null;

				db = new Database();
				strSql = "SELECT id FROM test_schedule WHERE clientid = " + intId + ";";
				int intSchedule = Convert.ToInt32(db.scalar(strSql));
				db = null;

				db = new Database();
				strSql = "DELETE FROM test_schedule WHERE id = " + intSchedule + ";";
				strSql += "DELETE FROM test_schedule_tests WHERE scheduleid = " + intSchedule + ";";
				db.execSql(strSql);
				db = null;

				db = new Database();
				strSql = "DELETE FROM users WHERE id = " + intId + ";DELETE FROM user_client WHERE userid = " + intId + ";";
				db.execSql(strSql);
				db = null;

				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=optician&id=" + Request.QueryString["opticianid"]);
			}
		}
	}
}
