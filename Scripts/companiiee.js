$(document).ready(function () {
    var received;
    var urlCM = "./MongoService.asmx/retAllCompaniesFromCollection";
    var urlCR = "./RavenService.asmx/retAllCompaniesFromCollectionR";
    var urlTemp = null;
    var basee = localStorage.getItem("dbres");
    var wrs = "CompaniesR";
    var ws = "Companies";
    var tempc = null;
    var idees = localStorage.getItem("idsc");
    var idesP = JSON.parse(idees);

    if (basee === "raven") {
        urlTemp = urlCR;
        tempc = wrs;
    }
    else {
        urlTemp = urlCM;
        tempc = ws;
    }

    getCompaniesWithAjax(urlTemp, function (data) {
        var xmldoc = $.parseXML(data),
            $xml = $(xmldoc),
            $title = $xml.find(tempc);
        received = $title;
        console.log($title);
        var j = 1;
        for (var i = 0; i < $title.length; i++) {
            var company = $title[i].children[1].innerHTML;
            var mail = $title[i].children[5].innerHTML;
            var type = $title[i].children[3].innerHTML;
            var loc = $title[i].children[4].innerHTML;
           
            var table = '<tr><th scope= "row">' + j + '</th>'
                + '<td>' + company + '</td>'
                + '<td>' + mail + '</td>'
                + '<td>' + type + '</td>'
                + '<td>' + loc + '</td>'
                + '<td> <button type="button" class="view btn btn-default" id="addd' + j + '">View</button></td></tr>';
            $("#listing").append(table);
            j = j + 1;
            console.log(idesP);
        }
    });

    $('#listing').on('click', '.view', function () {
        console.log(this);
        console.log(received[0].children[6].innerHTML);
        console.log($(this).closest("tr")[0].children[2].innerHTML);
        for (var i = 0; i < received.length; i++) {
            if (received[i].children[5].innerHTML === $(this).closest("tr")[0].children[2].innerHTML) {
                if (basee === "raven")
                    localStorage.setItem("companyViewR", JSON.stringify(received[i].children[0].innerHTML));
                else
                    localStorage.setItem("companyView", idesP[i]);
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