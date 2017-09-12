<%@ Page Language="C#" AutoEventWireup="true" CodeFile="workerslist.aspx.cs" Inherits="workerslist" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
    <head runat="server">
        <title>BanjiNet - Workers</title>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css"/>
        <link rel="stylesheet" href="~/Shared/user.css"/>
        <script type="text/javascript" src="Scripts/workers.js"></script>
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
                      <li class="active"><a href="#">Workers</a></li>
                      <li><a href="#">Companies</a></li>
                      <li><a href="#">Edit Profile</a></li>
                      <li><a href="UserProfile.aspx">User Profile</a></li>
                      <li><a href="#">Log Out</a></li>
                    </ul>
                  </div>
                  <!--/.nav-collapse -->
                    </div>
                <!--/.container-fluid -->
              </nav>
            </div> 

           <div>
               <h3 class="text-center">Workers: </h3>
               <table id="list" class="table table-hover">
                  <thead class="thead-inverse">
                    <tr>
                      <th>#</th>
                      <th>First Name</th>
                      <th>Last Name</th>
                      <th>Email</th>
                      <th>Company</th>
                    </tr>
                  </thead>
                  <tbody id="listing">
                                        
                  </tbody>
                </table>
           </div>           

        <!-- /top nav -->
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <script src="bootstrap/js/bootstrap.min.js"></script>
    </body>
</html>