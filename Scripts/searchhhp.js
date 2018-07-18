$(document).ready(function () {
    let received = [];
    let shown = [];
    let perm1 = 1, perm2 = 2, perm3 = 3, perm4 = 4, perm5 = 5;
    //perm1 = perm2 = perm3 = perm4 = perm5 = true;
    var count = 0;
    //db
    let basee = localStorage.getItem("dbres");

    //urlovi
    //var urlWMNP = "./RavenService.asmx/returnWorkerFromEmailNoPass";
    //var urlCMNP = "./RavenService.asmx/returnCompanyFromEmailNoPass";
    //var urlCN = "./RavenService.asmx/retCompanyFromName";
    //var urlWN = "./RavenService.asmx/retWorkerFromName";
    //var urlWLN = "./RavenService.asmx/retWorkerFromLastName";
    //var urlWS = "./RavenService.asmx/retWorkerWithSkill";
    //var urlA = [urlWMNP, urlWN, urlWLN, urlWS];
    //var urlB = [urlCMNP, urlCN];
    //mongo
    var urlWMNPM = "./MongoService.asmx/returnWorkerFromEmailNoPassSrc";
    var urlCMNPM = "./MongoService.asmx/returnCompanyFromEmailNoPassSrc";
    var urlCNM = "./MongoService.asmx/retCompanyFromNameSrc";
    var urlWNM = "./MongoService.asmx/retWorkerFromNameSrc";
    var urlWLNM = "./MongoService.asmx/retWorkerFromLastNameSrc";
    var urlWSM = "./MongoService.asmx/retWorkerWithSkill";
    var urlAM = [urlWMNPM, urlWNM, urlWLNM, urlWSM];
    var urlBM = [urlCMNPM, urlCNM];

    var suggest = document.getElementById("livesearch");
    var timeoutID = null;

    var urlOrderTemp1 = null;
    var urlOrderTemp2 = null;

    //if (basee === "raven" || basee === null)
    //{
    //    urlOrderTemp1 = urlA;
    //    urlOrderTemp2 = urlB;
    //}
    //else
    //{
    //    urlOrderTemp1 = urlAM;
    //    urlOrderTemp2 = urlBM;
    //}

    function findMember(propose, str) {
        console.log('search: ' + str);
        received = [];
        suggest.style.display = "block";
        $("#livesearch").empty();
        var all = true;
        var prep1 = [];
        var prep2 = [];
        count = 0;

        if ($('#ffirst').not(':checked').length) {
            perm1 = 0;
        }
        else {
            perm1 = 1;
            all = false;
        }

        if ($('#llast').not(':checked').length) {
            perm2 = 0;
        }
        else {
            perm2 = 2;
            all = false;
            console.log(perm2);
        }

        if ($('#mmail').not(':checked').length) {
            perm3 = 0;
        }
        else {
            perm3 = 3;
            all = false;
        }

        if ($('#ccomp').not(':checked').length) {
            perm4 = 0;;
        }
        else {
            perm4 = 4;
            all = false;
        }

        if ($('#skiill').not(':checked').length) {
            perm5 = 0;
        }
        else {
            perm5 = 5;
            all = false;
        }

        if (all === true)
        {
            prep1 = [1, 2, 3, 4, 5];
            //prep2 = [true, true];
        }
        else
        {
            prep1 = [perm1, perm2, perm3, perm4, perm5];
            //prep2 = [perm3, perm4];
        }

        console.log(prep1);

        getAjaxResponse(JSON.stringify(prep1), str, (data) => {
            let xmldoc = $.parseXML(data),
                $xml = $(xmldoc),
                $title = $xml.find("string");
            let par = JSON.parse($title.text());

            let tmp = [];

            par.forEach((el, ind) => {
                if (el[0] === "[") {
                    tmp.push({'recv': JSON.parse(el), 'ind': ind });
                }
                    
            });
            console.log(tmp);
            if (tmp.length === 0) alert("Nothing found!");
            else {
                let suggested = [];
                received = [];

                if (str === "") {

                } else {
                    tmp.forEach((el) => {
                        switch (el.ind) {
                            case 0:
                                el.recv.forEach((elTmp) => {
                                    received.push(elTmp);
                                    suggested.push({
                                        'Id': elTmp.Id,
                                        'val': elTmp.FirstName,
                                        'collection': 'worker'
                                    });
                                });
                                break;
                            case 1:
                                el.recv.forEach((elTmp) => {
                                    received.push(elTmp);
                                    suggested.push({
                                        'Id': elTmp.Id,
                                        'val': elTmp.LastName,
                                        'collection': 'worker'
                                    });
                                });
                                break;
                            case 2:
                                el.recv.forEach((elTmp) => {
                                    received.push(elTmp);
                                    suggested.push({
                                        'Id': elTmp.Id,
                                        'val': elTmp.Email,
                                        'collection': 'worker'
                                    });
                                });
                                break;
                            case 3:
                                el.recv.forEach((elTmp) => {
                                    received.push(elTmp);
                                    suggested.push({
                                        'Id': elTmp.Id,
                                        'val': elTmp.Email,
                                        'collection': 'company'
                                    });
                                });
                                break;
                            case 4:
                                el.recv.forEach((elTmp) => {
                                    received.push(elTmp);
                                    suggested.push({
                                        'Id': elTmp.Id,
                                        'val': elTmp.CompanyName,
                                        'collection': 'company'
                                    });
                                });
                                break;
                            case 5:
                                el.recv.forEach((elTmp) => {
                                    received.push(elTmp);
                                    suggested.push({
                                        'Id': elTmp.Id,
                                        'val': elTmp.Skills,
                                        'collection': 'worker'
                                    });
                                });
                                break;
                        }
                    });
                }

                //sklanjanje duplikata
                suggested = eliminateDuplicates(suggested);
                received = eliminateDuplicates(received);
                console.log(suggested);
                console.log(received);
            }

        });

        ////pretraga po radnicima
        //for (var l = 0; l < urlOrderTemp1.length; l++) {
        //    if (prep1[l] === true) {
        //        getAjaxResponse(urlOrderTemp1[l], name, function (data) {
        //            var xmldocs = $.parseXML(data),
        //                $xmls = $(xmldocs),
        //                $titles = $xmls.find("string");
        //            console.log(data);

        //            if ($titles.text() == "Worker not found!") {
        //                //alert("Nothing found!");
        //            }
        //            else {
        //                $("#listingW").empty();
        //                //$("#listW").css("display", "block");
        //                var jsons = JSON.parse($titles.text());

        //                var j = 1;
        //                if (Array.isArray(jsons)) {
        //                    for (var i = 0; i < jsons.length; i++) {
        //                        var checkRec = false;
        //                        var check;
        //                        var first = String(jsons[i].FirstName);
        //                        var second = String(jsons[i].Email);
        //                        var third = String(jsons[i].LastName);
        //                        console.log(first);
        //                        console.log(first.toUpperCase().match(str.toUpperCase()));

        //                        if (first.toUpperCase().match(str.toUpperCase()))
        //                            check = jsons[i].FirstName;
        //                        if (second.toUpperCase().match(str.toUpperCase()))
        //                            check = jsons[i].Email;
        //                        if (third.toUpperCase().match(str.toUpperCase()))
        //                            check = jsons[i].LastName;
        //                        if (jsons[i].Skills !== null) {
        //                            for (var c = 0; c < jsons[i].Skills.length; c++) {
        //                                var temp = String(jsons[i].Skills[c]);
        //                                if (temp.toUpperCase().match(str.toUpperCase()))
        //                                    check = jsons[i].Skills;
        //                            }
        //                        }

        //                        if (check !== null) {
        //                            var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Workers</p></div>';
        //                            console.log(suggest.hasChildNodes());

        //                            if (received.length !== 0) {
        //                                for (var m = 0; m < received.length; m++) {
        //                                    if (received[m].found.Id === jsons[i].Id && received[m].found.Email === jsons[i].Email)
        //                                        checkRec = true;
        //                                }
        //                                if (checkRec === false) {
        //                                    var temporary = { found: jsons[i], type: "Workers", queryS: check };
        //                                    received.push(temporary);
        //                                    count++;
        //                                }

        //                            }
        //                            else {
        //                                var temporary = { found: jsons[i], type: "Workers", queryS: check };
        //                                received.push(temporary);
        //                                count++;
        //                            }

        //                            if (propose === true) {

        //                                if (suggest.childNodes.length !== 0) {
        //                                    for (var k = 0; k < suggest.childNodes.length; k++) {
        //                                        //console.log($("#livesearch")[k]);
        //                                        if ($("#livesearch")[k].children[0].children[0].innerHTML !== check)
        //                                            $("#livesearch").append(childs);
        //                                    }
        //                                }
        //                                else
        //                                    $("#livesearch").append(childs);

        //                                if (suggest.innerHTML !== "") {
        //                                    suggest.style.border = "1px solid #A5ACB2";
        //                                }
        //                                else {
        //                                    suggest.style.border = "0px";
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else {
        //                    var checkRec = false;
        //                    var check;
        //                    var first = String(jsons.FirstName);
        //                    var second = String(jsons.Email);
        //                    var third = String(jsons.LastName);
        //                    console.log(first);
        //                    console.log(first.toUpperCase().match(str.toUpperCase()));

        //                    if (first.toUpperCase().match(str.toUpperCase()))
        //                        check = jsons.FirstName;
        //                    if (second.toUpperCase().match(str.toUpperCase()))
        //                        check = jsons.Email;
        //                    if (third.toUpperCase().match(str.toUpperCase()))
        //                        check = jsons.LastName;
        //                    if (jsons[i].Skills !== null) {
        //                        for (var c = 0; c < jsons[i].Skills.length; c++) {
        //                            var temp = String(jsons[i].Skills[c]);
        //                            if (temp.toUpperCase().match(str.toUpperCase()))
        //                                check = jsons[i].Skills;
        //                        }
        //                    }


        //                    if (check !== null) {
        //                        var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Workers</p></div>';
        //                        console.log(suggest.hasChildNodes());

        //                        if (received.length !== 0) {
        //                            for (var m = 0; m < received.length; m++) {
        //                                if (received[m].found.Id === jsons.Id)
        //                                    checkRec = true;
        //                            }
        //                            if (checkRec === false) {
        //                                var temporary = { found: jsons, type: "Workers", queryS: check };
        //                                received.push(temporary);
        //                                count++;
        //                            }

        //                        }
        //                        else {
        //                            var temporary = { found: jsons, type: "Workers", queryS: check };
        //                            received.push(temporary);
        //                            count++;
        //                        }

        //                        if (propose === true) {

        //                            if (suggest.childNodes.length !== 0) {
        //                                for (var k = 0; k < suggest.childNodes.length; k++) {
        //                                    //console.log($("#livesearch")[k]);
        //                                    if ($("#livesearch")[k].children[0].children[0].innerHTML !== check)
        //                                        $("#livesearch").append(childs);
        //                                }
        //                            }
        //                            else
        //                                $("#livesearch").append(childs);

        //                            if (suggest.innerHTML !== "") {
        //                                suggest.style.border = "1px solid #A5ACB2";
        //                            }
        //                            else {
        //                                suggest.style.border = "0px";
        //                            }
        //                        }
        //                    }
        //                }

        //                console.log(received);
        //            }
        //        });
        //    }
        //}

        ////pretraga po kompanijama
        //for (var l = 0; l < urlOrderTemp2.length; l++) {
        //    if (prep2[l] === true) {
        //        getAjaxResponse(urlOrderTemp2[l], name, function (data) {
        //            var xmldocs = $.parseXML(data),
        //                $xmls = $(xmldocs),
        //                $titles = $xmls.find("string");
        //            console.log(data);

        //            if ($titles.text() == "Company not found!") {
        //                //alert("Nothing found!");
        //            }
        //            else {
        //                $("#listingC").empty();
        //                //$("#listC").css("display", "block");
        //                var jsons = JSON.parse($titles.text());
        //                console.log(jsons.length);

        //                var j = 1;
        //                if (Array.isArray(jsons)) {
        //                    for (var i = 0; i < jsons.length; i++) {
        //                        var checkRec = false;
        //                        var check;
        //                        var first = String(jsons[i].Email);
        //                        var second = String(jsons[i].CompanyName);
        //                        if (first.toUpperCase().match(str.toUpperCase()))
        //                            check = jsons[i].Email;
        //                        else if (second.toUpperCase().match(str.toUpperCase()))
        //                            check = jsons[i].CompanyName;
        //                        console.log(check);
        //                        if (check !== null) {
        //                            var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Companies</p></div>';
        //                            console.log(suggest.hasChildNodes());

        //                            if (received.length !== 0) {
        //                                for (var m = 0; m < received.length; m++) {
        //                                    if (received[m].found.Id === jsons[i].Id)
        //                                        checkRec = true;
        //                                }
        //                                if (checkRec === false) {
        //                                    var temporary = { found: jsons[i], type: "Companies", queryS: check };
        //                                    received.push(temporary);
        //                                    count++;
        //                                }

        //                            }
        //                            else {
        //                                var temporary = { found: jsons[i], type: "Companies", queryS: check };
        //                                received.push(temporary);
        //                                count++;
        //                            }

        //                            if (propose === true) {

        //                                if (suggest.childNodes.length !== 0) {
        //                                    for (var k = 0; k < suggest.childNodes.length; k++) {
        //                                        //console.log($("#livesearch")[k]);
        //                                        if ($("#livesearch")[k].children[0].children[0].innerHTML !== check)
        //                                            $("#livesearch").append(childs);
        //                                    }
        //                                }
        //                                else
        //                                    $("#livesearch").append(childs);

        //                                if (suggest.innerHTML !== "") {
        //                                    suggest.style.border = "1px solid #A5ACB2";
        //                                }
        //                                else {
        //                                    suggest.style.border = "0px";
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else {
        //                    var checkRec = false;
        //                    var check = null;
        //                    var first = String(jsons.Email);
        //                    var second = String(jsons.CompanyName);
        //                    if (first.toUpperCase().match(str.toUpperCase()))
        //                        check = jsons.Email;
        //                    else if (second.toUpperCase().match(str.toUpperCase()))
        //                        check = jsons.CompanyName;
        //                    console.log(check);
        //                    if (check !== null) {
        //                        var childs = '<div class="row searchcont"><p class="col-lg-3">' + check + '</p><p class="col-lg-3" style="color: #778899;">Companies</p></div>';
        //                        console.log(suggest.hasChildNodes());

        //                        if (received.length !== 0) {
        //                            for (var m = 0; m < received.length; m++) {
        //                                if (received[m].found.Id === jsons.Id)
        //                                    checkRec = true;
        //                            }
        //                            if (checkRec === false) {
        //                                var temporary = { found: jsons, type: "Companies", queryS: check };
        //                                received.push(temporary);
        //                                count++;
        //                            }

        //                        }
        //                        else {
        //                            var temporary = { found: jsons, type: "Companies", queryS: check };
        //                            received.push(temporary);
        //                            count++;
        //                        }

        //                        if (propose === true) {

        //                            if (suggest.childNodes.length !== 0) {
        //                                for (var k = 0; k < suggest.childNodes.length; k++) {
        //                                    //console.log($("#livesearch")[k]);
        //                                    if ($("#livesearch")[k].children[0].children[0].innerHTML !== check)
        //                                        $("#livesearch").append(childs);
        //                                }
        //                            }
        //                            else
        //                                $("#livesearch").append(childs);

        //                            if (suggest.innerHTML !== "") {
        //                                suggest.style.border = "1px solid #A5ACB2";
        //                            }
        //                            else {
        //                                suggest.style.border = "0px";
        //                            }
        //                        }
        //                    }
        //                }

        //                console.log(received);
        //            }
        //        });
            //}
        //}

    }

    function insertIntoTables(rec) {
        $("#listingW").empty();
        $("#listingC").empty();

        if (count !== 0) {
            console.log(count);
            let j = 1, m = 1;
            for (let i = 0; i < count; i++) {
                if (rec[i].type === "Workers") {
                    $("#listW").css("display", "block");
                    let mail = rec[i].found.Email;
                    let first = rec[i].found.FirstName;
                    let last = rec[i].found.LastName;
                    let company = rec[i].found.CompanyName;
                    let skill = null;
                    if (rec[i].found.Skills !== null) {
                        skill = rec[i].found.Skills.join();
                    }
                    let table = '<tr><th scope= "row">' + j + '</th>'
                        + '<td>' + first + '</td>'
                        + '<td>' + last + '</td>'
                        + '<td>' + mail + '</td>'
                        + '<td>' + skill + '</td>'
                        + '<td>' + company + '</td>'
                        + '<td><button type="button" class="view btn btn-default" id="addd' + j + '">View</button></td></tr >';
                    $("#listingW").append(table);
                    j = j + 1;
                }
                else if (rec[i].type === "Companies") {
                    $("#listC").css("display", "block");
                    let company = rec[i].found.CompanyName;
                    let mail = rec[i].found.Email;
                    let type = rec[i].found.Type;
                    let loc = rec[i].found.Location;
                    let table = '<tr><th scope= "row">' + j + '</th>'
                        + '<td>' + company + '</td>'
                        + '<td>' + mail + '</td>'
                        + '<td>' + type + '</td>'
                        + '<td>' + loc + '</td>'
                        + '<td> <button type="button" class="view btn btn-default" id="adddd' + m + '">View</button></td></tr>';
                    $("#listingC").append(table);
                    m = m + 1;
                }


            }
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

        insertIntoTables(received);
    });

    //kako se kuca tekst tako se i pretraga vrsi
    $("#srcinput").keyup(function (e) {
        clearTimeout(timeoutID);
        timeoutID = setTimeout(findMember.bind(undefined, true, e.target.value), 500);
    });

    //sta se desava kad se klikne dugme go za pregtragu
    $("#srcbttn").click(function (e) {

        e.preventDefault();

        $("#listingC").empty();
        $("#listingW").empty();

        var search = $("#srcinput").val();
        suggest.style.display = "none";

        findMember.bind(undefined, false, search);

        console.log(received);
        if (received.length !== 0)
            insertIntoTables(received);

    });

    $('#listingC').on('click', '.view', function () {
        console.log(this);
        //console.log(received[0].children[6].innerHTML);
        console.log($(this).closest("tr")[0].children[2].innerHTML);
        for (var i = 0; i < received.length; i++) {
            if (received[i].found.Email === $(this).closest("tr")[0].children[2].innerHTML) {
                if (basee === "raven")
                    localStorage.setItem("companyViewR", JSON.stringify(received[i].found.Id));
                else
                    localStorage.setItem("companyView", JSON.stringify(received[i].found.Id));
            }
        }
        window.location.assign("./companyInfo.aspx");

    });

    $('#listingW').on('click', '.view', function () {
        for (var i = 0; i < received.length; i++) {
            if (received[i].found.Email === $(this).closest("tr")[0].children[3].innerHTML) {
                console.log($(this).closest("tr")[0].children[3].innerHTML);
                if (basee === "raven")
                    localStorage.setItem("workerViewR", received[i].found.Id);
                else
                    localStorage.setItem("workerView", received[i].found.Id);
            }
        }
        window.location.assign("./fellowworker.aspx");

    });

    function eliminateDuplicates(arr) {
        let tmpNoDoubles = [];
        let i = 0;
        while (i < arr.length) {
            let tmpEl = arr[i];
            tmpNoDoubles.push(tmpEl);
            arr.forEach((el) => {
                if (el.Id !== tmpEl.Id) tmpNoDoubles.push(el);
            });
            arr = tmpNoDoubles;
            tmpNoDoubles = [];
            i++;
        }
        return arr;
    }

    function getAjaxResponse(arr, sstring, fn) {

        $.ajax({
            url: './RavenService.asmx/searchAll',
            dataType: "text",
            type: "POST",
            data: { 'word': sstring, 'check': arr },
            error: function (err) {
                alert("Error", err);
            },
            success: function (data) {
                fn(data);
            }
        });
    }

});