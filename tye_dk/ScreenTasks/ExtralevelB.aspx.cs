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
using tye;

namespace tye.ScreenTasks
{
	/// <summary>
	/// Summary description for ExtralevelB.
	/// </summary>
	public partial class ExtralevelB : System.Web.UI.Page
	{
		private int intId = -1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			int intCols = 2;
			string strMode = "cols";
			if(Request.QueryString["cols"] != null && Request.QueryString["mode"] != null) {
				int.TryParse(Request.QueryString["cols"].ToString(), out intCols);
				if (Request.QueryString["mode"].ToString() == "random")
					strMode = "random";
			}
			litJavascript.Text = "<script type=\"text/javascript\">GeneratePossibleCols(); ResetCols(); SetCols(" + intCols.ToString() + ", '" + strMode + "');</script>";
			log.Value = log.Value;

			try {
				intId = Convert.ToInt32(Request.QueryString["intExerciseIdNo"]);
			}
			catch { }
			if (Session["tests"] == null && intId > -1) {
				Tests T = new Tests();
				T = T.GetTestFromId(intId);
				T.DatStarttime = DateTime.Now;
				Session["tests"] = T;
			}

			if(((Tests)Session["tests"]).DatStarttime.CompareTo(Convert.ToDateTime("01-01-2004 01:01:01")) == 0)
				((Tests)Session["tests"]).DatStarttime = DateTime.Now;
			
			taskName.InnerHtml = (string)((Tests)Session["tests"]).StrName;
			btnClose.Value = (string)((Tests)Session["tests"]).HashInfos[5];
			btnClose.Attributes.Add("onclick", "window.close();");
			attValue.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			btnToBack.Attributes.Add("onclick", "blnSaveUponExit=false;document.location.href = 'ExtralevelB.aspx?intExerciseIdNo=" + intId.ToString() + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];

			if(!score.Value.ToString().Equals("!")) {
				CreateLog();
			}
		}

		private void CreateLog() {
			Tests T = (Tests)Session["tests"];
			if (T == null)
				return;

			T.BlnCompleted = true;
			T.AttValue = log.Value;
			T.IntScore = Convert.ToInt32(score.Value);
			T.BlnHighScore = false;
			if(Convert.ToInt32(score.Value) >= T.IntHighScore) { 
				T.BlnHighScore = true;
				T.IntHighScore = Convert.ToInt32(score.Value);
			}
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
