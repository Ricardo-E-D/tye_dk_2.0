using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using tye.exceptions;

namespace tye.popups {

	public partial class HarrysBlocks : System.Web.UI.Page {
		protected int intLang = 1;
		protected void Page_Load(object sender, System.EventArgs e) {
			if(((Login)Session["login"]).BlnSucces == true) {
				try {
					intLang = Convert.ToInt32(Request.QueryString["lang"]);
					
				}
				catch(NoDataFound ndf) {
					mainbody.Controls.Add(new LiteralControl(ndf.Message(((Menu)Session["menu"]).IntLanguageId).ToString()));
				}
			}
			
			pnlDk.Visible = (intLang == (int)Shared.Language.Danish);
			pnlDe.Visible = (intLang == (int)Shared.Language.German);
			pnlGb.Visible = (intLang == (int)Shared.Language.English);
			pnlNo.Visible = (intLang == (int)Shared.Language.Norwegian);
			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
