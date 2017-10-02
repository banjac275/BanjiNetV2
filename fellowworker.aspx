<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fellowworker.aspx.cs" Inherits="fellowworker" %>

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
                var storage = JSON.parse(localStorage.getItem("workerViewR"));
                console.log(storage);

                var add = "<div>Name: " + storage.first + " " + storage.last + "</div><hr/>" +
                    "<div>Email: " + storage.mail + "</div><hr/>" +
                    "<div>Company: " + storage.company + "</div>";
                $("#personal").append(add);

                var name = { name: storage.company };
                getAjaxResponse(name, function (data) {
                    var xmldoc = $.parseXML(data),
                        $xml = $(xmldoc),
                        $title = $xml.find("string");
                    var parsed = JSON.parse($title.text());
                    console.log(parsed[0]);
                    
                    var addc = "<div>Company name: " + parsed[0].CompanyName + "</div><hr/>" +
                        "<div>Email: " + parsed[0].Email + "</div><hr/>" +
                        "<div>Type: " + parsed[0].Type + "</div><hr/>" +
                        "<div>Location: " + parsed[0].Location + "</div><hr/>" +
                        "<div>Owner: " + parsed[0].Owner + "</div>";
                    $("#firm").append(addc);

                    localStorage.setItem("workerView", "");


                    var sign = "no users were found with this name!";
                    if ($title.text() == sign) {
                        alert("No companies with that name are found!");
                    }
                    
                });

                function getAjaxResponse(sstring, fn) {

                    $.ajax({
                        //url: "./MongoService.asmx/retCompanyFromName",
                        url: "./RaptorService.asmx/returnCompanyFromNameR",
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
            </div>
        
           

        <!-- /top nav -->
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
    </body>
</html>
