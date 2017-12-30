$(document).ready(function () {
    var received;
    var friend = null;
    var user = JSON.parse(localStorage.getItem("userTemp"));
    var basee = localStorage.getItem("dbres"); 
    var prov = localStorage.getItem("companyCheck");
    var idees = localStorage.getItem("ids");
    var idesP = JSON.parse(idees);
    var urlWM = "./MongoService.asmx/retAllWorkersFromCollection";
    var urlWR = "./RavenService.asmx/retAllWorkersFromCollectionR";
    var wrs = "WorkersR";
    var ws = "Workers";
    var urlTemp = null;
    var tempw = null;

    if (basee === "raven")
    {
        urlTemp = urlWR;
        tempw = wrs;
    }
    else
    {
        urlTemp = urlWM;
        tempw = ws;
    }

    getWorkersWithAjax(urlTemp, function (data) {
        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find(tempw);
        received = $title;
        console.log(received);
        console.log(user);
        var k = 0;
        for (var i = 0; i < $title.length; i++) {
            var mail = $title[i].children[4].innerHTML;
            var first = $title[i].children[1].innerHTML;
            var last = $title[i].children[2].innerHTML;
            var company = $title[i].children[6].innerHTML;

            //console.log(JSON.parse(idees));

            if (basee === "raven") {
                if ($title[i].children[0].innerHTML === user.Id)
                    friend = "";
                else
                    friend = "Not Friends";

                if (user.Friends !== null) {
                    console.log($title[i]);
                    for (var j = 0; j < user.Friends.length; j++) {
                        var tmp = $title[i].children[0].innerHTML;
                        console.log(tmp.toString());
                        if ($title[i].children[0].innerHTML === user.Friends[j])
                            friend = "Friends";

                    }
                }
            }
            else
            {
                if (idesP[i] === user.Id)
                    friend = "";
                else
                    friend = "Not Friends";

                if (user.Friends !== null) {
                    
                    for (var j = 0; j < user.Friends.length; j++) {
                        var tmp = idesP[i];
                        console.log(tmp.toString());
                        if (idesP[i] === user.Friends[j].toString())
                            friend = "Friends";

                    }
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
            if (received[i].children[4].innerHTML === $(this).closest("tr")[0].children[3].innerHTML) {
                console.log($(this).closest("tr")[0].children[3].innerHTML);

                if (basee === "raven")
                    localStorage.setItem("workerViewR", received[i].children[0].innerHTML);
                else
                {
                    localStorage.setItem("workerView", idesP[i]);
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