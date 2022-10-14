using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExtensionMethods
/// </summary>
public static class ExtensionMethods
{
	public static string ToDefString(this DateTime date) {
		if (date != null)
			return date.ToString("yyyy-MM-dd");
		else return "";

	}
}