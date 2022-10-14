	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
namespace tye.Data {
		public class EquipmentItem {
			
			/*
			private List<Equipment> _equipments = new List<Equipment>();
			private int _id = 0;
			private bool _active = false;
			*/
			public int EquipmentID {
				get;
				set;
				/*get { return _id; } set { _id = value; }*/
			}
			
			public int ID {
				 get; set;
				/*get { return _id; } set { _id = value; }*/
			}
			public bool Active {
				 get; set;
				/*get { return _active; } set { _active = value; }*/
			}
			public EquipmentItem() { }

			private List<EquipmentItemInfo> _Items = new List<EquipmentItemInfo>();
		public List<EquipmentItemInfo> Infos { get { return _Items;  } set { _Items = value; } }

		}
	}
