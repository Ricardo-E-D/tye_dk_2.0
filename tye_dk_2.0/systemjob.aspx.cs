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

public partial class systemjob : System.Web.UI.Page
{
	protected void Page_Init(object sender, EventArgs e) {
        return;

		this.Form.DefaultFocus = tbLoginCode.ClientID;
		//fileImports();
		//Response.Write(tye.Data.User.EncryptPassword("zxcvbn", statics.App.GetSetting(SettingsKeys.EncryptionKey)));

        if (Request.QueryString["dotnector"] == "yup") {
            gogoDataJob();
        }
	}
    private void gogoDataJob() {
        using (var db = statics.GetApi()) {
            var eyetests = db.EyeTestGetCollection();

            // var programs = db.ProgramGetCollection();
            List<Program> programs = new List<Program>();
            programs.Add(db.ProgramGetSingle(11368));
            foreach (var program in programs) {

                foreach (var eyetest in eyetests) {
                    var programtest = program.ProgramEyeTests.FirstOrDefault(m => m.EyeTestID == eyetest.ID);
                    if (programtest == null)
                    {
                        db.ProgramEyeTestSave(new ProgramEyeTest() { 
                            Active = false,
                            EyeTestID = eyetest.ID,
                            ID = 0,
                            Locked = true,
                            Priority = eyetest.Priority,
                            ProgramID = program.ID
                        });
                    }
                    else {
                        //programtest.Priority = eyetest.Priority;
                        
                        //db.ProgramEyeTestSave(programtest, true);
                    }
                }

            }

        }

    }

}