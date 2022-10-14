using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using tye.exceptions;

namespace tye
{
	public class Schedule : System.Web.UI.Page
	{
		protected int intLock;
		protected CheckBox[] arrCB = new CheckBox[51];
		protected ListBox[] arrLB = new ListBox[51];
		protected int n = 1;
		protected string[][] arrInfos = new string[5][];
		protected int lId;
		protected int intScheduleId;
		protected int intIsLocked;
		protected bool blnIsFound = false;
		protected int intUid;


		public Schedule()
		{

		}

		public HtmlTable getOpticianSchedule(int intUserId, string strMode)
		{
			lId = ((Optician)Session["user"]).IntLanguageId;
			intUid = intUserId;

			for(int i = 1;i < 51;i++)
			{
				arrCB[i] = new CheckBox();
				arrCB[i].ID = "cb_" + i;
				arrLB[i] = new ListBox();
				arrLB[i].ID = "lb_" + i;
				arrLB[i].Enabled = false;
				arrLB[i].Rows = 1;
			}

			//Dansk
			arrInfos[1] = new string[] {"Træningsprogram for:","Kommentarer:","Træningstid:","Tekstøvelse","Skærmøvelse","Ingen lås","Programlås","Optikerlås","Godkend"};
			//Norsk
			arrInfos[2] = new string[] {"Træningsprogram for:","Kommentarer:","Træningstid:","Tekstøvelse","Skærmøvelse","Ingen lås","Programlås","Optikerlås","Godkend"};
			//Engelsk
			arrInfos[3] = new string[] {"Client:","Comment:","Time:","Text-exercise","Screen-exercise","No lock","Program lock","Optician lock","Submit"};
			//Tysk
			arrInfos[4] = new string[] {"Kunde:","Kommentar:","Zeit:","Text-Aufgabe","Bildschirm-Aufgabe","Keine Sperre","Programm. Sperre","Optische Sperre","eingeben"};

			HtmlTable objHt = new HtmlTable();

			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt.Attributes["class"] = "data_table";
			objHt.ID = "data_table";

			Database db = new Database();
// JB Ændring ændring punkt 2
			string strSql = "";
			if (strMode == "scheduleOrig") 
			{
				strSql = "SELECT test_schedule.id,comments,guide,CONCAT(firstname,' ',lastname) AS name,address,zipcode,city,email,phone FROM test_schedule INNER JOIN users ON clientid = users.id INNER JOIN user_client ON userid = users.id WHERE clientid = " + intUserId + " order by id asc;";
			} 
			else 
			{
				strSql = "SELECT test_schedule.id,comments,guide,CONCAT(firstname,' ',lastname) AS name,address,zipcode,city,email,phone FROM test_schedule INNER JOIN users ON clientid = users.id INNER JOIN user_client ON userid = users.id WHERE clientid = " + intUserId + " order by id desc;";
			}
			
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				intScheduleId = Convert.ToInt32(objDr["id"]);

				HtmlTableRow objHtr = new HtmlTableRow();

				HtmlTableCell objHtc = new HtmlTableCell();
				objHtc.ColSpan = 6;
				
				objHtc.Controls.Add(new LiteralControl("<p>" + arrInfos[lId][0].ToString() + "<br/>" + objDr["name"].ToString() + "<br/>" + objDr["address"].ToString() + "<br/>" + objDr["zipcode"] + " " + objDr["city"] + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("<a href='mailto:" + objDr["email"].ToString() + "'>" + objDr["email"].ToString() + "</a><br/>" + objDr["phone"] + "</p>"));
	
				objHtc.Controls.Add(new LiteralControl("<p>" + arrInfos[lId][1].ToString() + "<br/>" + objDr["comments"].ToString() + "</p>"));

				objHtc.Controls.Add(new LiteralControl("<p>" + arrInfos[lId][2].ToString() + "<br/>" + objDr["guide"].ToString() + "</p>"));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();

			db = new Database();

			strSql = "SELECT tests.id,tests.name,tests.tyename,tests.isscreen FROM tests WHERE languageid = " + ((Menu)Session["menu"]).IntLanguageId + " ORDER BY priority;";
			objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;
				throw new NoDataFound();
			}
			
			while(objDr.Read())
			{
				Database db1 = new Database();
				string strSql1 = "SELECT islocked FROM test_schedule_tests WHERE testid = " + Convert.ToInt32(objDr["id"]) + " AND scheduleid = " + intScheduleId + ";";
				MySqlDataReader objDr1 = db1.select(strSql1);

				if(!(objDr1.HasRows))
				{
					intIsLocked = 1;
					blnIsFound = false;
				}
				if(objDr1.Read())
				{
					intIsLocked = Convert.ToInt32(objDr1["islocked"]);
					blnIsFound = true;
				}

				db1.objDataReader.Close();
				db1 = null;

				HtmlTableRow objHtr = new HtmlTableRow();

				HtmlTableCell objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:15px;";
				objHtc.Attributes["class"] = "data_table_item";
				arrCB[n].Attributes["onclick"] = "handleLB(this," + n + ");";
				arrCB[n].Style.Add("border","0px");
				objHtc.Controls.Add(arrCB[n]);

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:15px;";
				objHtc.Attributes["class"] = "data_table_item";
				
				HtmlImage objI = new HtmlImage();

				objI.ID = "lock_" + objDr["id"].ToString();

				if(intIsLocked == -1)
				{
					objI.Src = "../../gfx/optician_lock.gif";
					objI.Alt = arrInfos[lId][7].ToString();
					objI.Attributes["title"] = arrInfos[lId][7].ToString(); 
				}
				else if(intIsLocked == 0)
				{
					objI.Src = "../../gfx/program_lock.gif";
					objI.Alt = arrInfos[lId][6].ToString();
					objI.Attributes["title"] = arrInfos[lId][6].ToString(); 
				}
				else
				{
					objI.Src = "../../gfx/no_lock.gif";
					objI.Alt = arrInfos[lId][5].ToString();
					objI.Attributes["title"] = arrInfos[lId][5].ToString(); 
				}
				

				objHtc.Controls.Add(objI);
				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.ID = "cell_" + n;
				objHtc.Style.Add("width","275px");
				if(!(blnIsFound))
				{
					objHtc.Style.Add("color","#666666");
				}
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["name"].ToString();

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:150px;";
				objHtc.Attributes["class"] = "data_table_item";
				
				arrLB[n].Items.Insert(0,new ListItem(arrInfos[lId][5].ToString(),"1"));
				arrLB[n].Items.Insert(1,new ListItem(arrInfos[lId][7].ToString(),"-1"));

				if(objDr["tyename"].ToString().StartsWith("3-D"))
				{
					arrLB[n].Items.Insert(2,new ListItem(arrInfos[lId][6].ToString(),"0"));
				}

				if(blnIsFound)
				{
					arrLB[n].Enabled = true;
					arrCB[n].Checked = true;

					//Linjen her laver en fejl, efter dataoverførslen
					try{
						arrLB[n].SelectedValue = intIsLocked.ToString();
					}
					catch(Exception e){
						//arrLB[n].SelectedValue = intIsLocked.ToString();
					}
				}

				arrLB[n].Attributes["onchange"] = "changeLock(" + n + ");";

				objHtc.Controls.Add(arrLB[n]);

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:20px;text-align:center;";
				objHtc.Attributes["class"] = "data_table_item";
									
				objI = new HtmlImage();
				
				if(Convert.ToInt32(objDr["isscreen"]) == 1)
				{
					objI.ID = "monitor_test_" + objDr["id"].ToString();
					objI.Src = "../../gfx/monitor_test.gif";
					objI.Alt = arrInfos[lId][4].ToString();
					objI.Attributes["title"] = arrInfos[lId][4].ToString();
				}
				else
				{
					objI.ID = "printed_test_" + objDr["id"].ToString();
					objI.Src = "../../gfx/printed_test.gif";
					objI.Alt = arrInfos[lId][3].ToString();
					objI.Attributes["title"] = arrInfos[lId][3].ToString();

				}
				
				// JB ændring til punkt 3
				int programId = getProgramId( intUserId );				
				
				switch ( ((Optician)Session["user"]).IntLanguageId) 
				{
					case 1:
						objI.Attributes["onClick"] = "location.href='?page=124&mode=details&program="+ programId +"&id=" + objDr["id"].ToString() + "&noLog=true&scheduleId="+intUid.ToString()+"';";				
						break;
					case 2:
						objI.Attributes["onClick"] = "location.href='?page=125&mode=details&program="+ programId +"&id=" + objDr["id"].ToString() + "&noLog=true&scheduleId="+intUid.ToString()+"';";				
						break;
					case 3:
						objI.Attributes["onClick"] = "location.href='?page=126&mode=details&program="+ programId +"&id=" + objDr["id"].ToString() + "&noLog=true&scheduleId="+intUid.ToString()+"';";				
						break;
					case 4:
						objI.Attributes["onClick"] = "location.href='?page=1184&mode=details&program="+ programId +"&id=" + objDr["id"].ToString() + "&noLog=true&scheduleId="+intUid.ToString()+"';";				
						break;
				}									
				objHtc.Attributes["onmouseover"] = "hoverRow(this,'in');";
				objHtc.Attributes["onmouseout"] = "hoverRow(this,'out');";
				// JB ændring til punkt 3

				objHtc.Controls.Add(objI);

				objHtr.Controls.Add(objHtc);
							
				objHt.Controls.Add(objHtr);
				
				n++;
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			HtmlTableRow objHtr1 = new HtmlTableRow();

			HtmlTableCell objHtc1 = new HtmlTableCell();
			objHtc1.ColSpan = 6;
				
			Button submit = new Button();
			submit.ID = "submit";
			submit.Text = arrInfos[lId][8].ToString();
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","20px");
			submit.Click += new EventHandler(saveSchedule);

			objHtc1.Controls.Add(submit);
			
			objHtr1.Controls.Add(objHtc1);
			objHt.Controls.Add(objHtr1);

			return objHt;
		}
// JB ændring til punkt 3
		private int getProgramId( int clientId ) 
		{
			int intProgramId = 0;
			Database db = new Database();
			string strSql = "SELECT id,guide FROM test_schedule WHERE isactive = 1 "+
				" AND clientid = " + clientId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{				
			
				intProgramId = Convert.ToInt32(objDr["id"]);
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			return intProgramId;
		}
		private void saveSchedule(object sender, EventArgs e)
		{
			Database db = new Database();
			string strSql = "DELETE FROM test_schedule_tests WHERE scheduleid = " + intScheduleId + ";";
			db.execSql(strSql);

			strSql = "SELECT tests.id FROM tests WHERE languageid = " + ((Menu)Session["menu"]).IntLanguageId + " ORDER BY priority;";
			
			MySqlDataReader objDr = db.select(strSql);

			n = 1;

			while(objDr.Read())
			{
				if(arrCB[n].Checked)
				{
					int test = 0;
					string selec = arrLB[n].SelectedValue.ToString();
					if(selec == "")
						test = 1;
					else
						test = Convert.ToInt32(arrLB[n].SelectedValue);

					Database db1 = new Database();
					string strSql1 = "INSERT INTO test_schedule_tests (scheduleid,testid,islocked) VALUES(" + intScheduleId + "," + Convert.ToInt32(objDr["id"]) + "," + test + ");";
					db1.execSql(strSql1);

					db1 = null;
				}

				n++;
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;


		}
	}
}
