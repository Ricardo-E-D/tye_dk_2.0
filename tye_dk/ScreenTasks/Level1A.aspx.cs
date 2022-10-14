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
	public partial class Level1A : System.Web.UI.Page
	{
		private int intId = -1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
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
			//btnClose.Attributes.Add("onclick", "blnSaveUponExit=true;window.close();");
			attValue.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertStart.Value = (string)((Tests)Session["tests"]).HashInfos[13];
			unlocked.Value = (string)((Tests)Session["tests"]).HashInfos[18];
			requirement.Value = ((Tests)Session["tests"]).IntRequirement.ToString();
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			btnToBack.Attributes.Add("onclick", "document.location.href = 'Level1A.aspx?intExerciseIdNo=" + intId.ToString() + "'");

			this.GenerateMenu();

			if(!timeSpent.Value.ToString().Equals("!")) {
				CreateLog();
			}
		}

		private void GenerateMenu()
		{
			this.vertical.Attributes["onclick"] = "AnimateCurve('horizontal'); StartLevel();";
			this.vertical.NavigateUrl = "#";
			this.vertical.Text = (string)((Tests)Session["tests"]).HashInfos[7];
			this.horizontal.Attributes["onclick"] = "AnimateCurve('vertical'); StartLevel();";
			this.horizontal.NavigateUrl = "#";
			this.horizontal.Text = (string)((Tests)Session["tests"]).HashInfos[8];
			this.circular.Attributes["onclick"] = "AnimateCurve('circular'); StartLevel();";
			this.circular.NavigateUrl = "#";
			this.circular.Text = (string)((Tests)Session["tests"]).HashInfos[21];
			this.random.Attributes["onclick"] = "AnimateCurve('random'); StartLevel();";
			this.random.NavigateUrl = "#";
			this.random.Text = (string)((Tests)Session["tests"]).HashInfos[22];
			this.diverse.Attributes["onclick"] = "AnimateCurve('diverse'); StartLevel();";
			this.diverse.NavigateUrl = "#";
			this.diverse.Text = (string)((Tests)Session["tests"]).HashInfos[23];
		}

		private void CreateLog()
		{
			Tests T = (Tests)Session["tests"];
			T.BlnCompleted = true;
			T.DblSeconds = Convert.ToDouble(timeSpent.Value);
			T.AttValue = log.Value;
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
