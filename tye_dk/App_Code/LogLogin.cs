using System;

namespace tye
{
	public class LogLogin : Log
	{
		private int intSuccess;
		private string strPassword;
		private User objUser;


		public LogLogin()
		{

		}

		public int IntSuccess
		{
			get
			{
				return intSuccess;
			}
			set
			{
				intSuccess = value;
			}
		}

		public string StrPassword
		{
			get
			{
				return strPassword;
			}
			set
			{
				strPassword = value;
			}
		}

		public void Insert(LogLogin objLl)
		{
			IntSuccess = 0;

			if (((Login)Session["login"]).BlnSucces == true)
			{
				switch(((Login)Session["login"]).IntUserType)
				{
					case 2:

						objUser = (User)Session["user"];

						break;
					case 3:

						objUser = (User)Session["user"];

						break;
					case 4:

						objUser = (User)Session["user"];

						break;
				}

				try 
				{
					IntSuccess = objUser.IntUserId;
				}
				catch (Exception e)
				{
					IntSuccess = 0;
				}
			}
				
			Database db = new Database();

			string strSql = "INSERT INTO log_login (addedtime,password,ip,success) VALUES(CURRENT_TIMESTAMP(),'" + objLl.StrPassword + "','" + objLl.StrIp + "'," + objLl.IntSuccess + ");";

			db.execSql(strSql);
            db.dbDispose();
			db = null;
			objUser = null;

		}
	}
}
