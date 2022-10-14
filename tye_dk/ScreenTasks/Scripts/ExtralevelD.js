var browser = navigator.appName.toLowerCase();
var cars = new Array();
var map1Arr = new Array();
var map = new Array();
var lineNr = 0;
var edgeCrashes = 0;
var totalCrashes = 0;
var start = 0;
var end = 0;
var points = 10000;
var onEdge = 0;

var browseWidth, browseHeight, addWidth, addHeight;

// Definitions
var maxEdgeCrashes = 5;
var maxTotalCrashes = 1;
var roadWidth = 14;
var roadEdgeWidth = roadWidth+9;

function SetVars(){
	cars = new Array();
	map1Arr = new Array();
	map = new Array();
	lineNr = 0;
	edgeCrashes = 0;
	totalCrashes = 0;
	start = 0;
	end = 0;
	points = 10000;
	onEdge = 0;
}

function LoadCars(){
	for(var i = 0; i <= 360; i+=10){
		var img = new Image();
		img.src = "Images/ExtralevelD/Cars/" + i + ".png";
		cars.push(img);
	}
}

//Vector Functions
function GenerateVectorLines(){
	for(var i = 0; i < map.length-2; i++){
		var x = map[i];
		var y = map[++i];
		var x2 = map[++i];
		var y2 = map[++i];
		map1Arr.push(new VectorLine(new Coord(x,y), new Coord(x2,y2)));
		i -= 2;
	}
}

Coord = function (x,y) {
  if(!x) var x=0;
  if(!y) var y=0;
  return {x: x, y: y};
}

Vector = function(x,y){
	this.x = x;
	this.y = y;
	this.length = CalcLength(this.x, this.y);
}

VectorLine = function(coord1, coord2){
	this.coord1 = coord1;
	this.coord2 = coord2;
	this.vX = (coord2.x - coord1.x);
	this.vY = (coord2.y - coord1.y);
	this.vector = new Vector(this.vX, this.vY);
	this.id = 1;
	this.length = CalcLength(this.vX, this.vY);
}

function GetProjectionVector(vectorLine, P0){
	 var PP0 = new Vector((P0.x-vectorLine.coord1.x), P0.y-vectorLine.coord1.y);
	 var c = (((vectorLine.vX * PP0.x) + (vectorLine.vY * PP0.y)) / Math.pow(CalcLength(vectorLine.vX, vectorLine.vY),2))
	 return new Vector(parseInt((vectorLine.vX*c)), parseInt((vectorLine.vY*c)));
}

function CalcLength(vX, vY){
	return Math.sqrt(Math.pow(vX,2) + Math.pow(vY,2));
}

function CompareSigns(vector1, vector2){
	var x = false;
	var y = false;
	if(((vector1.x >= 0) && (vector2.x >= 0)) || ((vector1.x <= 0) && (vector2.x <= 0))){
		x = true;
	}
	if(((vector1.y >= 0) && (vector2.y >= 0)) || ((vector1.y <= 0) && (vector2.y <= 0))){
		y = true;
	}
	if(x == true && y == true){
		if(vector1.length < vector2.length){
			return true;
		} else {
			return false;
		}
	} else {
		return false;
	}
}

function ProjectionDistance(vector1, vector2){
	var directX = (vector1.x / vector1.length);
	var directY = (vector1.y / vector1.length);
	var a = (directX * vector2.x) + (vector2.y * directY);	
	var b = vector2.length;
	var v = Math.acos((a/b));

	var dist = b * Math.sin(v);
	if(isNaN(dist)){
		dist = 1;
	}
	return parseInt(dist);
}

function CheckPositionOnTrack(screenCoord){
	if(start == 1){
		StartLevel();

	}
	if(lineNr == (map1Arr.length-1)){
		StopLevel();
		var scoreF = document.getElementById("score");
		scoreF.value = points - (time.GetInterval()*25) - (edgeCrashes*100);
		errorsField.value = edgeCrashes;
		end = 1;
		
		var interval = time.GetInterval();
		seconds = interval;
		score = scoreF.value;
		errors = edgeCrashes;
		if(parseInt(score) < 0) {
			score = 0;
			scoreF.value = 0;
		}
		var hscore = GetStringAtIndex(hiscoreField.value, 0);
		var alertStr  = SplitString(alertEndField.value,level,score,seconds,errors);
		if(parseInt(hscore) < score){
			alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 1);
		} else {
			alertStr += "\n" + GetStringAtIndex(hiscoreField.value, 2);
		}
		DetectScreenSize();
		alert(alertStr);
		document.Form1.submit();
		pausecomp(500);
	}
	
	var car = document.getElementById("car");
	var v = map1Arr[lineNr];
	var vNext = map1Arr[lineNr+1];
	var vBefore = map1Arr[lineNr-1];
	
	var pVector = GetProjectionVector(v, new Coord(screenCoord.x, screenCoord.y));
	var vectorLine = new VectorLine(new Coord(v.coord1.x, v.coord1.y), new Coord(screenCoord.x, screenCoord.y));
	
	if(CompareSigns(pVector, v.vector) == true){
		var dist = ProjectionDistance(v.vector, vectorLine.vector);
		var ang = GetAngle(v.vector, new Vector(10,0));			
		if(dist <= roadWidth){
			car.src = GetCarByAngle(v.vector, ang);
			car.style.left = screenCoord.x-12+addWidth;
			car.style.top = screenCoord.y-12+addHeight;
			start += 1;
			onEdge = 0;
		} else if (dist <= roadEdgeWidth) {
			if(onEdge == 0){
				onEdge = 1;
			} else {
				car.src = GetCarByAngle(v.vector, ang);
				car.style.left = screenCoord.x-12+addWidth;
				car.style.top = screenCoord.y-12+addHeight;
				//start = 1;
				edgeCrashes += 1;		
			}
		} else {
			if(start >= 1){
				totalCrashes += 1;
				end = 1;
			}
		}
	} else if (lineNr != (map1Arr.length-1) && CompareSigns(GetProjectionVector(map1Arr[(lineNr+1)], new Coord(screenCoord.x, screenCoord.y)), vNext.vector) == true){
		lineNr += 1;
		CheckPositionOnTrack(screenCoord);
		
	} else if (lineNr != 0 && CompareSigns(GetProjectionVector(map1Arr[(lineNr-1)],new Coord(screenCoord.x, screenCoord.y)), vBefore.vector) == true){
		lineNr -= 1;
		CheckPositionOnTrack(screenCoord);
	} 
}

function GetAngle(vector1, vector2){
	var a = (vector1.x*vector2.x) + (vector1.y *vector2.y);
	var b = vector1.length * vector2.length;
	
	var angle = Math.acos(a/b);
	return angle*(360/(Math.PI*2));
}

function GetCarByAngle(vector, angle){
	var ang = parseInt(angle);
	ang = ang - (ang % 10);
	var angIndex = ang/10;
	
	if(vector.x >= 0 && vector.y >= 0){
		return cars[angIndex].src;
	} else if (vector.x <= 0 && vector.y >= 0){
		return cars[angIndex].src;
	} else if (vector.x <= 0 && vector.y <= 0){
		return cars[36-angIndex].src;
	} else if (vector.x >= 0 && vector.y <= 0){
		return cars[36-angIndex].src;
	}
}

if(browser.indexOf("netscape") != -1) document.captureEvents(Event.MOUSEMOVE);

//Step 2: Create the reacting function
function processCoordinate(e){
	var el = document.getElementById("container");
	//el.innerHTML += "1";
	var posx = 0;
	var posy = 0;
	if (!e) var e = window.event;
	if (e.pageX || e.pageY)
	{
		posx = e.pageX;
		posy = e.pageY;
	}
	else if (e.clientX || e.clientY)
	{
		posx = e.clientX + document.body.scrollLeft;
		posy = e.clientY + document.body.scrollTop;
		
	}
	if(end != 1){	
		CheckPositionOnTrack(new Coord(posx-addWidth,posy-addHeight));
	} else {
		if(start >= 1 && totalCrashes == 1){
			var tempimg = document.getElementById("car");
			tempimg.src = "Images/ExtralevelD/Cars/explode.png";		
			alert(alertStartField.value);
			start = 0;
			totalCrashes = 0;
			edgeCrashes = 0;
			
			scoreField.value = "!";
			DetectScreenSize();
			document.location.href = "ExtralevelD.aspx?id=" + GetName("id") + "&intExerciseIdNo=" + document.getElementById("exNo").value;
		}
	}
}

//Step 3: Hook the two up
window.document.onmousemove = processCoordinate

function setMap(){
	map	= new Array();
	var number = document.getElementById("mapNum");
	if(number.value == "map1a"){
		var map1a = new Array(63,33,91,71,126,105,163,137,201,158,251,176,306,179,354,173,413,148,440,124,454,98,448,71,426,44,391,39,376,54,366,83,367,118,375,154,390,195,413,248,424,280,424,313,416,325,398,333,330,330,242,312,203,295,180,278,146,244,112,219,85,214,58,220,40,233,40,252,54,288,78,320,96,333,120,333,160,298,200,230,230,156,253,92,258,64,272,21);
		map = map1a;
	} else if (number.value == "map1b"){
		map = new Array(63,33,91,71,126,105,163,137,201,158,251,176,306,179,354,173,413,148,440,124,454,98,448,71,426,44,391,39,376,54,366,83,367,118,375,154,390,195,413,248,424,280,424,313,416,325,398,333,330,330,242,312,203,295,180,278,146,244,112,219,85,214,58,220,40,233,40,252,54,288,78,320,96,333,120,333,160,298,200,230,230,156,253,92,258,64,272,21);
	} else if (number.value == "map2a"){
		map = new Array(84,47,126,79,154,92,202,94,287,69,350,41,389,37,418,56,424,81,413,119,385,159,352,191,319,209,280,220,253,231,234,261,226,303,232,335,252,340,287,332,338,296,358,268,346,227,330,190,314,170,289,156,251,150,226,152,188,167,164,189,156,218,157,266,154,290,136,306,104,309,78,299,67,275,69,242,85,202,123,142,148,107,171,69,193,20);
	} else if (number.value == "map2b"){
		map = new Array(84,47,126,79,154,92,202,94,287,69,350,41,389,37,418,56,424,81,413,119,385,159,352,191,319,209,280,220,253,231,234,261,226,303,232,335,252,340,287,332,338,296,358,268,346,227,330,190,314,170,289,156,251,150,226,152,188,167,164,189,156,218,157,266,154,290,136,306,104,309,78,299,67,275,69,242,85,202,123,142,148,107,171,69,193,20);
	} else if (number.value == "map3a"){
		map = new Array(88,59,128,73,143,99,148,124,145,160,131,194,108,228,80,256,72,276,82,297,97,315,111,318,135,305,166,277,192,240,216,204,262,136,301,89,333,61,363,45,389,47,403,56,401,75,365,119,320,162,279,204,275,217,279,246,309,295,343,319,362,324,386,317,413,294,416,253,398,219,374,188,347,157,289,103,268,86,219,46);
	}  else if (number.value == "map3b"){
		map = new Array(95,62,124,71,144,100,147,148,133,194,92,242,75,275,82,296,99,315,127,311,157,285,194,238,239,170,276,117,315,74,350,50,376,45,396,50,404,63,394,86,361,126,320,162,286,198,276,213,279,243,297,278,326,308,365,322,400,307,417,276,408,238,385,204,329,145,289,103,263,83,220,48);
	}  else if (number.value == "map4a"){
		map = new Array(89,56,115,84,143,110,177,138,217,162,257,180,311,194,349,206,397,226,415,252,409,274,385,300,359,308,333,302,315,270,313,224,337,186,377,130,385,86,371,60,329,48,255,48,199,58,151,78,121,98,97,134,81,164,75,210,75,250,83,282,95,308,109,314,141,300,179,258,211,212,247,154,255,142,279,98);
	}  else if (number.value == "map4b"){
		map = new Array(89,56,115,84,143,110,177,138,217,162,257,180,311,194,349,206,397,226,415,252,409,274,385,300,359,308,333,302,315,270,313,224,337,186,377,130,385,86,371,60,329,48,255,48,199,58,151,78,121,98,97,134,81,164,75,210,75,250,83,282,95,308,109,314,141,300,179,258,211,212,247,154,255,142,279,98);
	} else if (number.value == "map5a"){
		map = new Array(76,48,125,80,170,97,209,101,250,94,314,66,353,39,382,34,398,40,408,52,407,73,392,94,367,113,315,132,271,146,229,154,173,169,138,185,111,203,96,228,94,264,97,288,106,306,129,314,173,303,234,269,282,237,315,215,350,198,382,196,404,209,415,231,419,269,414,305,402,319,378,322,346,297,319,263,299,232,285,198,260,131,247,84,231,24);
	} else if (number.value == "map5b"){
		map = new Array(80,49,116,75,156,93,196,101,232,99,288,77,352,43,366,35,388,35,406,45,410,61,400,85,378,107,336,127,284,143,202,163,158,177,124,193,100,219,94,271,102,301,118,315,176,301,238,267,298,223,340,201,370,197,404,207,418,247,414,303,396,325,380,321,346,299,312,253,290,215,260,131,244,79,230,33);
	}
	var mapElem = document.getElementById("maps");
	//var number = document.getElementById("mapNum");
	if(number.value.indexOf("b") != -1){
		roadWidth = 9;
		roadEdgeWidth = 18;
	}
	
	//eval("map = " + number.value);
	eval("mapElem.src = \"Images/ExtralevelD/" + number.value + ".png\"");
}

function DetectScreenSize(){
	SetVars();
	if (browser.indexOf("netscape") != -1){
		browseWidth = window.innerWidth;
		browseHeight = window.innerHeight;
	} else {
		browseWidth = document.body.scrollWidth;
		browseHeight = document.body.scrollHeight;
	}
	addWidth = (browseWidth / 2) - 245;
	addHeight = (browseHeight / 2) - 181;
	
	var mapElem = document.getElementById("maps");
	mapElem.style.left = addWidth;
	mapElem.style.top = addHeight;
	setMap();
	LoadCars();
	GenerateVectorLines();
}
