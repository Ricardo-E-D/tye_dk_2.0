using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using monosolutions.Utils;

public partial class controls_eyeTestScreen_letterHunt : UserControlBase {

	private string[] charArr = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
	private bool[] checkedCharArr;
	private int intTextID = 1;

	protected void Page_Load(object sender, System.EventArgs e) {
		int.TryParse(VC.RqValue("text"), out intTextID);

		if (intTextID < 1 || intTextID > 40)
			intTextID = 1;

		AddJavascript("$(function() { $('#hidAttribName').val('text'); $('#hidAttribValue').val('" + intTextID + "'); });");

		this.checkedCharArr = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
		
		this.GenerateMenu();
		this.PrintRightMenu();
		this.PrintText(intTextID);
		

		// mital 31/01-2009: date should be reset on every postback....I think (?)
		//((Tests)Session["tests"]).DatStarttime = DateTime.Now;
		// * mital
	}

	private void GenerateMenu() {
		for (int i = 1; i <= 40; i++) {
			litMenu.Text += ("<a class=\"" + (intTextID == i ? "negative" : "positive") + "small link\" href=\"" + VC.QueryStringStrip("text") + "text=" + i + "\">" + i + "</a>\n");
			if (i == 20)
				litMenu.Text += "<br />";
		}
	}

	private void PrintRightMenu() {
		for (int i = 0; i < this.charArr.Length; i++) {
			litRightMenu.Text += ("<div id=\"right" + charArr[i] + "\" class=\"rightMenuLabel\">" + charArr[i].ToUpper() + "</div>");
		}
	}

	private void PrintText(int id) {
		string txt = this.GetXMLText(id);
		string[] lines = txt.Split('\n');
		int intCount = 0;
		for (int i = 0; i < lines.Length; i++) {
			char[] charArr = lines[i].ToCharArray();
			for (int j = 0; j < charArr.Length; j++) {
				Label lbl = new Label();
				lbl.ID = "Echar" + intCount.ToString();
				lbl.EnableViewState = false;
				lbl.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				lbl.Text = charArr[j].ToString();
				lbl.Attributes["class"] = "elcText";
				lbl.Style["cursor"] = "pointer";
				lbl.Attributes["onclick"] = "SetCharacter('" + this.Check(charArr[j].ToString()).ToString() + "','" + charArr[j].ToString() + "', this)";
				this.textLabel.Controls.Add(lbl);

				intCount++;
			}

			Label lbl2 = new Label();
			lbl2.Text = "<br />";
			this.textLabel.Controls.Add(lbl2);
		}
	}

	private bool Check(string ch) {
		for (int i = 0; i < this.charArr.Length; i++) {
			if (this.charArr[i].ToString().Equals(ch.ToLower())) {
				int index = i;
				try {
					bool var = this.checkedCharArr[i - 1];
					if (var && (this.checkedCharArr[i] == false)) {
						this.checkedCharArr[i] = true;
						return true;
					} else {
						return false;
					}
				} catch {
					if (this.checkedCharArr[i] != true) {
						this.checkedCharArr[i] = true;
						return true;
					}
				}
			}
		}
		return false;
	}

	private string GetXMLText(int id) {

		try {
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlData);

			//xmlDoc.PreserveWhitespace = true;
			string xPath = string.Format("//texts/text[@id='{0}']", id);

			/* Use XPath to find the user name */
			XmlNode textNode = xmlDoc.SelectSingleNode(xPath);

			if (null != textNode) {
				XmlNodeList nl = xmlDoc.SelectNodes(xPath);
				string txt = ((XmlNode)nl[0]).SelectSingleNode("txt").InnerText;
				return txt;//.Replace("\n", "<br />");
			}
		} catch (Exception ex) {
			Response.Write(ex.Message.ToString());
		}
		return null;
	}

	#region xml

	string xmlData = @"<?xml version=&amp;1.0&amp; encoding=&amp;utf-8&amp; ?>
<texts>
	<text id=&amp;1&amp;>
		<txt>Iln chako evi nomd zeby thipg nare.
uth pirm nuroc dif stok. Nileg myt
lolf. Tixs nom raus zab tuin lugah.
Marb sewt rotsir puje. Yonak nesud
voz alee. Xart chod bugm turh sref
trea gen foru. Vab reps tique kowj.
Dagh meulb fwer ilg sida. Ubc they
bouf yed neoph vaik. Wolen kig peab
nad tenc xerb. Rait rebey fal zibt.
		</txt>
	</text>
	<text id=&amp;2&amp;>
		<txt>Kog dalp stey molb ihn zurc taiwf
pim noxod. Prus myl wof kipet ghul.
Zalv ubx pufo cirk ghons taw. Quos
mey lairp sut vaej obk tund zoelec
pech. Tym surg aben burz. Dof terav
tecib ulaw kars fups. Irt quech adg
doif vok nebel ach laurt. Goxe misd
chitk queal doj neav libef tagow.
Ligeh axd yabel fom nepok ratzin</txt>
	</text>
	<text id=&amp;3&amp;>
		<txt>Ulp sewtt gok gavy ziw toab dilf ur
miw. Vohy nizurc opd seux ckil. Yots
foxi tolz kawy vrus bab tigup nom.
Puv hals biet jawer moc. Zork tand
sdet youq dav galo zum. Derfran waf
zowst dape kich ques ibf garf duj
da. Exy zich dilns cabet lauf miteg
vibec foj darwd ebd. Lok narh phag
liguk tof repa suj danxy zerg meb</txt>
	</text>
	<text id=&amp;4&amp;>
		<txt>Pwev nagik dois tofb zuc mirav oxy
zid stouq spum nivog jelk ohy. Dirf
lopal ghibt urx. Noky sart wamt loc
duf daxib nujy. Zaw terk loip tefs
egh. Srat phol maew dur lepun tafoz.
Tearp farg lyz stib uch derj skiqu
kem gor soib. Fatiz laec dunb holf
beod gaim zive. Opf lekaw iny dech
noxeb sak dirp phylg. Afte ozip lum</txt>
	</text>
	<text id=&amp;5&amp;>
		<txt>Himz kolle dunth nacke horb kily.
Cith pyl mofod kuh ther nurvik dit
lazop juf. Gulo phots taj panil rok
doj brux. Kalb neb metar tobe. Pord
api wens suh terbod gaiw reaz bis
duig. Tympes galue quez lers kugi
zalc wod snote. Dowil geb kunch nim 
morb. Lavih dran wilk romop. Nefag
gurf nexap morc mayed lozorf geeb</txt>
	</text>
	<text id=&amp;6&amp;>
		<txt>Gane futh nebko. Vite sulc dispro
pirn luit queps upy norf. Muig quoy
pakab toaj turok rutha kulj. Nait
sud tojf reak lene ort pent sersom.
Caib nivart phar barco serg. Zife
bim tupe. Queth tef meger luge mals
Gak liem dacit. Eun howc doiv melbe
simm rawg zaic. Ferb nam nexder rik
kalt. Phayb rupeg nohs zirpe duns</txt>
	</text>
	<text id=&amp;7&amp;>
		<txt>Foth nar bolhy melk roce toreup. lx
shir dop tulf ursh mil pherk zoth.
Lofit rikk lun raug muiq thau quat
urlk yat duils. Veem ebn jop tarud
thec fobt klab non mer. Zurt derub
ird. Chen ferv yath gorf tach taipe
geuba dequi reet skug falm cheab.
Phed telc dawl dumn wenk voip maf
mewp nals norf. Mex opj denoy zaab </txt>
	</text>
	<text id=&amp;8&amp;>
		<txt>Mora toxib nehe kodl chor sikhe
vut ihoy redok. Quop nitterf phox
lulq pumg surb moch maj liray. Nurt
buj yoban. Waz nekob teel nedan omb
wezar. Snuh bhar difut yoz teac gud
tulg lemap suge quim kelf bemr wac.
ldl faus medl notag zilf kelub. Gej
olb emin covih radip kom kewen ped
atne. Xarf saby tiruc doil pimraz</txt>
	</text>
	<text id=&amp;9&amp;>
		<txt>Himz lolek thund. Kance broh yilk
tich lyp fodom huk reth kipurn tid.
Pozal fuj loug stoph. Jat linap kor
jod brux klab neb artem ebot dorp
ipa Snew hus. Dobert waig zear sib.
Guid sympet lague quez serl guik
claz dow eston. Liwod geb knuch min
borm havil. Nard kwil pomor gafen
furg panex crom dayem frozol beeg</txt>
	</text>
	<text id=&amp;10&amp;>
		<txt>Nage huft koben tive clus prosid
nirp tuil. Quesp puy fron. Guim quoy
bakap joat kurot athur. Julk tain
dus joft kear neel tro nept smoser.
Abic travin raph corab gers fize
mib. Pute queth fet greem gule. Salm
kag miel citad nue. Woch vido belom
mims warg caiz berf nam drexen. Kir
kalt baphy guper sohn periz sund</txt>
	</text>
	<text id=&amp;11&amp;>
		<txt>ruph lin koach vei mund bezy pight reng
thuz rimp corun fid kost eglin tym foll
stix mon suar baz nuil gahul bram tuws
ristor jube kanoy dusen zov elea tarx dac
gumb thur fres eart neg rouf bav sper
tique jewk ghad blube wref lig disa buc
yeth foub dey hopen kavi nelow qik beap
nad cenk brex dari yeber alf birz gipil</txt>
	</text>
	<text id=&amp;12&amp;>
		<txt>nomin gok plad yets blom hin cruz wufit
mip doxon surp lym fow tepik hulg volz
bux foup kric snogh wat quos yox irpal
tus jave bok dunt coloze cep myet rusub
gebid nabe fod varet bicet waul rask spu
tig quech dag efid kav leheb hac tural
exog smid kith lagu jod aven fibel wogat
hegil dax lebay mof kopen zintar gaph</txt>
	</text>
	<text id=&amp;13&amp;>
		<txt>lup stewt kog vagy wiz bots flid rugey
wim hovy cruzin dop exus klic stoy foix
zolt waky sruv bab pugit mon vup slah
ibet wajer com kroz dant teds quoy vad
galo moz frander faw stowz pade ques
bif frag jud dah yex chiz skib tebac keh
ufal gekim bivec jof dwahd deb koil hanj
gaph kegil fot aper jas xandy grez bem</txt>
	</text>
	<text id=&amp;14&amp;>
		<txt>slind vew kigan soid boft cuz varim yox
diz quost mups govin klej hoy friq lupol
boght rux kony tras mawt col sut birax
juny waz kert poad seft heg rast lophe
wame rud nepeb zofat apert graf zye bist
huc jed quisk bek reg siab zitaf cela
bund floh dobe maig vize pof wakel yin
ched bexon kas prid gyph afte ozup mul</txt>
	</text>
	<text id=&amp;15&amp;>
		<txt>gran ker hok molo bose hon lect mot
pok lud pire kol noth frip hul kilp
quar gub kral harb quot quak lirt dulb
bot jut pub tak vode tup sed pon culn
tob duag merc buh whuf yad quat cran
bute fav dio raj dape guz quif gulm wrut
quel sime guick hab klin tubil clev mald
weck mexaf pog cay nage frez girf jemp</txt>
	</text>
	<text id=&amp;16&amp;>
		<txt>Zoper agen yold thos def ber kloph cote
phun lix doup hirg kelm tolf kinos quom
quaz gan juk bory taph kobs pril bunc
sonn bab nej keorn plet fumb tauf nuze
stav houd tere phay gesh dil queft guim
frid buc guad lang seve fach tevig nual
dibe yal noc hoven milb eny wanic lonba
phen club froxie jemp onby traj zumble</txt>
	</text>
	<text id=&amp;17&amp;>
		<txt>volip hoft arn holby kelm croe peurot ix
rish dop tult hurs lim kreph thoz tifol
krik nul guar quim auth quat rulk tay
suild meve neb poj durat cet boft kalb
boc rem turz bured dir ench verf thay
forg chut apite begue quide tere gusk galt
bach dep lect wald mund newk poiv fa
wemp snal fron mex jop yonde baza deg</txt>
	</text>
	<text id=&amp;18&amp;>
		<txt>ol gup arom bixto heen dolk voche hekis
tuv yiho koder quop frittin phox qull gump
burs chom mab yaril trun jub naboy zaw
boken lete naden bom razew nusp ceg vo
tufid zoy cate gud gast parel gues quic
flek remb caw dil sauf delm gaten fliz
bluke jeg bol nime hovic pida mok newek
dep tane frax basy corit loid zavimp olon</txt>
	</text>
	<text id=&amp;19&amp;>
		<txt>mek deur himz lolek thund kance broh yilk
tich lyp fodom huk reth kipurn tiv pozul
fuj loug stoph taw linap vor jod brux klab
artem ebut darp upav snew hus dobert
zear sib guid sympet lagu quez serl vag
quik claz dow eston liwod geb knuch
havil nard kwil pomor gafen feche borm
panex maris dayam frozol beeg klin</txt>
	</text>
	<text id=&amp;20&amp;>
		<txt>vutom gik nage huft koben tive clus osid
nirp tuil quesp puy fron guim quoy pokup
joat kurot athur julk tain vobaz dus bacud
joft kear neel tro ept smoser abic travin
rath corab gers fize dib pute queth fet
greem gule salm kag miel citad nue
vido belom mimz wang calz beaf nax
kir kalt baphy guper sohn periz sund</txt>
	</text>
	<text id=&amp;21&amp;>
		<txt>Hoft arn holbt kelm croe peurot. lx
rish. Dop fult hurs lim kreph thoz
tifol krik nul guar quim. Auth quat
rulk tay suild meve neb poj durat.
Ceth boft kalb non rem turz bured
dir ench verf thay. Forg chat apite.
Bague quide tere gusk malf bache
deph lect. Wald mund newk poiv fam
wemp snal fron. Mex jop yonde baza</txt>
	</text>
	<text id=&amp;22&amp;>
		<txt>Arom bixto. Heen dolk roche hekis
tuv yiho koder quop frittin phox.
Qull gump burs chom maj yaril trun
jub naboy zaw boken lete. Naden bom
razew nush harb tufid. Zoy cate gud
gult. Pamel gues quim flek remb caw.
Dil sauf delm gaton. Fliz bluke jeg
bol nime hovic pidar mok newek dep
tane frax. Basy curit loid zarimp</txt>
	</text>
	<text id=&amp;23&amp;>
		<txt>Gran ker hok molo. Bose hon lect mot
pok lud toh pire kol noth frip hul
kalp. Quar gub kril harb quot quak
lirt. Delb bot jul pob tak dode tup
hed pon culn tob duan merc buh. Whuf
jad quat. Cran bute fav dio raj dape
Guz quif gulm wrut guel Sime guick
hap klib tubil clev. Mald pol weck
mexaf spog cay nage quez girf jemp</txt>
	</text>
	<text id=&amp;24&amp;>
		<txt>Agen yold thos. Def ber kloph cote
phun lix doup hirg kelm tolf kinos.
Quom gan quaz juk bog. Taph kobs pril
benc sonn jen. Keorn daf plet fumb
sur tauf. Nuze stav houd tere phay.
Gesh dil queft guim. Frid buc quad
lang. Sove fach tevip nual dibe yal
noc hoven milb ony wran lian phen.
Clug froxie jemp onby traj zumble</txt>
	</text>
	<text id=&amp;25&amp;>
		<txt>narg rek koh mool sobe ohn cetl motu
pok dul oth riep lok thun pirf uhl pilk
ruaq ugb lork bhar tquo quak rilt bolv
tob tuj bope tak deod upd dex nop nulc
bot uad cerm buh fwuh dabe quat nerz
teub vaf iod arj pade zug quif helg twur
luge mise kicgu dah blik tubil velc dalm
olp kwec fexam pog yac gane quez firg</txt>
	</text>
	<text id=&amp;26&amp;>
		<txt>neag doly soth def reb pholk toce nuph
xil poud hirv kelm folt sinok mouq gan
quaz kuj obq phat sobk lirp cnub nons
ej roke afd telp bumf rus zune tavs
dhoud tee yaph sheg ild queft vig dirf
bc dag galn vose fhac pevit laun ebid
aly onc noveh bilm yon mawr nial neph
faut gulc oxerf pem byon jat blezum gen</txt>
	</text>
	<text id=&amp;27&amp;>
		<txt>mup vez hokil fotok dica kolm boft koof
vint cosk lupol hidrom tren fich yuns orp
paj rogun joz thivar tol kob brak blost
juene yumb neks baft culen medot antar
baid thove pitet guig cegh yarb quez guat
fey lig cehib dague brical sive habed kwin
toef agec plech guidel naxal kove wirn
redak bant frixen jemp phoy bazone.</txt>
	</text>
	<text id=&amp;28&amp;>
		<txt>nux elag ofo jimp tirbil hox cish suldot
quimp velit poz yifur togu quont xul thurp
bast nin boum jary dokab dup rono plad
fude baut mos dafer gurb hoce fain vist
gake foid sepha treg guve quez guiz thra
fesce vima ebab kned betz heul waven
goom jame woegil nohl parik bis hoxed
bedaj tullis mek gimey viden rozel ber</txt>
	</text>
	<text id=&amp;29&amp;>
		<txt>dhoe stil onap cred myf bix moc hez togu
jod helk pyx wrog zil vuf smolt nik ruz
gamp hyb tawp vox sanc quork tuk bisy
baj pazt wrenk tox dof wabs bulst myzu
gand tew bocer fatz gepy bast quck gax
dich rebaf biz jalf deb setch gek chay
hukn mib nep bafil vob chone ply awec
croix ah strel yabe mez goelp noch fipt</txt>
	</text>
	<text id=&amp;30&amp;>
		<txt>jolk whin fleg vos pexy liat bromp duz
yip culn mog dist huk zerp soy wuth nost
figor quap zot bivy whax klom tiput wolk
capy raj stek dozer lub duc fraz mup
sart nadow thef byst zabid yeb chiw epat
duk gleq frud mish yeld thib jum ceval
wonk fing jilk beh axop marf ebel yola
gif hald perd grup bac mits tarf zuber</txt>
	</text>
	<text id=&amp;31&amp;>
		<txt>orf minug afend thox browt gipir kloc
strud jink logh quemp fow nisk tuy kovo
mus gabz thro naw smit crup kruj vox
penk yolar dus meft bawc yar cin devaz
wrot ghas pixe buft heg queb daf razife
fic kalm stey gune dace kich yax lono
apen bivel mich pek wabe oxfor bol
jid cham erd tudy figel nob shuz kafi</txt>
	</text>
	<text id=&amp;32&amp;>
		<txt>pok zid smat jey blume gof chup von
gynt sird pum lohy quext nik floz wuk
cimur stob gast yun habul pom tris dawt
ojer mun vop spek baze ruc quof tues
pach lemb nud gerb fawx chab gofe hyte
perd swiz quib kav mix jarb vey zude
hif seld cag knet malb gen clid flughe
kang voch wod exerk raf nime caly daiz</txt>
	</text>
	<text id=&amp;33&amp;>
		<txt>Hokil fotok dican kolm boft koof
vint cosk lupol hidrom tren. Fich
paj rogun. Joz thivar tol kub brak.
Blost jaune. Yumb neks baft culen
rive medot fage antar baid. Thove
pitet guik yarb. Quez quat fey lig
dague brical sive habod kwin toef.
Plech quidel naxal kove wirn dag.
Bant frixen jemp phoy bazone kelf</txt>
	</text>
	<text id=&amp;34&amp;>
		<txt>Elage ofo tirbil hox. Cish suldot
quimp velit poz hifur togu. Quont
thurp bast nin boum jary dokab. Pud
rone plage fud bant mos dafer gurb.
Fain vist gake froid sephar treg
guve quev guiz thrach fesce. Vima
knep botz houl waven mibold goom
jame. Woegil nohl parik bis hoxed
tullis mek gimey. Viden rozel beur</txt>
	</text>
	<text id=&amp;35&amp;>
		<txt>Narg rek koh mool. Sobe ohn cetl mot.
Pok dul oth riep lok thon pirf. Uhl
palk ruaq ugb lirk bhar tquo. Quak
rilt beld tob tuj bop tak deod upt.
Deh nop nulc bot nuad cerm buh fwuh
daj quat narc teub vaf iod. Arj pade
zug quif mulg twur. Luge mise kicgu
pah blik tubil velv dalm. Olp kwec
fexam pogs yac gane quez firg pemj</txt>
	</text>
	<text id=&amp;36&amp;>
		<txt>Neag doly soth def reb. Pholk toce
nup xil poud ghir kelm folt sinok
mouq. Gan quaz kuj obq phat sobk lirp.
Cneb nons nej roken. Afd telp bumf
rus faut. Zune tavs dhou teer yaph
sheg ild queft muig dirf buc. Daug
galn vose. Fhac pevit laun ebid aly
onc noveh bilm yon. Mawr nial neph
gulc oxierf pemj byon jrat blezum</txt>
	</text>
	<text id=&amp;37&amp;>
		<txt>Hoilk kotof nacid lomk tofb foko
tinv kosc. Lopul mordih nert chif
paj nurog zoj thivar. Olt buk karb
stolb nauje. Bumy sken fabt nulec.
Vire dotem geaf ranat daib voeth
tiept kuig bary quez. Guat yef gil
gaude calirb vies daboh niwk foet
chelp. Delgue laxan voke nirw gad.
Tanb nexirf pemj yoph mozeba felk</txt>
	</text>
	<text id=&amp;38&amp;>
		<txt>Geale foo librit hox sich. Todlus
piqum tilve zop rufih guto. Quont
purth tasb nin moub yarj bakod dup
noer gelap duf tanb som ferad. Brug
naif stiv keag. Droif raphes gret
vuge quev zuig charth scefe. Maiv
penk. Zobt louh nevaw dolbim mogo
maje gilowe. Holn kirap sib dexho
stilul kem megiy nived lezor reub</txt>
	</text>
	<text id=&amp;39&amp;>
		<txt>Lits pano derc fym bix omc zeh. Toug
doj kelh xup gorw zil fuv tloms. lkn
zurt pamg byh pawt vox. Casn quork
fyrz tuc bise baj. Paz wrenk tux dof
sawb. Stulh ym dagn evt cerbo tafz
pegy sbat quach agx chis. Faber zib
falj deb stehc kog yach nekuh. Bim
enp lifab bov neoch lyp cwea. Kirg
roxic ah lerst baye zem plequ chon.</txt>
	</text>
	<text id=&amp;40&amp;>
		<txt>Niwh gelf sov yexp tial promb zud
piy nulc gom stid uhk. Perz yos thuw.
Snat figor quap toz viby. Hawx molk
tepit dow pacy arj kets rezod. Ulb
chud zarf pum. Tras nadow feth styb
dibaz bey wich talp kud gleq durf.
shom. Dely bith muj davec. Koln ginf
kilj ewn paox famr leeb. Ropde igf
dalh loya purg acb stim ratf rebuz</txt>
	</text>
</texts>".Replace("&amp;", "\"");

	#endregion
}