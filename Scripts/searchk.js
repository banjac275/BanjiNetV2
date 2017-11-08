$(document).ready(function () {
    var received = [];
    var receivedW;
    var mailO, nameO;
    $("#listC").css("display", "none");
    $("#listW").css("display", "none");

    //urlovi
    var urlWMNP = "./RavenService.asmx/returnWorkerFromEmailNoPass";
    var urlCMNP = "./RavenService.asmx/returnCompanyFromEmailNoPass";
    var urlCN = "./RavenService.asmx/retCompanyFromName";
    var urlWN = "./RavenService.asmx/retWorkerFromName";
    var urlWLN = "./RavenService.asmx/retWorkerFromLastName";
    var urlWS = "./RavenService.asmx/retWorkerWithSkill";
    var urlA = [urlWMNP, urlWN, urlWLN, urlWS];
    var urlB = [urlCMNP, urlCN];

    var suggest = document.getElementById("livesearch");
    var timeoutID = null;

    function findMember(str) {
        console.log('search: ' + str);
        var name = { 'name': str };
        received = [];
        suggest.style.display = "block";
        $("#livesearch").empty();

        //pretraga po radnicima
        for (var l = 0; l < urlA.length; l++) {
            getAjaxResponse(urlA[l], name, function (data) {
                var xmldocs = $.parseXML(data),
                    $xmls = $(xmldocs),
                    $titles = $xmls.find("string");
                console.log(data);

                if ($titles.text() == "Worker not found!") {
                    //alert("Nothing found!");
                }
                else {
                    $("#listingW").empty();
                    //$("#listW").css("display", "block");
                    var jsons = JSON.parse($titles.text());

                    var j = 1;
                    if (Array.isArray(jsons)) {
                        for (var i = 0; i < jsons.length; i++) {
                            var checkRec = false;
                            var check;
                            var first = String(jsons[i].FirstName);
                            var second = String(jsons[i].Email);
                            var third = String(jsons[i].LastName);
                            console.log(first);
                            console.log(first.toUpperCase().match(str.toUpperCase()));

                            if (first.toUpperCase().match(str.toUpperCase()))
                                check = jsons[i].FirstName;
                            if (second.toUpperCase().match(str.toUpperCase()))
                                check = jsons[i].Email;
                            if (third.toUpperCase().match(str.toUpperCase()))
                                check = jsons[i].LastName;
                            if (jsons[i].Skills !== null) {
                                for (var c = 0; c < jsons[i].Skills.length; c++) {
                                    var temp = String(jsons[i].Skills[c]);
                                    if (temp.toUpperCase().match(str.toUpperCase()))
                                        check = jsons[i].Skills;
                                }
                            }
                            
                            if (check !== null) {
                                var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Workers</p></div>';
                                console.log(suggest.hasChildNodes());

                                if (received !== null) {
                                    for (var m = 0; m < received.length; m++) {
                                        if (received[m].found.Id === jsons[i].Id)
                                            checkRec = true;
                                    }
                                    if (checkRec === false) {
                                        var temporary = { found: jsons[i], type: "Workers", queryS: check };
                                        received.push(temporary);
                                    }

                                }
                                else {
                                    var temporary = { found: jsons[i], type: "Workers", queryS: check };
                                    received.push(temporary);
                                }

                                if (suggest.hasChildNodes()) {
                                    for (var k = 0; k < suggest.childNodes.length; k++) {
                                        //console.log($("#livesearch")[k]);
                                        if ($("#livesearch")[k].children[0].children[0].innerHTML !== check)
                                            $("#livesearch").append(childs);
                                    }
                                }
                                else
                                    $("#livesearch").append(childs);

                                if (suggest.innerHTML !== "") {
                                    suggest.style.border = "1px solid #A5ACB2";
                                }
                                else {
                                    suggest.style.border = "0px";
                                }
                            }
                        }
                    }
                    else
                    {
                        var checkRec = false;
                        var check;
                        var first = String(jsons.FirstName);
                        var second = String(jsons.Email);
                        var third = String(jsons.LastName);
                        console.log(first);
                        console.log(first.toUpperCase().match(str.toUpperCase()));

                        if (first.toUpperCase().match(str.toUpperCase()))
                            check = jsons.FirstName;
                        if (second.toUpperCase().match(str.toUpperCase()))
                            check = jsons.Email;
                        if (third.toUpperCase().match(str.toUpperCase()))
                            check = jsons.LastName;
                        if (jsons[i].Skills !== null) {
                            for (var c = 0; c < jsons[i].Skills.length; c++) {
                                var temp = String(jsons[i].Skills[c]);
                                if (temp.toUpperCase().match(str.toUpperCase()))
                                    check = jsons[i].Skills;
                            }
                        }
                       

                        if (check !== null) {
                            var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Workers</p></div>';
                            console.log(suggest.hasChildNodes());

                            if (received !== null) {
                                for (var m = 0; m < received.length; m++) {
                                    if (received[m].found.Id === jsons.Id)
                                        checkRec = true;
                                }
                                if (checkRec === false) {
                                    var temporary = { found: jsons, type: "Workers", queryS: check };
                                    received.push(temporary);
                                }

                            }
                            else {
                                var temporary = { found: jsons, type: "Workers", queryS: check };
                                received.push(temporary);
                            }

                            if (suggest.hasChildNodes()) {
                                for (var k = 0; k < suggest.childNodes.length; k++) {
                                    //console.log($("#livesearch")[k]);
                                    if ($("#livesearch")[k].children[0].children[0].innerHTML !== check)
                                        $("#livesearch").append(childs);
                                }
                            }
                            else
                                $("#livesearch").append(childs);

                            if (suggest.innerHTML !== "") {
                                suggest.style.border = "1px solid #A5ACB2";
                            }
                            else {
                                suggest.style.border = "0px";
                            }
                        }
                    }

                    console.log(received);
                }
            });
        }

        //pretraga po kompanijama
        for (var l = 0; l < urlB.length; l++) {
            getAjaxResponse(urlB[l], name, function (data) {
                var xmldocs = $.parseXML(data),
                    $xmls = $(xmldocs),
                    $titles = $xmls.find("string");
                console.log(data);

                if ($titles.text() == "Company not found!") {
                    //alert("Nothing found!");
                }
                else {
                    $("#listingC").empty();
                    //$("#listC").css("display", "block");
                    var jsons = JSON.parse($titles.text());
                    console.log(jsons.length);

                    var j = 1;
                    if (Array.isArray(jsons)) {
                        for (var i = 0; i < jsons.length; i++) {
                            var checkRec = false;
                            var check;
                            var first = String(jsons[i].Email);
                            var second = String(jsons[i].CompanyName);
                            if (first.toUpperCase().match(str.toUpperCase()))
                                check = jsons[i].Email;
                            else if (second.toUpperCase().match(str.toUpperCase()))
                                check = jsons[i].CompanyName;
                            console.log(check);
                            if (check !== null) {
                                var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Companies</p></div>';
                                console.log(suggest.hasChildNodes());

                                if (received !== null) {
                                    for (var m = 0; m < received.length; m++) {
                                        if (received[m].found.Id === jsons[i].Id)
                                            checkRec = true;
                                    }
                                    if (checkRec === false) {
                                        var temporary = { found: jsons[i], type: "Companies", queryS: check };
                                        received.push(temporary);
                                    }

                                }
                                else {
                                    var temporary = { found: jsons[i], type: "Companies", queryS: check };
                                    received.push(temporary);
                                }

                                if (suggest.hasChildNodes()) {
                                    for (var k = 0; k < suggest.childNodes.length; k++) {
                                        //console.log($("#livesearch")[k]);
                                        if ($("#livesearch")[k].children[0].children[0].innerHTML !== check)
                                            $("#livesearch").append(childs);
                                    }
                                }
                                else
                                    $("#livesearch").append(childs);

                                if (suggest.innerHTML !== "") {
                                    suggest.style.border = "1px solid #A5ACB2";
                                }
                                else {
                                    suggest.style.border = "0px";
                                }
                            }
                        }
                    }
                    else
                    {
                        var checkRec = false;
                        var check;
                        var first = String(jsons.Email);
                        var second = String(jsons.CompanyName);
                        if (first.toUpperCase().match(str.toUpperCase()))
                            check = jsons.Email;
                        else if (second.toUpperCase().match(str.toUpperCase()))
                            check = jsons.CompanyName;
                        console.log(check);
                        if (check !== null) {
                            var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Companies</p></div>';
                            console.log(suggest.hasChildNodes());

                            if (received !== null) {
                                for (var m = 0; m < received.length; m++) {
                                    if (received[m].found.Id === jsons.Id)
                                        checkRec = true;
                                }
                                if (checkRec === false) {
                                    var temporary = { found: jsons, type: "Companies", queryS: check };
                                    received.push(temporary);
                                }

                            }
                            else {
                                var temporary = { found: jsons, type: "Companies", queryS: check };
                                received.push(temporary);
                            }

                            if (suggest.hasChildNodes()) {
                                for (var k = 0; k < suggest.childNodes.length; k++) {
                                    //console.log($("#livesearch")[k]);
                                    if ($("#livesearch")[k].children[0].children[0].innerHTML !== check)
                                        $("#livesearch").append(childs);
                                }
                            }
                            else
                                $("#livesearch").append(childs);

                            if (suggest.innerHTML !== "") {
                                suggest.style.border = "1px solid #A5ACB2";
                            }
                            else {
                                suggest.style.border = "0px";
                            }
                        }
                    }

                    console.log(received);
                }
            });
        }

    }

    $("#livesearch").on('click', '.searchcont', function () {
        //console.log(this);
        //console.log($(this)[0].children[0].innerHTML);
        $(this).css("color", "#0000CC");
        if (suggest.childNodes.length === 1)
            suggest.style.display = "none";
        document.getElementById("srcinput").value = $(this)[0].children[0].innerHTML;
        console.log(received);
        $("#listingW").empty();
        $("#listingC").empty();

        if (received !== null)
        {
            var j = 1, m = 1;
            for (var i = 0; i < received.length; i++)
            {
                if (received[i].type === "Workers")
                {
                    $("#listW").css("display", "block");
                    var mail = received[i].found.Email;
                    var first = received[i].found.FirstName;
                    var last = received[i].found.LastName;
                    var company = received[i].found.CompanyName;
                    var skill = null;
                    if (received[i].found.Skills !== null)
                    {
                        skill = received[i].found.Skills.join();                           
                    }
                    var table = '<tr><th scope= "row">' + j + '</th>'
                        + '<td>' + first + '</td>'
                        + '<td>' + last + '</td>'
                        + '<td>' + mail + '</td>'
                        + '<td>' + skill + '</td>'
                        + '<td>' + company + '</td></tr>';
                    $("#listingW").append(table);
                    j = j + 1;
                }
                else if (received[i].type === "Companies")
                {
                    $("#listC").css("display", "block");
                    var company = received[i].found.CompanyName;
                    var mail = received[i].found.Email;
                    var type = received[i].found.Type;
                    var loc = received[i].found.Location;
                    var table = '<tr><th scope= "row">' + j + '</th>'
                        + '<td>' + company + '</td>'
                        + '<td>' + mail + '</td>'
                        + '<td>' + type + '</td>'
                        + '<td>' + loc + '</td></tr>';
                    $("#listingC").append(table);
                    m = m + 1;
                }
                

            }
        }

    });

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

        var search = $("#srcinput").val();
        suggest.style.display = "none";

        var name = { 'name': search };
        received = [];
        //suggest.style.display = "block";

        //pretraga po radnicima
        for (var l = 0; l < urlA.length; l++) {
            getAjaxResponse(urlA[l], name, function (data) {
                var xmldocs = $.parseXML(data),
                    $xmls = $(xmldocs),
                    $titles = $xmls.find("string");
                console.log(data);

                if ($titles.text() == "Worker not found!") {
                    //alert("Nothing found!");
                }
                else {
                    $("#listingW").empty();
                    //$("#listW").css("display", "block");
                    var jsons = JSON.parse($titles.text());

                    var j = 1;
                    if (Array.isArray(jsons)) {
                        for (var i = 0; i < jsons.length; i++) {
                            var checkRec = false;
                            var check;
                            var first = String(jsons[i].FirstName);
                            var second = String(jsons[i].Email);
                            var third = String(jsons[i].LastName);
                            console.log(first);
                            console.log(first.toUpperCase().match(search.toUpperCase()));

                            if (first.toUpperCase().match(search.toUpperCase()))
                                check = jsons[i].FirstName;
                            if (second.toUpperCase().match(search.toUpperCase()))
                                check = jsons[i].Email;
                            if (third.toUpperCase().match(search.toUpperCase()))
                                check = jsons[i].LastName;
                            if (jsons[i].Skills !== null) {
                                for (var c = 0; c < jsons[i].Skills.length; c++) {
                                    var temp = String(jsons[i].Skills[c]);
                                    if (temp.toUpperCase().match(search.toUpperCase()))
                                        check = jsons[i].Skills;
                                }
                            }
                            if (check !== null) {
                                var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Workers</p></div>';
                                console.log(suggest.hasChildNodes());

                                if (received !== null) {
                                    for (var m = 0; m < received.length; m++) {
                                        if (received[m].found.Id === jsons[i].Id)
                                            checkRec = true;
                                    }
                                    if (checkRec === false) {
                                        var temporary = { found: jsons[i], type: "Workers", queryS: check };
                                        received.push(temporary);
                                    }

                                }
                                else {
                                    var temporary = { found: jsons[i], type: "Workers", queryS: check };
                                    received.push(temporary);
                                }

                            }
                        }
                    }
                    else {
                        var checkRec = false;
                        var check;
                        var first = String(jsons.FirstName);
                        var second = String(jsons.Email);
                        var third = String(jsons.LastName);
                        console.log(first);
                        console.log(first.toUpperCase().match(search.toUpperCase()));

                        if (first.toUpperCase().match(search.toUpperCase()))
                            check = jsons.FirstName;
                        if (second.toUpperCase().match(search.toUpperCase()))
                            check = jsons.Email;
                        if (third.toUpperCase().match(search.toUpperCase()))
                            check = jsons.LastName;
                        if (jsons[i].Skills !== null) {
                            for (var c = 0; c < jsons[i].Skills.length; c++) {
                                var temp = String(jsons[i].Skills[c]);
                                if (temp.toUpperCase().match(search.toUpperCase()))
                                    check = jsons[i].Skills;
                            }
                        }
                        if (check !== null) {
                            var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Workers</p></div>';
                            console.log(suggest.hasChildNodes());

                            if (received !== null) {
                                for (var m = 0; m < received.length; m++) {
                                    if (received[m].found.Id === jsons.Id)
                                        checkRec = true;
                                }
                                if (checkRec === false) {
                                    var temporary = { found: jsons, type: "Workers", queryS: check };
                                    received.push(temporary);
                                }

                            }
                            else {
                                var temporary = { found: jsons, type: "Workers", queryS: check };
                                received.push(temporary);
                            }

                        }
                    }

                    console.log(received);
                }
            });
        }

        //pretraga po kompanijama
        for (var l = 0; l < urlB.length; l++) {
            getAjaxResponse(urlB[l], name, function (data) {
                var xmldocs = $.parseXML(data),
                    $xmls = $(xmldocs),
                    $titles = $xmls.find("string");
                console.log(data);

                if ($titles.text() == "Company not found!") {
                    //alert("Nothing found!");
                }
                else {
                    $("#listingC").empty();
                    //$("#listC").css("display", "block");
                    var jsons = JSON.parse($titles.text());
                    console.log(jsons.length);

                    var j = 1;
                    if (Array.isArray(jsons)) {
                        for (var i = 0; i < jsons.length; i++) {
                            var checkRec = false;
                            var check;
                            var first = String(jsons[i].Email);
                            var second = String(jsons[i].CompanyName);
                            if (first.toUpperCase().match(search.toUpperCase()))
                                check = jsons[i].Email;
                            else if (second.toUpperCase().match(search.toUpperCase()))
                                check = jsons[i].CompanyName;
                            console.log(check);
                            if (check !== null) {
                                var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Companies</p></div>';
                                console.log(suggest.hasChildNodes());

                                if (received !== null) {
                                    for (var m = 0; m < received.length; m++) {
                                        if (received[m].found.Id === jsons[i].Id)
                                            checkRec = true;
                                    }
                                    if (checkRec === false) {
                                        var temporary = { found: jsons[i], type: "Companies", queryS: check };
                                        received.push(temporary);
                                    }

                                }
                                else {
                                    var temporary = { found: jsons[i], type: "Companies", queryS: check };
                                    received.push(temporary);
                                }

                            }
                        }
                    }
                    else {
                        var checkRec = false;
                        var check;
                        var first = String(jsons.Email);
                        var second = String(jsons.CompanyName);
                        if (first.toUpperCase().match(search.toUpperCase()))
                            check = jsons.Email;
                        else if (second.toUpperCase().match(search.toUpperCase()))
                            check = jsons.CompanyName;
                        console.log(check);
                        if (check !== null) {
                            var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Companies</p></div>';
                            console.log(suggest.hasChildNodes());

                            if (received !== null) {
                                for (var m = 0; m < received.length; m++) {
                                    if (received[m].found.Id === jsons.Id)
                                        checkRec = true;
                                }
                                if (checkRec === false) {
                                    var temporary = { found: jsons, type: "Companies", queryS: check };
                                    received.push(temporary);
                                }

                            }
                            else {
                                var temporary = { found: jsons, type: "Companies", queryS: check };
                                received.push(temporary);
                            }

                        }
                    }

                    console.log(received);
                }
            });
        }

        if (received !== null) {
            //console.log("prosao");
            obrada(received);
        }

    });

    function obrada(recv) {
        var j = 1, m = 1;
        console.log("prosao");
        for (var i = 0; i < recv.length; i++) {
            if (recv[i].type === "Workers") {
                $("#listW").css("display", "block");
                
                var mail = recv[i].found.Email;
                var first = recv[i].found.FirstName;
                var last = recv[i].found.LastName;
                var company = recv[i].found.CompanyName;
                var skill = null;
                if (recv[i].found.Skills !== null) {
                    skill = recv[i].found.Skills.join();
                }
                var table = '<tr><th scope= "row">' + j + '</th>'
                    + '<td>' + first + '</td>'
                    + '<td>' + last + '</td>'
                    + '<td>' + mail + '</td>'
                    + '<td>' + skill + '</td>'
                    + '<td>' + company + '</td></tr>';
                $("#listingW").append(table);
                j = j + 1;
            }
            else if (recv[i].type === "Companies") {
                $("#listC").css("display", "block");
                var company = recv[i].found.CompanyName;
                var mail = recv[i].found.Email;
                var type = recv[i].found.Type;
                var loc = recv[i].found.Location;
                var table = '<tr><th scope= "row">' + j + '</th>'
                    + '<td>' + company + '</td>'
                    + '<td>' + mail + '</td>'
                    + '<td>' + type + '</td>'
                    + '<td>' + loc + '</td></tr>';
                $("#listingC").append(table);
                m = m + 1;
            }

        }
    }

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

