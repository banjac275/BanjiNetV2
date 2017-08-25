$(document).ready(function() {
    $("#signinbtn").click(function (e) {

        e.preventDefault();

        var Email = $("#emailsignin").val();
        var Password = $("#passwordsignin").val();
        var Checkbox = $("#remember").val();

        //pravimo data string
        var dataString = { 'mail': Email, 'pass': Password };//'Checkbox': Checkbox};

        if (Email == ''||Password == '')
        {
            alert("Please fill all fields!");
        }
        else {
            getAjaxResponse(dataString, function (data) {
                var xmlDoc = $.parseXML(data),
                    $xml = $(xmlDoc),
                    $title = $xml.find("string");
                alert($title.text());
                var sign = "No users were found with this email, please sign up!";
                if ($title.text() == sign)
                {
                    $("#signinbtn").css("display", "none");
                    $("#msgsignin").css("display", "inline");
                }
            });
        }

        return false;
    });

    function getAjaxResponse(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/returnWorkerFromEmail",
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

    $("#trans1").click(function (e) {

        e.preventDefault();

        $("#logbtn").removeClass("active");
        $("#regbtn").addClass("active");

        $("#tab1").removeClass("active");
        $("#tab2").addClass("active");
        
        $("#msgsignin").css("display", "none");
        $("#signinbtn").css("display", "inline");

    });

    //novi korisnici
    $("#signupbtn").click(function (e) {

        e.preventDefault();

        var Name = $("#firstname").val();
        var Surname = $("#lastname").val();
        var Email = $("#email").val();
        var Password = $("#password").val();
        var Reppass = $("#repeatedpwd").val();
        var Checkbox = "on";

        //pravimo data string
        var dataString = { 'mail': Email, 'pass': Password, 'name': Name, 'last': Surname,'check': Checkbox};

        if (Email == '' || Password == '' || Name == '' || Surname == '' || Reppass == '') {
            alert("Please fill all fields!");
        }
        else if (Password != Reppass)
        {
            alert("Password doesn't match, enter it again!");
        }
        else {
            getAjaxResponseSign(dataString, function (data) {
                var xmlDoc = $.parseXML(data),
                    $xml = $(xmlDoc),
                    $title = $xml.find("string");
                alert($title.text());
                var sign = "There already is a user with this email, please sign in!";
                if ($title.text() == sign) {
                    $("#signupbtn").css("display", "none");
                    $("#msgsignup").css("display", "inline");
                }
            });
        }

        return false;
    });

    function getAjaxResponseSign(sstring, fn) {

        $.ajax({
            url: "./MongoService.asmx/enterNewWorkerInDb",
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

    $("#trans2").click(function (e) {

        e.preventDefault();

        $("#regbtn").removeClass("active");
        $("#logbtn").addClass("active");

        $("#tab2").removeClass("active");
        $("#tab1").addClass("active");

    });
});