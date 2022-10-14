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
using System.Xml;
using System.IO;

namespace tye.ScreenTasks
{
	/// <summary>
	/// Summary description for ExtralevelC.
	/// </summary>
	public partial class ExtralevelC : System.Web.UI.Page
	{
		private string xmlFileName;
		private string[] charArr;
		private bool[] checkedCharArr;
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
			attValue.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertEnd.Value = (string)((Tests)Session["tests"]).HashInfos[14];
			alertStart.Value = (string)((Tests)Session["tests"]).HashInfos[13];
			hiscore.Value = ((Tests)Session["tests"]).IntHighScore.ToString() + "|" + (string)((Tests)Session["tests"]).HashInfos[17] + "|" + (string)((Tests)Session["tests"]).HashInfos[16];
			btnToBack.Attributes.Add("onclick", "document.location.href = 'ExtralevelC.aspx?intExerciseIdNo=" + intId.ToString() + "'");
			btnToBack.Value = (string)((Tests)Session["tests"]).HashInfos[6];

			this.charArr = new string[] {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
			this.checkedCharArr = new bool[] {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false};
			int id = 1;
			try {
				id = Convert.ToInt32(Request.QueryString["id"].ToString());
			} catch { } 

			this.xmlFileName = Server.MapPath("Texts/ExtralevelC.xml");
			this.GenerateMenu(id);
			this.PrintRightMenu();
			this.PrintText(id);
			if(!score.Value.ToString().Equals("!"))
				CreateLog();

			// mital 31/01-2009: date should be reset on every postback....I think (?)
			//((Tests)Session["tests"]).DatStarttime = DateTime.Now;
			// * mital
		}

		private void PrintText(int id)
		{
			string txt = this.GetXMLText(id);
			string[] lines = txt.Split('\n');
			int intCount = 0;
			for(int i = 0; i < lines.Length; i++)
			{
				char[] charArr = lines[i].ToCharArray();
				for(int j = 0; j < charArr.Length; j++)
				{ 
					Label lbl = new Label();
					lbl.ID = "Echar" + intCount.ToString();
					lbl.Text = charArr[j].ToString();
					lbl.Attributes["class"] = "elcText";
					lbl.Style["cursor"] = "pointer";
					lbl.Attributes["onclick"] = "SetCharacter('" + this.Check(charArr[j].ToString()).ToString() + "','" + charArr[j].ToString() + "', this)";
					this.textLabel.Controls.Add(lbl);

					intCount++;
				}
				
				Label lbl2 = new Label();
				lbl2.Text = "<br />";
				this.textLabel.Controls.Add(lbl2);
			}
		}

		private bool Check(string ch)
		{
			for(int i = 0; i < this.charArr.Length; i++)
			{
				if(this.charArr[i].ToString().Equals(ch.ToLower()))
				{
					int index = i;
					try 
					{
						bool var = this.checkedCharArr[i-1];
						if(var && (this.checkedCharArr[i] == false))
						{
							this.checkedCharArr[i] = true;
							return true;
						} 
						else 
						{
							return false;
						}
					} 
					catch 
					{
						if(this.checkedCharArr[i] != true)
						{
							this.checkedCharArr[i] = true;
							return true;
						} 
						
					}
				}
			}
			return false;
		}

		private void PrintRightMenu()
		{
			for(int i = 0; i < this.charArr.Length; i++)
			{
				Label lbl = new Label();
				lbl.Text = this.charArr[i].ToString().ToUpper();
				lbl.ID = "right" + this.charArr[i].ToString();
				lbl.CssClass = "rightMenuLabel";
				this.rightMenuLabel.Controls.Add(lbl);
				lbl = new Label();
				lbl.Text = "<br />";
				this.rightMenuLabel.Controls.Add(lbl);
			}
		}

		private void GenerateMenu(int id)
		{
			for(int i = 1; i <= 40; i++)
			{
				Label lbl = new Label();
				lbl.Text = " | ";
				HyperLink button = new HyperLink();
				button.Text = i.ToString();
				button.NavigateUrl = "ExtralevelC.aspx?id=" + i + "&intExerciseIdNo=" + intId.ToString();
				if(id == i)
				{
					button.Enabled = false;
					button.Style["color"] = "grey";
					button.Style["font-size"] = "14px";
				}
				this.letterMenu.Controls.Add(button);
				this.letterMenu.Controls.Add(lbl);
				if(i % 20 == 0)
				{
					Label lbl2 = new Label();
					lbl2.Text = "<br />";
					this.letterMenu.Controls.Add(lbl2);
				}
			}
		}

		private void CreateLog()
		{
			Tests T = (Tests)Session["tests"];
			string strTextId = "1";

			if(Request.QueryString["id"] != null) {
				if (Request.QueryString["id"].ToString() != "")
					strTextId = Request.QueryString["id"].ToString();
			}
			T.AttValue = strTextId;
			T.AttValue += " " + hideColor.Value;
			T.BlnCompleted = true;
			T.IntScore = Convert.ToInt32(score.Value);
			T.BlnHighScore = false;
			if(Convert.ToInt32(score.Value) >= T.IntHighScore)
			{ 
				T.BlnHighScore = true;
				T.IntHighScore = Convert.ToInt32(score.Value);
			}
			T.DblSeconds = Convert.ToDouble(timeSpent.Value);
			T.saveLog();
			Session["tests"] = null;
			
		}

		private string GetXMLText(int id)
		{	

			if ( File.Exists( this.xmlFileName ) )
			{
				try
				{
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.Load(this.xmlFileName);
					//xmlDoc.PreserveWhitespace = true;

					/* Construct an XPath to use for querying the XML file */
					string xPath = string.Format( "//texts/text[@id='{0}']",id);
                  
					/* Use XPath to find the user name */
					XmlNode textNode = xmlDoc.SelectSingleNode( xPath );

					bool isTextFound = ( null != textNode );
					if(isTextFound)
					{
						XmlNodeList nl = xmlDoc.SelectNodes(xPath);
						string txt = ((XmlNode)nl[0]).SelectSingleNode("txt").InnerText;
						return txt;//.Replace("\n", "<br />");
					}
				}
				catch (Exception ex)
				{
					Response.Write(ex.Message.ToString());
				}
				
			}
			return null;
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
