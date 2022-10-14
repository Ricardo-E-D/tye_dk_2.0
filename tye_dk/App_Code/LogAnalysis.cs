using System;

namespace tye
{
	public class LogAnalysis : Log
	{
		protected int intClientId;
		protected int theId;
		protected string strField;
		protected string strTableName;

		public LogAnalysis(int authorid,int clientid,int theid,string ip,string field,string tbname)
		{
			IntAuthorId = authorid;
			intClientId = clientid;
			theId = theid; //Control,start eller endid
			StrIp = ip;
			strField = field;
			strTableName = tbname;

			Insert();
		}

		new private void Insert()
		{
			Database db = new Database();
			string strSql = "INSERT INTO " + strTableName + " (addedtime,authorid,clientid," + strField + ",ip) VALUES(CURRENT_TIMESTAMP(),";
			strSql += IntAuthorId + "," + intClientId + "," + theId + ",'" + StrIp + "');";

			db.execSql(strSql);
            db.dbDispose();
			db = null;
		}
	}
}
