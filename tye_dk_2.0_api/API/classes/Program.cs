// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class Program {

		public Program() { }

		public int ID { get; set; }
		public int ClientUserID { get; set; }
		public string Comment { get; set; }

		List<ProgramEyeTest> _ProgramEyeTests = new List<ProgramEyeTest>();
		public List<ProgramEyeTest> ProgramEyeTests { get { return _ProgramEyeTests; } set { _ProgramEyeTests = value; } }
	}
}
