var timeSpentField;
var scoreField;
var requirementField;
var starttimeField;
var alertEndField;
var alertStartField;
var boolCompletedField;
var attValueField;
var hiscoreField;
var unlockedField;
var logField;
var test;

var score = "";
var level = "";
var seconds = "";
var errors  = "";

function GetStringAtIndex(str, index){
	var string = str.split("|");
	return string[index];
}

function SplitString(str, level, score, seconds, errors){
	var newStr = "";
	newStr = str.replace("[level]", level);
	newStr = newStr.replace("[score]", score);
	newStr = newStr.replace("[seconds]", seconds);
	newStr = newStr.replace("[errors]", errors);
	return newStr;
}

function Time(){
	this.date1 = null; 
	this.date2 = null;
	return this;
} 

Time.prototype.StartTime = function(){
	this.date1 = new Date();
}

Time.prototype.StopTime = function(){
	this.date2 = new Date();
}

Time.prototype.GetInterval = function(){
	return parseInt((this.GetMilliseconds(this.date2) - this.GetMilliseconds(this.date1))/1000);
}

Time.prototype.GetMillisecondsBetween = function(time2){ 
    var betweenTime = this.GetMilliseconds(time2) - this.GetMilliseconds(this.date1); 
    return betweenTime;
} 

Time.prototype.GetMilliseconds = function(time){
	return time.getMilliseconds() + (time.getSeconds() * 1000) + (time.getMinutes() * 1000*60) + (time.getHours() * 1000 * 60 * 60);
}
var now = new Date();
var nowString = now.getFullYear() + "" + now.getMonth + "" + now.getDate() + "" + now.getHours() + "" + now.getMinutes() + "" + now.getSeconds();

var time = new Time();

function pausecomp(Amount)
{
 d = new Date();    //today's date
while (1)
{
  mill=new Date();   // Date Now
  diff = mill-d;     //difference in milliseconds
  if( diff > Amount ) 
  {
	break;
  }
  
}
}



function StartLevel(){
	time.StartTime();
	
	timeSpentField = document.getElementById("timeSpent");
	scoreField = document.getElementById("score");
	requirementField = document.getElementById("requirement");
	startTimeField = document.getElementById("startTime");
	alertEndField = document.getElementById("alertEnd");
	alertStartField = document.getElementById("alertStart");
	boolCompletedField = document.getElementById("boolCompleted");
	attValueField = document.getElementById("attValue");
	hiscoreField = document.getElementById("hiscore");
	startTimeField = nowString;
	errorsField = document.getElementById("errors");
	unlockedField = document.getElementById("unlocked");
	logField = document.getElementById("log");
	test = document.getElementById("test");
}

function StopLevel(){
	time.StopTime();
	document.Form1.timeSpent.value = time.GetInterval() ;
	//alert(time.GetInterval() + "stoplevel");
}