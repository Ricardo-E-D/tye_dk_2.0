// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class Dictionary {

		private List<DictionaryEntry> _lst_ode = new List<DictionaryEntry>();

		public List<DictionaryEntry> Entries {
			get { return _lst_ode; }
		}

		public List<string> Keys {
			get {
				List<string> lstKeys = new List<string>();
				foreach (DictionaryEntry ode in _lst_ode) {
					if (!lstKeys.Contains(ode.Key))
						lstKeys.Add(ode.Key);
				}
				return lstKeys;
			}
		}

		public void Add(DictionaryEntry Entry) {
			if (_lst_ode.Contains(Entry))
				_lst_ode.Remove(Entry);
			_lst_ode.Add(Entry);
		}

		public bool EntryExists(string Key) {
			foreach (DictionaryEntry odeLoop in _lst_ode) {
				if (odeLoop.Key == Key)
					return true;
			}
			return false;
		}

		public DictionaryEntry GetEntry(string Key) {
			foreach (DictionaryEntry odeLoop in _lst_ode) {
				if (odeLoop.Key == Key)
					return odeLoop;
			}
			return null;
		}

		public string GetValue(string Key, Language Language) {
			string strReturn = "";
			if (EntryExists(Key)) {
				strReturn = GetEntry(Key).GetValue(Language);
			}
			return strReturn;
		}

		/// <summary>
		/// Gets a dictionary item value by Language ID instead of Language instance
		/// </summary>
		/// <param name="Key"></param>
		/// <param name="LanguageID"></param>
		/// <returns></returns>
		public string GetValueByID(string Key, int LanguageID) {
			string strReturn = "";
			if (EntryExists(Key)) {
				strReturn = GetEntry(Key).GetValueByID(LanguageID);
			}
			return strReturn;
		}
	}
}
