using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class dynJs : PageBase {
	protected void Page_Init(object sender, EventArgs e) {
		
		Response.Buffer = true;
		string script = String.Empty;

		switch (VC.RqValue("q")) { 
			case "dic":
				script = clientDictionary();
				break;
		}

		Response.Write(script + "");
		Response.ContentType = "text/javascript";
		Response.Flush();
		Response.End();
	}

	private string clientDictionary() {
		var clientDic = CacheHandler.GetItem("globalDictionaryClientSide");
		if (clientDic != null)
			return clientDic.ToString();
		else
			return String.Empty;
	}

}