using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;
using System.IO;
using Ionic.Zip;

public class DataDumper {

	private string strConnectionString = "";
	private MySqlConnection conn;
	private MySqlCommand sqlCom;

	private Boolean debug = false;

	/// <summary>
	/// Instanciates a new common db class and initializes connection to database based on the connectionstring passed
	/// </summary>
	/// <param name="ConnectionString"></param>
	public DataDumper(string ConnectionString) {
		try {
			strConnectionString = ConnectionString;
		} catch (Exception) {
			throw new ApplicationException("Connectionstring appears to be invalid");
		}
	}

	public DataDumper(string ConnectionString, bool GetFromAppSettings) {
		try {
			if (GetFromAppSettings) {
				strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
			} else {
				strConnectionString = ConnectionString;
			}
		} catch (Exception) {
			throw new ApplicationException("Connectionstring appears to be invalid");
		}
	}

	/// <summary>
	/// Open connection to the database and instanciate sqlCommand
	/// </summary>
	private void openConnection() {
		conn = new MySqlConnection(strConnectionString);
		conn.Open();
		sqlCom = new MySqlCommand();
		sqlCom.Connection = conn;
		sqlCom.CommandType = CommandType.Text;
	}

	///// <summary>
	///// Open connection to the database, executes sqlCommand based on passed SQL-string and returns an SqlDataReader
	///// </summary>
	///// <param name="strSQL">SQL-string to use with SqlCommand</param>
	///// <returns>SqlDataReader object</returns>
	//private SqlDataReader getReader(string strSQL)
	//{
	//   this.openConnection();
	//   sqlCom = new SqlCommand(strSQL, this.conn);
	//   SqlDataReader rs;
	//   rs = sqlCom.ExecuteReader(CommandBehavior.CloseConnection);
	//   return rs;
	//}

	/// <summary>
	/// Close any open connections and objects and dispose
	/// </summary>
	public void Dispose() {
		this.closeConnection();
	}

	/// <summary>
	/// Close any open connections and objects and dispose
	/// </summary>
	private void closeConnection() {
		if (sqlCom != null)
			sqlCom.Dispose();
		sqlCom = null;
		if (conn != null) {
			if (conn.State != ConnectionState.Closed)
				conn.Close();
			conn.Dispose();
		}
	}

	// MsSql // private enum Columns { ColumnName = 0, ColumnSize = 2, DataType = 12, AllowDBNull = 13, IsIdentity = 17, IsAutoIncrement = 18, IsLong = 21, DataTypeName = 24 };

	// MySql
	// ColumnName ColumnOrdinal ColumnSize NumericPrecision NumericScale IsUnique IsKey BaseCatalogName BaseColumnName BaseSchemaName BaseTableName DataType AllowDBNull ProviderType IsAliased IsExpression IsIdentity IsAutoIncrement IsRowVersion IsHidden IsLong IsReadOnly 
	// substitute column "IsIdentity" for columns "IsKey"
	private enum Columns { ColumnName = 0, ColumnSize = 2, DataType = 11, AllowDBNull = 12, IsIdentity = 6, IsAutoIncrement = 17, IsLong = 20, DataTypeName = 11 };
	// // MySql

	/// <summary>
	/// Save a databae table definition overview
	/// </summary>
	/// <param name="DatabaseTables"></param>
	/// <param name="OutputFilepath"></param>
	/// <param name="OverWrite"></param>
	/// <returns></returns>
	public string SaveTableDefinitionToCsv(string[] DatabaseTables, string OutputFilepath, bool OverWrite) {
		if (System.IO.File.Exists(OutputFilepath) && OutputFilepath.Trim().Length > 2 && !OverWrite) {
			throw new ApplicationException("Output file already exists!");
		}

		foreach (string DatabaseTable in DatabaseTables) {
			if (!DatabaseTableExists(DatabaseTable))
				throw new ApplicationException("Database table '" + DatabaseTable + "' doesn't exist in database!");
		}

		StringBuilder sb = new StringBuilder();
		string nl = Environment.NewLine;

		foreach (string DatabaseTable in DatabaseTables) {
			Hashtable hashColumnDefault = new Hashtable();

			openConnection();
			try {
				sqlCom.CommandText = "SELECT COLUMN_NAME, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE (TABLE_NAME = '" + DatabaseTable + "')";
				MySqlDataReader rs = sqlCom.ExecuteReader(); // getReader("SELECT COLUMN_NAME, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE (TABLE_NAME = '" + DatabaseTable + "')");
				if (rs.HasRows) {
					while (rs.Read()) {
						if (rs["COLUMN_DEFAULT"] != DBNull.Value) {
							if (!hashColumnDefault.ContainsKey(rs["COLUMN_NAME"].ToString())) {
								hashColumnDefault.Add(rs["COLUMN_NAME"].ToString().Trim(), rs["COLUMN_DEFAULT"].ToString().Trim());
							}
						}
					}
				}
				rs.Close();
			} catch (Exception ex) {
				string pis = ex.InnerException.Message;
				return pis;
			}
			//closeConnection();

			sqlCom.CommandText = "SELECT * FROM " + DatabaseTable + " LIMIT 1";
			MySqlDataReader rsa = sqlCom.ExecuteReader(); // getReader("SELECT TOP 1 * FROM " + DatabaseTable);
			DataTable dt = rsa.GetSchemaTable();
			rsa.Close();

			closeConnection();

			if (OutputFilepath.Trim().Length < 3) {
				sb.Append("<table cellpadding=\"3\" cellspacing=\"0\" border=\"1\">");
				//for (int i = 0; i < dt.Rows.Count; i++) {
				//    DataRow d = dt.Rows[i];
				//    for (int j = 0; j < dt.Columns.Count; j++) {
				//        sb.Append("<tr><td>" + dt.Columns[j].ColumnName + ":</td><td> " + d[j].ToString() + "</td></tr>" + nl);
				//    }
				//}
				sb.Append("<tr><td colspan=\"6\"><b>" + DatabaseTable + "</b></td></tr>");

				foreach (DataRow dtr in dt.Rows) {
					sb.Append("<tr>" + Environment.NewLine);
					sb.Append("<td>" + (Convert.ToBoolean(dtr[Convert.ToInt16(Columns.IsIdentity)]) == true ? "[pk]" : "&nbsp;") + "</td>");
					sb.Append("<td>" + dtr[Convert.ToInt16(Columns.ColumnName)].ToString());
					sb.Append("</td>");

					sb.Append("<td>" + dtr[Convert.ToInt16(Columns.DataTypeName)].ToString());

					if (!Convert.ToBoolean(dtr[Convert.ToInt16(Columns.IsLong)]))
						sb.Append(" (" + dtr[Convert.ToInt16(Columns.ColumnSize)].ToString() + ")");
					sb.Append("</td>");

					sb.Append("<td>" + dtr[Convert.ToInt16(Columns.DataType)].ToString().Replace("System.", "") + "</td>");
					sb.Append("<td>" + (Convert.ToBoolean(dtr[Convert.ToInt16(Columns.AllowDBNull)]) == true ? "&nbsp;" : "Not Null") + "</td>");
					if (hashColumnDefault.ContainsKey(dtr[Convert.ToInt16(Columns.ColumnName)].ToString())) {
						sb.Append("<td>" + hashColumnDefault[dtr[Convert.ToInt16(Columns.ColumnName)]].ToString() + "</td>");
					} else {
						sb.Append("<td>&nbsp;</td>");
					}

					sb.Append("<td>" + (Convert.ToBoolean(dtr[Convert.ToInt16(Columns.IsAutoIncrement)]) == true ? "autoincrement" : "&nbsp;") + "</td>");
					sb.Append("</tr>" + Environment.NewLine);
				}
				sb.Append("</table><br>");

			} else { // to csv

				sb.Append("\"" + DatabaseTable + "\"" + nl);

				foreach (DataRow dtr in dt.Rows) {
					sb.Append("\"" + (Convert.ToBoolean(dtr[Convert.ToInt16(Columns.IsIdentity)]) == true ? "[pk]" : "") + "\";");
					sb.Append("\"" + dtr[Convert.ToInt16(Columns.ColumnName)].ToString() + "\";");

					sb.Append("\"" + dtr[Convert.ToInt16(Columns.DataTypeName)].ToString());

					if (!Convert.ToBoolean(dtr[Convert.ToInt16(Columns.IsLong)]))
						sb.Append(" (" + dtr[Convert.ToInt16(Columns.ColumnSize)].ToString() + ")");
					sb.Append("\";");

					sb.Append("\"" + dtr[Convert.ToInt16(Columns.DataType)].ToString().Replace("System.", "") + "\";");
					sb.Append("\"" + (Convert.ToBoolean(dtr[Convert.ToInt16(Columns.AllowDBNull)]) == true ? "" : "Not Null") + "\";");
					if (hashColumnDefault.ContainsKey(dtr[Convert.ToInt16(Columns.ColumnName)].ToString())) {
						sb.Append("\"" + hashColumnDefault[dtr[Convert.ToInt16(Columns.ColumnName)]].ToString() + "\";");
					} else {
						sb.Append("\" \";");
					}

					sb.Append("\"" + (Convert.ToBoolean(dtr[Convert.ToInt16(Columns.IsAutoIncrement)]) == true ? "autoincrement" : "") + "\";");
					sb.Append(nl);
				}
				sb.Append(nl);
			}
		}

		if (OutputFilepath.Trim().Length < 3) {
			return sb.ToString();
		} else {
			if (!System.IO.File.Exists(OutputFilepath)) {
				System.IO.StreamWriter sw = System.IO.File.CreateText(OutputFilepath);
				sw.Write(sb.ToString());
				sw.Close();
				sw.Dispose();
			} else {
				if (OverWrite) {
					System.IO.File.Delete(OutputFilepath);
					System.IO.StreamWriter sw = System.IO.File.CreateText(OutputFilepath);
					sw.Write(sb.ToString());
					sw.Close();
					sw.Dispose();
				}
			}
		}
		return "";
	}

	/// <summary>
	/// Checks the a table with the name passed exists in database
	/// </summary>
	/// <param name="TableName"></param>
	/// <returns></returns>
	private bool DatabaseTableExists(string TableName) {
		bool blnFoundTable = false;
		openConnection();
		sqlCom.CommandText = "SELECT * FROM INFORMATION_SCHEMA.Tables WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = '" + TableName + "'";
		MySqlDataReader rs = sqlCom.ExecuteReader(); // getReader("SELECT * FROM INFORMATION_SCHEMA.Tables WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = '" + TableName + "'");
		blnFoundTable = rs.HasRows;
		closeConnection();
		return blnFoundTable;
	}

	/// <summary>
	/// Insert a new, empty, row in database table passed, taking into account any default values and primary keys that may exist
	/// </summary>
	/// <param name="TableName">Name of database table to insert row into</param>
	/// <returns>Integer, primary key column value of newly inserted row</returns>
	public string Dump(string TableName) {
		string strDir = HttpContext.Current.Server.MapPath(".") + "\\datadump\\";
		List<string> lstTablesCreate = new List<string>();
		if (File.Exists(strDir + "tables.tx")) {
			string s = "";

			foreach(string line in File.ReadAllLines(strDir + "tables.tx")) {
				if (line.StartsWith("--"))
					continue;
				if (line.Length == 0)
					continue;
				if (line.StartsWith("CREATE")) {
					if (s != "") {
						lstTablesCreate.Add(s);
						s = "";
					}
				}

				s += line;
			}
		

			StreamWriter swTables = new StreamWriter(strDir + "tables_out.sql");
			foreach (string strTablesCreate in lstTablesCreate)
				swTables.WriteLine(strTablesCreate);
			swTables.Close();
			swTables.Dispose();
		}

		List<string> tables = new List<string>();

		if (TableName == "") {
			openConnection();
			sqlCom.CommandText = "SELECT * FROM INFORMATION_SCHEMA.Tables WHERE TABLE_TYPE = 'BASE TABLE'";
			MySqlDataReader rs = sqlCom.ExecuteReader();
			if (rs.HasRows) {
				while (rs.Read())
					tables.Add(rs["TABLE_NAME"].ToString());
			}
			rs.Close();
			//rs.Dispose();
			closeConnection();
		} else {
			tables.Add(TableName);
		}

		StringBuilder sb = new StringBuilder();
		Dictionary<string, string> dicUniqueDataTypes = new Dictionary<string, string>();

		string strFilename = HttpContext.Current.Server.MapPath(".") + "\\datadump\\"; //diller.txt";

		if (!Directory.Exists(strFilename))
			Directory.CreateDirectory(strFilename);

		//StreamWriter swCreate = File.CreateText(strFilename);
		//swCreate.Close();
		//swCreate.Dispose();


		//tables.Clear();
		//tables.Add("questions");

		foreach (string table in tables) {
			using (StreamWriter sw = new StreamWriter(strFilename + table + ".txt", false, Encoding.UTF8)) {
				int intReturnValue = 0;
				string nl = Environment.NewLine;
				Hashtable hashColumnDefault = new Hashtable();
				Hashtable hashColumns = new Hashtable();
				openConnection();

				try {
					sqlCom.CommandText = "SELECT COLUMN_NAME, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE (TABLE_NAME = '" + TableName + "')";
					MySqlDataReader rs = sqlCom.ExecuteReader();
					if (rs.HasRows) {
						while (rs.Read()) { // get table information from schema
							if (rs["COLUMN_DEFAULT"] != DBNull.Value) {
								if (!hashColumnDefault.ContainsKey(rs["COLUMN_NAME"].ToString())) { // insert column default values into hashtable
									hashColumnDefault.Add(rs["COLUMN_NAME"].ToString().Trim(), rs["COLUMN_DEFAULT"].ToString().Trim());
								}
							}
						}
					}
					rs.Close();
				} catch (Exception ex) { // shit happened
					string pis = ex.InnerException.Message;
				}

				sqlCom.CommandText = "SELECT * FROM " + table + " LIMIT 1";
				MySqlDataReader rsa = sqlCom.ExecuteReader();
				DataTable dt = rsa.GetSchemaTable(); // and then retrieve the schema

				// close and dispose of objects
				rsa.Close();
				//rsa.Dispose();

				for (int i = 0; i < dt.Rows.Count; i++) { // loop through table field, but skip the Identity column if any. Append column name to statement
					if (dt.Rows[i][Convert.ToInt16(Columns.IsIdentity)] != DBNull.Value) {
						if (!Convert.ToBoolean(dt.Rows[i][Convert.ToInt16(Columns.IsIdentity)])) {
							hashColumns.Add(dt.Rows[i][Convert.ToInt16(Columns.ColumnName)].ToString(), "");
						}
					}
				}

				// all column names added to statement, now continue to values

				string qwer = "";

				for (int q = 0; q < dt.Columns.Count; q++)
					qwer += q + ": " + dt.Columns[q].ColumnName + "(" + dt.Rows[0][q].ToString() + ") ";

				List<string> lstColumnNames = new List<string>();
				List<string> lstColumnDataTypes = new List<string>();

				for (int i = 0; i < dt.Rows.Count; i++) { // same procedure as above; loop fields and skip Identity columns
					/*if (dt.Rows[i][Convert.ToInt16(Columns.IsIdentity)] != DBNull.Value) {
						if (Convert.ToBoolean(dt.Rows[i][Convert.ToInt16(Columns.IsIdentity)]) == true)
							continue;
					}*/

					string strColumnName = dt.Rows[i][Convert.ToInt16(Columns.ColumnName)].ToString();
					string strDataTypeName = dt.Rows[i][Convert.ToInt16(Columns.DataTypeName)].ToString();

					//sb.Append(strColumnName + "<br />");

					if (!dicUniqueDataTypes.ContainsKey(strDataTypeName))
						dicUniqueDataTypes.Add(strDataTypeName, strDataTypeName);

					lstColumnNames.Add(strColumnName);
					lstColumnDataTypes.Add(strDataTypeName);

				} // for

				if (table.EndsWith("old"))
					continue;

				sqlCom.CommandText = "SELECT * FROM " + table + " ORDER BY " + lstColumnNames[0] + " DESC LIMIT 60000";
				rsa = sqlCom.ExecuteReader();
				if (rsa.HasRows) {

					sw.WriteLine("USE TYE;");
					sb.Append("INSERT INTO " + table + " (");

					for (int i = 0; i < lstColumnNames.Count; i++) {
						sb.Append(lstColumnNames[i]);
						if (i < lstColumnNames.Count - 1)
							sb.Append(",");
					}
					sb.Append(") VALUES");
					bool blnFirst = true;
					while (rsa.Read()) {
						if (!blnFirst)
							sb.Append(", ");
						
						sb.Append("(");

						blnFirst = false;

						for (int i = 0; i < lstColumnNames.Count; i++) {
							string strDataTypeName = lstColumnDataTypes[i];
							string strColumnName = lstColumnNames[i];

							switch (strDataTypeName) { // switch column type and insert "default" value accordingly
								case "System.UInt32":
									sb.Append(rsa[strColumnName].ToString());
									break;
								case "System.UInt64":
									sb.Append(rsa[strColumnName].ToString());
									break;
								case "System.Int32":
									sb.Append(rsa[strColumnName].ToString());
									break;
								case "System.Int64":
									sb.Append(rsa[strColumnName].ToString());
									break;
								case "System.String":
									sb.Append("'" + rsa[strColumnName].ToString().Replace("'", "''") + "'");
									break;
								case "System.Boolean":
									sb.Append(rsa[strColumnName].ToString());
									break;
								case "System.SByte":
									sb.Append("'" + rsa[strColumnName].ToString() + "'");
									break;
								case "System.Single":
									sb.Append(rsa[strColumnName].ToString().Replace(",", "."));
									break;
								case "System.Double":
									sb.Append(rsa[strColumnName].ToString().Replace(",", "."));
									break;
								case "System.Decimal":
									sb.Append(rsa[strColumnName].ToString().Replace(",", "."));
									break;
								case "System.Byte":
									sb.Append(rsa[strColumnName].ToString());
									break;
								case "System.DateTime":
									try {
										DateTime d = Convert.ToDateTime(rsa[strColumnName].ToString());
										sb.Append("'" + d.Year + "-" + d.Month + "-" + d.Day + " " + d.Hour + ":" + d.Minute + ":" + d.Second + "'");
									} catch (Exception) {
										sb.Append("'1900-01-01 00:00:00'");
									}
									break;
								default:
									sb.Append("'" + rsa[strColumnName].ToString() + "'");
									break;

							} // switch
							if (i < lstColumnNames.Count - 1)
								sb.Append(",");
						} // for
						sb.Append(")");
						sw.WriteLine(sb.ToString().Replace(Environment.NewLine, ""));
						sb = new StringBuilder();
					} // while
					sw.WriteLine(";");
				}
				rsa.Close();
				closeConnection();

			} // using

		} // foreach

		foreach (string key in dicUniqueDataTypes.Keys) {
			//sb.Append(key + "<br />");
		}

		string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath(".") + "\\datadump\\", "*.txt");
		StreamWriter swConcat = new StreamWriter(HttpContext.Current.Server.MapPath(".") + "\\datadump\\sql_concat.sql", false, Encoding.UTF8);
		//StreamWriter swConcatSmall = new StreamWriter(HttpContext.Current.Server.MapPath(".") + "\\datadump\\concat_small.sql", false, Encoding.UTF8);
		int intFileCount = 1;

		foreach (string file in files) {
			FileInfo fi = new FileInfo(file);

			if (fi.Length < (1024 * 1024 * 2)) { // 2MB
				swConcat.WriteLine("-- " + file);
				foreach (string line in File.ReadAllLines(file, Encoding.UTF8)) {
					if (line.Length > 0)
						swConcat.WriteLine(line);
				}
			} else {
				StreamWriter swSingle = new StreamWriter(HttpContext.Current.Server.MapPath(".") + "\\datadump\\sql_concat_" + intFileCount + ".sql", false, Encoding.UTF8);
				swSingle.Write(File.ReadAllText(file));
				swSingle.Close();
				swSingle.Dispose();
				intFileCount++;
			}
			
			//swConcat.Write(File.ReadAllText(file));
		}
		foreach (string file in files)
			File.Delete(file);

		ZipFile ZF = new ZipFile();
		ZF.AddFile(HttpContext.Current.Server.MapPath(".") + "\\datadump\\sql_concat.sql");
		for (int i = 1; i <= intFileCount; i++) {
			if (File.Exists(HttpContext.Current.Server.MapPath(".") + "\\datadump\\sql_concat_" + i + ".sql"))
				ZF.AddFile(HttpContext.Current.Server.MapPath(".") + "\\datadump\\sql_concat_" + i + ".sql");
		}
		ZF.Save(HttpContext.Current.Server.MapPath(".") + "\\datadump\\concat.zip");


		 
		swConcat.Close();
		swConcat.Dispose();

		closeConnection();

		return sb.ToString();
	}

	public void ImportTables() {
		string strDir = HttpContext.Current.Server.MapPath(".") + "\\datadump\\";

		if (File.Exists(strDir + "tables_out.sql")) {
			openConnection();
			foreach (string line in File.ReadAllLines(strDir + "tables_out.sql")) {
				sqlCom.CommandText = line;
				sqlCom.ExecuteNonQuery();
			}
			closeConnection();
		}
	}
	public void ImportData() {
		string strDir = HttpContext.Current.Server.MapPath(".") + "\\datadump\\";
		int intErrorCount = 0;
		if (File.Exists(strDir + "concat.sql")) {
			openConnection();
			string[] lines = File.ReadAllLines(strDir + "concat.sql");
			int iLen = lines.Length;
			for (int i = 0; i < iLen; i++) { 
				string line = lines[i];
				if (!line.StartsWith("--") && line != "" && line.StartsWith("INSERT INTO")) {
					if (i < iLen - 1) {
						if (!lines[i + 1].StartsWith("INSERT INTO")) {
							int j = i + 1;
							while (!lines[j].StartsWith("INSERT INTO") && j < (iLen - 1)) {
								line += lines[j];
								j++;
							}
						}
					}
				}
				if (line.StartsWith("INSERT INTO")) {
					
					sqlCom.CommandText = line;
					try {
						sqlCom.ExecuteNonQuery();
					} catch (Exception) {
						intErrorCount++;
					}
				}
			}
			/*
			foreach (string line in File.ReadAllLines(strDir + "concat.sql")) {
				if (!line.StartsWith("--") && line != "") { 
					sqlCom.CommandText = line;
					sqlCom.ExecuteNonQuery();
				}
			}
			 * */
			closeConnection();

			throw new ApplicationException("Counted errors: " + intErrorCount);
		}
	}
}
