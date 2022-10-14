using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProcessStep
/// </summary>
public class ProcessStep
{
	public ProcessStep()
	{
	}

	//public enum ProcessType { 
	//   Caption,
	//   ResizeCrop,
	//   Adjustment
	//}
	
	//public ProcessType Type { get; set; }

	public Settings Settings { get; set; }

}