$(document).ready(function () {
    var received;
    var user = JSON.parse(localStorage.getItem("userTemp"));
    var prov = localStorage.getItem("companyCheck");
    console.log(prov);
    getWorkersWithAjax(function (data) {
        var friend;
        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find("WorkersR");
        received = $title;
        console.log(received);
        console.log(user);
        var k = 0;
        for (var i = 0; i < $title.length; i++) {
            var mail = $title[i].children[6].innerHTML;
            var first = $title[i].children[3].innerHTML;
            var last = $title[i].children[4].innerHTML;
            var company = $title[i].children[2].innerHTML;

            if ($title[i].children[0].innerHTML === user.Id)
                friend = "";
            else
                friend = "Not Friends";
            
            if (user.Friends !== null) {
                console.log("prolazi");
                for (var j = 0; j < user.Friends.length; j++) {
                    if ($title[i].children[0].innerHTML === user.Friends[j])
                        friend = "Friends";
                   
                }
            }
            

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
            //console.log(user);
        }
    });

    $('#listing').on('click', '.view', function () {
        for (var i = 0; i < received.length; i++) {
            if (received[i].children[6].innerHTML === $(this).closest("tr")[0].children[3].innerHTML) {
                console.log($(this).closest("tr")[0].children[3].innerHTML);
                localStorage.setItem("workerViewR", received[i].children[0].innerHTML);
            }
        }
        window.location.assign("./fellowworker.aspx");

    });

    function getWorkersWithAjax(fn) {

        $.ajax({
            //url: "./MongoService.asmx/retAllWorkersFromCollection",
            url: "./RavenService.asmx/retAllWorkersFromCollectionR",
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