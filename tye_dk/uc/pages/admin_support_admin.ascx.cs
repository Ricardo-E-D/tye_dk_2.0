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

	public partial class admin_support_admin : uc_pages
	{
		protected int intStatus;
		protected ListBox status = new ListBox();
		protected CheckBox CbStatus = new CheckBox();
		protected TextBox body = new TextBox();
		protected int intId;
		protected int intIsLocked;
		protected int intReferenceId;
		protected string strSql_name;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string strMode = Request.QueryString["mode"];
			intId = Convert.ToInt32(Request.QueryString["id"]);

			try
			{
				intStatus = Convert.ToInt32(Request.QueryString["status"]);
				
				switch(strMode){
					case "showthread":
						drawThread();
						drawReplyForm();
						break;
					default:
						drawSelectStatus();
						drawQuestionList();
						break;
				}
			}
			catch(NoDataFound ndf)
			{

                this.Controls.Add(new LiteralControl("<p>" + ndf.Message(((tye.Menu)Session["menu"]).IntLanguageId).ToString() + "</p>"));
			}
		}
		
		private void drawSelectStatus()
		{
			status.ID = "status";
			status.Rows = 1;
			status.Attributes["onchange"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&status='+this.value";
			
			ListItem objLi = new ListItem();
			objLi.Text = "Ubesvarede spørgsmål";
			objLi.Value = "0";
			status.Items.Insert(0,objLi);

			objLi = new ListItem();
			objLi.Text = "Besvarede spørgsmål";
			objLi.Value = "1";
			status.Items.Insert(0,objLi);

			objLi = new ListItem();
			objLi.Text = "Afsluttede spørgsmål";
			objLi.Value = "2";
			status.Items.Add(objLi);

			this.Controls.Add(status);

			status.SelectedValue = intStatus.ToString();
		}

		private void drawThread()
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
				db_name.objDataReader.Close();
				db_name.Dispose();
				db_name = null;
				i++;
			}						
			
			db.objDataReader.Close();
			db = null;
		}

		private void drawReplyForm()
		{
			this.Controls.Add(new LiteralControl("<br/><br/><br/>Svar: * "));

			RequiredFieldValidator body_val = new RequiredFieldValidator();

			body_val.ControlToValidate = "body";
			body_val.ErrorMessage = "Dette felt skal udfyldes.";
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

			CbStatus.Text = " Luk spørgsmålet med dette svar.";

			this.Controls.Add(CbStatus);
			
			this.Controls.Add(new LiteralControl("<br/>"));

			Button submit = new Button();

			submit.ID = "submit";
			submit.Text = "Gem svaret";
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

		private void drawQuestionList()
		{
			Database db = new Database();

			string strSql = "SELECT contact.id,topic,usertypeid,userid FROM contact INNER JOIN users ON userid = users.id WHERE referenceid = 0 AND status = " + intStatus;

			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				throw new NoDataFound();
			}

			HtmlTable objHt = new HtmlTable();
			objHt.ID = "questionlist";
			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.Attributes["style"] = "width:475px;border-collapse:collapse;margin-top:20px;";
			objHt.Attributes["class"] = "data_table";		

			HtmlTableRow objHtr = new HtmlTableRow();

			HtmlTableCell objHtc = new HtmlTableCell();
			objHtc.Attributes["style"] = "width:125px;";
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = "Aktivitet";

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["style"] = "width:200px;";
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = "Spørgsmål";

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Attributes["style"] = "width:150px;text-align:center;";
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.InnerHtml = "Oprettet af";

			objHtr.Controls.Add(objHtc);

			objHt.Controls.Add(objHtr);

			while(objDr.Read())
			{
				objHtr = new HtmlTableRow();

				objHtr.Attributes["onmouseover"] = "hoverRow(this,'in');";
				objHtr.Attributes["onmouseout"] = "hoverRow(this,'out');";
				objHtr.Attributes["onclick"] = "location.href='?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=showthread&id=" + Convert.ToInt32(objDr["id"]) + "';";

				Database db_1 = new Database();

				string strSql_1 = "SELECT DATE_FORMAT(contact.addedtime,'%d-%m-%Y %H:%i') AS thedate FROM contact WHERE referenceid = " + Convert.ToInt32(objDr["id"]) + " ORDER BY addedtime DESC LIMIT 0,1;";
				MySqlDataReader objDr_1 = db_1.select(strSql_1);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.Attributes["style"] = "text-align:center;";

				if(objDr_1.Read())
				{
					objHtc.InnerHtml = objDr_1["thedate"].ToString();
				}

				db_1.objDataReader.Close();
				db_1 = null;
	
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

				db_1 = new Database();
				switch(Convert.ToInt32(objDr["usertypeid"]))
				{
					case 2:
						strSql_1 = "SELECT name FROM user_optician WHERE userid = " + Convert.ToInt32(objDr["userid"]);
						break;
					case 3:
						strSql_1 = "SELECT CONCAT(firstname,' ',lastname) AS name FROM user_client WHERE userid = " + Convert.ToInt32(objDr["userid"]);
						break;
				}
				objDr_1 = db_1.select(strSql_1);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["style"] = "text-align:center;";
				objHtc.Attributes["class"] = "data_table_item";
				
				if(objDr_1.Read())
				{
					objHtc.InnerHtml = objDr_1["name"].ToString();
				}

				objHtr.Controls.Add(objHtc);

				db_1.objDataReader.Close();
				db_1 = null;

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);
			}

			this.Controls.Add(objHt);

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
	
		private void saveReply(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				string[] arrMailBody = new string[] {"","Kære bruger,\n\nTrainYourEyes har hermed svaret på dit spøgsmål i vores supportsystem, du kan finde svar på dit spørgsmål på http://trainyoureyes.com under support sektionen.\n\nVenlig hilsen\n\nMaria Beadle Kops\nOptometrist og indehaver af TrainYourEyes","!Kære bruger,\n\nTrainYourEyes har hermed svaret på dit spøgsmål i vores supportsystem, du kan finde svar på dit spørgsmål på http://trainyoureyes.com under support sektionen.\n\nVenlig hilsen\n\nMaria Beadle Kops\nOptometrist og indehaver af TrainYourEyes","/Kære bruger,\n\nTrainYourEyes har hermed svaret på dit spøgsmål i vores supportsystem, du kan finde svar på dit spørgsmål på http://trainyoureyes.com under support sektionen.\n\nVenlig hilsen\n\nMaria Beadle Kops\nOptometrist og indehaver af TrainYourEyes"};
				string[] arrMailHeader = new string[] {"","Dit spørgsmål er besvaret","!Dit spørgsmål er besvaret","/Dit spørgsmål er besvaret"};
				
				Wysiwyg wys = new Wysiwyg();
				Database db = new Database();

				string strSql = "INSERT INTO contact (addedtime,referenceid,userid,body,ip) VALUES(CURRENT_TIMESTAMP()," + intReferenceId + "," + ((User)Session["user"]).IntUserId;
				strSql += ",'" + wys.ToDb(1,body.Text) + "','" + Request.UserHostAddress.ToString() + "')";

				db.execSql(strSql);

				db = new Database();
				strSql = "UPDATE contact SET status = 1 WHERE id = " + intId;
				db.execSql(strSql);

				if(CbStatus.Checked){
					db = new Database();
					strSql = "UPDATE contact SET status = 2 WHERE id = " + intId;
					db.execSql(strSql);
				}

				db = null;
				wys = null;
			
				db = new Database();
				strSql = "SELECT usertypeid FROM users WHERE id = "+intId;
				int intUserTypeId = Convert.ToInt32(db.scalar(strSql));

				db = null;
				
				int intLanguageId = 1;

				switch(intUserTypeId){
					case 2:
						db = new Database();
						strSql = "SELECT languageid FROM user_optician INNER JOIN optician_code ON opticiancodeid = optician_code.id WHERE userid = " + intId;
						intLanguageId = Convert.ToInt32(db.scalar(strSql));
						db = null;
						break;
					case 3:
						db = new Database();
						strSql = "SELECT languageid FROM user_client WHERE userid = " + intId;
						intLanguageId = Convert.ToInt32(db.scalar(strSql));
						db = null;
						break;
                }

				db = new Database();
				strSql = "SELECT email FROM users INNER JOIN contact ON userid = users.id WHERE users.id = " + intId;
				MySqlDataReader objDr = db.select(strSql);

				if(objDr.Read())
				{
					if(objDr["email"].ToString() != ""){
						MailMessage objMail = new MailMessage();

						objMail.To = objDr["email"].ToString();
						objMail.Subject = arrMailHeader[intLanguageId].ToString();
						objMail.Body = arrMailBody[intLanguageId].ToString();
						objMail.From = "noreply@trainyoureyes.com";
						objMail.BodyFormat = MailFormat.Text;
						//SmtpMail.SmtpServer = "websmtp.hardball.nu";
                        SmtpMail.SmtpServer = "localhost";
				
						SmtpMail.Send(objMail);
						objMail = null;
					}
				}
				db.objDataReader.Close();
				db = null;

				Session["noerror"] = "<div id='noerror'>Svaret er nu gemt, og der er sendt en email til forfatteren af spørgsmålet.</div>";

				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=showthread&id=" + intId);
			}
		}
	}

}
