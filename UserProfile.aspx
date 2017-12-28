<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
    <head runat="server">
        <title>BanjiNet - User Profile</title>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="~/Shared/user.css"/>
        <script type="text/javascript">
            $(document).ready(function () {

                console.log(localStorage.getItem("job"));
                console.log(localStorage.getItem("firm")); 
                console.log(localStorage.getItem("friends")); 
                console.log(localStorage.getItem("jobR"));
                console.log(localStorage.getItem("firmR")); 
                console.log(localStorage.getItem("friendsR")); 
                var friendss = [];
                var moment = JSON.parse(localStorage.getItem("userTemp"));
                var personal = document.getElementById("personal");
                var basee = localStorage.getItem("dbres");

                if (personal !== null) {
                    if (basee === "mongo") {
                        if (localStorage.getItem("job") !== null) {
                            document.getElementById("write1").innerHTML = "User Info:";
                            document.getElementById("write2").innerHTML = "Company Info:";
                            $("#opt").css("display", "block");
                            document.getElementById("write3").innerHTML = "Former Employment:";
                            document.getElementById("write4").innerHTML = "Friends:";
                            var res = localStorage.getItem("job");
                            localStorage.removeItem("job");
                            var recFriend = localStorage.getItem("friends");
                            localStorage.removeItem("friends");
                            var name = { name: res };
                            var urlFC = "./MongoService.asmx/retCompanyFromName";
                            var urlFF = "./MongoService.asmx/retWorkerFromId";

                            console.log(name);

                            getAjaxResponse(urlFC, name, function (data) {
                                var xmldoc = $.parseXML(data),
                                    $xml = $(xmldoc),
                                    $title = $xml.find("string");
                                var parsed = JSON.parse($title.text());
                                console.log(parsed);

                                var addc = "<div>Company name: " + parsed.CompanyName + "</div><hr/>" +
                                    "<div>Email: " + parsed.Email + "</div><hr/>" +
                                    "<div>Type: " + parsed.Type + "</div><hr/>" +
                                    "<div>Location: " + parsed.Location + "</div><hr/>" +
                                    "<div>Owner: " + parsed.Owner + "</div>";
                                $("#firm").append(addc);

                                console.log("obavest");
                                var sign = "no users were found with this name!";
                                if ($title.text() == sign) {
                                    alert("No companies with that name are found!");
                                }

                            });

                            if (recFriend != null) {

                                console.log("prolaz");

                                var addf = "<div><table id='listt' class='table table-hover'><thead class='thead-inverse'>" +
                                    "<tr><th>#</th><th>First Name</th><th>Last Name</th><th>Email</th><th>Company</th>" +
                                    "<th>Profile</th><th>Unfriend</th></tr></thead><tbody id='list'></tbody></table></div>";
                                $("#friend").append(addf);

                                if (moment.PreviousEmployment !== null) {
                                    var prev = "<div><table class='table table-hover'><thead class='thead-inverse'>" +
                                        "<tr><th>#</th><th>Company:</th><th>From:</th><th>To:</th>" +
                                        "</tr></thead><tbody id='previous'></tbody></table></div>";
                                    $("#former").append(prev);

                                    var j = 0;
                                    for (var i = 0; i < moment.PreviousEmployment.length; i++) {
                                        j++;

                                        var inser = '<tr><th scope= "row">' + j + '</th>'
                                            + '<td>' + moment.PreviousEmployment[i].FirmName + '</td>'
                                            + '<td>' + moment.PreviousEmployment[i].StartTime + '</td>'
                                            + '<td>' + moment.PreviousEmployment[i].EndTime + '</td></tr>';
                                        $("#previous").append(inser);
                                    }
                                }

                                if (recFriend.length !== null) {

                                    var j = 0;
                                    var proc = JSON.parse(recFriend);
                                    console.log(proc);
                                    for (var i = 0; i < proc.length; i++) {
                                        var id = { id: proc[i].toString() };
                                        getAjaxResponse(urlFF, id, function (data) {
                                            var xmldoc = $.parseXML(data),
                                                $xml = $(xmldoc),
                                                $title = $xml.find("string");
                                            var parsedd = JSON.parse($title.text());
                                            console.log(parsedd);
                                            j = j + 1;
                                            friendss.push(parsedd);

                                            var table = '<tr><th scope= "row">' + j + '</th>'
                                                + '<td>' + parsedd.FirstName + '</td>'
                                                + '<td>' + parsedd.LastName + '</td>'
                                                + '<td>' + parsedd.Email + '</td>'
                                                + '<td>' + parsedd.CompanyName + '</td>'
                                                + '<td><button type="button" class="view btn btn-default" id="addd' + j + '">View</button></td>'
                                                + '<td><button type="button" class="remov btn btn-default" id="dell' + j + '">Remove</button></td></tr>';
                                            $("#list").append(table);


                                            var sign = "no users were found with this name!";
                                            if ($title.text() === sign) {
                                                alert("No users with that name are found!");
                                            }

                                        });
                                    }

                                }
                            }

                        }

                        if (localStorage.getItem("firm") !== null) {
                            document.getElementById("write1").innerHTML = "Company Info:";
                            document.getElementById("write2").innerHTML = "Company Workers:";
                            $("#opt").css("display", "none");
                            var res = localStorage.getItem("firm");
                            localStorage.removeItem("firm");
                            if (res === 1)
                                res = null;
                            var url = "./MongoService.asmx/retWorkerFromId";

                            var addc = "<div><table id='list' class='table table-hover'><thead class='thead-inverse'>" +
                                "<tr><th>#</th><th>First Name</th><th>Last Name</th><th>Email</th><th>Company</th>" +
                                "<th>Profile</th></tr></thead><tbody id='listing'></tbody></table></div>";
                            $("#firm").append(addc);

                            console.log(res);

                            if (res.length !== null) {

                                var j = 0;
                                var proc = JSON.parse(res);
                                console.log(proc);
                                for (var i = 0; i < proc.length; i++) {
                                    var id = { id: proc[i].toString() };
                                    getAjaxResponse(url, id, function (data) {
                                        var xmldoc = $.parseXML(data),
                                            $xml = $(xmldoc),
                                            $title = $xml.find("string");
                                        var parsedd = JSON.parse($title.text());
                                        console.log(parsedd);
                                        j = j + 1;

                                        var table = '<tr><th scope= "row">' + j + '</th>'
                                            + '<td>' + parsedd.FirstName + '</td>'
                                            + '<td>' + parsedd.LastName + '</td>'
                                            + '<td>' + parsedd.Email + '</td>'
                                            + '<td>' + parsedd.CompanyName + '</td></tr>';
                                        $("#listing").append(table);


                                        var sign = "no users were found with this name!";
                                        if ($title.text() === sign) {
                                            alert("No companies with that name are found!");
                                        }

                                    });
                                }

                            }

                        }
                    }

                    if (basee === "raven") {
                        if (localStorage.getItem("jobR") !== null) {
                            document.getElementById("write1").innerHTML = "User Info:";
                            document.getElementById("write2").innerHTML = "Company Info:";
                            $("#opt").css("display", "block");
                            document.getElementById("write3").innerHTML = "Former Employment:";
                            document.getElementById("write4").innerHTML = "Friends:";
                            var res = localStorage.getItem("jobR");
                            localStorage.removeItem("jobR");
                            var recFriend = localStorage.getItem("friendsR");
                            localStorage.removeItem("friendsR");
                            var name = { name: res };
                            var urlFC = "./RavenService.asmx/retCompanyFromNameR";
                            var urlFF = "./RavenService.asmx/retWorkerFromIdR";

                            console.log(name);

                            getAjaxResponse(urlFC, name, function (data) {
                                var xmldoc = $.parseXML(data),
                                    $xml = $(xmldoc),
                                    $title = $xml.find("string");
                                var parsed = JSON.parse($title.text());
                                console.log(parsed);

                                var addc = "<div>Company name: " + parsed.CompanyName + "</div><hr/>" +
                                    "<div>Email: " + parsed.Email + "</div><hr/>" +
                                    "<div>Type: " + parsed.Type + "</div><hr/>" +
                                    "<div>Location: " + parsed.Location + "</div><hr/>" +
                                    "<div>Owner: " + parsed.Owner + "</div>";
                                $("#firm").append(addc);

                                console.log("obavest");
                                var sign = "no users were found with this name!";
                                if ($title.text() == sign) {
                                    alert("No companies with that name are found!");
                                }

                            });

                            if (recFriend != null) {

                                console.log("prolaz");

                                var addf = "<div><table id='listt' class='table table-hover'><thead class='thead-inverse'>" +
                                    "<tr><th>#</th><th>First Name</th><th>Last Name</th><th>Email</th><th>Company</th>" +
                                    "<th>Profile</th><th>Unfriend</th></tr></thead><tbody id='list'></tbody></table></div>";
                                $("#friend").append(addf);

                                if (moment.PreviousEmployment !== null) {
                                    var prev = "<div><table class='table table-hover'><thead class='thead-inverse'>" +
                                        "<tr><th>#</th><th>Company:</th><th>From:</th><th>To:</th>" +
                                        "</tr></thead><tbody id='previous'></tbody></table></div>";
                                    $("#former").append(prev);

                                    var j = 0;
                                    for (var i = 0; i < moment.PreviousEmployment.length; i++) {
                                        j++;

                                        var inser = '<tr><th scope= "row">' + j + '</th>'
                                            + '<td>' + moment.PreviousEmployment[i].FirmName + '</td>'
                                            + '<td>' + moment.PreviousEmployment[i].StartTime + '</td>'
                                            + '<td>' + moment.PreviousEmployment[i].EndTime + '</td></tr>';
                                        $("#previous").append(inser);
                                    }
                                }

                                if (recFriend.length !== null) {

                                    var j = 0;
                                    var proc = JSON.parse(recFriend);
                                    console.log(proc);
                                    for (var i = 0; i < proc.length; i++) {
                                        var id = { id: proc[i].toString() };
                                        getAjaxResponse(urlFF, id, function (data) {
                                            var xmldoc = $.parseXML(data),
                                                $xml = $(xmldoc),
                                                $title = $xml.find("string");
                                            var parsedd = JSON.parse($title.text());
                                            console.log(parsedd);
                                            j = j + 1;
                                            friendss.push(parsedd);

                                            var table = '<tr><th scope= "row">' + j + '</th>'
                                                + '<td>' + parsedd.FirstName + '</td>'
                                                + '<td>' + parsedd.LastName + '</td>'
                                                + '<td>' + parsedd.Email + '</td>'
                                                + '<td>' + parsedd.CompanyName + '</td>'
                                                + '<td><button type="button" class="view btn btn-default" id="addd' + j + '">View</button></td>'
                                                + '<td><button type="button" class="remov btn btn-default" id="dell' + j + '">Remove</button></td></tr>';
                                            $("#list").append(table);


                                            var sign = "no users were found with this name!";
                                            if ($title.text() === sign) {
                                                alert("No users with that name are found!");
                                            }

                                        });
                                    }

                                }
                            }

                        }

                        if (localStorage.getItem("firmR") !== null) {

                            document.getElementById("write1").innerHTML = "Company Info:";
                            document.getElementById("write2").innerHTML = "Company Workers:";
                            $("#opt").css("display", "none");
                            var res = localStorage.getItem("firmR");
                            localStorage.removeItem("firmR");
                            if (res === 1)
                                res = null;
                            var url = "./RavenService.asmx/retWorkerFromIdR";

                            var addc = "<div><table id='list' class='table table-hover'><thead class='thead-inverse'>" +
                                "<tr><th>#</th><th>First Name</th><th>Last Name</th><th>Email</th><th>Company</th>" +
                                "<th>Profile</th></tr></thead><tbody id='listing'></tbody></table></div>";
                            $("#firm").append(addc);

                            console.log(res);

                            if (res.length !== null) {

                                var j = 0;
                                var proc = JSON.parse(res);
                                console.log(proc);
                                for (var i = 0; i < proc.length; i++) {
                                    var id = { id: proc[i].toString() };
                                    getAjaxResponse(url, id, function (data) {
                                        var xmldoc = $.parseXML(data),
                                            $xml = $(xmldoc),
                                            $title = $xml.find("string");
                                        var parsedd = JSON.parse($title.text());
                                        console.log(parsedd);
                                        j = j + 1;

                                        var table = '<tr><th scope= "row">' + j + '</th>'
                                            + '<td>' + parsedd.FirstName + '</td>'
                                            + '<td>' + parsedd.LastName + '</td>'
                                            + '<td>' + parsedd.Email + '</td>'
                                            + '<td>' + parsedd.CompanyName + '</td></tr>';
                                        $("#listing").append(table);


                                        var sign = "no users were found with this name!";
                                        if ($title.text() === sign) {
                                            alert("No companies with that name are found!");
                                        }

                                    });
                                }

                            }

                        }

                    }

                    $('#list').on('click', '.view', function () {
                        //alert('triggered');
                        console.log($(this).closest("tr")[0].children[3].innerHTML);
                        console.log(friendss);
                        for (var i = 0; i < friendss.length; i++) {
                            if (friendss[i].Email === $(this).closest("tr")[0].children[3].innerHTML)
                            {
                                if (basee === "raven")
                                    localStorage.setItem("workerViewR", friendss[i].Id);
                                else
                                    localStorage.setItem("workerView", friendss[i].Id);
                            }
                                
                        }
                        window.location.assign("./fellowworker.aspx");
                    });

                    $('#list').on('click', '.remov', function () {
                        //alert('triggered');
                        console.log($(this).closest("tr")[0].children[3].innerHTML);
                        console.log(friendss);

                        var urlf = "./RavenService.asmx/removeFriendR";
                        var urlfm = "./MongoService.asmx/removeFriend";
                        var urlt = null;

                        if (basee === "raven")
                            urlt = urlf;
                        else
                            urlt = urlfm;

                        for (var i = 0; i < friendss.length; i++) {
                            if (friendss[i].Email === $(this).closest("tr")[0].children[3].innerHTML) {
                                var idd = { id1: friendss[i].Id.toString(), id2: moment.Id };
                                console.log(friendss[i].Id.toString());
                                if (urlt !== null) {
                                    getAjaxResponse(urlt, idd, function (data) {
                                        var xmldoc = $.parseXML(data),
                                            $xml = $(xmldoc),
                                            $titlse = $xml.find("string");
                                        var parsed = $titlse.text();
                                        console.log(parsed);

                                        var sign = "no users were found with this name!";
                                        if ($titlse.text() == sign) {
                                            alert("No companies with that name are found!");
                                        }
                                        else {
                                            alert("Friend Removed!");
                                            window.location.assign("./UserProfile.aspx");
                                        }

                                    });
                                }
                            }
                        }

                    });

                }

                function getAjaxResponse(urll, sstring, fn) {

                    $.ajax({
                        url: urll,
                        dataType: "text",
                        type: "POST",
                        data: sstring,
                        error: function (err) {
                            alert("Error", err);
                            //console.log("obavest");
                        },
                        success: function (data) {
                            fn(data);
                        }
                    });
                }  
                
            });
        </script>
    </head>
    <body>
        <!-- top nav -->
           <div class="container">
              <nav class="navbar navbar-default nav-tabs">
                <div class="container-fluid">
                  <div id="navbar1" class="navbar-header">
                    <ul class="nav navbar-nav">
                      <li class="navbar-brand navbar-left"><a href="#">BanjiNet</a></li>
                      <li><a href="home.aspx">Home</a></li>
                      <li><a href="Search.aspx">Search</a></li>
                      <li><a href="workerslist.aspx">Workers</a></li>
                      <li><a href="companylist.aspx">Companies</a></li>
                      <li><a href="profileEditor.aspx">Edit Profile</a></li>
                      <li class="active"><a href="UserProfile.aspx">User Profile</a></li>
                      <li><a href="logout.aspx">Log Out</a></li>
                    </ul>
                  </div>
                  <!--/.nav-collapse -->
                    </div>
                <!--/.container-fluid -->
              </nav>
            </div> 

            <div class="container">
                <div class="row">
                    <div class="col-lg-4 panel panel-info">
                       <div id="write1" class="panel-title"> </div><hr />
                       <div class="panel-body" id="personal" runat="server">

                       </div>
                    </div>
                    <div class="col-lg-8 panel panel-info">
                        <div id="write2" class="panel-title"> </div><hr />
                        <div class="panel-body" id="firm" runat="server">

                       </div>
                    </div>                    
                </div>
                <div class="row" id="opt">
                    <div class="col-lg-4 panel panel-info">
                       <div id="write3" class="panel-title"> </div><hr />
                       <div class="panel-body" id="former" runat="server">

                       </div>
                    </div>
                    <div class="col-lg-8 panel panel-info">
                        <div id="write4" class="panel-title"> </div><hr />
                        <div class="panel-body" id="friend" runat="server">

                       </div>
                    </div>  
                </div>
            </div>
        
           

        <!-- /top nav -->
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
    </body>
</html>
