$(document).ready(function () {
    var received;
    getCompaniesWithAjax(function (data) {
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
                + '<td>' + loc + '</td>'
                + '<td> <button type="button" class="view btn btn-default" id="addd' + j + '">View</button></td></tr>';
            $("#listing").append(table);
            j = j + 1;
            //console.log(user);
        }
    });

    $('#listing').on('click', '.view', function () {
        console.log(this);
        
        console.log($(this).closest("tr")[0].children[2].innerHTML);
        for (var i = 0; i < received.length; i++) {
            if (received[i].children[1].innerHTML === $(this).closest("tr")[0].children[2].innerHTML) {
                localStorage.setItem("companyViewR", JSON.stringify(received[i].children[0].innerHTML));
            }
        }
        window.location.assign("./companyInfo.aspx");

    });

    function getCompaniesWithAjax(fn) {

        $.ajax({
            //url: "./MongoService.asmx/retAllCompaniesFromCollection",
            url: "./RavenService.asmx/retAllCompaniesFromCollectionR",
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