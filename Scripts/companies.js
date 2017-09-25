$(document).ready(function () {
    var received;
    getCompaniesWithAjax(function (data) {
        //var json = xml2;
        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find("CompaniesR");
        received = $title;
        console.log($title);
        var j = 1;
        for (var i = 0; i < $title.length; i++) {
            var company = $title[i].children[1].innerHTML;
            var mail = $title[i].children[6].innerHTML;
            var type = $title[i].children[3].innerHTML;
            var loc = $title[i].children[4].innerHTML;
            var table = '<tr><th scope= "row">' + j + '</th>'
                + '<td>' + company + '</td>'
                + '<td>' + mail + '</td>'
                + '<td>' + type + '</td>'
                + '<td>' + loc + '</td></tr>';
            $("#listing").append(table);
            j = j + 1;
            //console.log(user);
        }
    });

    $('#listing').on('click', 'tr', function () {
        console.log(this);
        //$(this).toggleClass("selected");
        console.log(this.children[1].innerHTML);
        for (var i = 0; i < received.length; i++) {
            if (received[i].children[1].innerHTML === this.children[1].innerHTML) {
                var company = received[i].children[1].innerHTML;
                var mail = received[i].children[6].innerHTML;
                var type = received[i].children[3].innerHTML;
                var loc = received[i].children[4].innerHTML;
                var owner = received[i].children[2].innerHTML;
                var workers = received[i].children[5].innerHTML;
                var forSend = { company: company, mail: mail, type: type, loc: loc, owner: owner, workers: workers };
                localStorage.setItem("companyViewR", JSON.stringify(forSend));
            }
        }
        window.location.assign("./companyInfo.aspx");

    });

    function getCompaniesWithAjax(fn) {

        $.ajax({
            //url: "./MongoService.asmx/retAllCompaniesFromCollection",
            url: "./RaptorService.asmx/retAllCompaniesFromCollectionR",
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