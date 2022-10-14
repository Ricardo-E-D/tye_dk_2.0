using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.Language language_get_single(Language language) {
			var element = (Data.Language)OW.WrapObject(typeof(Data.Language), language);
			return element;
		}

		public Data.Language LanguageGetSingle(int ID) {
			Data.Language data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.Languages
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = language_get_single(element);
				}
			}
			return data;
		}

		public List<Data.Language> LanguageGetCollection() {
			List<Data.Language> data = new List<Data.Language>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.Languages
									 orderby n.Name
									 select n);

				foreach (var element in elements) {
					data.Add(language_get_single(element));
				}
			}
			return data;
		}

	}
}