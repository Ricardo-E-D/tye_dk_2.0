namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Mail;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using MySql.Data.MySqlClient;
	using exceptions;

	public partial class support : uc_pages
	{
		protected TextBox name = new TextBox();
		protected TextBox address = new TextBox();
		protected TextBox zipcode = new TextBox();
		protected TextBox city = new TextBox();
		protected TextBox phone = new TextBox();
		protected TextBox email = new TextBox();
		protected TextBox topic = new TextBox();
		protected TextBox body = new TextBox();
		protected string strMode;
		protected string[] arrInfos;
		protected string[] arrChars;
		protected string[][] arrHeaders;
		protected Random r;
		protected int intNextChar;
		protected int intIsLocked;
		protected int intReferenceId;
		protected int intId;
		private int intLangId = 1;
		protected string strSql_name;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			strMode = Request.QueryString["mode"];

			try
			{
                if (((tye.Login)Session["login"]).BlnSucces == false)
				{
					throw new NoAccess();
				}

				Database db = new Database();

				string strSql = "SELECT body FROM content WHERE menuid = " + IntSubmenuId;

				MySqlDataReader objDr = db.select(strSql);

				if(objDr.Read())
				{
					arrInfos = objDr["body"].ToString().Split(Convert.ToChar("^"));
				}

				db.objDataReader.Close();
				db = null;	

				this.intLangId = ((User) Session["user"]).IntLanguageId;

				switch(((User)Session["user"]).IntUserTypeId)
				{
					case 2:
						switch(strMode)
						{
							case "showthread":
								if(drawThread()){
									drawReplyForm();
								}
								break;
							default:
								drawQuestionList();
								drawQuestionForm();
								break;
						}
						break;
					case 3:
						switch(strMode)
						{
							case "showthread":
								if(drawThread()){
									drawReplyForm();
								}
								break;
							default:
								drawQuestionList();
								drawQuestionForm();
								break;
						}
						break;
				}

			}
			catch(NoAccess na)
			{
                this.Controls.Add(new LiteralControl(na.Message(((tye.Menu)Session["menu"]).IntLanguageId)));
			}
			catch(NoDataFound ndf)
			{
                this.Controls.Add(new LiteralControl(ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId)));
				drawQuestionForm();
			}
		}

		private void drawQuestionList()
		{
			this.arrHeaders = new String[5][];
			arrHeaders[1] = new String[] {"Oprettet","Spørgsmål","Indlæg","Aktivitet"};
			arrHeaders[2] = new String[] {"Oprettet","Spørgsmål","Indlæg","Aktivitet"};
			arrHeaders[3] = new String[] {"Created","Question","Answers","Activity"};
			arrHeaders[4] = new String[] {"Datum","Frage","Antwort","Aktivitet"};

			Database db = new Database();

			string strSql = "SELECT id,topic,DATE_FORMAT(contact.addedtime,'%d-%m-%Y ') AS thedate FROM contact WHERE referenceid = 0 AND userid = " + ((User)Session["user"]).IntUserId;

			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				db.objDataReader.Close();
				db.Dispose();
				db = null;
				throw new NoDataFound();
			}

			HtmlTable objHt = new HtmlTable();
			objHt.ID = "questionlist";
			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;";
			objHt.Attributes["class"] = "data_table";		

			HtmlTableRow objHtr = new HtmlTableRow();

			HtmlTableCell objHtc = new HtmlTableCell();
			objHtc.Attributes["style"] = "width:100px;";
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = this.arrHeaders[this.intLangId][0];

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["style"] = "width:200px;";
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = this.arrHeaders[this.intLangId][1];

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["style"] = "width:50px;text-align:center;";
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = this.arrHeaders[this.intLangId][2];

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["style"] = "width:125px;text-align:center;";
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = this.arrHeaders[this.intLangId][3];
					
			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			while(objDr.Read())
			{
				objHtr = new HtmlTableRow();

				objHtr.Attributes["onmouseover"] = "hoverRow(this,'in');";
				objHtr.Attributes["onmouseout"] = "hoverRow(this,'out');";
				objHtr.Attributes["onclick"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=showthread&id=" + Convert.ToInt32(objDr["id"]) + "';";

				objHtc = new HtmlTableCell();

				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["thedate"].ToString();

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";

				if(objDr["topic"].ToString().Length > 25)
				{
					objHtc.InnerHtml = objDr["topic"].ToString().Substring(0,25) + "...";
				}
				else
				{
					objHtc.InnerHtml = objDr["topic"].ToString();
				}

				objHtr.Controls.Add(objHtc);

				Database db_1 = new Database();

				string strSql_1 = "SELECT COUNT(*) AS found FROM contact WHERE referenceid = " + Convert.ToInt32(objDr["id"]);

				MySqlDataReader objDr_1 = db_1.select(strSql_1);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "text-align:center;";
				objHtc.Attributes["class"] = "data_table_item";
				
				if(objDr_1.Read())
				{
					objHtc.InnerHtml = objDr_1["found"].ToString();
				}

				objHtr.Controls.Add(objHtc);

				db_1.objDataReader.Close();
				db_1 = null;

				db_1 = new Database();

				strSql_1 = "SELECT DATE_FORMAT(contact.addedtime,'%d-%m-%Y %H:%i') AS thedate FROM contact WHERE referenceid = " + Convert.ToInt32(objDr["id"]) + " ORDER BY addedtime DESC LIMIT 0,1;";
				objDr_1 = db_1.select(strSql_1);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.Attributes["style"] = "text-align:center;";

				if(!(objDr_1.HasRows))
				{
					objHtc.InnerHtml = "<span style='color:#999999;'>Ingen</span>";
				}

				if(objDr_1.Read())
				{
					objHtc.InnerHtml = objDr_1["thedate"].ToString();
				}

				db_1.objDataReader.Close();
				db_1 = null;
	
				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);
			}

			this.Controls.Add(objHt);

			db.objDataReader.Close();
			db = null;
		}

		private bool drawThread()
		{
			intId = Convert.ToInt32(Request.QueryString["id"]);
			
			if(Session["noerror"] != null)
			{
				this.Controls.Add(new LiteralControl(Session["noerror"].ToString()));

				Session["noerror"] = null;
			}
			
			HtmlTable objHt = new HtmlTable();
			objHt.ID = "question";
			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;margin-bottom:15px;";
			objHt.Attributes["class"] = "data_table";

			Database db = new Database();

			string strSql = "SELECT contact.id,usertypeid,userid,DATE_FORMAT(contact.addedtime,'%d-%m-%Y %H:%i') AS thedate,topic,body,status FROM contact INNER JOIN users ON userid = users.id WHERE contact.id = " + intId;
			
			MySqlDataReader objDr = db.select(strSql);
						
			if(objDr.Read())
			{
				Database db_name = new Database();				

				if(Convert.ToInt32(objDr["usertypeid"]) == 2)
				{
					strSql_name = "SELECT name FROM user_optician WHERE userid = " + Convert.ToInt32(objDr["userid"]);
				}
				else if(Convert.ToInt32(objDr["usertypeid"]) == 3)
				{
					strSql_name = "SELECT CONCAT(firstname,' ',lastname) AS name FROM user_client WHERE userid = " + Convert.ToInt32(objDr["userid"]);
				}

				MySqlDataReader objDr_name = db_name.select(strSql_name);

				intReferenceId = Convert.ToInt32(objDr["id"]);
				intIsLocked = Convert.ToInt32(objDr["status"]);

				HtmlTableRow objHtr = new HtmlTableRow();
				HtmlTableCell objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:475px;text-align:center;";
				objHtc.Attributes["class"] = "data_table_header";
				objHtc.InnerHtml = objDr["topic"].ToString();
				objHtc.ColSpan = 3;
					
				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow();

				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:15px;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = "#0";

				objHtr.Controls.Add(objHtc);
								
				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:220px;";
				objHtc.Attributes["class"] = "data_table_item";

				HtmlImage objHi = new HtmlImage();
				objHi.Alt = "0";
				objHi.ID = "qimg";
				objHi.Src = "../../gfx/auther.gif";
				objHi.Attributes["style"] = "margin-right:3px;float:left;";

				objHtc.Controls.Add(objHi);

				if(objDr_name.Read())
				{
					objHtc.Controls.Add(new LiteralControl("<div style='line-height:16px;'>" + objDr_name["name"].ToString() + "</div>"));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:240px;text-align:right;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["thedate"].ToString();

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);	

				objHtr = new HtmlTableRow();
				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:475px;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["body"].ToString();
				objHtc.ColSpan = 3;
					
				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);		
	
				//rettet
				db_name.objDataReader.Close();
				db_name = null;
			}

			this.Controls.Add(objHt);

			db.objDataReader.Close();
			db = null;

			db = new Database();

			strSql = "SELECT contact.id,usertypeid,userid,DATE_FORMAT(contact.addedtime,'%d-%m-%Y %H:%i') AS thedate,topic,body,status FROM contact INNER JOIN users ON userid = users.id WHERE contact.referenceid = " + intReferenceId + " ORDER BY contact.addedtime;";
			
			objDr = db.select(strSql);

			int i = 1;

			while(objDr.Read())
			{
				objHt = new HtmlTable();
				objHt.ID = "replytable_" + i;
				objHt.CellPadding = 0;
				objHt.CellSpacing = 0;
				objHt.Attributes["style"] = "width:475px;border-collapse:collapse;margin-bottom:10px;";
				objHt.Attributes["class"] = "data_table";

				Database db_name = new Database();

				if(Convert.ToInt32(objDr["usertypeid"]) == 2)
				{
					strSql_name = "SELECT name FROM user_optician WHERE userid = " + Convert.ToInt32(objDr["userid"]);
				}
				else if(Convert.ToInt32(objDr["usertypeid"]) == 3)
				{
					strSql_name = "SELECT CONCAT(firstname,' ',lastname) AS name FROM user_client WHERE userid = " + Convert.ToInt32(objDr["userid"]);
				}
				else if(Convert.ToInt32(objDr["usertypeid"]) == 4)
				{
					strSql_name = "SELECT CONCAT(firstname,' ',lastname) AS name FROM user_admin WHERE userid = " + Convert.ToInt32(objDr["userid"]);
				}

				MySqlDataReader objDr_name = db_name.select(strSql_name);

				HtmlTableRow objHtr = new HtmlTableRow();

				HtmlTableCell objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:15px;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = "#" + i;

				objHtr.Controls.Add(objHtc);
							
				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:220px;";
				objHtc.Attributes["class"] = "data_table_item";

				HtmlImage objHi = new HtmlImage();
				objHi.Alt = "0";
				objHi.ID = "reply_" + i;

				if(Convert.ToInt32(objDr["usertypeid"]) != 4)
				{
					objHi.Src = "../../gfx/auther.gif";
				}
				else
				{
					objHi.Src = "../../gfx/tye.gif";
				}

				objHi.Attributes["style"] = "margin-right:3px;float:left;";

				objHtc.Controls.Add(objHi);

				if(objDr_name.Read())
				{
					objHtc.Controls.Add(new LiteralControl("<div style='line-height:16px;'>" + objDr_name["name"].ToString() + "</div>"));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:240px;text-align:right;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["thedate"].ToString();

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);	

				objHtr = new HtmlTableRow();
				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "width:475px;";
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = objDr["body"].ToString();
				objHtc.ColSpan = 3;
				
				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);	
	
				this.Controls.Add(objHt);

				i++;

				//rettet
				db_name.objDataReader.Close();
				db_name = null;
			}						
			
			db.objDataReader.Close();
			db = null;

			if(intIsLocked == 0){
				return true;
			}else{
				return false;
			}
		}
		private void drawQuestionForm()
		{
			this.Controls.Add(new LiteralControl("<div class='page_subheader' style='margin-top:30px;margin-bottom:15px;'>" + arrInfos[18].ToString() + "</div>" + arrInfos[7].ToString() + ": * "));

			RequiredFieldValidator topic_val = new RequiredFieldValidator();

			topic_val.ControlToValidate = "topic";
			topic_val.ErrorMessage = arrInfos[10].ToString();
			topic_val.ID = "topic_val";
			topic_val.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(topic_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			topic.ID = "topic";
			topic.Width = 400;
			topic.Style.Add("width","475px");

			this.Controls.Add(topic);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[8].ToString() + ": * "));

			RequiredFieldValidator body_val = new RequiredFieldValidator();

			body_val.ControlToValidate = "body";
			body_val.ErrorMessage = arrInfos[10].ToString();
			body_val.ID = "body_val";
			body_val.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(body_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			body.ID = "body";
			body.Width = 400;
			body.Rows = 10;
			body.TextMode = TextBoxMode.MultiLine;
			body.Style.Add("width","475px");
			body.Style.Add("height","150px");

			this.Controls.Add(body);

			this.Controls.Add(new LiteralControl("<br/>"));

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = arrInfos[9].ToString();
			submit.Width = 400;
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","15px");
			submit.Click += new EventHandler(saveQuestion);
			
			this.Controls.Add(submit);
		}

		private void drawReplyForm()
		{
			this.Controls.Add(new LiteralControl("<div class='page_subheader' style='margin-top:30px;margin-bottom:15px;'>" + arrInfos[19].ToString() + "</div>"));

			RequiredFieldValidator body_val = new RequiredFieldValidator();

			body_val.ControlToValidate = "body";
			body_val.ErrorMessage = arrInfos[10].ToString();
			body_val.ID = "body_val";
			body_val.Display = ValidatorDisplay.Dynamic;
			
			this.Controls.Add(body_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			body.ID = "body";
			body.Width = 400;
			body.Rows = 10;
			body.TextMode = TextBoxMode.MultiLine;
			body.Style.Add("width","475px");
			body.Style.Add("height","100px");

			this.Controls.Add(body);

			this.Controls.Add(new LiteralControl("<br/>"));

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = arrInfos[9].ToString();
			submit.Width = 400;
			submit.Style.Add("width","475px");
			submit.Style.Add("margin-top","15px");
			submit.Style.Add("margin-bottom","15px");
			submit.Click += new EventHandler(saveReply);

			this.Controls.Add(submit);

			this.Controls.Add(new LiteralControl("<br/>"));

			HtmlAnchor back_link = new HtmlAnchor();

			back_link.ID = "back_link";
			back_link.HRef = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId;
			back_link.InnerHtml = "Tilbage";

			this.Controls.Add(back_link);
		}

		private void saveReply(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				string[] arrMailBody = new string[] {"","mail"};
				string[] arrMailHeader = new string[] {"","mail"};
				
				Wysiwyg wys = new Wysiwyg();
				Database db = new Database();

				string strSql = "INSERT INTO contact (addedtime,referenceid,userid,body,ip) VALUES(CURRENT_TIMESTAMP()," + intReferenceId + "," + ((User)Session["user"]).IntUserId;
				strSql += ",'" + wys.ToDb(1,body.Text) + "','" + Request.UserHostAddress.ToString() + "')";

				db.execSql(strSql);

				db = null;
				wys = null;
				
				Session["noerror"] = "<div id='noerror'>" + arrInfos[17].ToString() + "</div>";

				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=showthread&id=" + intId);
			}
		}

		private void saveQuestion(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				Wysiwyg wys = new Wysiwyg();
				Database db = new Database();

				string strSql = "INSERT INTO contact (addedtime,referenceid,userid,topic,body,ip) VALUES(";
				strSql += "CURRENT_TIMESTAMP(),0," + ((User)Session["user"]).IntUserId;
				strSql += ",'" + wys.ToDb(2,topic.Text) + "','" + wys.ToDb(1,body.Text) + "','" + Request.UserHostAddress.ToString() + "')";

				db.execSql(strSql);

				db = null;
				wys = null;

				db = new Database();

				strSql = "lock table contact write;";
				db.execSql(strSql);

				strSql = "SELECT id FROM contact ORDER BY id desc LIMIT 0,1;";

				MySqlDataReader objDr = db.select(strSql);		

				if(objDr.Read())
				{
					intReferenceId = Convert.ToInt32(objDr["id"]);
				}

				db.objDataReader.Close();
				db = null;

				db = new Database();

				strSql = "unlock tables;";
				db.execSql(strSql);

				db = null;

				MailMessage objMail = new MailMessage();
				objMail.To = "maria@trainyoureyes.com";
				objMail.Subject = "Der er oprettet et spørgsmål";
				objMail.Body = "Der er oprettet et nyt spørgsmål på http://www.trainyoureyes.com\n\nDu kan se det under punktet 'Support' -> 'Ubesvarede spørgsmål' når du er logget ind.\n\nMed venlig hilsen\nwww.trainyoureyes.com";
				objMail.From = "maria@trainyoureyes.com";
				objMail.BodyFormat = MailFormat.Text;
				//SmtpMail.SmtpServer = "websmtp.tye.dk";
                SmtpMail.SmtpServer = "localhost";

				SmtpMail.Send(objMail);
			
				objMail = null;

				Session["noerror"] = "<div id='noerror'>" + arrInfos[12].ToString() + "</div>";

				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=showthread&id=" + intReferenceId);
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
	}
}
