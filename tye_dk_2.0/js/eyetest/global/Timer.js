/***************************************************************** 

    Timer javascript class by Algorithm - Copyright (c) 2003 
    Website: http://www.codingforums.com/showthread.php?s=&threadid=10531 

    Main Features: 
    -    Non-conflicting OO-based timeouts. Uses only 1 global array 
    -    Allows calling of local object methods 

    Compatibility: 
    -    Tested on IE6 and Mozilla 1.1 
    -    DOM compliant browsers only 

    Note: This document was created with a tab-spacing of four (4) 

******************************************************************/ 

// The constructor should be called with 
// the parent object (optional, defaults to window). 

function Timer(){ 
    this.obj = (arguments.length)?arguments[0]:window; 
    return this; 
} 

// The set functions should be called with: 
// - The global name of the function (as a string) (required) 
// - The millisecond delay (required) 
// - Any number of extra arguments, which will all be 
//   passed to the function when it is evaluated. 

Timer.prototype.setInterval = function(func, msec){ 
    var i = Timer.getNew(); 
    var t = Timer.buildCall(this.obj, i, arguments); 
    Timer.set[i].timer = window.setInterval(t,msec); 
    return i; 
} 
Timer.prototype.setTimeout = function(func, msec){ 
    var i = Timer.getNew(); 
    Timer.buildCall(this.obj, i, arguments); 
    Timer.set[i].timer = window.setTimeout("Timer.callOnce("+i+");",msec); 
    return i; 
} 

// The clear functions should be called with 
// the return value from the equivalent set function. 

Timer.prototype.clearInterval = function(i){ 
    if(!Timer.set[i]) return; 
    window.clearInterval(Timer.set[i].timer); 
    Timer.set[i] = null; 
} 
Timer.prototype.clearTimeout = function(i){ 
    if(!Timer.set[i]) return; 
    window.clearTimeout(Timer.set[i].timer); 
    Timer.set[i] = null; 
} 

// Private data 

Timer.set = new Array(); 
Timer.buildCall = function(obj, i, args){ 
    var j, t=""; 
    Timer.set[i] = Timer.getData(obj); 
    if(obj != window){ 
        t = "Timer.set["+i+"].obj."; 
    } 
    t = t + args[0] + "("; 
    for(j=0; (j+2)<args.length; j++){ 
        Timer.set[i][j] = args[j+2]; 
        if(j>0) t = t + ", "; 
        t = t + "Timer.set["+i+"]["+j+"]"; 
    } 
    t = t + ");"; 
    Timer.set[i].call = t; 
    return t; 
} 
Timer.callOnce = function(i){ 
    if(!Timer.set[i]) return; 
    eval(Timer.set[i].call); 
    Timer.set[i] = null; 
} 
Timer.getData = function(obj){ 
    var data = new Array(); 
    data.obj = obj; 
    data.call = "void(0);"; 
    data.timer = null; 
    return data; 
} 
Timer.getNew = function(){ 
    var i = 0; 
    while(Timer.set[i]) i++; 
    return i; 
} 