$(document).ready(function () {

    var formerClick = 0;
    var previous = [];
    var rec = 0;
    var formerRemove = 0;

    $("#change").click(function (e) {

        e.preventDefault();

        var res;
        var urlls = "./RavenService.asmx/updateWorkerInRDb";

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
        var warn = null;

        if (formerClick > 0 && formerClick > rec)
        {
            for (var i = rec + 1; i <= formerClick; i++)
            {
                var s1 = "#firma" + i.toString();
                if (s1 === '')
                    warn = "Enter previous firm name!";
                var s2 = "#dates" + i.toString();
                var s2 = "#datee" + i.toString();
                var temp = { firm: $(s1).val(), dates: $(s2).val(), datee: $(s2).val() };
                previous.push(temp);
            }

            empty = JSON.stringify(previous);
        }




        //pravimo data string
        var dataString = { 'id': res.toString(), 'mail': email, 'pass': password, 'name': name, 'last': surname, 'company': company, 'previous': empty };

        if (email === '' || password === '' || name === '' || surname === '' || reppass === '' || company === '' || warn !== null ) {
            alert("Please fill all fields!");
        }
        else if (password !== reppass) {
            alert("Password doesn't match, enter it again!");

        }
        else {
            getAjaxResponse(urlls, dataString, function (data) {
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

        return false;
    });

    $("#delete").click(function (e) {

        e.preventDefault();

        var res;

        if (localStorage.getItem("userid") !== null) {
            res = localStorage.getItem("userid");
            console.log(res);
        }

        var dataString = { 'id': res.toString() };

        getAjaxResponseDelete(dataString, function (data) {
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
    });


    //dodavanje prethodnih zaposlenja
    $("#addformer").click(function (e) {

        e.preventDefault();

        formerClick += 1;

        var addRow = "<div class='rroww'>"
            + "<div class='row btn-block'>"
            + "<label class='control-label text-center col-lg-2' style='width: 30%;'>Former company: </label>"
            + "<label class='control-label text-right col-lg-2' style='width: 30%;'>Start date: </label>"
            + "<label class='control-label text-right col-lg-2' style='width: 40%;'>End date: </label>"
            + "</div>"
            + "<div class='row btn-block'>"
            + "<input type = 'text' name='firma' class='form-control formerelf col-lg-2' style='width: 37%; padding-right: 10px; margin-right: 0.5em' id='firma" + formerClick + "' data-minlength='2' placeholder='Company' required>"
            + "<input type = 'date' name='dates' class='form-control formerelr col-lg-2' style='width: 30%; padding-right: 10px; margin-right: 0.5em' id='dates" + formerClick + "' placeholder='Start date' required>"
            + "<input type = 'date' name='datee' class='form-control formerelr col-lg-2' style='width: 30%; margin-bottom: 0.5em' id='datee" + formerClick + "' placeholder='End date' required>"
            + "<button type='button' class='formerr btn btn-default text-center' id='rem" + formerClick + "'>Remove refference</button>"
            + "</div><hr/></div>";
        $('#former').append(addRow);


    });

    $('#former').on('click', '.formerr', function () {
        //alert('triggered');
        console.log($(this).closest("div.rroww"));
        $(this).closest("div.rroww")[0].remove();
        //console.log(friendss);
        //for (var i = 0; i < friendss.length; i++) {
        //    if (friendss[i].Email === $(this).closest("tr")[0].children[3].innerHTML)
        //        localStorage.setItem("workerViewR", friendss[i].Id);
        //}
    });

    //kompanije
    $("#changeCom").click(function (e) {

        e.preventDefault();

        var res;

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

        //pravimo data string
        var dataString = { 'id': res.toString(), 'mail': email, 'pass': password, 'name': company, 'owner': owner, 'type': type, 'loc': location };

        if (email === '' || password === '' || company === '' || owner === '' || type === '' || location === '' || reppass === '') {
            alert("Please fill all fields!");
        }
        else if (password !== reppass) {
            alert("Password doesn't match, enter it again!");

        }
        else {
            getAjaxResponseCompany(dataString, function (data) {
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

        return false;
    });

    $("#deleteCom").click(function (e) {

        e.preventDefault();

        var res;

        if (localStorage.getItem("companyid") !== null) {
            res = localStorage.getItem("companyid");
            console.log(res);
        }

        var dataString = { 'id': res.toString() };

        getAjaxResponseDeleteCompany(dataString, function (data) {
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

    function getAjaxResponseDelete(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/deleteWorkerWithId",
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

    function getAjaxResponseCompany(sstring, fn) {

        $.ajax({
            //url: "./MongoService.asmx/updateCompanyInDb",
            url: "./RaptorService.asmx/updateCompanyInRDb",
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

    function getAjaxResponseDeleteCompany(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/deleteCompanyWithId",
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

});