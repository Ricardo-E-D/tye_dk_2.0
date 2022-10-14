// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class EyeTestInfo {

		public EyeTestInfo() { }

		public int ID { get; set; }
		public int EyeTestID { get; set; }
		public int Priority { get; set; }
		public string InfoType { get; set; }
		public string InfoText { get; set; }
		public int LanguageID { get; set; }

	}
}
