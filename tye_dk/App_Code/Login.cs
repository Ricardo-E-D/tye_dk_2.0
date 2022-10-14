using System;
using System.Data.Odbc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using tye.exceptions;
using MySql.Data.MySqlClient;

namespace tye
{
	public class Login : System.Web.UI.Page
	{
		private string strIp;
		private string strPassword;
		private DateTime datLoginTime;
		private bool blnSucces;
		private int intUserType;

		public Login()
		{

		}

		public string StrIp {
			get { return strIp; }
			set { strIp = value; }
		}

		public string StrPassword {
			get { return strPassword; }
			set { strPassword = value; }
		}

		public DateTime DatLoginTime {
			get { return datLoginTime; }
			set { datLoginTime = value; }
		}
		
		public bool BlnSucces {
			get { return blnSucces; }
			set { blnSucces = value; }
		}
		
		public int IntUserType {
			get { return intUserType; }
			set { intUserType = value; }
		}

		public Exception ValidateLogin() {
			Exception exp = null;
			Database db = new Database();

			string strSql = "SELECT id,password,usertypeid,isActive" +
							" FROM users" +
							" WHERE password = BINARY '" + ((Login)Session["login"]).StrPassword + "';";

			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read() == false) { // Wrong password, not known to the system.
				db.objDataReader.Close();
                db.dbDispose();
				db = null;
                
				exp = new WrongPassword();
				throw exp;
			} else {	
				int isActive = 0;
				int id = 0;
						
				isActive = Convert.ToInt32(objDr["isActive"]);
				id = Convert.ToInt32(objDr["id"]);

				if(isActive == 0) {
					Database db2 = new Database();
					string strSql2 = "SELECT languageid FROM user_client WHERE userid = " + id;
					MySqlDataReader objDr2 = db2.select(strSql2);
					int lang_id = 0;

					if (objDr2.Read()) {
						lang_id = Convert.ToInt32(objDr2["languageid"]);
					}
					db2.objDataReader.Close();
					db2.dbDispose();
					db2 = null;
					db.objDataReader.Close();
					db.Dispose();
					db = null;
					exp = new LicenceExpired(lang_id);
					throw exp;

				} else { // Correct password and licence not expired
					
					((Login)Session["login"]).BlnSucces = true;
					((Login)Session["login"]).IntUserType = Convert.ToInt32(objDr["usertypeid"]);
				
					switch (Convert.ToInt32(objDr["usertypeid"])) {
						case 2: // Optician
							Optician optician = new Optician();
							optician.CreateObj(((Login)Session["login"]).StrPassword);
							break;
					
						case 3: // Users/client
							Database db1 = new Database();
							string strSql1 = "SELECT enddate FROM user_client WHERE userid = " + Convert.ToInt32(objDr["id"]);
							DateTime datEnddate = Convert.ToDateTime(db1.scalar(strSql1));
                            
							db1.dbDispose();
							db1 = null;

							if(Convert.ToDateTime(datEnddate.ToString("dd-MM-yyyy")) <= Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MM-yyyy"))) {
								db1 = new Database();
								strSql1 = "UPDATE users SET isactive = 0 WHERE id = " + Convert.ToInt32(objDr["id"]);
								db1.execSql(strSql1);
								db1.dbDispose();
								db1 = null;

								Database db2 = new Database();
								string strSql2 = "SELECT languageid FROM user_client WHERE userid = " + Convert.ToInt32(objDr["id"]);
								MySqlDataReader objDr2 = db2.select(strSql2);
								int lang_id = 0;

								if (objDr2.Read())
									lang_id = Convert.ToInt32(objDr2["languageid"]);
   							
								db2.objDataReader.Close();
								db2.dbDispose();
								db2 = null;

								exp = new LicenceExpired(lang_id);
								
								throw exp;
							}

							Client client = new Client();
							client.CreateObj(((Login)Session["login"]).StrPassword);
							break;

						case 4: // Maria
							Admin admin = new Admin();
							admin.CreateObj(((Login)Session["login"]).StrPassword);
							break;

						case 5: // distributor
							Admin dist = new Admin();
							dist.CreateObj(((Login)Session["login"]).StrPassword);
							break;

						default:
							db.objDataReader.Close();
							db.dbDispose();
							db = null;

							Response.Redirect(".");
							break;
					} // switch

					db.objDataReader.Close();
					db.dbDispose();
					db = null;				
				} // if
			} // if

			return exp;
		} // method
	}
}
