using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class exportLars : PageBase
{

	private string d(string key)
	{
		return Dictionary.GetValue(key, CurrentLanguage);
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		using (var ipa = statics.GetApi())
		{

			var clients = ipa.UserGetCollection(tye.Data.User.UserType.Client);

			StringBuilder sbAnam = new StringBuilder();
			sbAnam.Append("Optiker\t");
			sbAnam.Append("Klient\t");

			sbAnam.Append(d("anam_medicine") + "\t");
			sbAnam.Append(d("anam_sickness") + "\t");
			for (var i = 1; i <= 20; i++) {
				sbAnam.Append("Q" + i + "\t");
			}
			sbAnam.Append(d("anam_readingHours") + "\t");
			sbAnam.Append(d("anam_hoursNear") + "\t");
			sbAnam.Append(Environment.NewLine);


			StringBuilder sbMeasure = new StringBuilder();
			sbMeasure.Append("Optiker\t");
			sbMeasure.Append("Klient\t");

			sbMeasure.Append(d("mc_convergenceStep1") + "\t");
			sbMeasure.Append(d("mc_convergenceStep2") + "\t");
			sbMeasure.Append(d("mc_convergenceStep3") + "\t");
			sbMeasure.Append(d("mc_motilityStep1") + "\t");
			sbMeasure.Append("Note" + "\t");
			sbMeasure.Append(d("mc_motilityStep2") + "\t");
			sbMeasure.Append("Note" + "\t");
			sbMeasure.Append(d("mc_motilityStep3") + "\t");
			sbMeasure.Append("Note" + "\t");
			sbMeasure.Append(d("mc_motilityStep4") + "\t");
			sbMeasure.Append("Note" + "\t");
			sbMeasure.Append(d("mc_motilityStep5") + "\t");
			sbMeasure.Append("Note" + "\t");
			sbMeasure.Append(d("mc_motilityStep6") + "\t");
			sbMeasure.Append("Note" + "\t");
			sbMeasure.Append(Environment.NewLine);

			int counter = 0;
			int clientcount = clients.Count();
			foreach (var client in clients)
			{
				Response.Write("Client " + ++counter + " of " + clientcount + "<br />");
				Response.Flush();

				var optician = ipa.OpticianClientGetOptician(client.ID);
				var anams = ipa.AnamneseGetCollection(client.ID);
				var converge = ipa.MeasuringControlGetCollection(client.ID);

				if (!anams.Any() && !converge.Any()) {
					continue;
				}

				foreach (var item in converge)
				{
					sbMeasure.Append(optician.FullName + "\t");
					sbMeasure.Append(client.FullName + "\t");

					sbMeasure.Append(item.Convergence1 + "\t");
					sbMeasure.Append(item.Convergence2 + "\t");
					sbMeasure.Append(item.Convergence3 + "\t");

					sbMeasure.Append(item.MotilityPointsRightEye + "\t");
					sbMeasure.Append(item.NoteMotilityPointsRightEye + "\t");

					sbMeasure.Append(item.MotilityPointsLeftEye + "\t");
					sbMeasure.Append(item.NoteMotilityPointsLeftEye + "\t");

					sbMeasure.Append(item.MotilityPointsBothEyes + "\t");
					sbMeasure.Append(item.NoteMotilityPointsBothEyes + "\t");

					sbMeasure.Append(item.MotilityHorizontalEyeMovements + "\t");
					sbMeasure.Append(item.NoteMotilityHorizontalEyeMovements + "\t");

					sbMeasure.Append(item.MotilityHeadMovements + "\t");
					sbMeasure.Append(item.NoteMotilityHeadMovements + "\t");

					sbMeasure.Append(item.MotilityDidClientSway + "\t");
					sbMeasure.Append(item.NoteMotilityDidClientSway + "\t");
					sbMeasure.Append(Environment.NewLine);
				}

				foreach (var item in anams)
				{
					sbAnam.Append(optician.FullName + "\t");
					sbAnam.Append(client.FullName + "\t");

					sbAnam.Append(item.Medication + "\t");
					sbAnam.Append(item.Sicknesses + "\t");

					sbAnam.Append(item.Q1 + "\t");
					sbAnam.Append(item.Q2 + "\t");
					sbAnam.Append(item.Q3 + "\t");
					sbAnam.Append(item.Q4 + "\t");
					sbAnam.Append(item.Q5 + "\t");
					sbAnam.Append(item.Q6 + "\t");
					sbAnam.Append(item.Q7 + "\t");
					sbAnam.Append(item.Q8 + "\t");
					sbAnam.Append(item.Q9 + "\t");
					sbAnam.Append(item.Q10 + "\t");

					sbAnam.Append(item.Q11 + "\t");
					sbAnam.Append(item.Q12 + "\t");
					sbAnam.Append(item.Q13 + "\t");
					sbAnam.Append(item.Q14 + "\t");
					sbAnam.Append(item.Q15 + "\t");
					sbAnam.Append(item.Q16 + "\t");
					sbAnam.Append(item.Q17 + "\t");
					sbAnam.Append(item.Q18 + "\t");
					sbAnam.Append(item.Q19 + "\t");
					sbAnam.Append(item.Q20 + "\t");

					sbAnam.Append(item.MaxReadingHours + "\t");
					sbAnam.Append(item.DailyCloseRangeWork + "\t");
					sbAnam.Append(Environment.NewLine);
					

				}

			}

			using (var fs = System.IO.File.CreateText(@"c:\temp\tyeanam.txt")) {
				fs.Write(sbAnam.ToString());
			}
			using (var fs = System.IO.File.CreateText(@"c:\temp\tyeconverge.txt"))
			{
				fs.Write(sbMeasure.ToString());
			}

		} //using db

	}

}