using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Web.Hosting;
using HtmlAgilityPack;
using System.Xml.Linq;


public static class DataImportExtensions {
	public static string childNodeValue(this XmlNode node, String ChildNodeName) {
		string ret = "";
		if (node == null)
			return ret;
		else {
			var child = node.SelectSingleNode(ChildNodeName);
			if (child != null)
				ret = child.InnerText;
		}
		return ret;
	}
}

/// <summary>
/// Summary description for DataImport
/// </summary>
public class DataImport {
	HttpContext cont;

	private string filepath(string g) {
		string path = HostingEnvironment.MapPath("~/App_Data/importdata/xml/");
		if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("localhost"))
			path = @"C:\temp\GL - tye.dk - databasebackup\";

		return System.IO.Path.Combine(path, g);
	}

	private void w(string what) {
		System.Diagnostics.Debug.WriteLine(what);
		cont.Response.Write(what + "<br />");
		cont.Response.Flush();
	}

	public DataImport(HttpContext context) {
		cont = context;
	}

	private string xmlWrongXml(string filepath) {
		if (!File.Exists(filepath))
			return "";

		string g = File.ReadAllText(filepath, System.Text.Encoding.UTF8);
		string[] damn = new string[] { "h", "v", "A", "B", "Ah", "Av" };
		for (int i = 1; i < 4; i++) {
			int stop = 19;
			if (i == 2)
				stop = 100;
			if (i == 3)
				stop = 21;

			for (int j = 1; j <= stop; j++) {

				g = g.Replace("<" + i + "_" + j + ">", "<wrong_" + i + "_" + j + ">");
				g = g.Replace("</" + i + "_" + j + ">", "</wrong_" + i + "_" + j + ">");
				w("wrong xml: " + i + "/3 - " + j + "/" + stop);

				foreach (string d in damn) {
					g = g.Replace("<" + i + "_" + j + d + ">", "<wrong_" + i + "_" + j + d + ">");
					g = g.Replace("</" + i + "_" + j + d + ">", "</wrong_" + i + "_" + j + d + ">");
					//w("wrong xml: " + i + "/3 - " + j + d +"/" + stop);
				}
			}
		}
		return g;
	}
	#region queries
	// usertypeid 2 = optician
	// mapid 1 = Denmark
	// mapid 2 = Norway
	// mapid 3 = United Kingdom
	// mapid 4 = Germany
	/* <Opticians - danish> */
	/*
	 SELECT * FROM `users` 
		INNER JOIN `user_optician` ON `users`.`id` = `user_optician`.`userid`
		WHERE usertypeid = 2 AND  `user_optician`.`regionid` IN 
			(SELECT id FROM map_region WHERE mapid = 1)
		ORDER BY `users`.id
	 
	 //isactive = 1
	 */
	/* </Opticians - danish> */

	// <clients, all>
	/*
	 SELECT * FROM `users` 
		INNER JOIN `user_client` ON `users`.`id` = `user_client`.`userid`
		WHERE usertypeid = 3 
		order by `users`.id
	*/
	//AND isactive = 1 
	// </clients, all>


	// <ActivationCodes>
	/* SELECT id, opticianid, keycode, isprinted FROM `log_keys` */
	// </ActivationCodes>

	// <ActivationCodes-Client-Relations>
	/* 
	 SELECT password, addedtime, userid, enddate  FROM `users` 
		INNER JOIN `user_client` ON `users`.`id` = `user_client`.`userid`
	 */
	// </ActivationCodes-Client-Relations>

	#endregion

	internal static string RemoveUnwantedTags(string data) {
		var doc = new HtmlDocument();
		doc.LoadHtml(data);

		foreach (var script in doc.DocumentNode.Descendants("script").ToArray())
			script.Remove();
		foreach (var style in doc.DocumentNode.Descendants("style").ToArray())
			style.Remove();

		string strRetrn = doc.DocumentNode.InnerText.Replace("\t", "");
		while (strRetrn.IndexOf("\r\n\r\n") > -1)
			strRetrn = strRetrn.Replace(("\r\n\r\n"), "\r\n");

		return strRetrn;
	}

	public void ImportOpticians() {
		string FilePath = filepath("tye-8.xml"); // @"C:\temp\GL - tye.dk - databasebackup\tye-8.xml";
		if (!File.Exists(FilePath))
			return;

		//if (!new string[] { "Denmark", "Norway", "Germany", "United Kingdom" }.Contains(Country))
		//throw new Exception("wrong country!");


		using (var ipa = statics.GetApi()) {
			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			//string[] lines = File.ReadAllLines(FilePath);
			var countries = ipa.CountryGetCollection();

			XmlNodeList allopticians = doc.SelectNodes("tye/user_optician");
			int counter = 1;

			foreach (XmlNode node in allopticians) {
				int id = 0;
				if (!int.TryParse(node.childNodeValue("userid"), out id)) {
					w("couldn't parse user id " + id);
					continue;
				}

				var usernode = doc.SelectSingleNode("tye/users[id=" + id + "]");
				if (usernode == null) {
					w("couldn't find user id " + id);
				}

				w("Importing optician " + counter++ + " of " + allopticians.Count);

				string address = usernode.childNodeValue("address");
				string postal = usernode.childNodeValue("zipcode");
				string city = usernode.childNodeValue("city");
				string email = usernode.childNodeValue("email");
				string phone = usernode.childNodeValue("phone");
				string fax = usernode.childNodeValue("fax");
				string name = node.childNodeValue("name").Max(50);
				DateTime CreatedOn = DateTime.Now;

				if (!DateTime.TryParse(usernode.childNodeValue("addedtime"), out CreatedOn))
					CreatedOn = DateTime.Now;

				string countryname = "Denmark";
				switch (node.childNodeValue("regionid")) {
					case "1":
						countryname = "Denmark";
						break;
					case "2":
						countryname = "Norway";
						break;
					case "3":
						countryname = "Germany";
						break;
					case "4":
						countryname = "Germany";
						break;
				}

				tye.Data.User user = new tye.Data.User();
				user.Address = address.Max(100);
				user.PostalCode = postal.Max(50);
				user.City = city.Max(50);
				user.Country = countries.Where(n => n.Name == countryname).First();
				user.CountryID = user.Country.ID;
				user.CreatedOn = CreatedOn;
				user.Description = "";
				user.Email = email;
				user.Enabled = true;
				user.FirstName = name;
				user.MiddleName = "";
				user.LastName = "";
				user.FullName = name;
				user.ID = 0;
				user.JobTitle = "";
				user.MobilePhone = phone;
				user.MustChangePassword = true;
				user.Password = "";
				user.Phone = phone;
				user.Type = tye.Data.User.UserType.Optician;
				user.OldDatabaseID = id;
				user.Birthday = null;
				user.LanguageID = user.Country.ID;
				user.OldPassword = usernode.childNodeValue("password");

				ipa.UserSave(user);

				w("Importing optician " + user.FullName);
			}


		}

	}

	public void ImportClients(/*string FilePath*/) {
		string FilePath = filepath("tye-8.xml"); // @"C:\temp\GL - tye.dk - databasebackup\tye-8.xml";
		if (!File.Exists(FilePath))
			return;

		using (var ipa = statics.GetApi()) {
			string[] lines = File.ReadAllLines(FilePath);
			var countries = ipa.CountryGetCollection();
			var allOpticians = ipa.UserGetCollection().Where(n => n.Type == tye.Data.User.UserType.Optician);

			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			XmlNodeList allclients = doc.SelectNodes("tye/user_client");
			int counter = 0;
			foreach (XmlNode node in allclients) {
				int id = 0;
				int opticianid = 0;

				if (!int.TryParse(node.childNodeValue("userid"), out id)) {
					w("couldn't parse user id importing client ");
					continue;
				}
				int.TryParse(node.childNodeValue("opticianid"), out opticianid);

				XmlNode usernode = doc.SelectSingleNode("tye/users[id=" + id + "]");
				if (usernode == null) {
					w("couldn't find user importing clients - id " + id);
				}

				w("Importing optician " + counter++ + " of " + allclients.Count);

				DateTime birthday = new DateTime(1900, 1, 1);
				//active = (line[10] == "1");


				DateTime.TryParse(node.childNodeValue("birthday"), out birthday);

				tye.Data.User user = new tye.Data.User();
				user.Address = usernode.childNodeValue("address").Max(100);
				user.PostalCode = usernode.childNodeValue("zipcode").Max(50);
				user.City = usernode.childNodeValue("city").Max(50);
				user.Description = "";
				user.Email = usernode.childNodeValue("email");
				user.Enabled = true;
				user.FirstName = node.childNodeValue("firstname").Max(50);
				user.MiddleName = "";
				user.LastName = node.childNodeValue("lastname").Max(50);
				user.FullName = user.FirstName + " " + user.LastName;
				user.ID = 0;
				user.JobTitle = "";
				user.MobilePhone = usernode.childNodeValue("phone");
				user.MustChangePassword = false;
				user.Password = "";
				user.Phone = usernode.childNodeValue("phone");
				user.Type = tye.Data.User.UserType.Client;
				user.OldDatabaseID = id;
				user.Birthday = (birthday < new DateTime(1900, 1, 1) ? new DateTime(1900, 1, 1) : birthday);

				w("Importing user " + user.FullName);

				DateTime CreatedOn = DateTime.Now;

				if (!DateTime.TryParse(usernode.childNodeValue("addedtime"), out CreatedOn))
					CreatedOn = DateTime.Now;

				user.CreatedOn = CreatedOn;

				if (user.Email.ToLower().EndsWith("mital.dk"))
					continue;

				var optician = allOpticians.Where(n => n.OldDatabaseID == opticianid).FirstOrDefault();
				if (optician != null) {
					user.Country = optician.Country;
					user.CountryID = optician.Country.ID;
					user.LanguageID = user.Country.ID;
					var newUser = ipa.UserSave(user);
					ipa.OpticianClientAdd(optician.ID, newUser.ID);
				}

			}
		}
	}

	//public void ImportOpticians(string FilePath, string Country) {
	//   if (!File.Exists(FilePath))
	//      return;

	//   if (!new string[] { "Denmark", "Norway", "Germany", "United Kingdom" }.Contains(Country))
	//      throw new Exception("wrong country!");


	//   using (var ipa = statics.GetApi()) {
	//      string[] lines = File.ReadAllLines(FilePath);
	//      var countries = ipa.CountryGetCollection();

	//      foreach (var linedata in lines.Skip(1)) {

	//         var line = linedata.Split(new string[] { "#|#" }, StringSplitOptions.None);

	//         string address = "";
	//         string postal = "";
	//         string city = "";
	//         string email = "";
	//         string phone = "";
	//         string fax = "";
	//         bool active = true;
	//         string name = "";
	//         int id = 0;

	//         int.TryParse(line[0], out id);
	//         address = line[4];
	//         postal = line[5];
	//         city = line[6];
	//         email = line[7];
	//         phone = line[8];
	//         fax = line[9];
	//         active = (line[10] == "1");
	//         name = line[15].Max(50);

	//         tye.Data.User user = new tye.Data.User();
	//         user.Address = address.Max(100);
	//         user.PostalCode = postal.Max(50);
	//         user.City = city.Max(50);
	//         user.Country = countries.Where(n => n.Name == Country).First();
	//         user.Description = "";
	//         user.Email = email;
	//         user.Enabled = true;
	//         user.FirstName = name;
	//         user.MiddleName = "";
	//         user.LastName = "";
	//         user.FullName = name;
	//         user.ID = 0;
	//         user.JobTitle = "";
	//         user.MobilePhone = phone;
	//         user.MustChangePassword = true;
	//         user.Password = "";
	//         user.Phone = phone;
	//         user.Type = tye.Data.User.UserType.Optician;
	//         user.OldDatabaseID = id;
	//         user.Birthday = null;
	//         user.LanguageID = user.Country.ID;

	//         ipa.UserSave(user);

	//      }


	//   }

	//}


	public void ImportActivationCodes(/*string FilePath*/) {
		string FilePath = filepath("tye-2-log-1.xml"); // @"C:\temp\GL - tye.dk - databasebackup\tye-2-log-1.xml";
		if (!File.Exists(FilePath))
			return;

		using (var ipa = statics.GetApi()) {
			XmlDocument docKeys = new XmlDocument();
			docKeys.Load(FilePath);
			XmlNodeList oldKeys = docKeys.SelectNodes("tye/log_keys");

			XmlDocument docClients = new XmlDocument();
			docClients.Load(filepath("tye-8.xml") /* @"C:\temp\GL - tye.dk - databasebackup\tye-8.xml"*/);

			//XmlNodeList clients = docKeys.SelectNodes("tye/users");

			//string[] oldClientData = File.ReadAllLines(HostingEnvironment.MapPath("/App_Data/importdata/client/all.txt"));
			//string[] lines = File.ReadAllLines(FilePath);
			var allOpticians = ipa.UserGetCollection().Where(n => n.Type == tye.Data.User.UserType.Optician);
			int counter = 0;

			foreach (XmlNode node in oldKeys) {
				//var line = linedata.Split(new string[] { "#|#" }, StringSplitOptions.None);

				//id, opticianid, keycode, isprinted FROM

				w("Importing activation key " + counter++ + "of " + oldKeys.Count);

				int opticianID = 0;
				string keycode = node.childNodeValue("keycode");
				bool printed = (node.childNodeValue("isprinted") == "1");

				int.TryParse(node.childNodeValue("opticianid"), out opticianID);


				var optician = allOpticians.Where(n => n.OldDatabaseID == opticianID).FirstOrDefault();
				if (optician != null) {
					tye.Data.ActivationCode code = new tye.Data.ActivationCode();
					code.ID = 0;
					code.OpticianUserID = optician.ID;
					code.Printed = printed;
					code.Code = keycode;
					code.ActivationDate = null;
					code.ExpirationDate = null;

					//var oldUser = oldClientData.Where(n => n.Contains("#|#" + keycode + "#|#")).FirstOrDefault();
					XmlNode oldUser = docClients.SelectSingleNode("tye/users[password=\"" + keycode + "\"]");
					if (oldUser == null)
						continue;
					int iTryOldUserId = 0;
					//int.TryParse(oldUser.Split(new string[] { "#|#" }, StringSplitOptions.None)[0], out iTryOldUserId);
					int.TryParse(oldUser.childNodeValue("id"), out iTryOldUserId);

					DateTime dtExpiration = DateTime.MinValue;
					XmlNode expireNode = docKeys.SelectSingleNode("tye/log_endtime[clientid=" + iTryOldUserId + "]");
					if (expireNode != null && DateTime.TryParse(expireNode.childNodeValue("addedtime"), out dtExpiration)) {
						code.ActivationDate = dtExpiration;
					}
					if (expireNode != null && DateTime.TryParse(expireNode.childNodeValue("endtime"), out dtExpiration)) {
						code.ExpirationDate = dtExpiration;
					}

					var newButOldUser /* giggles*/ = ipa.UserSearch("OldDatabaseID == @0", new object[] { iTryOldUserId }.ToList());
					if (newButOldUser != null && newButOldUser.Count > 0) {
						code.ClientUserID = newButOldUser.First().ID;
					} else {
						w("didn't find user for password: " + keycode);
					}

					ipa.ActivationCodeSave(code);
				} else {
					// error
					string s = "";
				}
			}

		}
	}

	private class actOpt {
		public string name { get; set; }
		public int count { get; set; }
		
	}

	public void ImportUNUSEDActivationCodes(/*string FilePath*/) {
		string FilePath = filepath("tye-6.xml"); // @"C:\temp\GL - tye.dk - databasebackup\tye-2-log-1.xml";
		if (!File.Exists(FilePath))
			return;

		using (var ipa = statics.GetApi()) {
			//XmlDocument docKeys = new XmlDocument();
			//docKeys.Load(FilePath);
			//XmlNodeList oldKeys = docKeys.SelectNodes("tye/log_keys");
			//int oldkeyscount = oldKeys.Count;

			//XmlDocument docClients = new XmlDocument();
			//docClients.Load(filepath("tye-8.xml"));

			//XmlDocument docOpticianKeys = new XmlDocument();
			//docOpticianKeys.Load(filepath("tye-6.xml"));

			XmlDocument docKeys = new XmlDocument();
			docKeys.Load(FilePath);
			XmlNodeList oldKeys = docKeys.SelectNodes("tye/optician_keys");
			int oldkeyscount = oldKeys.Count;

			var allOpticians = ipa.UserGetCollection().Where(n => n.Type == tye.Data.User.UserType.Optician);
			int counter = 0;
			int newkeys = 0;

			Dictionary<string, int> dicKeyCount = new Dictionary<string, int>();

			foreach (XmlNode node in oldKeys) {
				//id, opticianid, keycode, isprinted FROM
				counter++;

				//if(counter % 50 == 0)
				//   w("Importing activation key " + counter++ + " of " + oldkeyscount);

				int opticianID = 0;
				string keycode = node.childNodeValue("password");
				//bool printed = (node.childNodeValue("isprinted") == "1");

				//var ocNode = docOpticianKeys.SelectSingleNode("optician_keys[password=\"" + keycode + "\"]");
				//if (ocNode != null) {
				//   w("skipping...");
				//}
				bool active = (node.childNodeValue("isactive") == "1");
				string q1 = (node.childNodeValue("isactive"));
				if (!active)
					continue;

				int.TryParse(node.childNodeValue("opticianid"), out opticianID);

				var optician = allOpticians.Where(n => n.OldDatabaseID == opticianID).FirstOrDefault();
				if (optician != null) {
					//var oldUser = oldClientData.Where(n => n.Contains("#|#" + keycode + "#|#")).FirstOrDefault();
					//XmlNode oldUser = docClients.SelectSingleNode("tye/users[password=\"" + keycode + "\"]");
					//if (oldUser != null)
					//   continue;

					tye.Data.ActivationCode code = new tye.Data.ActivationCode();
					code.ID = 0;
					code.OpticianUserID = optician.ID;
					code.Printed = true;
					code.Code = keycode;
					code.ActivationDate = null;
					code.ExpirationDate = null;
					code.ClientUserID = null;

					newkeys++;

					string key = (string.IsNullOrEmpty(optician.FullName) ? optician.ID.ToString() : optician.FullName);

					if (dicKeyCount.ContainsKey(key))
						dicKeyCount[key] += 1;
					else
						dicKeyCount.Add(key, 1);

					w("creating new keycode for optician " + optician.ID + ": " + keycode);

					//ipa.ActivationCodeSave(code);
				} else {
					// error
					string s = "";
				}
			}

			w("total forgotten keys: " + newkeys);

			List<actOpt> obs = new List<actOpt>();
			foreach (var k in dicKeyCount.Keys) {
				//w("old optician  " + k + " has " + dicKeyCount[k] + " unused codes.");
				obs.Add(new actOpt  { count = dicKeyCount[k], name = k });
			}

			foreach (actOpt o in obs.OrderByDescending(n => n.count).ThenBy(n => n.name)) {
				
				w(o.count + " unused codes for optician " + o.name);

			}
		}
	}
	/*
	/// <summary>
	/// Moved to "importActivationCodes"
	/// </summary>
	/// <param name="FilePath"></param>
	public void ImportActivationCodeClientRelations(string FilePath) {
		if (!File.Exists(FilePath))
			return;

		using (var ipa = statics.GetApi()) {
			string[] lines = File.ReadAllLines(FilePath);
			var allClients = ipa.UserGetCollection().Where(n => n.Type == tye.Data.User.UserType.Client);

			foreach (var linedata in lines) {
				var line = linedata.Split(new string[] { "#|#" }, StringSplitOptions.None);

				//password, addedtime, userid, enddate

				int userid = 0;
				string keycode = "";
				DateTime activated = new DateTime(1900, 1, 1);
				DateTime expire = new DateTime(1900, 1, 1);

				keycode = line[0];
				DateTime.TryParse(line[1], out activated);
				DateTime.TryParse(line[3], out expire);
				int.TryParse(line[2], out userid);

				var user = allClients.Where(n => n.OldDatabaseID == userid).FirstOrDefault();
				var code = ipa.ActivationCodeGetSingle(keycode);
				if (user != null && code != null && activated != null && expire != null) {
					code.ActivationDate = activated;
					code.ExpirationDate = expire;
					ipa.ActivationCodeSave(code);
				} else {
					// error
					string s = "";
				}
			}

		}
	}
	*/


	public void ImportEyeTests() {
		string FilePath = filepath("tye-6.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-6.xml";
		using (var ipa = statics.GetApi()) {

			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			//XmlNode top = doc.ChildNodes[1];
			List<string> lstAddedEyeTests = new List<string>();

			Dictionary<string, string> dicLevel = new Dictionary<string, string>(); ;
			dicLevel.Add("Grundlevel F", "vestibularTraining");
			dicLevel.Add("Grundlevel A", "reflexTraining");
			dicLevel.Add("Grundlevel H", "pokeGame");
			dicLevel.Add("Grundlevel E", "balanceBoard");
			dicLevel.Add("Ekstralevel F", "sunStar");
			dicLevel.Add("Ekstralevel E", "clapStomp");
			dicLevel.Add("Grundlevel B", "coordinationTraining");
			dicLevel.Add("Grundlevel C", "anglesInTheSnow");
			dicLevel.Add("Grundlevel I", "swimOnFloor");
			dicLevel.Add("Grundlevel J", "jumpRope");
			dicLevel.Add("Ekstralevel D", "racer");
			dicLevel.Add("Visualisering 1", "stevesCardGame");
			dicLevel.Add("Grundlevel D", "thumb");
			dicLevel.Add("Grundlevel K", "eyeHandCoordination");
			dicLevel.Add("Grundlevel G", "letterBall");
			dicLevel.Add("Ekstralevel A", "labyrinth");
			dicLevel.Add("Level 1A", "followTheEye");
			dicLevel.Add("Level 1B", "followMovements");
			dicLevel.Add("Ekstralevel C", "letterHunt");
			dicLevel.Add("Level 2", "pokeTest");
			dicLevel.Add("Ekstralevel L", "ninjaTraining");
			dicLevel.Add("Ekstralevel B", "columnJump");
			dicLevel.Add("3-D Level 0", "3dBalls");
			dicLevel.Add("3-D Level 1", "flower");
			dicLevel.Add("Level 3", "brocksLine");
			dicLevel.Add("Level 4", "larva");
			dicLevel.Add("3-D Level 2", "star");
			dicLevel.Add("Level 5", "snakesPositive");
			dicLevel.Add("3-D Level 3", "findTheNumbers");
			dicLevel.Add("3-D Level 4", "findTheFigure");
			dicLevel.Add("3-D Level 5", "jumpFixation");
			dicLevel.Add("Level 6", "snakesNegative");
			dicLevel.Add("3-D Level 6", "flowerNegative");
			dicLevel.Add("3-D Level 7", "flowerPosNeg");
			dicLevel.Add("Level 8", "letterFocus");
			dicLevel.Add("Level 7", "flipGlasses");
			dicLevel.Add("Ekstralevel G", "eyeRotation");
			dicLevel.Add("Ekstralevel H", "bucket");
			dicLevel.Add("Ekstralevel I", "apertureRulePositive");
			dicLevel.Add("Ekstralevel J", "apertureRuleNegative");
			dicLevel.Add("Ekstralevel K", "threeEyes");
			dicLevel.Add("Visualisering 2", "harrysBlocks");

			Dictionary<string, bool> dicHighscore = new Dictionary<string, bool>(); ;
			dicHighscore.Add("Ekstralevel F", false); //"sunStar");
			dicHighscore.Add("Ekstralevel E", false); //"clapStomp");
			dicHighscore.Add("Ekstralevel D", true); //"racer");
			dicHighscore.Add("Ekstralevel A", false); //"labyrinth");
			dicHighscore.Add("Level 1A", false); //"followTheEye");
			dicHighscore.Add("Ekstralevel C", true); //"letterHunt");
			dicHighscore.Add("Ekstralevel B", true); //"columnJump");
			dicHighscore.Add("3-D Level 0", false); //"3dBalls");
			dicHighscore.Add("3-D Level 1", true); //"flower");
			dicHighscore.Add("3-D Level 2", true); //"star");
			dicHighscore.Add("3-D Level 3", true); //"findTheNumbers");
			dicHighscore.Add("3-D Level 4", true); //"findTheFigure");
			dicHighscore.Add("3-D Level 5", true); //"jumpFixation");
			dicHighscore.Add("3-D Level 6", true); //"flowerNegative");
			dicHighscore.Add("3-D Level 7", true); //"flowerPosNeg");
			dicHighscore.Add("Ekstralevel H", false); //"bucket");

			foreach (XmlNode node in doc.SelectNodes("tye/tests")) {
				int iTry = 0;

				tye.Data.EyeTest test = new tye.Data.EyeTest() { ID = 0 };
				test.Name = node.ChildNodes[3].InnerText; //name
				test.OldBbName = node.ChildNodes[5].InnerText; //bbname


				if (test.OldBbName == "Visualization 1" || test.OldBbName == "Visualisierung 1")
					continue;

				if (lstAddedEyeTests.Contains(test.OldBbName))
					continue;
				else
					lstAddedEyeTests.Add(test.OldBbName);

				if (int.TryParse(node.ChildNodes[10].InnerText, out iTry) && iTry > 0)
					test.ScoreRequired = iTry;

				if (int.TryParse(node.ChildNodes[1].InnerText, out iTry) && iTry > 0)
					test.Priority = iTry;

				if (dicLevel.ContainsKey(test.OldBbName))
					test.InternalName = dicLevel[test.OldBbName];

				if (dicHighscore.ContainsKey(test.OldBbName))
					test.HighscoreApplicable = dicHighscore[test.OldBbName];
				else
					test.HighscoreApplicable = false;

				test.ScreenTest = (node.ChildNodes[9].InnerText == "1");

				ipa.EyeTestSave(test);
			}

		}
	}

	/// <summary>
	/// SELECT testid, tests_steps.priority, body, bbname, languageid, purpose, intro, important FROM `tests_steps` inner join `tests` on `tests`.id = tests_steps.testid
	/// </summary>
	/// <param name="FilePath"></param>
	public void ImportEyeTestInfos(/*string FilePath*/) {
		string FilePath = filepath("tye-6.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-6.xml";
		using (var ipa = statics.GetApi()) {

			var langs = ipa.LanguageGetCollection();
			var allEyeTests = ipa.EyeTestGetCollection();

			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			List<string> lstAddedEyeTests = new List<string>();

			foreach (XmlNode node in doc.SelectNodes("tye/tests")) {
				int iTry = 0;

				int iTryOldTestId = 0;
				int.TryParse(node.childNodeValue("id"), out iTryOldTestId);

				string bbname = node.childNodeValue("bbname");
				string body = node.childNodeValue("body");
				string purpose = node.childNodeValue("purpose");
				string intro = node.childNodeValue("intro");
				string important = node.childNodeValue("important");

				if (bbname == "Visualization 1" || bbname == "Visualisierung 1")
					bbname = "Visualisering 1";

				int iTryLanguage = 0;
				int iTryPriority = 0;

				int.TryParse(node.childNodeValue("languageid"), out iTryLanguage);
				int.TryParse(node.childNodeValue("priority"), out iTryPriority);

				var eyetest = allEyeTests.Where(n => n.OldBbName == bbname).FirstOrDefault();

				if (eyetest == null)
					continue;

				if (iTryPriority == 1) {
					// there are more texts with priority 1
					tye.Data.EyeTestInfo testPurpose = new tye.Data.EyeTestInfo() { ID = 0 };
					testPurpose.InfoText = purpose;
					testPurpose.InfoType = "Purpose";
					testPurpose.Priority = 1;
					testPurpose.EyeTestID = eyetest.ID;
					testPurpose.LanguageID = iTryLanguage;
					ipa.EyeTestInfoSave(testPurpose);

					tye.Data.EyeTestInfo testIntro = new tye.Data.EyeTestInfo() { ID = 0 };
					testIntro.InfoText = intro;
					testIntro.InfoType = "Intro";
					testIntro.Priority = 1;
					testIntro.EyeTestID = eyetest.ID;
					testIntro.LanguageID = iTryLanguage;
					ipa.EyeTestInfoSave(testIntro);

					tye.Data.EyeTestInfo testImportant = new tye.Data.EyeTestInfo() { ID = 0 };
					testImportant.InfoText = important;
					testImportant.InfoType = "Important";
					testImportant.Priority = 1;
					testImportant.EyeTestID = eyetest.ID;
					testImportant.LanguageID = iTryLanguage;
					ipa.EyeTestInfoSave(testImportant);

				}

				foreach (XmlNode nodeInfo in doc.SelectNodes("tye/tests_steps[testid=" + iTryOldTestId + "]")) {
					tye.Data.EyeTestInfo test = new tye.Data.EyeTestInfo() { ID = 0 };
					int.TryParse(nodeInfo.childNodeValue("priority"), out iTryPriority);
					test.EyeTestID = eyetest.ID;
					test.InfoText = nodeInfo.childNodeValue("body");
					test.InfoType = "Step";
					test.Priority = iTryPriority;
					test.LanguageID = iTryLanguage;

					ipa.EyeTestInfoSave(test);
				}
			}
		}
	}


	/// <summary>
	/// SELECT testid, tests_steps.priority, body, bbname, languageid, purpose, intro, important FROM `tests_steps` inner join `tests` on `tests`.id = tests_steps.testid
	/// </summary>
	/// <param name="FilePath"></param>
	public void ImportEyeTestInfosNoSteps(/*string FilePath*/) {
		string FilePath = filepath("tye-6.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-6.xml";
		using (var ipa = statics.GetApi()) {

			var langs = ipa.LanguageGetCollection();
			var allEyeTests = ipa.EyeTestGetCollection();

			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			List<string> lstAddedEyeTests = new List<string>();
			List<int> lstAddedEyeTestIDs = new List<int>();

			foreach (XmlNode node in doc.SelectNodes("tye/tests")) {
				int iTryOldTestId = 0;
				int.TryParse(node.childNodeValue("id"), out iTryOldTestId);

				string bbname = node.childNodeValue("bbname");
				string body = node.childNodeValue("body");
				string purpose = node.childNodeValue("purpose");
				string intro = node.childNodeValue("intro");
				string important = node.childNodeValue("important");

				if (bbname == "Visualization 1" || bbname == "Visualisierung 1")
					bbname = "Visualisering 1";

				int iTryLanguage = 0;
				int iTryPriority = 0;

				int.TryParse(node.childNodeValue("languageid"), out iTryLanguage);
				int.TryParse(node.childNodeValue("priority"), out iTryPriority);

				var eyetest = allEyeTests.Where(n => n.OldBbName == bbname).FirstOrDefault();

				if (eyetest == null)
					continue;

				if (iTryLanguage != 4)
					continue;

				int uniqueid = eyetest.ID + iTryLanguage;

				if (lstAddedEyeTestIDs.Contains(uniqueid))
					continue;

				//if (iTryPriority == 1) {

					lstAddedEyeTestIDs.Add(uniqueid);

					// there are more texts with priority 1
					tye.Data.EyeTestInfo testPurpose = new tye.Data.EyeTestInfo() { ID = 0 };
					testPurpose.InfoText = purpose;
					testPurpose.InfoType = "Purpose";
					testPurpose.Priority = 1;
					testPurpose.EyeTestID = eyetest.ID;
					testPurpose.LanguageID = iTryLanguage;
					ipa.EyeTestInfoSave(testPurpose);
					w("Purpose for test " + testPurpose.EyeTestID + ", language: " + iTryLanguage);

					tye.Data.EyeTestInfo testIntro = new tye.Data.EyeTestInfo() { ID = 0 };
					testIntro.InfoText = intro;
					testIntro.InfoType = "Intro";
					testIntro.Priority = 1;
					testIntro.EyeTestID = eyetest.ID;
					testIntro.LanguageID = iTryLanguage;
					ipa.EyeTestInfoSave(testIntro);
					w("Intro for test " + testPurpose.EyeTestID + ", language: " + iTryLanguage);

					tye.Data.EyeTestInfo testImportant = new tye.Data.EyeTestInfo() { ID = 0 };
					testImportant.InfoText = important;
					testImportant.InfoType = "Important";
					testImportant.Priority = 1;
					testImportant.EyeTestID = eyetest.ID;
					testImportant.LanguageID = iTryLanguage;
					ipa.EyeTestInfoSave(testImportant);
					w("Important for test " + testPurpose.EyeTestID + ", language: " + iTryLanguage);

				//}

			}
		}
	}


	public void ImportEyeTestNames(/*string FilePath*/) {
		string FilePath = filepath("tye-6.xml");// @"C:\temp\GL - tye.dk - databasebackup\tye-6.xml";
		using (var ipa = statics.GetApi()) {
			//string[] lines = File.ReadAllLines(FilePath);

			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			List<string> lstAddedEyeTests = new List<string>();

			var eyetests = ipa.EyeTestGetCollection();

			foreach (XmlNode node in doc.SelectNodes("tye/tests")) {
				int iTry = 0;
				int iTryLanguage = 0;
				string bbname = node.SelectSingleNode("bbname").InnerText;
				if (bbname == "Visualization 1" || bbname == "Visualisierung 1")
					bbname = "Visualisering 1";

				var eyetest = eyetests.Where(n => n.OldBbName == bbname).FirstOrDefault();
				if (eyetest == null) {
					throw new Exception("øv!");
				}
				int.TryParse(node.SelectSingleNode("languageid").InnerText, out iTryLanguage);

				tye.Data.EyeTestInfo testPurpose = new tye.Data.EyeTestInfo() { ID = 0 };
				testPurpose.InfoText = node.SelectSingleNode("name").InnerText;
				testPurpose.InfoType = "Name";
				testPurpose.Priority = 0;
				testPurpose.EyeTestID = eyetest.ID;
				testPurpose.LanguageID = iTryLanguage;
				ipa.EyeTestInfoSave(testPurpose);

			}

		}
	}

	// SELECT * FROM `test_schedule` WHERE `addedtime` > '2011-01-01 00:00:00' ORDER BY `id` DESC
	// SELECT * FROM `test_schedule_tests` 
	public void ImportPrograms() {
		string FilePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/importdata/test_schedule.xml");
		string FilePath1 = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/importdata/test_schedule_tests.xml");
		string FilePath2 = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/importdata/eyetest/eyetests.xml");

		string Path_test_schedule = filepath("tye-6.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-6.xml";
		string Path_test_schedule_tests = filepath("tye-7-test_schedule_tests.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-7-test_schedule_tests.xml";
		//string Path_eyetest =  @"C:\temp\GL - tye.dk - databasebackup\tye-6.xml";

		using (var ipa = statics.GetApi()) {
			var programs = ipa.ProgramGetCollection();

			foreach (var program in programs) {
				ipa.ProgramDeletePermanently(program.ID);
			}

			var eyetests = ipa.EyeTestGetCollection();
			var allUsers = ipa.UserGetCollection().Where(n => n.Type == tye.Data.User.UserType.Client).ToList();

			XmlDocument programDoc = new XmlDocument();
			programDoc.Load(Path_test_schedule);
			//XmlNode programTop = programDoc.ChildNodes[1];

			XmlDocument testsDoc = new XmlDocument();
			testsDoc.Load(Path_test_schedule_tests);
			//XmlNode testsTop = testsDoc.ChildNodes[1];

			// get only only schedule (and the higheste one) for each client.
			Dictionary<int, XmlNode> nodeDic = new Dictionary<int, XmlNode>();
			foreach (XmlNode node in programDoc.SelectNodes("tye/test_schedule")) { //[clientid=7629]")) {
				int iTryClientID = 0;
				if (!int.TryParse(node.childNodeValue("clientid"), out iTryClientID))
					continue;
				if (!nodeDic.ContainsKey(iTryClientID))
					nodeDic.Add(iTryClientID, node);
				else {

					int iTrySaveScheduleID = 0;
					int.TryParse(nodeDic[iTryClientID].childNodeValue("id"), out iTrySaveScheduleID);

					int iTryThisScheduleID = 0;
					int.TryParse(node.childNodeValue("id"), out iTryThisScheduleID);

					if (iTryThisScheduleID > iTrySaveScheduleID)
						nodeDic[iTryClientID] = node;
				}
			}

			int testschedulecount = nodeDic.Count;
			int childNodeCounter = 1;

			List<int> lstTempUserId = new List<int>();
			foreach (int key in nodeDic.Keys) {

				XmlNode node = nodeDic[key];

				//foreach (XmlNode node in programDoc.SelectNodes("tye/test_schedule[clientid=7629]")) {
				//cont.Response.Write(childNodeCounter++ + "/" + testschedulecount + "<br />");
				//cont.Response.Flush();
				if (childNodeCounter++ % 50 == 0)
					w("Program " + childNodeCounter + "/" + testschedulecount);


				int iTryOldProgramId = 0;
				int iTryUserID = 0;
				int.TryParse(node.childNodeValue("clientid"), out iTryUserID);
				int.TryParse(node.childNodeValue("id"), out iTryOldProgramId);

				if (lstTempUserId.Contains(iTryUserID)) {
					continue;
				} else {
					lstTempUserId.Add(iTryUserID);
				}

				var user = allUsers.FirstOrDefault(n => n.OldDatabaseID == iTryUserID);
				if (user == null) {
					// actually some users are missing. Clients belonging to deleted opticians don't exist.
					continue;
					//throw new Exception("øv!");
				}

				tye.Data.Program prog = new tye.Data.Program() { ID = 0 };
				prog.Comment = node.childNodeValue("guide");
				if (!String.IsNullOrEmpty(prog.Comment))
					prog.Comment += ("&lt;br /&gt;");
				prog.Comment += node.childNodeValue("comments");
				prog.ClientUserID = user.ID;

				var newProgramId = ipa.ProgramSave(prog).ID;

				// and now for the related exercises

				List<string> locked = new List<string>();
				/*
				List<XmlNode> realTestNodes = new List<XmlNode>();
				Dictionary<int, XmlNode> dic = new Dictionary<int,XmlNode>();

				var clientNodes = testsDoc.SelectNodes("/tye/test_schedule_tests[clientid=" + iTryUserID + "]");
				foreach (XmlNode cnode in clientNodes) {
					int itrysheduleid = 0;
					if (int.TryParse(cnode.childNodeValue("scheduleid"), out itrysheduleid)) {
						dic.Add(itrysheduleid, cnode);
					}
				}

				if (dic.Count == 0)
					continue;

				int intScheduleID = 0;
				foreach (var key in dic.Keys) {
					if (key > intScheduleID)
						intScheduleID = key;
				}*/
				//iTryOldProgramId
				var testNodes = testsDoc.SelectNodes("/tye/test_schedule_tests[scheduleid=" + iTryOldProgramId + "]");
				foreach (XmlNode testNode in testNodes) {
					int iTryTestId = 0;
					int.TryParse(testNode.childNodeValue("testid"), out iTryTestId);

					XmlNode orgTestNode = programDoc.SelectSingleNode("/tye/tests[id=" + iTryTestId + "]");
					if (orgTestNode != null) {
						string bbname = orgTestNode.childNodeValue("bbname");
						if (bbname == "Visualization 1" || bbname == "Visualisierung 1")
							bbname = "Visualisering 1";

						var eyeTest = eyetests.FirstOrDefault(n => n.OldBbName == bbname);
						bool islocked = (testNode.childNodeValue("isLocked") == "0");

						// isLocked == 0 = "Programlås"
						// isLocked == 1 = "Ingen lås" ??!??!?!??!??!??!
						// isLocked == -1 = LockedByOptician
						if (eyeTest != null) {
							//if (islocked)
							//   locked.Add(eyeTest.Name);
                            ipa.ProgramEyeTestSave(newProgramId, eyeTest.ID, islocked, eyeTest.Priority, (testNode.childNodeValue("isLocked") == "-1"));
						}
					}
				}

			}
		}

	}

	/// <summary>
	/// Old values: 0 = "Ikke angivet", 1 = Ja, 2 = Nej, 3 = Ved ikke.
	/// </summary>
	public void ImportAnamnese() {
		string FilePath = filepath("tye-1.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-1.xml";

		if (!File.Exists(FilePath))
			return;

		using (var ipa = statics.GetApi()) {
			var clients = ipa.UserGetCollection();

			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			XmlNodeList nodes = doc.SelectNodes("tye/a_anamnese");
			int total = nodes.Count;
			int counter = 1;

			foreach (XmlNode node in nodes) {

				int oldUserId = 0;
				int.TryParse(node.childNodeValue("clientid"), out oldUserId);

				DateTime dtAdded = DateTime.Now;
				DateTime.TryParse(node.childNodeValue("addedtime"), out dtAdded);

				double DailyCloseRangeWork = 0.0;
				double.TryParse(node.childNodeValue("a_20"), out DailyCloseRangeWork); // sidste værdi

				double MaxReadingHours = 0.0;
				double.TryParse(node.childNodeValue("a_19"), out MaxReadingHours); // næstsidste værdi

				// første anamnese-værdi er i kolonne 4 (indeks)

				int start = 4;
				int stop = 18;

				var user = clients.FirstOrDefault(n => n.OldDatabaseID == oldUserId);
				if (user == null)
					continue;

				tye.Data.Anamnese nam = new tye.Data.Anamnese();
				nam.ID = 0;
				nam.ClientUserID = user.ID;
				nam.Comments = "";
				nam.Created = dtAdded;
				nam.DailyCloseRangeWork = Convert.ToInt32(Math.Ceiling(DailyCloseRangeWork));
				nam.Injuries = "";
				nam.MaxReadingHours = Convert.ToInt32(Math.Ceiling(MaxReadingHours));
				nam.Medication = "";
				nam.Sicknesses = "";

				for (int i = start; i <= stop; i++) {
					//Old values: 0 = "Ikke angivet", 1 = Ja, 2 = Nej, 3 = Ved ikke.
					int value = 5; // 5 = ved ikke
					string nodeName = "a_" + i;
					string nodeValue = node.childNodeValue(nodeName);
					switch (nodeValue) {
						case "1":
							value = 2;
							break;
						case "2":
							value = 0;
							break;
					}
					nam.GetType().GetProperty("Q" + (i - start + 1)).SetValue(nam, value, null);
				}

				if (counter++ % 50 == 0)
					w("Anamnese " + counter + "/" + total);

				ipa.AnamneseSave(nam);

			}


		}

	}

	public void ImportEquipment() {
		string FilePath = filepath("tye-6.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-6.xml";

		XmlDocument doc = new XmlDocument();
		doc.Load(FilePath);

		using (var ipa = statics.GetApi()) {
			List<string> lstAddedEyeTests = new List<string>();

			//var eyetests = ipa.EyeTestGetCollection();
			var langs = ipa.LanguageGetCollection();
			int counter = 1;

			foreach (var rq in ipa.EquipmentGetCollection())
				ipa.EquipmentDelete(rq.ID);

			XmlNodeList nodes = doc.SelectNodes("tye/newequipment");
			foreach (XmlNode node in nodes) {
				int oldid = 0;

				w("equipment " + counter++ + " of " + nodes.Count);

				int.TryParse(node.SelectSingleNode("eID").InnerText, out oldid);

				string nameDk = node.childNodeValue("eName_DK");
				string nameDe = node.childNodeValue("eName_DE");
				string nameUk = node.childNodeValue("eName_UK");
				string nameNo = node.childNodeValue("eName_NO");

				string descriptionDk = node.childNodeValue("eDescription_DK");
				string descriptionDe = node.childNodeValue("eDescription_DE");
				string descriptionUk = node.childNodeValue("eDescription_UK");
				string descriptionNo = node.childNodeValue("eDescription_NO");

				tye.Data.Equipment quip = new tye.Data.Equipment() { ID = 0 };
				quip.Active = true;
				quip.ID = 0;
				quip.Name = nameDk;
				quip.Picture = "";
				quip = ipa.EquipmentSave(quip);

				tye.Data.EquipmentInfo info = new tye.Data.EquipmentInfo() { ID = 0 };
				info.Description = descriptionDk;
				info.EquipmentID = quip.ID;
				info.LanguageID = 1;
				info.Name = nameDk;
				ipa.EquipmentInfoSave(info);

				tye.Data.EquipmentInfo infoDe = new tye.Data.EquipmentInfo() { ID = 0 };
				infoDe.Description = descriptionDe;
				infoDe.EquipmentID = quip.ID;
				infoDe.LanguageID = 4;
				infoDe.Name = nameDe;
				ipa.EquipmentInfoSave(infoDe);

				tye.Data.EquipmentInfo infoUk = new tye.Data.EquipmentInfo() { ID = 0 };
				infoUk.Description = descriptionUk;
				infoUk.EquipmentID = quip.ID;
				infoUk.LanguageID = 3;
				infoUk.Name = nameUk;
				ipa.EquipmentInfoSave(infoUk);

				tye.Data.EquipmentInfo infoNo = new tye.Data.EquipmentInfo() { ID = 0 };
				infoNo.Description = descriptionNo;
				infoNo.EquipmentID = quip.ID;
				infoNo.LanguageID = 2;
				infoNo.Name = nameNo;
				ipa.EquipmentInfoSave(infoNo);


				ImportEquipmentItem(oldid, quip.ID, doc);
			}
		}
	}

	public void ImportEquipmentItem(int OldID, int NewId, XmlDocument doc) {
		using (var ipa = statics.GetApi()) {
			foreach (XmlNode node in doc.SelectNodes("/tye/newequipmentitem[eiEquipment=" + OldID + "]")) {
				if (node == null)
					return;

				string descriptionDk = node.childNodeValue("eiDescription_DK");
				string descriptionDe = node.childNodeValue("eiDescription_DE");
				string descriptionUk = node.childNodeValue("eiDescription_UK");
				string descriptionNo = node.childNodeValue("eiDescription_NO");

				double dblDk = 0;
				double dblDe = 0;
				double dblUk = 0;
				double dblNo = 0;

				double.TryParse(node.childNodeValue("eiPrice_DK"), out dblDk);
				double.TryParse(node.childNodeValue("eiPrice_DE"), out dblDe);
				double.TryParse(node.childNodeValue("eiPrice_UK"), out dblUk);
				double.TryParse(node.childNodeValue("eiPrice_NO"), out dblNo);

				// UK prices in euro like DE
				dblUk = dblDe;

				tye.Data.EquipmentItem quip = new tye.Data.EquipmentItem() { ID = 0 };
				quip.Active = true;
				quip.ID = 0;
				quip.EquipmentID = NewId;
				quip = ipa.EquipmentItemSave(quip);

				tye.Data.EquipmentItemInfo info = new tye.Data.EquipmentItemInfo() { ID = 0 };
				info.Description = RemoveUnwantedTags(descriptionDk);
				info.EquipmentItemID = quip.ID;
				info.LanguageID = 1;
				info.Price = dblDk;
				ipa.EquipmentItemInfoSave(info);

				tye.Data.EquipmentItemInfo infoDe = new tye.Data.EquipmentItemInfo() { ID = 0 };
				infoDe.Description = RemoveUnwantedTags(descriptionDe);
				infoDe.EquipmentItemID = quip.ID;
				infoDe.LanguageID = 4;
				infoDe.Price = dblDe;
				ipa.EquipmentItemInfoSave(infoDe);

				tye.Data.EquipmentItemInfo infoUk = new tye.Data.EquipmentItemInfo() { ID = 0 };
				infoUk.Description = RemoveUnwantedTags(descriptionUk);
				infoUk.EquipmentItemID = quip.ID;
				infoUk.LanguageID = 3;
				infoUk.Price = dblUk;
				ipa.EquipmentItemInfoSave(infoUk);

				tye.Data.EquipmentItemInfo infoNo = new tye.Data.EquipmentItemInfo() { ID = 0 };
				infoNo.Description = RemoveUnwantedTags(descriptionNo);
				infoNo.EquipmentItemID = quip.ID;
				infoNo.LanguageID = 2;
				infoNo.Price = dblNo;
				ipa.EquipmentItemInfoSave(infoNo);
			}
		}
	}

	private Dictionary<int, string> oldTestIdsAndNames() {
		Dictionary<int, string> dic = new Dictionary<int, string>();

		using (var ipa = statics.GetApi()) {
			XmlDocument doc = new XmlDocument();
			doc.Load(HostingEnvironment.MapPath("~/App_Data/importdata/eyetest/eyetests.xml"));

			XmlNode top = doc.ChildNodes[1];
			List<string> lstAddedEyeTests = new List<string>();

			foreach (XmlNode node in top.ChildNodes) {
				int iTry = 0;
				int.TryParse(node.ChildNodes[0].InnerText, out iTry);

				/*
				tye.Data.EyeTest test = new tye.Data.EyeTest() { ID = 0 };
				test.Name = node.ChildNodes[3].InnerText; //name
				test.OldBbName = node.ChildNodes[5].InnerText; //bbname
				*/
				if (!dic.ContainsKey(iTry))
					dic.Add(iTry, node.ChildNodes[5].InnerText);
			}

		}
		return dic;
	}

	public void TestClientLogFilter() {
		string FilePath = filepath("log_testresult.xml"); //@"C:\temp\GL - tye.dk - databasebackup\log_testresult.xml";
		if (!File.Exists(FilePath))
			return;

		using (var ipa = statics.GetApi()) {
			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			var clients = ipa.UserGetCollection();
			//var programs = ipa.ProgramGetCollection();
			var eyetests = ipa.EyeTestGetCollection();
			var dic = oldTestIdsAndNames();

			XmlNodeList nodes = doc.SelectNodes("tye/log_testresult"); //[programid=11267]");
			int total = nodes.Count;
			int counter = 1;
			int foundcount = 1;

			foreach (XmlNode node in nodes) {
				counter++;

				// <props>
				DateTime dtAdded = DateTime.Now;
				if (!DateTime.TryParse(node.childNodeValue("addedtime"), out dtAdded))
					continue;

				bool DateOK = (dtAdded >= new DateTime(2013, 1, 1) && dtAdded < new DateTime(2014, 1, 1));

				//w(counter + "...skipping");
				if (!DateOK)
					continue;

				w(counter + " - " + foundcount++ + "...going");
				
			}
		}
	}

	public void ImportClientEyeTestLog() {
		string FilePath = filepath("log_testresult.xml"); //@"C:\temp\GL - tye.dk - databasebackup\log_testresult.xml";
		if (!File.Exists(FilePath))
			return;

		using (var ipa = statics.GetApi()) {
			XmlDocument doc = new XmlDocument();
			doc.Load(FilePath);

			var clients = ipa.UserGetCollection();
			//var programs = ipa.ProgramGetCollection();
			var eyetests = ipa.EyeTestGetCollection();
			var dic = oldTestIdsAndNames();

			XmlNodeList nodes = doc.SelectNodes("tye/log_testresult"); //[programid=11267]");
			int total = nodes.Count;
			int counter = 1;

			foreach (XmlNode node in nodes) {
				counter++;

				// <props>
				DateTime dtAdded = DateTime.Now;
				if (!DateTime.TryParse(node.childNodeValue("addedtime"), out dtAdded))
					continue;

				bool DateOK = (dtAdded.Date >= new DateTime(2012, 1, 1) && dtAdded.Date < new DateTime(2013, 1, 1));
				if (!DateOK)
					continue;

				int oldUserID = 0;
				int oldEyeTestID = 0;
				int oldProgramID = 0;
				int seconds = 0;
				int score = 0;
				bool highscore = false;
				string attname = "";
				string attvalue = "";

				
				int.TryParse(node.childNodeValue("clientid"), out oldUserID);
				int.TryParse(node.childNodeValue("testid"), out oldEyeTestID);
				int.TryParse(node.childNodeValue("programid"), out oldProgramID);
				int.TryParse(node.childNodeValue("seconds").Replace(".", ","), out seconds);
				int.TryParse(node.childNodeValue("score").Replace(".", ","), out score);
				highscore = (node.childNodeValue("highscore") == "1");
				attname = node.childNodeValue("attname");
				attvalue = node.childNodeValue("attvalue");

				string s = "";
				// </props>

				if (counter % 100 == 0)
					w("Log " + counter + "/" + total);

				var user = clients.FirstOrDefault(n => n.OldDatabaseID == oldUserID);
				if (user == null)
					continue;

				var prog = ipa.ProgramGetSingleByUserID(user.ID);
				if (prog == null)
					continue;


				var eyetestInternalName = "";
				if (dic.ContainsKey(oldEyeTestID))
					eyetestInternalName = dic[oldEyeTestID];
				var eyetest = eyetests.FirstOrDefault(n => n.OldBbName == eyetestInternalName);

				if (eyetest == null)
					continue;

				var programEyeTest = prog.ProgramEyeTests.FirstOrDefault(n => n.EyeTestID == eyetest.ID);
				if (programEyeTest == null)
					continue;


				var log = new tye.Data.ClientEyeTestLog() { ID = 0 };
				log.AttribName = attname;
				log.AttribValue = attvalue;
				log.Comment = "";
				log.EndTime = dtAdded.AddSeconds(seconds);
				log.HighScore = highscore;
				log.ProgramEyeTestID = programEyeTest.ID;
				log.Score = score;
				log.StartTime = dtAdded;
				log.UpdateToken = "";

				ipa.ClientEyeTestLogSave(log);

				//System.Diagnostics.Debug.WriteLine("Saving " + counter + "/" + total);
			}
		}
	}

	public void Import21pointTranslations() {
		string input = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/importdata/measuring_directions.xml");

		using (var ipa = statics.GetApi()) {
			XmlDocument doc = new XmlDocument();
			doc.Load(input);

			XmlNode top = doc.ChildNodes[1];

			//var eyetests = ipa.EyeTestGetCollection();
			var langs = ipa.LanguageGetCollection();
			int counter = 1;

			foreach (var entr in ipa.DictionaryGet().Entries) {
				if (entr.Key.StartsWith("21point_"))
					ipa.DictionaryEntryDelete(entr.ID);
			}

			foreach (XmlNode node in top.ChildNodes) {

				string tyeid = node.SelectSingleNode("tyeid").InnerText.Trim();
				string languageid = node.SelectSingleNode("languageid").InnerText;
				string body = node.SelectSingleNode("body").InnerText;

				int langid = 0;
				if (!int.TryParse(languageid, out langid))
					continue;

				string key = "21point_" + tyeid;
				var dic = ipa.DictionaryGet();
				tye.Data.DictionaryEntry entry = dic.Entries.FirstOrDefault(n => n.Key == key);
				if (entry == null) {
					entry = new tye.Data.DictionaryEntry();
					entry.ID = 0;
					entry.Key = key;
					entry.SystemEntry = true;
					entry = ipa.DictionaryEntrySave(entry);
				}

				var lang = langs.FirstOrDefault(n => n.ID == langid);
				if (lang == null)
					continue;

				entry.SetValue(lang, body);
				ipa.DictionaryEntrySave(entry);

			}
		}
	}

	public void ImportMotility() {
		string FilePath = filepath("tye-1.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-1.xml";

		XmlDocument doc = new XmlDocument();
		doc.Load(FilePath);


		XmlNodeList nodes = doc.SelectNodes("tye/a_motilitet");
		int total = nodes.Count;
		int counter = 0;

		using (var ipa = statics.GetApi()) {
			foreach (XmlNode node in nodes) {
				counter++;

				DateTime dt = DateTime.Now;

				DateTime.TryParse(node.childNodeValue("addedtime"), out dt);
				int userid = 0;
				int.TryParse(node.childNodeValue("clientid"), out userid);

				var users = ipa.UserSearch("OldDatabaseID = @0", new object[] { userid }.ToList());
				if (users.Count != 1)
					continue;
				
				if (counter % 100 == 0)
					w("Motility " + counter + " of " + total);

				tye.Data.MeasuringControl mc = new tye.Data.MeasuringControl();
				mc.ClientUserID = users.First().ID;
				mc.Created = dt;
				mc.ID = 0;
				mc.MotilityDidClientSway = 0;
				mc.MotilityHeadMovements = Convert.ToInt16(node.childNodeValue("m_4"));
				mc.MotilityHorizontalEyeMovements = 0;
				mc.MotilityPointsBothEyes = Convert.ToInt16(node.childNodeValue("m_3"));
				mc.MotilityPointsLeftEye = Convert.ToInt16(node.childNodeValue("m_2"));
				mc.MotilityPointsRightEye = Convert.ToInt16(node.childNodeValue("m_1"));
				mc.NoteMotilityDidClientSway = "";
				mc.NoteMotilityHeadMovements = node.childNodeValue("m_4_1");
				mc.NoteMotilityHorizontalEyeMovements = "";
				mc.NoteMotilityPointsBothEyes = node.childNodeValue("m_3_1");
				mc.NoteMotilityPointsLeftEye = node.childNodeValue("m_2_1");
				mc.NoteMotilityPointsRightEye = node.childNodeValue("m_1_1");
				mc.Step1Changes = "";
				mc.Step1Comments = "";
				mc.Convergence1 = 0;
				mc.Convergence2 = 0;
				mc.Convergence3 = 0;

				ipa.MeasuringControlSave(mc);
			}
		}

	}

	public void ImportConvergence() {
		string FilePath = filepath("tye-1.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-1.xml";

		XmlDocument doc = new XmlDocument();
		doc.Load(FilePath);

		XmlNodeList nodes = doc.SelectNodes("tye/a_convergence");
		List<int> useridshadfirst = new List<int>();
		int counter = 0;
		int total = nodes.Count;

		using (var ipa = statics.GetApi()) {
			foreach (XmlNode node in nodes) {
				counter++;

				DateTime dt = DateTime.Now;

				DateTime.TryParse(node.childNodeValue("addedtime"), out dt);
				int userid = 0;
				int.TryParse(node.childNodeValue("clientid"), out userid);

				var users = ipa.UserSearch("OldDatabaseID = @0", new object[] { userid }.ToList());
				if (users.Count != 1)
					continue;

				if(counter % 100 == 0)
					w("Convergence " + counter + " of " + total);

				var firstuser = users.First();

				// take first mc record
				tye.Data.MeasuringControl mc = new tye.Data.MeasuringControl();
				var controls = ipa.MeasuringControlGetCollection(users.First().ID);

				// if no records exist, create one

				if (!controls.Any()) {
					tye.Data.MeasuringControl mcw = new tye.Data.MeasuringControl();
					mc.ClientUserID = firstuser.ID;
					mc.Created = dt;
					mc.ID = 0;
					mc.MotilityDidClientSway = 0;
					mc.MotilityHeadMovements = 0;
					mc.MotilityHorizontalEyeMovements = 0;
					mc.MotilityPointsBothEyes = 0;
					mc.MotilityPointsLeftEye = 0;
					mc.MotilityPointsRightEye = 0;
					mc.NoteMotilityDidClientSway = "";
					mc.NoteMotilityHeadMovements = "";
					mc.NoteMotilityHorizontalEyeMovements = "";
					mc.NoteMotilityPointsBothEyes = "";
					mc.NoteMotilityPointsLeftEye = "";
					mc.NoteMotilityPointsRightEye = "";
					mc.Step1Changes = "";
					mc.Step1Comments = "";
					mc.Convergence1 = Convert.ToInt32(node.childNodeValue("c_1"));
					mc.Convergence2 = Convert.ToInt32(node.childNodeValue("c_2"));
					mc.Convergence3 = Convert.ToInt32(node.childNodeValue("c_3"));

					ipa.MeasuringControlSave(mc);
					useridshadfirst.Add(firstuser.ID);
				} else {
					if (useridshadfirst.Contains(firstuser.ID)) {
						mc = new tye.Data.MeasuringControl();
						mc.ClientUserID = firstuser.ID;
						mc.Created = dt;
						mc.ID = 0;
						mc.MotilityDidClientSway = 0;
						mc.MotilityHeadMovements = 0;
						mc.MotilityHorizontalEyeMovements = 0;
						mc.MotilityPointsBothEyes = 0;
						mc.MotilityPointsLeftEye = 0;
						mc.MotilityPointsRightEye = 0;
						mc.NoteMotilityDidClientSway = "";
						mc.NoteMotilityHeadMovements = "";
						mc.NoteMotilityHorizontalEyeMovements = "";
						mc.NoteMotilityPointsBothEyes = "";
						mc.NoteMotilityPointsLeftEye = "";
						mc.NoteMotilityPointsRightEye = "";
						mc.Step1Changes = "";
						mc.Step1Comments = "";
						mc.Convergence1 = Convert.ToInt32(node.childNodeValue("c_1"));
						mc.Convergence2 = Convert.ToInt32(node.childNodeValue("c_2"));
						mc.Convergence3 = Convert.ToInt32(node.childNodeValue("c_3"));

						ipa.MeasuringControlSave(mc);
						useridshadfirst.Add(firstuser.ID);
					} else {
						mc = controls.OrderBy(n => n.Created).FirstOrDefault();
						mc.Convergence1 = Convert.ToInt32(node.childNodeValue("c_1"));
						mc.Convergence2 = Convert.ToInt32(node.childNodeValue("c_2"));
						mc.Convergence3 = Convert.ToInt32(node.childNodeValue("c_3"));
						ipa.MeasuringControlSave(mc);
					}
				}


				//if (!useridshadfirst.Contains(firstuser.ID)) {
				//   tye.Data.MeasuringControl mc = new tye.Data.MeasuringControl();
				//   mc.ClientUserID = users.First().ID;
				//   mc.Created = dt;
				//   mc.ID = 0;
				//   mc.MotilityDidClientSway = 0;
				//   mc.MotilityHeadMovements = 0;
				//   mc.MotilityHorizontalEyeMovements = 0;
				//   mc.MotilityPointsBothEyes = 0;
				//   mc.MotilityPointsLeftEye = 0;
				//   mc.MotilityPointsRightEye = 0;
				//   mc.NoteMotilityDidClientSway = "";
				//   mc.NoteMotilityHeadMovements = "";
				//   mc.NoteMotilityHorizontalEyeMovements = "";
				//   mc.NoteMotilityPointsBothEyes = "";
				//   mc.NoteMotilityPointsLeftEye = "";
				//   mc.NoteMotilityPointsRightEye = "";
				//   mc.Step1Changes = "";
				//   mc.Step1Comments = "";
				//   mc.Convergence1 = Convert.ToInt32(node.childNodeValue("c_1"));
				//   mc.Convergence2 = Convert.ToInt32(node.childNodeValue("c_2"));
				//   mc.Convergence3 = Convert.ToInt32(node.childNodeValue("c_3"));

				//   ipa.MeasuringControlSave(mc);
				//   useridshadfirst.Add(firstuser.ID);
				//} else {
				//   tye.Data.MeasuringControl mc = new tye.Data.MeasuringControl();
				//   var controls = ipa.MeasuringControlGetCollection(users.First().ID);
				//   if (!controls.Any() && !useridshadfirst.Contains(users.First().ID))
				//      continue;
				//   mc = controls.OrderBy(n => n.Created).FirstOrDefault();
				//   mc.Convergence1 = Convert.ToInt32(node.childNodeValue("c_1"));
				//   mc.Convergence2 = Convert.ToInt32(node.childNodeValue("c_2"));
				//   mc.Convergence3 = Convert.ToInt32(node.childNodeValue("c_3"));
				//   ipa.MeasuringControlSave(mc);
				//}







			}
		}

	}

	public void Import21Points() {
		string FilePath = filepath("tye-9-21.xml"); //@"C:\temp\GL - tye.dk - databasebackup\tye-9-a21.xml";

		XmlDocument doc = new XmlDocument();
		doc.LoadXml(xmlWrongXml(FilePath));

		XmlNodeList nodes = doc.SelectNodes("tye/a_21");
		List<int> useridshadfirst = new List<int>();

		using (var ipa = statics.GetApi()) {
			foreach (XmlNode node in nodes) {
			} // foreach
		} // using

	}
}