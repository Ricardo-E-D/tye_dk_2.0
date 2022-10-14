// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tye.Data {
	/// <summary>
	/// Summary description for ValueSet
	/// </summary>
	[Serializable]
	public class ValueSet {

		private Dictionary<string, string> Values = new Dictionary<string, string>(5);
		public ValueSet() {
		}

		public void AddValue(string Key, object Value) {
			if (!Values.ContainsKey(Key))
				Values.Add(Key, Value.ToString());
			else
				Values[Key] = Value.ToString();
		}
		public string GetValue(string Key) {
			if (Values.ContainsKey(Key))
				return Values[Key];
			else
				return "";
		}
		public bool HasKey(string Key) {
			return Values.Keys.Contains(Key);
		}
		public string[] Keys() { return Values.Keys.ToArray(); }
		public int Count() { return Values.Count; }

		public new string ToString() {
			string strReturn = "";
			foreach (string Key in this.Keys()) {
				strReturn += (Key + ":" + GetValue(Key).Replace(":", "") + "\n");
			}
			return strReturn;
		}

		public void FromString(string ValuesAsString) {
			string[] lines = ValuesAsString.Split('\n');
			foreach (string line in lines) {
				string[] pair = line.Split(':');
				if (pair.Length == 2) {
					this.AddValue(pair[0], pair[1]);
				}
			}
		}

	}
}
