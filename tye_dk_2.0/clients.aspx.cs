using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using monosolutions.Utils;

public partial class clients : PageBase {

	int EditID = 0;
	bool Editing = false;
	PropertyMapper PM = null;

	public static readonly log4net.ILog log = log4net.LogManager.GetLogger("TyeLogger");

	private void checkPermissions() {
		if (!new object[] { 
			tye.Data.User.UserType.Optician}
			.Contains(CurrentUser.Type))
			Response.Redirect("/");
	}

	protected void Page_Init(object sender, EventArgs e) {
		//log.Info("Testing logger mechanism");

		checkPermissions();
		PM = new PropertyMapper(new object());

		switch (CurrentLanguage.ID) { 
			case 1: // Danish
				dpExpirationDate.Culture = dpBirthday.Culture = new System.Globalization.CultureInfo("da-DK");
				break;
			case 2: // Norsk
				dpExpirationDate.Culture = dpBirthday.Culture = new System.Globalization.CultureInfo("nb-NO");
				break;
			case 3: // English
				dpExpirationDate.Culture = dpBirthday.Culture = new System.Globalization.CultureInfo("en-GB");
				break;
			case 4: // German
				dpExpirationDate.Culture = dpBirthday.Culture = new System.Globalization.CultureInfo("de-DE");
				break;
		}

		dpExpirationDate.LabelToday = dpBirthday.LabelToday = DicValue("todayIs");

		PM.AddMapping(tbAddress, "Address");
		PM.AddMapping(tbCity, "City");
		PM.AddMapping(tbEmail, "Email");
		PM.AddMapping(tbMobilePhone, "MobilePhone");
		PM.AddMapping(tbFirstName, "FirstName");
		PM.AddMapping(dpBirthday, "Birthday");
		PM.AddMapping(tbMiddleName, "MiddleName");
		PM.AddMapping(tbLastName, "LastName");
		PM.AddMapping(tbPhone, "Phone");
		PM.AddMapping(tbPostalCode, "PostalCode");
		PM.AddMapping(tbState, "State");
		PM.AddMapping(ddlCountry, "CountryID");
		PM.AddMapping(ddlLanguage, "LanguageID");
		//PM.AddMapping(ddlUserType, "Type");
		PM.AddMapping(chkEnabled, "Enabled");

		int.TryParse(VC.RqValue("id"), out EditID);
		Editing = VC.RqHasValue("id");

		plhEdit.Visible = Editing;
		plhList.Visible = !plhEdit.Visible;

		clientTools.Visible = CurrentUserIsAdmin();

		if (Editing) {
			using (var ipa = statics.GetApi()) {
				ddlCountry.DataSource = ipa.CountryGetCollection();
				ddlCountry.DataTextField = "Name";
				ddlCountry.DataValueField = "ID";
				ddlCountry.DataBind();

				ddlLanguage.DataSource = ipa.LanguageGetCollection();
				ddlLanguage.DataTextField = "Name";
				ddlLanguage.DataValueField = "ID";
				ddlLanguage.DataBind();
			}

			lnkImpersonate.Visible = CurrentUserIsAdmin();
			lnkCreateNew.Visible = false;
			populateData();
		} else {
			using (var ipa = statics.GetApi()) {
				if (!IsPostBack) {
					ddlFilterCountry.DataSource = ipa.CountryGetCollection();
					ddlFilterCountry.DataTextField = "Name";
					ddlFilterCountry.DataValueField = "ID";
					ddlFilterCountry.DataBind();
					populateClients();
				}
			}
		}
	}

	protected void ddlFilterCountry_SelectedIndexChanged(object sender, EventArgs e) {
		//populateClients();
	}

	protected void ElnkCreateClientProgram_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			var client = ipa.UserGetSingle(EditID);
			if (client == null || client.Type != tye.Data.User.UserType.Client) {
				log.Error("Exiting ElnkCreateClientProgram_Click. Client doesn't exist: " + EditID);
				return;
			}

			var program = ipa.ProgramGetSingleByUserID(client.ID);
			if (program != null) {
				log.Info("Deleting Program for client " + EditID);
				foreach (var pet in program.ProgramEyeTests)
					ipa.ProgramEyeTestDeletePermanently(pet.ID);
				ipa.ProgramDeletePermanently(program.ID);
			}

			program = new tye.Data.Program() { ClientUserID = client.ID, ID = 0, Comment = "" };
			var anams = ipa.AnamneseGetCollection(client.ID);
			if (anams.Count == 0) {
				log.Error("Exiting ElnkCreateClientProgram_Click. No anamnese found for client " + EditID);
				return;
			}

			program = ipa.ProgramSave(program);

			var eyetests = ipa.EyeTestGetCollection();
			var anam = anams.OrderByDescending(n => n.Created).FirstOrDefault();
			var points21 = ipa.Measuring21GetCollection(client.ID).FirstOrDefault();
			var control = ipa.MeasuringControlGetCollection(client.ID).OrderBy(n => n.Created).FirstOrDefault();
			var defaults = new string[] { /*"3dBalls",*/ "flower", "star", /*"findTheNumbers",*/ "findTheFigure", "jumpFixation", "flowerNegative", "flowerPosNeg", "followMovements", "letterBall", "pokeTest", "brocksLine", "larva", "snakesNegative", "snakesPositive", "letterFocus", "flipGlasses", "ninjaTraining" };
			var defaultScores = new int[] { 2, 3, 4 };
			List<string> lstAddedNotes = new List<string>();

			var Q4 = new string[] { "labyrinth", "letterHunt", "columnJump", "followTheEye", "harrysBlocks", "stevesCardGame" };
			var Q7 = new string[] { "labyrinth", "letterHunt", "columnJump", "racer", "followTheEye" };
			var Q9 = new string[] { "labyrinth", "letterHunt", "columnJump", "racer", "eyeHandCoordination" };
			var Q11 = new string[] { "labyrinth", "letterHunt", "columnJump", "followTheEye", "racer", "eyeHandCoordination" };
			var Q12 = new string[] { "labyrinth", "letterHunt", "columnJump", "followTheEye" };
			var Q13 = new string[] { "reflexTraining", "coordinationTraining", "anglesInTheSnow", "thumb" };
			var Q14 = new string[] { "balanceBoard", "pokeTest", /*"vestibularTraining",*/ "vestibularTrainingWithBar" };
			var Q15 = new string[] { /*"vestibularTraining",*/ "vestibularTrainingWithBar" };
			var Q16 = new string[] { "eyeHandCoordination", "reflexTraining", "coordinationTraining", "anglesInTheSnow", "thumb", "racer", "clapStomp", "sunStar", "balanceBoard" };
			var Q17 = new string[] { "eyeHandCoordination", "reflexTraining", "coordinationTraining", "anglesInTheSnow", "thumb", "racer", "clapStomp", "sunStar", "balanceBoard" };
			var Q18 = new string[] { /*"vestibularTraining", */"vestibularTrainingWithBar", "balanceBoard" };

			// <default eyetests>
			foreach (string eyetestName in defaults) {
				var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
				//ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = (eyetest.ScreenTest && eyetestName != "3dBalls"), Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
				ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = (eyetest.ScreenTest && eyetestName != "flower"), Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
			}
			// </default eyetests>

			// <test for "cheat" program>
			bool blnCheatOk = true;
			foreach (var prop in anam.GetType().GetProperties()) {
				if (prop.Name.StartsWith("Q")) {
					int intQ = (int)prop.GetValue(anam, null);
					if (intQ > 0) {
						blnCheatOk = false;
						break;
					}
				}
			}
			if (blnCheatOk) {
				if (anam.MaxReadingHours != 2 || anam.DailyCloseRangeWork != 2)
					blnCheatOk = false;
			}

			if (blnCheatOk) {
				log.Info("Exiting ElnkCreateClientProgram_Click. Cheat program created for client " + EditID);
				Response.Redirect("program.aspx?ClientUserID=" + EditID + "&ru=" + Server.UrlEncode("clients.aspx?id=" + EditID));
			}
			// </test for "cheat" program>

			// <anamnese>
			if (defaultScores.Contains(anam.Q4)) {
				foreach (string eyetestName in Q4) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q7)) {
				foreach (string eyetestName in Q7) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q9)) {
				foreach (string eyetestName in Q9) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q11)) {
				foreach (string eyetestName in Q11) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q12)) {
				foreach (string eyetestName in Q12) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q13)) {
				foreach (string eyetestName in Q13) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID }); 
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q14)) {
				foreach (string eyetestName in Q14) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID }); 
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q15)) {
				foreach (string eyetestName in Q15) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID }); 
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q16)) {
				foreach (string eyetestName in Q16) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID }); 
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q17)) {
				foreach (string eyetestName in Q17) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID }); 
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			if (defaultScores.Contains(anam.Q18)) {
				foreach (string eyetestName in Q18) {
					var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID }); 
                    //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
				}
			}
			// </anamnese>

			// <measure control>
			if (control != null) {
				if (control.Convergence1 > 1) { // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
					foreach (string eyetestName in new string[] { "labyrinth", "letterHunt", "columnJump" }) {
						var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                        ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID }); 
                        //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
					}

					if (control.Convergence2 == 2 || control.Convergence2 == 3) {
						foreach (string eyetestName in new string[] { "labyrinth", "letterHunt", "columnJump" }) {
							var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                            ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                            //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
						}
					}

					if (control.Convergence3 == 1) {
						foreach (string eyetestName in new string[] { /*"vestibularTraining",*/ "vestibularTrainingWithBar", "balanceBoard" }) {
							var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                            ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID }); 
                            //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
						}
					}

				}  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				bool motilityBigDiff = Math.Abs(control.MotilityPointsRightEye - control.MotilityPointsLeftEye) > 1;
				if (motilityBigDiff) {
					foreach (string eyetestName in new string[] { "labyrinth", "letterHunt", "columnJump" }) {
						var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                        ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                        //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
					}
				}
				var ott = new int[] { 1, 2, 3 };
				if (ott.Contains(control.MotilityPointsRightEye) || ott.Contains(control.MotilityPointsLeftEye) || ott.Contains(control.MotilityPointsBothEyes)) {
					foreach (string eyetestName in new string[] { "labyrinth", "letterHunt", "columnJump", "racer" }) {
						var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                        ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                        //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
					}
				}

				if ((new int[] { 2, 3, 4 }).Contains(control.MotilityHorizontalEyeMovements)) {
					foreach (string eyetestName in new string[] { "labyrinth", "letterHunt", "columnJump", "racer" }) {
						var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                        ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                        //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
					}
				}
				if ((new int[] { 1 }).Contains(control.MotilityHeadMovements)) {
					foreach (string eyetestName in new string[] { "labyrinth", "letterHunt", "columnJump", "racer" }) {
						var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                        ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                        //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
					}
				}
				if ((new int[] { 1 }).Contains(control.MotilityDidClientSway)) {
					foreach (string eyetestName in new string[] { /*"vestibularTraining",*/ "vestibularTrainingWithBar", "balanceBoard" }) {
						var eyetest = eyetests.FirstOrDefault(n => n.InternalName == eyetestName);
                        ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = true, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                        //ipa.ProgramEyeTestSave(program.ID, eyetest.ID, false, eyetest.Priority);
					}
				}
			}
			// </measure control>


			// <points21>
			if (points21 != null) {
			}

			// </points21>

			// important to reload here ... or the eye tests will be removed on save after notes!
			program = ipa.ProgramGetSingle(program.ID);
			if (program.ProgramEyeTests.Count(m => m.Active) < 1) {
				log.Error("New program for client " + EditID + " doesn't contain any eyetests");
			}
			// <notes>


			if (program.ProgramEyeTests.Count <= 16)
				program.Comment += DicValue("programNote2Months") + "<br />";
			else if (program.ProgramEyeTests.Count >= 17 && program.ProgramEyeTests.Count <= 30)
				program.Comment += DicValue("programNote4Months") + "<br />";
			else if (program.ProgramEyeTests.Count >= 31)
				program.Comment += DicValue("programNote6Months") + "<br />";

			if (defaultScores.Contains(anam.Q9)) {
				if (!lstAddedNotes.Contains("programNoteMonocular")) {
					program.Comment += DicValue("programNoteMonocular") + "<br />";
					lstAddedNotes.Add("programNoteMonocular");
				}
			}

			if (defaultScores.Contains(anam.Q11)) {
				if (!lstAddedNotes.Contains("programNoteMonocular")) {
					program.Comment += DicValue("programNoteMonocular") + "<br />";
					lstAddedNotes.Add("programNoteMonocular");
				}
			}

			if (defaultScores.Contains(anam.Q12)) {
				if (!lstAddedNotes.Contains("programNoteMigraine")) {
					program.Comment += DicValue("programNoteMigraine") + "<br />";
					lstAddedNotes.Add("programNoteMigraine");
				}
			}

			if (control != null) {
				if (control.Convergence1 > 1) { // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
					if (control.Convergence2 == 2 || control.Convergence2 == 3) {
						if (!lstAddedNotes.Contains("programNoteMonocular")) {
							program.Comment += DicValue("programNoteMonocular") + "<br />";
							lstAddedNotes.Add("programNoteMonocular");
						}
					}
					if (control.Convergence3 == 1) {
						if (!lstAddedNotes.Contains("programNoteMonocular")) {
							program.Comment += DicValue("programNoteMonocular") + "<br />";
							lstAddedNotes.Add("programNoteMonocular");
						}
					}
				}  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				bool motilityBigDiff = Math.Abs(control.MotilityPointsRightEye - control.MotilityPointsLeftEye) > 1;
				if (motilityBigDiff) {
					if (!lstAddedNotes.Contains("programNoteMonocular")) {
						program.Comment += DicValue("programNoteMonocular") + "<br />";
						lstAddedNotes.Add("programNoteMonocular");
					}
				}
				if ((new int[] { 1 }).Contains(control.MotilityHeadMovements)) {
					if (!lstAddedNotes.Contains("programCheckCoordination")) {
						program.Comment += DicValue("programCheckCoordination") + "<br />";
						lstAddedNotes.Add("programCheckCoordination");
					}
				}
				if ((new int[] { 1 }).Contains(control.MotilityDidClientSway)) {
					if (!lstAddedNotes.Contains("programCheckBalance")) {
						program.Comment += DicValue("programCheckBalance") + "<br />";
						lstAddedNotes.Add("programCheckBalance");
					}
				}
			} // /control

			// </notes>
			program = ipa.ProgramSave(program);

            // now add all the inactive tests
            foreach (var eyetest in eyetests) {
                if (!program.ProgramEyeTests.Any(m => m.EyeTestID == eyetest.ID)) {
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = false, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                }
            }

            // and finally custom tests
            foreach (var eyetest in ipa.EyeTestGetCollection(CurrentUser.ID)) {
                if (!program.ProgramEyeTests.Any(m => m.EyeTestID == eyetest.ID)) {
                    ipa.ProgramEyeTestSave(new tye.Data.ProgramEyeTest() { Active = false, Locked = false, Priority = eyetest.Priority, ProgramID = program.ID, EyeTestID = eyetest.ID });
                }
            }

			log.Info("Program test count on final save: " + program.ProgramEyeTests.Count + ". Client ID: " + EditID);
		}

		save(); // save the client just to be sure
		Response.Redirect("program.aspx?ClientUserID=" + EditID + "&ru=" + Server.UrlEncode("clients.aspx?id=" + EditID));
	}

	protected void eLnkCreateNew_Click(object sender, EventArgs e) {
		using (var ipa = statics.GetApi()) {
			if (ipa.ActivationCodesRemaining(CurrentUser.ID).Count() < 1)
				redir();
			
			var availableCode = ipa.ActivationCodeGetCollection(CurrentUser.ID).Where(n => n.ClientUserID == null && n.Code == ddlUseCode.SelectedItem.Text).FirstOrDefault();
			if (availableCode == null) {
				statics.log.Error("Couldn't find available ActivationCode in spite of ActivationCodesRemaining returning > 0");
				redir();
			}

			var newUser = new tye.Data.User() {
				ID = 0,
				Type = tye.Data.User.UserType.Client,
				Address = "",
				Birthday = DateTime.Now.AddYears(-1),
				City = "",
				CountryID = 1, // Denmark
				CreatedOn = DateTime.UtcNow,
				Description = "",
				Email = "",
				Enabled = true,
				FirstName = "",
				MiddleName = "",
				LastName = "",
				LanguageID = 1, /* Danish */
				MobilePhone = "",
				MustChangePassword = false,
				Password = "",
				Phone = "",
				PostalCode = "",
				State = ""
			};

			newUser = ipa.UserSave(newUser);
			ipa.OpticianClientAdd(CurrentUser.ID, newUser.ID);
			availableCode.ClientUserID = newUser.ID;
			ipa.ActivationCodeSave(availableCode);
			ipa.ProgramSave(new tye.Data.Program() { ClientUserID = newUser.ID, ID = 0, Comment = "" });

			Response.Redirect("clients.aspx?id=" + newUser.ID);
		}

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
		int intSave = save();
		if (intSave > 0)
			Response.Redirect(VC.QueryStringStripNoTrail("id"));
	}

	protected void eLnkSave_Click(object sender, EventArgs e) {
		int intSave = save();
		if (intSave <= 0)
			return;
		else
			Response.Redirect(VC.QueryStringStrip("id") + "id=" + intSave);
	}

	private int save() {
		tye.Data.User user = new tye.Data.User();

		using (var ipa = statics.GetApi()) {
			if (EditID > 0) { // existing
				user = ipa.UserGetSingle(EditID);
			} else {
				user.MustChangePassword = false; // client don't need to change passwords
			}
			if (user != null) {
				// 2013-12-04: this is allowed because of children and such without own email :-(
				//var usersWithEmail = ipa.UserSearch("Email.ToLower().Equals(@0) && ID != @1", new object[] { tbEmail.Text.Trim().ToLower(), user.ID }.ToList());
				//if (usersWithEmail.Any()) {
				//   // display error
				//   AddJavascript("tye.showMessage('" + DicValue("error_userEmailExists").JsEncode() + "', 'error');");
				//   return -1;
				//}

				PM.Object = user;
				PM.MapToProperties();
				user.Type = tye.Data.User.UserType.Client;
				user.Birthday = dpBirthday.SelectedDate;

				if (EditID == 0)
					user.CreatedOn = DateTime.UtcNow;

				user = ipa.UserSave(user);
				if (EditID == 0) {
					ipa.OpticianClientAdd(CurrentUser.ID, user.ID);
					ipa.ProgramSave(new tye.Data.Program() { ClientUserID = user.ID, ID = 0, Comment = "" });
				}

				if (plhExpiresOn.Visible && EditID > 0) { // existing
					if (user == null || user.Type != tye.Data.User.UserType.Client)
						return user.ID;

					var codes = ipa.ActivationCodeGetCollectionByClient(user.ID);
					if (codes.Any()) {
						var first = codes
							.Where(n => n.ExpirationDate.HasValue)
							.OrderByDescending(n => n.ExpirationDate)
							.FirstOrDefault();

						if (dpExpirationDate.SelectedDate < first.ActivationDate)
							return user.ID;

						first.ExpirationDate = dpExpirationDate.SelectedDate;
						ipa.ActivationCodeSave(first);
					}
				}

			}
		}
		return user.ID;
	}

	// EDIT
	private void populateData() {
		using (var ipa = statics.GetApi()) {
			if (EditID > 0) { // existing
				var user = ipa.UserGetSingle(EditID);
				if (user == null || user.Type != tye.Data.User.UserType.Client)
					redir();

				if (ipa.OpticianClientGetOptician(EditID).ID != CurrentUser.ID)
					redir();

				PM.Object = user;
				PM.MapToControls();
				dpBirthday.SelectedDate = user.Birthday;
				litCreatedOn.Text = user.CreatedOn.ToDefString();

				var codes = ipa.ActivationCodeGetCollectionByClient(user.ID);
				if (codes.Any()) {
					var first = codes
						.Where(n => n.ExpirationDate.HasValue)
						.OrderByDescending(n => n.ExpirationDate)
						.FirstOrDefault();
					if (first != null)
						dpExpirationDate.SelectedDate = first.ExpirationDate.Value;
					else {
						plhExpiresOn.Visible = false; // cannot change expiration until client logged in at least once
					}

					if (codes.Any()) { 
						var firstActive = codes.OrderByDescending(n => n.ExpirationDate).FirstOrDefault();
						if (firstActive != null) {
							litCode.Text = DicValue("code") + ": " + firstActive.Code;
						}
					}
				} else {
					plhExpiresOn.Visible = false;
				}

			} else { // new
				lnkDeleteClient.Visible = lnkImpersonate.Visible =
				lnkToProgram.Visible = false;
				lnkToClientLog.Visible = false;
				lnkToAnamnese.Visible = false;
				lnkToStartMeasuring.Visible = false;
				chkEnabled.Checked = true;
			}

			string ru = Server.UrlEncode(VC.QueryStringStripNoTrail(""));
			
			var anams = ipa.AnamneseGetCollection(EditID);
			var measures = ipa.MeasuringControlGetCollection(EditID).OrderBy(n => n.Created).ToList();
			var measures21 = ipa.Measuring21GetCollection(EditID);
			lnkToStartMeasuring.Text = " (" + (measures.Count > 0 ? "1" : "0") + ") " + DicValue("startMeasuring");
			lnkToAnamnese.Text = "(" + anams.Count + ") " + DicValue("anamnese");
			// fake shit to satisfy maria's start measuring fetish :)
			lnkToMeasuringControl.Text = " (" + (measures.Count > 1 ? (measures.Count - 1).ToString() : "0") + ") " + DicValue("mc_namePlural");
			lnkToMeasuring21.Text = " (" + measures21.Count + ") " + DicValue("mc21_name");

			lnkCreateClientProgram.Visible = (anams.Count > 0);
			transClientGenerateProgramWarning.Visible = !lnkCreateClientProgram.Visible;

			lnkToAnamnese.NavigateUrl = "clientAnamnese.aspx?ClientUserID=" + EditID + "&ru=" + ru;
			lnkToProgram.NavigateUrl = "program.aspx?ClientUserID=" + EditID + "&ru=" + ru;
			if (measures.Count > 0) {
				lnkToStartMeasuring.NavigateUrl = "measuringControl.aspx?ClientUserID=" + EditID + "&MeasuringID=" +
					measures.OrderBy(q => q.Created).First().ID
					+ "&isStart=true&ru=" + ru;
			} else {
				lnkToStartMeasuring.NavigateUrl = "measuringControl.aspx?ClientUserID=" + EditID + "&MeasuringID=0&isStart=true&ru=" + ru;
				lnkToMeasuringControl.Visible = false;
			}
			
			lnkToMeasuringControl.NavigateUrl = "measuringControl.aspx?ClientUserID=" + EditID + "&ru=" + ru;
			lnkToMeasuring21.NavigateUrl = "measuring21.aspx?ClientUserID=" + EditID + "&ru=" + ru;
			lnkToClientLog.NavigateUrl = "clientLog.aspx?ClientUserID=" + EditID + "&ru=" + ru;
			//lnkToClientLog.Target = "_blank";


			// hide generation link when programeyetests exist
			var program = ipa.ProgramGetSingleByUserID(EditID);
			if (program != null && program.ProgramEyeTests.Count > 0)
				pnlCreateClientProgram.Visible = false;

		}
	}

	private void populateClients() {
		int intCountryID = 1;
		if (!int.TryParse(ddlFilterCountry.SelectedValue, out intCountryID))
			intCountryID = 1;

		using (var ipa = statics.GetApi()) {
			var clients = ipa.UserGetCollectionByOptician(CurrentUser.ID);
			repClientsActive.DataSource = clients.Where(n => n.Enabled);
			repClientsActive.DataBind();

			repClientsInactive.DataSource = clients.Where(n => !n.Enabled);
			repClientsInactive.DataBind();

			litActiveCount.Text = repClientsActive.Items.Count.ToString();
			litInactiveCount.Text = repClientsInactive.Items.Count.ToString();

			var codesRemain = ipa.ActivationCodesRemaining(CurrentUser.ID);
			int intRemainingCodes = codesRemain.Count;

			lnkCreateNew.Visible = ddlUseCode.Visible = (intRemainingCodes > 0);
			if (ddlUseCode.Visible) {
				ddlUseCode.DataSource = codesRemain;
				ddlUseCode.DataTextField = "Code";
				ddlUseCode.DataValueField = "ID";
				ddlUseCode.DataBind();
			}
			litRemainingCodes.Text = DicValue("remainingCodes").Replace("{0}", intRemainingCodes.ToString());
		}
	}

	private void redir() {
		if (VC.RqHasValue("ru"))
			Response.Redirect(VC.RqValue("ru"));
		else
			Response.Redirect(VC.QueryStringStripNoTrail("id"));
	}
}