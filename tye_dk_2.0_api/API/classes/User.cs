// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class User {
		
		public int ID { get; set; }
		public string Description { get; set; }
		public string FullName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public bool Enabled { get; set; }
		public string Address { get; set; }
		public string PostalCode { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public int CountryID { get; set; }
		public Country Country { get; set; }
		public string JobTitle { get; set; }
		public string Email { get; set; }
		public string MobilePhone { get; set; }
		public string Phone { get; set; }
		public string Password { get; set; }
		public string OldPassword { get; set; }
		public UserType Type { get; set; }
		public bool MustChangePassword { get; set; }
		private Pud _Pud = new Pud();
		public Pud Pud { get { return _Pud; } set { _Pud = value; } }
		public DateTime CreatedOn { get; set; }
		public bool ShowOnMap { get; set; }

		public int OldDatabaseID { get; set; }
		public DateTime? Birthday { get; set; }
		public int LanguageID { get; set; }
		public Language Language { get; set; }

		public string Lat { get; set; }
		public string Long { get; set; }

		public bool TermsAccepted { get; set; }

		public enum UserType {
			SBA = 1,
			Administrator = 2,
			Distributor = 3,
			Optician = 4,
			Client = 5
		}

		public bool CheckPassword(string strPassword, string SecretKey) {
			System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(strPassword + SecretKey);
			bs = x.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs) {
				s.Append(b.ToString("x2").ToLower());
			}
			return (this.Password == s.ToString());
		}

		public static string EncryptPassword(string strPassword, string SecretKey) {
			System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(strPassword + SecretKey);
			bs = x.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs) {
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();
		}


		/* <impersonation> */

		public User ImpersonatingUser { get; set; }

		/* </impersonation> */
	}
}
