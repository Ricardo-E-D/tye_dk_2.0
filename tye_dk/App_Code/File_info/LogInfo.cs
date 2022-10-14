using System;
using System.IO;

namespace tye.File_info
{
	/// <summary>
	/// Summary description for LogINfo.
	/// </summary>
	public class LogInfo
	{
		public static void writeLogMessage(string message) 
		{

			try 
			{
				FileStream fStream = new FileStream(Attributes.LOG_FOLDER+"/tyeLog.txt", FileMode.Append, FileAccess.Write);

				StreamWriter writer = new StreamWriter(fStream);

				DateTime time = DateTime.Now;
				string time_S = time.ToString("MM/dd/yyyy HH:mm:ss");

				writer.WriteLine(time_S + ":" + message);
				writer.Close();
				fStream.Close();
			} 
			catch (Exception e) 
			{
				string test = e.ToString();
			}
		}
	}
}
