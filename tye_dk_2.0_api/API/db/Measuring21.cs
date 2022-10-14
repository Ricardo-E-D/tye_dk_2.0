using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.Measuring21 measuring21_get_single(Measuring21 data) {
			var element = (Data.Measuring21)Helpers.Deserialize(Convert.FromBase64String(data.Data));
			element.ID = data.ID;
			element.Created = data.Created;
			element.ClientUserID = data.ClientUserID;
			return element;
		}

		public Data.Measuring21 Measuring21GetSingle(int ID) {
			Data.Measuring21 data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.Measuring21
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = measuring21_get_single(element);
				}
			}
			return data;
		}

		public List<Data.Measuring21> Measuring21GetCollection(int ClientUserID) {
			List<Data.Measuring21> data = new List<Data.Measuring21>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.Measuring21
									 where n.ClientUserID == ClientUserID
									 orderby n.Created descending
									 select n);

				foreach (var element in elements) {
					data.Add(measuring21_get_single(element));
				}
			}
			return data;
		}

		public void Measuring21Save(Data.Measuring21 data) {
			string strPud = "";

			try {
				byte[] bytPud = Helpers.Serialize(data);
				strPud = Convert.ToBase64String(bytPud);
			} catch (Exception) { }

			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (data.ID == 0) { // new
					Measuring21 entry = new Measuring21() { 
						ID = 0,
						Created = DateTime.UtcNow, 
						ClientUserID = data.ClientUserID,
						Data = strPud 
					};

					dc.Measuring21.AddObject(entry);
					dc.SaveChanges();
				} else {  // existing
					var entry = (from n in dc.Measuring21
									 where n.ID == data.ID
									 select n).FirstOrDefault();
					if (entry != null) {
						entry.Data = strPud;
						dc.SaveChanges();
					}
				}
			}

		}
	}
}