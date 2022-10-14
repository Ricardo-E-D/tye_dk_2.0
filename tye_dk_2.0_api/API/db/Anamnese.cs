using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.Anamnese anamnese_get_single(Anamnese data) {
			var element = (Data.Anamnese)Helpers.Deserialize(Convert.FromBase64String(data.Data));
			element.ID = data.ID;
			element.Created = data.Created;
			element.ClientUserID = data.ClientUserID;
			return element;
		}

		public Data.Anamnese AnamneseGetSingle(int ID) {
			Data.Anamnese data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.Anamnese
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = anamnese_get_single(element);
				}
			}
			return data;
		}

		public List<Data.Anamnese> AnamneseGetCollection(int ClientUserID) {
			List<Data.Anamnese> data = new List<Data.Anamnese>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.Anamnese
									 where n.ClientUserID == ClientUserID
									 orderby n.Created descending
									 select n);

				foreach (var element in elements) {
					data.Add(anamnese_get_single(element));
				}
			}
			return data;
		}

		public Data.Anamnese AnamneseSave(Data.Anamnese data) {
			string strPud = "";
			Data.Anamnese anam = data;
			try {
				byte[] bytPud = Helpers.Serialize(data);
				strPud = Convert.ToBase64String(bytPud);
			} catch (Exception) { }

			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (data.ID == 0) { // new
					Anamnese entry = new Anamnese() { 
						ID = 0,
						Created = data.Created,
						ClientUserID = data.ClientUserID,
						Data = strPud 
					};

					dc.Anamnese.AddObject(entry);
					dc.SaveChanges();
					anam = anamnese_get_single(entry);
				} else {  // existing
					var entry = (from n in dc.Anamnese
									 where n.ID == data.ID
									 select n).FirstOrDefault();
					if (entry != null) {
						entry.Data = strPud;
						entry.Created = data.Created;
						dc.SaveChanges();
					}
				}
			} // using
			return anam;
		} // method
	}
}