function GetName(varname){
  var url = window.location.href;

  // Next, split the url by the ?
  var qparts = url.split("?");

  // Check that there is a querystring, return "" if not
  if (qparts.length == 0)
  {
    return "";
  }

  // Then find the querystring, everything after the ?
  var query = qparts[1];

  // Split the query string into variables (separates by &s)
  var vars = query.split("&");
  var value = "";

  // Iterate through vars, checking each one for varname
  for (var i=0;i<vars.length;i++)
  {
    // Split the variable by =, which splits name and value
    var parts = vars[i].split("=");
    
    // Check if the correct variable
    if (parts[0] == varname)
    {
      // Load value into variable
      value = parts[1];

      // End the loop
      break;
    }
  }
  
  // Convert escape code
  value = unescape(value);

  // Convert "+"s to " "s


  // Return the value
  return   value.replace(/\+/g," ");;
}

