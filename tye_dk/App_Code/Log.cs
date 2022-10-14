using System;
using MySql.Data.MySqlClient;

namespace tye
{
	public class Log : System.Web.UI.Page
	{
		private int intAuthorId;
		private string strIp;
		protected DateTime datAddedTime;
		
		public int IntAuthorId
		{
			get
			{
				return intAuthorId;
			}
			set
			{
				intAuthorId = value;
			}
		}

		public DateTime DatAddedTime
		{
			get
			{
				return datAddedTime;
			}
			set
			{
				datAddedTime = value;
			}
		}

		public string StrIp
		{
			get
			{
				return strIp;
			}
			set
			{
				strIp = value;
			}
		}

		public Log()
		{

		}

		public void Insert(){}

	}
}
