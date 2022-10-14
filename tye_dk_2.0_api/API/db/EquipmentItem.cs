using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {
		
		private Data.EquipmentItem equipmentitem_get_single(EquipmentItem data) {
			var quip = (Data.EquipmentItem)OW.WrapObject(typeof(Data.EquipmentItem), data);
			foreach (var info in data.EquipmentItemInfoes)
				quip.Infos.Add(equipmentiteminfo_get_single(info));
			return quip;
		}

		public void EquipmentItemDelete(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.EquipmentItems
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					dc.DeleteObject(data);
					dc.SaveChanges();
				}
			} // using
		}

		public Data.EquipmentItem EquipmentItemGetSingle(int ID) {
			Data.EquipmentItem equipmentitem = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.EquipmentItems
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					equipmentitem = equipmentitem_get_single(data);
				}
			} // using
			return equipmentitem;
		}

		public List<Data.EquipmentItem> EquipmentItemGetCollection(int EquipmentID) {
			List<Data.EquipmentItem> lstequipmentitem = new List<Data.EquipmentItem>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.EquipmentItems
								 where n.EquipmentID == EquipmentID
								 select n);
				foreach (var data in datas)
					lstequipmentitem.Add(equipmentitem_get_single(data));
			} // using
			return lstequipmentitem;
		}

		public Data.EquipmentItem EquipmentItemSave(Data.EquipmentItem savedata) {
			Data.EquipmentItem equipmentitem = savedata;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (savedata.ID == 0) { // new
					var data = new EquipmentItem();
					data = (EquipmentItem)OW.WrapExistingObject(data, savedata);
					dc.EquipmentItems.AddObject(data);
					dc.SaveChanges();
					equipmentitem = equipmentitem_get_single(data);
				} else { // existing
					var data = (from n in dc.EquipmentItems
									where n.ID == savedata.ID
									select n).FirstOrDefault();
					if (data != null) {
						data = (EquipmentItem)OW.WrapExistingObject(data, savedata);
						dc.SaveChanges();
					}
				}
			} // using
			return equipmentitem;
		}

	}
}
