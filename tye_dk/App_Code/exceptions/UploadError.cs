using System;
using MySql.Data.MySqlClient;

namespace tye.exceptions
{
	public class UploadError : Exception
	{
		private string strError;

		public string StrError
		{
			set
			{
				strError = value;
			}
		}

		new public string Message(int lId)
		{
			string strTmp = "";

			Database db = new Database();
			string strSql = "SELECT header,description,todo FROM errors WHERE languageid = " + lId + " AND typeid = 6;";
			MySqlDataReader objDr = db.select(strSql);

			if(objDr.Read()){
				strTmp = "<div class='page_subheader'>" + objDr["header"].ToString() + "</div>";
				strTmp += objDr["description"].ToString();
				strTmp += "<br/><br/>Fejlen var: " + strError;
				strTmp += "<div style='margin-top:15px;' class='italic_text'>" + objDr["todo"].ToString() + "</div>";
			}
			
			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;

			return strTmp;
		}

		public UploadError()
		{
			
		}
	}
}
