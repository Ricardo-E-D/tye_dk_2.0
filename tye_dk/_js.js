// JScript File
var blnAllDetailsDivVisible = true;

function g(v) { return document.getElementById(v); }

function colorizeLink(obj, className, tagName, type) {
	var arrAll = document.getElementsByTagName(tagName);
	var len = arrAll.length;
	for(var i = 0; i < len; i++) {
		if(arrAll[i].className == className) {
			switch(type) {
				case "bold":
					arrAll[i].style.fontWeight = "normal";
				break;
			}
		}
	}
	switch(type) {
		case "bold":
			obj.style.fontWeight = "bold";
			obj.style.color = "#00f";
		break;
	}
}

function switchDiv(divOn, divClassName) {
	var arrDiv = document.getElementsByTagName("div");
	for(var i = 0, len = arrDiv.length; i < len; i++) {
		if(arrDiv[i].className == divClassName) {
			arrDiv[i].style.display = "none";
		}
	}
	if(g(divOn))
		g(divOn).style.display = "block";
}

function hoverRow(element, mode, ignorePointer) {
	if(mode == 'in') {
		cursortype = (ignorePointer ? "default" : "pointer");
		colorval = '#C7DCEA';
	} else if(mode == 'out') {
		cursortype = (ignorePointer ? "default" : "pointer");
		colorval = '#B2CDDF';
	}
	element.style.background = colorval;
	element.style.cursor = cursortype;
}

function do_check_price(obj, rounding) {
    var str = obj.value;
    var num = new Number();
    if (str.length == 0) {
        obj.value = '0,00';
        return(true);
    }
    else {
        str = str.replace(/,/, '.');
        num = parseFloat(str);
        if (isNaN(num)) {
            obj.value = '0,00';
            alert('Feltet skal indeholde et tal.');
            obj.select();
            obj.focus();
            return(false);
        }
        else {
            num = Math.round(100*num) / 100.0;
            if (rounding) {
            num = Math.ceil(num*4) / 4.0;
        }
        obj.value = format_number(num, 2);
        return(true);
       }
    }
}

function do_check_price_max(obj, rounding, max) {
    var str = obj.value;
    var num = new Number();
    if (str.length == 0) {
        obj.value = '0,00';
        return(true);
    }
    else {
        str = str.replace(/,/, '.');
        num = parseFloat(str);
        if (isNaN(num)) {
            obj.value = '0,00';
            alert('Feltet skal indeholde et tal.');
            obj.select();
            obj.focus();
            return(false);
        }
        else {
            num = Math.round(100*num) / 100.0;
            if (rounding) {
            num = Math.ceil(num*4) / 4.0;
        }
        if(parseInt(num) > parseInt(max)) { num = parseInt(max); }
        obj.value = format_number(num, 2);
        return(true);
       }
    }
}
function do_check_price_max_x(obj, rounding, max) {
    var str = obj.value;
    var num = new Number();
    if(str=="x"||str=="X") { return(true); }
    if (str.length == 0) {
        obj.value = '0,00';
        return(true);
    }
    else {
        str = str.replace(/,/, '.');
        num = parseFloat(str);
        if (isNaN(num)) {
            obj.value = '0,00';
            alert('Feltet skal indeholde et tal.');
            obj.select();
            obj.focus();
            return(false);
        }
        else {
            num = Math.round(100*num) / 100.0;
            if (rounding) {
            num = Math.ceil(num*4) / 4.0;
        }
        if(parseInt(num) > parseInt(max)) { num = parseInt(max); }
        obj.value = format_number(num, 2);
        return(true);
       }
    }
}

function format_number(expr, decplaces) {
    var str = '' + Math.round(eval(expr) * Math.pow(10,decplaces));
	while (str.length <= decplaces) {
	    str = '0' + str;
	}
	var decpoint = str.length - decplaces;
	return(str.substring(0,decpoint) + ',' + str.substring(decpoint, str.length));
}

function toggleDetailsDiv(intIdNo) {
	if(g('divLogDetails' + intIdNo)) {
		var obj = g('divLogDetails' + intIdNo);
		obj.style.display = (obj.style.display != "none" ? "none" : "block");
		if(g('divLogDetailsShort' + intIdNo))
			g('divLogDetailsShort' + intIdNo).style.display = (obj.style.display == "none" ? "block" : "none");
	}
}

function toggleAllDetailsDiv() {
	var arrDiv = document.getElementsByTagName("div");
	for(var i = 0, len = arrDiv.length; i < len; i++) {
		if(arrDiv[i].className == "divLogDetails") {
			arrDiv[i].style.display = (blnAllDetailsDivVisible ? "none" : "block");  
			var intLogDivId = arrDiv[i].id.replace("divLogDetails", "");
			if(g('divLogDetailsShort' + intLogDivId))
				g('divLogDetailsShort' + intLogDivId).style.display = (blnAllDetailsDivVisible ? "block" : "none");
		}
	}
	blnAllDetailsDivVisible = !blnAllDetailsDivVisible;
}

function sureDelete(msg) {
	switch(msg) {
		case "equipment":
			var rValue = confirm("Er du sikker på, at du vil slette dette udstyr? Handlingen vil slette alt tilhørende data inkl. billeder!");
			return rValue;
			break;
		case "equipmentItem":
			var rValue = confirm("Er du sikker på, at du vil slette dette udstyr?");
			return rValue;
			break;
		case "equipmentImage":
			var rValue = confirm("Er du sikker på, at du ville dette billede?");
			return rValue;
			break;
		default:
			alert("no msg defined!");
	}
}

var strLastColorizeTableRowBackgroundColor = "";

function colorizeTableRow(blnIn, obj) {
	var blnContinue = true;
	while(obj.parentNode && blnContinue) {
		obj = obj.parentNode;
		if(obj.tagName.toLowerCase() == "tr") {
			blnContinue = false;
		}
	}
	if(blnIn) {
		strLastColorizeTableRowBackgroundColor = obj.className;
		obj.className = "tr_hightligt";
	}
	else {
		obj.className = strLastColorizeTableRowBackgroundColor;
	}
}