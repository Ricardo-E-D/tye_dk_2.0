using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class program : PageBase {

	int EditID = 0;
	bool Editing = false;

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Optician }
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

    private void sortProgramTests(int programid) {
        using (var ipa = statics.GetApi())
        {
            var program = ipa.ProgramGetSingle(programid);
            if (program == null) {
                Response.Redirect("/");
            }

            var user = ipa.UserGetSingle(program.ClientUserID);
            var optician = ipa.OpticianClientGetOptician(user.ID);

            if (optician.ID != CurrentUser.ID) {
                Response.Redirect("/");
            }


            string order = VC.RqValue("order");
            if (string.IsNullOrEmpty(order)) {
                Response.Redirect("/");
            }


            int sortValue = 1;
            foreach (var testid in order.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                int iTryTestId = 0;
                if (!int.TryParse(testid, out iTryTestId)) {
                    continue;
                }
                sortValue++;

                var test = program.ProgramEyeTests.FirstOrDefault(m => m.EyeTestID == iTryTestId);
                // we don't add eye tests which are not in use to programs...but that kinda fucks up this logic
                if (test != null)
                {
                    ipa.ProgramEyeTestSave(program.ID, test.EyeTestID, test.Locked, sortValue, test.LockedByOptician);
                }
                //else {
                //    ipa.ProgramEyeTestSave(program.ID, test.EyeTestID, true, test.Priority, true); // mark everthing as locked
                //}
            }

            Response.Redirect("/program.aspx?ClientUserID=" + user.ID);
        }
    }


    private void resetProgramTestsSortOrder(int programid)
    {
        using (var ipa = statics.GetApi())
        {
            var program = ipa.ProgramGetSingle(programid);
            if (program == null)
            {
                Response.Redirect("/");
            }

            var user = ipa.UserGetSingle(program.ClientUserID);
            var optician = ipa.OpticianClientGetOptician(user.ID);

            if (optician.ID != CurrentUser.ID)
            {
                Response.Redirect("/");
            }

            var eyeTests = ipa.EyeTestGetCollection();
            eyeTests.AddRange(ipa.EyeTestGetCollection(optician.ID));


            foreach (var eyeTest in eyeTests)
            {
                var test = program.ProgramEyeTests.FirstOrDefault(m => m.EyeTestID == eyeTest.ID);

                if (test != null)
                {
                    ipa.ProgramEyeTestSave(program.ID, test.EyeTestID, test.Locked, eyeTest.Priority, test.LockedByOptician);
                }
                //else {
                //    ipa.ProgramEyeTestSave(program.ID, eyeTest.ID, true, eyeTest.Priority, true);
                //}
            }

            Response.Redirect("/program.aspx?ClientUserID=" + user.ID);
        }
    }

	protected void Page_Init(object sender, EventArgs e) {
		checkPermissions();

        int clientProgramSortingId = 0;
        if(int.TryParse(VC.RqValue("programid"), out clientProgramSortingId) && clientProgramSortingId > 0 && VC.RqValue("action") == "sort") {
            sortProgramTests(clientProgramSortingId);
            if (int.TryParse(VC.RqValue("ClientUserID"), out EditID) && EditID > 0) {
                Response.Redirect("/program.aspx?ClientUserID=" + EditID);
            }
        }
        if (int.TryParse(VC.RqValue("resetSortOrder"), out clientProgramSortingId) && clientProgramSortingId > 0)
        {
            resetProgramTestsSortOrder(clientProgramSortingId);
        }

		if (!int.TryParse(VC.RqValue("ClientUserID"), out EditID) || EditID == 0)
			redir();

		using (var ipa = statics.GetApi()) {
			var user = ipa.UserGetSingle(EditID);
			if (user == null)
				redir();

			// prevent access to other opticians users
			if (ipa.OpticianClientGetOptician(user.ID).ID != CurrentUser.ID)
				redir();

			lnkToPrint.NavigateUrl = "programPrint.aspx?ClientUserID=" + EditID;
			lnkBackToClient.NavigateUrl = "clients.aspx?id=" + EditID;
			lnkBackToClient.Text = " - " + user.FullName;

			AddJavascript("addCheckEvent()");

            
			var program = ipa.ProgramGetSingleByUserID(EditID);
			if (program == null)
				return;

            lnkResetSortOrder.NavigateUrl = "program.aspx?resetSortOrder=" + program.ID;

			if (!String.IsNullOrEmpty(program.Comment)) {
				litProgramComments.Text = Server.HtmlDecode(program.Comment);
				tbEditProgramComment.Text = Server.HtmlDecode(program.Comment).Replace("<br />", "\n").Replace("<br>", "\n");
			}

			var EyeTests = ipa.EyeTestGetCollection();
            EyeTests.AddRange(ipa.EyeTestGetCollection(CurrentUser.ID)); // add custom eye tests

            // for sorting
            var programeyetestids = program.ProgramEyeTests.OrderBy(m => m.Priority).Select(m => m.EyeTestID).ToList();
			
            foreach (var EyeTest in EyeTests.OrderBy(m => programeyetestids.IndexOf(m.ID)))
            //foreach (var EyeTest in EyeTests.OrderBy(m => m.Priority))
            {
				var activeTest = program.ProgramEyeTests.Where(n => n.Active && n.EyeTestID == EyeTest.ID).FirstOrDefault();

				TableRow row = new TableRow();
                row.Attributes.Add("data-testid", EyeTest.ID.ToString());
                row.Attributes.Add("data-programid", program.ID.ToString());

				TableCell tdActive = new TableCell();
				TableCell tdName = new TableCell();
				TableCell tdOpticianLock = new TableCell();
				TableCell tdLock = new TableCell();
				TableCell tdHighscore = new TableCell();

				CheckBox chkTestLockedByOptician = new CheckBox() {
					ID = "chkTestLockedByOptician" + EyeTest.ID,
					Checked = (activeTest != null ? activeTest.LockedByOptician : false),
					Text = "<img src=\"/img/lock.png\" alt=\"\" />"
				};

				CheckBox chkTestActive = new CheckBox() {
					ID = "chkTestActive" + EyeTest.ID,
					Checked = (activeTest != null),
					CssClass = "eyeTestActive"
				};

				tdOpticianLock.Controls.Add(chkTestLockedByOptician);
				tdActive.Controls.Add(chkTestActive);

				tdName.Text = "<a href=\"eyeTest.aspx?ID=" + EyeTest.ID + "\">" + EyeTest.InfoValue("Name", CurrentLanguage) + "</a>";

				bool EyeTestLockedByDefault = EyeTest.ScreenTest && EyeTest.ScoreRequired > 0;
				if (activeTest != null)
					EyeTestLockedByDefault = activeTest.Locked;

				CheckBox chkLocked = new CheckBox() {
					ID = "chkTestLocked" + EyeTest.ID,
					Checked = EyeTestLockedByDefault,
					Text = "<img src=\"/img/lock.png\" alt=\"\" />"
				};

				// no point in adding the control for EyeTest where locking doesn't apply
				if (EyeTest.ScoreRequired > 0)
					tdLock.Controls.Add(chkLocked);

				var highscore = ipa.ClientEyeTestLogGetHighScore(EditID, EyeTest.ID);
				if (highscore != null)
					tdHighscore.Text = highscore.Score.ToString();

                var iconCell = new TableCell() { Text = "<img src=\"/img/" + (EyeTest.ScreenTest ? "screen" : "text") + ".png\" alt=\"\" />" };
                iconCell.Text += "<i class=\"positivesmall fa fa-sort sort-handle link sort-visible\"></i>";

				row.Cells.Add(iconCell);
				row.Cells.Add(tdActive);
				row.Cells.Add(tdOpticianLock);
				row.Cells.Add(tdLock);
				row.Cells.Add(tdName);
				row.Cells.Add(tdHighscore);

                tdActive.CssClass += " sort-hide";
                tdOpticianLock.CssClass += " sort-hide";
                tdLock.CssClass += " sort-hide";

				tblProgram.Rows.Add(row);


				//if (activeTest != null)
				//   plhControls.Controls.Add(new LiteralControl("+++"));
				//plhControls.Controls.Add(new LiteralControl(EyeTest.Name + "<br />"));
			}
		}
		if (VC.RqHasValue("ru")) {
			lnkCancel.NavigateUrl = VC.RqValue("ru");
		}
	}

	protected void ddlFilterCountry_SelectedIndexChanged(object sender, EventArgs e) {
		//populateClients();
	}

	protected void eLnkImpersonate_Click(object sender, EventArgs e) {
		LinkButton snd = (LinkButton)sender;

		if (!CurrentUserIsAdmin()) {
			return;
		}

		using (var ipa = statics.GetApi()) {
			var imp = ipa.UserGetSingle(EditID);
			if (imp != null && (imp.Type != tye.Data.User.UserType.SBA)) {
				if (snd.ID != "lnkImpersonateWithLanguage") {
					imp.Language = CurrentUser.Language;
				}
				Impersonating = true;

				var cu = CurrentUser;
				cu.ImpersonatingUser = imp;
				CurrentUser = cu;
			}
		}
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	protected void eLnkSaveAndClose_Click(object sender, EventArgs e) {
		int intSave = save(((LinkButton)sender).ID);
		if (intSave <= 0)
			return;

		redir();
		Response.Redirect(VC.QueryStringStripNoTrail("id"));
	}

	protected void eLnkSave_Click(object sender, EventArgs e) {
		int intSave = save(((LinkButton)sender).ID);
		if (intSave <= 0)
			return;
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	// save the program comment
	protected void ElnkEditProgramCommentSave_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) { 
			var program = ipa.ProgramGetSingleByUserID(EditID);
			if (program == null)
				return;

			program.Comment = tbEditProgramComment.Text.Replace("\r\n", "<br />").Replace("\n", "<br />");
			ipa.ProgramSave(program);
		}
		Response.Redirect(VC.QueryStringStripNoTrail(""));
	}

	private int save(string senderLinkButtonID) {
		tye.Data.Program program = new tye.Data.Program();

		using (var ipa = statics.GetApi()) {
			program = ipa.ProgramGetSingleByUserID(EditID);
			if (program == null) {
				statics.log.Error("There was no eyetest program for userid " + EditID);
				redir();
			}

			List<int> lstActiveEyeTestIds = new List<int>();
			foreach (var control in VC.FindControlTypeRecursive(tblProgram, typeof(CheckBox))) {
				if (control is CheckBox && control.ID.StartsWith("chkTestActive") && ((CheckBox)control).Checked) {
					int iTry = 0;
					if (int.TryParse(control.ID.Replace("chkTestActive", ""), out iTry)) {
						lstActiveEyeTestIds.Add(iTry);
					}
				}
			}

            //if (program.ProgramEyeTests.Where(n => !lstActiveEyeTestIds.Contains(n.EyeTestID)).Any() && senderLinkButtonID != "lnkSaveWithConfirmation") {
            //    pnlConfirmEyeTestRemoval.Visible = true;
            //    return -1;
            //}
			pnlConfirmEyeTestRemoval.Visible = false;

            if (!program.ProgramEyeTests.Any()) { 
                // when this is a new program
                ipa.EyeTestGetCollection().ForEach(m => {
                    if (!lstActiveEyeTestIds.Contains(m.ID)) {
                        
                    
                    program.ProgramEyeTests.Add(new tye.Data.ProgramEyeTest() { 
                         Active = false,
                         EyeTestID = m.ID,
                         ID = 0,
                         Locked = true,
                         LockedByOptician = false,
                         Priority = m.Priority,
                         ProgramID = program.ID
                    });
                    }
                });
                ipa.EyeTestGetCollection(CurrentUser.ID).ForEach(m =>
                {
                    if (!lstActiveEyeTestIds.Contains(m.ID))
                    {
                        program.ProgramEyeTests.Add(new tye.Data.ProgramEyeTest()
                        {
                            Active = false,
                            EyeTestID = m.ID,
                            ID = 0,
                            Locked = true,
                            LockedByOptician = false,
                            Priority = m.Priority,
                            ProgramID = program.ID
                        });
                    }
                });

                program.ProgramEyeTests.ForEach(m => {
                    ipa.ProgramEyeTestSave(m);
                });
            }

			//program.ProgramEyeTests.RemoveAll(n => !lstActiveEyeTestIds.Contains(n.EyeTestID));
            program.ProgramEyeTests.ForEach(m => {
                if (!lstActiveEyeTestIds.Contains(m.EyeTestID))
                {
                    if (m.Active) {
                        m.Active = false;
                        ipa.ProgramEyeTestUpdateActive(m.ID, false);
                    }
                }
                else {
                    if (!m.Active) {
                        m.Active = true;
                        ipa.ProgramEyeTestUpdateActive(m.ID, true);
                    }
                }
            });
			
            var newEyeTests = ipa.EyeTestGetCollection(lstActiveEyeTestIds.Except(program.ProgramEyeTests.Select(n => n.EyeTestID)).ToList());
            newEyeTests.AddRange(ipa.EyeTestGetCollection(CurrentUser.ID).Where(m => !program.ProgramEyeTests.Select(n => n.EyeTestID).Contains(m.ID)));

            foreach (var newEyeTest in newEyeTests) {
                CheckBox chkLocked = (CheckBox)VC.FindControlRecursive(tblProgram, "chkTestLocked" + newEyeTest.ID);
                CheckBox chkLockedByOptician = (CheckBox)VC.FindControlRecursive(tblProgram, "chkTestLockedByOptician" + newEyeTest.ID);
                var newPet = new tye.Data.ProgramEyeTest()
                {
                    EyeTestID = newEyeTest.ID,
                    ID = 0,
                    Locked = false,
                    ProgramID = program.ID,
                    Active = true,
                    Priority = newEyeTest.Priority
                };

                newPet.Locked = (chkLocked != null && chkLocked.Checked);
                newPet.LockedByOptician = (chkLockedByOptician != null && chkLockedByOptician.Checked);
                ipa.ProgramEyeTestSave(newPet);
                program.ProgramEyeTests.Add(newPet);
            }

			// update all ProgramEyeTest with program lock
			foreach (var pet in program.ProgramEyeTests) {
				CheckBox chkLocked = (CheckBox)VC.FindControlRecursive(tblProgram, "chkTestLocked" + pet.EyeTestID);
				CheckBox chkLockedByOptician = (CheckBox)VC.FindControlRecursive(tblProgram, "chkTestLockedByOptician" + pet.EyeTestID);
				pet.Locked = (chkLocked != null && chkLocked.Checked);
				pet.LockedByOptician = (chkLockedByOptician != null && chkLockedByOptician.Checked);
			}

			ipa.ProgramSave(program);
		}
		return program.ID;
	}

	private void redir() {
		if (VC.RqHasValue("ru"))
			Response.Redirect(VC.RqValue("ru"));
		else
			Response.Redirect("clients.aspx");
	}
}