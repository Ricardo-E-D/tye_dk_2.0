using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.EyeTestInfo eyetestinfo_get_single(EyeTestInfo data) {
			var element = (Data.EyeTestInfo)OW.WrapObject(typeof(Data.EyeTestInfo), data);
			return element;
		}

		public Data.EyeTestInfo EyeTestInfoGetSingle(int ID) {
			Data.EyeTestInfo data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.EyeTestInfos
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = eyetestinfo_get_single(element);
				}
			}
			return data;
		}

		public List<Data.EyeTestInfo> EyeTestInfoGetCollection() {
			List<Data.EyeTestInfo> data = new List<Data.EyeTestInfo>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.EyeTestInfos
									 select n);

				foreach (var element in elements) {
					data.Add(eyetestinfo_get_single(element));
				}
			}
			return data;
		}

		public List<Data.EyeTestInfo> EyeTestInfoGetCollection(int EyeTestID) {
			List<Data.EyeTestInfo> data = new List<Data.EyeTestInfo>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.EyeTestInfos
									 where n.EyeTestID == EyeTestID
									 select n);

				foreach (var element in elements) {
					data.Add(eyetestinfo_get_single(element));
				}
			}
			return data;
		}

		public List<Data.EyeTestInfo> EyeTestInfoGetCollection(int EyeTestID, int LanguageID) {
			List<Data.EyeTestInfo> data = new List<Data.EyeTestInfo>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.EyeTestInfos
									 where n.EyeTestID == EyeTestID && n.LanguageID == LanguageID
									 select n);

				foreach (var element in elements) {
					data.Add(eyetestinfo_get_single(element));
				}
			}
			return data;
		}

		public void EyeTestInfoSave(Data.EyeTestInfo data) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (data.ID == 0) { // new
					EyeTestInfo entry = (EyeTestInfo)OW.WrapObject(typeof(EyeTestInfo), data);
					dc.EyeTestInfos.AddObject(entry);
					dc.SaveChanges();
				} else {  // existing
					var entry = (from n in dc.EyeTestInfos
									 where n.ID == data.ID
									 select n).FirstOrDefault();
					if (entry != null) {
						entry = (EyeTestInfo)OW.WrapExistingObject(entry, data);
						dc.SaveChanges();
					}
				}
			}
		}

		public void EyeTestInfoDelete(int EyeTestInfoID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {

				var entry = (from n in dc.EyeTestInfos
								 where n.ID == EyeTestInfoID
								 select n).FirstOrDefault();
				if (entry != null) {
					dc.DeleteObject(entry);
					dc.SaveChanges();
				}
			}
		}
	}
}