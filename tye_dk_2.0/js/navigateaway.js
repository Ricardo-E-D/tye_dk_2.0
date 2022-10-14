//Initial form value
var initialValue = '';
//Form value at page navigate away event
var userValue = '';
//Default navigate away message
var NAVIGATE_AWAY_MESSAGE = "The changes you made will be lost if you navigate away from this page.";
//Flag to track whether onbeforeunload event is fired already. Required for IE only
var onBeforeUnloadFired = false;
//Flag to disable onbeforeunload plugin 
var DISABLE_ONBEFOREUNLOAD_PLUGIN = false;
//Flag whether source Element class causing the firing of OnBeforeUnload event have "nonavigate" class
var IgnoreNavigateForEventSource = false;

//Read initial form values and attach the onbeforeunload event
$(document).ready(function () {

	initialValue = GetFormValues();
	window.onbeforeunload = handleOnBeforeUnload;
	//Detect whether the event source has "nonavigate" class specified
	$("a,input,img").click(function () {
		IgnoreNavigateForEventSource = $(this).hasClass("nonavigate");
	});

});

//Disables navigate away feature
function DisableNavigateAway() {
    DISABLE_ONBEFOREUNLOAD_PLUGIN = true;
}

//Sets navigate away message
function SetNavigateAwayMessage(message) {
    NAVIGATE_AWAY_MESSAGE = message;
}

//Do not show navigate away message for the specified element Id
function IgnoreNavigateAwayFor(elementId) {
    $("#" + elementId).addClass('nonavigate');
}

//Reads control values in the form
function GetFormValues() {
    var formValues = '';
    $.each($('form').serializeArray(), function(i, field) {
        if (field.name != '__EVENTVALIDATION'
        && field.name != '__EVENTTARGET'
        && field.name != '__EVENTARGUMENT'
        && field.name != '__VIEWSTATE'
        && field.name != '__VIEWSTATEENCRYPTED') {

            var inputField = $("[name='" + field.name + "']");
            var displayProperty = $(inputField).css("display");

            // Ignore the form element which have style property display="none"
				// ignore birthday datetimepicker
            if (displayProperty != "none" && field.name.indexOf('dpBirthday') < 0 && field.name.indexOf('dpExpirationDate') < 0) {
                formValues = formValues + "-" + field.name + ":" + field.value;
            }
        }
    });

    //Read the check box element values as these are not returned by the form.serializeArray() Jquery method
    $(':checkbox').each(function() {
        formValues = formValues + "-" + $(this).attr("checked");
    });

    //Read the check box element values as these are not returned by the form.serializeArray() Jquery method
    $(':radio').each(function() {
        formValues = formValues + "-" + $(this).attr("checked");
    });

    //Read the check box element values as these are not returned by the form.serializeArray() Jquery method
    $(':file').each(function() {
        formValues = formValues + "-" + $(this).val();
    });
    return formValues;
}

//Reset the onbeforeunload flag : Required for IE only as IE has a bug of firing the onbeforeunload
//event twice
function ResetOnBeforeUnloadFired() {
    onBeforeUnloadFired = false;
}

//OnBeforeUnload event handler
function handleOnBeforeUnload(event) {

    //Do not show message if plugin is disabled 
    if (DISABLE_ONBEFOREUNLOAD_PLUGIN) return;
    //Execute function if the onbeforeunload not fired already : Required for IE only as IE has a bug of firing the onbeforeunload
    //event twice
    if (!onBeforeUnloadFired) {
        onBeforeUnloadFired = true;

        if (IgnoreNavigateForEventSource) {
            //Reset the flag
            IgnoreNavigateForEventSource = false;
            return;
        }
        
        //Reset the onBeforeUnloadFired flag after a few milliseconds. Meanwhile, display the navigate
        //away message if the initial form value is not same as current form value 

        //Resetting the onBeforeUnloadFired flag after a few milliseconds ensures that if the same event is fired
        //twice (In IE), the flag will be reset by this time and hence the same event handling codes will
        //not be executed
        window.setTimeout("ResetOnBeforeUnloadFired()", 10);

        userValue = GetFormValues();

        //$('body').append($('<div></div><br /><br /><br /><br />').html(initialValue));
        //$('body').append($('<div>' + userValue + '</div>'));

        //Display navigate away message if the initial form value and current form value is not the same
        if (userValue != initialValue) {
            if (NAVIGATE_AWAY_MESSAGE == "") {
                return "The changes you made will be lost if you navigate away from this page.";
            }
            else {
                return NAVIGATE_AWAY_MESSAGE;
            }
        }
    }
}