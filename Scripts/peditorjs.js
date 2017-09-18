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

                var sign = "no users were found with this email, please sign up!";
                if ($title.text() === sign) {
                    console.log("");
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

        getAjaxResponse(dataString, function (data) {
            var xmldoc = $.parseXML(data),
                $xml = $(xmldoc),
                $title = $xml.find("string");
            console.log($title.text());

            var sign = "Worker deleted!";
            if ($title.text() === sign) {
                alert("Deleting worker was successful!");
                window.location.assign("./index.aspx");
            }

        });
    });

    function getAjaxResponse(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/updateWorkerInDb",
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
    
});