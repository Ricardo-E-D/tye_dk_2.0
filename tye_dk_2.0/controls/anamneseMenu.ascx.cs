using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class controls_anamneseMenu : UserControl {
	
	protected void Page_Load(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			int ClientUserID = 0;
			int.TryParse(VC.RqValue("ClientUserID"), out ClientUserID);

			var anams = ipa.MeasuringControlGetCollection(ClientUserID);

			lnkToAnamnese.NavigateUrl = "/clientAnamnese.aspx?ClientUserID=" + ClientUserID;
			lnkToMeasuring21.NavigateUrl = "/measuring21.aspx?ClientUserID=" + ClientUserID;
			lnkToMeasuringControl.NavigateUrl = "/measuringControl.aspx?ClientUserID=" + ClientUserID;
			
			if(anams.Count == 0) {
				lnkToStartMeasuringControl.NavigateUrl = "/measuringControl.aspx?ClientUserID=" + ClientUserID + "&MeasuringID=0&isStart=true";
				lnkToMeasuringControl.Visible = false;
			} else  {
				lnkToStartMeasuringControl.NavigateUrl = "/measuringControl.aspx?ClientUserID=" + ClientUserID + "&MeasuringID=" + anams.OrderBy(n => n.Created).First().ID + "&isStart=true";
			}

		}
	}


}