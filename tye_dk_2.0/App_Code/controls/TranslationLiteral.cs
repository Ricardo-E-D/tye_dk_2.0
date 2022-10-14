using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tye.Controls {
	/// <summary>
	/// Summary description for TranslationLiteral
	/// </summary>
	public class TransLit : System.Web.UI.WebControls.PlaceHolder {

		public TransLit() { }

		public string CssClass { get; set; }
		public string TagName { get; set; }
		/// <summary>
		/// The translation key
		/// </summary>
		public string Key { get; set; }
		public string Text { get; set; }
		public bool ToLower { get; set; }
		
		private bool _ignoreLineBreaks = false;
		public bool IgnoreLineBreaks { get { return _ignoreLineBreaks; } set { _ignoreLineBreaks = value;  } }

		private string getDictionaryValue() {
			string strReturn = "";
			if (this.Page.Master == null)
				return strReturn;

			Type typeMaster = this.Page.Master.GetType();

			if (typeMaster.BaseType.BaseType.Name == "MasterBase") {
				object objLanguage = typeMaster.GetProperty("CurrentLanguage").GetValue(this.Page.Master, new object[0]);
				object objDictionary = typeMaster.GetProperty("Dictionary").GetValue(this.Page.Master, new object[0]);
				if (objDictionary != null && objLanguage != null)
					strReturn = objDictionary.GetType().GetMethod("GetValue").Invoke(objDictionary, new object[] { this.Key, objLanguage }).ToString();
			}
			return strReturn;
		}
		private string evalToLower(string text) {
			return (ToLower ? text.ToLower() : text);
		}
		
		protected override void Render(System.Web.UI.HtmlTextWriter writer) {
			if (!String.IsNullOrEmpty(TagName))
				writer.Write("<"
					+ TagName
					+ (!String.IsNullOrEmpty(CssClass) ? " class=\"" + CssClass + "\"" : "")
					+ ">");

			if (String.IsNullOrEmpty(Key))
				writer.Write(evalToLower(Text));
			else { 
				if(IgnoreLineBreaks)
					writer.Write(evalToLower(getDictionaryValue()));
				else
					writer.Write(evalToLower(getDictionaryValue().Replace(Environment.NewLine, "<br />")));
			}
			RenderChildren(writer);

			if (!String.IsNullOrEmpty(TagName))
				writer.Write("</" + TagName + ">");
		}

	}
}