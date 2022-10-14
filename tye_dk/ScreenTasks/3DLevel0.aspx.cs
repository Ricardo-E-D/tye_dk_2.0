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
	public partial class _3DLevel0 : System.Web.UI.Page
	{
		private int intId = -1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			int.TryParse(Request.QueryString["intExerciseIdNo"], out intId);
			if(Session["tests"] == null) {
				if(intId > -1) {
					Tests T = new Tests();
					T = T.GetTestFromId(intId);
					Session["tests"] = T;
				}
			}
			taskName.InnerHtml = (string)((Tests)Session["tests"]).StrName;
			textGuide.InnerHtml = (string)((Tests)Session["tests"]).HashInfos[19];
			unlocked.Value = (string)((Tests)Session["tests"]).HashInfos[18];
			btnClose.Attributes.Add("onclick", "pausecomp(1000); window.close()");
			((Tests)Session["tests"]).DatStarttime = DateTime.Now;
			btnClose.Value = (string)((Tests)Session["tests"]).HashInfos[5];
			//btnClose.Attributes.Add("onclick", "Stop(); window.close()");
			btnToBack.Attributes.Add("onclick", "document.location.href = '3DLevel0.aspx" + (intId > -1 ? "?intExerciseIdNo=" + intId.ToString() : "") + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];
			btnToBack.Visible = false;
			if(!timeSpent.Value.ToString().Equals("!")) {
				this.CreateLog();
			}
		}

		public void CreateLog()
		{
			Tests T = (Tests)Session["tests"];
			T.BlnCompleted = true;
			T.DblSeconds = Convert.ToDouble(timeSpent.Value);
			T.saveLog();
			Session["tests"] = null;
	
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
