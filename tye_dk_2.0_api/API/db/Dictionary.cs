using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API {
	public partial class API {

		public tye.Data.Dictionary DictionaryGet() {
			Data.Dictionary dic = new Data.Dictionary();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var entries = (from n in dc.DictionaryEntries
									select n);
				foreach (var entry in entries) {
					dic.Add(dictionaryEntry_get_single(entry));
				}
			}
			
			return dic;
		}

		public string DictionaryGetClientSide() {
			string q = "tye.dictionary = {";
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var entries = (from n in dc.DictionaryEntries
									where n.ClientSide == true
									select n);

				var langs = LanguageGetCollection();

				foreach (var entry in entries) {
					var csentry = dictionaryEntry_get_single(entry);

					q += csentry.Key.JsEncode() + ":{";

					int langCount = langs.Count;
					for (int i = 0; i < langCount; i++) {
						var lang = langs[i];
						object csValue = new {
							Key = entry.Key,
							Language = lang.ID,
							Value = csentry.GetValue(lang)
						};
						q += "l" + lang.ID + ":'"
							+ csentry.GetValue(lang).JsEncode()
							+ "'" + (i < langCount - 1 ? "," : "");
					}
					q += "},";
				}

			}
			if (q.EndsWith(","))
				q = q.Remove(q.Length - 1);
			return q + "};";
		}

		private tye.Data.DictionaryEntry dictionaryEntry_get_single(DictionaryEntry de) {
			var entry = (tye.Data.DictionaryEntry)OW.WrapObject(typeof(tye.Data.DictionaryEntry), de, new string[] { "EntityState" });
			entry.FromXml(de.Values);
			return entry;
		}

		public void DictionaryEntryDelete(int ID) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				var element = (from n in dc.DictionaryEntries
									where n.ID == ID
									select n).FirstOrDefault();
				if (element != null) {
					dc.DictionaryEntries.DeleteObject(element);
					dc.SaveChanges();
				}
			}
		}
		
		public Data.DictionaryEntry DictionaryEntryGetSingle(int ID) {
			Data.DictionaryEntry entry = new Data.DictionaryEntry();
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) { 
				var ent = (from n in dc.DictionaryEntries
							  where n.ID == ID
							  select n).FirstOrDefault();
				if(ent != null)
					entry = dictionaryEntry_get_single(ent);
			}
			return entry;
		}

		public Data.DictionaryEntry DictionaryEntrySave(Data.DictionaryEntry dictionaryEntry) {
			using (DatabaseEntities dc = new DatabaseEntities(connectionString)) {
				if (dictionaryEntry.ID == 0) { // new
					DictionaryEntry entry = (DictionaryEntry)OW.WrapObject(typeof(DictionaryEntry), dictionaryEntry);
					entry.Values = dictionaryEntry.ToXml();
					dc.DictionaryEntries.AddObject(entry);
					dc.SaveChanges();
					return dictionaryEntry_get_single(entry);
				} else {  // existing
					var entry = (from n in dc.DictionaryEntries
									 where n.ID == dictionaryEntry.ID
									 select n).FirstOrDefault();
					if (entry != null) {
						entry = (DictionaryEntry)OW.WrapExistingObject(entry, dictionaryEntry);
						entry.Values = dictionaryEntry.ToXml();
						dc.SaveChanges();
						return dictionaryEntry_get_single(entry);
					}
				}
			}
			return null;
		}
	} // class
} // namespace
