using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Net;
using System.Xml;

/// <summary>
/// Summary description for jobLatLong
/// </summary>
public class jobLatLong : IJob {
	public jobLatLong() {
	}

	public void Execute(IJobExecutionContext context) {
		using (var ipa = statics.GetApi()) {
			var opts = ipa.UserGetCollection().Where(n => n.Enabled && n.Type == tye.Data.User.UserType.Optician);
			// http://maps.googleapis.com/maps/api/geocode/xml?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=true
			using (var client = new WebClient()) {
				foreach (var opt in opts) {
					try {
						string url = "http://maps.googleapis.com/maps/api/geocode/xml" +
					"?address=" +
					opt.Address + ",+" +
					opt.PostalCode + "+" +
					opt.City + ",+" +
					opt.Country.Name +
					"&sensor=false";

						var strXml = client.DownloadString(url);
						XmlDocument xml = new XmlDocument();
						xml.LoadXml(strXml);

						var top = xml.ChildNodes[1];
						if (top == null || !top.HasChildNodes)
							continue;

						if (top.FirstChild.InnerText != "OK")
							continue;

						var geo = top.SelectSingleNode("result/geometry/location");
						if (geo == null)
							continue;

						string poslat = geo.ChildNodes[0].InnerText;
						string poslong = geo.ChildNodes[1].InnerText;

						opt.Lat = poslat;
						opt.Long = poslong;

						ipa.UserSave(opt);
						//litLocations.Text = "['" + intCounter + "', " + poslat + ", " + poslong + ", " + intCounter++ + "]," + litLocations.Text;
					} catch (Exception ex) {
						string s = ex.ToString();
					}
				} // foreach
			}
		}
	}
}