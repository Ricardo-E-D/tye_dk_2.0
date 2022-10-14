using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class Translation
{
	private Hashtable controls = new Hashtable(); 
	private Hashtable general = new Hashtable(); 
	private Hashtable strings = new Hashtable(); 
	private Hashtable controls_array = new Hashtable(); 
	private Hashtable general_array = new Hashtable(); 
	private Hashtable strings_array = new Hashtable(); 
	private string _filename = ""; 
	private string _mainsection = "";  
	private string _lang = "Dk";
	public static string[] DbLangs = new string[] { "Dk", "No", "Gb", "De" };

	/// <summary>
	/// Loads file and prepares tranlation collections
	/// </summary>
	/// <param name="Filename">Full path to XML-file containing translations</param>
	/// <param name="MainSection">The section name to load from XML file (this doesn't apply to the 'General' section)</param>
	/// <param name="Language">String representation of Language to load. Valid values are "DK", "UK", "DE"</param>
	public Translation(string Filename, string MainSection, string Language)
	{
		_filename = Filename;
		_mainsection = MainSection;
		_lang = Language;
		loadGeneral();
		loadStrings();
		loadControls();
	}

	private void loadGeneral()
	{
		load("/data/section[@name='general']/string", general, general_array);
	}
	private void loadStrings()
	{
		load("/data/section[@name='" + _mainsection + "']/string", strings, strings_array);
	}
	private void loadControls()
	{
		load("/data/section[@name='" + _mainsection + "']/controls/control", controls, controls_array);
	}
	private void load(string section, Hashtable hash, Hashtable hashArray)
	{
		try {
			XmlDocument m_xmld;
			XmlNodeList m_nodelist;

			m_xmld = new XmlDocument();
			m_xmld.Load(_filename);
			m_nodelist = m_xmld.SelectNodes(section);

			foreach (XmlNode m_node in m_nodelist) {
				Hashtable hashArrayValue = new Hashtable(DbLangs.Length);
				foreach (string lang in DbLangs) {
					hashArrayValue.Add(lang, m_node.Attributes.GetNamedItem("text" + lang).Value);
				}
				
				string strName = m_node.Attributes.GetNamedItem("name").Value;
				string strText = m_node.Attributes.GetNamedItem("text" + _lang).Value;
				if (!hash.ContainsKey(strName))
					hash.Add(strName, strText);

				if (!hashArray.ContainsKey(strName))
					hashArray.Add(strName, hashArrayValue);
			}
		}
		catch (Exception ex) {
			throw new ApplicationException(ex.Message);
		}
		finally {
		}
	}
	private void processControls(System.Web.UI.Control ctrl) {
		if (ctrl.ID != null) {
			if (controls.ContainsKey(ctrl.ID)) {
				string s = controls[ctrl.ID].ToString().Replace("&#60;", "<").Replace("&#62;", ">");
				s = s.Replace("&lt;", "<").Replace("&gt;", ">");

				if (ctrl is Button) {
					Button btn = (Button)ctrl;
					btn.Text = s;
				}
				if (ctrl is CheckBox) {
					CheckBox chk = (CheckBox)ctrl;
					chk.Text = s;
				}
				if (ctrl is HyperLink) {
					HyperLink tbc = (HyperLink)ctrl;
					tbc.Text = s;
				}
				if (ctrl is ImageButton) {
					ImageButton imgB = (ImageButton)ctrl;
					imgB.ToolTip = imgB.AlternateText = s;
				}
				if (ctrl is Label) {
					Label lb = (Label)ctrl;
					lb.Text = s;
				}
				if (ctrl is Literal) {
					Literal lit = (Literal)ctrl;
					lit.Text = s;
				}
				if (ctrl is Panel) {
					Panel pnl = (Panel)ctrl;
					pnl.GroupingText = s;
				}
				if (ctrl is RadioButton) {
					RadioButton rdb = (RadioButton)ctrl;
					rdb.Text = s;
				}
				if (ctrl is TableCell) {
					TableCell tbc = (TableCell)ctrl;
					tbc.Text = s;
				}
				if (ctrl is HtmlTableCell) {
					HtmlTableCell htc = (HtmlTableCell)ctrl;
					htc.InnerHtml = s;
				}
			} //end if
		}
		foreach (Control tControl in ctrl.Controls) {
			processControls(tControl);
		}
	}
	public void TranslateControls(Control Page)
	{
		foreach (Control ctrl in Page.Controls) {
			processControls(ctrl);
		}
	}
	public string GetString(string Key) {
		if (strings.ContainsKey(Key)) {
			return strings[Key].ToString();
		} else { 
			return "not found: " + Key; 
		}
	}
	public string GetGeneral(string Key)
	{
		if (general.ContainsKey(Key)) {
			return general[Key].ToString();
		} else {
			return "not found: " + Key;
		}
	}
	public string GetControlString(string Key) {
		if (controls.ContainsKey(Key)) {
			return controls[Key].ToString();
		} else {
			return "not found: " + Key;
		}
	}
	public Hashtable HashGetString(string Key)
	{
		if (strings_array.ContainsKey(Key)) {
			return (Hashtable)strings_array[Key];
		} else {
			return null;
		}
	}
	public Hashtable HashGetGeneral(string Key)
	{
		if (general_array.ContainsKey(Key)) {
			return (Hashtable)general_array[Key];
		} else {
			return null;
		}
	}
	public Hashtable HashGetControlString(string Key)
	{
		if (controls_array.ContainsKey(Key)) {
			return (Hashtable)controls_array[Key];
		} else {
			return null;
		}
	}
}
