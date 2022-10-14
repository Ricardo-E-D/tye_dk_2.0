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
using System.IO;

namespace tye.ScreenTasks
{
	public partial class _3DLevel3 : System.Web.UI.Page {

		private int intId = -1;

		protected void Page_Load(object sender, System.EventArgs e) {
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

			textGuide.InnerHtml = (string)((Tests)Session["tests"]).HashInfos[19];
			taskName.InnerHtml = (string)((Tests)Session["tests"]).StrName;
			btnClose.Attributes.Add("onclick", "StopLvl(''); pausecomp(1000); window.close()");
			((Tests)Session["tests"]).DatStarttime = DateTime.Now;
			btnClose.Value = (string)((Tests)Session["tests"]).HashInfos[5];
			attValue.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertStart.Value = (string)((Tests)Session["tests"]).HashInfos[13];
			unlocked.Value = (string)((Tests)Session["tests"]).HashInfos[18];
			requirement.Value = ((Tests)Session["tests"]).IntRequirement.ToString();
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			//this.textGuide.InnerHtml = (string)test.HashInfos[19];
			btnToBack.Attributes.Add("onclick", "document.location.href = '3DLevel3.aspx?intExerciseIdNo=" + intId.ToString() + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];
			this.LoadImages();
			if(!timeSpent.Value.ToString().Equals("!")) {
				CreateLog();
			}
		}

		private void LoadImages() {
			string script = "<script language=javascript>;";
			script += "var imagesCircles = new Array();";
			script += "imgDir = \"Images/3DLevel3/\";";
			
			//DirectoryInfo di = new DirectoryInfo(Attributes.SCREENTASKS_IMAGES+"3DLevel3/circles/");
            DirectoryInfo di = new DirectoryInfo(Request.PhysicalApplicationPath + "/ScreenTasks/Images/3DLevel3/circles/");
			
			int counter = 0;
			
			foreach(FileInfo fi in di.GetFiles("*.gif")) {
				if(counter % 2 == 0) {
					script += "var arr = new Array();";
					script += "arr.push('" + fi.Name + "');";
				} else  {
					script += "arr.push('" + fi.Name + "');";
					string str = fi.Name;
					string[] arr = str.Split('-');
					script += "imagesCircles[" + arr[0] + "] = arr;";
				}
				counter++;
			}

			script += "</script>";
			Page.RegisterClientScriptBlock("script", script);
		}

		private void CreateLog() {
			Tests T = (Tests)Session["tests"];
			if(boolCompleted.Value == "true"){
				T.BlnCompleted = true;
			}
			T.IntScore = Convert.ToInt32(score.Value);

			T.BlnHighScore = false;

			if(Convert.ToInt32(score.Value) > T.IntHighScore)
			{ 
				T.BlnHighScore = true;
				T.IntHighScore = Convert.ToInt32(score.Value);
			}
			T.DblSeconds = Convert.ToDouble(timeSpent.Value);
			T.AttValue = errors.Value;
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
