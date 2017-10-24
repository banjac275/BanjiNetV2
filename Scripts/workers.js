$(document).ready(function () {
    var received;
    var user = JSON.parse(localStorage.getItem("userTemp"));
    getWorkersWithAjax(function (data) {

        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find("WorkersR");
        received = $title;
        console.log(received);
        console.log(user);
        var j = 1;
        for (var i = 0; i < $title.length; i++) {
            var mail = $title[i].children[6].innerHTML;
            var first = $title[i].children[3].innerHTML;
            var last = $title[i].children[4].innerHTML;
            var company = $title[i].children[2].innerHTML;
            var friend = "";
            console.log(user.Friends);
            if (user.Friends !== null) {
                console.log("prolazi");
                for (var j = 0; j < user.Friends.length; j++) {
                    if ($title[i].children[0].innerHTML === user.Friends[j])
                        friend = "Friends";
                    else if ($title[i].children[0].innerHTML === user.Id)
                        friend = "";
                    else
                        friend = "Not Friends";
                }
            }
            else if ($title[i].children[0].innerHTML === user.Id)
                friend = "";
            else
                friend = "Not Friends";
            var table = '<tr><th scope= "row">' + j + '</th>'
                + '<td>' + first + '</td>'
                + '<td>' + last + '</td>'
                + '<td>' + mail + '</td>'
                + '<td>' + company + '</td>'
                + '<td>' + friend + '</td></tr >';
            $("#listing").append(table);
            j = j + 1;
            //console.log(user);
        }
    });

    $('#listing').on('click', 'tr', function () {
        console.log(this);
        //$(this).toggleClass("selected");
        console.log(this.children[3].innerHTML);
        for (var i = 0; i < received.length; i++) {
            if (received[i].children[6].innerHTML === this.children[3].innerHTML) {
                //var mail = received[i].children[6].innerHTML;
                //var first = received[i].children[3].innerHTML;
                //var last = received[i].children[4].innerHTML;
                //var company = received[i].children[2].innerHTML;
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