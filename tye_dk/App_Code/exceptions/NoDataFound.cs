using System;
using MySql.Data.MySqlClient;

namespace tye.exceptions
{
	public class NoDataFound : Exception
	{
		new public string Message(int lId)
		{
			string strTmp = "";

			Database db = new Database();
			string strSql = "SELECT header,description,todo FROM errors WHERE languageid = " + lId + " AND typeid = 5;";
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()){
				strTmp += objDr["description"].ToString();
			}
			
			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return strTmp;
		}

		public NoDataFound()
		{
            
		}
	}
}
