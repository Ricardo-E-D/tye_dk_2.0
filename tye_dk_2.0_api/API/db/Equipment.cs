using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {
		
		private Data.Equipment equipment_get_single(Equipment data) {
			var quip =  (Data.Equipment)OW.WrapObject(typeof(Data.Equipment), data);
			foreach (var info in data.EquipmentInfoes)
				quip.Infos.Add(equipmentinfo_get_single(info));
			foreach (var item in data.EquipmentItems)
				quip.Items.Add(equipmentitem_get_single(item));
			return quip;
		}

		public void EquipmentDelete(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.Equipments
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					dc.DeleteObject(data);
					dc.SaveChanges();
				}
			} // using
		}

		public Data.Equipment EquipmentGetSingle(int ID) {
			Data.Equipment equipment = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.Equipments
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					equipment = equipment_get_single(data);
				}
			} // using
			return equipment;
		}

		public List<Data.Equipment> EquipmentGetCollection() {
			List<Data.Equipment> lstequipment = new List<Data.Equipment>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.Equipments
								 select n);
				foreach (var data in datas)
					lstequipment.Add(equipment_get_single(data));
			} // using
			return lstequipment;
		}

		public Data.Equipment EquipmentSave(Data.Equipment savedata) {
			Data.Equipment equipment = savedata;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (savedata.ID == 0) { // new
					var data = new Equipment();
					data = (Equipment)OW.WrapExistingObject(data, savedata);
					dc.Equipments.AddObject(data);
					dc.SaveChanges();
					equipment = equipment_get_single(data);
				} else { // existing
					var data = (from n in dc.Equipments
									where n.ID == savedata.ID
									select n).FirstOrDefault();
					if (data != null) {
						data = (Equipment)OW.WrapExistingObject(data, savedata);
						dc.SaveChanges();
					}
				}
			} // using
			return equipment;
		}
	}
}
