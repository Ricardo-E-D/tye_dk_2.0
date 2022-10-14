// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class Language {

		public Language() { }
		public Language(int ID) {
			this.ID = ID;
		}
		public Language(int ID, string Name) {
			this.ID = ID;
			this.Name = Name;
		}

		public int ID { get; set; }
		public string Name { get; set; }
	}
}
