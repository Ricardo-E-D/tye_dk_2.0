using System;

namespace tye
{

	public class Questionaire
	{
		public static int intStep = 0;

		private double[] arrStep1 = new double[13];
		private double[] arrStep2 = new double[2];
		private double[] arrStep3 = new double[2];

		public Questionaire()
		{
            
		}

		public double[] ArrStep1
		{
			get
			{
				return arrStep1;
			}
			set
			{
				arrStep1 = value;
			}
		}

		public double[] ArrStep2
		{
			get
			{
				return arrStep2;
			}
			set
			{
				arrStep2 = value;
			}
		}

		public double[] ArrStep3
		{
			get
			{
				return arrStep3;
			}
			set
			{
				arrStep3 = value;
			}
		}
		
	}
}
