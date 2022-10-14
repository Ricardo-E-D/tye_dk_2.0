using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Drawing.Text;
	using System.Drawing.Imaging;
	using System.Drawing.Drawing2D;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using tye.exceptions;

namespace tye
{
	public class Print : System.Web.UI.Page
	{
		protected int intAccess;
		protected string[] arrInfos = new string[5] {"","Print alle","!Print alle","Print all","Print all"};
		protected int intProgramId;

		public Print()
		{

		}

		public HtmlGenericControl printKeyCards(DateTime datAddedTime,int intOpticianId)
		{
			HtmlGenericControl listDiv = new HtmlGenericControl();
			string[] arrValid = new string[4] {"Koden er gyldig i 6 mdr.","!Koden er gyldig i 6 mdr.","The code is valid for 6 months.","The code is valid for 6 months."};

			Database db = new Database();
            string strSql = "SELECT name,address,zipcode,city,email,phone,languageid FROM users INNER JOIN user_optician ON userid = users.id INNER JOIN optician_code ON opticiancodeid = optician_code.id WHERE users.id = " + intOpticianId;
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				string strOptician = objDr["name"].ToString() + "\n" + objDr["address"].ToString() + "\n" + objDr["zipcode"].ToString() + " " + objDr["city"].ToString() + "\n" + objDr["phone"].ToString() + "\n" + objDr["email"].ToString();
				string strImgPath = Attributes.GFX_FOLDER+"keycard.jpg";			
				string strAddedTime = datAddedTime.ToString("yyyy-MM-dd HH:mm:ss");

				listDiv.Style.Add("text-align","center");

				Database db1 = new Database();
				string strSql1 = "SELECT id,keycode FROM log_keys WHERE opticianid = " + intOpticianId + " AND addedtime = '" + strAddedTime + "';";
				MySqlDataReader objDr1 = db1.select(strSql1);

				while(objDr1.Read())
				{
					Database db2 = new Database();
					string strSql2 = "UPDATE log_keys SET isprinted = 1 WHERE id = " + Convert.ToInt32(objDr1["id"]);
					db2.execSql(strSql2);
					db2 = null;

					Bitmap bmpCard = new Bitmap(strImgPath);
					bmpCard.SetResolution(300,300);
					Graphics g = Graphics.FromImage(bmpCard);
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.TextRenderingHint = TextRenderingHint.AntiAlias;
					g.DrawString(strOptician,new Font("tahoma",3,FontStyle.Regular),SystemBrushes.WindowText, 72, 111);
					switch(Convert.ToInt32(objDr["languageid"]))
					{
						case 1:
							g.DrawString(arrValid[0].ToString(),new Font("tahoma",2,FontStyle.Regular),SystemBrushes.WindowText, 112, 188);
							break;
						case 2:
							g.DrawString(arrValid[1].ToString(),new Font("tahoma",2,FontStyle.Regular),SystemBrushes.WindowText, 112, 188);
							break;
						case 3:
							g.DrawString(arrValid[2].ToString(),new Font("tahoma",2,FontStyle.Regular),SystemBrushes.WindowText, 112, 188);
							break;
					}
					g.DrawString(objDr1["keycode"].ToString(),new Font("Courier",5,FontStyle.Regular),SystemBrushes.WindowText, 125, 50);
					bmpCard.Save(Files.strServerSavePath + "Jh760gdjkLL99/" + objDr1["keycode"].ToString() + ".jpg", bmpCard.RawFormat);
/*
					Bitmap bmpCard = new Bitmap(strImgPath);
					bmpCard.SetResolution(300,300);
					Graphics g = Graphics.FromImage(bmpCard);
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.TextRenderingHint = TextRenderingHint.AntiAlias;
					g.DrawString(strOptician,new Font("tahoma",9,FontStyle.Regular),SystemBrushes.WindowText, 219, 365);
					switch(Convert.ToInt32(objDr["languageid"]))
					{
						case 1:
							g.DrawString(arrValid[0].ToString(),new Font("tahoma",4,FontStyle.Regular),SystemBrushes.WindowText, 400, 613);
							break;
						case 2:
							g.DrawString(arrValid[1].ToString(),new Font("tahoma",4,FontStyle.Regular),SystemBrushes.WindowText, 400, 613);
							break;
						case 3:
							g.DrawString(arrValid[2].ToString(),new Font("tahoma",4,FontStyle.Regular),SystemBrushes.WindowText, 400, 613);
							break;
					}
					g.DrawString(objDr1["keycode"].ToString(),new Font("Courier",14,FontStyle.Regular),SystemBrushes.WindowText, 405, 160);
					bmpCard.Save(Files.strServerSavePath + "Jh760gdjkLL99/" + objDr1["keycode"].ToString() + ".png", ImageFormat.Png);
*/
					listDiv.Controls.Add(new LiteralControl("<img src='"+ Files.strServerFilePath + "Jh760gdjkLL99/" + objDr1["keycode"] + ".jpg' id='"+ objDr1["keycode"] +"' alt='"+ objDr1["keycode"] +"' />"));

					//listDiv.Controls.Add(new LiteralControl("<br/><br/>"));
					bmpCard.Dispose();
				}
			
				db1.objDataReader.Close();
                db1.dbDispose();
                objDr1.Close();
				db1 = null;			
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return listDiv;
		}

		public HtmlTable printSchedule(int intUserId)
		{
			string[] arrTrainingStart = new string[] { "", "Start på træningsprogram",  "Start på træningsprogram", "Program starts", "Programmanfänge" };
			//string[] arrInfos[1] = new string[] { "Start på træningsprogram" };
			//string[] arrInfos[2] = new string[] { "Start på træningsprogram" };
			//string[] arrInfos[3] = new string[] { "Program starts" };
			//string[] arrInfos[4] = new string[] { "Programmanfänge" };

			int intLanguageId = ((User)Session["user"]).IntLanguageId;

			HtmlTable objHt = new HtmlTable();

			objHt.CellPadding = 3;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:575px;border-collapse:collapse;border:1px solid black;";
			objHt.ID = "data_table";

			Database db = new Database();
// jb : Ændring / Rettelse punkt 5

			string strSql = "SELECT test_schedule.id,test_schedule.guide,test_schedule.addedtime,CONCAT(user_client.firstname,' ',user_client.lastname) " +
				" AS name ,users.address,users.zipcode,users.city FROM test_schedule "+
				" INNER JOIN users ON users.id = test_schedule.clientid "+
				" INNER JOIN user_client ON users.id = user_client.userid "+
				" WHERE test_schedule.isactive = 1 AND clientid = " + intUserId + " order by id desc";

			MySqlDataReader objDr = db.select(strSql);

			HtmlTableRow objHtr = new HtmlTableRow();
			HtmlTableCell objHtc = new HtmlTableCell();

			if(objDr.Read())
			{
				objHtr = new HtmlTableRow();
				objHtc = new HtmlTableCell();
				objHtc.Style.Add("border","1px solid black");
				objHtc.ColSpan = 5;
				objHtc.InnerHtml = objDr["name"].ToString() + "<br/>" + objDr["address"].ToString() + "<br/>" + objDr["zipcode"].ToString() + " " + objDr["city"].ToString() + "<br/>" + arrTrainingStart[intLanguageId] + ": " + objDr["addedtime"].ToString() + "<br>" + objDr["guide"] + "<br>";
					
				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				intProgramId = Convert.ToInt32(objDr["id"]);
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();

			db = new Database();
			strSql = "SELECT tests.id,tests.name,tests.isscreen,test_schedule_tests.islocked FROM test_schedule_tests INNER JOIN tests ";
			strSql += "ON test_schedule_tests.testid = tests.id INNER JOIN test_schedule ON test_schedule_tests.scheduleid = test_schedule.id WHERE languageid = " + ((Menu)Session["menu"]).IntLanguageId;
			strSql += " AND test_schedule.clientid = " + intUserId + "  and  test_schedule_tests.scheduleid = "+ intProgramId.ToString()+" ORDER BY priority;";

			objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db = null;
				throw new NoDataFound();
			}
			
			while(objDr.Read())
			{
				objHtr = new HtmlTableRow();
				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:275px;";
				objHtc.InnerHtml = objDr["name"].ToString();
				objHtc.Style.Add("border","1px solid black");

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:140px;";
				objHtc.Style.Add("border","1px solid black");
				objHtc.InnerHtml = "Hiscore: ";

				if(Convert.ToInt32(objDr["isscreen"]) == 1){
					Database db1 = new Database();
					string strSql1 = "SELECT score FROM log_testresult WHERE testid ="+objDr["id"]+" AND clientid = "+ intUserId +" AND highscore = 1 LIMIT 0,1";
					int intHighScore = Convert.ToInt32(db1.scalar(strSql1));
					
					if(intHighScore > 0){
						objHtc.InnerHtml += intHighScore.ToString();
					}
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();

				objHtc.Attributes["style"] = "width:20px;text-align:center;";
				objHtc.Style.Add("border","1px solid black");
									
				HtmlImage objI = new HtmlImage();

				if(Convert.ToInt32(objDr["isscreen"]) == 1)
				{
					objI.ID = "monitor_test_" + objDr["id"].ToString();
					objI.Src = "../gfx/monitor_test.gif";
				}
				else
				{
					objI.ID = "printed_test_" + objDr["id"].ToString();
					objI.Src = "../gfx/printed_test.gif";
				}
				
				objHtc.Controls.Add(objI);

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
				objHtc.Style.Add("border", "1px solid black");

				objHtr.Controls.Add(objHtc);
	
				objHt.Controls.Add(objHtr);
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return objHt;
		}

		public HtmlTable printInstructions(int intUserId)
		{
			Database db = new Database();
			string strSql = "SELECT access_www FROM user_client WHERE userid = " + intUserId + ";";
			intAccess = Convert.ToInt32(db.scalar(strSql));

			HtmlTable objHt = new HtmlTable();

			objHt.Style.Add("width","475px");
			objHt.Attributes["class"] = "data_table";
			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.ID = "tests";

			db = new Database();
			strSql = "SELECT distinct tests.id,tests.name FROM tests INNER JOIN test_schedule_tests ON tests.id = testid INNER JOIN test_schedule ON scheduleid = test_schedule.id WHERE clientid = " + intUserId + " ORDER BY priority;";

			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				throw new NoDataFound();
			}

			while(objDr.Read())
			{
				HtmlTableRow objHtr = new HtmlTableRow();
				HtmlTableCell objHtc = new HtmlTableCell();

				objHtc.Style.Add("width","425px");
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["name"].ToString();

				objHtr.Controls.Add(objHtc);
				objHtc = new HtmlTableCell();

				objHtc.Style.Add("width","50px");
				objHtc.Style.Add("text-align","right");
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = "<a href='#' onclick=\"window.open('popups/print.aspx?mode=instructions&type=single&id=" + objDr["id"].ToString() + "&www=" + intAccess + "','Print','width=650,height=570,toolbars=no,scrollbars=yes,resizeable=no')\">Print</a>";

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			HtmlTableRow objHtr1 = new HtmlTableRow();
			HtmlTableCell objHtc1 = new HtmlTableCell();

			objHtc1.Style.Add("width","475px");
			objHtc1.ColSpan = 2;
			objHtc1.Attributes["class"] = "data_table_item";
			objHtc1.InnerHtml = "<a href='#' onclick=\"window.open('popups/print.aspx?mode=instructions&type=all&id=" + intUserId + "&www=" + intAccess + "','Print','width=650,height=570,toolbars=no,scrollbars=yes,resizeable=no')\">" + arrInfos[((User)Session["user"]).IntLanguageId].ToString() + "</a>";

			objHtr1.Controls.Add(objHtc1);
			objHt.Controls.Add(objHtr1);

			return objHt;
		}
	}
}
