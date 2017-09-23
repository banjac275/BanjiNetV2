$(document).ready(function () {
    $("#signinbtn").click(function (e) {

        e.preventDefault();

        var email = $("#emailsignin").val();
        var password = $("#passwordsignin").val();
        var checkbox = $("#remember").val();

        //pravimo data string
        var datastring = { 'mail': email, 'pass': password };//'checkbox': checkbox};

        if (email === '' || password === '') {
            alert("please fill all fields!");
        }
        else {

            getAjaxResponse(datastring, function (data) {
                var xmldoc = $.parseXML(data),
                    $xml = $(xmldoc),
                    $title = $xml.find("string");
                console.log($title.text());

                var sign = "No users were found with this email, please sign up!";
                var badp = "Bad password, please try again!";
                if ($title.text() === sign) {
                    getAjaxResponseCompany(datastring, function (dataa) {
                        var xmldocs = $.parseXML(dataa),
                            $xmls = $(xmldocs),
                            $titles = $xmls.find("string");
                        console.log($titles.text());


                        if ($titles.text() === sign) {
                            alert(sign);
                            $("#signinbtn").css("display", "none");
                            $("#msgsignin").css("display", "inline");
                        }
                        else if ($titles.text() === badp) {
                            alert(badp);
                        }
                        else {
                            alert("Login Success!");
                            window.location.assign("./userprofile.aspx");
                        }
                    });
                    //alert(sign);
                }
                else if ($title.text() === badp) {
                    alert(badp);
                }
                else {
                    alert("Login Success!");
                    window.location.assign("./userprofile.aspx");
                }
            });
        }

        return false;
    });





    function getAjaxResponse(sstring, fn) {

        $.ajax({
            //url: "./MongoService.asmx/returnWorkerFromEmail",
            url: "./RaptorService.asmx/returnWorkerFromEmailR",
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

    function getAjaxResponseCompany(sstring, fn) {

        $.ajax({
            //url: "./MongoService.asmx/returnCompanyFromEmail",
            url: "./RaptorService.asmx/returnCompanyFromEmailR",
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
        var dataString = { 'mail': Email, 'pass': Password, 'name': Name, 'last': Surname, 'check': Checkbox };

        if (Email === '' || Password === '' || Name === '' || Surname === '' || Reppass === '') {
            alert("Please fill all fields!");
        }
        else if (Password !== Reppass) {
            alert("Password doesn't match, enter it again!");
        }
        else {
            getAjaxResponseSign(dataString, function (data) {
                var xmlDoc = $.parseXML(data),
                    $xml = $(xmlDoc),
                    $title = $xml.find("string");

                var sign = "There already is a user with this email, please sign in!";
                if ($title.text() === sign) {
                    $("#signupbtn").css("display", "none");
                    $("#msgsignup").css("display", "inline");
                }
                else {
                    alert("Sign Up Success!");
                    window.location.assign("./userprofile.aspx");
                }
            });
        }


        return false;
    });

    //nove kompanije
    $("#signupbtnc").click(function (e) {

        e.preventDefault();

        var Email = $("#email-company").val();
        var CompanyName = $("#companyname").val();
        var Owner = $("#ceo").val();
        var Type = $("#typec").val();
        var Location = $("#located").val();
        var Password = $("#passwordc").val();
        var Reppass = $("#repeatedpwdc").val();
        var Checkbox = "on";

        //pravimo data string
        var dataString = { 'company': CompanyName, 'owner': Owner, 'type': Type, 'location': Location, 'mail': Email, 'pass': Password, 'check': Checkbox };

        if (Email === '' || Password === '' || CompanyName === '' || Owner === '' || Reppass === '' || Type === '' || Location === '') {
            alert("Please fill all fields!");
        }
        else if (Password !== Reppass) {
            alert("Password doesn't match, enter it again!");
        }
        else {
            getAjaxResponseSignCompany(dataString, function (data) {
                var xmlDoc = $.parseXML(data),
                    $xml = $(xmlDoc),
                    $title = $xml.find("string");

                var fail = "User database entry failed!";
                var sign = "There already is a company with this email, please sign in!";
                if ($title.text() === sign) {
                    $("#signupbtnc").css("display", "none");
                    $("#msgsignupc").css("display", "inline");
                }
                else if ($title.text() === fail) {
                    alert(fail);
                }
                else {
                    alert("Sign Up Success!");
                    //console.log($title.text());
                    window.location.assign("./userprofile.aspx");
                }
            });
        }


        return false;
    });

    function getAjaxResponseSign(sstring, fn) {

        $.ajax({
            //url: "./MongoService.asmx/enterNewWorkerInDb",
            url: "./RaptorService.asmx/enterNewWorkerInRDb",
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

    function getAjaxResponseSignCompany(sstring, fn) {

        $.ajax({
            //url: "./MongoService.asmx/enterNewCompanyInDb",
            url: "./RaptorService.asmx/enterNewCompanyInRDb",
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

        $("#msgsignup").css("display", "none");
        $("#signupbtn").css("display", "inline");

    });

    $("#trans3").click(function (e) {

        e.preventDefault();

        $("#regbtn").removeClass("active");
        $("#logbtn").addClass("active");

        $("#tab2").removeClass("active");
        $("#tab1").addClass("active");

        $("#msgsignupc").css("display", "none");
        $("#signupbtnc").css("display", "inline");

    });
});