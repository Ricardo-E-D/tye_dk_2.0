using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Xml;
using tye.Data;

namespace umbraco_opticianmap {
	public partial class Default1 : System.Web.UI.Page {

		private string nodeVal(XmlNode node) {
			return "";
		}

		public tye.API.API GetApi() {
			string c = "DatabaseEntities";
			try {
				if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("localhost"))
					c += "Local";
			} catch (Exception) { }
			string strConn = "";

			strConn = @"metadata=res://*/db.dbModel.csdl|res://*/db.dbModel.ssdl|res://*/db.dbModel.msl;provider=System.Data.SqlClient;provider connection string='data source=.\SQLEXPRESS;attachdbfilename=E:\Development\clients\tye\tye_dk_2.0\App_Data\tye2.mdf;integrated security=True;user instance=True;multipleactiveresultsets=True;App=EntityFramework'";
			return new tye.API.API(strConn);
		}

		protected void Page_Init(object sender, EventArgs e) {
			int intCounter = 2;
			List<User> opticians = new List<User>();
			using (var ipa = GetApi()) {
				opticians = ipa.UserGetCollection().Where(n => n.Type == tye.Data.User.UserType.Optician && n.Enabled).ToList();
			}
			// http://maps.googleapis.com/maps/api/geocode/xml?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=true
			using (var client = new WebClient()) {
				foreach (var optician in opticians) {
					try {
						string url = "http://maps.googleapis.com/maps/api/geocode/xml" +
					"?address=" +
					optician.Address + ",+" +
					optician.PostalCode + "+" +
					optician.City + ",+" +
					optician.Country.Name +
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
						//Response.Write(geo.ChildNodes[0].InnerText + "<br />");
						//Response.Write(geo.ChildNodes[1].InnerText);
						//Response.Write("<br /><br />");
						litLocations.Text = "['" + intCounter + "', " + poslat + ", " + poslong + ", " + intCounter++ + "]," + litLocations.Text;
					} catch (Exception ex) {
						string s = ex.ToString();
					}
				} // foreach
			} // using

		}
	}
}