using monosolutions.Utils;
using System;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;

public partial class clientEyeTestScreen : PageBase
{

    protected int ProgramEyeTestID = 0;
    private bool IgnoreProgram = false;

    private void checkPermissions()
    {
        if (!new object[] {
            tye.Data.User.UserType.Optician,
            tye.Data.User.UserType.Client}
            .Contains(CurrentUser.Type))
            Response.Redirect("/");
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //checkPermissions();

        if (!int.TryParse(VC.RqValue("ID"), out ProgramEyeTestID))
        {
            redir();
        }
        if (VC.RqHasValue("IgnoreProgram") && VC.RqValue("IgnoreProgram") == "true")
        {
            IgnoreProgram = true;
        }

        //var eyetestid = 0;

        using (var ipa = statics.GetApi())
        {
            if (IgnoreProgram)
            { // showing the eye test for everyone but clients

                var eyetest = ipa.EyeTestGetSingle(ProgramEyeTestID);
                if (eyetest == null)
                {
                    redir();
                }
                //eyetestid = eyetest.ID;

                litEyeTestName.Text = eyetest.InfoValue("Name", CurrentLanguage);

                string path = "~/controls/eyeTestScreen/" + eyetest.InternalName + ".ascx";
                if (System.IO.File.Exists(HostingEnvironment.MapPath(path)))
                {
                    plhUc.Controls.Add(LoadControl(path));
                }
                path = "/js/eyetest/" + eyetest.InternalName + ".js";
                if (System.IO.File.Exists(HostingEnvironment.MapPath("~" + path)))
                {
                    plhUc.Controls.AddAt(0, new LiteralControl("<script type=\"text/javascript\" src=\"" + path + "?v=1\"></script>"));
                }

                if (eyetest.ScoreRequired > 0) { 
                    AddJavascript("eyeTestScreen.setScoreRequired('" + eyetest.ScoreRequired + "');");
                }
            }
            else
            {
                // eye test for clients
                var pet = ipa.ProgramEyeTestGetSingle(ProgramEyeTestID);
                if (pet == null)
                {
                    redir();
                }

                var program = ipa.ProgramGetSingle(pet.ProgramID);
                if ((program == null || program.ClientUserID != CurrentUser.ID))
                {
                    redir();
                }

                var eyetest = ipa.EyeTestGetSingle(pet.EyeTestID);
                if (eyetest == null)
                {
                    redir();
                }

                var activeTest = program.ProgramEyeTests.Where(n => n.EyeTestID == eyetest.ID).FirstOrDefault();
                if ((activeTest == null || activeTest.Locked || activeTest.LockedByOptician))
                {
                    redir();
                }

                var user = CurrentUser;
                //eyetestid = eyetest.ID;

                litEyeTestName.Text = eyetest.InfoValue("Name", CurrentLanguage);

                string path = "~/controls/eyeTestScreen/" + eyetest.InternalName + ".ascx";
                if (System.IO.File.Exists(HostingEnvironment.MapPath(path)))
                {
                    plhUc.Controls.Add(LoadControl(path));
                }
                path = "/js/eyetest/" + eyetest.InternalName + ".js";
                if (System.IO.File.Exists(HostingEnvironment.MapPath("~" + path)))
                {
                    plhUc.Controls.AddAt(0, new LiteralControl("<script type=\"text/javascript\" src=\"" + path + "\"></script>"));
                }

                if (eyetest.ScoreRequired > 0)
                    AddJavascript("eyeTestScreen.setScoreRequired('" + eyetest.ScoreRequired + "');");

            }

            //if (eyetestid == 875) {
            //    extraScripts.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" href =\"/css/eyetest-saccade.css\" />"));
            //}
        }
    }


    private void redir()
    {
        if (VC.RqHasValue("ru"))
            Response.Redirect(VC.RqValue("ru"));
        else
            Response.Redirect("clientProgram.aspx");
    }
}