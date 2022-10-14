using System;

namespace tye
{
	/// <summary>
	/// Summary description for User.
	/// </summary>
	public abstract class User : System.Web.UI.Page
	{
		private int intUserId;
		private string strPassword;
		private DateTime datAddedTime;
		private string strAddress;
		private string strZipCode;
		private string strCity;
		private string strEmail;
		private string strPhone;
		private int intIsActive;
		private int intUserTypeId;
		private int intLanguageId;
		private int intProgramId = 0;
		private bool _isDistributor = false;

		public int IntUserId
		{
			get
			{
				return intUserId;
			}
			set
			{
				intUserId = value;
			}
		}

		public int IntProgramId {
			get { return intProgramId; }
			set { intProgramId = value; }
		}

		public string StrZipCode
		{
			get
			{
				return strZipCode;
			}
			set
			{
				strZipCode = value;
			}
		}

		public int IntIsActive
		{
			get
			{
				return intIsActive;
			}
			set
			{
				intIsActive = value;
			}
		}

		public int IntUserTypeId
		{
			get
			{
				return intUserTypeId;
			}
			set
			{
				intUserTypeId = value;
			}
		}
		
		public int IntLanguageId
		{
			get
			{
				return intLanguageId;
			}
			set
			{
				intLanguageId = value;
			}
		}

		public string StrPassword
		{
			get
			{
				return strPassword;
			}
			set
			{
				strPassword = value;
			}
		}

		public string StrAddress
		{
			get
			{
				return strAddress;
			}
			set
			{
				strAddress = value;
			}
		}

		public string StrCity
		{
			get
			{
				return strCity;
			}
			set
			{
				strCity = value;
			}
		}

		public string StrEmail
		{
			get
			{
				return strEmail;
			}
			set
			{
				strEmail = value;
			}
		}
		
		public string StrPhone
		{
			get
			{
				return strPhone;
			}
			set
			{
				strPhone = value;
			}
		}


		public bool IsDistributor {
			get { return _isDistributor; }
			set { _isDistributor = value; }
		}


		public DateTime DatAddedTime
		{
			get
			{
				return datAddedTime;
			}
			set
			{
				datAddedTime = value;
			}
		}

		public User()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void CreateObj(string strPassword){}

		public void Add(){}

		public void Edit(){}

		public void Delete(){}

		public int getAge(DateTime birthdate)
		{
			int tmpAge;

			tmpAge = ((TimeSpan)DateTime.Now.Subtract(birthdate)).Days / 365;

			return tmpAge;
		}

		public string getFirstName(string strText)
		{
			if(strText.LastIndexOf(Convert.ToChar(" ")) != -1)
			{
				return strText.Substring(0,strText.LastIndexOf(Convert.ToChar(" ")));
			}
			else
			{
				return strText;
			}
		}

		public string getLastName(string strText)
		{
			if(strText.LastIndexOf(Convert.ToChar(" ")) != -1)
			{
				return strText.Substring(strText.LastIndexOf(Convert.ToChar(" "))+1,strText.Length - strText.Substring(0,strText.LastIndexOf(Convert.ToChar(" "))+1).Length);
			}
			else
			{
				return "";
			}
		}

		
	}
}
