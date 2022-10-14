var browser = navigator.appName.toLowerCase();
var level = 0;
var score = 0;

function init(){
	var img = document.getElementById("img3D");
	var rand = parseInt((Math.random()*2));
	img.src = "Images/3DLevel1/" + images[0][rand];
	//alert(rand);
}

function DisplayImage(str){
	if(level < 40){
		var scoreField = document.getElementById("scoreField");
		var img = document.getElementById("img3D");
		alert(img.src);
		if(img.src.indexOf("-" + str) != -1) {
			var rand = parseInt((Math.random()*2));
			level++;
			img.src = "Images/3DLevel1/" + images[level][rand];
			score += 5;
			scoreField.innerHTML = "Score: " + score;
		} else {
			if(score == 0){
				level = 0;
				score = 0;
				init();
			} else {
				score -= 5;
			}
			scoreField.innerHTML = "Score: " + score;
		}
	} else {
		//Do log stuff
	}
	
}


if(browser.indexOf("netscape") != -1) document.captureEvents(Event.KEYDOWN);
//Step 2: Create the reacting function
function processkey(e){
	var Key
	
	if (browser.indexOf("netscape") != -1){
		Key = String.fromCharCode(e.which);
	} else{
        Key = String.fromCharCode(window.event.keyCode);
        
	}
	if(Key == "1" || Key == "2" || Key == "3" || Key == "4" || Key == "5" || Key == "6"){
		DisplayImage(Key);
	}
}
//Step 3: Hook the two up
document.onkeypress = processkey
