// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;
using tye.Data;
using System.Web.Hosting;

public partial class importheaven : System.Web.UI.Page
{
	protected void Page_Init(object sender, EventArgs e) {
		if(VC.RqValue("jabba") == "pizza")
			fileImports();
		//Response.Write(tye.Data.User.EncryptPassword("zxcvbn", statics.App.GetSetting(SettingsKeys.EncryptionKey)));
	}

	private void fileImports() {
		DataImport di = new DataImport(HttpContext.Current);
		bool blnDisableAllImport = false;
		
		//2
		if (false && !blnDisableAllImport) { // opticians
			using (var ipa = statics.GetApi()) {
				System.Diagnostics.Debug.WriteLine("Deleting activation codes");
				ipa.ActivationCodeDeleteAll();
				System.Diagnostics.Debug.WriteLine("Deleting users");
				ipa.UserDeleteAllForDataImport();
			}
			System.Diagnostics.Debug.WriteLine("Importing opticians");
			di.ImportOpticians();
		}
		
		//2
		if (false && !blnDisableAllImport) { // clients
			System.Diagnostics.Debug.WriteLine("Importing clients");
			di.ImportClients(/*HostingEnvironment.MapPath("/App_Data/importdata/client/all.txt")*/);
		}

		if (false && !blnDisableAllImport) {
			//di.ImportUNUSEDActivationCodes();
		}
		if (false && !blnDisableAllImport) {
			di.ImportEyeTestInfosNoSteps();
		}

		// 2
		if (false && !blnDisableAllImport) { // codes
			using (var ipa = statics.GetApi()) {
				ipa.ActivationCodeDeleteAll();
			}
			di.ImportActivationCodes(/*HostingEnvironment.MapPath("/App_Data/importdata/code/all.txt")*/);
		}
		
		if (false && !blnDisableAllImport) { // eyetests
			using (var ipa = statics.GetApi()) {
				var codes = ipa.EyeTestGetCollection();
				foreach (var code in codes) {
					ipa.EyeTestDelete(code.ID);
				}
			}
			di.ImportEyeTests(/*HostingEnvironment.MapPath("/App_Data/importdata/eyetest/eyetests.xml")*/);
			di.ImportEyeTestInfos(/*HostingEnvironment.MapPath("/App_Data/importdata/eyetest/eyetestinfos.xml")*/);
			di.ImportEyeTestNames(/*HostingEnvironment.MapPath("/App_Data/importdata/eyetest/eyetests.xml")*/);
		}

		

		if (false && !blnDisableAllImport) { // anamnese
			di.ImportAnamnese();
		}

		if (false && !blnDisableAllImport) { // anamnese
			di.ImportEquipment();
		}

		// !!!
		if (false && !blnDisableAllImport) { // test_schedule
			di.ImportPrograms();
		}
		
		// !!!
		if (false && !blnDisableAllImport) { // log_testresult
			di.ImportClientEyeTestLog();
		}
		//di.TestClientLogFilter();

		if (false && !blnDisableAllImport) { // anamnese
			di.ImportMotility();
			di.ImportConvergence();
		}

		//if (1 == 2 && !blnDisableAllImport) { // anamnese
		//   di.Import21Points();
		//}
		

		if (/* one-time thing*/ false && 1 == 2 && !blnDisableAllImport) { // anamnese
			di.Import21pointTranslations();
		}

	}
}