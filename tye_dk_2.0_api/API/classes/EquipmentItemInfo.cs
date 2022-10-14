	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
namespace tye.Data {
		public class EquipmentItemInfo {
			
			/*
			private int _id = 0;
			private List<EquipmentItem> _equipment_items = new List<EquipmentItem>();
			private List<Language> _languages = new List<Language>();
			private float _price = 0;
			private string _description = "";
			*/
			public int ID {
				 get; set;
				/*get { return _id; } set { _id = value; }*/
			}
			public int EquipmentItemID {
				get;
				set;
				/*get { return _id; } set { _id = value; }*/
			}
			public EquipmentItem EquipmentItem {
				 get; set;
				/*get { return _equipment_items; } set { _equipment_items = value; }*/
			}
			public int LanguageID { get; set; }
			public tye.Data.Language Language {
				get;
				set;
				/*get { return _languageids; } set { _languageids = value; }*/
			}
			public double Price {
				 get; set;
				/*get { return _price; } set { _price = value; }*/
			}
			public string Description {
				 get; set;
				/*get { return _description; } set { _description = value; }*/
			}
			public EquipmentItemInfo() { }
		}
	}
