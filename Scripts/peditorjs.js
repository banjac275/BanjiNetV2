$(document).ready(function () {

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

        //pravimo data string
        var dataString = { 'id': res.toString(), 'mail': email, 'pass': password, 'name': name, 'last': surname, 'check': checkbox, 'company': company };

        if (email === '' || password === '' || name === '' || surname === '' || reppass === '' || company === '') {
            alert("Please fill all fields!");
        }
        else if (password !== reppass) {
            alert("Password doesn't match, enter it again!");

        }
        else {
            getAjaxResponse(dataString, function (data) {
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
            else
            {
                alert($title.text());
            }

        });
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
        var dataString = { 'id': res.toString(), 'mail': email, 'pass': password, 'name': company, 'owner': owner, 'type': type, 'loc': location, 'check': checkbox };

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

    function getAjaxResponse(sstring, fn) {

        $.ajax({
            //url: "./MongoService.asmx/updateWorkerInDb",
            url: "./RaptorService.asmx/updateWorkerInRDb",
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
            url: "./MongoService.asmx/updateCompanyInDb",
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