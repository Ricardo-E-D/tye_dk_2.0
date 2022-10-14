using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;
using tye.exceptions;

namespace tye
{
	public class ViewLog
	{
		private int intPageId;
		private int intSubmenuId;
		private int intLanguageId;
		
		string[][] arrInfos = new string[5][];

		public ViewLog(int pageId,int submenuId)
		{
			intPageId = pageId;
			intSubmenuId = submenuId;

			intLanguageId = 1;

			fillArrays();
		}
		public ViewLog(int pageId,int submenuId,int languageId)
		{
			intPageId = pageId;
			intSubmenuId = submenuId;
			intLanguageId = languageId;

			fillArrays();
		}

		private void fillArrays(){
			arrInfos[1] = new string[] { "Brugerlog for:", "Dato", "Øvelse", "Hiscore", "Detaljer", "Ja", "Nej", "Ingen log fundet", "Score", "Tidsforbrug", "Kommentar", "Sek.", "Min.", "Vis/skjul alle detaljer", "Tidspunkt" };
			arrInfos[2] = new string[] { "Brugerlog for:", "Dato", "Øvelse", "Hiscore", "Detaljer", "Ja", "Nej", "Ingen log fundet", "Score", "Tidsforbrug", "Kommentar", "Sek.", "Min.", "Vis/skjul alle detaljer", "Tidspunkt" };
			arrInfos[3] = new string[] { "Client:", "Date", "Test", "Hiscore", "Details", "Yes", "No", "No log found", "Score", "Time used", "Comment", "Sec.", "Min.", "Show/hide all details", "Time" };
			arrInfos[4] = new string[] { "Kunden-protokoll für:", "Datum", "Übung", "Hiscore", "Details", "Ja", "Nein", "No log found", "score", "Trainingzeit", "kommentar", "Sec.", "Min.", "Zeigen/Verstecken alle Details", "Zeit" };
		}

		public HtmlTable detailsTestLog(int intUserId,DateTime datAddedTime) 
		{
			return null;

			string strAddedTime = datAddedTime.ToString("yyyy-MM-dd HH:mm:ss");
			HtmlTable htList = new HtmlTable();
			htList.CellPadding = 0;
			htList.CellSpacing = 0;
			htList.Style.Add("width","480px");
			htList.Style.Add("border-collapse","collapse");
			htList.Attributes["class"] = "data_table";

			HtmlTableRow trList = new HtmlTableRow();
			HtmlTableCell tcList = new HtmlTableCell();

			Database db = new Database();						
			
			string strSql = "SELECT CONCAT(firstname,' ',lastname) AS name FROM user_client WHERE userid = " + intUserId;
			string strName = db.scalar(strSql).ToString();

			trList.Controls.Add(tcList);
			htList.Controls.Add(trList);

			trList = new HtmlTableRow();
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_item";
			tcList.ColSpan = 4; //attname, tid, score,hiscore
			tcList.Style.Add("padding","5px");
			tcList.Controls.Add(new LiteralControl(arrInfos[intLanguageId][0].ToString() + "  " + strName));

			trList.Controls.Add(tcList);
			htList.Controls.Add(trList);

			trList = new HtmlTableRow();
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","110");
			tcList.InnerHtml = arrInfos[intLanguageId][10];
			trList.Controls.Add(tcList);
			
			tcList = new HtmlTableCell();

			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","130");
			tcList.InnerHtml = arrInfos[intLanguageId][8];
			trList.Controls.Add(tcList);

			tcList = new HtmlTableCell();
			tcList.Attributes["class"] = "data_table_header";
			tcList.Style.Add("width","120");
			tcList.InnerHtml = arrInfos[intLanguageId][9];
			trList.Controls.Add(tcList);

			htList.Controls.Add(trList);
            db.dbDispose();
            
			db = new Database();
			strSql = "SELECT id,seconds,score,highscore,attvalue,attname FROM log_testresult WHERE clientid = "+ intUserId +" AND addedtime = '"+strAddedTime+"';";
			MySqlDataReader objDr = db.select(strSql);
			
			while(objDr.Read()){
				string strHiscore = arrInfos[intLanguageId][6].ToString();

				if(Convert.ToInt32(objDr["highscore"]) == 1){
					strHiscore = arrInfos[intLanguageId][5].ToString();
				}

				trList = new HtmlTableRow();
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("width","110");
				
				tcList.InnerHtml = objDr["attname"].ToString() + ": " + objDr["attvalue"].ToString();

				trList.Controls.Add(tcList);
				
				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("width","130");
				decimal dblSeconds = Convert.ToInt32(objDr["seconds"]);
				decimal dblMin = 0;

				if(dblSeconds > 60)
				{
					dblMin = dblSeconds / 60;
					dblMin = decimal.Floor(dblMin);
					dblSeconds = dblSeconds - (decimal.Floor(dblMin) * 60);
				}
				
				tcList.InnerHtml = decimal.Round(dblMin,0) + " " +arrInfos[intLanguageId][12].ToString() + " ";
				tcList.InnerHtml += decimal.Round(dblSeconds,0) + " " +arrInfos[intLanguageId][11].ToString();

				trList.Controls.Add(tcList);

				tcList = new HtmlTableCell();

				tcList.Attributes["class"] = "data_table_item";
				tcList.Style.Add("width","120");
				tcList.InnerHtml = objDr["score"].ToString();
				trList.Controls.Add(tcList);

				htList.Controls.Add(trList);
			}
			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return htList;
		}

		public HtmlTable testLog(int intUserId){
			HtmlTable htList = new HtmlTable();
			htList.CellPadding = 0;
			htList.CellSpacing = 0;
			htList.Style.Add("width","550px");
			htList.Style.Add("border-collapse","collapse");
			htList.Attributes["class"] = "data_table";

			HtmlTableRow trList = new HtmlTableRow();
			HtmlTableCell tcList = new HtmlTableCell();

			Database db = new Database();
			string strSql = "SELECT CONCAT(firstname,' ',lastname) AS name FROM user_client WHERE userid = " + intUserId;
			string strName = db.scalar(strSql).ToString();


			tcList.Attributes["class"] = "data_table_item";
			//tcList.ColSpan = 2;
			tcList.Controls.Add(new LiteralControl(arrInfos[intLanguageId][0].ToString() + " " + strName));
			trList.Controls.Add(tcList);
			htList.Rows.Add(trList);

			trList = new HtmlTableRow();
			tcList = new HtmlTableCell();
			//tcList.ColSpan = 2;
			tcList.InnerHtml = "<a style=\"cursor:pointer;\" onclick=\"toggleAllDetailsDiv();\">" + arrInfos[intLanguageId][13] + "</a>";
			trList.Cells.Add(tcList);
			htList.Rows.Add(trList);

			trList = new HtmlTableRow();
			tcList = new HtmlTableCell();

			tcList = new HtmlTableCell();
			//tcList.Attributes["class"] = "data_table_header";
			//tcList.Style.Add("width","240px");
			tcList.Controls.Add(new LiteralControl(arrInfos[intLanguageId][2]));
			trList.Controls.Add(tcList);

			htList.Controls.Add(trList);
            
            db.dbDispose();

			Hashtable htHighscores = new Hashtable();

			db = new Database();

			// START - find highest highscores
			#region Exercises with highscores
			/*
			Ingen highscore:

			Level1a - følg øjet				
			ExtraLevelF - Sol-stjerne		ids: 5, 45, 86, 250
			ExtraLevelE - Klappe trampe		ids: 3, 46, 85, 251
			ExtraLevelA - Labyrinter		ids: 15, 55, 95, 260
			3D Level 0 - Boldene			ids: 22, 62, 102, 267
			ExtraLevelH - Spand				ids: 37, 77, 118, 282


			Highscore:

			ExtraLevelD - Racerbil
			ExtraLevelC - Bogstavjagt
			ExtraLevelB - Kolonnehop
			3D Level 1 - Blomsten
			3D Level 2 - Stjernen
			3D Level 3 - Find tallene
			3D Level 4 - Find figuren
			3D Level 5 - Springfiksering
			3D Level 6 - Blomsten negativ
			3D Level 7 - Blomsten pos/neg
			*/
			#endregion
			strSql = "SELECT id, TABLE1.testid, clientid, programid, seconds, score, attname, attvalue" +
				" FROM (SELECT addedtime, tests.name, tests.id AS testid" + 
				" FROM log_testresult" + 
				" INNER JOIN tests ON testid = tests.id" +
				" WHERE clientid = " + intUserId + 
				" GROUP BY log_testresult.addedtime, log_testresult.testid" + 
				" ORDER BY addedtime DESC) AS TABLE1" + 
				" LEFT JOIN" + 
				" (SELECT * FROM log_testresult WHERE clientid = " + intUserId + ") AS TABLE2" + 
				" ON TABLE1.addedtime = TABLE2.addedtime WHERE highscore = 1";
			MySqlDataReader rs = db.select(strSql);
			int[] arrIdsWithoutHighscore = new int[] { 5, 45, 86, 250, 3, 46, 85, 251, 15, 55, 95, 260, 22, 62, 102, 267, 37, 77, 118, 282 };
			if(rs.HasRows) {
				while(rs.Read()) {
					int intId = Convert.ToInt32(rs["testid"].ToString());
					int intScore = Convert.ToInt32(rs["score"].ToString());
					int intSeconds = Convert.ToInt32(rs["seconds"].ToString());
					
					//bool blnStoringSeconds = (intScore == 0);			// is this "seconds exercise" or a "highscore exercise"
					bool blnStoringSeconds = Array.IndexOf(arrIdsWithoutHighscore, intId) >= 0;			// is this "seconds exercise" or a "highscore exercise"
					int intStoreValue = (blnStoringSeconds ? intSeconds : intScore); // store seconds if score is zero
					//int intStoreValue = (intScore == 0 ? intSeconds : intScore); // store seconds if score is zero
					int iq = Array.IndexOf(arrIdsWithoutHighscore, intId);
					int[] arrValues = new int[] { intStoreValue, Convert.ToInt32(rs["id"].ToString()) };

					if(!htHighscores.ContainsKey(intId)) {
						htHighscores.Add(intId, arrValues); // just add intStoreValue if keys doesn't already exist
					} else {
						int intSavedValue = ((int[])htHighscores[intId])[0];
						if (blnStoringSeconds) { // if we're storing seconds eval id new value is lower than the old one
							if (intStoreValue < intSavedValue) {
								((int[])htHighscores[intId])[0] = intStoreValue;
								((int[])htHighscores[intId])[1] = Convert.ToInt32(rs["id"].ToString());
							}
						} else {
							if (intStoreValue > intSavedValue) { // else saved the score
								((int[])htHighscores[intId])[0] = intStoreValue;
								((int[])htHighscores[intId])[1] = Convert.ToInt32(rs["id"].ToString());
							}
						}
					}
				} 
			}
			db.objDataReader.Close();
			rs.Close();

			// END - ffind highest highscores
			strSql = "SELECT * FROM";
			strSql += " (SELECT addedtime, log_testresult.id, tests.name, tests.id AS testid";
			strSql += " FROM log_testresult";
			strSql += " INNER JOIN tests ON testid = tests.id";
			strSql += " WHERE clientid = " + intUserId;
			strSql += " GROUP BY log_testresult.addedtime, testid";
			strSql += " ORDER BY addedtime DESC) AS TABLE1";
			strSql += " LEFT JOIN (SELECT * FROM log_testresult WHERE clientid = " + intUserId + ") AS TABLE2";
			strSql += " ON TABLE1.addedtime = TABLE2.addedtime";
			MySqlDataReader objDr = db.select(strSql);
			
			if(!(objDr.HasRows)){
				tcList = new HtmlTableCell();
				trList = new HtmlTableRow();

				tcList.Attributes["class"] = "data_table_item";
				//tcList.ColSpan = 2;
				tcList.Controls.Add(new LiteralControl(arrInfos[intLanguageId][7]));

				trList.Controls.Add(tcList);
				htList.Controls.Add(trList);
			}
			string strTempId = "";
			int intTempTestId = 0;
			bool blnGoOn = false;
			bool blnFirst = true;
			int intDivCount = 1;
			bool blnLightRow = false;

			System.Text.StringBuilder sbDetail = new System.Text.StringBuilder();
			DateTime dtTemp = new DateTime(1900, 1, 1);
			Hashtable hashDistinctExercises = new Hashtable();
			List<string> lstSortingExercises = new List<string>();
			sbDetail.Append("<table border=\"0\">");
			int intTempDe = 0;

			while(objDr.Read()) // loop records
			{
				string strExerciseName = objDr["name"].ToString();

				blnGoOn = false;
				DateTime dtT = Convert.ToDateTime(objDr["addedtime"].ToString());
				if (dtT.Year != dtTemp.Year || dtT.Month != dtTemp.Month || dtT.Day != dtTemp.Day) {
					if (!blnFirst) { // first time don't add closing tags
						sbDetail.Append("</table></div>");
						sbDetail.Append("<div id=\"divLogDetailsShort" + (intDivCount - 1) + "\" style=\"display:none;\">");
						intTempDe = 0;
						lstSortingExercises.Sort(); // sort the names once again (more could have been added since last time)
						
						foreach(string exercise in lstSortingExercises) {
							if(hashDistinctExercises.ContainsKey(exercise)) {
								sbDetail.Append(hashDistinctExercises[exercise]);
								if (intTempDe < hashDistinctExercises.Keys.Count - 1)
									sbDetail.Append(", ");
								intTempDe++;
							}
						}
						sbDetail.Append("</div>");
						sbDetail.Append("</td></tr>");
					}
					
					hashDistinctExercises = new Hashtable();

					blnFirst = false; 
					sbDetail.Append("<tr><td class=\"data_table_header\" style=\"width:550px;\">" + 
									"<a style=\"cursor:pointer;\" onclick=\"toggleDetailsDiv(" + intDivCount.ToString() + ");\">" +
									Convert.ToDateTime(objDr["addedtime"]).ToString("dd-MM-yyyy") + "</a>" +
									"</td></tr>");
					sbDetail.Append("<tr><td><div id=\"divLogDetails" + intDivCount.ToString() + "\" class=\"divLogDetails\">");
					sbDetail.Append("<table cellpadding=\"2\" cellspacing=\"0\" style=\"width:100%;\">\n\n\n");
					blnGoOn = true;
					dtTemp = dtT;
					intDivCount++;
				}

				if (blnGoOn) // add heading row to table (time, comment, time spent, score)
				{
					blnLightRow = true;
					sbDetail.Append("<tr>");
					sbDetail.Append("<td></td>");
					sbDetail.Append("<td><b>" + arrInfos[intLanguageId][14] + "</b></td>");
					for (int i = 10; i > 7; i--) {
						sbDetail.Append("<td><b>" + arrInfos[intLanguageId][i] + "</b></td>");
					}
					sbDetail.Append("</tr>");
				}

				if (!hashDistinctExercises.ContainsKey(strExerciseName))
					hashDistinctExercises.Add(strExerciseName, strExerciseName);
				if (!lstSortingExercises.Contains(strExerciseName))
					lstSortingExercises.Add(strExerciseName);

				sbDetail.Append("<tr " + (blnLightRow ? "style=\"background-color:#A0C2D8;\"" : "") + ">");
				sbDetail.Append("<td>" + strExerciseName + "</td>");
				sbDetail.Append("<td>" + Convert.ToDateTime(objDr["addedtime"].ToString()).ToString("hh:mm:ss") + "</td>");
				decimal dblSeconds = Convert.ToInt32(objDr["seconds"]);
				decimal dblMin = 0;
				if (dblSeconds > 60) {
					dblMin = dblSeconds / 60;
					dblMin = decimal.Floor(dblMin);
					dblSeconds = dblSeconds - (decimal.Floor(dblMin) * 60);
				}

				sbDetail.Append("<td>" + objDr["attname"].ToString() + ": " + objDr["attvalue"].ToString() + "</td>");
				sbDetail.Append("<td>" + decimal.Round(dblMin, 0) + " " + arrInfos[intLanguageId][12].ToString() + " " + decimal.Round(dblSeconds, 0) + " " + arrInfos[intLanguageId][11].ToString() + "</td>");
				sbDetail.Append("<td>");
				sbDetail.Append(objDr["score"].ToString() + "</td>");
				if (htHighscores.ContainsKey(Convert.ToInt32(objDr["testid"].ToString()))) {
					int[] intCurrentHighscore = ((int[])htHighscores[(Convert.ToInt32(objDr["testid"].ToString()))]);
					if (intCurrentHighscore[1] == Convert.ToInt32(objDr["id"].ToString())) {
						sbDetail.Append("<td><img src=\"gfx/bookmark-new.png\"></td>");
					} else {
						sbDetail.Append("<td></td>");
					}
				} else {
					sbDetail.Append("<td></td>");
				}
				sbDetail.Append("</tr>");
				
				blnLightRow = !blnLightRow;
			}

			sbDetail.Append("</table></div>");
			
			// final loop too :-) (I tend to forgot that shit...)
			sbDetail.Append("<div id=\"divLogDetailsShort" + (intDivCount - 1) + "\" style=\"display:none;\">");
			intTempDe = 0;
			lstSortingExercises.Sort(); // sort the names once again (more could have been added since last time)
			foreach (string exercise in lstSortingExercises) {
				if (hashDistinctExercises.ContainsKey(exercise)) {
					sbDetail.Append(hashDistinctExercises[exercise]);
					if (intTempDe < hashDistinctExercises.Keys.Count - 1)
						sbDetail.Append(", ");
					intTempDe++;
				}
			}
			sbDetail.Append("</div>");
			// final loop too :-) 
			sbDetail.Append("</td></tr></table>");
			tcList.InnerHtml = sbDetail.ToString();
			
			trList.Cells.Add(tcList);
			htList.Rows.Add(trList);
			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;
		
		return htList;
		}


	}
}
