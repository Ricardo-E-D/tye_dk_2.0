using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {
		
		private Data.EyeTestLink eyetestlink_get_single(EyeTestLink data) {
			var item =  (Data.EyeTestLink)OW.WrapObject(typeof(Data.EyeTestLink), data);
            return item;
		}

		public void EyeTestLinkDelete(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.EyeTestLinks
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					dc.DeleteObject(data);
					dc.SaveChanges();
				}
			} // using
		}

		public Data.EyeTestLink EyeTestLinkGetSingle(int ID) {
			Data.EyeTestLink eyetestlink = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var data = (from n in dc.EyeTestLinks
								where n.ID == ID
								select n).FirstOrDefault();
				if (data != null) {
					eyetestlink = eyetestlink_get_single(data);
				}
			} // using
			return eyetestlink;
		}

		public List<Data.EyeTestLink> EyeTestLinkGetCollection(int OpticianID, int EyeTestID) {
			List<Data.EyeTestLink> lsteyetestlink = new List<Data.EyeTestLink>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var datas = (from n in dc.EyeTestLinks
                             where n.OpticianID == OpticianID && n.EyeTestID == EyeTestID
								 select n);
				foreach (var data in datas)
					lsteyetestlink.Add(eyetestlink_get_single(data));
			} // using
			return lsteyetestlink;
		}

		public Data.EyeTestLink EyeTestLinkSave(Data.EyeTestLink savedata) {
			Data.EyeTestLink eyetestlink = savedata;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (savedata.ID == 0) { // new
					var data = new EyeTestLink();
					data = (EyeTestLink)OW.WrapExistingObject(data, savedata);
					dc.EyeTestLinks.AddObject(data);
					dc.SaveChanges();
					eyetestlink = eyetestlink_get_single(data);
				} else { // existing
					var data = (from n in dc.EyeTestLinks
									where n.ID == savedata.ID
									select n).FirstOrDefault();
					if (data != null) {
						data = (EyeTestLink)OW.WrapExistingObject(data, savedata);
						dc.SaveChanges();
					}
				}
			} // using
			return eyetestlink;
		}
	}
}
