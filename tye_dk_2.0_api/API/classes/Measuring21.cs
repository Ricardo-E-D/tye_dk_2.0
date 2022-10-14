using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	
	[Serializable]
	public class Measuring21 {
		public Measuring21() { }

		public int ID { get; set; }
		public int ClientUserID { get; set; }
		public DateTime Created { get; set; }

		public int Dominans { get; set; }
		public int CoverTestFar { get; set; }
		public int CoverTestNear{ get; set; }

		public string FixationParity { get; set; }
		public string StereopsisFar { get; set; }
		public string StereopsisNear { get; set; }
		public string PupilReflex { get; set; }
		public string VisionField { get; set; }

		public string SettingsAreaFrom { get; set; }
		public string SettingsAreaTo { get; set; }

		public string VisusRight { get; set; }
		public string VisusRightComment { get; set; }
		public string VisusLeft { get; set; }
		public string VisusLeftComment { get; set; }
		public string VisusBinocular { get; set; }
		public string VisusBinocularComment { get; set; }

		public string HabituelRight { get; set; }
		public string HabituelRightComment { get; set; }
		public string HabituelLeft { get; set; }
		public string HabituelLeftComment { get; set; }
		public string HabituelBinocular { get; set; }
		public string HabituelBinocularComment { get; set; }

		public string PinholevisusRight { get; set; }
		public string PinholevisusRightComment { get; set; }
		public string PinholevisusLeft { get; set; }
		public string PinholevisusLeftComment { get; set; }
		public string PinholevisusBinocular { get; set; }
		public string PinholevisusBinocularComment { get; set; }

		public decimal ntb21_3 { get; set; }
		public decimal ntb21_13a { get; set; }
		public decimal ntb21_4Hsf { get; set; }
		public decimal ntb21_4Hcyl1 { get; set; }
		public decimal ntb21_4Hcyl2 { get; set; }
		public decimal ntb21_4Hvisus { get; set; }
		public decimal ntb21_4Vsf { get; set; }
		public decimal ntb21_4Vcyl1 { get; set; }
		public decimal ntb21_4Vcyl2 { get; set; }
		public decimal ntb21_4Vvisus { get; set; }
		public decimal ntb21_5Hsf { get; set; }
		public decimal ntb21_5Hlag { get; set; }
		public decimal ntb21_5Hnetto { get; set; }
		public decimal ntb21_5Vsf { get; set; }
		public decimal ntb21_5Vlag { get; set; }
		public decimal ntb21_5Vnetto { get; set; }
		public decimal ntb21_7Hsf { get; set; }
		public decimal ntb21_7Hcyl1 { get; set; }
		public decimal ntb21_7Hcyl2 { get; set; }
		public decimal ntb21_7Hvisus { get; set; }
		public decimal ntb21_7Vsf { get; set; }
		public decimal ntb21_7Vcyl1 { get; set; }
		public decimal ntb21_7Vcyl2 { get; set; }
		public decimal ntb21_7Vvisus { get; set; }
		public decimal ntb21_7aHsf { get; set; }
		public decimal ntb21_7aHcyl1 { get; set; }
		public decimal ntb21_7aHcyl2 { get; set; }
		public decimal ntb21_7aHvisus { get; set; }
		public decimal ntb21_7aVsf { get; set; }
		public decimal ntb21_7aVcyl1 { get; set; }
		public decimal ntb21_7aVcyl2 { get; set; }
		public decimal ntb21_7aVvisus { get; set; }
		public decimal ntb21_8 { get; set; }
		public decimal? ntb21_9 { get; set; }
		public decimal ntb21_10_1 { get; set; }
		public decimal ntb21_10_2 { get; set; }
		public decimal ntb21_11_1 { get; set; }
		public decimal ntb21_11_2 { get; set; }
		public decimal ntb21_12Hs { get; set; }
		public decimal ntb21_12Hi { get; set; }
		public decimal ntb21_12Vs { get; set; }
		public decimal ntb21_12Vi { get; set; }
		public decimal ntb21_13b { get; set; }
		public decimal ntb21_14aHsf { get; set; }
		public decimal ntb21_14aHlag { get; set; }
		public decimal ntb21_14aHnetto { get; set; }
		public decimal ntb21_14aVsf { get; set; }
		public decimal ntb21_14aVlag { get; set; }
		public decimal ntb21_14aVnetto { get; set; }
		public decimal ntb21_15a { get; set; }
		public decimal ntb21_14bHsf { get; set; }
		public decimal ntb21_14bHlag { get; set; }
		public decimal ntb21_14bHnetto { get; set; }
		public decimal ntb21_14bVsf { get; set; }
		public decimal ntb21_14bVlag { get; set; }
		public decimal ntb21_14bVnetto { get; set; }
		public decimal ntb21_15b { get; set; }
		public decimal? ntb21_16a { get; set; }
		public decimal ntb21_16b_1 { get; set; }
		public decimal ntb21_16b_2 { get; set; }
		public decimal? ntb21_17a { get; set; }
		public decimal ntb21_17b_1 { get; set; }
		public decimal ntb21_17b_2 { get; set; }
		public decimal ntb21_18Hs { get; set; }
		public decimal ntb21_18Hi { get; set; }
		public decimal ntb21_18Vs { get; set; }
		public decimal ntb21_18Vi { get; set; }
		public decimal ntb21_19right { get; set; }
		public decimal ntb21_19left { get; set; }
		public decimal ntb21_19both { get; set; }
		public decimal ntb21_20 { get; set; }
		public decimal ntb21_21 { get; set; }
		public string ddl21_3 { get; set; }
		public string ddl21_13a { get; set; }
		public string ddl21_4H { get; set; }
		public string ddl21_4V { get; set; }
		public string ddl21_5H { get; set; }
		public string ddl21_5V { get; set; }
		public string ddl21_7H { get; set; }
		public string ddl21_7V { get; set; }
		public string ddl21_7aH { get; set; }
		public string ddl21_7aV { get; set; }
		public string ddl21_8 { get; set; }
		public string ddl21_10 { get; set; }
		public string ddl21_11 { get; set; }
		public string ddl21_12 { get; set; }
		public string ddl21_13b { get; set; }
		public string ddl21_14aH { get; set; }
		public string ddl21_14aV { get; set; }
		public string ddl21_15a { get; set; }
		public string ddl21_14bH { get; set; }
		public string ddl21_14bV { get; set; }
		public string ddl21_15b { get; set; }
		public string ddl21_16b { get; set; }
		public string ddl21_17b { get; set; }
		public string ddl21_18 { get; set; }

		public bool rb21_12H { get;set; }
		public bool rb21_12V { get;set; }
		public bool rb21_18H { get;set; }
		public bool rb21_18V { get;set; }

	}
}
