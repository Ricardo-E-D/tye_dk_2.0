using System;
using System.Data;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Email
/// </summary>
public class Email
{
	private string _sendername = "";
	private string _senderemail = "";
	private string _subject = "";
	private string _body = "";
	private string _smtpserver = "localhost";
	private string _recipient = "";
	private MailFormat _bodyformat = MailFormat.Text;

	public Email()
	{
	}

	public string RecipientEmail {
		get { return _recipient; }
		set { _recipient = value; }
	}
	
	public string SenderEmail {
		get { return _sendername; }
		set { _sendername = value; }
	}

	public string Subject {
		get { return _subject; }
		set { _subject = value; }
	}
	public string Body{
		get { return _body; }
		set { _body  = value; }
	}

	public string SmtpServer {
		get { return _smtpserver; }
		set { _smtpserver = value; }
	}
	
	public MailFormat BodyFormat {
		get { return _bodyformat; }
		set { _bodyformat = value; }
	}

	public void Send() {
		MailMessage objMail = new MailMessage();

		objMail.To = this.RecipientEmail; // objDr["email"].ToString();
		objMail.Subject = this.Subject; // arrMailHeader[intLanguageId].ToString();
		objMail.Body = this.Body; // arrMailBody[intLanguageId].ToString();
		objMail.From = this.SenderEmail; // "noreply@trainyoureyes.com";
		objMail.BodyFormat = this.BodyFormat; // MailFormat.Text;
		//SmtpMail.SmtpServer = "websmtp.hardball.nu";
		System.Web.Mail.SmtpMail.SmtpServer = this.SmtpServer; // "localhost";

		System.Web.Mail.SmtpMail.Send(objMail);
		objMail = null;
	}

}
