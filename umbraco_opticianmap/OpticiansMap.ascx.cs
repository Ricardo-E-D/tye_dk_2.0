using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Net;
using tye.Data;

namespace umbraco_opticianmap {
	public partial class OpticiansMap : System.Web.UI.UserControl {
		public tye.API.API GetApi() {
			string c = "DatabaseEntities";
			try {
				if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("localhost"))
					c += "Local";
			} catch (Exception) { }
			string strConn = "";

			strConn = @"metadata=res://*/db.dbModel.csdl|res://*/db.dbModel.ssdl|res://*/db.dbModel.msl;provider=System.Data.SqlClient;provider connection string='data source=mssql3.unoeuro.com;initial catalog=tye_dk_db;persist security info=True;user id=tye_dk;password=a3z4chdg;multipleactiveresultsets=True;App=EntityFramework'";
			return new tye.API.API(strConn);
		}

		protected void Page_Init(object sender, EventArgs e) {
			int intCounter = 2;
			List<User> opticians = new List<User>();
			using (var ipa = GetApi()) {
				opticians = ipa.UserGetCollection().Where(n => n.Type == tye.Data.User.UserType.Optician && n.Enabled && n.ShowOnMap).ToList();
			}
			// http://maps.googleapis.com/maps/api/geocode/xml?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=true
			using (var client = new WebClient()) {
				foreach (var optician in opticians) {
					try {

						if (String.IsNullOrEmpty(optician.Lat) || String.IsNullOrEmpty(optician.Long)) {
							continue;
						}

						string info = optician.FullName + "<br />" + optician.Address + "<br />" 
							+ optician.PostalCode + " " + optician.City + " " + optician.State;
						info += "<br />" + optician.Phone;

						info = info.Replace(Environment.NewLine, "");
						litLocations.Text = "['" + info + "', " + optician.Lat + ", " + optician.Long + ", " + intCounter++ + "]," + litLocations.Text;
					} catch (Exception ex) {
						string s = ex.ToString();
					}
				} // foreach
			} // using

		}
	}
}