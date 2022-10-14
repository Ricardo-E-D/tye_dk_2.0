using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Calculations
/// </summary>
public class Calculations {
	public Calculations() {
	}

	public void EvalHighscores(int UserID) {

		using (var ipa = statics.GetApi()) {

			var EyeTests = ipa.EyeTestGetCollection();

			// wonder how this is going to perform in the long run??!

			// get all eyetest where highscore cals make sence

			int score = 0;

			foreach (var eyetest in EyeTests.Where(n => n.HighscoreApplicable)) {
				// get all logs for this eyetest
				var logs = ipa.ClientEyeTestLogGetCollection(UserID, eyetest.ID);
				// now loop distinct attribvalues (for distinct levels and such)

				// skip level eval for letterHunt and 3d tests
				string[] internalNamesSkip = new string[] { "letterHunt", "3dBalls", "flower", "star", "findTheNumbers", "findTheFigure", "jumpFixation", "flowerNegative", "flowerPosNeg" };
				if (internalNamesSkip.Contains(eyetest.InternalName)) {
					// clear highscores
					foreach (var highscore in logs.Where(n => n.HighScore)) {
						highscore.HighScore = false;
						ipa.ClientEyeTestLogSave(highscore);
					}
					var hs = logs.OrderByDescending(n => n.Score).FirstOrDefault();
					if (hs != null) {
						hs.HighScore = true;
						ipa.ClientEyeTestLogSave(hs);
					}

				} else {
					foreach (var attribValue in logs.Select(n => n.AttribValue).Distinct()) {
						score = 0;

						// reset highscores
						foreach (var record in logs.Where(n => n.AttribValue == attribValue && n.HighScore)) {
							record.HighScore = false;
							ipa.ClientEyeTestLogSave(record);
						}

						// get current highscore
						var hs = logs
							.Where(n => n.AttribValue == attribValue && n.Score > 0)
							.OrderByDescending(n => n.Score)
							.FirstOrDefault();

						if (hs != null) {
							hs.HighScore = true;
							ipa.ClientEyeTestLogSave(hs);
						}
						/*
						// loop the scores

						foreach (var record in logs.Where(n => n.AttribValue == attribValue).OrderBy(n => n.EndTime)) {
							// log score is greater than accumulated score
							if (record.Score > score) {
								// save for later
								score = record.Score;
								// update record if not marked as highscore
								if (!record.HighScore) {
									record.HighScore = true;
									ipa.ClientEyeTestLogSave(record);
								} // if
							} else {  // not highscore
								if (record.HighScore) {
									record.HighScore = false;
									ipa.ClientEyeTestLogSave(record);
								}
							} // if score
						} // foreach

						*/
					} // foreach
				} // if letterHunt
			}
		}
	}
}