using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Translate
/// </summary>
/// 

namespace tye
{
	public class Translate
	{
		//new string[] { 0: none, 1: dansk, 2: norsk, 3: engelsk, 4: tysk }; 
		
		public static string Yes(int languageId) {
			string[] _yes = new string[] { "", "Ja", "Ja", "Yes", "Ja" };
			return _yes[languageId];
		}

		public static string No(int languageId)
		{
			string[] _no = new string[] { "", "Nej", "Nei", "No", "Nein" };
			return _no[languageId];
		}

		public static string SelfPrint(int languageId)
		{
			string[] _sp = new string[] { "", "Printer selv", "Printer selv", "Client prints", "Klient Drucke" };
			return _sp[languageId];
		}
		public static string TrainingScheme(int languageId)
		{
			string[] _sp = new string[] { "", "Synstræningsskema", "Synstreningskjema", "VT-schedule", "Visualtraining Übungsplan" };
			return _sp[languageId];
		}
		public static string Language(int languageId)
		{
			string[] _data = new string[] { "", "Dansk", "Norsk", "Engelsk", "Tysk" };
			return _data[languageId];
		}
		public static string ViewLog(int languageId)
		{
			string[] _data = new string[] { "", "Se trænings-log", "Se trenings-log", "Watch training log", "Übungsprotokoll ansehen" };
			return _data[languageId];
			
		}
		/// <summary>
		/// I know this doesn't below in the yoghurt section!!!!
		/// </summary>
		public static string noSq(string data) {
			string m = data.ToString();
			m = m.Replace("'", "''");
			return m;
		}
	}
}