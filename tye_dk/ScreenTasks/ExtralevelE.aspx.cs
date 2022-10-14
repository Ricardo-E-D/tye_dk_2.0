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
	/// Summary description for ExtralevelE.
	/// </summary>
	public partial class ExtralevelE : System.Web.UI.Page
	{
		private int intId = -1;
		private int intLevel = 1;

		protected void Page_Init(object sender, System.EventArgs e) {
			try {
				intId = Convert.ToInt32(Request.QueryString["intExerciseIdNo"]);
			} catch {}

			if (Session["tests"] == null && Request.QueryString["intExerciseIdNo"] != null) {
				Tests T = new Tests();
				T = T.GetTestFromId(intId);
				T.DatStarttime = DateTime.Now;
				Session["tests"] = T;
			}
		}
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			try {
				intLevel = Convert.ToInt32(Request.QueryString["id"].ToString());
			}
			catch { }
			if(((Tests)Session["tests"]).DatStarttime.CompareTo(Convert.ToDateTime("01-01-2004 01:01:01")) == 0) 				((Tests)Session["tests"]).DatStarttime = DateTime.Now;
	
			taskName.InnerHtml = (string)((Tests)Session["tests"]).StrName;
			btnClose.Value = (string)((Tests)Session["tests"]).HashInfos[5];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertStart.Value = (string)((Tests)Session["tests"]).HashInfos[13];
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			btnToBack.Attributes.Add("onclick", "blnMakeParentPostback=false;document.location.href = 'ExtralevelE.aspx?id=" + intLevel.ToString() + "&intExerciseIdNo=" + intId.ToString() + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];
			
			this.GenerateMenu(intLevel);
			
			if(!IsPostBack) {
				Tests T = (Tests)Session["tests"];
				if(Convert.ToInt32("0" + T.AttValue) > 0) {
					int intTempLevel = Convert.ToInt32("0" + T.AttValue);
					//if(intTempLevel != intLevel) {
						int intTempOrgLevel = intLevel; // temporarily save new level
						intLevel = intTempLevel;        // while we overwrite it with previous level (from before postback)
						CreateLog();                    // so that the log will reflect correctly the exercise done
						intLevel = intTempOrgLevel;     // finally, of course, write new level back to global variable
						T = new Tests();
						T = T.GetTestFromId(intId);
						T.DatStarttime = DateTime.Now;
						Session["tests"] = T;
					//}
				}
			}

			((Tests)Session["tests"]).AttValue = intLevel.ToString();
			if(!timeSpent.Value.ToString().Equals("!"))
				CreateLog();
			
			if (IsPostBack) {
				if (Session["tests"] != null) {
					Tests T = (Tests)Session["tests"];
					T.DatStarttime = DateTime.Now;
					T.AttValue = intLevel.ToString(); //!!!
					Session["tests"] = T;
				}
			}
		}

		private void GenerateMenu(int id)
		{
			this.level.Value = id.ToString();
			for(int i = 0; i <= 5; i++) {
				Label lbl = new Label();
				lbl.Text = " | ";
				HyperLink button = new HyperLink();
				if(i == 0)
					button.Text = (string)((Tests)Session["tests"]).HashInfos[12];
				else 
					button.Text = i.ToString();
				
				button.NavigateUrl = "ExtralevelE.aspx?id=" + i + "&intExerciseIdNo=" + intId.ToString();
				button.Attributes.Add("onclick", "blnMakeParentPostback=false;");

				if(id == i) {
					button.Enabled = false;
					button.Style["color"] = "grey";
					button.Style["font-size"] = "14px";
				}
				this.letterMenu.Controls.Add(button);
				this.letterMenu.Controls.Add(lbl);
			}
		}

		public void CreateLog()
		{
			if (Session["tests"] != null) {
				Tests T = (Tests)Session["tests"];
				T.BlnCompleted = true;
				T.AttValue = intLevel.ToString(); //!!!
				if(timeSpent.Value == "!") {
					TimeSpan ts = DateTime.Now.Subtract(T.DatStarttime);
					T.DblSeconds = (ts.Minutes * 60) + ts.Seconds;
				} else {
					T.DblSeconds = Convert.ToDouble(timeSpent.Value);
				}
				T.saveLog();
				Session["tests"] = null;
			}
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
