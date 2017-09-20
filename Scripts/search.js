$(document).ready(function () {
    var received;
    var receivedW;
    var mailO, nameO;
    $("#listC").css("display", "none");
    $("#listW").css("display", "none");

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

        if (search == '') {
            alert("Nothing to search!");
        }
        else {

            getAjaxResponseWorkerEmail(mail, function (data) {
                var xmldoc = $.parseXML(data),
                    $xml = $(xmldoc),
                    $title = $xml.find("string");
                console.log($title.text());

                var sign = "Worker not found with mail!";
                if ($title.text() == sign) {
                    getAjaxResponseCompanyEmail(mailO, function (dataa) {
                        var xmldocs = $.parseXML(dataa),
                            $xmls = $(xmldocs),
                            $titles = $xmls.find("string");
                        console.log($titles.text());

                        var sign2 = "Company not found with mail!";
                        if ($titles.text() == sign2) {
                            getAjaxResponseCompanyName(nameO, function (dataaa) {
                                var xmldocss = $.parseXML(dataaa),
                                    $xmlss = $(xmldocss),
                                    $titless = $xmlss.find("string");
                                console.log($titless.text());

                                var badp = "Company with that name doesn't exist in our registry!";
                                if ($titless.text() == badp) {
                                    getAjaxResponseWorkerName(nameO, function (dataaaa) {
                                        var xmldocsss = $.parseXML(dataaaa),
                                            $xmlsss = $(xmldocsss),
                                            $titlesss = $xmlsss.find("string");
                                        console.log(dataaaa);

                                        var badp2 = "Worker with that name doesn't exist in our registry!";
                                        if ($titlesss.text() == badp2) {
                                            getAjaxResponseWorkerLastName(nameO, function (dataaaaa) {
                                                var xmldocssss = $.parseXML(dataaaaa),
                                                    $xmlssss = $(xmldocssss),
                                                    $titlessss = $xmlssss.find("string");
                                                console.log(dataaaaa);

                                                var badp3 = "Worker with that last name doesn't exist in our registry!";
                                                if ($titlessss.text() == badp3) {
                                                    alert("Nothing found!");
                                                }
                                                else {
                                                    $("#listW").css("display", "block");                                                    
                                                    var jsonss = JSON.parse($titlessss.text());
                                                    receivedW = jsonss;
                                                    var j = 1;
                                                    for (var i = 0; i < jsonss.length; i++) {
                                                        var mail = jsonss[0].Email;
                                                        var first = jsonss[0].FirstName;
                                                        var last = jsonss[0].LastName;
                                                        var company = jsonss[0].CompanyName;
                                                        var table = '<tr><th scope= "row">' + j + '</th>'
                                                            + '<td>' + first + '</td>'
                                                            + '<td>' + last + '</td>'
                                                            + '<td>' + mail + '</td>'
                                                            + '<td>' + company + '</td></tr>';
                                                        $("#listingW").append(table);
                                                        j = j + 1;
                                                    }
                                                }
                                            });
                                        }
                                        else {
                                            
                                            $("#listW").css("display", "block");                                            
                                            var jsons = JSON.parse($titlesss.text());
                                            receivedW = jsons;
                                            var j = 1;
                                            for (var i = 0; i < jsons.length; i++) {
                                                var mail = jsons[i].Email;
                                                var first = jsons[i].FirstName;
                                                var last = jsons[i].LastName;
                                                var company = jsons[i].CompanyName;
                                                var table = '<tr><th scope= "row">' + j + '</th>'
                                                    + '<td>' + first + '</td>'
                                                    + '<td>' + last + '</td>'
                                                    + '<td>' + mail + '</td>'
                                                    + '<td>' + company + '</td></tr>';
                                                $("#listingW").append(table);
                                                j = j + 1;
                                            }
                                        }
                                    });
                                }
                                else {
                                    
                                    $("#listC").css("display", "block");
                                    var par = JSON.parse($titless.text());
                                    received = par;
                                    var j = 1;
                                    for (var i = 0; i < $titless.length; i++) {
                                        var company = par[i].CompanyName;
                                        var mail = par[i].Email;
                                        var type = par[i].Type;
                                        var loc = par[i].Location;
                                        var table = '<tr><th scope= "row">' + j + '</th>'
                                            + '<td>' + company + '</td>'
                                            + '<td>' + mail + '</td>'
                                            + '<td>' + type + '</td>'
                                            + '<td>' + loc + '</td></tr>';
                                        $("#listingC").append(table);
                                        j = j + 1;
                                    }
                                }
                            });
                        }
                        else {
                            $("#listC").css("display", "block");
                            var res = JSON.parse($titles.text())
                            received = res;
                            var j = 1;
                            for (var i = 0; i < res.length; i++) {
                                var company = res.CompanyName;
                                var mail = res.Email;
                                var type = res.Type;
                                var loc = res.Location;
                                var table = '<tr><th scope= "row">' + j + '</th>'
                                    + '<td>' + company + '</td>'
                                    + '<td>' + mail + '</td>'
                                    + '<td>' + type + '</td>'
                                    + '<td>' + loc + '</td></tr>';
                                $("#listingC").append(table);
                                j = j + 1;
                            }
                        }
                    });
                }
                else {
                    $("#listW").css("display", "block");
                    var json = JSON.parse($title.text());
                    receivedW = json;
                    //console.log($title);
                    var j = 1;
                    for (var i = 0; i < json.length; i++) {
                        var mail = json.Email;
                        var first = json.FirstName;
                        var last = json.LastName;
                        var company = json.CompanyName;
                        var table = '<tr><th scope= "row">' + j + '</th>'
                            + '<td>' + first + '</td>'
                            + '<td>' + last + '</td>'
                            + '<td>' + mail + '</td>'
                            + '<td>' + company + '</td></tr>';
                        $("#listingW").append(table);
                        j = j + 1;
                    }
                }
            });
        }

        return false;
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

    function getAjaxResponseWorkerEmail(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/returnWorkerFromEmailNoPass",
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

    function getAjaxResponseCompanyEmail(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/returnCompanyFromEmailNoPass",
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

    function getAjaxResponseCompanyName(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/retCompanyFromName",
            dataType: "text",
            type: "POST",
            data: sstring,
            error: function (err) {
                alert("Error", err.toString());
            },
            success: function (data) {
                fn(data);
            }
        });
    }

    function getAjaxResponseWorkerName(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/retWorkerFromName",
            dataType: "text",
            type: "POST",
            data: sstring,
            error: function (err) {
                alert("Error", err.toString());
            },
            success: function (data) {
                fn(data);
            }
        });
    }

    function getAjaxResponseWorkerLastName(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/retWorkerFromLastName",
            dataType: "text",
            type: "POST",
            data: sstring,
            error: function (err) {
                alert("Error", err.toString());
            },
            success: function (data) {
                fn(data);
            }
        });
    }

    function exec5(name) {
        getAjaxResponseWorkerLastName(name, function (dataaaaa) {
            var xmldocssss = $.parseXML(dataaaaa),
                $xmlssss = $(xmldocssss),
                $titlessss = $xmlssss.find("string");
            console.log(dataaaaa);

            var badp3 = "Worker with that last name doesn't exist in our registry!";
            if ($titlessss.text() == badp3) {
                alert(badp3);
            }
            else {
                alert("Entity found!");
                $("#listW").css("display", "block");
                receivedW = $titlessss;
                console.log($titlessss[0]);
                var j = 1;
                for (var i = 0; i < $titlessss.length; i++) {
                    var mail = $titlessss[i].children[6].innerHTML;
                    var first = $titlessss[i].children[3].innerHTML;
                    var last = $titlessss[i].children[4].innerHTML;
                    var company = $titlessss[i].children[2].innerHTML;
                    var table = '<tr><th scope= "row">' + j + '</th>'
                        + '<td>' + first + '</td>'
                        + '<td>' + last + '</td>'
                        + '<td>' + mail + '</td>'
                        + '<td>' + company + '</td></tr>';
                    $("#listingW").append(table);
                    j = j + 1;

                }
            }
        });
    }

    function exec4(name) {
        getAjaxResponseWorkerName(name, function (dataaaa) {
            var xmldocsss = $.parseXML(dataaaa),
                $xmlsss = $(xmldocsss),
                $titlesss = $xmlsss.find("string");
            console.log(dataaaa);

            var badp2 = "Worker with that name doesn't exist in our registry!";
            if ($titlesss.text() == badp2) {
                exec5(nameO);
            }
            else {
                alert("Entity found!");
                $("#listW").css("display", "block");
                receivedW = $titlesss;
                console.log($titlesss[0]);
                var j = 1;
                for (var i = 0; i < $titlesss.length; i++) {
                    var mail = $titlesss[i].children[6].innerHTML;
                    var first = $titlesss[i].children[3].innerHTML;
                    var last = $titlesss[i].children[4].innerHTML;
                    var company = $titlesss[i].children[2].innerHTML;
                    var table = '<tr><th scope= "row">' + j + '</th>'
                        + '<td>' + first + '</td>'
                        + '<td>' + last + '</td>'
                        + '<td>' + mail + '</td>'
                        + '<td>' + company + '</td></tr>';
                    $("#listingW").append(table);
                    j = j + 1;

                }
            }
        });
    }

    function exec3(name) {
        getAjaxResponseCompanyName(name, function (dataaa) {
            var xmldocss = $.parseXML(dataaa),
                $xmlss = $(xmldocss),
                $titless = $xmlss.find("string");
            console.log($titless.text());

            var badp = "Company with that name doesn't exist in our registry!";
            if ($titless.text() == badp) {
                exec4(nameO);
            }
            else {
                alert("Entity found!");
                $("#listC").css("display", "block");
                received = $titless;
                //console.log($title[0].children[5].innerHTML);
                var j = 1;
                for (var i = 0; i < $titless.length; i++) {
                    var company = $titless[i].children[1].innerHTML;
                    var mail = $titless[i].children[6].innerHTML;
                    var type = $titless[i].children[3].innerHTML;
                    var loc = $titless[i].children[4].innerHTML;
                    var table = '<tr><th scope= "row">' + j + '</th>'
                        + '<td>' + company + '</td>'
                        + '<td>' + mail + '</td>'
                        + '<td>' + type + '</td>'
                        + '<td>' + loc + '</td></tr>';
                    $("#listingC").append(table);
                    j = j + 1;
                }
            }
        });
    }

    function exec2(mail, name) {
        getAjaxResponseCompanyEmail(mail, function (dataa) {
            var xmldocs = $.parseXML(dataa),
                $xmls = $(xmldocs),
                $titles = $xmls.find("string");
            console.log($titles.text());

            var sign2 = "Company not found with mail!";
            if ($titles.text() == sign2) {
                exec3(nameO);
            }
            else {
                $("#listC").css("display", "block");
                received = JSON.parse($titles.text());
                var res = JSON.parse($titles.text())
                var j = 1;
                for (var i = 0; i < $titles.length; i++) {
                    var company = res.CompanyName;
                    var mail = res.Email;
                    var type = res.Type;
                    var loc = res.Location;
                    var table = '<tr><th scope= "row">' + j + '</th>'
                        + '<td>' + company + '</td>'
                        + '<td>' + mail + '</td>'
                        + '<td>' + type + '</td>'
                        + '<td>' + loc + '</td></tr>';
                    $("#listingC").append(table);
                    j = j + 1;
                }
            }
        });
    }

});

