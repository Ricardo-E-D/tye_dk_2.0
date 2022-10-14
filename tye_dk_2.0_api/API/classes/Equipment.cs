using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace tye.Data {
	public class Equipment {

		/*
		private int _id = 0;
		private bool _active = false;
		private string _picture = "";
		*/
		public int ID {
			get;
			set;
			/*get { return _id; } set { _id = value; }*/
		}
		public bool Active {
			get;
			set;
			/*get { return _active; } set { _active = value; }*/
		}
		public string Picture {
			get;
			set;
			/*get { return _picture; } set { _picture = value; }*/
		}
		public string Name {
			get;
			set;
			/*get { return _picture; } set { _picture = value; }*/
		}
		public Equipment() { }

		private List<EquipmentInfo> _Infos = new List<EquipmentInfo>();
		public List<EquipmentInfo> Infos { get { return _Infos;  } set { _Infos = value; } }

		private List<EquipmentItem> _Items = new List<EquipmentItem>();
		public List<EquipmentItem> Items { get { return _Items;  } set { _Items = value; } }
	}
}
