using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Shared
/// </summary>
public class Shared
{
	public static bool debug = !false;
	public static string MariaMail = "maria@trainyoureyes.com";
	public static string[] DbLangs = new string[] { "Dk", "No", "Gb", "De" };
	//public static string[] ShopEmails = new string[] { "maria@trainyoureyes.com", "maria@trainyoureyes.com", "maria@trainyoureyes.com", "maria@trainyoureyes.com" };
	public static string[] ShopEmails = new string[] { "Louise@TrainYourEyes.com", "maria@trainyoureyes.com", "maria@trainyoureyes.com", "Andreas.Oehm@TrainYourEyes.com" };

	public enum Language {
		None = 0,
		Danish = 1,
		Norwegian = 2,
		English = 3,
		German = 4
	}
	public static Language LangFromId(int intLanguage) {
		Language lngReturn = Language.None;
		foreach (int value in Enum.GetValues(typeof(Shared.Language))) {
			if(value == intLanguage) {
				lngReturn = (Language)Enum.ToObject(typeof(Language), value);
			}
		}
		return lngReturn;
	}

	public static string LanguageAbbr(Language L) {
		switch (L) { 
			case Language.Danish:
				return "Dk";
			case Language.Norwegian:
				return "No";
			case Language.English:
				return "Gb";
			case Language.German:
				return "De";
			default:
				return "";
		}
	}
	public static string LanguageAbbr(int LanguageId) {
		switch (LanguageId) {
			case 1:
				return "Dk";
			case 2:
				return "No";
			case 3:
				return "Gb";
			case 4:
				return "De";
			default:
				return "";
		}
	}
	//public static int LangDist(int LanguageId) {
	//    int returnvalue = LanguageId;
	//    if(HttpContext.Current.Session["user"] != null) {
	//        tye.User u = (tye.User)HttpContext.Current.Session["user"];
	//        if(!u == null) {
	//            if(u.IsDistributor)
	//                returnvalue = 
	//        }
	//    }
	//}

	public static int UserLang() {
		int intReturn = (int)Shared.Language.Danish;
		if (HttpContext.Current.Session["user"] != null) {
			tye.User u = (tye.User)HttpContext.Current.Session["user"];
			if (u != null)
				intReturn = u.IntLanguageId;
		}
		return intReturn;
	}

	// checks if current user is distributor
	public static bool UserIsDist() {
		bool blnReturn = false;
		if(HttpContext.Current.Session["user"] != null) {
			tye.User u = (tye.User)HttpContext.Current.Session["user"];
			if(u != null)
				blnReturn = u.IsDistributor;
		}
			return blnReturn;
	}

	public static string RqValue(string v) {
		if (HttpContext.Current.Request.QueryString[v] != null) {
			return HttpContext.Current.Request.QueryString[v].ToString();
		} else {
			return "";
		}
	}

	public static string StripHtmlTags(string msg) {
		return Regex.Replace(msg, "<[^>]*>", String.Empty);
	}

	public Shared()
	{
	}
}
