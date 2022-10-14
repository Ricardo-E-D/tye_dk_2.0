using System;

namespace tye
{
	public class LogPageEdit : Log
	{
		
		private int intContentId;

		public LogPageEdit()
		{

		}

		public int IntContentId
		{
			get
			{
				return intContentId;
			}
			set
			{
				intContentId = value;
			}
		}

		public void Insert(LogPageEdit objPe)
		{
			Database db = new Database();

			string strSql = "INSERT INTO log_pageedit (addedtime,authorid,contentid,ip) VALUES(CURRENT_TIMESTAMP()," + objPe.IntAuthorId + "," + objPe.IntContentId + ",'" + objPe.StrIp + "');";

			db.execSql(strSql);
            db.dbDispose();
			db = null;
		}

	}
}
