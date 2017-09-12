<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
    <head runat="server">
        <title>BanjiNet - User Profile</title>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css"/>
        <link rel="stylesheet" href="~/Shared/user.css"/>
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
                      <li><a href="#">Edit Profile</a></li>
                      <li class="active"><a href="UserProfile.aspx">User Profile</a></li>
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
