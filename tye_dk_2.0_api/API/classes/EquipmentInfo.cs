using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace tye.Data {
	public class EquipmentInfo {

		/*
		private int _id = 0;
		private List<Equipment> _equipments = new List<Equipment>();
		private List<Language> _languages = new List<Language>();
		private string _name = "";
		private string _descpription = "";
		*/
		public int ID {
			get;
			set;
			/*get { return _id; } set { _id = value; }*/
		}
		public int EquipmentID { get; set; }

		public Equipment Equipment {
			get;
			set;
			/*get { return _equipmentids; } set { _equipmentids = value; }*/
		}
		public int LanguageID { get; set; }
		public tye.Data.Language Language {
			get;
			set;
			/*get { return _languageids; } set { _languageids = value; }*/
		}
		public string Name {
			get;
			set;
			/*get { return _name; } set { _name = value; }*/
		}
		public string Description {
			get;
			set;
			/*get { return _descpription; } set { _descpription = value; }*/
		}
		public EquipmentInfo() { }
	}
}
