using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class ClientEyeTestLog {

		/*
		private int _id = 0;
		private List<ProgramEyeTest> _program_eye_tests = new List<ProgramEyeTest>();
		private int _score = 0;
		private DateTime _start_time = new DateTime();
		private DateTime _end_time = new DateTime();
		private bool _high_score = false;
		private string _attrib_name = "";
		private string _attrib_value = "";
		private string _comment = "";
		*/

		public int ID {
			get;
			set;
			/*get { return _id; } set { _id = value; }*/
		}
		public int ProgramEyeTestID { get; set; }
		public ProgramEyeTest ProgramEyeTest {
			get;
			set;
			/*get { return _program_eye_tests; } set { _program_eye_tests = value; }*/
		}
		public int Score {
			get;
			set;
			/*get { return _score; } set { _score = value; }*/
		}
		public DateTime StartTime {
			get;
			set;
			/*get { return _start_time; } set { _start_time = value; }*/
		}
		public DateTime? EndTime {
			get;
			set;
			/*get { return _end_time; } set { _end_time = value; }*/
		}
		public bool HighScore {
			get;
			set;
			/*get { return _high_score; } set { _high_score = value; }*/
		}
		public string AttribName {
			get;
			set;
			/*get { return _attrib_name; } set { _attrib_name = value; }*/
		}
		public string AttribValue {
			get;
			set;
			/*get { return _attrib_value; } set { _attrib_value = value; }*/
		}
		public string Comment {
			get;
			set;
			/*get { return _comment; } set { _comment = value; }*/
		}
		public string UpdateToken { get; set; }
		public ClientEyeTestLog() { }

	}
}
