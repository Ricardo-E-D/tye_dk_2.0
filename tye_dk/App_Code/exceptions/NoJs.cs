using System;
using MySql.Data.MySqlClient;

namespace tye.exceptions
{
	public class NoJs : Exception
	{
		new public string Message()
		{
			string strTmp = "";

			Database db = new Database();
			string strSql = "SELECT header,description,todo FROM errors WHERE typeid = 2;";
			MySqlDataReader objDr = db.select(strSql);

			while(objDr.Read()){
				strTmp += "<div class='page_subheader'>" + objDr["header"].ToString() + "</div>";
				strTmp += objDr["description"].ToString();
				strTmp += "<div style='margin-top:15px;margin-bottom:20px;' class='italic_text'>" + objDr["todo"].ToString() + "</div>";
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return strTmp;
		}

		public NoJs()
		{
			
		}
	}
}
