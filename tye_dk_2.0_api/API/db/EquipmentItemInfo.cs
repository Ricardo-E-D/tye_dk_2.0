using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {
		private Data.EquipmentItemInfo equipmentiteminfo_get_single(EquipmentItemInfo data) {
			return (Data.EquipmentItemInfo)OW.WrapObject(typeof(Data.EquipmentItemInfo), data);
		}
		public Data.EquipmentItemInfo EquipmentItemInfoGetSingle(int ID) {
			Data.EquipmentItemInfo equipmentiteminfo = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.EquipmentItemInfoes
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					equipmentiteminfo = equipmentiteminfo_get_single(data);
				}
			} // using
			return equipmentiteminfo;
		}

		public List<Data.EquipmentItemInfo> EquipmentItemInfoGetCollection() {
			List<Data.EquipmentItemInfo> lstequipmentiteminfo = new List<Data.EquipmentItemInfo>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.EquipmentItemInfoes
								 select n);
				foreach (var data in datas)
					lstequipmentiteminfo.Add(equipmentiteminfo_get_single(data));
			} // using
			return lstequipmentiteminfo;
		}

		public Data.EquipmentItemInfo EquipmentItemInfoSave(Data.EquipmentItemInfo savedata) {
			Data.EquipmentItemInfo equipmentiteminfo = savedata;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (savedata.ID == 0) { // new
					var data = new EquipmentItemInfo();
					data = (EquipmentItemInfo)OW.WrapExistingObject(data, savedata);
					dc.EquipmentItemInfoes.AddObject(data);
					dc.SaveChanges();
					equipmentiteminfo = equipmentiteminfo_get_single(data);
				} else { // existing
					var data = (from n in dc.EquipmentItemInfoes
									where n.ID == savedata.ID
									select n).FirstOrDefault();
					if (data != null) {
						data = (EquipmentItemInfo)OW.WrapExistingObject(data, savedata);
						dc.SaveChanges();
					}
				}
			} // using
			return equipmentiteminfo;
		}
	}
}
