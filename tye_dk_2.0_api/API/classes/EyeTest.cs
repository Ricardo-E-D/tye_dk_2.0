// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class EyeTest {

		public EyeTest() { }

		public int ID { get; set; }
		public string Name { get; set; }
		public string OldBbName { get; set; }
		public int ScoreRequired { get; set; }
		public bool ScreenTest { get; set; }
		public int Priority { get; set; }
		public string InternalName { get; set; }
		public bool HighscoreApplicable { get; set; }

        public int? OpticianID { get; set; }

		List<EyeTestInfo> _Infos = new List<EyeTestInfo>();
		public List<EyeTestInfo> EyeTestInfos {
			get { return _Infos; }
			set { _Infos = value; }
		}

		public string InfoValue(string Type, int LanguageID) {
			string strReturn = String.Empty;
			var eti = EyeTestInfos.Where(n => n.InfoType == Type && n.LanguageID == LanguageID).FirstOrDefault();
			if (eti != null)
				strReturn = eti.InfoText;
			return strReturn;
		}

		public string InfoValue(string Type, Language Language) {
			return InfoValue(Type, Language.ID);
		}
	}
}
