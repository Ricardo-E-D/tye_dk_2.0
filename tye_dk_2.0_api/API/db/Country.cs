using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		private Data.Country country_get_single(Country data) {
			var element = (Data.Country)OW.WrapObject(typeof(Data.Country), data);
			return element;
		}

		public Data.Country CountryGetSingle(int ID) {
			Data.Country data = null;
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.Countries
									where n.ID == ID
									select n).FirstOrDefault();

				if (element != null) {
					data = country_get_single(element);
				}
			}
			return data;
		}

		public List<Data.Country> CountryGetCollection() {
			List<Data.Country> data = new List<Data.Country>();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var elements = (from n in dc.Countries
									 orderby n.Name
									 select n);

				foreach (var element in elements) {
					data.Add(country_get_single(element));
				}
			}
			return data;
		}

	}
}