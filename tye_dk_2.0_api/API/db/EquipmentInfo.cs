
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {
		
		private Data.EquipmentInfo equipmentinfo_get_single(EquipmentInfo data) {
			return (Data.EquipmentInfo)OW.WrapObject(typeof(Data.EquipmentInfo), data);
		}
		
		public Data.EquipmentInfo EquipmentInfoGetSingle(int ID) {
			Data.EquipmentInfo equipmentinfo = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.EquipmentInfoes
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					equipmentinfo = equipmentinfo_get_single(data);
				}
			} // using
			return equipmentinfo;
		}

		public List<Data.EquipmentInfo> EquipmentInfoGetCollection() {
			List<Data.EquipmentInfo> lstequipmentinfo = new List<Data.EquipmentInfo>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.EquipmentInfoes
								 select n);
				foreach (var data in datas)
					lstequipmentinfo.Add(equipmentinfo_get_single(data));
			} // using
			return lstequipmentinfo;
		}

		public Data.EquipmentInfo EquipmentInfoSave(Data.EquipmentInfo savedata) {
			Data.EquipmentInfo equipmentinfo = savedata;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (savedata.ID == 0) { // new
					var data = new EquipmentInfo();
					data = (EquipmentInfo)OW.WrapExistingObject(data, savedata);
					dc.EquipmentInfoes.AddObject(data);
					dc.SaveChanges();
					equipmentinfo = equipmentinfo_get_single(data);
				} else { // existing
					var data = (from n in dc.EquipmentInfoes
									where n.ID == savedata.ID
									select n).FirstOrDefault();
					if (data != null) {
						data = (EquipmentInfo)OW.WrapExistingObject(data, savedata);
						dc.SaveChanges();
					}
				}
			} // using
			return equipmentinfo;
		}
	}
}
