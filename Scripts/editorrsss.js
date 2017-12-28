$(document).ready(function () {

    var formerClick = 0;
    var formerSkil = 0;
    var previous = [];
    var arrSkill = [];
    var rec = 0;
    var formerRemove = 0;
    var element = document.getElementById("former");
    var dbraven = document.getElementById("raven");
    var dbmongo = document.getElementById("mongo");
    var dblabel = document.getElementById("lab1");
    var dblabel2 = document.getElementById("lab2");
    var urlC = "./RavenService.asmx/updateCompanyInRDb";
    var urlDW = "./RavenService.asmx/deleteWorkerWithId";
    var urllls = "./RavenService.asmx/retWorkerFromIdR"; 
    var urlls = "./RavenService.asmx/updateWorkerInRDb";
    var urlDC = "./RavenService.asmx/deleteCompanyWithId";
    var dbase = null;
    var urlTemp = null;
    var urlMWI = "./MongoService.asmx/retWorkerFromId";
    var urlMUW = "./MongoService.asmx/updateWorkerInDb";
    var urlDWM = "./MongoService.asmx/deleteWorkerWithId";
    var urlCUM = "./MongoService.asmx/updateCompanyInDb";
    var urlDCM = "./MongoService.asmx/deleteCompanyWithId";

    if (localStorage.getItem("dbres") !== null) {
        dbase = localStorage.getItem("dbres");
        console.log(dbase);
    }

    if (dbase !== null) {
        console.log(rec);
        if (dbase === "raven") {
            console.log("yup");
            urlTemp = urllls;
            dbraven.checked = true;
            dbmongo.checked = false;
            $("#lab1").addClass("active");
        }
        else {
            urlTemp = urlMWI;
            dbraven.checked = false;
            dbmongo.checked = true;
            $("#lab2").addClass("active");
        }

    }

    if (element !== null)
    {
        
        if (localStorage.getItem("userid") !== null) {
            rec = localStorage.getItem("userid");
            console.log(rec);
        }

        var dataS = { id: rec.toString() }

        if (urlTemp !== null) {
            getAjaxResponse(urlTemp, dataS, function (data) {
                var xmldoc = $.parseXML(data),
                    $xml = $(xmldoc),
                    $title = $xml.find("string");
                var par = JSON.parse($title.text());
                console.log(par);

                if (par.PreviousEmployment !== null) {
                    for (var l = 0; l < par.PreviousEmployment.length; l++) {
                        var t = par.PreviousEmployment[l];

                        formerClick += 1;

                        var addRows = "<div class='rroww'>"
                            + "<div class='row btn-block'>"
                            + "<label class='control-label text-center col-lg-2' style='width: 30%;'>Former company: </label>"
                            + "<label class='control-label text-right col-lg-2' style='width: 30%;'>Start date: </label>"
                            + "<label class='control-label text-right col-lg-2' style='width: 40%;'>End date: </label>"
                            + "</div>"
                            + "<div class='row btn-block'>"
                            + "<input type = 'text' name='firma' class='form-control formerelf col-lg-2' style='width: 37%; padding-right: 10px; margin-right: 0.5em' id='firma" + formerClick + "' data-minlength='2' placeholder='Company' value='" + t.FirmName + "' required>"
                            + "<input type = 'date' name='dates' class='form-control formerelr col-lg-2' style='width: 30%; padding-right: 10px; margin-right: 0.5em' id='dates" + formerClick + "' placeholder='Start date' value='" + t.StartTime + "' required>"
                            + "<input type = 'date' name='datee' class='form-control formerelr col-lg-2' style='width: 30%; margin-bottom: 0.5em' id='datee" + formerClick + "' placeholder='End date' value='" + t.EndTime + "' required>"
                            + "<button type='button' class='formerr btn btn-default text-center' id='rem" + formerClick + "'>Remove refference</button>"
                            + "</div><hr/></div>";
                        $('#former').append(addRows);

                    }
                }

                if (par.Skills !== null) {
                    for (var p = 0; p < par.Skills.length; p++) {
                        var m = par.Skills[p];

                        formerSkil += 1;

                        var addRow = "<div class='rrowwss'>"
                            + "<div class='row btn-block'>"
                            + "<input type = 'text' name='firma' class='form-control formerelf col-lg-2' style='width: 50%; padding-right: 10px; margin-right: 0.5em' id='skills" + formerSkil + "' data-minlength='2' placeholder='Skill' value='" + m + "' required>"
                            + "<button type='button' class='formers btn btn-default text-center' style='width: 35%; margin-bottom: 0.5em' id='rems" + formerSkil + "'>Remove Skill</button>"
                            + "</div><hr/></div>";
                        $('#skil').append(addRow);

                    }
                }

                var sign = "No users were found with this email, please sign up!";
                if ($title.text() === sign) {
                    console.log(sign);
                }

            });
        }

    }

    $("#change").click(function (e) {

        e.preventDefault();

        var res;
        

        if (localStorage.getItem("userid") !== null) {
            res = localStorage.getItem("userid");
            console.log(res);
        }

        var email = $("#email").val();
        var password = $("#password").val();
        var reppass = $("#repeatedpwd").val();
        var checkbox = $("#remember").val();
        var name = $("#firstname").val();
        var surname = $("#lastname").val();
        var company = $("#firm").val();
        var empty = null;
        var emptyS = null;
        var warn = null;
        var urlTemps = null;
        var dbc = "raven";

        if (dbraven.checked === true)
            dbc = dbraven.value;
        else
            dbc = dbmongo.value;

        if (formerSkil > 0) {

            for (var i = 1; i <= formerSkil; i++) {
                var sS1 = "#skills" + i.toString();
                if (sS1 === '')
                    warn = "Enter skill name!";
                var tempS = $(sS1).val();
                arrSkill.push(tempS);     
                console.log(tempS);
                    
            }
            emptyS = JSON.stringify(arrSkill);
        }

        console.log(emptyS);

        if (formerClick > 0) {

            for (var b = 1; b <= formerClick; b++) {
                var s1 = "#firma" + b.toString();
                if (s1 === '')
                    warn = "Enter previous firm name!";
                var s2 = "#dates" + b.toString();
                var s3 = "#datee" + b.toString();
                var temp = { firm: $(s1).val(), dates: $(s2).val(), datee: $(s3).val() };
                previous.push(temp);
                console.log(temp);

            }
            empty = JSON.stringify(previous);
        }

        console.log(empty); 
        console.log(dbc); 
        

        if (dbase !== null) {
            if (dbase === "raven") {
                urlTemps = urlls;
            }
            else {
                urlTemps = urlMUW;
            }

        }

        console.log(localStorage.getItem("dbres"));
        console.log(urlTemps); 

        //pravimo data string
        var dataString = { 'id': res.toString(), 'mail': email, 'pass': password, 'name': name, 'last': surname, 'company': company, 'previous': empty, 'skills': emptyS, 'dbch': dbc };

        if (email === '' || password === '' || name === '' || surname === '' || reppass === '' || company === '' || warn !== null ) {
            previous = [];
            arrSkill = [];
            alert("Please fill all fields!");
        }
        else if (password !== reppass) {
            alert("Password doesn't match, enter it again!");

        }
        else {
                if (urlTemps !== null) {
                    getAjaxResponse(urlTemps, dataString, function (data) {
                        var xmldoc = $.parseXML(data),
                            $xml = $(xmldoc),
                            $title = $xml.find("string");
                        console.log($title.text());

                        var sign = "No users were found with this email, please sign up!";
                        if ($title.text() === sign) {
                            console.log(sign);
                        }
                        else {
                            alert("Update Success!");
                            window.location.assign("./profileEditor.aspx");
                        }
                    });
                }
        }

        return false;
    });

    $("#delete").click(function (e) {

        e.preventDefault();

        var res;
        var urlTempd = null;

        if (dbase !== null) {
            if (dbase === "raven") {
                urlTempd = urlDW;
            }
            else {
                urlTempd = urlDWM;
            }

        }

        if (localStorage.getItem("userid") !== null) {
            res = localStorage.getItem("userid");
            console.log(res);
        }

        var dataString = { 'id': res.toString() };
        if (urlTempd != null) {
            getAjaxResponse(urlTempd, dataString, function (data) {
                var xmldoc = $.parseXML(data),
                    $xml = $(xmldoc),
                    $title = $xml.find("string");
                console.log($title.text());

                var sign = "Worker deleted!";
                if ($title.text() === sign) {
                    alert("Deleting worker was successful!");
                    window.location.assign("./logout.aspx");
                }
                else {
                    alert($title.text());
                }

            });
        }
    });


    //dodavanje prethodnih zaposlenja
    $("#addformer").click(function (e) {

        e.preventDefault();

        formerClick += 1;

        var addRow = "<div class='rroww'>"
            + "<div class='row btn-block'>"
            + "<label class='control-label text-center col-lg-2' style='width: 37%;'>Former company: </label>"
            + "<label class='control-label text-right col-lg-2' style='width: 32%;'>Start date: </label>"
            + "<label class='control-label text-right col-lg-2' style='width: 30%;'>End date: </label>"
            + "</div>"
            + "<div class='row btn-block'>"
            + "<input type = 'text' name='firma' class='form-control formerelf col-lg-2' style='width: 37%; padding-right: 10px; margin-right: 0.5em' id='firma" + formerClick + "' data-minlength='2' placeholder='Company' required>"
            + "<input type = 'date' name='dates' class='form-control formerelr col-lg-2' style='width: 30%; padding-right: 10px; margin-right: 0.5em' id='dates" + formerClick + "' placeholder='Start date' required>"
            + "<input type = 'date' name='datee' class='form-control formerelr col-lg-2' style='width: 30%; margin-bottom: 0.5em' id='datee" + formerClick + "' placeholder='End date' required>"
            + "<button type='button' class='formerr btn btn-default text-center' id='rem" + formerClick + "'>Remove refference</button>"
            + "</div><hr/></div>";
        $('#former').append(addRow);


    });

    //brise jedan stari posao
    $('#former').on('click', '.formerr', function () {
        
        console.log($(this).closest("div.rroww"));
        $(this).closest("div.rroww")[0].remove();

        formerClick -= 1;

    });

    //dodaje jedan skill
    $("#addskil").click(function (e) {

        e.preventDefault();

        formerSkil += 1;

        var addRow = "<div class='rrowwss'>"
            + "<div class='row btn-block'>"
            + "<input type = 'text' name='firma' class='form-control formerelf col-lg-2' style='width: 50%; padding-right: 10px; margin-right: 0.5em' id='skills" + formerSkil + "' data-minlength='2' placeholder='Skill' required>"
            + "<button type='button' class='formers btn btn-default text-center' style='width: 35%; margin-bottom: 0.5em' id='rems" + formerSkil + "'>Remove Skill</button>"
            + "</div><hr/></div>";
        $('#skil').append(addRow);


    });

    //brise jedan skill
    $('#skil').on('click', '.formers', function () {

        console.log($(this).closest("div.rrowwss"));
        $(this).closest("div.rrowwss")[0].remove();

        formerSkil -= 1;

    });

    //kompanije
    $("#changeCom").click(function (e) {

        e.preventDefault();

        var res;
        var dbc = "raven";

        if (dbraven.checked === true)
            dbc = dbraven.value;
        else
            dbc = dbmongo.value;

        if (localStorage.getItem("companyid") !== null) {
            res = localStorage.getItem("companyid");
            console.log(res);
        }

        var email = $("#emails").val();
        var company = $("#companyname").val();
        var owner = $("#owner").val();
        var type = $("#type").val();
        var location = $("#location").val();
        var password = $("#passwords").val();
        var reppass = $("#repeatedpwds").val();
        var checkbox = $("#remembers").val();
        var urlTempc = null;

        if (dbase !== null) {
            if (dbase === "raven") {
                urlTempc = urlC;
            }
            else {
                urlTempc = urlCUM;
            }

        }

        //pravimo data string
        var dataString = { 'id': res.toString(), 'mail': email, 'pass': password, 'name': company, 'owner': owner, 'type': type, 'loc': location, 'dbch': dbc };

        if (email === '' || password === '' || company === '' || owner === '' || type === '' || location === '' || reppass === '') {
            alert("Please fill all fields!");
        }
        else if (password !== reppass) {
            alert("Password doesn't match, enter it again!");

        }
        else {
                if (urlTempc !== null) {
                    getAjaxResponse(urlTempc, dataString, function (data) {
                            var xmldoc = $.parseXML(data),
                                $xml = $(xmldoc),
                                $title = $xml.find("string");
                            console.log($title.text());

                            var sign = "No users were found with this email, please sign up!";
                            if ($title.text() === sign) {
                                console.log(sign);
                            }
                            else {
                                alert("Update Success!");
                                window.location.assign("./profileEditor.aspx");
                            }
                    });
                }
        }

        return false;
    });

    $("#deleteCom").click(function (e) {

        e.preventDefault();

        var res;
        var urlTempdc = null;

        if (dbase !== null) {
            if (dbase === "raven") {
                urlTempdc = urlDC;
            }
            else {
                urlTempdc = urlDCM;
            }

        }

        if (localStorage.getItem("companyid") !== null) {
            res = localStorage.getItem("companyid");
            console.log(res);
        }

        var dataString = { 'id': res.toString() };
        if (urlTempdc != null) {
            getAjaxResponse(urlTempdc, dataString, function (data) {
                var xmldoc = $.parseXML(data),
                    $xml = $(xmldoc),
                    $title = $xml.find("string");
                console.log($title.text());

                var sign = "Company deleted!";
                if ($title.text() === sign) {
                    alert("Deleting company was successful!");
                    window.location.assign("./logout.aspx");
                }

            });
        }
    });

    function getAjaxResponse(urll, sstring, fn) {

        $.ajax({
            //url: "./MongoService.asmx/updateWorkerInDb",
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