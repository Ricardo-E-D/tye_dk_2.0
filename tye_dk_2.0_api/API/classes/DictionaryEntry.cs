// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace tye.Data {
	public class DictionaryEntry {
		private Dictionary<int, string> dic = new Dictionary<int, string>();

		public DictionaryEntry() { }
		public DictionaryEntry(string Key) {
			this.Key = Key;
		}

		public string Key {
			get;
			set;
		}

		public int ID {
			get;
			set;
		}
		public bool SystemEntry { get; set; }
		public string Value { 
			get { return ToXml(); } 
			set { this.FromXml(value); } 
		}
		public bool ClientSide { get; set; }

		public void SetValue(Language Language, string Value) {
			if (!dic.ContainsKey(Language.ID))
				dic.Add(Language.ID, Value);
			else
				dic[Language.ID] = Value;
		}


		public string GetValue(Language Language) {
			string strReturnValue = "";
			if (dic.ContainsKey(Language.ID))
				strReturnValue = dic[Language.ID].ToString();
			return strReturnValue;
		}

		public string GetValueByID(int LanguageID) {
			string strReturnValue = "";
			if (dic.ContainsKey(LanguageID))
				strReturnValue = dic[LanguageID].ToString();
			return strReturnValue;
		}

		public string ToXml() {
			using (StringWriter sw = new StringWriter()) {
				using (XmlTextWriter w = new XmlTextWriter(sw)) {

					w.WriteStartDocument(true);
					w.WriteStartElement("items");
					w.Flush();

					foreach (int Key in dic.Keys) {
						w.WriteStartElement("item");

						w.WriteStartElement("key");
						w.WriteString(Key.ToString());
						w.WriteEndElement();

						w.WriteStartElement("value");
						w.WriteString(dic[Key]);
						w.WriteEndElement();

						w.WriteEndElement();
					}
					w.WriteEndElement();
					w.Flush();
					//w.WriteEndDocument();
				}
				return sw.ToString();
			}
		}

		public void FromXml(string xml) {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);

			if (!doc.HasChildNodes)
				return;

			dic.Clear();
			XmlNode n1 = doc.ChildNodes[0];
			XmlNode n2 = doc.ChildNodes[1];

			foreach (XmlNode node in n2.ChildNodes) {
				if (node.ChildNodes.Count == 2) {
					int i = 0;
					string q = node.ChildNodes[0].InnerXml.ToString();

					if (!int.TryParse(node.ChildNodes[0].InnerText, out i))
						continue;

					dic.Add(i, node.ChildNodes[1].InnerText + "");
				}
			}
			
		}

	}
}
