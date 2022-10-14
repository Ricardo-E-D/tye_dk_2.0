using System;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

namespace tye
{
	public class Tests : System.Web.UI.Page
	{
		private string strName;
		protected int intTestId;
		protected int intProgramId;
		protected double dblSeconds;
		protected DateTime datStarttime = Convert.ToDateTime("01-01-2004 01:01:01");
		protected int intScore;
		protected int intHighScore;
		protected bool blnHighScore = false;
		protected int intRequirement;
		protected bool blnCompleted = false;
		protected bool blnIsScreen = false;
		protected Hashtable hashInfos;
		protected string strIp;
		protected bool blnSaveLog;
		protected int intPriority;
		protected string attName;
		protected string attValue;


		public Tests() {}

		public Tests(string name,int testId,int programId,int requirement,Hashtable infos,string ip,bool saveLog,int priority,int highscore)
		{
			strName = name;
			intTestId = testId;
			intProgramId = programId;
			intRequirement = requirement;
			hashInfos = infos;
			strIp = ip;
			blnSaveLog = saveLog;
			intHighScore = highscore;
			intPriority = priority;
			attName = getAttName();
			IsScreenExercise = getExerciseIsScreen(testId);
		}

		public Tests GetTestFromId(int intTestId) {

			int intId = intTestId;
			Database db = new Database();

			string strSql = "SELECT test_info_cat.id, tests_infos.body FROM" +
							" tests_infos INNER JOIN" +
							" test_info ON tests_infos.id = test_info.infoid INNER JOIN" +
							" test_info_cat ON test_info_cat.id = tests_infos.catid WHERE" +
							" test_info.testid = " + intId.ToString();

			MySqlDataReader objDr = db.select(strSql);
			Hashtable hashInfos = new Hashtable();
			string Name = "";
			int Requirements = -1;
			int Priority = -1;

			while (objDr.Read()) {
				if (!hashInfos.ContainsKey(Convert.ToInt32(objDr["id"]))) {
					hashInfos.Add(Convert.ToInt32(objDr["id"]), objDr["body"].ToString());
				}
			}

			db.objDataReader.Close();
			db = new Database();

			strSql = "SELECT name,tyename,purpose,isscreen,intro,important,requirement,priority,path FROM" +
				" tests LEFT JOIN test_files ON fileid = test_files.id WHERE" +
				" tests.id = " + intId.ToString();

			objDr = db.select(strSql);
			if(!(objDr.HasRows)) {
				db.objDataReader.Close();
				db = null;
				throw new ApplicationException("wups!!");
			} else {
				objDr.Read();
				Name = objDr["name"].ToString();
				Requirements = Convert.ToInt32(objDr["requirement"]);
				Priority = Convert.ToInt32(objDr["priority"]);
			}
			User U = (User)Session["user"];
			Tests T = new Tests(Name, intId, U.IntProgramId, Requirements, hashInfos, System.Web.HttpContext.Current.Request.UserHostAddress.ToString(), false, Priority, -1);
			db.objDataReader.Close();
			objDr.Close();
			db.Dispose();
			T.intHighScore = GetHighScore(intId);
			return T;
		}

		private int GetHighScore(int TestId) {
			int intReturn = -1;
			Database db = new Database();
			User U = (User)Session["user"];
			string strSql = "SELECT score FROM" +
							" log_testresult l" +
							" WHERE clientid = " + U.IntUserId.ToString() +
							" AND testid = " + TestId.ToString() + 
							" AND highscore = 1 ORDER BY score DESC";
			MySqlDataReader objDr = db.select(strSql);
			if(objDr.HasRows) {
				objDr.Read();
				int.TryParse(objDr[0].ToString(), out intReturn);
			}
			db.objDataReader.Close();
			objDr.Close();
			db.Dispose();
			return intReturn;
		}

		private bool getExerciseIsScreen(int testId) {
			bool blnReturnValue = false;
			Database db = new Database();
			MySqlDataReader rs = db.select("SELECT isscreen FROM tests WHERE id = " + testId.ToString());
			if(rs.HasRows) {
				rs.Read();
				blnReturnValue = (Convert.ToInt32(rs["isscreen"]) == 1);
			}
			db.objDataReader.Close();
			rs.Close();
			db.Dispose();
			return blnReturnValue;
		}

		private string getAttName(){
			string tmpStr = "";

			if(HashInfos.ContainsKey(15)){
				tmpStr = HashInfos[15].ToString();
			}

			return tmpStr;
		}

		public DateTime DatStarttime
		{
			get { return datStarttime; }
			set { datStarttime = value; }
		}

		public string StrName {
			get { return strName; }
		}

		public string StrIp {
			get { return strIp; }
		}

		public int IntTestId {
			get { return intTestId; }
		}

		public int IntScore {
			get { return intScore; }
			set { intScore = value; }
		}

		public int IntHighScore {
			get { return intHighScore; }
			set { intHighScore = value; }
		}

		public double DblSeconds
		{
			get
			{
				return dblSeconds;
			}
			set
			{
				dblSeconds = value;
			}
		}

		public int IntProgramId
		{
			get
			{
				return intProgramId;
			}
		}

		public int IntRequirement
		{
			get
			{
				return intRequirement;
			}
		}

		public bool BlnCompleted
		{
			get
			{
				return blnCompleted;
			}
			set
			{
				blnCompleted = value;
			}
		}

		/// <summary>
		/// Gets or sets if the exercise is a screen task
		/// </summary>
		public bool IsScreenExercise {
			get { return blnIsScreen; }
			set { blnIsScreen = value; }
		}

		public bool BlnHighScore
		{
			get
			{
				return blnHighScore;
			}
			set
			{
				blnHighScore = value;
			}
		}

		public string AttValue
		{
			get
			{
				return attValue;
			}
			set
			{
				attValue = value;
			}
		}

		public string AttName
		{
			get
			{
				return attName;
			}
			set
			{
				attName = value;
			}
		}

		public bool BlnSaveLog
		{
			get
			{
				return blnSaveLog;
			}
		}





		public Hashtable HashInfos
		{
			get
			{
				return hashInfos;
			}
		}

		private void unlockNextLevel() {
			Database db = new Database();
			string strSql = "SELECT test_schedule_tests.id,testid,islocked"
				+ " FROM test_schedule_tests INNER JOIN tests ON testid = tests.id"
				+ " INNER JOIN test_schedule ON scheduleid = test_schedule.id"
				+ " WHERE test_schedule.id = " + intProgramId + " AND tests.priority > " + intPriority 
				+ " AND bbname LIKE '3-D%' AND islocked <> -1 ORDER BY priority LIMIT 0,1";

			MySqlDataReader objDr = db.select(strSql);
			if(objDr.Read()) {
				if(Convert.ToInt32(objDr["islocked"]) == 0) {
					Database db_up = new Database();
					string strSql_up = "UPDATE test_schedule_tests SET islocked = 1 WHERE id = " + Convert.ToInt32(objDr["id"]);
					db_up.execSql(strSql_up);
					db_up = null;
				}
			}

			db.objDataReader.Close();
            db.dbDispose();
            objDr.Close();
			db = null;
		}
		

		public void saveLog() {
			string strSql = "";
			if (((Tests)Session["tests"]).blnCompleted) { 
				unlockNextLevel();
				((Tests)Session["tests"]).blnCompleted = false;
			}

			int intBlnHighscore = 0;
            Database db;
			if(((Tests)Session["tests"]).BlnHighScore) {
				intBlnHighscore = 1;
                db = new Database();
				strSql = "UPDATE log_testresult SET highscore = " + intBlnHighscore.ToString() + " WHERE clientid = " + ((User)Session["user"]).IntUserId + " AND testid = " + ((Tests)Session["tests"]).IntTestId;
				db.execSql(strSql);
                db.dbDispose();
				db = null;
			}

			int intSecondsLimit = 4;
			int intTID = ((Tests)Session["tests"]).IntTestId;
			
			if(intTID == 11 || intTID == 51 || intTID == 91 || intTID == 256) //Racerbil
				intSecondsLimit = 4;
			
			//intSecondsLimit = 4; // !!! For debuggin/development only !!! Try to remeber to delete before upload, please...mital...herrow...(?) :-)

			// don't save log if test time is 20 seconds or less. If so it's most likely a double-logging
			if (((Tests)Session["tests"]).DblSeconds > intSecondsLimit)  {
				db = new Database();
				Tests T = (Tests)Session["tests"];
				strSql = "INSERT INTO log_testresult" +
						 " (addedtime,clientid,testid,programid,seconds,score,highscore,ip,attname,attvalue) VALUES(";
				strSql += "'" + datStarttime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
						  ((User)Session["user"]).IntUserId + "," +
						  T.IntTestId + "," +
						  T.IntProgramId + "," +
						  T.DblSeconds.ToString().Replace(",", ".") + "," +
						  T.IntScore + "," + 
						  intBlnHighscore + ",'" +
						  T.StrIp + "','" +
						  T.AttName + "','" +
						  T.AttValue + "');";

				db.execSql(strSql);
				db.dbDispose();
			}
			db = null;
		}
	}
}
