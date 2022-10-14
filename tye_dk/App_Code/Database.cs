using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;

namespace tye
{
	/// <summary>
	/// Summary description for Database.
	/// </summary>
	public class Database : System.Web.UI.Page
	{
		
		//static private string strConnection = "DATA SOURCE=lone.andersenit.dk;DATABASE=tyedk;USER ID=tyedk;PASSWORD=mkg58j3t;";
		
		//static private string strConnection = "DATA SOURCE=192.168.2.20;DATABASE=tyedk;USER ID=root;PASSWORD=02spiril04;";
        //static private string strConnection = "DATA SOURCE=127.0.0.1;DATABASE=tye;USER ID=tyedk;PASSWORD=ryoueyeballingme;";
        //static private string strConnection = "DATA SOURCE=217.74.211.235;DATABASE=tye;USER ID=tyedk;PASSWORD=ryoueyeballingme;Max Pool Size=500;";
        static private string strConnection = "DATA SOURCE=mital-sql-local;DATABASE=tye;USER ID=tyedk;PASSWORD=ryoueyeballingme;Max Pool Size=500;";


		private string strSql;
		public MySqlConnection objConnection = new MySqlConnection(strConnection);
		private MySqlCommand objCommand;
		public MySqlDataReader objDataReader = null;
		protected object objScalar = null;
		
		public Database()
		{
			// TODO: Add constructor logic here
		}
		
		public string StrSql
		{
			get
			{
				return strSql;
			}
			set
			{
				strSql = value;
			}
		}

		public MySqlDataReader select(string strSql)
		{
			objConnection.Open();
			objCommand = new MySqlCommand(strSql, objConnection);
			
			objDataReader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
		
			return objDataReader;
					
		}

		public object scalar(string StrSql)
		{
//		 if (objConnection == null)
//         {
//             objConnection = new MySqlConnection(strConnection);
//         }
			objConnection.Open();
			objCommand = new MySqlCommand(StrSql, objConnection);
			objScalar = objCommand.ExecuteScalar();
			if(objScalar == null)
			{
				objScalar = "0";
			}
			objConnection.Close();
			return objScalar;
            //dbDispose();
		}

		public void execSql(string strSql)
		{
//            if (objConnection == null)
//            {
//                objConnection = new MySqlConnection(strConnection);
//            }
            objCommand = new MySqlCommand(strSql, objConnection);
            objConnection.Open();
			objCommand.ExecuteNonQuery();
			objConnection.Close();
            //dbDispose();
		}

        public void dbDispose()
        {
//            if (objConnection != null)
//            {
//                objConnection.Close();
//                objConnection.Dispose();
//                objConnection = null;
//            }
//            if (objDataReader != null)
//            {
//                objDataReader.Close();
//                objDataReader = null;
//            }
        }
	}
}
