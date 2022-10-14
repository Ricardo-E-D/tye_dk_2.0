using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco;

namespace UmbracoUserControls {
	public partial class testing : System.Web.UI.UserControl {

		private void write(string text) {

			this.Controls.Add(new LiteralControl(text + "<br />"));
		}

		private IEnumerable<umbraco.cms.businesslogic.member.Member> getAllOpticians() { 
			return umbraco.cms.businesslogic.member.Member.GetAllAsList().Where(n => n.ContentType.Alias == "Optician");
		}
		protected void Page_Load(object sender, EventArgs e) {

			foreach (var member in getAllOpticians()) { //umbraco.cms.businesslogic.member.Member.GetAllAsList()) {
				this.Controls.Add(new LiteralControl(member.LoginName));
				this.Controls.Add(new LiteralControl("<br />"));
				this.Controls.Add(new LiteralControl(member.Text));

				this.Controls.Add(new LiteralControl("<br />Properties:<br />"));
				write(member.ContentType.Alias);
				foreach (var prop in member.GenericProperties) {
					write(prop.PropertyType.Name);
				}
				
				this.Controls.Add(new LiteralControl("<br /><br />"));

			}

		}
	}
}