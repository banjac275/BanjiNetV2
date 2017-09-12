<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fellowworker.aspx.cs" Inherits="fellowworker" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
    <head runat="server">
        <title>BanjiNet - Fellow Worker Profile</title>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css"/>
        <link rel="stylesheet" href="~/Shared/user.css"/>
        <script type="text/javascript">
            $(document).ready(function () {
                var storage = JSON.parse(localStorage.getItem("workerView"));
                console.log(storage);
                var add = "<div>Name: " + storage.first + " " + storage.last + "</div><hr/>" +
                    "<div>Email: " + storage.mail + "</div><hr/>" +
                    "<div>Company: " + storage.company + "</div><hr/>";
                $("#personal").append(add);
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
                      <li><a href="#">Search</a></li>
                      <li><a href="workerslist.aspx">Workers</a></li>
                      <li><a href="#">Companies</a></li>
                      <li><a href="profileEditor.aspx">Edit Profile</a></li>
                      <li><a href="UserProfile.aspx">User Profile</a></li>
                      <li><a href="#">Log Out</a></li>
                    </ul>
                  </div>
                  <!--/.nav-collapse -->
                    </div>
                <!--/.container-fluid -->
              </nav>
            </div> 

            <div class="container">
                <div class="row">
                    <div class="col-4 panel panel-info">
                       <div class="panel-title">User Info: </div><hr />
                       <div class="panel-body" id="personal" runat="server">

                       </div>
                    </div>
                    <div class="col-8" id="firm" runat="server">

                    </div>                    
                </div>
            </div>
        
           

        <!-- /top nav -->
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <script src="bootstrap/js/bootstrap.min.js"></script>
    </body>
</html>
