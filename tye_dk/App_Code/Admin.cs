using System;
using MySql.Data.MySqlClient;
using System.Web.SessionState;

namespace tye
{
	public class Admin : User
	{
		private string strName;
		private Random r;
		private string strPassword = "";
		private int intNextChar;
		private string[] arrChars;
		protected bool blnFound = false;

		public Admin()
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
		
		new public void CreateObj(string strPassword)
		{
			string strSql = "SELECT users.id,address,zipcode,city,email,phone,usertypes.name AS usertype_name," +
				" usertypeid,firstname,lastname,languageid" +
				" FROM users" + 
				" INNER JOIN user_admin ON user_admin.userid = users.id" +
				" INNER JOIN usertypes ON usertypeid = usertypes.id" +
				" WHERE password = '" + strPassword + "' AND isactive = 1;";

			Database db = new Database();
							
			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read() == true)
			{
				Admin user = new Admin();
	
				user.IntUserId = Convert.ToInt32(objDr["id"]);
				user.IntLanguageId = Convert.ToInt32(objDr["languageid"]);
				user.IntUserTypeId = Convert.ToInt32(objDr["usertypeid"]);
				user.StrZipCode = objDr["zipcode"].ToString();
				user.StrAddress = objDr["address"].ToString();
				user.StrCity = objDr["city"].ToString();
				user.StrEmail = objDr["email"].ToString();
				user.StrName = objDr["firstname"].ToString() + " " + objDr["lastname"].ToString();
				user.StrPhone = objDr["phone"].ToString();
				user.IsDistributor = (objDr["usertype_name"].ToString() == "distributor");
    
				((Menu)Session["menu"]).IntLanguageId = Convert.ToInt32(user.IntLanguageId);
				((Menu)Session["menu"]).StrAccess = "access_" + objDr["usertype_name"].ToString();

				Session["user"] = user;

				user = null;

			}

			db.objDataReader.Close();
            db.dbDispose();
			db = null;
		}

		public string generatePassword()
		{
			arrChars = new string[54] {"a","b","c","d","e","f","g","h","i","j","k","m","n","p","q","r","s","t","u","v","x","y","z","A","B","C","D","E","F","G","H","J","K","L","M","N","P","Q","R","S","T","U","V","X","Y","Z","2","3","4","5","6","7","8","9"};

			r = new Random();

			blnFound = false;
			strPassword = "";
		
			for(int i = 0;i < 6;i++)
			{
				intNextChar = (int)(54.00 * r.NextDouble());

				strPassword += arrChars[intNextChar];
			}

			r = null;

			Database db = new Database();
			string strSql = "SELECT COUNT(*) AS FOUND FROM users WHERE password = '" + strPassword + "';";
			MySqlDataReader objDr = db.select(strSql);

			if (objDr.Read())
			{
				if(Convert.ToInt32(objDr["found"]) != 0)
				{
					blnFound = true;
				}
			}

			db.objDataReader.Close();
            db.dbDispose();
			db = null;

			db = new Database();
			strSql = "SELECT COUNT(*) AS FOUND FROM optician_keys WHERE password = '" + strPassword + "';";
			objDr = db.select(strSql);

			if (objDr.Read())
			{
				if(Convert.ToInt32(objDr["found"]) != 0)
				{
					blnFound = true;
				}
			}

			db.objDataReader.Close();
            db.dbDispose();
			db = null;

			if(blnFound == true)
			{
				strPassword = generatePassword();
			}

			return strPassword;

		}

	}
}
