var vCoords = new Array(new Array(50,50,50,100,700,50,700,100),
						new Array(700,100,700,200,50,100,50,150));

var hCoords = new Array(new Array(50,50,150,100,0,500,100,500),
						new Array(100,500,200,500,150,50,200,50));

var cCoords = new Array(new Array(100,100,400,0,700,300,500,500),
						new Array(500,500,200,600,0,400,200,200),
						new Array(200,200,300,100,600,0,600,200),
						new Array(600,200,500,400,200,400,250,250),
						new Array(250,250,300,200,400,300,350,450));
						
var rCoords = new Array(new Array(50,50,0,0,500,50,600,100),
						new Array(600,100,600,150,150,500,50,450),
						new Array(50,450,50,300,50,150,300,100),
						new Array(300,100,600,150,600,550,500,450),
						new Array(500,450,400,400,50,200,100,100),
						new Array(100,100,300,100,400,500,400,500));
						
var sCoords = new Array(new Array(50,50,50,50,500,500,500,500),
						new Array(500,500,500,500,150,150,150,150),
						new Array(150,150,150,150,500,50,500,50),
						new Array(500,50,500,50,50,500,50,500),
						new Array(50,500,50,500,400,100,400,100),
						new Array(400,100,400,100,50,50,50,50));

var image = new Image();
image.src = "Images/Level1A/spacer.png";


function GetCoordsToAnimate(arr, xMultiplier, yMultiplier, multiplier, step, screenFactor){
	//	var para = document.getElementById("test");
	var coordinates = new Array();
	var html = "";

	for(var k = 0; k <= multiplier; k++){
		for(var j = 0; j < arr.length; j++){
			var tempArr = new Array();
			var C1 = new coord((arr[j][0]+(xMultiplier*k))*screenFactor, (arr[j][1]+(yMultiplier*k))*screenFactor);
			var C2 = new coord((arr[j][2]+(xMultiplier*k))*screenFactor, (arr[j][3]+(yMultiplier*k))*screenFactor);
			var C3 = new coord((arr[j][4]+(xMultiplier*k))*screenFactor, (arr[j][5]+(yMultiplier*k))*screenFactor);
			var C4 = new coord((arr[j][6]+(xMultiplier*k))*screenFactor, (arr[j][7]+(yMultiplier*k))*screenFactor);
			
			var bezierCoord;
			var bezierCoord2;

			for(var i = 0; i < step; i++){		
				bezierCoord = getBezier((i/step), C1, C2, C3, C4); 
				tempArr.push(new coord(bezierCoord.x, bezierCoord.y));
			}
			
			while(tempArr.length > 0){
				coordinates.push(tempArr.pop());
			}
		}
	}
	return coordinates;
}

var interval = null;
var coordinates = new Array();
var index1 = 0;
var htm = "";
var blnDirectionForward = true;
var intMoveInterval = 35;

function Stop(){
	window.clearInterval(interval);
	StopLevel();
	seconds = timeSpentField.value;
	
	var alertStr  = SplitString(alertEndField.value,level,score,seconds,errors);
	
	alert(alertStr);
	document.Form1.submit();
}

function MoveTo(){
	var eyeElem = document.getElementById("eye");
	var testElem = document.getElementById("test");
	var aimElem = document.getElementById("aim");
	if(blnDirectionForward) {
		if(index1 < coordinates.length){
			var coord = coordinates[index1];
			eyeElem.style.left = parseInt(coord.x) + "px";
			eyeElem.style.top = parseInt(coord.y) + "px";
			index1+=1;
		} else {
			index1 = coordinates.length - 1;
			blnDirectionForward = !blnDirectionForward;
		}
	} else {
		if(index1 > 0){
			var coord = coordinates[index1];
			eyeElem.style.left = parseInt(coord.x) + "px";
			eyeElem.style.top = parseInt(coord.y) + "px";
			index1-=1;
		} else {
			index1 = 0;
			blnDirectionForward = !blnDirectionForward;
		}
	}
}

function AnimateCurve(type){
	window.clearInterval(interval);
	index1 = 0;
	logField = document.getElementById("log");
	logField.value = type;
	if(type == "random"){
		coordinates = GetCoordsToAnimate(rCoords, 0, 0, 0, 150, 1);	
		interval = window.setInterval("MoveTo()", intMoveInterval);
	} else if(type == "horizontal"){
		coordinates = GetCoordsToAnimate(hCoords, 150, 0, 4, 150, 1);
		logField.value = "vertikal";
		interval = window.setInterval("MoveTo()", intMoveInterval);
	} else if(type == "vertical"){
		coordinates = GetCoordsToAnimate(vCoords, 0, 100, 4, 150, 1);
		logField.value = "horisontal";
		interval = window.setInterval("MoveTo()", intMoveInterval);
	} else if(type == "circular"){
		coordinates = GetCoordsToAnimate(cCoords, 0, 0, 0, 150, 1);
		interval = window.setInterval("MoveTo()", intMoveInterval);
	} else if(type == "diverse"){
		coordinates = GetCoordsToAnimate(sCoords, 0, 0, 0, 150, 1);
		interval = window.setInterval("MoveTo()", intMoveInterval);
	}
}