using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;
using System.Web.SessionState;
using System.Web.Mail;
using tye.exceptions;

namespace tye
{
	/// <summary>
	/// Summary description for Optician.
	/// </summary>
	public class Optician : User
	{
		private string strName;
		private string strOpticianCode;
		private int intRegionId;
		private int intOpticianCodeId;
		private int intOpticianChainId;
		private string strTyeLanguageId;

		public Optician()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string StrName
		{
			get
			{
				return strName;
			}
			set
			{
				strName = value;
			}
		}
		
		public string StrOpticianCode
		{
			get
			{
				return strOpticianCode;
			}
			set
			{
				strOpticianCode = value;
			}
		}

		public int IntRegionId
		{
			get
			{
				return intRegionId;
			}
			set
			{
				intRegionId = value;
			}
		}

		public int IntOpticianCodeId
		{
			get
			{
				return intOpticianCodeId;
			}
			set
			{
				intOpticianCodeId = value;
			}
		}

		public int IntOpticianChainId
		{
			get
			{
				return intOpticianChainId;
			}
			set
			{
				intOpticianChainId = value;
			}
		}

		public string StrTyeLanguageId
		{
			get
			{
				return strTyeLanguageId;
			}
			set
			{
				strTyeLanguageId = value;
			}
		}

		new public void CreateObj(string strPassword)
		{
			string strSql = "SELECT users.id,address,zipcode,city,email,phone,usertypes.name AS usertype_name,usertypeid,user_optician.name AS username,optician_code.optician,language.tyeid,languageid,optician_chain.name AS chainname";
			strSql += " FROM users INNER JOIN usertypes ON usertypeid = usertypes.id INNER JOIN user_optician ON users.id = user_optician.userid INNER JOIN optician_code ON opticiancodeid = optician_code.id";
			strSql += " INNER JOIN language ON optician_code.languageid = language.id INNER JOIN optician_chain ON chainid = optician_chain.id WHERE password = '" + strPassword + "' AND users.isactive = 1;";

			Database db = new Database();
						
			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read() == true)
			{
				Optician user = new Optician();

				user.IntUserId = Convert.ToInt32(objDr["id"]);
				user.IntLanguageId = Convert.ToInt32(objDr["languageid"]);
				user.StrOpticianCode = objDr["tyeid"].ToString() + objDr["chainname"].ToString() + objDr["optician"].ToString();
				user.IntUserTypeId = Convert.ToInt32(objDr["usertypeid"]);
				user.StrZipCode = objDr["zipcode"].ToString();
				user.StrAddress = objDr["address"].ToString();
				user.StrCity = objDr["city"].ToString();
				user.StrEmail = objDr["email"].ToString();
				user.StrName = objDr["username"].ToString();
				user.StrPhone = objDr["phone"].ToString();

				((Menu)Session["menu"]).IntLanguageId = Convert.ToInt32(user.IntLanguageId);
				((Menu)Session["menu"]).StrAccess = "access_" + objDr["usertype_name"].ToString();

				Session["user"] = user;

				user = null;
			}
			db.objDataReader.Close();
            db.dbDispose();
			db = null;
		}

		public void Add(Optician objO)
		{
			Database db = new Database();
			Database db_lock = new Database();
			Wysiwyg wys = new Wysiwyg();

			string strSql = "INSERT INTO users (password,addedtime,address,zipcode,city,email,phone,isactive,usertypeid) VALUES('";
			strSql += wys.ToDb(1,objO.StrPassword.ToString()) + "',";
			strSql += "CURRENT_TIMESTAMP(),'";
			strSql += wys.ToDb(1,objO.StrAddress.ToString()) + "','";
			strSql += wys.ToDb(1,objO.StrZipCode.ToString()) + "','";
			strSql += wys.ToDb(1,objO.StrCity.ToString()) + "','";
			strSql += wys.ToDb(1,objO.StrEmail.ToString()) + "','";
			strSql += wys.ToDb(1,objO.StrPhone.ToString()) + "',";
			strSql += Convert.ToInt32(objO.IntIsActive) + ",";
			strSql += Convert.ToInt32(objO.IntUserTypeId) + ")";

			db.execSql(strSql);

			strSql = "lock table users write;";

			db_lock.execSql(strSql);

			strSql = "SELECT id FROM users ORDER BY id desc LIMIT 0,1;";

			MySqlDataReader objDr = db.select(strSql);		

			if(objDr.Read())
			{
				objO.IntUserId = Convert.ToInt32(objDr["id"]);
			}

			db.objDataReader.Close();

            strSql = "unlock tables;";
			db_lock.execSql(strSql);

			strSql = "INSERT INTO optician_code (languageid,chainid,optician) VALUES('";
			strSql += Convert.ToInt32(objO.IntLanguageId) + "','";
			strSql += Convert.ToInt32(objO.IntOpticianChainId) + "','";
			strSql += objO.StrOpticianCode.ToString() + "');";

			db.execSql(strSql);

			strSql = "lock table optician_code write;";

			db_lock.execSql(strSql);

			strSql = "SELECT id	FROM optician_code ORDER BY id desc LIMIT 0,1;";

			objDr = db.select(strSql);			

			if(objDr.Read())
			{
				objO.IntOpticianCodeId = Convert.ToInt32(objDr["id"]);
			}

			db.objDataReader.Close();

			strSql = "unlock tables;";
			db_lock.execSql(strSql);

            strSql = "INSERT INTO user_optician (userid,opticiancodeid,name,regionid) VALUES('";
			strSql += Convert.ToInt32(objO.IntUserId) + "','";
			strSql += Convert.ToInt32(objO.IntOpticianCodeId) + "','";
			strSql += wys.ToDb(1,objO.StrName.ToString()) + "','";
			strSql += Convert.ToInt32(objO.IntRegionId) + "');";

			db.execSql(strSql);
            
            db.dbDispose();
            db_lock.dbDispose();

			db_lock = null;
			db = null;
			
			Session["noerror"] = "<div id='noerror'>Optikeren <span class='bold_text'>" + objO.strName + "</span> er nu gemt med kodeordet <span class='bold_text'>" + objO.StrPassword + "</span>. Der er sendt en mail til optikeren med de nødvendige oplysninger.</div>";
			
			sendMailToOptician(objO);
			
			objO = null;
		}

		private void sendMailToOptician(Optician objO)
		{
			string[] arrMailBody = new string[5];
			string[] arrMailHeader = new string[5] {"","Du er nu oprettet som bruger","!Du er nu oprettet som bruger","TrainYourEyes.com has hereby entered you in our system", "TrainYourEyes.com hat Sie nun ins System aufgenommen"};

			arrMailBody[0] = "";
			arrMailBody[1] = "Kære [navn],\n\nTrainYourEyes har hermed oprettet dig i vores system som du kan finde på http://www.trainyoureyes.com.\n\nDu er blevet oprettet med følgende oplysninger:\nKodeord: [kodeord]\nNavn: [navn]\nAdresse: [adresse]\nPost nr. & By: [postnr] [by]\nTelefon: [telefon]\nEmail: [email]\n\nDu bedes kontrollere om disse oplysninger er korrekte, hvis ikke så send venligt en email til maria@trainyoureyes.com med de korrekte oplysninger.\n\nPå http://www.trainyoureyes.com kan du finde svar på oftestillede spørgsmål, læse om TrainYourEyes, kontakte TrainYourEyes og meget mere.\n\nVenlig hilsen\n\nMaria Beadle Kops\nOptometrist og indehaver af TrainYourEyes";
			arrMailBody[2] = "!Kære [navn],\n\nTrainYourEyes har hermed oprettet dig i vores system som du kan finde på http://www.trainyoureyes.com.\n\nDu er blevet oprettet med følgende oplysninger:\nKodeord: [kodeord]\nNavn: [navn]\nAdresse: [adresse]\nPost nr. & By: [postnr] [by]\nTelefon: [telefon]\nEmail: [email]\n\nDu bedes kontrollere om disse oplysninger er korrekte, hvis ikke så send venligt en email til maria@trainyoureyes.com med de korrekte oplysninger.\n\nPå http://www.trainyoureyes.com kan du finde svar på oftestillede spørgsmål, læse om TrainYourEyes, kontakte TrainYourEyes og meget mere.\n\nVenlig hilsen\n\nMaria Beadle Kops\nOptometrist og indehaver af TrainYourEyes";
			arrMailBody[3] = "Dear [navn],\n\nTrainYourEyes.com has hereby entered you in our system, which you can find at http://www.trainyoureyes.com.\n\nYou’ve been entered with the following information:\nPassword: [kodeord]\nName: [navn]\nAdress: [adresse]\nPostal code and city: [postnr] [by]\nTelephone: [telefon]\nE-mail: [email]\n\nDu bedes kontrollere om disse oplysninger er korrekte, hvis ikke så send venligt en email til maria@trainyoureyes.com med de korrekte oplysninger.\n\nPå http://www.trainyoureyes.com kan du finde svar på oftestillede spørgsmål, læse om TrainYourEyes, kontakte TrainYourEyes og meget mere.\n\nVenlig hilsen\n\nMaria Beadle Kops\nOptometrist og indehaver af TrainYourEyes";
			arrMailBody[4] = "Sehr geehrte Frau / sehr geehrter Herr [navn],\n\nTrainYourEyes.com hat Sie nun ins System aufgenommen. Sie finden es unter http://www.trainyoureyes.com.\n\nSie wurden mit folgenden Informationen registriert:\nPasswort: [kodeord]\nName: [navn]\nAdresse: [adresse]\nPostleitzahl und Ort: [postnr] [by]\nTelefon: [telefon]\nE-mail: [email]\n\nBitte überprüfen Sie diese Informationen auf ihre Richtigkeit. Falls sie fehlerhaft sein sollten, senden Sie bitte ein E-Mail mit den richtigen Informationen an maria@trainyoureyes.com.\n\nUnter http://www.trainyoureyes.com können Sie Antworten auf die am häufigsten gestellten Fragen (FAQ) finden, mehr über TrainYourEyes erfahren, TrainYourEyes kontaktieren und vieles mehr.\n\nMit besten Grüßen\n\nMaria Beadle Kops\nOptikerin und Eigentümerin von TrainYourEyes";

			MailMessage objMail = new MailMessage();
			objMail.To = objO.StrEmail;
			objMail.Subject = arrMailHeader[objO.IntLanguageId];
			objMail.Body = arrMailBody[objO.IntLanguageId].Replace("[navn]",objO.strName).Replace("[kodeord]",objO.StrPassword).Replace("[adresse]",objO.StrAddress).Replace("[postnr]",objO.StrZipCode).Replace("[by]",objO.StrCity).Replace("[telefon]",objO.StrPhone).Replace("[email]",objO.StrEmail);
			objMail.From = "noreply@trainyoureyes.com";
			objMail.BodyFormat = MailFormat.Text;
			//SmtpMail.SmtpServer = "websmtp.hardball.nu";
            SmtpMail.SmtpServer = "localhost";

			try
			{
				SmtpMail.Send(objMail);
				objMail = null;
			}
			catch(Exception MailEx)
			{
				string strErrMessage = MailEx.Message + "<br>" + MailEx.InnerException;
			}
		}

		public HtmlTable getControlCard(int intControlId)
		{
			int lId = ((Optician)Session["user"]).IntLanguageId;

			Hashtable htValue = new Hashtable();

			Database db = new Database();
			string strSql = "SELECT a_1,a_2,a_3,a_4,a_4_1,a_5,a_5_1,a_6,a_6_1,a_7,a_7_1,a_8,a_9 FROM a_control WHERE id = " + intControlId;

			MySqlDataReader objDr = db.select(strSql);

			if(!(objDr.HasRows))
			{
				throw new NoDataFound();
			}

			if(objDr.Read())
			{
				htValue.Add("1",objDr["a_1"].ToString());
				htValue.Add("2",objDr["a_2"].ToString());
				htValue.Add("3",objDr["a_3"].ToString());
				htValue.Add("4",objDr["a_4"].ToString());
				htValue.Add("4_1",objDr["a_4_1"].ToString());
				htValue.Add("5",objDr["a_5"].ToString());
				htValue.Add("5_1",objDr["a_5_1"].ToString());
				htValue.Add("6",objDr["a_6"].ToString());
				htValue.Add("6_1",objDr["a_6_1"].ToString());
				htValue.Add("7",objDr["a_7"].ToString());
				htValue.Add("7_1",objDr["a_7_1"].ToString());
				htValue.Add("8",objDr["a_8"].ToString());
				htValue.Add("9",objDr["a_9"].ToString());
			}

			db.objDataReader.Close();
            db.dbDispose();

			string[][] arrInfos = new string[5][];

			//Dansk
			arrInfos[1] = new string[] {"Konvergensnærpunkt","Motilitet","Ændringer siden sidst","Bemærkninger","Afstand:","Opgav:","Ubehag:","Højre øje:","Venstre øje:","Begge øjne:","Hovedbevægelser:"};
			//Norsk
			arrInfos[2] = new string[] {"Konvergensnærpunkt","Motilitet","Ændringer siden sidst","Bemærkninger","Afstand:","Opgav:","Ubehag:","Højre øje:","Venstre øje:","Begge øjne:","Hovedbevægelser:"};
			//Engelsk
			arrInfos[3] = new string[] {"Convergence near-point","Motility","Changes since last time","Comments","Distance:","Gave up:","Discomfort:","Right eye:","Left eye:","Both eyes:","Movements of the head:"};
			//Tysk
			arrInfos[4] = new string[] {"Konvergenz-Nahpunkt","Motilität","Änderungen seit dem letzten Mal"," Kommentare ","Entfernung:","Gab auf:","Unwohl:","Rechtes Auge:","Linkes Auge:","Beide Augen:","Bewegungen des Kopfes:"};

			string[][] arrResults = new string[5][];

			//Dansk
			arrResults[1] = new string[] {"0-5","5-10","10-15","15-20","20-25","25-30","30-35","35-40","> 40","Nej","Højre øje","Venstre øje","Ja","Ved ikke"};
			//Norsk
			arrResults[2] = new string[] {"0-5","5-10","10-15","15-20","20-25","25-30","30-35","35-40","> 40","Nej","Højre øje","Venstre øje","Ja","Ved ikke"};
			//Engelsk
			arrResults[3] = new string[] {"0-5","5-10","10-15","15-20","20-25","25-30","30-35","35-40","> 40","Nej","Højre øje","Venstre øje","Ja","Ved ikke"};
			//Tysk
			arrResults[4] = new string[] {"0-5","5-10","10-15","15-20","20-25","25-30","30-35","35-40","> 40","Nein","Rechtes Auge","Linkes Auge","Ja","Ved ikke"};

			HtmlTable objHt = new HtmlTable();

			db = new Database();
			strSql = "SELECT CONCAT(firstname,' ',lastname) AS name,address,zipcode,city,phone,email,a_control.addedtime FROM users INNER JOIN user_client ON userid = users.id INNER JOIN a_control ON users.id = clientid WHERE a_control.id = " + intControlId;
		
			objDr = db.select(strSql);

			HtmlTableRow objHtr = new HtmlTableRow(); //Brugerinfo

			HtmlTableCell objHtc = new HtmlTableCell();

			if(objDr.Read())
			{		
				objHt.Style.Add("width","600px");
				objHt.Style.Add("border","solid 1px #000000");
				objHt.Style.Add("border-collapse","collapse");
				objHt.CellPadding = 0;
				objHt.CellSpacing = 0;
				objHt.ID = "testcard";

				objHtc.Style.Add("width","600px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.ColSpan = 3;	
				objHtc.InnerHtml = "<div style='padding:3px;'>" + objDr["name"].ToString();
				if(objDr["address"].ToString() != "")
				{
					objHtc.InnerHtml += "<br/>" + objDr["address"].ToString();
				}
				if(objDr["zipcode"].ToString() != "" && objDr["city"].ToString() != "")
				{
					objHtc.InnerHtml += "<br/>" + objDr["zipcode"].ToString() + " " + objDr["city"].ToString();
				}
				if(objDr["phone"].ToString() != "")
				{
					objHtc.InnerHtml += "<br/>" + objDr["phone"].ToString();
				}
				if(objDr["email"].ToString() != "")
				{
					objHtc.InnerHtml += "<br/>" + objDr["email"].ToString();
				}
				
				objHtc.InnerHtml += "</div>";

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //Dato

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","600px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.ColSpan = 3;	
				objHtc.InnerHtml = "<div style='padding:3px;text-align:right;'>" + Convert.ToDateTime(objDr["addedtime"]).ToString("dd-MM-yyyy") + "</div>";

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
			}

			db.objDataReader.Close();

			objHtr = new HtmlTableRow(); //Konvergensnærpunkt, motilitet

			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","300px");
			objHtc.Style.Add("border","solid 1px #000000");
			objHtc.ColSpan = 2;
			objHtc.Style.Add("vertical-align","top");
			objHtc.Style.Add("padding-bottom","5px");

			HtmlGenericControl hdiv = new HtmlGenericControl("div");
			hdiv.ID = arrInfos[lId][5].ToString();
			hdiv.Attributes["class"] = "testcarddiv";
			hdiv.Style.Add("width","135px");
			hdiv.InnerHtml = arrInfos[lId][0].ToString().ToUpper();

			objHtc.Controls.Add(hdiv);
				
			objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][4].ToString() + " "));

			if(Convert.ToInt32(htValue["1"]) > 0)
			{
				objHtc.Controls.Add(new LiteralControl(arrResults[lId][Convert.ToInt32(htValue["1"])-1].ToString()));
			}

			if(Convert.ToInt32(htValue["2"]) == 1)
			{
				objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][5].ToString() + " " + arrResults[lId][9].ToString()));
			}
			else if(Convert.ToInt32(htValue["2"]) == 2)
			{
				objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][5].ToString() + " " + arrResults[lId][10].ToString()));
			}
			else if(Convert.ToInt32(htValue["2"]) == 3)
			{
				objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][5].ToString() + " " + arrResults[lId][11].ToString()));
			}

			if(Convert.ToInt32(htValue["2"]) == 1)
			{
				objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][5].ToString() + " " + arrResults[lId][12].ToString()));
			}
			else if(Convert.ToInt32(htValue["2"]) == 2)
			{
				objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][5].ToString() + " " + arrResults[lId][9].ToString()));
			}
			else
			{
				objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][5].ToString()));
			}

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","300px");
			objHtc.Style.Add("border","solid 1px #000000");
			objHtc.Style.Add("vertical-align","top");
			objHtc.Style.Add("padding-bottom","5px");
				
			hdiv = new HtmlGenericControl("div");
			hdiv.ID = arrInfos[lId][6].ToString();
			hdiv.Attributes["class"] = "testcarddiv";
			hdiv.Style.Add("width","135px");
			hdiv.InnerHtml = arrInfos[lId][1].ToString().ToUpper();

			objHtc.Controls.Add(hdiv);

			objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][7].ToString() + " " + htValue["4"].ToString() + "<div style='padding-left:3px;'>" + htValue["4_1"].ToString() + "</div>"));

			objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][8].ToString() + " " + htValue["5"].ToString() + "<div style='padding-left:3px;'>" + htValue["5_1"].ToString() + "</div>"));

			objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][9].ToString() + " " + htValue["6"].ToString() + "<div style='padding-left:3px;'>" + htValue["6_1"].ToString() + "</div>"));

			objHtc.Controls.Add(new LiteralControl("<br/>&nbsp;" + arrInfos[lId][10].ToString() + " " + htValue["7"].ToString() + "<div style='padding-left:3px;'>" + htValue["7_1"].ToString() + "</div>"));

			objHtr.Controls.Add(objHtc);
			objHt.Controls.Add(objHtr);

			objHtr = new HtmlTableRow(); //Bemærkninger

			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","300px");
			objHtc.Style.Add("border","solid 1px #000000");
			objHtc.ColSpan = 2;
			objHtc.Style.Add("vertical-align","top");
			objHtc.Style.Add("padding-bottom","5px");

			hdiv = new HtmlGenericControl("div");
			hdiv.ID = arrInfos[lId][5].ToString();
			hdiv.Attributes["class"] = "testcarddiv";
			hdiv.Style.Add("width","135px");
			hdiv.InnerHtml = arrInfos[lId][2].ToString().ToUpper();

			objHtc.Controls.Add(hdiv);
				
			objHtc.Controls.Add(new LiteralControl("<div style='padding-left:3px;'>" + htValue["8"].ToString() + "</div>"));

			objHtr.Controls.Add(objHtc);

			objHtc = new HtmlTableCell();
			objHtc.Style.Add("width","300px");
			objHtc.Style.Add("border","solid 1px #000000");
			objHtc.Style.Add("vertical-align","top");
			objHtc.Style.Add("padding-bottom","5px");
				
			hdiv = new HtmlGenericControl("div");
			hdiv.ID = arrInfos[lId][6].ToString();
			hdiv.Attributes["class"] = "testcarddiv";
			hdiv.Style.Add("width","135px");
			hdiv.InnerHtml = arrInfos[lId][3].ToString().ToUpper();

			objHtc.Controls.Add(hdiv);
			objHtc.Controls.Add(new LiteralControl("<div style='padding-left:3px;'>" + htValue["9"].ToString() + "</div>"));

			objHtr.Controls.Add(objHtc);
			objHt.Controls.Add(objHtr);
			db.Dispose();
			db = null;
			return objHt;

		}

		public HtmlTable getTestCard(int intUserId,bool isfirst)
		{
			int lId = ((Optician)Session["user"]).IntLanguageId;

			Hashtable htValue = new Hashtable();

			//Værdier fra konvergensnærpunkt
			Database db = new Database();
			string strSql = "SELECT c_1,c_2,c_3 FROM a_convergence WHERE clientid = " + intUserId;

			if(isfirst)
			{
				strSql += " AND isfirst = 1;";
			}
			else
			{
				strSql += " AND isfirst = 0;";
			}

			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read())
			{
				htValue.Add("c_1",From99To0(objDr["c_1"].ToString()));
				htValue.Add("c_2",From99To0(objDr["c_2"].ToString()));
				htValue.Add("c_3",From99To0(objDr["c_3"].ToString()));
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close(); objDr = null;

			//Værdier fra Motilitet
			db = new Database();
			strSql = "SELECT m_1,m_2,m_3,m_4 FROM a_motilitet WHERE clientid = " + intUserId;

			if(isfirst)
			{
				strSql += " AND isfirst = 1;";
			}
			else
			{
				strSql += " AND isfirst = 0;";
			}

			objDr = db.select(strSql);

			if(objDr.Read())
			{
				htValue.Add("m_1",From99To0(objDr["m_1"].ToString()));
				htValue.Add("m_2",From99To0(objDr["m_2"].ToString()));
				htValue.Add("m_3",From99To0(objDr["m_3"].ToString()));
				htValue.Add("m_4",From99To0(objDr["m_4"].ToString()));
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();

			//Værdier fra 21
			db = new Database();
			strSql = "SELECT 1_1,";
			
			for(int i = 2;i < 20;i++)
			{
				strSql += "1_" + i + ",";
			}

			for(int i = 1;i < 101;i++)
			{
				strSql += "2_" + i + ",";
			}

			strSql += "3_3,3_4h,3_4v,3_5h,3_5v,3_8,3_9,3_10,3_11,3_13A,3_13B,3_14Ah,3_15A,3_16A,3_16B,3_17A,3_17B,3_19,3_20,3_21 FROM a_21 WHERE clientid = " + intUserId;
			
			if(isfirst)
			{
				strSql += " AND isfirst = 1;";
			}
			else
			{
				strSql += " AND isfirst = 0;";
			}

			objDr = db.select(strSql);

			if(objDr.Read())
			{
				for(int i = 1;i < 20;i++)
				{
					htValue.Add("1_" + i,From99To0(objDr["1_" + i].ToString()));
				}

				for(int i = 1;i < 101;i++)
				{
					htValue.Add("2_" + i,From99To0(objDr["2_" + i].ToString()));
				}

				htValue.Add("3_3",objDr["3_3"].ToString());
				htValue.Add("3_4h",objDr["3_4h"].ToString());
				htValue.Add("3_4v",objDr["3_4v"].ToString());
				htValue.Add("3_5h",objDr["3_5h"].ToString());
				htValue.Add("3_5v",objDr["3_5v"].ToString());
				htValue.Add("3_8",objDr["3_8"].ToString());
				htValue.Add("3_9",objDr["3_9"].ToString());
				htValue.Add("3_10",objDr["3_10"].ToString());
				htValue.Add("3_11",objDr["3_11"].ToString());
				htValue.Add("3_13A",objDr["3_13A"].ToString());
				htValue.Add("3_13B",objDr["3_13B"].ToString());
				htValue.Add("3_14Ah",objDr["3_14Ah"].ToString());
				htValue.Add("3_15A",objDr["3_15A"].ToString());
				htValue.Add("3_16A",objDr["3_16A"].ToString());
				htValue.Add("3_16B",objDr["3_16B"].ToString());
				htValue.Add("3_17A",objDr["3_17A"].ToString());
				htValue.Add("3_17B",objDr["3_17B"].ToString());
				htValue.Add("3_19",objDr["3_19"].ToString());
				htValue.Add("3_20",objDr["3_20"].ToString());
				htValue.Add("3_21",objDr["3_21"].ToString());
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();

			string[][] arrInfos = new string[5][];

			//Dansk
			arrInfos[1] = new string[] {"Visus","Fixationsdisparitet","Stereopsis","Afstand:","Nær:","Dominans","Synsfelt","Covertest","Farvesynstest","Pupilrefleks","Motilitet","H:","V:","Begge:","Hovedbevægelser:","Konvergensnærpunkt","Opgav:","Indstillingsområder","Fra","til","Pinholevisus","Binoculært:","sf:","cyl:","Informativ række","Lag:","Netto:","Typesyndromet","s:","i:"};
			//Norsk
			arrInfos[2] = new string[] {"Visus","Fixationsdisparitet","Stereopsis","Afstand:","Nær:","Dominans","Synsfelt","Covertest","Farvesynstest","Pupilrefleks","Motilitet","H:","V:","Begge:","Hovedbevægelser:","Konvergensnærpunkt","Opgav:","Indstillingsområder","Fra","til","Pinholevisus","Binoculært:","sf:","cyl:","Informativ række","Lag:","Netto:","Typesyndromet","s:","i:"};
			//Engelsk
			arrInfos[3] = new string[] {"Visual acuity","Fixation disparity","Stereopsis","Distant:","Near:","Dominance","Field of view","Cover test","Colour vision test"," Pupillary reflex","Motility","R:","L:","Both:","Movements of the head:","Convergence near-point","Gave up:","Adjustment areas","From","To","Pin-hole visual acuity","Binocularly:","sf:","cyl:","Informative row","Layer:","Net:","The Type syndrome","s:","i:"};
			//Tysk
			arrInfos[4] = new string[] {"Visuelle Sehkraft","Fixation Disparität","Stereopsis","entfernt:","nah:","Dominanz","Gesichtsfeld","Abdecktest","Farsehtest"," Pupillenreflex","Motilität","R:","L:","Beide:","Bewegungen des Kopfes:","Konvergenz- Nahpunkt"," Gab auf:"," Regelung","von","nach","Nadelloch - visuelle Aktivität","Binokular:","Sph:","Zyl:","Informative Reihe","Ebene:","Netz:","Das Syndrom Typ","s:","i:"};

			string[][] arrResults = new string[5][];

			//Dansk
			arrResults[1] = new string[] {"Højre øje","Venstre øje","Ortofori","Esofori","Exofori","Lejlighedsvis esotropi","Lejlighedsvis exotropi","Stadig højre esotropi","Stadig venstre esotropi","Stadig højre exotropi","Stadig venstre exotropi","Alternerende esotropi","Alternerende exotropi","Hyperfori","Hypofori","Højre hypertropi","Venstre hypertropi","Højre hypotropi","Venstre hypotropi","Ja","Nej","Ved ikke","0-5","5-10","10-15","15-20","20-25","25-30","30-35","35-40","> 40","SI","LO","Orto/H","Hyperfori/V","Hyperfori/H","Hypofori/V","Hypofori/H"};
			//Norsk
			arrResults[2] = new string[] {"Højre øje","Venstre øje","Ortofori","Esofori","Exofori","Lejlighedsvis esotropi","Lejlighedsvis exotropi","Stadig højre esotropi","Stadig venstre esotropi","Stadig højre exotropi","Stadig venstre exotropi","Alternerende esotropi","Alternerende exotropi","Hyperfori","Hypofori","Højre hypertropi","Venstre hypertropi","Højre hypotropi","Venstre hypotropi","Ja","Nej","Ved ikke","0-5","5-10","10-15","15-20","20-25","25-30","30-35","35-40","> 40","SI","LO","Orto/H","Hyperfori/V","Hyperfori/H","Hypofori/V","Hypofori/H"};
			//Engelsk
			arrResults[3] = new string[] {"Højre øje","Venstre øje","Ortofori","Esofori","Exofori","Lejlighedsvis esotropi","Lejlighedsvis exotropi","Stadig højre esotropi","Stadig venstre esotropi","Stadig højre exotropi","Stadig venstre exotropi","Alternerende esotropi","Alternerende exotropi","Hyperfori","Hypofori","Højre hypertropi","Venstre hypertropi","Højre hypotropi","Venstre hypotropi","Ja","Nej","Ved ikke","0-5","5-10","10-15","15-20","20-25","25-30","30-35","35-40","> 40","SI","LO","Orto/H","Hyperfori/V","Hyperfori/H","Hypofori/V","Hypofori/H"};
			//Tysk
			arrResults[4] = new string[] {"Højre øje","Venstre øje","Ortofori","Esofori","Exofori","Lejlighedsvis esotropi","Lejlighedsvis exotropi","Stadig højre esotropi","Stadig venstre esotropi","Stadig højre exotropi","Stadig venstre exotropi","Alternerende esotropi","Alternerende exotropi","Hyperfori","Hypofori","Højre hypertropi","Venstre hypertropi","Højre hypotropi","Venstre hypotropi","Ja","Nej","Ved ikke","0-5","5-10","10-15","15-20","20-25","25-30","30-35","35-40","> 40","SI","LO","Orto/H","Hyperfori/V","Hyperfori/H","Hypofori/V","Hypofori/H"};

			HtmlTable objHt = new HtmlTable();

			db = new Database();
			strSql = "SELECT CONCAT(firstname,' ',lastname) AS name,address,zipcode,city,phone,email FROM users INNER JOIN user_client ON userid = users.id WHERE userid = " + intUserId;
		
			objDr = db.select(strSql);

			if(objDr.Read())
			{		
				objHt.Style.Add("width","600px");
				objHt.Style.Add("border","solid 1px #000000");
				objHt.Style.Add("border-collapse","collapse");
				objHt.CellPadding = 0;
				objHt.CellSpacing = 0;
				objHt.ID = "testcard";

				HtmlTableRow objHtr = new HtmlTableRow(); //Brugerinfo

				HtmlTableCell objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","600px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.ColSpan = 3;	
				objHtc.InnerHtml = objDr["name"].ToString();
				if(objDr["address"].ToString() != "")
				{
					objHtc.InnerHtml += "<br/>" + objDr["address"].ToString();
				}
				if(objDr["zipcode"].ToString() != "" && objDr["city"].ToString() != "")
				{
					objHtc.InnerHtml += "<br/>" + objDr["zipcode"].ToString() + " " + objDr["city"].ToString();
				}
				if(objDr["phone"].ToString() != "")
				{
					objHtc.InnerHtml += "<br/>" + objDr["phone"].ToString();
				}
				if(objDr["email"].ToString() != "")
				{
					objHtc.InnerHtml += "<br/>" + objDr["email"].ToString();
				}
				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //Visus, Fixationsdisparitet, Stereopsis

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");
				objHtc.ColSpan = 2;
				objHtc.RowSpan = 2;
				
				HtmlGenericControl hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][0].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][0].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				
                //if(Convert.ToDouble(htValue["1_11"]) > 0.00 && Convert.ToDouble(htValue["1_12"]) > 0.00 && Convert.ToDouble(htValue["1_13"]) > 0.00)
                //{
                //    objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + htValue["1_11"].ToString() + "<br/>"));
                //    objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + htValue["1_12"].ToString() + "<br/>"));
                //    objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][21].ToString() + " " + htValue["1_13"].ToString() + "<br/>"));
                //}
                //else
                //{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + htValue["1_14"].ToString() + "<br/>"));
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + htValue["1_15"].ToString() + "<br/>"));
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][21].ToString() + " " + htValue["1_16"].ToString() + "<br/>"));
				//}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][1].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][1].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["1_4"].ToString()));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
				objHtr = new HtmlTableRow(); 

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][2].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][2].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][3].ToString() + " " + htValue["1_5"].ToString()));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][4].ToString() + " " + htValue["1_6"].ToString()));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);
				objHtr = new HtmlTableRow(); //Dominans, Synsfelt

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.ColSpan = 2;
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][5].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][5].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				
				if(Convert.ToInt32(htValue["1_1"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrResults[lId][0].ToString()));
				}
				else if(Convert.ToInt32(htValue["1_1"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrResults[lId][1].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");
				
				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][6].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][6].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["1_8"].ToString()));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //Covertest,Farvesynstest

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.ColSpan = 2;
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][7].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][7].ToString().ToUpper();
				objHtc.Controls.Add(hdiv);

				if(Convert.ToInt32(htValue["1_2"]) > 0)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][3].ToString() + " " + arrResults[lId][Convert.ToInt32(htValue["1_2"])-1].ToString() + "<br/>"));
				}
				if(Convert.ToInt32(htValue["1_3"]) > 0)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][4].ToString() + " " + arrResults[lId][Convert.ToInt32(htValue["1_3"])-1].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");
				
				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][8].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][8].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;<br/>"));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //Pupilrefleks, Motilitet

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.ColSpan = 2;
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][9].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][9].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["1_7"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");
				
				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][10].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][10].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + htValue["m_1"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + htValue["m_2"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][13].ToString() + " " + htValue["m_3"].ToString() + "<br/>"));

				if(Convert.ToInt32(htValue["m_4"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][14].ToString() + " " + arrResults[lId][19].ToString()));
				}
				else if(Convert.ToInt32(htValue["m_4"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][14].ToString() + " " + arrResults[lId][20].ToString()));
				}
				else if(Convert.ToInt32(htValue["m_4"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][14].ToString() + " " + arrResults[lId][21].ToString()));
				}

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //Konvergensnærpunkt,Indstillingsområder

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.ColSpan = 2;
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][15].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][15].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);

				if(Convert.ToInt32(htValue["c_1"]) > 0)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][3].ToString() + " " + arrResults[lId][Convert.ToInt32(htValue["c_1"])+21].ToString() + " cm<br/>"));
				}
				if(Convert.ToInt32(htValue["c_2"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][16].ToString() + " " + arrResults[lId][20].ToString() + "<br/>"));
				}
				if(Convert.ToInt32(htValue["c_2"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][16].ToString() + " " + arrResults[lId][0].ToString() + "<br/>"));
				}
				if(Convert.ToInt32(htValue["c_2"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][16].ToString() + " " + arrResults[lId][1].ToString() + "<br/>"));
				}
		
				if(Convert.ToInt32(htValue["c_3"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][14].ToString() + " " + arrResults[lId][19].ToString()));
				}
				if(Convert.ToInt32(htValue["c_3"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][14].ToString() + " " + arrResults[lId][20].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");
				
				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][17].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][17].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][18].ToString() + " " + htValue["1_9"].ToString() + " cm " + arrInfos[lId][19].ToString() + " " + htValue["1_10"].ToString() + " cm"));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#3,Pinholevisus,#13A

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#3";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#3";
				objHtc.Controls.Add(hdiv);

				if(Convert.ToInt32(htValue["2_73"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_1"].ToString() + " " + arrResults[lId][3].ToString()));
				}
				if(Convert.ToInt32(htValue["2_73"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;0"));
				}
				if(Convert.ToInt32(htValue["2_73"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_1"].ToString() + " " + arrResults[lId][4].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_3"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_3"].ToString().ToLower() + ".gif' alt='" + htValue["3_3"].ToString() + "' id='3' />";
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");
				objHtc.RowSpan = 2;

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][20].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][20].ToString().ToUpper();
				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + htValue["1_17"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + htValue["1_18"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][21].ToString() + " " + htValue["1_19"].ToString()));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#3,Pinholevisus,#13A

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#13A";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#13A";
				objHtc.Controls.Add(hdiv);

				if(Convert.ToInt32(htValue["2_74"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_2"].ToString() + " " + arrResults[lId][3].ToString()));
				}
				if(Convert.ToInt32(htValue["2_74"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;0"));
				}
				if(Convert.ToInt32(htValue["2_74"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_2"].ToString() + " " + arrResults[lId][4].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_13A"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_13A"].ToString().ToLower() + ".gif' alt='" + htValue["3_13A"].ToString() + "' id='13A' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#4,Informativ række,#5

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#4";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#4";

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_75"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_75"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}
				
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_3"].ToString() + " " + arrInfos[lId][23].ToString() + " " + htValue["2_4"].ToString() + " " + htValue["2_5"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_6"].ToString() + "<br/>"));
				
				
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_76"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_76"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}
				
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_7"].ToString() + " " + arrInfos[lId][23].ToString() + " " + htValue["2_8"].ToString() + " " + htValue["2_9"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_10"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_4h"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_4h"].ToString().ToLower() + ".gif' alt='" + htValue["3_4h"].ToString() + "' id='4h' />";
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");
				objHtc.RowSpan = 2;

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][24].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][24].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;"));
				objHtc.Controls.Add(getInfRow(intUserId,isfirst));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#4,Informativ række,#5

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#5";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#5";

				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_77"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_77"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}
				
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_11"].ToString() + " " + arrInfos[lId][25].ToString() + " -" + htValue["2_12"].ToString() + " " + arrInfos[lId][26].ToString() + " " + htValue["2_13"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_78"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_78"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}
				
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_14"].ToString() + " " + arrInfos[lId][25].ToString() + " -" + htValue["2_15"].ToString() + " " + arrInfos[lId][26].ToString() + " " + htValue["2_16"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_5h"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_5h"].ToString().ToLower() + ".gif' alt='" + htValue["3_5h"].ToString() + "' id='5h' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#7,Typesyndrom,#7A

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#7";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#7";

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_79"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_79"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}
				
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_17"].ToString() + " " + arrInfos[lId][23].ToString() + " " + htValue["2_18"].ToString() + " " + htValue["2_19"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_20"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_80"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_80"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}				
				
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_21"].ToString() + " " + arrInfos[lId][23].ToString() + " " + htValue["2_22"].ToString() + " " + htValue["2_23"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_24"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","300px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");
				objHtc.RowSpan = 20;

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = arrInfos[lId][27].ToString();
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","135px");
				hdiv.InnerHtml = arrInfos[lId][27].ToString().ToUpper();

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("<div style='padding-left:5px;'>" + getTypeSyndrom(intUserId,true) + "</div>"));

				objHtr.Controls.Add(objHtc);
				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#7,Typesyndrom,#7A

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#7A";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#7A";

				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_81"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_81"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}	

				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_25"].ToString() + " " + arrInfos[lId][23].ToString() + " " + htValue["2_26"].ToString() + " " + htValue["2_27"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_28"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_82"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_82"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}				
				
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_29"].ToString() + " " + arrInfos[lId][23].ToString() + " " + htValue["2_30"].ToString() + " " + htValue["2_31"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_32"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#8

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#8";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#8";
				objHtc.Controls.Add(hdiv);
			
				if(Convert.ToInt32(htValue["2_83"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_33"].ToString() + " " + arrResults[lId][3].ToString()));
				}
				if(Convert.ToInt32(htValue["2_83"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;0"));
				}
				if(Convert.ToInt32(htValue["2_83"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_33"].ToString() + " " + arrResults[lId][4].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_8"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_8"].ToString().ToLower() + ".gif' alt='" + htValue["3_8"].ToString() + "' id='8' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#9

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#9";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#9";

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_34"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_9"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_9"].ToString().ToLower() + ".gif' alt='" + htValue["3_9"].ToString() + "' id='9' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#10

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#10";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#10";

				objHtc.Controls.Add(hdiv);

                objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_35"].ToString() + "/" + htValue["2_36"].ToString()));

				if(Convert.ToInt32(htValue["2_84"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" " + arrResults[lId][31].ToString()));
				}else if(Convert.ToInt32(htValue["2_84"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl(" " + arrResults[lId][32].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_10"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_10"].ToString().ToLower() + ".gif' alt='" + htValue["3_10"].ToString() + "' id='10' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#11

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#11";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#11";

				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_37"].ToString() + "/" + htValue["2_38"].ToString()));

				if(Convert.ToInt32(htValue["2_85"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" " + arrResults[lId][31].ToString()));
				}
				else if(Convert.ToInt32(htValue["2_85"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl(" " + arrResults[lId][32].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_11"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_11"].ToString().ToLower() + ".gif' alt='" + htValue["3_11"].ToString() + "' id='11' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#12

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#12";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#12";

				objHtc.Controls.Add(hdiv);
				
				if(Convert.ToInt32(htValue["2_97"]) == 1)
				{
					if(Convert.ToInt32(htValue["2_86"]) > 0)
					{
						objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrResults[lId][Convert.ToInt32(htValue["2_86"])+32].ToString() + " " + arrInfos[lId][11].ToString() + " " + arrInfos[lId][28].ToString() + " " + htValue["2_39"].ToString() + " " + arrInfos[lId][29].ToString() + " " + htValue["2_40"].ToString()));
					}
				}
				else if(Convert.ToInt32(htValue["2_98"]) == 1)
				{
					if(Convert.ToInt32(htValue["2_86"]) > 0)
					{
						objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrResults[lId][Convert.ToInt32(htValue["2_86"])+32].ToString() + " " + arrInfos[lId][12].ToString() + " " + arrInfos[lId][28].ToString() + " " + htValue["2_41"].ToString() + " " + arrInfos[lId][29].ToString() + " " + htValue["2_42"].ToString()));
					}
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#13B

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#13B";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#13B";
				objHtc.Controls.Add(hdiv);
				
				if(Convert.ToInt32(htValue["2_87"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_43"].ToString() + " " + arrResults[lId][3].ToString()));
				}
				if(Convert.ToInt32(htValue["2_87"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;0"));
				}
				if(Convert.ToInt32(htValue["2_87"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_43"].ToString() + " " + arrResults[lId][4].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_13B"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_13B"].ToString().ToLower() + ".gif' alt='" + htValue["3_13B"].ToString() + "' id='13B' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#14A

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#14A";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#14A";

				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_88"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_88"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}		

				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_44"].ToString() + " " + arrInfos[lId][23].ToString() + " -" + htValue["2_45"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_46"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_89"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_89"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}
			
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_47"].ToString() + " " + arrInfos[lId][23].ToString() + " -" + htValue["2_48"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_49"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_14Ah"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_14Ah"].ToString().ToLower() + ".gif' alt='" + htValue["3_14Ah"].ToString() + "' id='3_14Ah' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#15A

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#15A";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#15A";
				objHtc.Controls.Add(hdiv);

				if(Convert.ToInt32(htValue["2_90"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_50"].ToString() + " " + arrResults[lId][3].ToString()));
				}
				if(Convert.ToInt32(htValue["2_90"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;0"));
				}
				if(Convert.ToInt32(htValue["2_90"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_50"].ToString() + " " + arrResults[lId][4].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_15A"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_15A"].ToString().ToLower() + ".gif' alt='" + htValue["3_15A"].ToString() + "' id='3_15A' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#14B

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#14B";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#14B";

				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_91"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_91"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}

				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_51"].ToString() + " " + arrInfos[lId][23].ToString() + " -" + htValue["2_52"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_53"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + arrInfos[lId][22].ToString()));
				
				if(Convert.ToInt32(htValue["2_92"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" +"));
				}
				else if(Convert.ToInt32(htValue["2_92"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl(" -"));
				}
				
				objHtc.Controls.Add(new LiteralControl(" " + htValue["2_54"].ToString() + " " + arrInfos[lId][23].ToString() + " -" + htValue["2_55"].ToString() + "&deg; " + arrInfos[lId][0].ToString() + ": " + htValue["2_56"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#15B

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#15B";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#15B";

				objHtc.Controls.Add(hdiv);

				if(Convert.ToInt32(htValue["2_93"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_57"].ToString() + " " + arrResults[lId][3].ToString()));
				}
				if(Convert.ToInt32(htValue["2_93"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;0"));
				}
				if(Convert.ToInt32(htValue["2_93"]) == 3)
				{
					objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_57"].ToString() + " " + arrResults[lId][4].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#16A

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#16A";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#16A";

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_58"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_16A"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_16A"].ToString().ToLower() + ".gif' alt='" + htValue["3_16A"].ToString() + "' id='3_16A' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#16B

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#16B";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#16B";

				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_59"].ToString() + "/" + htValue["2_60"].ToString()));

				if(Convert.ToInt32(htValue["2_94"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" " + arrResults[lId][31].ToString()));
				}
				else if(Convert.ToInt32(htValue["2_94"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl(" " + arrResults[lId][32].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_16B"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_16B"].ToString().ToLower() + ".gif' alt='" + htValue["3_16B"].ToString() + "' id='3_16B' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#17A

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#17A";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#17A";

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_61"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_17A"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_17A"].ToString().ToLower() + ".gif' alt='" + htValue["3_17A"].ToString() + "' id='3_17A' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#17B

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#17B";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#17B";

				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + htValue["2_62"].ToString() + "/" + htValue["2_63"].ToString()));

				if(Convert.ToInt32(htValue["2_95"]) == 1)
				{
					objHtc.Controls.Add(new LiteralControl(" " + arrResults[lId][31].ToString()));
				}
				else if(Convert.ToInt32(htValue["2_95"]) == 2)
				{
					objHtc.Controls.Add(new LiteralControl(" " + arrResults[lId][32].ToString()));
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_17B"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_17B"].ToString().ToLower() + ".gif' alt='" + htValue["3_17B"].ToString() + "' id='3_17B' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#18

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#18";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#18";

				objHtc.Controls.Add(hdiv);

				if(Convert.ToInt32(htValue["2_99"]) == 1)
				{
					if(Convert.ToInt32(htValue["2_96"]) > 0)
					{
						objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrResults[lId][Convert.ToInt32(htValue["2_96"])+32].ToString() + " " + arrInfos[lId][11].ToString() + " " + arrInfos[lId][28].ToString() + " " + htValue["2_64"].ToString() + " " + arrInfos[lId][29].ToString() + " " + htValue["2_65"].ToString()));
					}
				}
				else if(Convert.ToInt32(htValue["2_100"]) == 1)
				{
					if(Convert.ToInt32(htValue["2_96"]) > 0)
					{
						objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrResults[lId][Convert.ToInt32(htValue["2_96"])+32].ToString() + " " + arrInfos[lId][12].ToString() + " " + arrInfos[lId][28].ToString() + " " + htValue["2_66"].ToString() + " " + arrInfos[lId][29].ToString() + " " + htValue["2_67"].ToString()));
					}
				}

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#19

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#19";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#19";

				objHtc.Controls.Add(hdiv);

				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][11].ToString() + " " + htValue["2_68"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][12].ToString() + " " + htValue["2_69"].ToString() + "<br/>"));
				objHtc.Controls.Add(new LiteralControl("&nbsp;" + arrInfos[lId][13].ToString() + " " + htValue["2_70"].ToString() + "<br/>"));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_19"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_19"].ToString().ToLower() + ".gif' alt='" + htValue["3_19"].ToString() + "' id='3_19' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#20

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#20";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#20";

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp; -" + htValue["2_71"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_20"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_20"].ToString().ToLower() + ".gif' alt='" + htValue["3_20"].ToString() + "' id='3_20' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);

				objHtr = new HtmlTableRow(); //#21

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","290px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","top");
				objHtc.Style.Add("padding-bottom","5px");

				hdiv = new HtmlGenericControl("div");
				hdiv.ID = "#21";
				hdiv.Attributes["class"] = "testcarddiv";
				hdiv.Style.Add("width","35px");
				hdiv.InnerHtml = "#21";

				objHtc.Controls.Add(hdiv);
				objHtc.Controls.Add(new LiteralControl("&nbsp; +" + htValue["2_72"].ToString()));

				objHtr.Controls.Add(objHtc);

				objHtc = new HtmlTableCell();
				objHtc.Style.Add("width","10px");
				objHtc.Style.Add("border","solid 1px #000000");
				objHtc.Style.Add("vertical-align","bottom");
				objHtc.Style.Add("background","#CCCCCC");

				if(htValue["3_21"].ToString() != "")
				{
					objHtc.InnerHtml = "<img src='../gfx/" + htValue["3_21"].ToString().ToLower() + ".gif' alt='" + htValue["3_21"].ToString() + "' id='3_21' />";
				}

				objHtr.Controls.Add(objHtc);

				objHt.Controls.Add(objHtr);
			}
			db.objDataReader.Close();
			db.Dispose();
			db = null;
			return objHt;
		}

		public HtmlGenericControl getInfRow(int intUserId,bool isfirst)
		{
			Hashtable htValue = new Hashtable();

			Database db = new Database();
			string strSql = "SELECT ";
			
			for(int i = 1;i < 101;i++)
			{
				strSql += "2_" + i + ",";
			}

			strSql += "3_3,3_4h,3_5h,3_8,3_9,3_10,3_11,3_13A,3_13B,3_14Ah,3_15A,3_16A,3_16B,3_17A,3_17B,3_19,3_20,3_21 FROM a_21 WHERE clientid = " + intUserId;

			if(isfirst)
			{
				strSql += " AND isfirst = 1;";
			}
			else
			{
				strSql += " AND isfirst = 0;";
			}

			MySqlDataReader objDr = db.select(strSql);
			
			bool blnFound99 = false;

			if(objDr.Read())
			{
				for(int i = 1;i < 101;i++)
				{
					if(Convert.ToString(objDr["2_"+i]) == "-1")
					{
						blnFound99 = true;
					}
					htValue.Add(i.ToString(),objDr["2_" + i].ToString());
				}

				htValue.Add("3_3",objDr["3_3"].ToString());
				htValue.Add("3_4h",objDr["3_4h"].ToString());
				htValue.Add("3_5h",objDr["3_5h"].ToString());
				htValue.Add("3_8",objDr["3_8"].ToString());
				htValue.Add("3_9",objDr["3_9"].ToString());
				htValue.Add("3_10",objDr["3_10"].ToString());
				htValue.Add("3_11",objDr["3_11"].ToString());
				htValue.Add("3_13A",objDr["3_13A"].ToString());
				htValue.Add("3_14Ah",objDr["3_14Ah"].ToString());
				htValue.Add("3_15A",objDr["3_15A"].ToString());
				htValue.Add("3_16A",objDr["3_16A"].ToString());
				htValue.Add("3_16B",objDr["3_16B"].ToString());
				htValue.Add("3_17A",objDr["3_17A"].ToString());
				htValue.Add("3_17B",objDr["3_17B"].ToString());
				htValue.Add("3_19",objDr["3_19"].ToString());
				htValue.Add("3_20",objDr["3_20"].ToString());
				htValue.Add("3_21",objDr["3_21"].ToString());
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();

			HtmlGenericControl infDiv = new HtmlGenericControl("div");
			infDiv.ID = "infDiv";
			infDiv.Attributes["class"] = "infrowdiv";
			
			if(blnFound99 == false)
			{
				//#7
				//Hvis denne er plus = over stregen
				//Hvis denne er plan (som er det samme som nul) = på stregen
				//Hvis denne er minus = under stregen

				HtmlImage img = new HtmlImage();
				img.ID = "#7";
				img.Alt = "#7";

				if(Convert.ToDouble(htValue["17"]) > 0)
				{
					img.Src = "../gfx/infrow/7o.gif";
				}
				if(Convert.ToDouble(htValue["17"]) == 0)
				{
					img.Src = "../gfx/infrow/7p.gif";
				}
				if(Convert.ToDouble(htValue["17"]) < 0)
				{
					img.Src = "../gfx/infrow/7u.gif";
				}

				infDiv.Controls.Add(img);

				//Parantes start

				img = new HtmlImage();
				img.ID = "_(_";
				img.Alt = "_(_";
				img.Src = "../gfx/infrow/lb.gif";

				infDiv.Controls.Add(img);

				//#5
				//Høj = over stregen
				//Neutral = på stregen
				//Lav = under stregen

				img = new HtmlImage();
				img.ID = "#5";
				img.Alt = "#5";

				if(htValue["3_5h"].ToString() == "H")
				{
					img.Src = "../gfx/infrow/5o.gif";
					infDiv.Controls.Add(img);
				}
				if(htValue["3_5h"].ToString() == "N")
				{
					img.Src = "../gfx/infrow/5p.gif";
					infDiv.Controls.Add(img);
				}
				if(htValue["3_5h"].ToString() == "L")
				{
					img.Src = "../gfx/infrow/5u.gif";
					infDiv.Controls.Add(img);
				}
		
				
				//#9
				//Høj = over stregen
				//Neutral = på stregen
				//Lav = under stregen

				img = new HtmlImage();
				img.ID = "#9";
				img.Alt = "#9";

				if(htValue["3_9"].ToString() == "H")
				{
					img.Src = "../gfx/infrow/9o.gif";
					infDiv.Controls.Add(img);
				}
				if(htValue["3_9"].ToString() == "N")
				{
					img.Src = "../gfx/infrow/9p.gif";
					infDiv.Controls.Add(img);
				}
				if(htValue["3_9"].ToString() == "L")
				{
					img.Src = "../gfx/infrow/9u.gif";
					infDiv.Controls.Add(img);
				}

				//#10 & #11
				//Hvilken er lowlow?

				img = new HtmlImage();

				if(htValue["3_10"].ToString() == "LL")
				{
					img.ID = "#10";
					img.Alt = "#10";
					img.Src = "../gfx/infrow/10u.gif";
				}
				else
				{
					img.ID = "#11";
					img.Alt = "#11";
					img.Src = "../gfx/infrow/11u.gif";
				}
				
				infDiv.Controls.Add(img);

				//#16B & #17B
				//Hvilken er lowlow?

				img = new HtmlImage();

				if(htValue["3_16B"].ToString() == "LL")
				{
					img.ID = "#16B";
					img.Alt = "#16B";
					img.Src = "../gfx/infrow/16bu.gif";
				}
				else
				{
					img.ID = "#17B";
					img.Alt = "#17B";
					img.Src = "../gfx/infrow/17bu.gif";
				}
				
				infDiv.Controls.Add(img);

				//Parantes slut

				img = new HtmlImage();
				img.ID = "_)_";
				img.Alt = "_)_";
				img.Src = "../gfx/infrow/rb.gif";

				infDiv.Controls.Add(img);

				//#14A & #15A
				//#14A = H & 15A = L == 14A over stregen & 15A under stregen
				//#15A = H & 14A = L == 15A over stregen & 14A under stregen
				//#14A = 15A == begge med samme placering

				img = new HtmlImage();

				if(htValue["3_14Ah"].ToString() == "H" && htValue["3_15A"].ToString() == "L")
				{
					img.ID = "#14#15";
					img.Alt = "#14#15";
					img.Src = "../gfx/infrow/1415.gif";
					
					infDiv.Controls.Add(img);
				}
				else if(htValue["3_15A"].ToString() == "H" && htValue["3_14Ah"].ToString() == "L")
				{
					img.ID = "#15#14";
					img.Alt = "#15#14";
					img.Src = "../gfx/infrow/1514.gif";
					
					infDiv.Controls.Add(img);
				}
				else if(htValue["3_15A"].ToString() == "H" && htValue["3_14Ah"].ToString() == "H")
				{
					img.ID = "#14#15o";
					img.Alt = "#14#14o";
					img.Src = "../gfx/infrow/1415o.gif";
					
					infDiv.Controls.Add(img);
				}
				else if(htValue["3_15A"].ToString() == "L" && htValue["3_14Ah"].ToString() == "L")
				{
					img.ID = "#14#15u";
					img.Alt = "#14#14u";
					img.Src = "../gfx/infrow/1415u.gif";
					
					infDiv.Controls.Add(img);
				}

				//Fhast Parantes start

				img = new HtmlImage();
				img.ID = "_[_";
				img.Alt = "_[_";
				img.Src = "../gfx/infrow/lsb.gif";

				infDiv.Controls.Add(img);


				//#16A & #17A
				//#16A = H & 17A = L == 16A over stregen & 17A under stregen
				//#17A = H & 16A = L == 17A over stregen & 16A under stregen
				//#16A = 17A == begge med samme placering

				img = new HtmlImage();

				if(htValue["3_16A"].ToString() == "H" && htValue["3_17A"].ToString() == "L")
				{
					img.ID = "#16#17";
					img.Alt = "#16#17";
					img.Src = "../gfx/infrow/1617.gif";
					
					infDiv.Controls.Add(img);
				}
				else if(htValue["3_17A"].ToString() == "H" && htValue["3_16A"].ToString() == "L")
				{
					img.ID = "#17#16";
					img.Alt = "#17#16";
					img.Src = "../gfx/infrow/1716.gif";
					
					infDiv.Controls.Add(img);
				}
				else if(htValue["3_17A"].ToString() == "H" && htValue["3_16A"].ToString() == "H")
				{
					img.ID = "#16#17o";
					img.Alt = "#16#16o";
					img.Src = "../gfx/infrow/1617o.gif";
					
					infDiv.Controls.Add(img);
				}
				else if(htValue["3_17A"].ToString() == "L" && htValue["3_16A"].ToString() == "L")
				{
					img.ID = "#16#17u";
					img.Alt = "#16#16u";
					img.Src = "../gfx/infrow/1617u.gif";
					
					infDiv.Controls.Add(img);
				}
				else if(htValue["3_17A"].ToString() == "N" && htValue["3_16A"].ToString() == "N")
				{
					img.ID = "#16#17u";
					img.Alt = "#16#16u";
					img.Src = "../gfx/infrow/1617p.gif";
					
					infDiv.Controls.Add(img);
				}


				//#20 & #21
				//#20 > #21 = 20 over stregen og 21 under
				//#20 < #21 = 21 over stregen og 20 under
				//#20 = #21 == ved siden af hinanden over eller under bestemt af H/L
				//Hvis den ene er H og den anden L placeres begge på stregen

				img = new HtmlImage();
				
				if(Convert.ToDouble(htValue["71"]) > Convert.ToDouble(htValue["72"]))
				{
					img.ID = "#20#21";
					img.Alt = "#20#21";
					img.Src = "../gfx/infrow/2021.gif";
					
					infDiv.Controls.Add(img);
				}

				if(Convert.ToDouble(htValue["71"]) < Convert.ToDouble(htValue["72"]))
				{
					img.ID = "#21#20";
					img.Alt = "#21#20";
					img.Src = "../gfx/infrow/2120.gif";
					
					infDiv.Controls.Add(img);
				}

				if(Convert.ToDouble(htValue["71"]) == Convert.ToDouble(htValue["72"]))
				{
					if(htValue["3_20"].ToString() == "H" && htValue["3_21"].ToString() == "H")
					{
						img.ID = "#20#21o";
						img.Alt = "#20#21o";
						img.Src = "../gfx/infrow/2021o.gif";
					}
					else if(htValue["3_20"].ToString() == "L" && htValue["3_21"].ToString() == "L")
					{
						img.ID = "#20#21u";
						img.Alt = "#20#21u";
						img.Src = "../gfx/infrow/2021u.gif";
					}
					else
					{
						img.ID = "#20#21p";
						img.Alt = "#20#21p";
						img.Src = "../gfx/infrow/2021p.gif";
					}
					
					infDiv.Controls.Add(img);
				}

				//Fhast Parantes slut

				img = new HtmlImage();
				img.ID = "_]_";
				img.Alt = "_]_";
				img.Src = "../gfx/infrow/rsb.gif";

				infDiv.Controls.Add(img);

				//#19
				//Placeres efter H,N,L

				img = new HtmlImage();
				img.ID = "#19";
				img.Alt = "#19";

				if(htValue["3_19"].ToString() == "H")
				{
					img.Src = "../gfx/infrow/19o.gif";
				}
				if(htValue["3_19"].ToString() == "N")
				{
					img.Src = "../gfx/infrow/19p.gif";
				}
				else
				{
					img.Src = "../gfx/infrow/19u.gif";
				}

				infDiv.Controls.Add(img);
			}
			else{
				string[] arrInfos = new string[5] {"","Ikke alle målinger er indtastes hvorfor informativ række ikke kan beregnes","!Ikke alle målinger er indtastes hvorfor informativ række ikke kan beregnes","/Ikke alle målinger er indtastes hvorfor informativ række ikke kan beregnes","Not all measurements entered, informative row will not be calculated."};
				infDiv.InnerHtml = arrInfos[((User)Session["user"]).IntLanguageId].ToString();
			}
			return infDiv;
		}

		private string getTypeSyndrom(int intUserId,bool isfirst)
		{
			string strType = "";
			string strAdSteps = "<ul>";
			int lId = ((Optician)Session["user"]).IntLanguageId;

			Database db = new Database();
			string strSql = "SELECT birthdate FROM user_client WHERE userid = " + intUserId + ";";

			MySqlDataReader objDr_1 = db.select(strSql);

			int intAge = 0;

			if (objDr_1.Read() == true) {

				if((Optician)Session["user"] != null)
					intAge = 106;//((Optician)Session["user"]).getAge(Convert.ToDateTime(objDr_1["birthdate"]));
				else
					intAge = 0;
			}

			string[][] arrTypeA = new string[5][];
			string[][] arrTypeB1 = new string[5][];
			string[][] arrTypeB2 = new string[5][];
			string[][] arrTypeC = new string[5][];
			string[][] arrAdSteps = new string[5][];

			//Typer
			arrTypeA[1] = new string[]{"Klientens synsproblemer kan være af patalogisk art. Såfremt patalogiske årsager kan udelukkes korrigeres som hvis klienten havde været en B2 type: Korrigeres med max plus på afstand og reduceret plus på nær."};
			arrTypeA[2] = new string[]{"Klientens synsproblemer kan være af patalogisk art. Såfremt patalogiske årsager kan udelukkes korrigeres som hvis klienten havde været en B2 type: Korrigeres med max plus på afstand og reduceret plus på nær."};
			arrTypeA[3] = new string[]{"The patient’s vision problems may be of pathological nature. If pathological causes can be excluded, correction is performed as had the patient been a type B2: Corrected with maximum plus at distance and reduced plus at near range."};
			//Tysk
			arrTypeA[4] = new string[]{"Das Sehvermögen des Patienten könnte pathologisch verändert sein. Wenn alle pathologischen Ursachen ausgeschlossen werden können, kann dem Patienten gemäß einem Typ B2 eine Korrektur angepasst werden. Maximale Plus-Korrektur in der Ferne und reduzierte Plus-Korrektur in der Nähe."};

			arrTypeB1[1] = new string[]{"Klientens synsproblemer skyldes at akkomodationen er svagere end konvergensevnen på både afstand og nær.","Korriger med max plus på alle afstande.","Korriger med reduceret plus på afstand og reduceret plus til nær."};
			arrTypeB1[2] = new string[]{"Klientens synsproblemer skyldes at akkomodationen er svagere end konvergensevnen på både afstand og nær.","Korriger med max plus på alle afstande.","Korriger med reduceret plus på afstand og reduceret plus til nær."};
			arrTypeB1[3] = new string[]{"The patient’s vision problems may be caused by the accommodation being weaker than the convergence ability both at distance and at near range.", "Correct with maximum plus at all distances", "Correct with reduced plus at distance and reduced plus at near range."};
			//Tysk
			arrTypeB1[4] = new string[]{"Das Problem des Patienten könnte dadurch bedingt sein, dass die Akkommodation schwächer als die Konvergenzfähigkeit sowohl in der Nähe als auch in der Ferne ist.", " Maximale Plus-Korrektur in allen Entfernungen ", "reduzierte Plus-Korrektur in der Ferne und in der Nähe."};

			arrTypeB2[1] = new string[]{"Klientens synsproblemer skyldes specielt akkomodationsproblemer på nær.","Korriger med max plus på nær, mens plus skal reduceres på afstand.","Korriger med reduceret plus på alle afstande."};
			arrTypeB2[2] = new string[]{"Klientens synsproblemer skyldes specielt akkomodationsproblemer på nær.","Korriger med max plus på nær, mens plus skal reduceres på afstand.","Korriger med reduceret plus på alle afstande."};
			arrTypeB2[3] = new string[]{"The patient’s vision problems may be caused chiefly by accommodation issues at near range.", "Correct with maximum plus at near range, whereas plus has to be reduced at distance.", "Correct with reduced plus on all distances."};
			//tysk
			arrTypeB2[4] = new string[]{"Das Problem des Patienten könnte vor allem durch Schwäche der Akkommodation in der Nähe bedingt sein.", " Maximale Plus-Korrektur in der Nähe, während die Plus-Korrektur in der Ferne reduziert werden sollte.", "reduzierte Plus-Korrektur in allen Entfernungen."};

			arrTypeC[1] = new string[]{"Klientens synsproblemer skyldes konvergensproblemer. Korriger aldrig med fuldt plus, men beskær plus på alle afstand. Dette skal kombineres med intensiv synstræning."};
			arrTypeC[2] = new string[]{"Klientens synsproblemer skyldes konvergensproblemer. Korriger aldrig med fuldt plus, men beskær plus på alle afstand. Dette skal kombineres med intensiv synstræning."};
			arrTypeC[3] = new string[]{"The patient’s vision problems may be caused by convergence problems. Never correct with full plus, but trim plus at all distances. This should be combined with intensive vision training."};
			//Tysk
			arrTypeC[4] = new string[]{"Das Problem des Patienten könnte vor allem durch Schwäche der Konvergenz bedingt sein. Nie mit voller Plus-Korrektur ausgleichen, immer nur angepasst in allen Entfernungen. Zusätzlich empfiehlt sich eine Kombination mit intensivem Visualtraining."};

			//Tilpasningsstadier
			arrAdSteps[1] = new string[]{"<li>Tilpasningsstadie 1: Der er en ukompliceret tilstand.</li>","<li>Tilpasningsstadie 2: der er en forholdsvis ukompliceret tilstand, der kan løses med (plus-)linser alene.</li>","<li>Tilpasningsstadie 3: Hvis der fortsat er plus tilbage i Nettoresultaterne, kan pluslinser alene løse problemet.</li>","<li>Tilpasningsstadie 4. Såfremt problemet ikke er alt for gammelt, er (plus-) linser evt. nok, gerne suppleret med synstræning.</li>","<li>Tilpasningsstadie 6. Synstræning bør indgå i den anbefalede behandling.</li>","<li>Tilpasningsstadie 7: Der bør tilbydes intensiv synstræning som senere kan suppleres med pluslinser af passende styrke, evt. bifocale briller.</li>"};
			arrAdSteps[2] = new string[]{"<li>Tilpasningsstadie 1: Der er en ukompliceret tilstand.</li>","<li>Tilpasningsstadie 2: der er en forholdsvis ukompliceret tilstand, der kan løses med (plus-)linser alene.</li>","<li>Tilpasningsstadie 3: Hvis der fortsat er plus tilbage i Nettoresultaterne, kan pluslinser alene løse problemet.</li>","<li>Tilpasningsstadie 4. Såfremt problemet ikke er alt for gammelt, er (plus-) linser evt. nok, gerne suppleret med synstræning.</li>","<li>Tilpasningsstadie 6. Synstræning bør indgå i den anbefalede behandling.</li>","<li>Tilpasningsstadie 7: Der bør tilbydes intensiv synstræning som senere kan suppleres med pluslinser af passende styrke, evt. bifocale briller.</li>"};
			arrAdSteps[3] = new string[]{"<li>Adaptation phase 1: Which is a non-complicated condition.</li>","<li> Adaptation phase 2: Which is a relatively non-complicated condition, which can be solved solely by the use of (plus)lenses.</li>","<li> Adaptation phase 3: If the net results continue to show plus, plus lenses will be sufficient to solve the problem.</li>","<li> Adaptation phase 4. If the problems is not too ingrained, (plus) lenses may be adequate treatment, possibly supplemented by vision training.</li>","<li> Adaptation phase 6. Vision training should be integrated in the recommended treatment.</li>","<li> Adaptation phase 7: Intensive vision training should be offered, which may later be supplemented by plus lenses of suitable strength, possibly bifocal spectacles.</li>"};
			//Tysk
			arrAdSteps[4] = new string[]{"<li>Adaptationsphase 1: Unkomplizierter Zustand .</li>","<li> Adaptationsphase 2: Relativ unkompliziertes Problem, das nur durch Plus-Linsen gelöst werden kann.  .</li>","<li> Adaptationsphase 3: Wenn die Netz-Ergebnisse weiter Plus zeigen, dann reichen auch Plus-Linsen aus um das Problem zu lösen..</li>","<li> Adaptationsphase 4: Wenn das Problem nicht zu überlagert ist, sind Plus-Linsen die beste Lösung, möglicherweise unterstützt durch Visualtraining.  .</li>","<li> Adaptationsphase 6: Visualtraining sollte empfohlen werden..</li>","<li> Adaptationsphase 7: Intensives Visualtraining  sollte empfohlen werden, später kann es auch durch Linsen oder Brillen der passenden Stärke, eventuell bifokal, unterstützt werden.</li>"};

         db.dbDispose();
         objDr_1.Close();

			Hashtable htValue = new Hashtable();

			db = new Database();
			strSql = "SELECT ";
			
			for(int i = 1;i < 101;i++)
			{
				strSql += "2_" + i + ",";
			}

			strSql += "3_3,3_4h,3_5h,3_8,3_9,3_10,3_11,3_13A,3_13B,3_14Ah,3_15A,3_16A,3_16B,3_17A,3_17B,3_19,3_20,3_21 FROM a_21 WHERE clientid = " + intUserId;

			if(isfirst)
			{
				strSql += " AND isfirst = 1;";
			}
			else
			{
				strSql += " AND isfirst = 0;";
			}

			MySqlDataReader objDr = db.select(strSql);

			bool blnFound99 = false;

			if(objDr.Read())
			{
				for(int i = 1;i < 101;i++)
				{
					if(Convert.ToString(objDr["2_"+i]) == "-1")
					{
						blnFound99 = true;
					}
					htValue.Add(i.ToString(),objDr["2_" + i].ToString());
				}

				htValue.Add("3_3",objDr["3_3"].ToString());
				htValue.Add("3_4h",objDr["3_4h"].ToString());
				htValue.Add("3_5h",objDr["3_5h"].ToString());
				htValue.Add("3_8",objDr["3_8"].ToString());
				htValue.Add("3_9",objDr["3_9"].ToString());
				htValue.Add("3_10",objDr["3_10"].ToString());
				htValue.Add("3_11",objDr["3_11"].ToString());
				htValue.Add("3_13A",objDr["3_13A"].ToString());
				htValue.Add("3_14Ah",objDr["3_14Ah"].ToString());
				htValue.Add("3_15A",objDr["3_15A"].ToString());
				htValue.Add("3_16A",objDr["3_16A"].ToString());
				htValue.Add("3_16B",objDr["3_16B"].ToString());
				htValue.Add("3_17A",objDr["3_17A"].ToString());
				htValue.Add("3_17B",objDr["3_17B"].ToString());
				htValue.Add("3_19",objDr["3_19"].ToString());
				htValue.Add("3_20",objDr["3_20"].ToString());
				htValue.Add("3_21",objDr["3_21"].ToString());
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();

			//Tillægssjov

			Wysiwyg wys = new Wysiwyg();

			if(htValue["3_5h"].ToString() == "L")
			{
				strAdSteps += arrAdSteps[lId][5].ToString();
			}
			if(htValue["3_10"].ToString() == "LL")
			{
				strAdSteps += arrAdSteps[lId][4].ToString();
			}
			if(intAge > 30)
			{
                string var61 = htValue["61"].ToString().ToLower();
                string var58 = htValue["58"].ToString().ToLower();
                string var71 = htValue["71"].ToString().ToLower();
                string var72 = htValue["72"].ToString().ToLower();
                if (var61 != "x" && var58 != "x" && var71 != "x" && var72 != "x")
                {
                    if (wys.GetNumeric(Convert.ToDouble(htValue["61"])) > wys.GetNumeric(Convert.ToDouble(htValue["58"])) && wys.GetNumeric(Convert.ToDouble(htValue["71"])) > wys.GetNumeric(Convert.ToDouble(htValue["72"])))
                    {
                        strAdSteps += arrAdSteps[lId][3].ToString();
                    }
                }
			}
			if(htValue["3_15A"].ToString() == "H" && htValue["3_14Ah"].ToString() == "L")
			{
				strAdSteps += arrAdSteps[lId][2].ToString();
			}
			if((htValue["3_15A"].ToString() == "H" && htValue["3_14Ah"].ToString() == "H") || (htValue["3_15A"].ToString() == "L" && htValue["3_14Ah"].ToString() == "L") || (htValue["3_15A"].ToString() == "N" && htValue["3_14Ah"].ToString() == "N"))
			{
				strAdSteps += arrAdSteps[lId][1].ToString();
			}
			if(htValue["3_15A"].ToString() == "L" && htValue["3_14Ah"].ToString() == "H")
			{
				strAdSteps += arrAdSteps[lId][0].ToString();
			}
			
			strAdSteps += "</ul>";
		
			//Typen bestemmes her
			//Hvis #4, #11, #13B og #17B alle er lave og/eller lowlow = A-type
			//Hvis  # 16B er lowlow = B1 type
			//Hvis  # 17B er lowlow = B2 type
			//Hvis #5, # 10 og #16 B er lowlow samtidig med at # 9 er høj eller neutral
			//kombineret med at #8 er en exofori og der i #15A er en større exofori end i #8
			//kombineret med at #17A numerisk er større end #16A og #20 numerisk er større end #21
			//kombineret med at #19 er høj og at tallet under Begge her er større end tallet under hhv H og V = C-type

			try {
				if((htValue["3_4h"].ToString() == "L" || htValue["3_4h"].ToString() == "LL") && (htValue["3_11"].ToString() == "L" || htValue["3_11"].ToString() == "LL") && (htValue["3_13B"].ToString() == "L" || htValue["3_13B"].ToString() == "LL") && (htValue["3_17B"].ToString() == "L" || htValue["3_17B"].ToString() == "LL"))
				{
					strType = arrTypeA[lId][0].ToString();
	
					strType += strAdSteps;
					
	
				}
			}
			catch {
				
			}
			if(htValue["3_16B"].ToString() == "LL")
			{
				strType = arrTypeB1[lId][0].ToString();

				if(htValue["3_11"].ToString() == "LL" && htValue["3_16B"].ToString() == "LL")
				{
					strType += arrTypeB1[lId][1].ToString();
				}

				if(htValue["3_10"].ToString() == "LL" && htValue["3_16B"].ToString() == "LL")
				{
					strType += arrTypeB1[lId][2].ToString();
				}

				strType += strAdSteps;

			}
			if(htValue["3_17B"].ToString() == "LL")
			{
				strType = arrTypeB2[lId][0].ToString();

				if(htValue["3_11"].ToString() == "LL" && htValue["3_17B"].ToString() == "LL")
				{
					strType += arrTypeB2[lId][1].ToString();
				}

				if(htValue["3_10"].ToString() == "LL" && htValue["3_17B"].ToString() == "LL")
				{
					strType += arrTypeB2[lId][2].ToString();
				}

				strType += strAdSteps;				
			}

			if(htValue["3_5h"].ToString() == "LL" && htValue["3_10"].ToString() == "LL" && htValue["3_16B"].ToString() == "LL" && (htValue["3_9"].ToString() == "H" || htValue["3_9"].ToString() == "N"))
			{
				if(Convert.ToInt32(htValue["82"]) == 3 && Convert.ToInt32(htValue["89"]) == 3 && Convert.ToDouble(htValue["50"]) > Convert.ToDouble(htValue["33"]))
				{
					if(wys.GetNumeric(Convert.ToDouble(htValue["61"])) > wys.GetNumeric(Convert.ToDouble(htValue["58"])) && wys.GetNumeric(Convert.ToDouble(htValue["71"])) > wys.GetNumeric(Convert.ToDouble(htValue["72"])))
					{
						if(htValue["3_19"].ToString() == "H" && Convert.ToDouble(htValue["70"]) > Convert.ToDouble(htValue["69"]) && Convert.ToDouble(htValue["70"]) > Convert.ToDouble(htValue["68"]))
						{
							strType = arrTypeC[lId][0].ToString();
						}
					}
				}
			}

			wys = null;
			if(blnFound99){
				string[] arrInfos = new string[5] {"","Ikke alle målinger er indtastes hvorfor typesyndrom ikke kan beregnes","!Ikke alle målinger er indtastes hvorfor typesyndrom ikke kan beregnes","/Ikke alle målinger er indtastes hvorfor typesyndrom ikke kan beregnes","/Ikke alle målinger er indtastes hvorfor typesyndrom ikke kan beregnes"};
				strType = arrInfos[IntLanguageId].ToString();
			}
			return strType;
		}

		private string From99To0(string strValue)
		{
			if(strValue == "99.0" || strValue == "99"){
				return "";
			}else{
				return strValue;
			}
		}

	}
}
