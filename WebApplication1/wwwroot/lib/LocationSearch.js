$(document).ready(function () {
    document.getElementById("Search").onclick = function () { getLocations() };
});
function getLocations() {
    document.getElementById("overlay").style.display = "block";
    var gLatitude = document.getElementById("txtLatitude").value;
    var gLongitude = document.getElementById("txtLongitude").value;
    var maxDistance = document.getElementById("maxDistance").value;
    var maxResults = document.getElementById("maxResults").value;
    var sUrl = '/Home/findLocation?Latitude=' + gLatitude + '&Longitude=' + gLongitude + '&maxDistance=' + maxDistance + '&maxResults=' + maxResults;
    
    jQuery.ajax({
        type: 'GET',
        url: sUrl,
        dataType: 'json',
        success: function (data) {
            if (data.allLocations == null) {
                alert("No Location Found");
            }
            else {
                for (i = 0; i < data.allLocations.length; i++) {

                    var name = data.allLocations[i]["Name"];
                    var longitude = data.allLocations[i]["Longitude"];
                    var latitude = data.allLocations[i]["Latitude"];
                    delete data.allLocations[i]["distance"];

                }
                document.getElementById("jsonOutput").innerHTML = JSON.stringify(data.allLocations, null, 4);
                document.getElementById("rDuration").value = data.timeToSearch;
                document.getElementById("fileDuration").value = data.timeToRead;
                document.getElementById("rLatitude").value = data.latitude;
                document.getElementById("rLongitude").value = data.longitude;
                document.getElementById("rDistance").value = data.maxDistance;
                document.getElementById("rMax").value = data.maxResults;
            }
            document.getElementById("overlay").style.display = "none";
            document.getElementById("json").style.display = "block";
            document.getElementById("tTimes").style.display = "block";

        },

        error: function (msg, url, line) {
            alert('error trapped in error: function(msg, url, line)');
            alert('msg = ' + msg.status + ', url = ' + url + ', line = ' + line);

        }
    });

}