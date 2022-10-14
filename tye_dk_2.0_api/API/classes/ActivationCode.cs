// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
	public class ActivationCode {

		public ActivationCode() { }

		public int ID { get; set; }
		public string Code { get; set; }
		public int OpticianUserID { get; set; }
		public int? ClientUserID { get; set; }
		public DateTime? ActivationDate { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public bool Printed { get; set; }

	}
}
