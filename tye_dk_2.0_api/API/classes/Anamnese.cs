using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	
	[Serializable]
	public class Anamnese {
		public Anamnese() { }

		public int ID { get; set; }
		public int ClientUserID { get; set; }
		public DateTime Created { get; set; }

		public int Q1 { get; set; }
		public int Q2 { get; set; }
		public int Q3 { get; set; }
		public int Q4 { get; set; }
		public int Q5 { get; set; }
		public int Q6 { get; set; }
		public int Q7 { get; set; }
		public int Q8 { get; set; }
		public int Q9 { get; set; }
		public int Q10 { get; set; }
		public int Q11 { get; set; }
		public int Q12 { get; set; }
		public int Q13 { get; set; }
		public int Q14 { get; set; }
		public int Q15 { get; set; }
		public int Q16 { get; set; }
		public int Q17 { get; set; }
		public int Q18 { get; set; }
		public int Q19 { get; set; }
		public int Q20 { get; set; }

		/// <summary>
		/// How long can you read before your eyes tire? (hours)
		/// </summary>
		public int MaxReadingHours { get; set; }
		
		/// <summary>
		/// How many hours do you work a day with stuff in your face?
		/// </summary>
		public int DailyCloseRangeWork { get; set; }

		public string Comments { get; set; }
		public string Injuries { get; set; }
		public string Medication { get; set; }
		public string Sicknesses { get; set; }

	}
}
