using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Hosting;

public partial class controls_eyeTestScreen_findTheNumbers : UserControlBase {

    protected void Page_Load(object sender, EventArgs e)
    {
		 LoadImages();
    }
	 private void LoadImages() {
		 string script = "images = new Array(); ";
		 script += "imgDir = \"/img/eyetest/findTheNumbers/circles/\"; var arr;";
		 
		 DirectoryInfo di = new DirectoryInfo(HostingEnvironment.MapPath("~/img/eyetest/findTheNumbers/circles"));
		 int counter = 0;

		 foreach (FileInfo fi in di.GetFiles("*.gif")) {
			 if (counter % 2 == 0) {
				 script += "arr = new Array();\n";
				 script += "arr.push('" + fi.Name + "');\n";
			 } else {
				 script += "arr.push('" + fi.Name + "');\n";
				 string str = fi.Name;
				 string[] arr = str.Split('-');
				 script += "images[" + arr[0] + "] = arr;\n";
			 }
			 counter++;
		 }

		 litScript.Text = "$(function() { " + script + "});";
	 }
}