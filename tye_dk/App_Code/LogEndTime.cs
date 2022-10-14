using System;

namespace tye
{
	public class LogEndTime : Log
	{
		protected int intClientId;
		protected DateTime datEndTime;

		public LogEndTime(DateTime addedtime,int authorid,int clientid,DateTime endtime,string ip)
		{
			DatAddedTime = addedtime;
			IntAuthorId = authorid;
			intClientId = clientid;
			datEndTime = endtime;
			StrIp = ip;

			Insert();
		}

		new private void Insert()
		{
			Database db = new Database();
			string strSql = "INSERT INTO log_endtime (addedtime,authorid,clientid,endtime,ip) VALUES('" + DatAddedTime.ToString("yyyy-MM-dd HH:mm:ss") + "',";
			strSql += IntAuthorId + "," + intClientId + ",'" + datEndTime.ToString("yyyy-MM-dd") + "','" + StrIp + "');";
			db.execSql(strSql);
            db.dbDispose();
			db = null;
		}
	}
}
