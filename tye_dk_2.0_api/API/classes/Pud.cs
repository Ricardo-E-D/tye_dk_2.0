// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	/// <summary>
	/// Persistant User Data
	/// </summary>
	[Serializable]
	public class Pud {
		System.Collections.Hashtable ht = new System.Collections.Hashtable();

		public Pud() { }

		public enum PudKeys {
			EyeTestInfoLanguage
		}

		public void SetValue(PudKeys Key, object value) {
			SetValue(Key.ToString(), value);
		}
		public void SetValue(string Key, object value) {
			if (ht.ContainsKey(Key))
				ht[Key] = value;
			else
				ht.Add(Key, value);
		}
		public bool HasValue(PudKeys Key) {
			return HasValue(Key.ToString());
		}
		public bool HasValue(string Key) {
			return ht.ContainsKey(Key);
		}
		public object GetValue(PudKeys Key) {
			return GetValue(Key.ToString());
		}
		public object GetValue(string Key) {
			return (HasValue(Key) ? ht[Key] : null as object);
		}

		public string Serialize() {
			return monosolutions.Utils.Serialization.Binary.Serialize(this);
		}

		public static Pud DeSerialize(string Data) {
			return (Pud)monosolutions.Utils.Serialization.Binary.Deserialize(Data);
		}
	}
}
