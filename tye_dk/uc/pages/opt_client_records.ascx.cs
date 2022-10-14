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
	using exceptions;

	public partial class opt_client_records : uc_pages
	{
		string strMode;
		int intId;
		int lId;

		protected DateTime datEndTime;

		protected TextBox usercode = new TextBox();
		protected TextBox name = new TextBox();
		protected TextBox address = new TextBox();
		protected TextBox zipcode = new TextBox();
		protected TextBox city = new TextBox();
		protected TextBox birthdate = new TextBox();
		protected TextBox phone = new TextBox();
		protected TextBox fax = new TextBox();
		protected TextBox email = new TextBox();
		protected TextBox enddate = new TextBox();
		protected CheckBox access_www = new CheckBox();

		protected HtmlAnchor end = new HtmlAnchor();
		protected HtmlAnchor control = new HtmlAnchor();
		private int languageid = 1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			int tmpUserId = ((User)Session["user"]).IntLanguageId;

            tmpUserId = ((tye.Menu)Session["menu"]).IntLanguageId;			
			tmpUserId = ((Optician)Session["user"]).IntLanguageId;
			this.languageid = ((Optician)Session["user"]).IntLanguageId;
	
			if(IntSubmenuId == 0)
			{
				switch(((Optician)Session["user"]).IntLanguageId)
				{
					case 1:
						Response.Redirect("?page=103&submenu=105");
						break;
					case 2:
						Response.Redirect("?page=107&submenu=109");
						break;
					case 3:
						Response.Redirect("?page=111&submenu=113");
						break;
					case 4:
						Response.Redirect("?page=1178&submenu=1179");
						break;
				}
			}

			strMode = Request.QueryString["mode"];
			intId = Convert.ToInt32(Request.QueryString["id"]);
			lId = ((Optician)Session["user"]).IntLanguageId;

			switch(strMode)
			{
				case "schedule":
					if(CheckId())
					{
						drawSchedulePage();
					}
					else
					{
						throw new NoDataFound();
					}
					break;
				case "scheduleOrig":
					if(CheckId())
					{
						drawSchedulePage();
					}
					else
					{
						throw new NoDataFound();
					}
					break;
				case "details":
					if(CheckId())
					{
						drawDetailsPage();
					}
					else
					{
						throw new NoDataFound();
					}
					break;
				case "editinfo":
					if(CheckId())
					{
						drawEditPage();
					}
					else
					{
						throw new NoDataFound();
					}
					break;
				case "end":
				switch(((Optician)Session["user"]).IntLanguageId)
				{
					case 1:
						Response.Redirect("?page=103&submenu=104&mode=end&id=" + intId + "&ref="+Request.QueryString["ref"]);
						break;
					case 2:
						Response.Redirect("?page=107&submenu=108&mode=end&id=" + intId + "&ref="+Request.QueryString["ref"]);
						break;
					case 3:
						Response.Redirect("?page=111&submenu=112&mode=end&id=" + intId + "&ref="+Request.QueryString["ref"]);
						break;
					case 4:
						Response.Redirect("?page=1178&submenu=1179&mode=end&id=" + intId + "&ref="+Request.QueryString["ref"]);
						break;
				}
					break;
				case "control":
				switch(((Optician)Session["user"]).IntLanguageId)
				{
					case 1:
						Response.Redirect("?page=103&submenu=104&mode=control&id=" + intId + "&ref="+Request.QueryString["ref"]);
						break;
					case 2:
						Response.Redirect("?page=107&submenu=108&mode=control&id=" + intId + "&ref="+Request.QueryString["ref"]);
						break;
					case 3:
						Response.Redirect("?page=111&submenu=112&mode=control&id=" + intId + "&ref="+Request.QueryString["ref"]);
						break;
					case 4:
						Response.Redirect("?page=1178&submenu=1179&mode=control&id=" + intId + "&ref="+Request.QueryString["ref"]);
						break;
				}
					break;
				case "print":
					this.Controls.Add(new Print().printInstructions(intId));
					break;
				case "log":
					this.Controls.Add(new ViewLog(IntPageId,IntSubmenuId, languageid).testLog(intId));
					break;
				case "detaillog":
					this.Controls.Add( new LiteralControl( getTestNameFromDate( intId,((Optician)Session["user"]).IntLanguageId,  Convert.ToDateTime(Request.QueryString["addedtime"].Replace("_", " ")))));
					this.Controls.Add(new ViewLog(IntPageId,IntSubmenuId, languageid).detailsTestLog(intId,Convert.ToDateTime(Request.QueryString["addedtime"].Replace("_", " "))));
					break;
				default:
					drawListPage();
					break;
			}
		}

		private bool CheckId()
		{
			if(intId == 0)
			{
				return false;
			}

			return true;
		}

		private bool CheckNull(object objMDR)
		{
			if(objMDR.ToString() == "")
			{
				return false;
			}

			return true;
		}

		private void drawEditPage() //Ret stamdata
		{
			string[][] arrInfos = new string[5][];
 
			//Dansk
			arrInfos[1] = new string[] {"Bruger id","Navn","Adresse","Postnummer","By","Fødselsdag","Telefon","Fax","Email","Adgang til WWW","Godkend","Dette felt skal udfyldes.","Der skal indtastes en gyldig dato.","Der skal indtastes en gyldig email.","Udløbsdato","Minimum dagsdato, maksimum +6 måneder"};
			//Norsk
			arrInfos[2] = new string[] {"!Bruger id","!Navn","!Adresse","!Postnummer","!By","!Fødselsdag","!Telefon","!Fax","!Email","!Adgang til WWW","!Godkend","!Dette felt skal udfyldes.","!Der skal indtastes en gyldig dato.","!Der skal indtastes en gyldig email.","!Udløbsdato","!Minimum dagsdato, maksimum +6 måneder"};
			//Engelsk
			arrInfos[3] = new string[] {"User-id", "Name","Address","Postal coder","Town","Birthday","Telephone","Fax","E-mail","Access to WWW","Approve","This field must be completed.","A valid date must be entered.","A valid e-mail address must be entered.","expiration date; ","today’s date at the very least,+6 months at the most"};
			//JB OVERSÆTTELSE
			arrInfos[4] = new string[] {"User-id", "Name", "Adresse","Postleitzahl","Ort","Geburtstag","Telefon","Fax","E-mail","Internetzugang","o.k.","Diese Felder müssen ausgefüllt werden.","Es muss ein gültiges Datum eingegeben werden.","Es muss eine gültige e-mail-Adresse eingegeben werden.","Gültigkeit bis; ","Mindestens heute, maximal + 6 Monate"};
			
			this.Controls.Add(new LiteralControl(arrInfos[lId][0].ToString() + ":<br/>"));

			usercode.ID = "usercode";
			usercode.Width = 12;
			usercode.Style.Add("width","55px");
			usercode.ReadOnly = true;
			usercode.Style.Add("background","#cccccc");

			this.Controls.Add(usercode);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][14].ToString() + ": * "));

			RangeValidator enddate_val = new RangeValidator();

			enddate_val.Type = ValidationDataType.Date;
			enddate_val.MaximumValue = DateTime.Now.AddMonths(6).ToString("dd/MM/yyyy").Replace("-","/");
			enddate_val.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy").Replace("-","/");
			enddate_val.ID = "enddate_val";
			enddate_val.ControlToValidate = "enddate";
			enddate_val.ErrorMessage = arrInfos[lId][15].ToString();
			enddate_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(enddate_val);

			this.Controls.Add(new LiteralControl("<br/>"));
			
			enddate.ID = "enddate";
			enddate.Width = 15;
			enddate.MaxLength = 10;
			enddate.Style.Add("width","85px");

			this.Controls.Add(enddate);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][1].ToString() + ": * "));

			RequiredFieldValidator name_val = new RequiredFieldValidator();

			name_val.ID = "name_val";
			name_val.ControlToValidate = "name";
			name_val.ErrorMessage = arrInfos[lId][11].ToString();
			name_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(name_val);

			this.Controls.Add(new LiteralControl("<br/>"));

			name.ID = "name";
			name.Width = 50;
			name.Style.Add("width","200px");

			this.Controls.Add(name);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][2].ToString() + ":"));

			this.Controls.Add(new LiteralControl("<br/>"));

			address.ID = "address";
			address.Width = 50;
			address.Style.Add("width","200px");

			this.Controls.Add(address);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][3].ToString() + ":"));

			this.Controls.Add(new LiteralControl("<br/>"));

			zipcode.ID = "zipcode";
			zipcode.Width = 8;
			zipcode.Style.Add("width","45px");

			this.Controls.Add(zipcode);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][4].ToString() + ":"));
			
			this.Controls.Add(new LiteralControl("<br/>"));
			
			city.ID = "city";
			city.Width = 200;
			city.Style.Add("width","200px");

			this.Controls.Add(city);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][5].ToString() + ":"));
			
			RangeValidator birthdate_val = new RangeValidator();

			birthdate_val.Type = ValidationDataType.Date;
			birthdate_val.MaximumValue = DateTime.Now.ToString("dd/MM/yyyy");
			birthdate_val.MinimumValue = "01/01/1900";
			birthdate_val.ID = "birthdate_val";
			birthdate_val.ControlToValidate = "birthdate";
			birthdate_val.ErrorMessage = arrInfos[lId][12].ToString();
			birthdate_val.Display = ValidatorDisplay.Dynamic;

			this.Controls.Add(birthdate_val);			

			this.Controls.Add(new LiteralControl("<br/>"));
			
			birthdate.ID = "birthdate";
			birthdate.Width = 15;
			birthdate.MaxLength = 10;
			birthdate.Text = "01/01/1900";
			birthdate.Style.Add("width","85px");
			birthdate.Attributes.Add("onfocus","if(this.value == '01/01/1900')this.value = '';");
			birthdate.Attributes.Add("onblur","if(this.value == '')this.value = '01/01/1900';");

			this.Controls.Add(birthdate);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][6].ToString() + ":"));
	
			this.Controls.Add(new LiteralControl("<br/>"));

			phone.ID = "phone";
			phone.Width = 200;
			phone.Style.Add("width","200px");

			this.Controls.Add(phone);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][7].ToString() + ":"));
				
			this.Controls.Add(new LiteralControl("<br/>"));

			fax.ID = "fax";
			fax.Width = 200;
			fax.Style.Add("width","200px");

			this.Controls.Add(fax);

			this.Controls.Add(new LiteralControl("<br/><br/>" + arrInfos[lId][8].ToString() + ":"));

			RegularExpressionValidator email_reg = new RegularExpressionValidator();
			email_reg.ControlToValidate = "email";
			email_reg.ErrorMessage = arrInfos[lId][13].ToString();
			email_reg.ID = "email_reg";
			email_reg.Display = ValidatorDisplay.Dynamic;				
			email_reg.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
			this.Controls.Add(email_reg);

			this.Controls.Add(new LiteralControl("<br/>"));

			email.ID = "email";
			email.Width = 65;
			email.MaxLength = 255;
			email.Attributes["style"] = "width:200px;";
		
			this.Controls.Add(email);	
		
			this.Controls.Add(new LiteralControl("<br/><br/>"));

			access_www.ID = "access_www";
			access_www.Text = arrInfos[lId][9].ToString();
			access_www.Checked = true;

			this.Controls.Add(access_www);

			this.Controls.Add(new LiteralControl("<br/>"));

			Button submit = new Button();

			submit.ID = "submit";
			submit.Width = 360;
			submit.Style.Add("width","360px");
			submit.Style.Add("margin-top","20px");
			submit.Text = arrInfos[lId][10].ToString();
			submit.Click += new EventHandler(updateInfo);

			this.Controls.Add(submit);

			// Her sættes værdier hvis punktet har været gemt

			Database db = new Database();

			string strSql = "SELECT CONCAT(firstname,' ',lastname) AS name,birthdate,address,zipcode,city,phone,fax,email,access_www,password,enddate FROM users INNER JOIN user_client ON userid = users.id WHERE users.id = " + intId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				usercode.Text = objDr["password"].ToString();
				name.Text = objDr["name"].ToString();
				address.Text = objDr["address"].ToString();
				zipcode.Text = objDr["zipcode"].ToString();
				city.Text = objDr["city"].ToString();
				phone.Text = objDr["phone"].ToString();
				fax.Text = objDr["fax"].ToString();
				email.Text = objDr["email"].ToString();
				birthdate.Text = objDr["birthdate"].ToString().Substring(0,10).Replace("-","/");
				enddate.Text = Convert.ToDateTime(objDr["enddate"]).ToString("dd/MM/yyyy").Replace("-","/");

				datEndTime = Convert.ToDateTime(objDr["enddate"]);
					
				if(Convert.ToInt32(objDr["access_www"]) == 0)
				{
					access_www.Checked = false;
				}

				enddate.Attributes.Add("onfocus","if(this.value == '" + Convert.ToDateTime(objDr["enddate"]).ToString("dd/MM/yyyy").Replace("-","/") + "')this.value = '';");
				enddate.Attributes.Add("onblur","if(this.value == '')this.value = '" + Convert.ToDateTime(objDr["enddate"]).ToString("dd/MM/yyyy").Replace("-","/") + "';");
			}
		
			db.objDataReader.Close();
			db = null;
		}

		private void drawDetailsPage() //Viser detaljer for en klient
		{
			string[][] arrInfos = new string[5][];

			arrInfos[1] = new string[] {"Journalkort for:","Kommentar:","Træningstid:","Type","Oprettelsesdato","Opret kontrolmåling","Opret slutmåling","Ret stamdata","Se klient-log","Print klientinstruktioner","Se træningsprogram","Ingen målinger fundet.","Journalkort start","Kontrolmåling","Journalkort slut","Print træningsprogram","Anamnese", "år"};
			arrInfos[2] = new string[] {"!Journalkort for:","!Kommentar:","!Træningstid:","!Type","!Oprettelsesdato","!Opret kontrolmåling","!Opret slutmåling","!Ret stamdata","!Se klient-log","!Print klientinstruktioner","!Se træningsprogram","!Ingen målinger fundet.","!Journalkort start","!Kontrolmåling","!Journalkort slut","!Print træningsprogram","!Anamnese", "År"};
			arrInfos[3] = new string[] { "/Case Record for:".Substring(1) ,"/Comments:".Substring(1),"/Training periode:".Substring(1),"/Type".Substring(1),"/Start date".Substring(1),"/Make Check-ups".Substring(1),"/Make Case record - Endpoint".Substring(1),"/Adjust the main data of the patient".Substring(1),"/Watch the patient-log".Substring(1),"/Print Patient Instructions".Substring(1),"/Watch the traning programe".Substring(1)
										   ,"/No check ups found.".Substring(1),"/Case record start".Substring(1),"/Checkup".Substring(1),"/Case record end".Substring(1),"/Print training programe".Substring(1),"/Anamnesis".Substring(1),"years"};
			arrInfos[4] = new string[] { "=Case Record für:".Substring(1),
										 "=Bemerkungen:".Substring(1),
										"=Trainingseinheiten:".Substring(1),
										"=Typ".Substring(1),
										"=Start Datum".Substring(1),
										"=Zwischenüberprüfungen".Substring(1),
										"=Einen Case Record Beenden".Substring(1),
										"=Kundendaten überarbeiten".Substring(1),
										"=Kunden-Protokoll ansehen".Substring(1),
										"=Kunden-Anweisungen audrucken".Substring(1),
										"=Das Trainingsprogramm überwachen".Substring(1),
										"=No check ups found.".Substring(1),
										"=Case record start".Substring(1),
										"=Zwischenüberprüfungen".Substring(1),
										"=Einen Case Record Beenden".Substring(1),
										"=Das Trainingsprogramm ausdrucken".Substring(1),
										"=Anamnesis".Substring(1),
										"Jahre"};

			this.Controls.Add(new LiteralControl("<p class='bold_text'>" + arrInfos[lId][0].ToString() + "</p>"));			

			Database db = new Database();

			string strSql = "SELECT users.id,CONCAT(firstname,' ',lastname) AS name,address,zipcode,city,email,phone,birthdate,comments,guide,password ";
			strSql += "FROM users INNER JOIN user_client ON users.id = userid INNER JOIN test_schedule ON clientid = users.id WHERE users.id = " + intId;

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				if(CheckNull(objDr["name"]))
				{
					this.Controls.Add(new LiteralControl(objDr["name"].ToString()));
				}
				if(CheckNull(objDr["address"]))
				{
					this.Controls.Add(new LiteralControl("<br/>" + objDr["address"].ToString()));
				}
				if(CheckNull(objDr["zipcode"]) && CheckNull(objDr["city"]))
				{
					this.Controls.Add(new LiteralControl("<br/>" + objDr["zipcode"].ToString() + " " + objDr["city"].ToString()));
				}
				if(CheckNull(objDr["email"]))
				{
					this.Controls.Add(new LiteralControl("<br/><a href='mailto:" + objDr["email"].ToString() + "'>" + objDr["email"].ToString() + "</a>"));
				}
				if(CheckNull(objDr["phone"]))
				{
					this.Controls.Add(new LiteralControl("<br/>" + objDr["phone"].ToString()));
				}
				if(((Optician)Session["user"]).getAge(Convert.ToDateTime(objDr["birthdate"])) < 110)
				{
					this.Controls.Add(new LiteralControl("<br/>" + ((Optician)Session["user"]).getAge(Convert.ToDateTime(objDr["birthdate"])).ToString() + " "+ arrInfos[lId][17].ToString()+"."));
				}
				
				this.Controls.Add(new LiteralControl("<br/>" + objDr["password"].ToString()));
				

				this.Controls.Add(new LiteralControl("<p class='bold_text'>" + arrInfos[lId][1].ToString() + "</p>"));

				if(CheckNull(objDr["comments"]))
				{
					this.Controls.Add(new LiteralControl(objDr["comments"].ToString()));
				}

				this.Controls.Add(new LiteralControl("<p class='bold_text'>" + arrInfos[lId][2].ToString() + "</p>"));

				if(CheckNull(objDr["guide"]))
				{
					this.Controls.Add(new LiteralControl(objDr["guide"].ToString()));
				}
			}

			db.objDataReader.Close();

			HtmlTable objHt = new HtmlTable();
			objHt.Style.Add("width","475px");
			objHt.Style.Add("margin-top","15px");
			objHt.Attributes["class"] = "data_table";
			objHt.CellPadding = 0;
			objHt.CellSpacing = 0;
			objHt.ID = "datatable";

			HtmlTableRow objHtr = new HtmlTableRow();

			HtmlTableCell objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.Style.Add("width","350px");
			objHtc.InnerHtml = arrInfos[lId][3].ToString();

			objHtr.Controls.Add(objHtc);
			
			objHtc = new HtmlTableCell();
			objHtc.Attributes["class"] = "data_table_header";
			objHtc.Style.Add("width","125px");
			objHtc.InnerHtml = arrInfos[lId][4].ToString();

			objHtr.Controls.Add(objHtc);
			objHt.Controls.Add(objHtr);

			db = new Database();
			strSql = "SELECT addedtime FROM a_21 WHERE clientid = " + intId + " AND isfirst = 1;";

			objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				objHtr = new HtmlTableRow();

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.ColSpan = 2;
				objHtc.InnerHtml = arrInfos[lId][11].ToString();

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
			}
			if(objDr.Read())
			{
				objHtr = new HtmlTableRow();

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = "<a href='#' onclick=\"window.open('popups/Testcard.aspx?clientid=" + intId + "&isfirst=true','testcard','width=635,height=580,scrollbars=yes,toolbar=no,resizeable=no');\">" + arrInfos[lId][12].ToString() + "</a> - <a href='#' onclick=\"window.open('popups/Anamnese.aspx?clientid=" + intId + "&isfirst=1','anamnese','width=635,height=580,scrollbars=yes,toolbar=no,resizeable=no');\">" +arrInfos[lId][16].ToString() + "</a>";

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Attributes["class"] = "data_table_item";
				objHtc.InnerHtml = Convert.ToDateTime(objDr["addedtime"]).ToString("dd-MM-yyyy HH:mm");

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);		
	
				Database db1 = new Database();
				string strSql1 = "SELECT id,addedtime FROM a_control WHERE clientid = " + intId + " ORDER BY addedtime;";
				MySqlDataReader objDr1 = db1.select(strSql1);

				while(objDr1.Read())
				{
					objHtr = new HtmlTableRow();

					objHtc = new HtmlTableCell();
					objHtc.Attributes["class"] = "data_table_item";
					objHtc.InnerHtml = "<a href='#' onclick=\"window.open('popups/Controlcard.aspx?id=" + objDr1["id"].ToString() + "','controlcard','width=635,height=400,scrollbars=yes,toolbar=no,resizeable=no');\">" +arrInfos[lId][13].ToString() + "</a>";
					objHtr.Controls.Add(objHtc);

					objHtc = new HtmlTableCell();
					objHtc.Attributes["class"] = "data_table_item";
					objHtc.InnerHtml = Convert.ToDateTime(objDr1["addedtime"]).ToString("dd-MM-yyyy HH:mm");

					objHtr.Controls.Add(objHtc);

					objHt.Controls.Add(objHtr);
				}

				db1.objDataReader.Close();

				db1 = new Database();
				strSql1 = "SELECT addedtime FROM a_21 WHERE clientid = " + intId + " AND isfirst = 0;";
						
				objDr1 = db1.select(strSql1);

				if(objDr1.Read())
				{
					objHtr = new HtmlTableRow();

					objHtc = new HtmlTableCell();
					objHtc.Attributes["class"] = "data_table_item";
					objHtc.InnerHtml = "<a href='#' onclick=\"window.open('popups/Testcard.aspx?clientid=" + intId + "&isfirst=false','testcard','width=635,height=580,scrollbars=yes,toolbar=no,resizeable=no');\">" +arrInfos[lId][14].ToString() + "</a> - <a href='#' onclick=\"window.open('popups/Anamnese.aspx?clientid=" + intId + "&isfirst=0','anamnese','width=635,height=580,scrollbars=yes,toolbar=no,resizeable=no');\">" +arrInfos[lId][16].ToString() + "</a>";

					objHtr.Controls.Add(objHtc);

					objHtc = new HtmlTableCell();
					objHtc.Attributes["class"] = "data_table_item";
					objHtc.InnerHtml = Convert.ToDateTime(objDr1["addedtime"]).ToString("dd-MM-yyyy HH:mm");

					objHtr.Controls.Add(objHtc);
					objHt.Controls.Add(objHtr);	

					control.Visible = false;
					end.Visible = false;
				}

				db1.objDataReader.Close();
			
			}

			db.objDataReader.Close();
		
			this.Controls.Add(objHt);

			this.Controls.Add(new LiteralControl("<ul>"));

			HtmlAnchor schedule = new HtmlAnchor();
			schedule.HRef = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=schedule&id=" + intId;
			schedule.InnerHtml = "<li>" + arrInfos[lId][10].ToString() + "</li>";

			this.Controls.Add(schedule);

			HtmlAnchor schedulePrint = new HtmlAnchor();
			schedulePrint.HRef = "javascript:void(0);";
			schedulePrint.Attributes["onclick"] = "window.open('popups/print.aspx?mode=schedule&type=optician&id=" + intId + "','print','width=550,height=580,toolbar=no,scrollbars=yes,resizeable=no');";
			schedulePrint.InnerHtml = "<li>" + arrInfos[lId][15].ToString() + "</li>";

			this.Controls.Add(schedulePrint);



			end.HRef = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=end&id=" + intId + "&ref=" +IntSubmenuId;;
			end.InnerHtml = "<li>" + arrInfos[lId][6].ToString() + "</li>";

			this.Controls.Add(end);

			control.HRef = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=control&id=" + intId + "&ref=" +IntSubmenuId;
			control.InnerHtml = "<li>" + arrInfos[lId][5].ToString() + "</li>";

			this.Controls.Add(control);

			HtmlAnchor info = new HtmlAnchor();
			info.HRef = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=editinfo&id=" + intId;
			info.InnerHtml = "<li>" + arrInfos[lId][7].ToString() + "</li>";

			this.Controls.Add(info);

			HtmlAnchor log = new HtmlAnchor();
			log.HRef = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=log&id=" + intId;
			log.InnerHtml = "<li>" + arrInfos[lId][8].ToString() + "</li>";

			this.Controls.Add(log);

			HtmlAnchor print = new HtmlAnchor();
			print.HRef = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=print&id=" + intId;
			print.InnerHtml = "<li>" + arrInfos[lId][9].ToString() + "</li>";

			this.Controls.Add(print);

			// Original Program
			Database db2 = new Database();
			string strSql2 = "SELECT count(*) as records from test_schedule where clientid = "+ intId;
						
			MySqlDataReader objDr2 = db2.select(strSql2);
			if ( objDr2.Read() ) 
			{
				if ( Convert.ToInt32(objDr2["records"]) > 1 )
				{
					schedule = new HtmlAnchor();
					schedule.ID = "OriginalSchedule";
					schedule.HRef = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=scheduleOrig&id=" + intId;
					schedule.InnerHtml = "<li>" + arrInfos[lId][10].ToString() + " Original</li>";
					this.Controls.Add(schedule);					
				}
			}
			objDr2.Close();
			db2 = null;
			// Original Program

			this.Controls.Add(new LiteralControl("</ul>"));

		}

		private void drawSchedulePage()
		{
			Literal js = new Literal();
			js.Text = "<script type='text/javascript'>\n";
			js.Text += "function handleLB(objCB,intId){\n";
			js.Text += "if(objCB.checked == true){\n";
			js.Text += "document.getElementById('_"+ Files.strCtl +"_lb_'+intId).disabled = false;\n";
			js.Text += "document.getElementById('_"+ Files.strCtl +"_cell_'+intId).style.color = 'black';\n";
			js.Text += "}else{\n";
			js.Text += "document.getElementById('_"+ Files.strCtl +"_lb_'+intId).disabled = true;\n";
			js.Text += "document.getElementById('_"+ Files.strCtl +"_lb_'+intId).selectedIndex = '0';\n";
			js.Text += "document.getElementById('_"+ Files.strCtl +"_cell_'+intId).style.color = '#666666';\n";
			js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).src = 'gfx/no_lock.gif';\n";
			js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).alt = '';\n";
			js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).title = '';\n";
			js.Text += "}\n";
			js.Text += "}\n";

			Database db = new Database();
            string strSql = "SELECT tests.id FROM tests WHERE languageid = " + ((tye.Menu)Session["menu"]).IntLanguageId + " ORDER BY priority;";
			MySqlDataReader objDr = db.select(strSql);

			js.Text += "function handleLBonLoad(){\n";

			while(objDr.Read())
			{
				js.Text += "if(document.getElementById('_" + Files.strCtl + "_cb_" + objDr["id"].ToString() + "').checked == true){\n";
					js.Text += "document.getElementById('_"+ Files.strCtl +"_lb_" + objDr["id"].ToString() + "').disabled = false;\n";
					js.Text += "document.getElementById('_"+ Files.strCtl +"_cell_" + objDr["id"].ToString() + "').style.color = 'black';\n";
				js.Text += "}else{\n";
					js.Text += "document.getElementById('_"+ Files.strCtl +"_lb_" + objDr["id"].ToString() + "').disabled = true;\n";
					js.Text += "document.getElementById('_"+ Files.strCtl +"_cell_" + objDr["id"].ToString() + "').style.color = '#666666';\n";
				js.Text += "}\n";

				js.Text += "if(document.getElementById('_"+ Files.strCtl +"_lb_" + objDr["id"].ToString() + "').value == '1'){\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').src = 'gfx/no_lock.gif';\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').alt = '';\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').title = '';\n";
				js.Text += "}else if(document.getElementById('_"+ Files.strCtl +"_lb_" + objDr["id"].ToString() + "').value == '0'){\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').src = 'gfx/program_lock.gif';\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').alt = '';\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').title = '';\n";
				js.Text += "}else if(document.getElementById('_"+ Files.strCtl +"_lb_" + objDr["id"].ToString() + "').value == '-1'){\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').src = 'gfx/optician_lock.gif';\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').alt = '';\n";
					js.Text += "document.getElementById('_" + Files.strCtl + "_lock_" + objDr["id"].ToString() + "').title = '';\n";
				js.Text += "}\n";

			}

			db.objDataReader.Close();
			db = null;

			js.Text += "}\n";

			js.Text += "function changeLock(intId){\n";
			js.Text += "if(document.getElementById('_"+ Files.strCtl +"_lb_'+intId).value == '1'){\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).src = 'gfx/no_lock.gif';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).alt = '';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).title = '';\n";
			js.Text += "}else if(document.getElementById('_"+ Files.strCtl +"_lb_'+intId).value == '0'){\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).src = 'gfx/program_lock.gif';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).alt = '';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).title = '';\n";
			js.Text += "}else if(document.getElementById('_"+ Files.strCtl +"_lb_'+intId).value == '-1'){\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).src = 'gfx/optician_lock.gif';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).alt = '';\n";
				js.Text += "document.getElementById('_" + Files.strCtl + "_lock_'+intId).title = '';\n";
			js.Text += "}\n";
			js.Text += "}\n";
			js.Text += "</script>";

			Head_ph.Controls.Add(js);
			Page_body.Attributes["onload"] = "handleLBonLoad();";
						
			this.Controls.Add(new Schedule().getOpticianSchedule(intId, strMode));	
		}

		private void drawListPage() //Viser en liste over klienter for den pågældende optiker
		{
			string[][] headerinfo = new string[5][];
			headerinfo[1] = new string[]{"Navn", "Oprettet"};
			headerinfo[2] = new string[]{"Navn", "Oprettet"};
			headerinfo[3] = new string[]{"Name", "Created"};
			headerinfo[4] = new string[]{"Name", "Created"};

			Database db = new Database();

			string strSql = "SELECT users.id,CONCAT(firstname,' ',lastname) AS name,enddate,DATE_FORMAT(addedtime,'%d-%m-%Y %I:%i') AS thedate FROM users INNER JOIN user_client ON user_client.userid = users.id WHERE opticianid = " + ((Optician)Session["user"]).IntUserId + " order by name;";

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
			objHlc.DataNavigateUrlFormatString = "../../?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=details&id={0}";
			
			objHlc.HeaderText = headerinfo[lId][0].ToString();//"Navn:";
			
			objHlc.DataTextField = "name";
			objHlc.HeaderStyle.CssClass = "data_table_header";
			objHlc.ItemStyle.CssClass = "data_table_item";

			objDg.Columns.Add(objHlc);

			BoundColumn objBc = new BoundColumn();

			objBc.DataField = "thedate";
			 
			objBc.HeaderText = headerinfo[lId][1].ToString();//"Oprettet:";
			objBc.HeaderStyle.CssClass = "data_table_header";
			objBc.ItemStyle.CssClass = "data_table_item";
			objBc.HeaderStyle.Width = 120;
			objBc.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			objBc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
			objDg.Columns.Add(objBc);
		
			objDg.DataBind();

			this.Controls.Add(objDg);

			if(!(db.objDataReader.HasRows))
			{
				db.objDataReader.Close();
				this.Controls.Add(new LiteralControl(new NoDataFound().Message(((User)Session["user"]).IntLanguageId).ToString()));
			}

			db.objDataReader.Close();

			db = null;
		}

		private void updateInfo(object sender, EventArgs e) //Gemmer stamdata
		{
			if(Page.IsValid)
			{
				Wysiwyg wys = new Wysiwyg();

				int intAccess = 0;
				int isActive = 0;
				string birthdateToDb = Convert.ToDateTime(birthdate.Text).ToString("yyyy-MM-dd");

				if(access_www.Checked == true)
				{
					intAccess = 1;
				}
				
				DateTime tmp_enddate = Convert.ToDateTime(enddate.Text);

				if(Convert.ToDateTime(tmp_enddate.ToString("dd-MM-yyyy")) >= Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MM-yyyy"))) {
					isActive = 1;
					DateTime datetest1 = Convert.ToDateTime(tmp_enddate.ToString("dd-MM-yyyy"));
					DateTime datetest2 = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MM-yyyy"));
					string test = "";
				}

				string strSql = "UPDATE user_client SET firstname = '" + wys.ToDb(1,((Optician)Session["user"]).getFirstName(name.Text)) + "',lastname = '" + wys.ToDb(1,((Optician)Session["user"]).getLastName(name.Text)) + "',birthdate = '" + birthdateToDb + "', enddate = '" + Convert.ToDateTime(enddate.Text).ToString("yyyy-MM-dd") + "',access_www = " + intAccess + " WHERE userid = " + intId + ";";
				
				strSql += "UPDATE users SET address = '" + wys.ToDb(1,address.Text) + "',zipcode = '" + wys.ToDb(1,zipcode.Text) + "', isactive = "+isActive+", city = '" + wys.ToDb(1,city.Text) + "',phone = '" + wys.ToDb(1,phone.Text) + "',fax = '" + wys.ToDb(1,fax.Text) + "',email = '" + wys.ToDb(1,email.Text) + "' WHERE users.id = " + intId + ";";

				Database db = new Database();
				db.execSql(strSql);

				db = null;
				wys = null;

				if(!(Convert.ToDateTime(datEndTime.ToString("dd-MM-yyyy")).Equals(Convert.ToDateTime(enddate.Text).ToString("dd-MM-yyyy"))))
				{
					LogEndTime let = new LogEndTime(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),((Optician)Session["user"]).IntUserId,intId,Convert.ToDateTime(Convert.ToDateTime(enddate.Text).ToString("yyyy-MM-dd")),Request.UserHostAddress.ToString());
					let = null;
				}

				Response.Redirect("?page=" + IntPageId + "&submenu=" + IntSubmenuId + "&mode=details&id=" + intId);
			}
		}

		public string getTestNameFromDate ( int intUserId, int intLanguageId, DateTime datAddedTime ) 
		{
			string strAddedTime = datAddedTime.ToString("yyyy-MM-dd HH:mm:ss");
			string returnName ="";
			Database db = new Database();
			string strSql = "SELECT tests.name FROM log_testresult "+	
				" inner join tests on ( tests.priority = log_testresult.testid  ) " +
				" WHERE clientid = "+ intUserId +" AND addedtime = '"+strAddedTime+"' and tests.languageid = " + intLanguageId.ToString();
				MySqlDataReader objDr = db.select(strSql);
			while(objDr.Read())
			{
				returnName = objDr["name"].ToString()+"<BR>";
			}
				db = null;
				
			return returnName;
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
