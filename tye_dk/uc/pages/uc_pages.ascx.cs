namespace tye.uc.pages
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	public partial class uc_pages : System.Web.UI.UserControl
	{
		private PlaceHolder head_ph;
		private int intPageId;
		private int intSubmenuId;
		private HtmlGenericControl page_header;
		private HtmlGenericControl page_body;
		private HtmlForm main_form;

		public PlaceHolder Head_ph
		{
			get
			{
				return head_ph;
			}
			set
			{
				head_ph = value;
			}
		}


		public int IntPageId
		{
			get
			{
				return intPageId;
			}
			set
			{
				intPageId = value;
			}
		}

		public int IntSubmenuId
		{
			get
			{
				return intSubmenuId;
			}
			set
			{
				intSubmenuId = value;
			}
		}

		public HtmlGenericControl Page_header
		{
			get
			{
				return page_header;
			}
			set
			{
				page_header = value;
			}
		}

		public HtmlGenericControl Page_body
		{
			get
			{
				return page_body;
			}
			set
			{
				page_body = value;
			}
		}

		public HtmlForm Main_form
		{
			get
			{
				return main_form;
			}
			set
			{
				main_form = value;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
	}
}
