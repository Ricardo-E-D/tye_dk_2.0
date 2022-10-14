
var interval = null;
var coordinates = new Array();
var index1 = 0;
var htm = "";
var blnDirectionForward = true;
var intMoveInterval = 15;
var eyeElem = null,
	 testElem = null,
	 aimElem = null;


var vCoords = new Array(
						//   start x, start y, midpoint x, midpoint y , , end x, end y
						new Array(50,	50,	150,	100,	0,		500,	100,	500),
						new Array(100,	500,	200,	500,	150,	50,	200,	50),
						new Array(200,	50,	300,	100,	0,		500,	250,	500),
						new Array(250,	500,	400,	500,	150,	500,	300,	50),
						new Array(300,	50,	500,	500,	150,	500,	350,	500)
					);

var hCoords = new Array(new Array(50, 50, 50, 100, 700, 50, 700, 100),
						new Array(700, 100, 700, 200, 50, 100, 50, 150));

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
image.src = "/img/eyetest/followTheEye/spacer.png";

function GetCoordsToAnimate(arr, xMultiplier, yMultiplier, multiplier, step, screenFactor){
	var coordinates = new Array(), html;
	
	eyeElem = document.getElementById("eye");
	testElem = document.getElementById("test");
	aimElem = document.getElementById("aim");

	for(var k = 0; k <= multiplier; k++){
		for(var j = 0; j < arr.length; j++){
			var bezierCoord,
				bezierCoord2,
				tempArr = new Array(),
				C1 = new coord((arr[j][0]+(xMultiplier*k))*screenFactor, (arr[j][1]+(yMultiplier*k))*screenFactor),
				C2 = new coord((arr[j][2]+(xMultiplier*k))*screenFactor, (arr[j][3]+(yMultiplier*k))*screenFactor),
				C3 = new coord((arr[j][4]+(xMultiplier*k))*screenFactor, (arr[j][5]+(yMultiplier*k))*screenFactor),
				C4 = new coord((arr[j][6]+(xMultiplier*k))*screenFactor, (arr[j][7]+(yMultiplier*k))*screenFactor);
			
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

function Stop(){
	window.clearInterval(interval);
	//StopLevel();
	seconds = timeSpentField.value;
	var alertStr  = SplitString(alertEndField.value,level,score,seconds,errors);
	alert(alertStr);
	document.Form1.submit();
}

function MoveTo(){
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

function AnimateCurve(type) {
	var steps = 180, screenfactor = 1;
	
	eyeTestScreen.stop();
	eyeTestScreen.start();

	window.clearInterval(interval);
	index1 = 0;
	//	logField = document.getElementById("log");
	//	logField.value = type;
	$('#hidAttribValue').val(type);

	if(type == "random"){
		coordinates = GetCoordsToAnimate(rCoords, 0, 0, 0, steps, screenfactor);	
	} else if(type == "horizontal"){
		coordinates = GetCoordsToAnimate(hCoords, 150, 0, 4, steps, screenfactor);
		//logField.value = "vertikal";
	} else if(type == "vertical"){
		coordinates = GetCoordsToAnimate(vCoords, 0, 100, 4, steps, screenfactor);
		//logField.value = "horisontal";
	} else if(type == "circular"){
		coordinates = GetCoordsToAnimate(cCoords, 0, 0, 0, steps, screenfactor);
	} else if(type == "diagonal"){
		coordinates = GetCoordsToAnimate(sCoords, 0, 0, 0, steps, screenfactor);
	}

	interval = window.setInterval(function () { MoveTo(); }, intMoveInterval);
	if (!init) {
		eyeInit();
		init = true;
	}
}

var init = false;
function eyeInit() {
	$('#eye').css('border', '1px solid #ffffff');
	var container = $('#eye').parent();
	for(var i = 0, len = vCoords.length; i < len; i++) {
		
		var dot42 = $('<div>&nbsp;</div>');
		container.append(dot42);
		dot42.css({ position: 'absolute', left: vCoords[i][0], top: vCoords[i][1], width: '2px', height: '2px', backgroundColor: '#ffffff' });

		var dot43 = $('<div>&nbsp;</div>');
		container.append(dot43);
		dot43.css({ position: 'absolute', left: vCoords[i][6], top: vCoords[i][7], width: '2px', height: '2px', backgroundColor: '#ffffff' });

	}
}