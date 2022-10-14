using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

/// <summary>
/// Summary description for wsTye
/// </summary>
[WebService(Namespace = "wsTye")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class wsTye : System.Web.Services.WebService {

	public wsTye() {
		//InitializeComponent(); 
	}

	private bool userOk {
		get {
			MasterBase mb = new MasterBase();
			return (mb.CurrentUser != null);
		}
	}

	private tye.Data.User currentUser {
		get {
			return ((new MasterBase()).CurrentUser);
		}
	}

	[WebMethod(EnableSession = true)]
	[ScriptMethod]
	public string HelloWorld() {
		System.Threading.Thread.Sleep(2000);
		return "Hello World - " + DateTime.Now;
	}

	[WebMethod(EnableSession = true)]
	[ScriptMethod]
	public string EyeTestStart(int ProgramEyeTestID) {
		string r = String.Empty;

		if (!userOk)
			return r;
		
		using (var ipa = statics.GetApi()) {
			var pet = ipa.ProgramEyeTestGetSingle(ProgramEyeTestID);
			if (pet == null)
				return r; // throw new Exception("wrong ProgramEyeTestID");

			var program = ipa.ProgramGetSingle(pet.ProgramID);
			if (program == null)
				return r; //throw new Exception("wrong ProgramEyeTestID -> Program");

			if (program.ClientUserID != currentUser.ID)
				return r; //throw new Exception("wrong ProgramEyeTestID. Doesn't belong to client.");

			tye.Data.ClientEyeTestLog cet = new tye.Data.ClientEyeTestLog() {
				ID = 0,
				StartTime = DateTime.Now,
				ProgramEyeTestID = ProgramEyeTestID,
				UpdateToken = monosolutions.Utils.Strings.CreateRandomString(13, false)
			};

			var cets = ipa.ClientEyeTestLogSearch("ProgramEyeTestID == @0 && EndTime == null", new object[] { ProgramEyeTestID }.ToList(), "StartTime");

			if (cets.Any()) { // in case previous logs went wrong
				cet = cets.First();
				cet.StartTime = DateTime.UtcNow;
			}
			ipa.ClientEyeTestLogSave(cet);

			r = cet.UpdateToken;
		}
		return r;
	}

	//[WebMethod(EnableSession = true)]
	//[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	//public void EyeTestEnd(int ProgramEyeTestID, string UpdateToken) {
	//   if (!userOk)
	//      return;

	//   using (var ipa = statics.GetApi()) {
	//      ipa.ClientEyeTestLogUpdateEndTime(ProgramEyeTestID, UpdateToken, DateTime.UtcNow);
	//   }

	//}

	[WebMethod(EnableSession = true)]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public void EyeTestEnd(int ProgramEyeTestID, string UpdateToken, string AttribName, string AttribValue, int Score) {
		if (!userOk)
			return;

		using (var ipa = statics.GetApi()) {
			//// todo: must eval next 3d test locked....

			//var tests = ipa.EyeTestGetCollection();
			//var eyeTest = ipa.EyeTestGetFromProgramEyeTestID(ProgramEyeTestID);

			ipa.ClientEyeTestLogUpdateEndTime(ProgramEyeTestID, UpdateToken, DateTime.Now, AttribName, AttribValue, Score);
		}

	}

}
