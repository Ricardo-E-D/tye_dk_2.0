using System;

namespace tye
{
	public class LogKeys : Log
	{
		protected int intOptician;
		protected string strKey;

		public LogKeys(int intAuthor,int intOpt,string strK,string strIP,DateTime datAdded)
		{
			DatAddedTime = datAdded;
			IntAuthorId = intAuthor;
			intOptician = intOpt;
			strKey = strK;
			StrIp = strIP;

			Insert();
		}
	
		new private void Insert()
		{
			Database db = new Database();
			string strSql = "INSERT INTO log_keys (addedtime,authorid,opticianid,keycode,ip) VALUES('" + DatAddedTime.ToString("yyyy-MM-dd HH:mm:ss") + "',";
			strSql += IntAuthorId + "," + intOptician + ",'" + strKey + "','" + StrIp + "');";

			db.execSql(strSql);
            db.dbDispose();
			db = null;
		}
	}
}
