// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class ProgramEyeTest {

		public ProgramEyeTest() { }

		public int ID { get; set; }
		public int EyeTestID { get; set; }
		public int ProgramID { get; set; }
		public bool Locked { get; set; }
		public bool LockedByOptician { get; set; }
        public int Priority { get; set; }
        
        /// <summary>
        /// For showing/hiding for client
        /// </summary>
        public bool Active { get; set; }
	}
}
