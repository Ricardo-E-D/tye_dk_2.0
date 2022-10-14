using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExtensionMethods
/// </summary>
public static class ExtensionMethods
{
	public static string Max(this string Text, int MaxLength) {
		return (Text.Length > MaxLength ? Text.Substring(0, MaxLength) : Text);
	}
	public static string NodeValue(this System.Xml.XmlNode node, string PropertyName) {
		var sn = node.SelectSingleNode(PropertyName);
		return (sn == null ? "" : sn.InnerText);
	}

	public static string JsEncode(this string Text) {
		//return Text.Replace("'", @"\'").Replace(Environment.NewLine, "");
		return Text.Replace(Environment.NewLine, "&lt;br /&gt;")
					.Replace(@"\", @"\\")
					.Replace("'", @"\'")
					.Replace("\n\r", "&lt;br /&gt;");
	}
	
}