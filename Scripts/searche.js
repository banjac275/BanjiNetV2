$(document).ready(function () {
    var received;
    var receivedW;
    var mailO, nameO;
    $("#listC").css("display", "none");
    $("#listW").css("display", "none");

    //urlovi
    var urlWMNP = "./RavenService.asmx/returnWorkerFromEmailNoPass";
    var urlCMNP = "./MongoService.asmx/returnCompanyFromEmailNoPass";
    var urlCN = "./MongoService.asmx/retCompanyFromName";
    var urlWN = "./RavenService.asmx/retWorkerFromName";
    var urlWLN = "./MongoService.asmx/retWorkerFromLastName";

    var suggest = document.getElementById("livesearch");
    var timeoutID = null;

    function findMember(str) {
        console.log('search: ' + str);
        var name = { 'name': str };

        getAjaxResponse(urlWN,name, function (data) {
            var xmldocs = $.parseXML(data),
                $xmls = $(xmldocs),
                $titles = $xmls.find("string");
            console.log(data);

            if ($titles.text() == "Worker with that name doesn't exist in our registry!") {
                //alert("Nothing found!");
            }
            else {
                $("#listingW").empty();
                $("#listW").css("display", "block");
                var jsons = JSON.parse($titles.text());
                receivedW = jsons;
                var j = 1;
                for (var i = 0; i < jsons.length; i++) {
                    var check = false;
                    var first = jsons[0].FirstName;
                    var childs = '<div class="row"><p class="col-lg-3">' + first + '</p><p class="col-lg-3" style="color: #778899;">Workers</p></div>';
                    console.log(suggest.hasChildNodes());
                    if (suggest.hasChildNodes())
                    {
                        for (var k = 0; k < suggest.childNodes.length; k++)
                        {
                            if (suggest.childNodes[k].text !== first)
                                $("#livesearch").append(childs);
                        }
                    }
                    else
                        $("#livesearch").append(childs);
                    
                    if (suggest.innerHTML !== "")
                    {
                        suggest.style.border = "1px solid #A5ACB2";
                    }
                    else
                    {
                        suggest.style.border = "0px";
                    }
                    //var last = jsons[0].LastName;
                    //var company = jsons[0].CompanyName;
                    //var table = '<tr><th scope= "row">' + j + '</th>'
                    //    + '<td>' + first + '</td>'
                    //    + '<td>' + last + '</td>'
                    //    + '<td>' + mail + '</td>'
                    //    + '<td>' + company + '</td></tr>';
                    //$("#listingW").append(table);
                    //j = j + 1;
                }
            }
        });                                      

    }

    //kako se kuca tekst tako se i pretraga vrsi
    $("#srcinput").keyup(function (e) {
        clearTimeout(timeoutID);
        timeoutID = setTimeout(findMember.bind(undefined, e.target.value), 500);
    });

    //sta se desava kad se klikne dugme go za pregtragu
    $("#srcbttn").click(function (e) {

        e.preventDefault();

        $("#listingC").empty();
        $("#listingW").empty();

        $("#listC").css("display", "none");
        $("#listW").css("display", "none");

        var search = $("#srcinput").val();

        //pravimo data string
        var name = { 'name': search };
        var mail = { 'mail': search };
        mailO = mail;
        nameO = name;

        //if (search == '') {
        //    alert("Nothing to search!");
        //}
        //else {

        //    getAjaxResponseWorkerEmail(mail, function (data) {
        //        var xmldoc = $.parseXML(data),
        //            $xml = $(xmldoc),
        //            $title = $xml.find("string");
        //        console.log($title.text());

        //        var sign = "Worker not found with mail!";
        //        if ($title.text() == sign) {
        //            getAjaxResponseCompanyEmail(mailO, function (dataa) {
        //                var xmldocs = $.parseXML(dataa),
        //                    $xmls = $(xmldocs),
        //                    $titles = $xmls.find("string");
        //                console.log($titles.text());

        //                var sign2 = "Company not found with mail!";
        //                if ($titles.text() == sign2) {
        //                    getAjaxResponseCompanyName(nameO, function (dataaa) {
        //                        var xmldocss = $.parseXML(dataaa),
        //                            $xmlss = $(xmldocss),
        //                            $titless = $xmlss.find("string");
        //                        console.log($titless.text());

        //                        var badp = "Company with that name doesn't exist in our registry!";
        //                        if ($titless.text() == badp) {
        //                            getAjaxResponseWorkerName(nameO, function (dataaaa) {
        //                                var xmldocsss = $.parseXML(dataaaa),
        //                                    $xmlsss = $(xmldocsss),
        //                                    $titlesss = $xmlsss.find("string");
        //                                console.log(dataaaa);

        //                                var badp2 = "Worker with that name doesn't exist in our registry!";
        //                                if ($titlesss.text() == badp2) {
        //                                    getAjaxResponseWorkerLastName(nameO, function (dataaaaa) {
        //                                        var xmldocssss = $.parseXML(dataaaaa),
        //                                            $xmlssss = $(xmldocssss),
        //                                            $titlessss = $xmlssss.find("string");
        //                                        console.log(dataaaaa);

        //                                        var badp3 = "Worker with that last name doesn't exist in our registry!";
        //                                        if ($titlessss.text() == badp3) {
        //                                            alert("Nothing found!");
        //                                        }
        //                                        else {
        //                                            $("#listW").css("display", "block");                                                    
        //                                            var jsonss = JSON.parse($titlessss.text());
        //                                            receivedW = jsonss;
        //                                            var j = 1;
        //                                            for (var i = 0; i < jsonss.length; i++) {
        //                                                var mail = jsonss[0].Email;
        //                                                var first = jsonss[0].FirstName;
        //                                                var last = jsonss[0].LastName;
        //                                                var company = jsonss[0].CompanyName;
        //                                                var table = '<tr><th scope= "row">' + j + '</th>'
        //                                                    + '<td>' + first + '</td>'
        //                                                    + '<td>' + last + '</td>'
        //                                                    + '<td>' + mail + '</td>'
        //                                                    + '<td>' + company + '</td></tr>';
        //                                                $("#listingW").append(table);
        //                                                j = j + 1;
        //                                            }
        //                                        }
        //                                    });
        //                                }
        //                                else {
                                            
        //                                    $("#listW").css("display", "block");                                            
        //                                    var jsons = JSON.parse($titlesss.text());
        //                                    receivedW = jsons;
        //                                    var j = 1;
        //                                    for (var i = 0; i < jsons.length; i++) {
        //                                        var mail = jsons[i].Email;
        //                                        var first = jsons[i].FirstName;
        //                                        var last = jsons[i].LastName;
        //                                        var company = jsons[i].CompanyName;
        //                                        var table = '<tr><th scope= "row">' + j + '</th>'
        //                                            + '<td>' + first + '</td>'
        //                                            + '<td>' + last + '</td>'
        //                                            + '<td>' + mail + '</td>'
        //                                            + '<td>' + company + '</td></tr>';
        //                                        $("#listingW").append(table);
        //                                        j = j + 1;
        //                                    }
        //                                }
        //                            });
        //                        }
        //                        else {
                                    
        //                            $("#listC").css("display", "block");
        //                            var par = JSON.parse($titless.text());
        //                            received = par;
        //                            var j = 1;
        //                            for (var i = 0; i < $titless.length; i++) {
        //                                var company = par[i].CompanyName;
        //                                var mail = par[i].Email;
        //                                var type = par[i].Type;
        //                                var loc = par[i].Location;
        //                                var table = '<tr><th scope= "row">' + j + '</th>'
        //                                    + '<td>' + company + '</td>'
        //                                    + '<td>' + mail + '</td>'
        //                                    + '<td>' + type + '</td>'
        //                                    + '<td>' + loc + '</td></tr>';
        //                                $("#listingC").append(table);
        //                                j = j + 1;
        //                            }
        //                        }
        //                    });
        //                }
        //                else {
        //                    $("#listC").css("display", "block");
        //                    var res = JSON.parse($titles.text())
        //                    received = res;
        //                    var j = 1;
        //                    for (var i = 0; i < res.length; i++) {
        //                        var company = res.CompanyName;
        //                        var mail = res.Email;
        //                        var type = res.Type;
        //                        var loc = res.Location;
        //                        var table = '<tr><th scope= "row">' + j + '</th>'
        //                            + '<td>' + company + '</td>'
        //                            + '<td>' + mail + '</td>'
        //                            + '<td>' + type + '</td>'
        //                            + '<td>' + loc + '</td></tr>';
        //                        $("#listingC").append(table);
        //                        j = j + 1;
        //                    }
        //                }
        //            });
        //        }
        //        else {
        //            $("#listW").css("display", "block");
        //            var json = JSON.parse($title.text());
        //            receivedW = json;
        //            //console.log($title);
        //            var j = 1;
        //            for (var i = 0; i < json.length; i++) {
        //                var mail = json.Email;
        //                var first = json.FirstName;
        //                var last = json.LastName;
        //                var company = json.CompanyName;
        //                var table = '<tr><th scope= "row">' + j + '</th>'
        //                    + '<td>' + first + '</td>'
        //                    + '<td>' + last + '</td>'
        //                    + '<td>' + mail + '</td>'
        //                    + '<td>' + company + '</td></tr>';
        //                $("#listingW").append(table);
        //                j = j + 1;
        //            }
        //        }
        //    });
        //}

        return false;
    });

    $('#livesearch').on('click', 'p', function () {
        console.log(this);
        //$(this).toggleClass("selected");
        //console.log(this.children[1].innerHTML);
        //for (var i = 0; i < received.length; i++) {
        //    if (received[i].Email == this.children[1].innerHTML) {
        //        var company = received[i].CompanyName;
        //        var mail = received[i].Email;
        //        var type = received[i].Type;
        //        var loc = received[i].Location;
        //        var owner = received[i].Owner;
        //        var workers = received[i].Employees;
        //        var forSend = { company: company, mail: mail, type: type, loc: loc, owner: owner, workers: workers };
        //        localStorage.setItem("companyView", JSON.stringify(forSend));
        //    }
        //}
        //window.location.assign("./companyInfo.aspx");

    });

    $('#listingC').on('click', 'tr', function () {
        console.log(this);
        //$(this).toggleClass("selected");
        console.log(this.children[1].innerHTML);
        for (var i = 0; i < received.length; i++) {
            if (received[i].Email == this.children[1].innerHTML) {
                var company = received[i].CompanyName;
                var mail = received[i].Email;
                var type = received[i].Type;
                var loc = received[i].Location;
                var owner = received[i].Owner;
                var workers = received[i].Employees;
                var forSend = { company: company, mail: mail, type: type, loc: loc, owner: owner, workers: workers };
                localStorage.setItem("companyView", JSON.stringify(forSend));
            }
        }
        window.location.assign("./companyInfo.aspx");

    });

    $('#listingW').on('click', 'tr', function () {
        console.log(this);
        //$(this).toggleClass("selected");
        console.log(this);
        for (var i = 0; i < receivedW.length; i++) {
            if (receivedW[i].Email == this.children[3].innerHTML) {
                var mail = receivedW[i].Email;
                var first = receivedW[i].FirstName;
                var last = receivedW[i].LastName;
                var company = receivedW[i].CompanyName;
                var forSend = { first: first, last: last, mail: mail, company: company };
                localStorage.setItem("workerView", JSON.stringify(forSend));
            }
        }
        window.location.assign("./fellowworker.aspx");

    });

    function getAjaxResponse(urll, sstring, fn) {

        $.ajax({
            url: urll,
            dataType: "text",
            type: "POST",
            data: sstring,
            error: function (err) {
                alert("Error", err);
            },
            success: function (data) {
                fn(data);
            }
        });
    }

});

