using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	
	[Serializable]
	public class MeasuringControl {
		public MeasuringControl() { }

		public int ID { get; set; }
		public int ClientUserID { get; set; }
		public DateTime Created { get; set; }
		public bool IsStart { get; set; }

		public int Convergence1 { get; set; }
		public int Convergence2 { get; set; }
		public int Convergence3 { get; set; }

		// 1
		public int MotilityPointsRightEye { get; set; }
		// 2
		public int MotilityPointsLeftEye { get; set; }
		// 3
		public int MotilityPointsBothEyes { get; set; }
		// 4
		public int MotilityHeadMovements { get; set; }
		
		public int MotilityHorizontalEyeMovements { get; set; }

		public int MotilityDidClientSway { get; set; }

		// 1
		public string NoteMotilityPointsRightEye { get; set; }
		// 2
		public string NoteMotilityPointsLeftEye { get; set; }
		// 3
		public string NoteMotilityPointsBothEyes { get; set; }
		// 4
		public string NoteMotilityHeadMovements { get; set; }

		public string NoteMotilityHorizontalEyeMovements { get; set; }

		public string NoteMotilityDidClientSway { get; set; }

		public string Step1Changes { get; set; }
		public string Step1Comments { get; set; }

		public string testing { get; set; }
	}
}
