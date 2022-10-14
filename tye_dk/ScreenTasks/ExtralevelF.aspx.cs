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
	/// Summary description for ExtralevelF.
	/// </summary>
	public partial class ExtralevelF : System.Web.UI.Page
	{
		private int intId = -1;

		protected void Page_Init(object sender, System.EventArgs e) {
			int.TryParse(Request.QueryString["intExerciseIdNo"], out intId);
			if (Session["tests"] == null && Request.QueryString["intExerciseIdNo"] != null) {
				Tests T = new Tests();
				T = T.GetTestFromId(intId);
				T.DatStarttime = DateTime.Now;
				Session["tests"] = T;
			} 
		}

		protected void Page_Load(object sender, System.EventArgs e) {
			if(((Tests)Session["tests"]).DatStarttime.CompareTo(Convert.ToDateTime("01-01-2004 01:01:01")) == 0)
				((Tests)Session["tests"]).DatStarttime = DateTime.Now;
			
			taskName.InnerHtml = (string)((Tests)Session["tests"]).StrName;
			btnClose.Value = (string)((Tests)Session["tests"]).HashInfos[5];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertStart.Value = (string)((Tests)Session["tests"]).HashInfos[13];
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			btnToBack.Attributes.Add("onclick", "document.location.href = 'ExtralevelF.aspx?intExerciseIdNo=" + intId.ToString() + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];
			
			this.slowerDiv.InnerHtml = (string)((Tests)Session["tests"]).HashInfos[10];
			this.fasterDiv.InnerHtml = (string)((Tests)Session["tests"]).HashInfos[9];
			
			if(!timeSpent.Value.ToString().Equals("!")) 
				CreateLog();

			if (IsPostBack) {
				if (Session["tests"] != null) {
					Tests T = (Tests)Session["tests"];
					T.DatStarttime = DateTime.Now;
					Session["tests"] = T;
				}
			}
		}

		public void CreateLog()
		{
			if(Session["tests"] != null) {
				Tests T = (Tests)Session["tests"];
				T.BlnCompleted = true;
				T.DblSeconds = Convert.ToDouble(timeSpent.Value);
				T.AttValue = attValue.Value;
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
