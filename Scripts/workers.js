$(document).ready(function () {
    var received;
    getWorkersWithAjax(function (data) {
        //var json = xml2;
        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find("Workers");
        received = $title;
        //console.log($title[0]);
        var j = 1;
        for (var i = 0; i < $title.length; i++) {
            var mail = $title[i].children[5].innerHTML;
            var first = $title[i].children[2].innerHTML;
            var last = $title[i].children[3].innerHTML;
            var company = $title[i].children[1].innerHTML;
            var table = '<tr><th scope= "row">' + j + '</th>'
                + '<td>' + first + '</td>'
                + '<td>' + last + '</td>'
                + '<td>' + mail + '</td>'
                + '<td>' + company + '</td></tr>';
            $("#listing").append(table);
            j = j + 1;
            //console.log(user);
        }   
    });

    $('#listing').on('click','tr', function () {
        console.log(this);
        $(this).toggleClass("selected");
        console.log(this.children[3].innerHTML);
        for (var i = 0; i < received.length; i++)
        {
            if (received[i].children[5].innerHTML == this.children[3].innerHTML)
            {
                var mail = received[i].children[5].innerHTML;
                var first = received[i].children[2].innerHTML;
                var last = received[i].children[3].innerHTML;
                var company = received[i].children[1].innerHTML;
                var forSend = { first: first, last: last, mail: mail, company: company };
                localStorage.setItem("workerView", JSON.stringify(forSend));
            }
        }
        window.location.assign("./fellowworker.aspx");

    });

    function getWorkersWithAjax(fn) {

        $.ajax({
            url: "./MongoService.asmx/retAllWorkersFromCollection",
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