using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class controls_eyeTestScreen_racer : UserControlBase {
	protected int ProgramEyeTestID = 0;
	
	protected void Page_Init(object sender, EventArgs e) {
		int.TryParse(VC.RqValue("ID"), out ProgramEyeTestID);
		if(new string[] { "map1a",
		"map1b",
		"map2a",
		"map2b",
		"map3a",
		"map3b",
		"map4a",
		"map4b",
		"map5a",
		"map5b" }.Contains(VC.RqValue("map"))) {
			mapNum.Value = VC.RqValue("map");
		}
	}

	protected void Page_Load(object sender, EventArgs e) { }
}