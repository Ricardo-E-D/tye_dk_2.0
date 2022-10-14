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

namespace tye.popups
{
	public partial class Testcard : System.Web.UI.Page
	{
		int intUserId;
		string strIsFirst;
		bool blnIsFirst;
	

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(((Login)Session["login"]).BlnSucces == true)
			{
				try
				{
					intUserId = Convert.ToInt32(Request.QueryString["clientid"]);
					strIsFirst = Request.QueryString["isfirst"];

					if(CheckId())
					{
						mainbody.Controls.Add(((Optician)Session["user"]).getTestCard(intUserId,blnIsFirst));
					}
					else
					{
						throw new NoDataFound();
					}
				}
				catch(NoDataFound ndf)
				{
					mainbody.Controls.Add(new LiteralControl(ndf.Message(((Menu)Session["menu"]).IntLanguageId).ToString()));
				}
			}
		}

		private bool CheckId()
		{
			if(intUserId == 0 || strIsFirst == "")
			{
				return false;
			}
			else
			{
				blnIsFirst = Convert.ToBoolean(strIsFirst);
			}

			return true;
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
