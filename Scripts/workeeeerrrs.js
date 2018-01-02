$(document).ready(function () {
    var received;
    var friend = null;
    var user = JSON.parse(localStorage.getItem("userTemp"));
    var basee = localStorage.getItem("dbres"); 
    var prov = localStorage.getItem("companyCheck");
    var urlWM = "./MongoService.asmx/retAllWorkersFromCollection";
    var urlWR = "./RavenService.asmx/retAllWorkersFromCollectionR";
    var urlTemp = null;

    if (basee === "raven")
    {
        urlTemp = urlWR;
    }
    else
    {
        urlTemp = urlWM;
    }

    getWorkersWithAjax(urlTemp, function (data) {
        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find("string");
        received = JSON.parse($title.text());
        console.log(received);
        console.log(user);
        var k = 0;
        for (var i = 0; i < received.length; i++) {
            var mail = received[i].Email;
            var first = received[i].FirstName;
            var last = received[i].LastName;
            var company = received[i].CompanyName;

            //console.log(JSON.parse(idees));

            if (received[i].Id === user.Id)
                friend = "";
            else
                friend = "Not Friends";

            if (user.Friends !== null) {
                //console.log($title[i]);
                for (var j = 0; j < user.Friends.length; j++) {
                    //var tmp = $title[i].children[0].innerHTML;
                    //console.log(tmp.toString());
                    if (received[i].Id === user.Friends[j])
                        friend = "Friends";

                }
            }               
            

            if (prov === "faulty") {
                $("#fstat").show();
                console.log(friend);
                k = k + 1;
                var table = '<tr><th scope= "row">' + k + '</th>'
                    + '<td>' + first + '</td>'
                    + '<td>' + last + '</td>'
                    + '<td>' + mail + '</td>'
                    + '<td>' + company + '</td>'
                    + '<td>' + friend + '</td>'
                    + '<td><button type="button" class="view btn btn-default" id="addd' + k + '">View</button></td></tr >';
                $("#listing").append(table);
            }
            else
            {
                $("#fstat").hide();
                console.log(friend);
                k = k + 1;
                var table = '<tr><th scope= "row">' + k + '</th>'
                    + '<td>' + first + '</td>'
                    + '<td>' + last + '</td>'
                    + '<td>' + mail + '</td>'
                    + '<td>' + company + '</td>'
                    + '<td><button type="button" class="view btn btn-default" id="addd' + k + '">View</button></td></tr >';
                $("#listing").append(table);
            
            }
                
        }
    });

    $('#listing').on('click', '.view', function () {
        for (var i = 0; i < received.length; i++) {
            if (received[i].Email === $(this).closest("tr")[0].children[3].innerHTML) {
                console.log($(this).closest("tr")[0].children[3].innerHTML);

                if (basee === "raven")
                    localStorage.setItem("workerViewR", received[i].Id);
                else
                {
                    localStorage.setItem("workerView", received[i].Id);
                }
                    
            }
        }
        window.location.assign("./fellowworker.aspx");

    });

    function getWorkersWithAjax(urlFin, fn) {

        $.ajax({
            url: urlFin,
            dataType: "text",
            type: "POST",
            error: function (err) {
                alert("Error", err);
            },
            success: function (data) {
                fn(data);
            }
        });
    }

});