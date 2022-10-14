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

namespace tye.ScreenTasks
{
	/// <summary>
	/// Summary description for Level1A.
	/// </summary>
	public partial class Circel : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlInputButton btnClose, btnToBack;
		protected System.Web.UI.HtmlControls.HtmlInputHidden timeSpent, attValue, score, alertEnd, alertStart, requirement, boolCompleted,unlocked, startTime, hiscore,errors, log;
		protected HtmlGenericControl letterMenu;
		protected HyperLink vertical, horizontal, circular, random,diverse;

		protected void Page_Load(object sender, System.EventArgs e)
		{	

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
