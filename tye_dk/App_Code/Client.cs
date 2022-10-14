using System;
using MySql.Data.MySqlClient;
using System.Web.SessionState;

namespace tye
{
	public class Client : User
	{
		private string strName;
		private int intOpticianId;
		private DateTime datEnddate;

		public Client()
		{

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
		
		public int IntOpticianId
		{
			get
			{
				return intOpticianId;
			}
			set
			{
				intOpticianId = value;
			}
		}

		public DateTime DatEnddate
		{
			get
			{
				return datEnddate;
			}
			set
			{
				datEnddate = value;
			}
		}

		new public void CreateObj(string strPassword)
		{
			string strSql = "SELECT users.id,address,zipcode,city,email,phone,firstname,lastname,usertypeid,usertypes.name as usertype_name,languageid,opticianid FROM users";
			strSql += " INNER JOIN user_client ON users.id = user_client.userid INNER JOIN usertypes ON users.usertypeid = usertypes.id WHERE password = '" + strPassword + "' AND users.isactive = 1";
			
			Database db = new Database();
							
			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read() == true)
			{
				Client user = new Client();
	
				user.IntUserId = Convert.ToInt32(objDr["id"]);
				user.IntLanguageId = Convert.ToInt32(objDr["languageid"]);
				user.IntOpticianId = Convert.ToInt32(objDr["opticianid"]);
				user.IntUserTypeId = Convert.ToInt32(objDr["usertypeid"]);
				user.StrZipCode = objDr["zipcode"].ToString();
				user.StrAddress = objDr["address"].ToString();
				user.StrCity = objDr["city"].ToString();
				user.StrEmail = objDr["email"].ToString();
				user.StrName = objDr["firstname"].ToString() + " " + objDr["lastname"].ToString();
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

	}
}
