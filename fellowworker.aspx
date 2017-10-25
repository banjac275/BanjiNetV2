﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fellowworker.aspx.cs" Inherits="fellowworker" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
    <head runat="server">
        <title>BanjiNet - Fellow Worker Profile</title>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="~/Shared/user.css"/>
        <script type="text/javascript">
            $(document).ready(function () {
                var storage = localStorage.getItem("workerViewR");
                console.log(storage);
                var url = "./RavenService.asmx/retCompanyFromNameR";
                var urll = "./RavenService.asmx/retWorkerFromIdR";
                var user = JSON.parse(localStorage.getItem("userTemp"));

                var addf = "<div><table id='listt' class='table table-hover'><thead class='thead-inverse'>" +
                    "<tr><th>#</th><th>First Name</th><th>Last Name</th><th>Email</th><th>Company</th>" +
                    "<th>Profile</th></tr></thead><tbody id='list'></tbody></table></div>";
                $("#friend").append(addf);

                var ajdi = { id: storage };
                getAjaxResponse(urll, ajdi, function (dataa) {
                    var xmldocs = $.parseXML(dataa),
                        $xmls = $(xmldocs),
                        $titles = $xmls.find("string");
                    var parse = JSON.parse($titles.text());

                    var add = "<div>Name: " + parse.FirstName + " " + parse.LastName + "</div><hr/>" +
                        "<div>Email: " + parse.Email + "</div><hr/>" +
                        "<div>Company: " + parse.CompanyName + "</div><hr/>" +
                        "<div>Skills: " + parse.Skills + "</div>";
                    $("#personal").append(add);

                    if (user.Friends !== null)
                    {
                        for (var i = 0; i < user.Friends.length; i++)
                        {
                            if (parse.Id === user.Friends[i])
                            {
                                //$("#add").css("display", "none");
                                //$("#rem").css("display", "block");
                                $("#add").addClass("disabled");
                                $("#rem").removeClass("disabled");
                            }
                        }
                    }
                    else if (parse.Id === user.Id)
                    {
                        //$("#add").css("display", "block");
                        //$("#rem").css("display", "none");
                        $("#add").addClass("disabled");
                        $("#rem").addClass("disabled");
                    }
                    else
                    {
                        $("#rem").addClass("disabled");
                        $("#add").removeClass("disabled");
                    }

                    var name = { name: parse.CompanyName };
                    getAjaxResponse(url, name, function (data) {
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

                        localStorage.setItem("workerViewR", "");


                        var sign = "no users were found with this name!";
                        if ($title.text() == sign) {
                            alert("No companies with that name are found!");
                        }

                        var k = 0;
                        if (parse.Friends !== null) {
                            for (var j = 0; j < parse.Friends.length; j++) {
                                var idi = { id: parse.Friends[j] }
                                console.log(parse.Friends);
                                getAjaxResponse(urll, idi, function (datas) {
                                    var xmldocss = $.parseXML(datas),
                                        $xmlss = $(xmldocss),
                                        $titless = $xmls.find("string");
                                    var parsee = JSON.parse($titless.text());

                                    k = k + 1;

                                    var table = '<tr><th scope= "row">' + k + '</th>'
                                        + '<td>' + parsee.FirstName + '</td>'
                                        + '<td>' + parsee.LastName + '</td>'
                                        + '<td>' + parsee.Email + '</td>'
                                        + '<td>' + parsee.CompanyName + '</td>'
                                        + '<td><button type="button" class="btn btn-default" id="prof' + j + '">View</button></td></tr>';
                                    $("#list").append(table);


                                    var sign = "no users were found with this name!";
                                    if ($titless.text() === sign) {
                                        alert("No users with that name are found!");
                                    }


                                });
                            }
                        }
                    
                    });
                });

                $("#add").click(function (e) {

                    e.preventDefault();

                    //$("#add").css("display", "none");
                    //$("#rem").css("display", "block");
                    $("#add").addClass("disabled");
                    $("#rem").removeClass("disabled");

                    var ids = { id1: storage, id2: user.Id };
                    var urlf = "./RavenService.asmx/addFriendR";

                    getAjaxResponse(urlf, ids, function (datas) {
                        var xmldoc = $.parseXML(datas),
                            $xml = $(xmldoc),
                            $title = $xml.find("string");
                        var parsed = $title.text();
                        console.log(parsed);


                        var sign = "no users were found with this name!";
                        if ($title.text() == sign) {
                            alert("Friends not added!");
                        }
                        else
                        {
                            alert("Friend Added!");
                            window.location.assign("./fellowworker.aspx");
                        }

                    });

                });

                $("#rem").click(function (e) {

                    e.preventDefault();

                    //$("#add").css("display", "block");
                    //$("#rem").css("display", "none");
                    $("#rem").addClass("disabled");
                    $("#add").removeClass("disabled");

                });

                function getAjaxResponse(urlll, sstring, fn) {

                    $.ajax({
                        //url: "./MongoService.asmx/retCompanyFromName",
                        url: urlll,
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
                      <li><a href="#">Home</a></li>
                      <li><a href="Search.aspx">Search</a></li>
                      <li><a href="workerslist.aspx">Workers</a></li>
                      <li><a href="companylist.aspx">Companies</a></li>
                      <li><a href="profileEditor.aspx">Edit Profile</a></li>
                      <li><a href="UserProfile.aspx">User Profile</a></li>
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
                       <div class="panel-title"> User Info: </div><hr />
                       <div class="panel-body" id="personal" runat="server">

                       </div>
                    </div>
                    <div class="col-lg-8 panel panel-info">
                        <div class="panel-title"> Company Info: </div><hr />
                        <div class="panel-body" id="firm" runat="server">

                       </div>
                    </div>                    
                </div>
                <div class="row">
                    <div class="col-lg-4 panel panel-info">
                        <div class="text-center col-lg-6">Resolve Friendship</div>
                        <button type="button" class="btn btn-default col-lg-4 text-right btn-block" id="add">Add Friend</button>
                        <button type="button" class="btn btn-default col-lg-4 text-right btn-block" id="rem">Unfriend</button>
                    </div>
                </div>
                <div class="row" id="opt">
                    <div class="col-lg-4 panel panel-info">
                       <div id="write3" class="panel-title">Previous Employment</div><hr />
                       <div class="panel-body" id="former" runat="server">

                       </div>
                    </div>
                    <div class="col-lg-8 panel panel-info">
                        <div id="write4" class="panel-title">Friends</div><hr />
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
