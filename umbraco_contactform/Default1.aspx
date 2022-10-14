<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default1.aspx.cs" Inherits="umbraco_opticianmap.Default1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	 <title>Google Maps Multiple Markers</title> 
  <script src="http://maps.google.com/maps/api/js?sensor=false" 
          type="text/javascript"></script>
</head>
<body>
  <div id="map" style="width: 500px; height: 400px;"></div>

  <script type="text/javascript">
  	var locations = [
      <asp:Literal runat="server" ID="litLocations" />
//		['Bondi Beach', -33.890542, 151.274856, 4],
//      ['Coogee Beach', -33.923036, 151.259052, 5],
//      ['Cronulla Beach', -34.028249, 151.157507, 3],
//      ['Manly Beach', -33.80010128657071, 151.28747820854187, 2],
      ['Maroubra Beach', -33.950198, 151.259302, 1]
    ];

  	var map = new google.maps.Map(document.getElementById('map'), {
  		zoom: 5,
  		center: new google.maps.LatLng(55.700807, 9.547691),
  		mapTypeId: google.maps.MapTypeId.ROADMAP
  	});

  	var infowindow = new google.maps.InfoWindow();

  	var marker, i;

  	for (i = 0; i < locations.length; i++) {
  		marker = new google.maps.Marker({
  			position: new google.maps.LatLng(locations[i][1], locations[i][2]),
  			map: map
  		});

  		google.maps.event.addListener(marker, 'click', (function (marker, i) {
  			return function () {
  				infowindow.setContent(locations[i][0]);
  				infowindow.open(map, marker);
  			}
  		})(marker, i));
  	}
  </script>
</body>
</html>
