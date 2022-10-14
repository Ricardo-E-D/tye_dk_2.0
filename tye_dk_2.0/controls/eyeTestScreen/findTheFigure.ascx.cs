using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Hosting;

public partial class controls_eyeTestScreen_findTheFigure : UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
		 LoadImages();
    }
	 private void LoadImages() {
		 string script = "images = new Array(); ";
		 script += "imgDir = \"/img/eyetest/findTheFigure/\"; var arr;";

		 DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~/img/eyetest/findTheFigure/"));
		 int counter = 0;

		 foreach (FileInfo fi in di.GetFiles("*.gif")) {
			 if (fi.Name.ToLower().EndsWith("exp.gif"))
				 continue;
				
			script += "var arr = new Array();";

			script += "arr.push('" + fi.Name + "');";
			string str = fi.Name;
			string[] arr = str.Split('-');
			script += "images[" + arr[0] + "] = arr;";
			 
			 counter++;
		 }

		 litScript.Text = "$(function() { " + script + "});";
	 }
}