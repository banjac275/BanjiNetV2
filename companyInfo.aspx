<%@ Page Language="C#" AutoEventWireup="true" CodeFile="companyInfo.aspx.cs" Inherits="companyInfo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
    <head runat="server">
        <title>BanjiNet - Company Info</title>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="~/Shared/user.css"/>
        <script type="text/javascript">
            $(document).ready(function () {
                var storage = localStorage.getItem("companyViewR");
                var received = [];
                var urlW = "./RavenService.asmx/retWorkerFromIdR";
                var urlC = "./RavenService.asmx/retCompanyFromIdR";

                var tempp = storage.replace(/"/g, "");
                console.log(storage);
                var idC = { id: tempp };
                getAjaxResponse(urlC, idC, function (data) {
                    var xmldoc = $.parseXML(data),
                        $xml = $(xmldoc),
                        $title = $xml.find("string");
                    var parsed = JSON.parse($title.text());
                    console.log(parsed);

                    var add = "<div>Company Name: " + parsed.CompanyName + "</div><hr/>" +
                        "<div>Email: " + parsed.Email + "</div><hr/>" +
                        "<div>Owner: " + parsed.Owner + "</div><hr/>" +
                        "<div>Type: " + parsed.Type + "</div><hr/>" +
                        "<div>Location: " + parsed.Location + "</div>";
                    $("#personal").append(add);
                
                    if (parsed.Employees !== null) {
                        var j = 0;
                        for (var i = 0; i < parsed.Employees.length; i++) {
                            var temp = parsed.Employees[i].replace(/"/g, "");
                            console.log(temp);
                            var id = { id: temp };
                            getAjaxResponse(urlW, id, function (dataa) {
                                var xmldocs = $.parseXML(dataa),
                                    $xmls = $(xmldocs),
                                    $titles = $xmls.find("string");
                                var parsedd = JSON.parse($titles.text());
                                console.log(parsedd);
                                received.push(parsedd);
                                j = j + 1;

                                var table = '<tr><th scope= "row">' + j + '</th>'
                                    + '<td>' + parsedd.FirstName + '</td>'
                                    + '<td>' + parsedd.LastName + '</td>'
                                    + '<td>' + parsedd.Email + '</td>'
                                    + '<td>' + parsedd.CompanyName + '</td>'
                                    + '<td><button type="button" class="view btn btn-default" id="addd' + j + '">View</button></td></tr>';
                                $("#listing").append(table);


                                var sign = "no users were found with this name!";
                                if ($title.text() == sign) {
                                    alert("No companies with that name are found!");
                                }

                            });    
                            

                        }
                    }
                });

                $('#listing').on('click', '.view', function () {
                    for (var i = 0; i < received.length; i++) {
                        if (received[i].Email === $(this).closest("tr")[0].children[3].innerHTML) {
                            console.log($(this).closest("tr")[0].children[3].innerHTML);
                            localStorage.setItem("workerViewR", received[i].Id);
                        }
                    }
                    window.location.assign("./fellowworker.aspx");

                });

                function getAjaxResponse(urll, sstring, fn) {

                    $.ajax({
                        //url: "./MongoService.asmx/retWorkerFromId",
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
                       <div class="panel-title"> Company Info: </div><hr />
                       <div class="panel-body" id="personal" runat="server">

                       </div>
                    </div>
                    <div class="col-lg-8 panel panel-info">
                        <div class="panel-title"> Company Workers: </div><hr />
                        <div class="panel-body" id="firm" runat="server">
                            <div>
                                <table id="list" class="table table-hover">
                                  <thead class="thead-inverse">
                                    <tr>
                                      <th>#</th>
                                      <th>First Name</th>
                                      <th>Last Name</th>
                                      <th>Email</th>
                                      <th>Company</th>
                                      <th>Profile</th>
                                    </tr>
                                  </thead>
                                  <tbody id="listing">
                                        
                                  </tbody>
                                </table>
                           </div>

                       </div>
                    </div>                    
                </div>
            </div>
        
           

        <!-- /top nav -->
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
    </body>
</html>
