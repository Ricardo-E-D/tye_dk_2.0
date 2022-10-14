using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.ActivationCode activationcode_get_single(ActivationCode data) {
			var element = (Data.ActivationCode)OW.WrapObject(typeof(Data.ActivationCode), data);
			return element;
		}

		public void ActivationCodeDelete(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.ActivationCodes
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					dc.ActivationCodes.DeleteObject(element);
					dc.SaveChanges();
				}
			}
		}

		public void ActivationCodeDeleteAll() {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.ActivationCodes
									select n);

				foreach(var del in element)
					dc.ActivationCodes.DeleteObject(del);
				
				dc.SaveChanges();
			}
		}

		public Data.ActivationCode ActivationCodeGetSingle(int ID) {
			Data.ActivationCode data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.ActivationCodes
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = activationcode_get_single(element);
				}
			}
			return data;
		}

		public Data.ActivationCode ActivationCodeGetSingle(string Code) {
			Data.ActivationCode data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.ActivationCodes
									where n.Code == Code
									select n).FirstOrDefault();

				if (element != null) {
					data = activationcode_get_single(element);
				}
			}
			return data;
		}

		public List<Data.ActivationCode> ActivationCodeGetCollection() {
			List<Data.ActivationCode> data = new List<Data.ActivationCode>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.ActivationCodes
									 select n);

				foreach (var element in elements) {
					data.Add(activationcode_get_single(element));
				}
			}
			return data;
		}

		public List<Data.ActivationCode> ActivationCodeGetCollection(int OpticianID) {
			List<Data.ActivationCode> data = new List<Data.ActivationCode>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.ActivationCodes
									 where n.OpticianUserID == OpticianID
									 select n);

				foreach (var element in elements) {
					data.Add(activationcode_get_single(element));
				}
			}
			return data;
		}

		public List<Data.ActivationCode> ActivationCodeGetCollectionByClient(int ClientUserID) {
			List<Data.ActivationCode> data = new List<Data.ActivationCode>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.ActivationCodes
									 where n.ClientUserID == ClientUserID
									 select n);

				foreach (var element in elements) {
					data.Add(activationcode_get_single(element));
				}
			}
			return data;
		}

		public List<Data.ActivationCode> ActivationCodesRemaining(int OpticianID) {
			List<Data.ActivationCode> codes = new List<Data.ActivationCode>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var list = (from n in dc.ActivationCodes
									 where n.OpticianUserID == OpticianID && n.ClientUserID == null
									 select n);
				foreach (var code in list)
					codes.Add(activationcode_get_single(code));
			}
			return codes;
		}

		public void ActivationCodeSave(Data.ActivationCode data) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (data.ID == 0) { // new
					ActivationCode entry = (ActivationCode)OW.WrapObject(typeof(ActivationCode), data);
					dc.ActivationCodes.AddObject(entry);
					if (!data.ActivationDate.HasValue)
						data.ActivationDate = null;
					if (!data.ExpirationDate.HasValue)
						data.ExpirationDate = null;
					dc.SaveChanges();
				} else {  // existing
					var entry = (from n in dc.ActivationCodes
									 where n.ID == data.ID
									 select n).FirstOrDefault();
					if (entry != null) {
						entry = (ActivationCode)OW.WrapExistingObject(entry, data);
						if (!data.ActivationDate.HasValue)
							data.ActivationDate = null;
						if (!data.ExpirationDate.HasValue)
							data.ExpirationDate = null;
						dc.SaveChanges();
					}
				}
			}
		}

	}
}