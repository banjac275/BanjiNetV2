﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
    <head runat="server">
        <title>BanjiNet - Home</title>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="~/Shared/user.css"/>
        <link rel="stylesheet" href="~/Shared/home.css"/>
        <script type="text/javascript" src="Scripts/homes.js"></script>
    </head>
    <body>
        <!-- top nav -->
           <div class="container">
              <nav class="navbar navbar-default nav-tabs">
                <div class="container-fluid">
                  <div id="navbar1" class="navbar-header">
                    <ul class="nav navbar-nav">
                      <li class="navbar-brand navbar-left"><a href="#">BanjiNet</a></li>
                      <li class="active"><a href="home.aspx">Home</a></li>
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
        <!-- /top nav -->
        <div class="row">
            <div class="col-lg-3"></div>
               <div class="col-lg-6" id="contents">
               
               </div> 
            <div class="col-lg-3"></div>
        </div>
        
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
    </body>
</html>
