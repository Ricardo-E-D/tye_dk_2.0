using System;

namespace tye
{
	/// <summary>
	/// Summary description for Errors.
	/// </summary>
	public class Errors
	{
		private string strHeader;
		private string strTechHeader;
		private string strDescription;
		private string strTechDescription;
		private int intReferer;
		private int intLine;
		private string strToDo;
		private int intErrorType;

		public Errors()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string StrHeader
		{
			get
			{
				return strHeader;
			}
			set
			{
				strHeader = value;
			}
		}

		public string StrTechHeader
		{
			get
			{
				return strTechHeader;
			}
			set
			{
				strTechHeader = value;
			}
		}

		public string StrDescription
		{
			get
			{
				return strDescription;
			}
			set
			{
				strDescription = value;
			}
		}

		public string StrTechDescription
		{
			get
			{
				return strTechDescription;
			}
			set
			{
				strTechDescription = value;
			}
		}

		public int IntReferer
		{
			get
			{
				return intReferer;
			}
			set
			{
				intReferer = value;
			}
		}
		
		public string StrToDo
		{
			get
			{
				return strToDo;
			}
			set
			{
				strToDo = value;
			}
		}
		
		public int IntLine
		{
			get
			{
				return intLine;
			}
			set
			{
				intLine = value;
			}
		}

		public int IntErrorType
		{
			get
			{
				return intErrorType;
			}
			set
			{
				intErrorType = value;
			}
		}		
	}
}
