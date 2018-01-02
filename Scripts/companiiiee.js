$(document).ready(function () {
    var received;
    var urlCM = "./MongoService.asmx/retAllCompaniesFromCollection";
    var urlCR = "./RavenService.asmx/retAllCompaniesFromCollectionR";
    var urlTemp = null;
    var basee = localStorage.getItem("dbres");

    if (basee === "raven") {
        urlTemp = urlCR;

    }
    else {
        urlTemp = urlCM;

    }

    getCompaniesWithAjax(urlTemp, function (data) {
        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find("string");
        received = JSON.parse($title.text());
        //console.log($title);
        var j = 1;
        for (var i = 0; i < received.length; i++) {
            var company = received[i].CompanyName;
            var mail = received[i].Email;
            var type = received[i].Type;
            var loc = received[i].Location;
           
            var table = '<tr><th scope= "row">' + j + '</th>'
                + '<td>' + company + '</td>'
                + '<td>' + mail + '</td>'
                + '<td>' + type + '</td>'
                + '<td>' + loc + '</td>'
                + '<td> <button type="button" class="view btn btn-default" id="addd' + j + '">View</button></td></tr>';
            $("#listing").append(table);
            j = j + 1;
            //console.log(idesP);
        }
    });

    $('#listing').on('click', '.view', function () {
        console.log(this);
        //console.log(received[0].children[6].innerHTML);
        console.log($(this).closest("tr")[0].children[2].innerHTML);
        for (var i = 0; i < received.length; i++) {
            if (received[i].Email === $(this).closest("tr")[0].children[2].innerHTML) {
                if (basee === "raven")
                    localStorage.setItem("companyViewR", JSON.stringify(received[i].Id));
                else
                    localStorage.setItem("companyView", received[i].Id);
            }
        }
        window.location.assign("./companyInfo.aspx");

    });

    function getCompaniesWithAjax(urls, fn) {

        $.ajax({
            url: urls,
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