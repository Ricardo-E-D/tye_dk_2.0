using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using tye.exceptions;
using tye;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace tye.popups
{
	public partial class Print : System.Web.UI.Page
	{
		protected string strMode;
		protected string strType;
		protected int intId;
		protected int intAccess;
		protected int lId;
		protected string strTestName;

		protected int[] arrTestsCheck = new int[] {21,6,5};
		protected string[][] arrInfos = new string[5][];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				arrInfos[1] = new string[] {"Formål","Introduktion","Trin","Vigtigt"};
				arrInfos[2] = new string[] {"Formål","Introduksjon","Trinn","Vigtig"};
				arrInfos[3] = new string[] {"Purpose","Introduction","Step","Important"};
				//Tysk ...
				arrInfos[4] = new string[] {"Zweck","Einleitung","Schritte","Wichtig"};
				
				strMode = Request.QueryString["mode"];
				strType = Request.QueryString["type"];
				intId = Convert.ToInt32(Request.QueryString["id"]);
				intAccess = Convert.ToInt32(Request.QueryString["www"]);
				lId = ((User)Session["user"]).IntLanguageId;

				try
				{
					Database db = new Database();
					string strSql = "SELECT name FROM tests WHERE id = " + intId;
					strTestName = db.scalar(strSql).ToString();
                    db.dbDispose();
					db = null;

					switch(strMode)
					{
						case "instructions":
							if(strType == "single")
							{
								printSingleInstruction();
							}
							else
							{
								printAllInstructions();
							}
						break;
						case "schedule":
							if(strType == "optician"){
								mainbody.Controls.Add(new tye.Print().printSchedule(intId));
							}else{

							}
							break;
						case "keycards":
							string strAddedtime = Convert.ToDateTime(Request.QueryString["addedtime"].Replace("_", " ")).ToString("yyyy-MM-dd_HH:mm:ss");
							mainbody.Controls.Add(new tye.Print().printKeyCards(Convert.ToDateTime(strAddedtime.Replace("_", " ")),intId));

							//Graphics g = Graphics.FromImage(bmpCard);
							//g.SmoothingMode = SmoothingMode.AntiAlias ;
							//g.DrawString("Hest",new Font("verdana",10),SystemBrushes.WindowText, 1, 1);
							//Response.ContentType="image/jpeg";
							//bmpCard.Save(Response.OutputStream, bmpCard.RawFormat);
					
							break;

					}
				}
				catch(NoDataFound ndf)
				{
					//mainbody.Controls.Add(new LiteralControl(ndf.Message(((Menu)Session["menu"]).IntLanguageId).ToString()));
				}
			}
		}

		private bool CheckId()
		{
			if(intId == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		private bool ExistArray(int testId)
		{
			bool blnExist = false;

			for(int i = 0;i <= arrTestsCheck.GetUpperBound(0);i++)
			{
				if(arrTestsCheck[i] == testId)
				{
					blnExist = true;
				}
			}

			return blnExist;
		}

		private void printSingleInstruction()
		{
			mainbody.Controls.Add(new LiteralControl("<div id='page_header'>" + strTestName + "</div>"));

			if(ExistArray(intId))
			{
				Database db = new Database();
				string strSql = "SELECT purpose,intro,steps,important FROM tests_steps_nowww WHERE testid = " + intId;

				MySqlDataReader objDr = db.select(strSql);

				if(!(objDr.HasRows))
				{
					throw new NoDataFound();
				}
				if(objDr.Read())
				{
					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][0].ToString() + "</div>" + objDr["purpose"].ToString().Substring(Convert.ToInt32(objDr["purpose"].ToString().IndexOf(Convert.ToChar("<"))),Convert.ToInt32(objDr["purpose"].ToString().Length)-Convert.ToInt32(objDr["purpose"].ToString().IndexOf(Convert.ToChar("<")))) + "<br/>"));
					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][1].ToString() + "</div>" + objDr["intro"].ToString() + "<br/><br/>"));
					mainbody.Controls.Add(new LiteralControl(objDr["steps"].ToString() + "<br/><br/>"));
					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][3].ToString() + "</div>" + objDr["important"].ToString()));
				}

				db.objDataReader.Close();
                db.dbDispose();
                objDr.Close();
				db = null;
			}
			else
			{
				Database db = new Database();
				string strSql = "SELECT purpose,intro,important FROM tests WHERE id = " + intId;

				MySqlDataReader objDr = db.select(strSql);

				if(!(objDr.HasRows))
				{
					throw new NoDataFound();
				}
				if(objDr.Read())
				{
					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][0].ToString() + "</div>" + objDr["purpose"].ToString().Substring(Convert.ToInt32(objDr["purpose"].ToString().IndexOf(Convert.ToChar("<"))),Convert.ToInt32(objDr["purpose"].ToString().Length)-Convert.ToInt32(objDr["purpose"].ToString().IndexOf(Convert.ToChar("<")))) + "<br/>"));
					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][1].ToString() + "</div>" + objDr["intro"].ToString() + "<br/>"));
					
					Database db1 = new Database();
					string strSql1 = "SELECT body FROM tests_steps WHERE testid = " + intId + " ORDER BY priority;";

					MySqlDataReader objDr1 = db1.select(strSql1);

					int i = 1;

					while(objDr1.Read())
					{
						mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][2].ToString() + " " + i + "</div>" + objDr1["body"].ToString() + "<br/>"));
						i++;
					} 

					db1.objDataReader.Close();
                    db1.dbDispose();
                    objDr1.Close();
					db1 = null;

					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][3].ToString() + "</div>" + objDr["important"].ToString()));
				}

				db.objDataReader.Close();
                db.dbDispose();
                objDr.Close();
				db = null;
			}
		}

		private void printAllInstructions()
		{
			Database db = new Database();
			string strSql = "SELECT distinct tests.id,tests.intro,tests.purpose,tests.important,tests.name FROM tests INNER JOIN test_schedule_tests ON tests.id = testid INNER JOIN test_schedule ON scheduleid = test_schedule.id WHERE clientid = " + intId + " ORDER BY priority;";

			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db.Dispose();
				db = null;
				throw new NoDataFound();
			}
			while(objDr.Read())
			{
				mainbody.Controls.Add(new LiteralControl("<div id='page_header' style='margin-top:45px;'>" + objDr["name"] + "</div>"));

				if(ExistArray(Convert.ToInt32(objDr["id"])))
				{
					Database db1 = new Database();
					string strSql1 = "SELECT purpose,intro,steps,important FROM tests_steps_nowww WHERE testid = " + Convert.ToInt32(objDr["id"]);

					MySqlDataReader objDr1 = db1.select(strSql1);

					if(!(objDr1.HasRows))
					{
						db.objDataReader.Close();
						db.Dispose();
						db = null;
						db1.objDataReader.Close();
						db1.Dispose();
						db1 = null;
						throw new NoDataFound();
					}
					if(objDr1.Read())
					{
						mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][0].ToString() + "</div>" + objDr1["purpose"].ToString().Substring(Convert.ToInt32(objDr1["purpose"].ToString().IndexOf(Convert.ToChar("<"))),Convert.ToInt32(objDr1["purpose"].ToString().Length)-Convert.ToInt32(objDr1["purpose"].ToString().IndexOf(Convert.ToChar("<")))) + "<br/>"));
						mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][1].ToString() + "</div>" + objDr1["intro"].ToString() + "<br/><br/>"));
						mainbody.Controls.Add(new LiteralControl(objDr1["steps"].ToString() + "<br/><br/>"));
						mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][3].ToString() + "</div>" + objDr1["important"].ToString()));
					}

					db1.objDataReader.Close();
					db1 = null;
				}
				else
				{
					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][0].ToString() + "</div>" + objDr["purpose"].ToString().Substring(Convert.ToInt32(objDr["purpose"].ToString().IndexOf(Convert.ToChar("<"))),Convert.ToInt32(objDr["purpose"].ToString().Length)-Convert.ToInt32(objDr["purpose"].ToString().IndexOf(Convert.ToChar("<")))) + "<br/>"));
					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][1].ToString() + "</div>" + objDr["intro"].ToString() + "<br/>"));
					
					Database db1 = new Database();
					string strSql1 = "SELECT body FROM tests_steps WHERE testid = " + objDr["id"].ToString() + " ORDER BY priority;";

					MySqlDataReader objDr1 = db1.select(strSql1);

					int i = 1;

					while(objDr1.Read())
					{
						mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][2].ToString() + " " + i + "</div>" + objDr1["body"].ToString() + "<br/>"));
						i++;
					} 

					db1.objDataReader.Close();
                    db1.dbDispose();
                    objDr1.Close();
					db1 = null;

					mainbody.Controls.Add(new LiteralControl("<div class='page_subheader'>" + arrInfos[lId][3].ToString() + "</div>" + objDr["important"].ToString()));

				}
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
